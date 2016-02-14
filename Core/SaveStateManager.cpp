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
		MessageManager::DisplayMessage("Save States", "State #" + std::to_string(stateIndex) + " saved.");
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
			uint32_t emuVersion, fileFormatVersion;

			file.read((char*)&emuVersion, sizeof(emuVersion));
			if(emuVersion > EmulationSettings::GetMesenVersion()) {
				MessageManager::DisplayMessage("Save States", "Cannot load save states created by a more recent version of Mesen. Please download the latest version.");
				return false;
			}

			file.read((char*)&fileFormatVersion, sizeof(fileFormatVersion));
			if(fileFormatVersion != SaveStateManager::FileFormatVersion) {
				MessageManager::DisplayMessage("Save States", "State #" + std::to_string(stateIndex) + " is incompatible with this version of Mesen.");
				return false;
			}

			Console::Pause();
			Console::LoadState(file);
			Console::Resume();

			MessageManager::DisplayMessage("Save States", "State #" + std::to_string(stateIndex) + " loaded.");
			result = true;
		} else {
			MessageManager::DisplayMessage("Save States", "Invalid save state file.");
		}
		file.close();
	} 
	
	if(!result) {
		MessageManager::DisplayMessage("Save States", "Slot is empty.");
	}

	return result;
}