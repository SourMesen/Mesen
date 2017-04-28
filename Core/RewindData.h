#pragma once
#include "stdafx.h"
#include <deque>

class RewindData
{
private:
	vector<uint8_t> SaveStateData;
	uint32_t OriginalSaveStateSize;

	void CompressState(string stateData, vector<uint8_t> &compressedState);

public:
	std::deque<uint8_t> InputLogs[4];
	int32_t FrameCount = 0;

	void LoadState();
	void SaveState();
};
