#pragma once

#include "stdafx.h"

class SaveStateManager
{
private:
	static const uint32_t MaxIndex = 7;
	static atomic<uint32_t> _lastIndex;

	static string GetStateFilepath(int stateIndex);	

public:
	static const uint32_t FileFormatVersion = 8;

	static uint64_t GetStateInfo(int stateIndex);

	static void SaveState();
	static bool LoadState();

	static void SaveState(ostream &stream);
	static bool SaveState(string filepath);
	static void SaveState(int stateIndex, bool displayMessage = true);
	static bool LoadState(istream &stream, bool hashCheckRequired = true);
	static bool LoadState(string filepath, bool hashCheckRequired = true);
	static bool LoadState(int stateIndex);

	static void SaveRecentGame(string romName, string romPath, string patchPath);
	static void LoadRecentGame(string filename, bool resetGame);

	static void MoveToNextSlot();
	static void MoveToPreviousSlot();
};