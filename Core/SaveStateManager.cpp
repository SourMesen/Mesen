#include "stdafx.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/ZipWriter.h"
#include "../Utilities/ZipReader.h"
#include "SaveStateManager.h"
#include "MessageManager.h"
#include "Console.h"
#include "EmulationSettings.h"
#include "VideoDecoder.h"
#include "Debugger.h"
#include "MovieManager.h"
#include "RomData.h"

SaveStateManager::SaveStateManager(shared_ptr<Console> console)
{
	_console = console;
	_lastIndex = 1;
}

string SaveStateManager::GetStateFilepath(int stateIndex)
{
	string folder = FolderUtilities::GetSaveStateFolder();
	string filename = FolderUtilities::GetFilename(_console->GetRomInfo().RomName, false) + "_" + std::to_string(stateIndex) + ".mst";
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

void SaveStateManager::SelectSaveSlot(int slotIndex)
{
	_lastIndex = slotIndex;
	MessageManager::DisplayMessage("SaveStates", "SaveStateSlotSelected", std::to_string(_lastIndex));
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

void SaveStateManager::GetSaveStateHeader(ostream &stream)
{
	uint32_t emuVersion = EmulationSettings::GetMesenVersion();
	uint32_t formatVersion = SaveStateManager::FileFormatVersion;
	stream.write("MST", 3);
	stream.write((char*)&emuVersion, sizeof(emuVersion));
	stream.write((char*)&formatVersion, sizeof(uint32_t));

	RomInfo romInfo = _console->GetRomInfo();
	stream.write((char*)&romInfo.MapperID, sizeof(uint16_t));
	stream.write((char*)&romInfo.SubMapperID, sizeof(uint8_t));

	string sha1Hash = romInfo.Hash.Sha1;
	stream.write(sha1Hash.c_str(), sha1Hash.size());

	std::stringstream screenshotStream;
	_console->GetVideoDecoder()->TakeScreenshot(screenshotStream, true);
	uint32_t screenshotLength = (uint32_t)screenshotStream.tellp();
	stream.write((char*)&screenshotLength, sizeof(uint32_t));
	stream.write(screenshotStream.str().c_str(), screenshotLength);

	string romName = romInfo.RomName;
	uint32_t nameLength = (uint32_t)romName.size();
	stream.write((char*)&nameLength, sizeof(uint32_t));
	stream.write(romName.c_str(), romName.size());
}

void SaveStateManager::SaveState(ostream &stream)
{
	GetSaveStateHeader(stream);
	_console->SaveState(stream);
}

bool SaveStateManager::SaveState(string filepath)
{
	ofstream file(filepath, ios::out | ios::binary);

	if(file) {
		_console->Pause();
		SaveState(file);
		file.close();

		shared_ptr<Debugger> debugger = _console->GetDebugger(false);
		if(debugger) {
			debugger->ProcessEvent(EventType::StateSaved);
		}

		_console->Resume();
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
		if(fileFormatVersion <= 11) {
			MessageManager::DisplayMessage("SaveStates", "SaveStateIncompatibleVersion");
			return false;
		} else {
			int32_t mapperId = -1;
			int32_t subMapperId = -1;
			uint16_t id;
			uint8_t sid;
			stream.read((char*)&id, sizeof(uint16_t));
			stream.read((char*)&sid, sizeof(uint8_t));
			mapperId = id;
			subMapperId = sid;

			char hash[41] = {};
			stream.read(hash, 40);

			if(fileFormatVersion >= 13) {
				//Skip screenshot data
				uint32_t screenshotLength = 0;
				stream.read((char*)&screenshotLength, sizeof(uint32_t));
				stream.seekg(screenshotLength, std::ios::cur);
			}

			uint32_t nameLength = 0;
			stream.read((char*)&nameLength, sizeof(uint32_t));
			
			vector<char> nameBuffer(nameLength);
			stream.read(nameBuffer.data(), nameBuffer.size());
			string romName(nameBuffer.data(), nameLength);
			
			RomInfo romInfo = _console->GetRomInfo();
			bool gameLoaded = !romInfo.Hash.Sha1.empty();
			if(romInfo.Hash.Sha1 != string(hash)) {
				//CRC doesn't match
				if(!_console->GetSettings()->CheckFlag(EmulationFlags::AllowMismatchingSaveState) || !gameLoaded ||
					romInfo.MapperID != mapperId || romInfo.SubMapperID != subMapperId)
				{
					//If mismatching states aren't allowed, or a game isn't loaded, or the mapper types don't match, try to find and load the matching ROM
					HashInfo info;
					info.Sha1 = hash;
					if(!_console->LoadMatchingRom(romName, info)) {
						MessageManager::DisplayMessage("SaveStates", "SaveStateMissingRom", romName);
						return false;
					}
				}
			}
		}

		//Stop any movie that might have been playing/recording if a state is loaded
		//(Note: Loading a state is disabled in the UI while a movie is playing/recording)
		MovieManager::Stop();

		_console->LoadState(stream, fileFormatVersion);

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
		_console->Pause();
		if(LoadState(file, hashCheckRequired)) {
			result = true;
		}
		file.close();
		shared_ptr<Debugger> debugger = _console->GetDebugger(false);
		if(debugger) {
			debugger->ProcessEvent(EventType::StateLoaded);
		}
		_console->Resume();
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
	if(!_console->GetSettings()->CheckFlag(EmulationFlags::ConsoleMode) && !_console->GetSettings()->CheckFlag(EmulationFlags::DisableGameSelectionScreen) && _console->GetRomInfo().Format != RomFormat::Nsf) {
		string filename = FolderUtilities::GetFilename(_console->GetRomInfo().RomName, false) + ".rgd";
		ZipWriter writer;
		writer.Initialize(FolderUtilities::CombinePath(FolderUtilities::GetRecentGamesFolder(), filename));

		std::stringstream pngStream;
		_console->GetVideoDecoder()->TakeScreenshot(pngStream, true);
		writer.AddFile(pngStream, "Screenshot.png");

		std::stringstream stateStream;
		SaveStateManager::SaveState(stateStream);
		writer.AddFile(stateStream, "Savestate.mst");

		std::stringstream romInfoStream;
		romInfoStream << romName << std::endl;
		romInfoStream << romPath << std::endl;
		romInfoStream << patchPath << std::endl;
		writer.AddFile(romInfoStream, "RomInfo.txt");
		writer.Save();
	}
}

void SaveStateManager::LoadRecentGame(string filename, bool resetGame)
{
	ZipReader reader;
	reader.LoadArchive(filename);

	stringstream romInfoStream, stateStream;
	reader.GetStream("RomInfo.txt", romInfoStream);
	reader.GetStream("Savestate.mst", stateStream);

	string romName, romPath, patchPath;
	std::getline(romInfoStream, romName);
	std::getline(romInfoStream, romPath);
	std::getline(romInfoStream, patchPath);

	_console->Pause();
	try {
		if(_console->Initialize(romPath, patchPath)) {
			if(!resetGame) {
				SaveStateManager::LoadState(stateStream, false);
			}
		}
	} catch(std::exception&) { 
		_console->Stop();
	}
	_console->Resume();
}

int32_t SaveStateManager::GetSaveStatePreview(string saveStatePath, uint8_t* pngData)
{
	ifstream stream(saveStatePath, ios::binary);

	if(!stream) {
		return -1;
	}

	char header[3];
	stream.read(header, 3);
	if(memcmp(header, "MST", 3) == 0) {
		uint32_t emuVersion = 0;

		stream.read((char*)&emuVersion, sizeof(emuVersion));
		if(emuVersion > EmulationSettings::GetMesenVersion()) {
			return -1;
		}

		uint32_t fileFormatVersion = 0;
		stream.read((char*)&fileFormatVersion, sizeof(fileFormatVersion));
		if(fileFormatVersion <= 12) {
			return -1;
		}

		//Skip some header fields
		stream.seekg(43, ios::cur);

		uint32_t screenshotLength = 0;
		stream.read((char*)&screenshotLength, sizeof(screenshotLength));

		if(screenshotLength > 0) {
			stream.read((char*)pngData, screenshotLength);
			return screenshotLength;
		}

		return -1;
	}

	return -1;
}