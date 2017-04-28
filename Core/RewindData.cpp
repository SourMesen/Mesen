#include "stdafx.h"
#include "RewindData.h"
#include "Console.h"
#include "../Utilities/miniz.h"

void RewindData::LoadState()
{
	unsigned long length = OriginalSaveStateSize;
	uint8_t* buffer = new uint8_t[length];
	uncompress(buffer, &length, SaveStateData.data(), (unsigned long)SaveStateData.size());
	Console::LoadState(buffer, length);
	delete[] buffer;
}

void RewindData::CompressState(string stateData, vector<uint8_t>& compressedState)
{
	unsigned long compressedSize = compressBound((unsigned long)stateData.size());
	uint8_t* compressedData = new uint8_t[compressedSize];
	compress(compressedData, &compressedSize, (unsigned char*)stateData.c_str(), (unsigned long)stateData.size());
	compressedState = vector<uint8_t>(compressedData, compressedData + compressedSize);
	delete[] compressedData;
}

void RewindData::SaveState()
{
	std::stringstream state;
	Console::SaveState(state);

	string stateData = state.str();
	vector<uint8_t> compressedState;
	CompressState(stateData, compressedState);
	SaveStateData = compressedState;
	OriginalSaveStateSize = (uint32_t)stateData.size();
	FrameCount = 0;
}
