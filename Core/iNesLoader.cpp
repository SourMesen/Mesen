#include "stdafx.h"
#include "iNesLoader.h"
#include "../Utilities/CRC32.h"
#include "MessageManager.h"

std::unordered_map<uint32_t, uint8_t> iNesLoader::_mapperByCrc = {
	//Namco 175 games that are marked as mapper 19 but should be 210, submapper 1
	{ 0x0C47946D, 210 }, //Chibi Maruko-chan: Uki Uki Shopping
	{ 0x808606F0, 210 }, //Famista 91'
	{ 0x81B7F1A8, 210 }, //Heisei Tensai Bakabon
	{ 0x46FD7843, 210 }, //Splatterhouse: Wanpaku Graffiti
	{ 0x1DC0F740, 210 }, //Wagyan Land 2

	//Namco 340 games that are marked as mapper 19 but should be 210, submapper 2
	{ 0xBD523011, 210 }, //Dream Master
	{ 0xC247CC80, 210 }, //Family Circuit '91
	{ 0x6EC51DE5, 210 }, //Famista '92
	{ 0xADFFD64F, 210 }, //Famista '93
	{ 0x429103C9, 210 }, //Famista '94
	{ 0x2447E03B, 210 }, //Top Striker
	{ 0xD323B806, 210 }, //Wagnyan Land 3
};

std::unordered_map<uint32_t, uint8_t> iNesLoader::_submapperByCrc = {
	//MC-ACC Games (MMC3 variant) - Mapper 4, SubMapper 3
	{ 0xC527C297, 3 }, //Alien 3
	{ 0xAF05F37E, 3 }, //George Foreman's KO Boxing
	{ 0xA80A0F01, 3 }, //Incredible Crash Dummies
	{ 0x982DFB38, 3 }, //Mickey's Safari in Letterland
	{ 0x018A8699, 3 }, //Roger Clemens' MVP Baseball
	{ 0x2370C0A9, 3 }, //Rollerblade Racer
	{ 0x7416903F, 3 }, //Simpsons, The: Bart vs. The World
	{ 0x5991B9D0, 3 }, //Simpsons, The: Bartman Meets Radioactive Man
	{ 0xD679627A, 3 }, //Spider-Man: Return of the Sinister Six
	{ 0x7E57FBEC, 3 }, //T&C Surf Designs 2: Thrilla's Surfari
	{ 0xEA27B477, 3 }, //T2: Terminator 2: Judgment Day
	{ 0x7B4ED0BB, 3 }, //WWF King of the Ring
	{ 0xD4611B79, 3 }, //WWF WrestleMania: Steel Cage Challenge

	//IREM-G101 - Mapper 32, SubMapper 1
	{ 0x243A8735, 1 }, //Major League

	//IREM Holy Diver - Mapper 78, SubMapper 1
	{ 0xBA51AC6F, 3 }, //Holy Diver

	//CAMERICA-BF9097 - Mapper 71, SubMapper 1
	{ 0x1BC686A8, 1 }, //Fire Hawk

	//Namco 175 games that are marked as mapper 19 but should be 210, submapper 1
	{ 0x0C47946D, 1 }, //Chibi Maruko-chan: Uki Uki Shopping
	{ 0x808606F0, 1 }, //Famista 91'
	{ 0x81B7F1A8, 1 }, //Heisei Tensai Bakabon
	{ 0x46FD7843, 1 }, //Splatterhouse: Wanpaku Graffiti
	{ 0x1DC0F740, 1 }, //Wagyan Land 2

	//Namco 340 games that are marked as mapper 19 but should be 210, submapper 2
	{ 0xBD523011, 2 }, //Dream Master
	{ 0xC247CC80, 2 }, //Family Circuit '91
	{ 0x6EC51DE5, 2 }, //Famista '92
	{ 0xADFFD64F, 2 }, //Famista '93
	{ 0x429103C9, 2 }, //Famista '94
	{ 0x2447E03B, 2 }, //Top Striker
	{ 0xD323B806, 2 }, //Wagnyan Land 3
};

RomData iNesLoader::LoadRom(vector<uint8_t>& romFile)
{
	RomData romData;

	uint8_t* buffer = romFile.data();
	NESHeader header;
	memcpy((char*)&header, buffer, sizeof(NESHeader));
	buffer += sizeof(NESHeader);

	header.SanitizeHeader(romFile.size());

	romData.MapperID = header.GetMapperID();
	romData.SubMapperID = header.GetSubMapper();
	romData.MirroringType = header.GetMirroringType();
	romData.HasBattery = header.HasBattery();
	romData.IsPalRom = header.IsPalRom();
	romData.IsVsSystem = header.IsVsSystem();
	romData.IsPlayChoice = header.IsPlaychoice();
	romData.HasTrainer = header.HasTrainer();
	romData.ChrRamSize = header.GetChrRamSize();

	if(romData.HasTrainer) {
		//512-byte trainer at $7000-$71FF (stored before PRG data)
		romData.TrainerData.insert(romData.TrainerData.end(), buffer, buffer + 512);
		buffer += 512;
	}

	if(header.GetRomHeaderVersion() != RomHeaderVersion::Nes2_0) {
		//Check rom CRC to set submapper as needed
		uint32_t romCrc = CRC32::GetCRC(buffer, header.GetPrgSize() + header.GetChrSize());
		auto crcCheckResult = _mapperByCrc.find(romCrc);
		if(crcCheckResult != _mapperByCrc.end()) {
			romData.MapperID = crcCheckResult->second;
			#ifdef _DEBUG
			MessageManager::DisplayMessage("GameInfo", "Mapper number corrected.");
			#endif
		}

		crcCheckResult = _submapperByCrc.find(romCrc);
		if(crcCheckResult != _submapperByCrc.end()) {
			romData.SubMapperID = crcCheckResult->second;
			#ifdef _DEBUG
			MessageManager::DisplayMessage("GameInfo", "Submapper number corrected.");
			#endif
		}
	}

	romData.PrgRom.insert(romData.PrgRom.end(), buffer, buffer + header.GetPrgSize());
	buffer += header.GetPrgSize();
	romData.ChrRom.insert(romData.ChrRom.end(), buffer, buffer + header.GetChrSize());

	return romData;
}
