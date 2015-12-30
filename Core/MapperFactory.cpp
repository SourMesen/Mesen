#include "stdafx.h"
#include "MessageManager.h"
#include "MapperFactory.h"
#include "ROMLoader.h"
#include "AXROM.h"
#include "Bandai74161_7432.h"
#include "CNROM.h"
#include "CpRom.h"
#include "ColorDreams.h"
#include "GxRom.h"
#include "IremG101.h"
#include "JalecoJfxx.h"
#include "JalecoSs88006.h"
#include "MMC1.h"
#include "MMC2.h"
#include "MMC3.h"
#include "MMC3_189.h"
#include "MMC4.h"
#include "MMC5.h"
#include "Nanjing.h"
#include "Nina03_06.h"
#include "NROM.h"
#include "TaitoTc0190.h"
#include "UnlPci556.h"
#include "UNROM.h"
#include "VRC2_4.h"
#include "BF909x.h"

BaseMapper* MapperFactory::GetMapperFromID(uint8_t mapperID)
{
#ifdef _DEBUG
	MessageManager::DisplayMessage("Game Info", "Mapper: " + std::to_string(mapperID));
#endif

	switch(mapperID) {
		case 0: return new NROM();
		case 1: return new MMC1();
		case 2: return new UNROM();
		case 3: return new CNROM();
		case 4: return new MMC3();
		case 5: return new MMC5();
		case 7: return new AXROM();
		case 9: return new MMC2();
		case 10: return new MMC4();
		case 11: return new ColorDreams();
		case 13: return new CpRom();
		case 16: break; //18 games
		case 18: return new JalecoSs88006();
		case 19: break; //16 games
		case 21: return new VRC2_4(VRCVariant::VRC4a);  //Conflicts: VRC4c
		case 22: return new VRC2_4(VRCVariant::VRC2a);
		case 23: return new VRC2_4(VRCVariant::VRC2b);  //Conflicts: VRC4e
		case 25: return new VRC2_4(VRCVariant::VRC4b);  //Conflicts: VRC2c, VRC4d
		case 27: return new VRC2_4(VRCVariant::VRC4_27);  //Untested
		case 32: return new IremG101();
		case 33: return new TaitoTc0190();
		case 34: break;
		case 37: break;
		case 38: return new UnlPci556();
		case 66: return new GxRom();
		case 70: return new Bandai74161_7432(false);
		case 71: return new BF909x();
		case 79: return new Nina03_06();
		case 87: return new JalecoJfxx(false);
		case 101: return new JalecoJfxx(true);
		case 152: return new Bandai74161_7432(true);
		case 163: return new Nanjing();
		case 189: return new MMC3_189();
	}

	MessageManager::DisplayMessage("Error", "Unsupported mapper, cannot load game.");
	return nullptr;
}

shared_ptr<BaseMapper> MapperFactory::InitializeFromFile(string romFilename, stringstream *filestream, string ipsFilename)
{
	ROMLoader loader;

	if(loader.LoadFile(romFilename, filestream, ipsFilename)) {
		uint8_t mapperID = loader.GetMapperID();

		shared_ptr<BaseMapper> mapper(GetMapperFromID(mapperID));

		if(mapper) {
			mapper->Initialize(loader);
			return mapper;
		}
	}
	return nullptr;
}

