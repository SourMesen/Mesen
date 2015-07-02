#pragma once

#include "stdafx.h"

class SaveStateManager
{
private:
	static wstring SaveStateManager::GetStateFilepath(int stateIndex);

public:
	static uint64_t SaveStateManager::GetStateInfo(int stateIndex);
	static void SaveStateManager::SaveState(int stateIndex);
	static bool SaveStateManager::LoadState(int stateIndex);

};