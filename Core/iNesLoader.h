#pragma once
#include "stdafx.h"
#include "RomData.h"

class iNesLoader
{
public:
	RomData LoadRom(vector<uint8_t>& romFile, NESHeader *preloadedHeader);
};