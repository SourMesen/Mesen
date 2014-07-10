#pragma once
#include "stdafx.h"
#include "Console.h"
#include "MapperFactory.h"
#include "ROMLoader.h"
#include "AXROM.h"
#include "CNROM.h"
#include "ColorDreams.h"
#include "MMC1.h"
#include "MMC2.h"
#include "MMC3.h"
#include "NROM.h"
#include "UNROM.h"
#include "VRC2_4.h"

BaseMapper* MapperFactory::GetMapperFromID(uint8_t mapperID)
{
	switch(mapperID) {
		case 0: return new NROM();
		case 1: return new MMC1();
		case 2: return new UNROM();
		case 3: return new CNROM();
		case 4: return new MMC3();
		case 5: break; //11 games
		case 7: return new AXROM();
		case 9: return new MMC2();
		case 11: return new ColorDreams();
		case 16: break; //18 games
		case 19: break; //16 games
		case 21: return new VRC2_4(VRCVariant::VRC4a);  //Conflicts: VRC4c
		case 22: return new VRC2_4(VRCVariant::VRC2a);
		case 23: return new VRC2_4(VRCVariant::VRC2b);  //Conflicts: VRC4e
		case 25: return new VRC2_4(VRCVariant::VRC4b);  //Conflicts: VRC2c, VRC4d
		case 27: return new VRC2_4(VRCVariant::VRC4_27);  //Untested
		case 71: return new UNROM(); //TODO: "It's largely a clone of UNROM, and Camerica games were initially emulated under iNES Mapper 002 before 071 was assigned."
		default: Console::DisplayMessage(L"Unsupported mapper, cannot load file.");
	}

	return nullptr;
}

shared_ptr<BaseMapper> MapperFactory::InitializeFromFile(wstring filename)
{
	ROMLoader loader;

	if(loader.LoadFile(filename)) {
		uint8_t mapperID = loader.GetMapperID();

		BaseMapper* mapper = GetMapperFromID(mapperID);

		if(mapper) {
			mapper->Initialize(loader);
			return shared_ptr<BaseMapper>(mapper);
		} else {
			loader.FreeMemory();
			return nullptr;
		}
	} else {
		return nullptr;
	}
}
