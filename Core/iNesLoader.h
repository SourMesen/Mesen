#pragma once
#include "stdafx.h"
#include "RomData.h"
#include "BaseLoader.h"

class iNesLoader : public BaseLoader
{
public:
	using BaseLoader::BaseLoader;

	void LoadRom(RomData& romData, vector<uint8_t>& romFile, NESHeader *preloadedHeader);
};