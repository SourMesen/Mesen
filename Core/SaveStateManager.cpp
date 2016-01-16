#include "stdafx.h"

#include "SaveStateManager.h"
#include "MessageManager.h"
#include "Console.h"
#include "../Utilities/FolderUtilities.h"
#include "EmulationSettings.h"

string SaveStateManager::GetStateFilepath(int stateIndex)
{
	string folder = FolderUtilities::GetSaveStateFolder();
	string filename = FolderUtilities::GetFilename(Console::GetROMPath(), false) + "_" + std::to_string(stateIndex) + ".mst";	
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

void SaveStateManager::SaveState(int stateIndex)
{
	string filepath = SaveStateManager::GetStateFilepath(stateIndex);
	ofstream file(filepath, ios::out | ios::binary);

	if(file) {
		Console::Pause();

		uint32_t emuVersion = EmulationSettings::GetMesenVersion();
		file.write("MST", 3);
		file.write((char*)&emuVersion, sizeof(emuVersion));
		file.write((char*)&SaveStateManager::FileFormatVersion, sizeof(uint32_t));

		Console::SaveState(file);
		Console::Resume();
		file.close();		
		MessageManager::DisplayMessage("Game States", "State #" + std::to_string(stateIndex) + " saved.");
	}
}

bool SaveStateManager::LoadState(int stateIndex)
{
	string filepath = SaveStateManager::GetStateFilepath(stateIndex);
	ifstream file(filepath, ios::in | ios::binary);
	bool result = false;

	if(file) {
		char header[3];
		file.read(header, 3);
		if(memcmp(header, "MST", 3) == 0) {
			Console::Pause();

			uint32_t emuVersion, fileFormatVersion;
			file.read((char*)&emuVersion, sizeof(emuVersion));
			file.read((char*)&fileFormatVersion, sizeof(fileFormatVersion));

			if(emuVersion != EmulationSettings::GetMesenVersion() || fileFormatVersion != SaveStateManager::FileFormatVersion) {
				MessageManager::DisplayMessage("Game States", "State #" + std::to_string(stateIndex) + " does not match emulator version.");
			}

			Console::LoadState(file);
			Console::Resume();

			MessageManager::DisplayMessage("Game States", "State #" + std::to_string(stateIndex) + " loaded.");
			result = true;
		}
		file.close();
	} 
	
	if(!result) {
		MessageManager::DisplayMessage("Game States", "Slot is empty.");
	}

	return result;
}