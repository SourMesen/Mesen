#include "stdafx.h"
#include "iNesLoader.h"
#include "../Utilities/CRC32.h"
#include "../Utilities/md5.h"
#include "../Utilities/HexUtilities.h"
#include "GameDatabase.h"
#include "EmulationSettings.h"

RomData iNesLoader::LoadRom(vector<uint8_t>& romFile, NESHeader *preloadedHeader)
{
	RomData romData;

	NESHeader header;
	uint8_t* buffer = romFile.data();
	uint32_t dataSize = (uint32_t)romFile.size();
	if(preloadedHeader) {
		header = *preloadedHeader;
	} else {
		memcpy((char*)&header, buffer, sizeof(NESHeader));
		buffer += sizeof(NESHeader);
		dataSize -= sizeof(NESHeader);
	}

	romData.Info.Format = RomFormat::iNes;

	//NSF header size
	romData.Info.FilePrgOffset = sizeof(NESHeader);

	romData.Info.IsNes20Header = (header.GetRomHeaderVersion() == RomHeaderVersion::Nes2_0);
	romData.Info.MapperID = header.GetMapperID();
	romData.Info.SubMapperID = header.GetSubMapper();
	romData.Info.Mirroring = header.GetMirroringType();
	romData.Info.HasBattery = header.HasBattery();
	romData.Info.System = header.GetGameSystem();
	romData.Info.VsType = header.GetVsSystemType();
	romData.Info.VsPpuModel = header.GetVsSystemPpuModel();
	romData.Info.InputType = header.GetInputType();
	romData.Info.HasTrainer = header.HasTrainer();
	romData.Info.NesHeader = header;

	romData.ChrRamSize = header.GetChrRamSize();
	romData.SaveChrRamSize = header.GetSaveChrRamSize();
	romData.WorkRamSize = header.GetWorkRamSize();
	romData.SaveRamSize = header.GetSaveRamSize();

	if(romData.Info.HasTrainer) {
		if(dataSize >= 512) {
			//512-byte trainer at $7000-$71FF (stored before PRG data)
			romData.TrainerData.insert(romData.TrainerData.end(), buffer, buffer + 512);
			buffer += 512;
			dataSize -= 512;
		} else {
			romData.Error = true;
			MessageManager::Log("[iNes] Invalid file (file length does not match header information) - load operation cancelled.");
			return romData;
		}
	}

	size_t bytesRead = buffer - romFile.data();

	uint32_t romCrc = CRC32::GetCRC(buffer, romFile.size() - bytesRead);
	romData.Info.Hash.PrgChrCrc32 = romCrc;
	romData.Info.Hash.PrgChrMd5 = GetMd5Sum(buffer, romFile.size() - bytesRead);

	uint32_t prgSize = 0;
	uint32_t chrSize = 0;

	if(!GameDatabase::IsEnabled() || !GameDatabase::GetDbRomSize(romData.Info.Hash.PrgChrCrc32, prgSize, chrSize)) {
		//Fallback on header sizes when game is not in DB (or DB is disabled)
		prgSize = header.GetPrgSize();
		chrSize = header.GetChrSize();
	}

	if(prgSize + chrSize > dataSize) {
		//Invalid rom file
		MessageManager::Log("[iNes] Invalid file (file length does not match header information) - load operation cancelled.");
		romData.Error = true;
		return romData;
	} else if(prgSize + chrSize < dataSize) {
		MessageManager::Log("[iNes] Warning: File is larger than excepted (based on the file header).");
	}

	romData.PrgRom.insert(romData.PrgRom.end(), buffer, buffer + prgSize);
	buffer += prgSize;
	romData.ChrRom.insert(romData.ChrRom.end(), buffer, buffer + chrSize);

	romData.Info.Hash.PrgCrc32 = CRC32::GetCRC(romData.PrgRom.data(), romData.PrgRom.size());

	Log("PRG CRC32: 0x" + HexUtilities::ToHex(romData.Info.Hash.PrgCrc32, true));
	Log("PRG+CHR CRC32: 0x" + HexUtilities::ToHex(romData.Info.Hash.PrgChrCrc32, true));

	if(romData.Info.IsNes20Header) {
		Log("[iNes] NES 2.0 file: Yes");
	}
	Log("[iNes] Mapper: " + std::to_string(romData.Info.MapperID) + " Sub:" + std::to_string(romData.Info.SubMapperID));
	
	if(romData.Info.System == GameSystem::VsSystem) {
		string type = "Vs-UniSystem";
		switch(romData.Info.VsType) {
			case VsSystemType::Default: break;
			case VsSystemType::IceClimberProtection: type = "VS-UniSystem (Ice Climbers)"; break;
			case VsSystemType::RaidOnBungelingBayProtection: type = "VS-DualSystem (Raid on Bungeling Bay)"; break;
			case VsSystemType::RbiBaseballProtection: type = "VS-UniSystem (RBI Baseball)"; break;
			case VsSystemType::SuperXeviousProtection: type = "VS-UniSystem (Super Xevious)"; break;
			case VsSystemType::TkoBoxingProtection: type = "VS-UniSystem (TKO Boxing)"; break;
			case VsSystemType::VsDualSystem: type = "VS-DualSystem"; break;
		}
		Log("[iNes] System: " + type);
	}

	Log("[iNes] PRG ROM: " + std::to_string(romData.PrgRom.size()/1024) + " KB");
	Log("[iNes] CHR ROM: " + std::to_string(romData.ChrRom.size()/1024) + " KB");
	if(romData.ChrRamSize > 0 || romData.Info.IsNes20Header) {
		Log("[iNes] CHR RAM: " + std::to_string(romData.ChrRamSize / 1024) + " KB");
	} else if(romData.ChrRom.size() == 0) {
		Log("[iNes] CHR RAM: 8 KB");
	}
	if(romData.WorkRamSize > 0 || romData.Info.IsNes20Header) {
		Log("[iNes] Work RAM: " + std::to_string(romData.WorkRamSize / 1024) + " KB");
	}
	if(romData.SaveRamSize > 0 || romData.Info.IsNes20Header) {
		Log("[iNes] Save RAM: " + std::to_string(romData.SaveRamSize / 1024) + " KB");
	}

	Log("[iNes] Mirroring: " + string(romData.Info.Mirroring == MirroringType::Horizontal ? "Horizontal" : romData.Info.Mirroring == MirroringType::Vertical ? "Vertical" : "Four Screens"));
	Log("[iNes] Battery: " + string(romData.Info.HasBattery ? "Yes" : "No"));
	if(romData.Info.HasTrainer) {
		Log("[iNes] Trainer: Yes");
	}

	if(!_checkOnly) {
		GameDatabase::SetGameInfo(romData.Info.Hash.PrgChrCrc32, romData, GameDatabase::IsEnabled() && header.GetRomHeaderVersion() != RomHeaderVersion::Nes2_0, preloadedHeader != nullptr);
	}

	return romData;
}
