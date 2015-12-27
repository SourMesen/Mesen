#pragma once

#include "stdafx.h"

class IpsPatcher
{
public:
	static bool PatchBuffer(string ipsFilepath, uint8_t* inputBuffer, size_t inputBufferSize, uint8_t** outputBuffer, size_t &outputBufferSize);
};