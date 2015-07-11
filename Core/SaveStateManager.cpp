#include "stdafx.h"

#include "SaveStateManager.h"
#include "MessageManager.h"
#include "Console.h"
#include "../Utilities/FolderUtilities.h"

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

	if(file) {
		Console::Pause();
		Console::LoadState(file);
		Console::Resume();
		file.close();
		MessageManager::DisplayMessage("Game States", "State #" + std::to_string(stateIndex) + " loaded.");
		return true;
	}

	MessageManager::DisplayMessage("Game States", "Slot is empty.");
	return false;
}