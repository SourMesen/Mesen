#include "stdafx.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/ZipWriter.h"
#include "../Utilities/ZipReader.h"
#include "SaveStateManager.h"
#include "MessageManager.h"
#include "Console.h"
#include "EmulationSettings.h"
#include "VideoDecoder.h"

const uint32_t SaveStateManager::FileFormatVersion;
atomic<uint32_t> SaveStateManager::_lastIndex(1);

string SaveStateManager::GetStateFilepath(int stateIndex)
{
	string folder = FolderUtilities::GetSaveStateFolder();
	string filename = FolderUtilities::GetFilename(Console::GetRomName(), false) + "_" + std::to_string(stateIndex) + ".mst";	
	return FolderUtilities::CombinePath(folder, filename);
}

uint64_t SaveStateManager::GetStateInfo(int stateIndex)
{
	string filepath = SaveStateManager::GetStateFilepath(stateIndex);
	ifstream file(filepath, ios::in | ios::binary);

	if(file) {
		file.close();
		return FolderUtilities::GetFileModificationTime(filepath);
	}
	return 0;
}

void SaveStateManager::MoveToNextSlot()
{
	_lastIndex = (_lastIndex % MaxIndex) + 1;
	MessageManager::DisplayMessage("SaveStates", "SaveStateSlotSelected", std::to_string(_lastIndex));
}

void SaveStateManager::MoveToPreviousSlot()
{
	_lastIndex = (_lastIndex == 1 ? SaveStateManager::MaxIndex : (_lastIndex - 1));
	MessageManager::DisplayMessage("SaveStates", "SaveStateSlotSelected", std::to_string(_lastIndex));
}

void SaveStateManager::SaveState()
{
	SaveState(_lastIndex);
}

bool SaveStateManager::LoadState()
{
	return LoadState(_lastIndex);
}

void SaveStateManager::SaveState(ostream &stream)
{
	Console::Pause();

	uint32_t emuVersion = EmulationSettings::GetMesenVersion();
	stream.write("MST", 3);
	stream.write((char*)&emuVersion, sizeof(emuVersion));
	stream.write((char*)&SaveStateManager::FileFormatVersion, sizeof(uint32_t));

	Console::SaveState(stream);
	Console::Resume();
}

void SaveStateManager::SaveState(int stateIndex, bool displayMessage)
{
	string filepath = SaveStateManager::GetStateFilepath(stateIndex);
	ofstream file(filepath, ios::out | ios::binary);

	if(file) {
		_lastIndex = stateIndex;
		SaveState(file);
		file.close();

		if(displayMessage) {
			MessageManager::DisplayMessage("SaveStates", "SaveStateSaved", std::to_string(stateIndex));
		}
	}
}

bool SaveStateManager::LoadState(istream &stream)
{
	char header[3];
	stream.read(header, 3);
	if(memcmp(header, "MST", 3) == 0) {
		uint32_t emuVersion, fileFormatVersion;

		stream.read((char*)&emuVersion, sizeof(emuVersion));
		if(emuVersion > EmulationSettings::GetMesenVersion()) {
			MessageManager::DisplayMessage("SaveStates", "SaveStateNewerVersion");
			return false;
		}

		stream.read((char*)&fileFormatVersion, sizeof(fileFormatVersion));
		if(fileFormatVersion != SaveStateManager::FileFormatVersion) {
			MessageManager::DisplayMessage("SaveStates", "SaveStateIncompatibleVersion"); // , std::to_string(stateIndex));
			return false;
		}

		Console::Pause();
		Console::LoadState(stream);
		Console::Resume();

		return true;
	}

	return false;
}

bool SaveStateManager::LoadState(int stateIndex)
{
	string filepath = SaveStateManager::GetStateFilepath(stateIndex);
	ifstream file(filepath, ios::in | ios::binary);
	bool result = false;

	if(file) {
		if(LoadState(file)) {
			_lastIndex = stateIndex;
			MessageManager::DisplayMessage("SaveStates", "SaveStateLoaded", std::to_string(stateIndex));
			result = true;
		} else {
			MessageManager::DisplayMessage("SaveStates", "SaveStateInvalidFile");
		}
		file.close();
	} 
	
	if(!result) {
		MessageManager::DisplayMessage("SaveStates", "SaveStateEmpty");
	}

	return result;
}

void SaveStateManager::SaveRecentGame(string romName, string romPath, string patchPath, int32_t archiveFileIndex)
{
	if(!EmulationSettings::CheckFlag(EmulationFlags::ConsoleMode) && Console::GetRomFormat() != RomFormat::Nsf) {
		string filename = FolderUtilities::GetFilename(Console::GetRomName(), false) + ".rgd";
		ZipWriter writer(FolderUtilities::CombinePath(FolderUtilities::GetRecentGamesFolder(), filename));

		std::stringstream pngStream;
		VideoDecoder::GetInstance()->TakeScreenshot(pngStream);
		writer.AddFile(pngStream, "Screenshot.png");

		std::stringstream stateStream;
		SaveStateManager::SaveState(stateStream);
		writer.AddFile(stateStream, "Savestate.mst");

		std::stringstream romInfoStream;
		romInfoStream << romName << std::endl;
		romInfoStream << romPath << std::endl;
		romInfoStream << patchPath << std::endl;
		romInfoStream << std::to_string(archiveFileIndex) << std::endl;
		writer.AddFile(romInfoStream, "RomInfo.txt");
	}
}

void SaveStateManager::LoadRecentGame(string filename)
{
	ZipReader reader;
	reader.LoadArchive(filename);

	std::stringstream romInfoStream = reader.GetStream("RomInfo.txt");
	std::stringstream stateStream = reader.GetStream("Savestate.mst");

	string romName, romPath, patchPath, archiveIndex;
	std::getline(romInfoStream, romName);
	std::getline(romInfoStream, romPath);
	std::getline(romInfoStream, patchPath);
	std::getline(romInfoStream, archiveIndex);

	Console::Pause();
	try {
		Console::LoadROM(romPath, nullptr, std::stoi(archiveIndex.c_str()), patchPath);
		SaveStateManager::LoadState(stateStream);
	} catch(std::exception ex) { 
		Console::GetInstance()->Stop();
	}
	Console::Resume();
}