#include "stdafx.h"
#include "iNesLoader.h"
#include "../Utilities/CRC32.h"
#include "MessageManager.h"

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
	{ 0xD4611B79, 3 }  //WWF WrestleMania: Steel Cage Challenge
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

	if(romData.HasTrainer) {
		//512-byte trainer at $7000-$71FF (stored before PRG data)
		romData.TrainerData.insert(romData.TrainerData.end(), buffer, buffer + 512);
		buffer += 512;
	}

	if(header.GetRomHeaderVersion() != RomHeaderVersion::Nes2_0) {
		//Check rom CRC to set submapper as needed
		uint32_t romCrc = CRC32::GetCRC(buffer, header.GetPrgSize() + header.GetChrSize());
		auto crcCheckResult = _submapperByCrc.find(romCrc);
		if(crcCheckResult != _submapperByCrc.end()) {
			romData.SubMapperID = crcCheckResult->second;
			MessageManager::DisplayMessage("GameInfo", "Header information corrected.");
		}
	}

	romData.PrgRom.insert(romData.PrgRom.end(), buffer, buffer + header.GetPrgSize());
	buffer += header.GetPrgSize();
	romData.ChrRom.insert(romData.ChrRom.end(), buffer, buffer + header.GetChrSize());

	return romData;
}
