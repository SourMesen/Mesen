#pragma once

#include "stdafx.h"

class IpsPatcher
{
public:
	static vector<uint8_t> PatchBuffer(string ipsFilepath, vector<uint8_t> input);
	static vector<uint8_t> CreatePatch(vector<uint8_t> originalData, vector<uint8_t> newData);
};