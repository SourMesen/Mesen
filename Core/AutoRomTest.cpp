#include "stdafx.h"

#include "AutoRomTest.h"
#include "Console.h"
#include "EmulationSettings.h"
#include "MessageManager.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/md5.h"
#include "../Utilities/ZipWriter.h"
#include "../Utilities/ZipReader.h"

AutoRomTest::AutoRomTest()
{
	Reset();

	MessageManager::RegisterNotificationListener(this);
}

AutoRomTest::~AutoRomTest()
{
	Reset();

	MessageManager::UnregisterNotificationListener(this);
}

void AutoRomTest::SaveFrame(uint16_t* ppuFrameBuffer)
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

void AutoRomTest::ValidateFrame(uint16_t* ppuFrameBuffer)
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
		_testResult = false;
		_signal.Signal();
	} else if (_currentCount == 0 && _repetitionCount.empty()) {
		//End of test
		_signal.Signal();
	}
}

void AutoRomTest::ProcessNotification(ConsoleNotificationType type, void* parameter)
{
	if(type == ConsoleNotificationType::PpuFrameDone) {
		if(_recording) {
			SaveFrame((uint16_t*)parameter);
		} else if(_runningTest) {
			ValidateFrame((uint16_t*)parameter);
		}
	}
}

void AutoRomTest::Reset()
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
	_testResult = true;
}

void AutoRomTest::Record(string filename, bool reset)
{
	_filename = filename;

	string mrtFilename = FolderUtilities::CombinePath(FolderUtilities::GetFolderName(filename), FolderUtilities::GetFilename(filename, false) + ".mrt");
	_file.open(mrtFilename, ios::out | ios::binary);

	if(_file) {
		Console::Pause();
		Reset();

		_recording = true;

		//Start recording movie alongside with screenshots
		Movie::Record(FolderUtilities::CombinePath(FolderUtilities::GetFolderName(filename), FolderUtilities::GetFilename(filename, false) + ".mmo"), reset);

		Console::Resume();
	}
}

bool AutoRomTest::Run(string filename)
{
	ZipReader zipReader;
	zipReader.LoadZipArchive(filename);
	std::stringstream testData = zipReader.ExtractFile("TestData.mrt");
	std::stringstream testMovie = zipReader.ExtractFile("TestMovie.mmo");
	std::stringstream testRom = zipReader.ExtractFile("TestRom.nes");

	if(testData) {
		char header[3];
		testData.read((char*)&header, 3);
		if(memcmp((char*)&header, "MRT", 3) != 0) {
			//Invalid test file
			return false;
		}

		EmulationSettings::SetEmulationSpeed(0);
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

		_runningTest = true;

		//Start playing movie
		Console::LoadROM("TestRom", &testRom);
		Movie::Play(testMovie);

		Console::Resume();

		_signal.Wait();

		Console::GetInstance()->Stop();

		_runningTest = false;

		EmulationSettings::SetEmulationSpeed(100);

		return _testResult;
	}

	return false;
}

void AutoRomTest::Stop()
{
	if(_recording) {
		Save();
	}
	Reset();
}

void AutoRomTest::Save()
{
	//Wait until the next frame is captured to end the recording
	_signal.Wait();
	_repetitionCount.push_back(_currentCount);
	_recording = false;

	//Stop recording the movie
	Movie::Stop();

	_file.write("MRT", 3);

	uint32_t hashCount = (uint32_t)_screenshotHashes.size();
	_file.write((char*)&hashCount, sizeof(uint32_t));
		
	for(uint32_t i = 0; i < hashCount; i++) {
		_file.write((char*)&_repetitionCount[i], sizeof(uint8_t));
		_file.write((char*)&_screenshotHashes[i][0], 16);
	}

	_file.close();

	string mrtFilename = FolderUtilities::CombinePath(FolderUtilities::GetFolderName(_filename), FolderUtilities::GetFilename(_filename, false) + ".mrt");
	string mmoFilename = FolderUtilities::CombinePath(FolderUtilities::GetFolderName(_filename), FolderUtilities::GetFilename(_filename, false) + ".mmo");

	ZipWriter writer(_filename);
	writer.AddFile(mrtFilename, "TestData.mrt");
	writer.AddFile(mmoFilename, "TestMovie.mmo");
	writer.AddFile(Console::GetROMPath(), "TestRom.nes");

	std::remove(mrtFilename.c_str());
	std::remove(mmoFilename.c_str());

	MessageManager::DisplayMessage("Test", "Test file saved to: " + FolderUtilities::GetFilename(_filename, true));
}