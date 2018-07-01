#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "RomData.h"
class MemoryManager;

class MapperFactory
{
	private:
		static BaseMapper* GetMapperFromID(RomData &romData);

	public:
		static constexpr uint16_t FdsMapperID = 65535;
		static constexpr uint16_t NsfMapperID = 65534;

		static shared_ptr<BaseMapper> InitializeFromFile(shared_ptr<Console> console, string romFilename, vector<uint8_t> &fileData);
};
