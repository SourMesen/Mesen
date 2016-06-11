#pragma once

#include "stdafx.h"

class SaveStateManager
{
private:
	static string GetStateFilepath(int stateIndex);

public:
	static const uint32_t FileFormatVersion = 4;

	static uint64_t GetStateInfo(int stateIndex);
	static void SaveState(int stateIndex);
	static bool LoadState(int stateIndex);

};