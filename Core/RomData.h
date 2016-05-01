#pragma once
#include "stdafx.h"

enum class MirroringType
{
	Horizontal,
	Vertical,
	ScreenAOnly,
	ScreenBOnly,
	FourScreens
};

enum class RomHeaderVersion
{
	iNes = 0,
	Nes2_0 = 1,
	OldiNes = 2
};

struct RomData
{
	string Filename;

	uint16_t MapperID;
	uint8_t SubMapperID = 0;
	bool HasBattery = false;
	bool IsPalRom = false;
	bool IsVsSystem = false;
	bool IsPlayChoice = false;
	MirroringType MirroringType;

	vector<uint8_t> PrgRom;
	vector<uint8_t> ChrRom;
	vector<vector<uint8_t>> FdsDiskData;

	vector<uint8_t> RawData;
	uint32_t Crc32;

	bool Error = false;
};

struct NESHeader
{
	/*
	Thing 	Archaic			 	iNES 									NES 2.0
	Byte 6	Mapper low nibble, Mirroring, Battery/Trainer flags
	Byte 7 	Unused 				Mapper high nibble, Vs. 		Mapper high nibble, NES 2.0 signature, PlayChoice, Vs.
	Byte 8 	Unused 				Total PRG RAM size (linear) 	Mapper highest nibble, mapper variant
	Byte 9 	Unused 				TV system 							Upper bits of ROM size
	Byte 10 	Unused 				Unused 								PRG RAM size (logarithmic; battery and non-battery)
	Byte 11 	Unused 				Unused 								VRAM size (logarithmic; battery and non-battery)
	Byte 12 	Unused 				Unused 								TV system
	Byte 13 	Unused 				Unused 								Vs. PPU variant
	*/
	char NES[4];
	uint8_t PrgCount;
	uint8_t ChrCount;
	uint8_t Byte6;
	uint8_t Byte7;
	uint8_t Byte8;
	uint8_t Byte9;
	uint8_t Byte10;
	uint8_t Byte11;
	uint8_t Byte12;
	uint8_t Byte13;
	uint8_t Reserved[2];

	uint16_t GetMapperID()
	{
		switch(GetRomHeaderVersion()) {
			case RomHeaderVersion::Nes2_0:
				return (Byte8 & 0x0F << 4) | (Byte7 & 0xF0) | (Byte6 >> 4);
			default:
			case RomHeaderVersion::iNes:
				return (Byte7 & 0xF0) | (Byte6 >> 4);
			case RomHeaderVersion::OldiNes:
				return (Byte6 >> 4);
		}
	}

	bool HasBattery()
	{
		return (Byte6 & 0x02) == 0x02;
	}

	bool HasTrainer()
	{
		return (Byte6 & 0x04) == 0x04;
	}

	bool IsPalRom()
	{
		switch(GetRomHeaderVersion()) {
			case RomHeaderVersion::Nes2_0: return (Byte12 & 0x01) == 0x01;
			case RomHeaderVersion::iNes: return (Byte9 & 0x01) == 0x01;
			default: return false;
		}
	}

	bool IsPlaychoice()
	{
		switch(GetRomHeaderVersion()) {
			case RomHeaderVersion::Nes2_0:
			case RomHeaderVersion::iNes: return (Byte7 & 0x02) == 0x02;
			default: return false;
		}
	}

	bool IsVsSystem()
	{
		switch(GetRomHeaderVersion()) {
			case RomHeaderVersion::Nes2_0: 
			case RomHeaderVersion::iNes: return (Byte7 & 0x01) == 0x01;
			default: return false;
		}
	}

	RomHeaderVersion GetRomHeaderVersion()
	{
		if((Byte7 & 0x0C) == 0x08) {
			return RomHeaderVersion::Nes2_0;
		} else if((Byte7 & 0x0C) == 0x00) {
			return RomHeaderVersion::iNes;
		} else {
			return RomHeaderVersion::OldiNes;
		}
	}

	uint32_t GetPrgSize()
	{
		if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
			return (((Byte9 & 0x0F) << 4) | PrgCount) * 0x4000;
		} else {
			return PrgCount * 0x4000;
		}
	}

	uint32_t GetChrSize()
	{
		if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
			return (((Byte9 & 0xF0) << 4) | ChrCount) * 0x2000;
		} else {
			return ChrCount * 0x2000;
		}
	}

	uint32_t GetWorkRamSize()
	{
		uint8_t value = Byte10 & 0x0F;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value);
	}

	uint32_t GetSaveRamSize()
	{
		uint8_t value = (Byte10 & 0xF0) >> 4;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value);
	}

	uint32_t GetChrRamSize()
	{
		uint8_t value = Byte11 & 0x0F;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value);
	}

	uint32_t GetSavedChrRamSize()
	{
		uint8_t value = (Byte10 & 0xF0) >> 4;
		return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value);
	}

	uint8_t GetSubMapper()
	{
		if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
			return (Byte8 & 0xF0) >> 4;
		} else {
			return 0;
		}
	}

	MirroringType GetMirroringType()
	{
		if(Byte6 & 0x08) {
			return MirroringType::FourScreens;
		} else {
			return Byte6 & 0x01 ? MirroringType::Vertical : MirroringType::Horizontal;
		}
	}

	void SanitizeHeader(size_t romLength)
	{
		size_t calculatedLength = sizeof(NESHeader) + 0x4000 * PrgCount;
		while(calculatedLength > romLength) {
			PrgCount--;
			calculatedLength = sizeof(NESHeader) + 0x4000 * PrgCount;
		}

		calculatedLength = sizeof(NESHeader) + 0x4000 * PrgCount + 0x2000 * ChrCount;
		while(calculatedLength > romLength) {
			ChrCount--;
			calculatedLength = sizeof(NESHeader) + 0x4000 * PrgCount + 0x2000 * ChrCount;
		}
	}
};