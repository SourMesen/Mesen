#include "stdafx.h"
#include "iNesLoader.h"
#include "../Utilities/CRC32.h"
#include "GameDatabase.h"
#include "EmulationSettings.h"

RomData iNesLoader::LoadRom(vector<uint8_t>& romFile)
{
	RomData romData;

	uint8_t* buffer = romFile.data();
	NESHeader header;
	memcpy((char*)&header, buffer, sizeof(NESHeader));
	buffer += sizeof(NESHeader);

	header.SanitizeHeader(romFile.size());

	romData.IsNes20Header = (header.GetRomHeaderVersion() == RomHeaderVersion::Nes2_0);
	romData.MapperID = header.GetMapperID();
	romData.SubMapperID = header.GetSubMapper();
	romData.MirroringType = header.GetMirroringType();
	romData.HasBattery = header.HasBattery();
	if(header.IsPalRom()) {
		romData.System = GameSystem::NesPal;
	} else if(header.IsVsSystem()) {
		romData.System = GameSystem::VsUniSystem;
	} else if(header.IsPlaychoice()) {
		romData.System = GameSystem::Playchoice;
	}
	romData.HasTrainer = header.HasTrainer();
	romData.ChrRamSize = header.GetChrRamSize();

	if(romData.HasTrainer) {
		//512-byte trainer at $7000-$71FF (stored before PRG data)
		romData.TrainerData.insert(romData.TrainerData.end(), buffer, buffer + 512);
		buffer += 512;
	}

	uint32_t romCrc = CRC32::GetCRC(buffer, header.GetPrgSize() + header.GetChrSize());
	romData.PrgRom.insert(romData.PrgRom.end(), buffer, buffer + header.GetPrgSize());
	buffer += header.GetPrgSize();
	romData.ChrRom.insert(romData.ChrRom.end(), buffer, buffer + header.GetChrSize());

	if(!EmulationSettings::CheckFlag(EmulationFlags::DisableGameDatabase) && header.GetRomHeaderVersion() != RomHeaderVersion::Nes2_0) {
		GameDatabase::UpdateRomData(romCrc, romData);
	}

	return romData;
}
