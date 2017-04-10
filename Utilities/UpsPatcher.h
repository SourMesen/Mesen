#pragma once

#include "stdafx.h"

class UpsPatcher
{
private:
	static uint64_t UpsPatcher::ReadBase128Number(ifstream &file);

public:
	static vector<uint8_t> PatchBuffer(string upsFilepath, vector<uint8_t> input);
};