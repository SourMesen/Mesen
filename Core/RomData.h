#pragma once
#include "stdafx.h"
#include <cmath>

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

enum class GameSystem
{
	NesNtsc,
	NesPal,
	Famicom,
	Dendy,
	VsUniSystem,
	Playchoice,
	FDS,
	Unknown,
};

enum class BusConflictType
{
	Default = 0,
	Yes,
	No
};

struct HashInfo
{
	uint32_t Crc32Hash = 0;
	uint32_t PrgCrc32Hash = 0;
	string Sha1Hash;
	string PrgChrMd5Hash;
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
				return ((Byte8 & 0x0F) << 4) | (Byte7 & 0xF0) | (Byte6 >> 4);
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
			return (((Byte9 & 0x0F) << 8) | PrgCount) * 0x4000;
		} else {
			if(PrgCount == 0) {
				return 256 * 0x4000; //0 is a special value and means 256
			} else {
				return PrgCount * 0x4000;
			}
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
		if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
			uint8_t value = Byte10 & 0x0F;
			return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value - 1);
		} else {
			return -1;
		}
	}

	uint32_t GetSaveRamSize()
	{
		if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
			uint8_t value = (Byte10 & 0xF0) >> 4;
			return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value - 1);
		} else {
			return -1;
		}
	}

	int32_t GetChrRamSize()
	{
		if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
			uint8_t value = Byte11 & 0x0F;
			return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value - 1);
		} else {
			return -1;
		}
	}

	uint32_t GetSaveChrRamSize()
	{
		if(GetRomHeaderVersion() == RomHeaderVersion::Nes2_0) {
			uint8_t value = (Byte11 & 0xF0) >> 4;
			return value == 0 ? 0 : 128 * (uint32_t)std::pow(2, value - 1);
		} else {
			return -1;
		}
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

	void SanitizeHeader(size_t romLength)
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
};

struct NsfHeader
{
	char Header[5];
	uint8_t Version;
	uint8_t TotalSongs;
	uint8_t StartingSong;
	uint16_t LoadAddress;
	uint16_t InitAddress;
	uint16_t PlayAddress;
	char SongName[256];
	char ArtistName[256];
	char CopyrightHolder[256];
	uint16_t PlaySpeedNtsc;
	uint8_t BankSetup[8];
	uint16_t PlaySpeedPal;
	uint8_t Flags;
	uint8_t SoundChips;
	uint8_t Padding[4];

	//NSFe extensions
	char RipperName[256];
	char TrackName[20000];
	int32_t TrackLength[256];
	int32_t TrackFade[256];
};

struct GameInfo
{
	uint32_t Crc;
	string System;
	string Board;
	string Pcb;
	string Chip;
	uint16_t MapperID;
	uint32_t PrgRomSize;
	uint32_t ChrRomSize;
	uint32_t ChrRamSize;
	uint32_t WorkRamSize;
	uint32_t SaveRamSize;
	bool HasBattery;
	string Mirroring;
	string InputType;
	string BusConflicts;
	string SubmapperID;
};

enum class RomFormat
{
	Unknown = 0,
	iNes = 1,
	Unif = 2,
	Fds = 3,
	Nsf = 4,
};

struct RomData
{
	string RomName;
	string Filename;
	RomFormat Format;

	uint16_t MapperID = 0;
	uint8_t SubMapperID = 0;
	GameSystem System = GameSystem::Unknown;
	bool HasBattery = false;
	bool HasTrainer = false;
	MirroringType Mirroring = MirroringType::Horizontal;
	BusConflictType BusConflicts = BusConflictType::Default;
	
	int32_t ChrRamSize = -1;
	int32_t SaveChrRamSize = -1;
	int32_t SaveRamSize = -1;
	int32_t WorkRamSize = -1;

	bool IsNes20Header = false;

	vector<uint8_t> PrgRom;
	vector<uint8_t> ChrRom;
	vector<uint8_t> TrainerData;
	vector<vector<uint8_t>> FdsDiskData;
	vector<vector<uint8_t>> FdsDiskHeaders;

	vector<uint8_t> RawData;
	string Sha1;
	string PrgChrMd5;
	uint32_t Crc32 = 0;
	uint32_t PrgCrc32 = 0;
	uint32_t PrgChrCrc32 = 0;

	bool Error = false;

	NESHeader NesHeader;
	NsfHeader NsfInfo;
	GameInfo DatabaseInfo;
};