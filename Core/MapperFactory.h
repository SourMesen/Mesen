#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "RomData.h"

class MapperFactory
{
	private:
		static BaseMapper* GetMapperFromID(RomData &romData);

	public:
		static const uint16_t FdsMapperID = 65535;
		static const uint16_t NsfMapperID = 65534;
		static shared_ptr<BaseMapper> InitializeFromFile(string romFilename, stringstream *filestream, string ipsFilename, int32_t archiveFileIndex);
};
