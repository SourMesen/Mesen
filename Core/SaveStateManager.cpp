#include "stdafx.h"

#include "SaveStateManager.h"
#include "MessageManager.h"
#include "Console.h"
#include "../Utilities/FolderUtilities.h"

wstring SaveStateManager::GetStateFilepath(int stateIndex)
{
	wstring folder = FolderUtilities::GetSaveStateFolder();
	wstring filename = FolderUtilities::GetFilename(Console::GetROMPath(), false) + L"_" + std::to_wstring(stateIndex) + L".mst";	
	return FolderUtilities::CombinePath(folder, filename);
}

uint64_t SaveStateManager::GetStateInfo(int stateIndex)
{
	wstring filepath = SaveStateManager::GetStateFilepath(stateIndex);
	ifstream file(filepath, ios::in | ios::binary);

	if(file) {
		file.close();
		return FolderUtilities::GetFileModificationTime(filepath);
	}
	return 0;
}

void SaveStateManager::SaveState(int stateIndex)
{
	wstring filepath = SaveStateManager::GetStateFilepath(stateIndex);
	ofstream file(filepath, ios::out | ios::binary);

	if(file) {
		Console::Pause();
		Console::SaveState(file);
		Console::Resume();
		file.close();		
		MessageManager::DisplayMessage(L"Game States", L"State #" + std::to_wstring(stateIndex) + L" saved.");
	}
}

bool SaveStateManager::LoadState(int stateIndex)
{
	wstring filepath = SaveStateManager::GetStateFilepath(stateIndex);
	ifstream file(filepath, ios::in | ios::binary);

	if(file) {
		Console::Pause();
		Console::LoadState(file);
		Console::Resume();
		file.close();
		MessageManager::DisplayMessage(L"Game States", L"State #" + std::to_wstring(stateIndex) + L" loaded.");
		return true;
	}

	MessageManager::DisplayMessage(L"Game States", L"Slot is empty.");
	return false;
}