#pragma once

#include "stdafx.h"

class BpsPatcher
{
private:
	static uint64_t ReadBase128Number(ifstream &file);

public:
	static vector<uint8_t> PatchBuffer(string bpsFilepath, vector<uint8_t> input);
};