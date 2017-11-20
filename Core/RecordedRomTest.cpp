#include "stdafx.h"

#include "RecordedRomTest.h"
#include "Console.h"
#include "EmulationSettings.h"
#include "MessageManager.h"
#include "Debugger.h"
#include "MovieManager.h"
#include "PPU.h"
#include "VirtualFile.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/md5.h"
#include "../Utilities/ZipWriter.h"
#include "../Utilities/ZipReader.h"
#include "../Utilities/ArchiveReader.h"

RecordedRomTest::RecordedRomTest()
{
	Reset();

	MessageManager::RegisterNotificationListener(this);
}

RecordedRomTest::~RecordedRomTest()
{
	Reset();

	MessageManager::UnregisterNotificationListener(this);
}

void RecordedRomTest::SaveFrame(uint16_t* ppuFrameBuffer)
{
	uint8_t md5Hash[16];
	GetMd5Sum(md5Hash, ppuFrameBuffer, PPU::PixelCount * sizeof(uint16_t));

	if(memcmp(_previousHash, md5Hash, 16) == 0 && _currentCount < 255) {
		_currentCount++;
	} else {
		uint8_t* hash = new uint8_t[16];
		memcpy(hash, md5Hash, 16);
		_screenshotHashes.push_back(hash);
		if(_currentCount > 0) {
			_repetitionCount.push_back(_currentCount);
		}
		_currentCount = 1;

		memcpy(_previousHash, md5Hash, 16);

		_signal.Signal();
	}
}

void RecordedRomTest::ValidateFrame(uint16_t* ppuFrameBuffer)
{
	uint8_t md5Hash[16];
	GetMd5Sum(md5Hash, ppuFrameBuffer, PPU::PixelCount * sizeof(uint16_t));

	if(_currentCount == 0) {
		_currentCount = _repetitionCount.front();
		_repetitionCount.pop_front();
		_screenshotHashes.pop_front();
	}
	_currentCount--;

	if(memcmp(_screenshotHashes.front(), md5Hash, 16) != 0) {
		_badFrameCount++;
		Debugger::BreakIfDebugging();
	} 
	
	if (_currentCount == 0 && _repetitionCount.empty()) {
		//End of test
		_runningTest = false;
		_signal.Signal();
	}
}

void RecordedRomTest::ProcessNotification(ConsoleNotificationType type, void* parameter)
{
	switch(type) {
		case ConsoleNotificationType::PpuFrameDone:
			if(_recording) {
				SaveFrame((uint16_t*)parameter);
			} else if(_runningTest) {
				ValidateFrame((uint16_t*)parameter);
			}
			break;
		case ConsoleNotificationType::MovieEnded:
			if(_recordingFromMovie) {
				Save();
			}
			break;
	}
}

void RecordedRomTest::Reset()
{
	memset(_previousHash, 0xFF, 16);
	
	_currentCount = 0;
	_repetitionCount.clear();

	for(uint8_t* hash : _screenshotHashes) {
		delete[] hash;
	}
	_screenshotHashes.clear();

	_runningTest = false;
	_recording = false;
	_badFrameCount = 0;
	_recordingFromMovie = false;
}

void RecordedRomTest::Record(string filename, bool reset)
{
	_filename = filename;

	string mrtFilename = FolderUtilities::CombinePath(FolderUtilities::GetFolderName(filename), FolderUtilities::GetFilename(filename, false) + ".mrt");
	_file.open(mrtFilename, ios::out | ios::binary);

	if(_file) {
		Console::Pause();
		Reset();

		_recording = true;

		//Start recording movie alongside with screenshots
		MovieManager::Record(FolderUtilities::CombinePath(FolderUtilities::GetFolderName(filename), FolderUtilities::GetFilename(filename, false) + ".mmo"), reset);

		Console::Resume();
	}
}

void RecordedRomTest::RecordFromMovie(string testFilename, VirtualFile movieFile)
{
	if(!movieFile.IsValid()) {
		return;
	}

	_filename = testFilename;

	string mrtFilename = FolderUtilities::CombinePath(FolderUtilities::GetFolderName(testFilename), FolderUtilities::GetFilename(testFilename, false) + ".mrt");
	_file.open(mrtFilename, ios::out | ios::binary);

	if(_file) {
		Console::Pause();
		Reset();

		_recording = true;

		//Start playing movie
		MovieManager::Play(movieFile);
		movieFile.ReadFile(_movieData);
		_recordingFromMovie = true;

		Console::Resume();
	}
}

void RecordedRomTest::RecordFromTest(string newTestFilename, string existingTestFilename)
{
	ZipReader zipReader;
	zipReader.LoadArchive(existingTestFilename);
	stringstream testMovie, testRom;
	zipReader.GetStream("TestMovie.mmo", testMovie);
	zipReader.GetStream("TestRom.nes", testRom);
	VirtualFile romFile(testRom, newTestFilename);

	if(testMovie && testRom) {
		Console::Pause();
		Console::LoadROM(romFile);
		testRom.seekg(0, ios::beg);
		_romStream << testRom.rdbuf();

		RecordFromMovie(newTestFilename, VirtualFile(existingTestFilename, "TestMovie.mmo"));
		Console::Resume();
	}
}

