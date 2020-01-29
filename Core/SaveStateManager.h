#pragma once
#include "stdafx.h"

class Console;

class SaveStateManager
{
private:
	static constexpr uint32_t MaxIndex = 10;

	atomic<uint32_t> _lastIndex;
	shared_ptr<Console> _console;

	string GetStateFilepath(int stateIndex);	
	void SaveScreenshotData(ostream& stream);
	bool GetScreenshotData(vector<uint8_t>& out, istream& stream);

public:
	static constexpr uint32_t FileFormatVersion = 13;

	SaveStateManager(shared_ptr<Console> console);

	void SaveState();
	bool LoadState();

	void GetSaveStateHeader(ostream & stream);

	void SaveState(ostream &stream);
	bool SaveState(string filepath);
	void SaveState(int stateIndex, bool displayMessage = true);
	bool LoadState(istream &stream, bool hashCheckRequired = true);
	bool LoadState(string filepath, bool hashCheckRequired = true);
	bool LoadState(int stateIndex);

	void SaveRecentGame(string romName, string romPath, string patchPath);
	void LoadRecentGame(string filename, bool resetGame);

	int32_t GetSaveStatePreview(string saveStatePath, uint8_t* pngData);

	void SelectSaveSlot(int slotIndex);
	void MoveToNextSlot();
	void MoveToPreviousSlot();
};