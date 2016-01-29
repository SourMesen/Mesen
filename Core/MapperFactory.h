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
		static shared_ptr<BaseMapper> InitializeFromFile(string romFilename, stringstream *filestream, string ipsFilename);
};
