#include "stdafx.h"
#include "NESHeader.h"
#include "RomData.h"
#include "MessageManager.h"

uint16_t NESHeader::GetMapperID()
{
	switch(GetRomHeaderVersion()) {
		case RomHeaderVersion::Nes2_0:
			return ((Byte8 & 0x0F) << 4) | (Byte7 & 0xF0) | (Byte6 >> 4);
		default:
		case RomHeaderVersion::iNes:
			return (Byte7 & 0xF0) | (Byte6 >> 4);
		case RomHeaderVersion::OldiNes:
			return (Byte6 >> 4);
	}
}

bool NESHeader::HasBattery()
{
	return (Byte6 & 0x02) == 0x02;
}

bool NESHeader::HasTrainer()
{
	return (Byte6 & 0x04) == 0x04;
}

GameSystem NESHeader::GetNesGameSystem()
{
	if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
		switch(Byte12 & 0x03) {
			case 0: return GameSystem::NesNtsc;
			case 1: return GameSystem::NesPal;
			case 2: return GameSystem::NesNtsc; //Game works with both NTSC/PAL, pick NTSC by default
			case 3: return GameSystem::Dendy;
		}
	} else if(GetRomHeaderVersion() == RomHeaderVersion::iNes) {
		return (Byte9 & 0x01) ? GameSystem::NesPal : GameSystem::NesNtsc;
	}
	return GameSystem::NesNtsc;
}

GameSystem NESHeader::GetGameSystem()
{
	if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
		switch(Byte7 & 0x03) {
			case 0: return GetNesGameSystem();
			case 1: return GameSystem::VsSystem;
			case 2: return GameSystem::Playchoice;
			case 3:
				switch(Byte13) {
					case 0: return GetNesGameSystem();
					case 1: return GameSystem::VsSystem;
					case 2: return GameSystem::Playchoice;
					default:
						MessageManager::Log("[iNES] Unsupported console type detected (using NES NTSC instead)");
						return GameSystem::NesNtsc;

				}
				break;
		}
	} else if(GetRomHeaderVersion() == RomHeaderVersion::iNes) {
		if(Byte7 & 0x01) {
			return GameSystem::VsSystem;
		} else if(Byte7 & 0x02) {
			return GameSystem::Playchoice;
		}
	}
	return GetNesGameSystem();
}

RomHeaderVersion NESHeader::GetRomHeaderVersion()
{
	if((Byte7 & 0x0C) == 0x08) {
		return RomHeaderVersion::Nes2_0;
	} else if((Byte7 & 0x0C) == 0x00) {
		return RomHeaderVersion::iNes;
	} else {
		return RomHeaderVersion::OldiNes;
	}
}

uint32_t NESHeader::GetPrgSize()
{
	if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
		return (((Byte9 & 0x0F) << 8) | PrgCount) * 0x4000;
	} else {
		if(PrgCount == 0) {
			return 256 * 0x4000; //0 is a special value and means 256
		} else {
			return PrgCount * 0x4000;
		}
	}
}

uint32_t NESHeader::GetChrSize()
{
	if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
		return (((Byte9 & 0xF0) << 4) | ChrCount) * 0x2000;
	} else {
		return ChrCount * 0x2000;
	}
}

uint32_t NESHeader::GetWorkRamSize()
{
	if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
		uint8_t value = Byte10 & 0x0F;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value - 1);
	} else {
		return -1;
	}
}

uint32_t NESHeader::GetSaveRamSize()
{
	if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
		uint8_t value = (Byte10 & 0xF0) >> 4;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value - 1);
	} else {
		return -1;
	}
}

int32_t NESHeader::GetChrRamSize()
{
	if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
		uint8_t value = Byte11 & 0x0F;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value - 1);
	} else {
		return -1;
	}
}

uint32_t NESHeader::GetSaveChrRamSize()
{
	if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
		uint8_t value = (Byte11 & 0xF0) >> 4;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value - 1);
	} else {
		return -1;
	}
}

uint8_t NESHeader::GetSubMapper()
{
	if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
		return (Byte8 & 0xF0) >> 4;
	} else {
		return 0;
	}
}

MirroringType NESHeader::GetMirroringType()
{
	if(Byte6 & 0x08) {
		if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
			if(Byte6 & 0x01) {
				//Based on proposal by rainwarrior/Myask: http://wiki.nesdev.com/w/index.php/Talk:NES_2.0
				return MirroringType::ScreenAOnly;
			} else {
				return MirroringType::FourScreens;
			}
		} else {
			return MirroringType::FourScreens;
		}
	} else {
		return Byte6 & 0x01 ? MirroringType::Vertical : MirroringType::Horizontal;
	}
}

GameInputType NESHeader::GetInputType()
{
	if(Byte15 <= 0x2A) {
		return (GameInputType)Byte15;
	}

	MessageManager::Log("[iNES] Unknown controller type.");
	return GameInputType::Default;	
}

VsSystemType NESHeader::GetVsSystemType()
{
	if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
		if((Byte13 >> 4) <= 0x06) {
			return (VsSystemType)(Byte13 >> 4);
		}
		MessageManager::Log("[iNES] Unknown VS System Type specified.");
	}
	return VsSystemType::Default;
}

PpuModel NESHeader::GetVsSystemPpuModel()
{
	if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
		switch(Byte13 & 0x0F) {
			case 0: return PpuModel::Ppu2C03;
			case 1:
				MessageManager::Log("[iNES] Unsupport VS System Palette specified (2C03G).");
				return PpuModel::Ppu2C03;

			case 2: return PpuModel::Ppu2C04A;
			case 3: return PpuModel::Ppu2C04B;
			case 4: return PpuModel::Ppu2C04C;
			case 5: return PpuModel::Ppu2C04D;
			case 6: return PpuModel::Ppu2C03;
			case 7: return PpuModel::Ppu2C03;
			case 8: return PpuModel::Ppu2C05A;
			case 9: return PpuModel::Ppu2C05B;
			case 10: return PpuModel::Ppu2C05C;
			case 11: return PpuModel::Ppu2C05D;
			case 12: return PpuModel::Ppu2C05E;

			default:
				MessageManager::Log("[iNES] Unknown VS System Palette specified.");
				break;
		}
	}
	return PpuModel::Ppu2C03;
}

void NESHeader::SanitizeHeader(size_t romLength)
{
	size_t calculatedLength = sizeof(NESHeader) + GetPrgSize();
	while(calculatedLength > romLength) {
		Byte9 = 0;
		PrgCount--;
		calculatedLength = sizeof(NESHeader) + GetPrgSize();
	}

	calculatedLength = sizeof(NESHeader) + GetPrgSize() + GetChrSize();
	while(calculatedLength > romLength) {
		Byte9 = 0;
		ChrCount--;
		calculatedLength = sizeof(NESHeader) + GetPrgSize() + GetChrSize();
	}
}
