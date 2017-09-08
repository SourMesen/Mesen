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

	string sha1Hash = Console::GetHashInfo().Sha1Hash;
	stream.write(sha1Hash.c_str(), sha1Hash.size());

	string romName = Console::GetRomName();
	uint32_t nameLength = (uint32_t)romName.size();
	stream.write((char*)&nameLength, sizeof(uint32_t));
	stream.write(romName.c_str(), romName.size());

	Console::SaveState(stream);
	Console::Resume();
}

bool SaveStateManager::SaveState(string filepath)
{
	ofstream file(filepath, ios::out | ios::binary);

	if(file) {
		SaveState(file);
		file.close();
		return true;
	}
	return false;
}

void SaveStateManager::SaveState(int stateIndex, bool displayMessage)
{
	string filepath = SaveStateManager::GetStateFilepath(stateIndex);
	if(SaveState(filepath)) {
		if(displayMessage) {
			MessageManager::DisplayMessage("SaveStates", "SaveStateSaved", std::to_string(stateIndex));
		}
	}
}

bool SaveStateManager::LoadState(istream &stream, bool hashCheckRequired)
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
		if(fileFormatVersion < 5) {
			MessageManager::DisplayMessage("SaveStates", "SaveStateIncompatibleVersion");
			return false;
		} else if(fileFormatVersion == 5) {
			//No SHA1 field in version 5
			if(hashCheckRequired) {
				//Can't manually load < v5 save states, since we can't know what game the save state is for
				MessageManager::DisplayMessage("SaveStates", "SaveStateIncompatibleVersion");
				return false;
			}
		} else {
			char hash[41] = {};
			stream.read(hash, 40);

			uint32_t nameLength = 0;
			stream.read((char*)&nameLength, sizeof(uint32_t));
			
			vector<char> nameBuffer(nameLength);
			stream.read(nameBuffer.data(), nameBuffer.size());
			string romName(nameBuffer.data(), nameLength);
			
			if(Console::GetHashInfo().Sha1Hash != string(hash)) {
				//Wrong game
				HashInfo info;
				info.Sha1Hash = hash;
				if(!Console::LoadROM(romName, info)) {
					MessageManager::DisplayMessage("SaveStates", "SaveStateMissingRom", romName);
					return false;
				}
			}
		}

		Console::Pause();
		Console::LoadState(stream);
		Console::Resume();

		return true;
	}
	MessageManager::DisplayMessage("SaveStates", "SaveStateInvalidFile");
	return false;
}

bool SaveStateManager::LoadState(string filepath, bool hashCheckRequired)
{
	ifstream file(filepath, ios::in | ios::binary);
	bool result = false;

	if(file.good()) {
		if(LoadState(file, hashCheckRequired)) {
			result = true;
		}
		file.close();
	} else {
		MessageManager::DisplayMessage("SaveStates", "SaveStateEmpty");
	}

	return result;
}

bool SaveStateManager::LoadState(int stateIndex)
{
	string filepath = SaveStateManager::GetStateFilepath(stateIndex);
	if(LoadState(filepath, false)) {
		MessageManager::DisplayMessage("SaveStates", "SaveStateLoaded", std::to_string(stateIndex));
		return true;
	}
	return false;
}

void SaveStateManager::SaveRecentGame(string romName, string romPath, string patchPath)
{
	if(!EmulationSettings::CheckFlag(EmulationFlags::ConsoleMode) && !EmulationSettings::CheckFlag(EmulationFlags::DisableGameSelectionScreen) && Console::GetRomFormat() != RomFormat::Nsf) {
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
		writer.AddFile(romInfoStream, "RomInfo.txt");
	}
}

void SaveStateManager::LoadRecentGame(string filename, bool resetGame)
{
	ZipReader reader;
	reader.LoadArchive(filename);

	std::stringstream romInfoStream = reader.GetStream("RomInfo.txt");
	std::stringstream stateStream = reader.GetStream("Savestate.mst");

	string romName, romPath, patchPath;
	std::getline(romInfoStream, romName);
	std::getline(romInfoStream, romPath);
	std::getline(romInfoStream, patchPath);

	Console::Pause();
	try {
		Console::LoadROM(romPath, patchPath);
		if(!resetGame) {
			SaveStateManager::LoadState(stateStream, false);
		}
	} catch(std::exception ex) { 
		Console::GetInstance()->Stop();
	}
	Console::Resume();
}