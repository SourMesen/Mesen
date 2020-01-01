#pragma once
#include "stdafx.h"

class MemoryManager;
class Console;
class BaseMapper;
class VirtualFile;
struct RomData;

class MapperFactory
{
	private:
		static BaseMapper* GetMapperFromID(RomData &romData);

	public:
		static constexpr uint16_t FdsMapperID = 65535;
		static constexpr uint16_t NsfMapperID = 65534;
		static constexpr uint16_t StudyBoxMapperID = 65533;

		static shared_ptr<BaseMapper> InitializeFromFile(shared_ptr<Console> console, VirtualFile &romFile, RomData &outRomData);
};
