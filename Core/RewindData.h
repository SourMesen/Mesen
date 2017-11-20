#pragma once
#include "stdafx.h"
#include <deque>
#include "BaseControlDevice.h"

class RewindData
{
private:
	vector<uint8_t> SaveStateData;
	uint32_t OriginalSaveStateSize = 0;

	void CompressState(string stateData, vector<uint8_t> &compressedState);

public:
	std::deque<ControlDeviceState> InputLogs[BaseControlDevice::PortCount];
	int32_t FrameCount = 0;

	void LoadState();
	void SaveState();
};
