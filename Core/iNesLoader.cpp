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
	romData.NesHeader = header;

	if(romData.HasTrainer) {
		//512-byte trainer at $7000-$71FF (stored before PRG data)
		romData.TrainerData.insert(romData.TrainerData.end(), buffer, buffer + 512);
		buffer += 512;
	}

	uint32_t romCrc = CRC32::GetCRC(buffer, header.GetPrgSize() + header.GetChrSize());
	romData.PrgRom.insert(romData.PrgRom.end(), buffer, buffer + header.GetPrgSize());
	buffer += header.GetPrgSize();
	romData.ChrRom.insert(romData.ChrRom.end(), buffer, buffer + header.GetChrSize());

	romData.PrgCrc32 = CRC32::GetCRC(romData.PrgRom.data(), romData.PrgRom.size());

	stringstream crcHex;
	crcHex << std::hex << std::uppercase << std::setfill('0') << std::setw(8) << romCrc;
	MessageManager::Log("PRG+CHR CRC32: 0x" + crcHex.str());

	if(romData.IsNes20Header) {
		MessageManager::Log("[iNes] NES 2.0 file: Yes");
	}
	MessageManager::Log("[iNes] Mapper: " + std::to_string(romData.MapperID) + " Sub:" + std::to_string(romData.SubMapperID));
	MessageManager::Log("[iNes] PRG ROM: " + std::to_string(romData.PrgRom.size()/1024) + " KB");
	MessageManager::Log("[iNes] CHR ROM: " + std::to_string(romData.ChrRom.size()/1024) + " KB");
	if(romData.ChrRamSize > 0) {
		MessageManager::Log("[iNes] CHR RAM: " + std::to_string(romData.ChrRamSize) + " KB");
	} else if(romData.ChrRom.size() == 0) {
		MessageManager::Log("[iNes] CHR RAM: 8 KB");
	}
	MessageManager::Log("[iNes] Mirroring: " + string(romData.MirroringType == MirroringType::Horizontal ? "Horizontal" : romData.MirroringType == MirroringType::Vertical ? "Vertical" : "Four Screens"));
	MessageManager::Log("[iNes] Battery: " + string(romData.HasBattery ? "Yes" : "No"));
	if(romData.HasTrainer) {
		MessageManager::Log("[iNes] Trainer: Yes");
	}

	if(!EmulationSettings::CheckFlag(EmulationFlags::DisableGameDatabase) && header.GetRomHeaderVersion() != RomHeaderVersion::Nes2_0) {
		GameDatabase::UpdateRomData(romCrc, romData);
	}

	return romData;
}