int32_t RecordedRomTest::Run(string filename)
{
	string testName = FolderUtilities::GetFilename(filename, false);
	if(testName.compare("5.MMC3_rev_A") == 0 || testName.compare("6-MMC6") == 0 || testName.compare("6-MMC3_alt") == 0) {
		EmulationSettings::SetFlags(EmulationFlags::Mmc3IrqAltBehavior);
	} else {
		EmulationSettings::ClearFlags(EmulationFlags::Mmc3IrqAltBehavior);
	}

	if(testName.compare("demo_pal") == 0 || testName.substr(0, 4).compare("pal_") == 0) {
		EmulationSettings::SetNesModel(NesModel::PAL);
	} else {
		EmulationSettings::SetNesModel(NesModel::NTSC);
	}

	VirtualFile testMovie(filename, "TestMovie.mmo");
	VirtualFile testRom(filename, "TestRom.nes");

	ZipReader zipReader;
	zipReader.LoadArchive(filename);
	
	stringstream testData;
	zipReader.GetStream("TestData.mrt", testData);

	if(testData && testMovie.IsValid() && testRom.IsValid()) {
		char header[3];
		testData.read((char*)&header, 3);
		if(memcmp((char*)&header, "MRT", 3) != 0) {
			//Invalid test file
			return false;
		}

		EmulationSettings::SetFlags(EmulationFlags::ForceMaxSpeed);
		EmulationSettings::SetMasterVolume(0);
		
		Console::Pause();
		
		Reset();

		uint32_t hashCount;
		testData.read((char*)&hashCount, sizeof(uint32_t));
			
		for(uint32_t i = 0; i < hashCount; i++) {
			uint8_t repeatCount = 0;
			testData.read((char*)&repeatCount, sizeof(uint8_t));
			_repetitionCount.push_back(repeatCount);

			uint8_t* screenshotHash = new uint8_t[16];
			testData.read((char*)screenshotHash, 16);
			_screenshotHashes.push_back(screenshotHash);
		}

		_currentCount = _repetitionCount.front();
		_repetitionCount.pop_front();

		//Start playing movie
		if(Console::LoadROM(testRom)) {
			_runningTest = true;
			MovieManager::Play(testMovie);

			Console::Resume();
			EmulationSettings::ClearFlags(EmulationFlags::Paused);
			_signal.Wait();
			_runningTest = false;
			Console::GetInstance()->Stop();
		} else {
			//Something went wrong when loading the rom
			return -2;
		}

		EmulationSettings::ClearFlags(EmulationFlags::ForceMaxSpeed);
		EmulationSettings::SetMasterVolume(1.0);

		return _badFrameCount;
	}

	return -1;
}

void RecordedRomTest::Stop()
{
	if(_recording) {
		Save();
	}
	Reset();
}

void RecordedRomTest::Save()
{
	//Wait until the next frame is captured to end the recording
	_signal.Wait();
	_repetitionCount.push_back(_currentCount);
	_recording = false;

	//Stop playing/recording the movie
	MovieManager::Stop();

	_file.write("MRT", 3);

	uint32_t hashCount = (uint32_t)_screenshotHashes.size();
	_file.write((char*)&hashCount, sizeof(uint32_t));
		
	for(uint32_t i = 0; i < hashCount; i++) {
		_file.write((char*)&_repetitionCount[i], sizeof(uint8_t));
		_file.write((char*)&_screenshotHashes[i][0], 16);
	}

	_file.close();

	ZipWriter writer;
	writer.Initialize(_filename);

	string mrtFilename = FolderUtilities::CombinePath(FolderUtilities::GetFolderName(_filename), FolderUtilities::GetFilename(_filename, false) + ".mrt");
	writer.AddFile(mrtFilename, "TestData.mrt");
	std::remove(mrtFilename.c_str());

	if(_recordingFromMovie) {
		writer.AddFile(_movieData, "TestMovie.mmo");
		writer.AddFile(_romStream, "TestRom.nes");
	} else {
		string mmoFilename = FolderUtilities::CombinePath(FolderUtilities::GetFolderName(_filename), FolderUtilities::GetFilename(_filename, false) + ".mmo");
		writer.AddFile(mmoFilename, "TestMovie.mmo");
		std::remove(mmoFilename.c_str());

		writer.AddFile(Console::GetRomPath().GetFilePath(), "TestRom.nes");
	}
	writer.Save();

	MessageManager::DisplayMessage("Test", "TestFileSavedTo", FolderUtilities::GetFilename(_filename, true));
}