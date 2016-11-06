#pragma once
#include "stdafx.h"
#include "RomData.h"
#include "GameDatabase.h"
#include <unordered_map>

class UnifLoader
{
private:
	std::unordered_map<string, int> _boardMappings = {
		{ "11160", MapperFactory::UnknownBoard },
		{ "12-IN-1", MapperFactory::UnknownBoard },
		{ "13in1JY110", MapperFactory::UnknownBoard },
		{ "190in1", MapperFactory::UnknownBoard },
		{ "22211", 132 },
		{ "3D-BLOCK", MapperFactory::UnknownBoard },
		{ "411120-C", MapperFactory::UnknownBoard },
		{ "42in1ResetSwitch", 226 },
		{ "43272", MapperFactory::UnknownBoard },
		{ "603-5052", 238 },
		{ "64in1NoRepeat", MapperFactory::UnknownBoard },
		{ "70in1", MapperFactory::UnknownBoard },
		{ "70in1B", MapperFactory::UnknownBoard },
		{ "810544-C-A1", MapperFactory::UnknownBoard },
		{ "8157", MapperFactory::UnknownBoard },
		{ "8237", 215 },
		{ "8237A", MapperFactory::UnknownBoard },
		{ "830118C", MapperFactory::UnknownBoard },
		{ "A65AS", MapperFactory::UnknownBoard },
		{ "AC08", MapperFactory::UnknownBoard },
		{ "ANROM", 7 },
		{ "AX5705", MapperFactory::UnknownBoard },
		{ "BB", MapperFactory::UnknownBoard },
		{ "BS-5", MapperFactory::UnknownBoard },
		{ "CC-21", MapperFactory::UnknownBoard },
		{ "CITYFIGHT", MapperFactory::UnknownBoard },
		{ "COOLBOY", MapperFactory::UnifCoolboy },
		{ "10-24-C-A1", MapperFactory::UnknownBoard },
		{ "CNROM", 3 },
		{ "CPROM", 13 },
		{ "D1038", 60 },
		{ "DANCE", MapperFactory::UnknownBoard },
		{ "DANCE2000", MapperFactory::UnknownBoard },
		{ "DREAMTECH01", MapperFactory::UnifDreamTech01 },
		{ "EDU2000", MapperFactory::UnifEdu2000 },
		{ "EKROM", 5 },
		{ "ELROM", 5 },
		{ "ETROM", 5 },
		{ "EWROM", 5 },
		{ "FK23C", MapperFactory::UnknownBoard },
		{ "FK23CA", MapperFactory::UnknownBoard },
		{ "FS304", 162 },
		{ "G-146", MapperFactory::UnknownBoard },
		{ "GK-192", 58 },
		{ "GS-2004", MapperFactory::UnifGs2004 },
		{ "GS-2013", MapperFactory::UnifGs2013 },
		{ "Ghostbusters63in1", MapperFactory::UnknownBoard },
		{ "H2288", 123 },
		{ "HKROM", MapperFactory::UnknownBoard },
		{ "KOF97", MapperFactory::UnifKof97 },
		{ "KONAMI-QTAI", MapperFactory::UnknownBoard },
		{ "KS7010", MapperFactory::UnknownBoard },
		{ "KS7012", MapperFactory::UnknownBoard },
		{ "KS7013B", MapperFactory::UnknownBoard },
		{ "KS7016", MapperFactory::UnknownBoard },
		{ "KS7017", MapperFactory::UnknownBoard },
		{ "KS7030", MapperFactory::UnknownBoard },
		{ "KS7031", MapperFactory::UnknownBoard },
		{ "KS7032", 142 },
		{ "KS7037", MapperFactory::UnknownBoard },
		{ "KS7057", MapperFactory::UnknownBoard },
		{ "LE05", MapperFactory::UnknownBoard },
		{ "LH10", MapperFactory::UnknownBoard },
		{ "LH32", 125 },
		{ "LH53", MapperFactory::UnknownBoard },
		{ "MALISB", MapperFactory::UnknownBoard },
		{ "MARIO1-MALEE2", MapperFactory::UnifMalee },
		{ "MHROM", 66 },
		{ "N625092", 221 },
		{ "NROM", 0 },
		{ "NROM-128", 0 },
		{ "NROM-256", 0 },
		{ "NTBROM", 68 },
		{ "NTD-03", MapperFactory::UnknownBoard },
		{ "NovelDiamond9999999in1", MapperFactory::UnifNovelDiamond },
		{ "OneBus", MapperFactory::UnknownBoard },
		{ "PEC-586", MapperFactory::UnknownBoard },
		{ "RET-CUFROM", 29 },
		{ "RROM", 0 },
		{ "RROM-128", 0 },
		{ "SA-002", 136 },
		{ "SA-0036", 149 },
		{ "SA-0037", 148 },
		{ "SA-009", 160 },
		{ "SA-016-1M", 146 },
		{ "SA-72007", 145 },
		{ "SA-72008", 133 },
		{ "SA-9602B", MapperFactory::UnknownBoard },
		{ "SA-NROM", 143 },
		{ "SAROM", 1 },
		{ "SBROM", 1 },
		{ "SC-127", 35 },
		{ "SCROM", 1 },
		{ "SEROM", 1 },
		{ "SGROM", 1 },
		{ "SHERO", MapperFactory::UnifStreetHeroes },
		{ "SKROM", 1 },
		{ "SL12", 116 },
		{ "SL1632", 14 },
		{ "SL1ROM", 1 },
		{ "SLROM", 1 },
		{ "SMB2J", MapperFactory::UnifSmb2j },
		{ "SNROM", 1 },
		{ "SOROM", 1 },
		{ "SSS-NROM-256", MapperFactory::UnknownBoard },
		{ "SUNSOFT_UNROM", 93 },
		{ "Sachen-74LS374N", 150 },
		{ "Sachen-74LS374NA", 243 },
		{ "Sachen-8259A", 141 },
		{ "Sachen-8259B", 138 },
		{ "Sachen-8259C", 139 },
		{ "Sachen-8259D", 137 },
		{ "Super24in1SC03", MapperFactory::UnknownBoard },
		{ "SuperHIK8in1", 45 },
		{ "Supervision16in1", 53 },
		{ "T-227-1", MapperFactory::UnknownBoard },
		{ "T-230", MapperFactory::UnknownBoard },
		{ "T-262", MapperFactory::UnknownBoard },
		{ "TBROM", 4 },
		{ "TC-U01-1.5M", 147 },
		{ "TEK90", 90 },
		{ "TEROM", 4 },
		{ "TF1201", MapperFactory::UnifTf1201 },
		{ "TFROM", 4 },
		{ "TGROM", 4 },
		{ "TKROM", 4 },
		{ "TKSROM", 4 },
		{ "TLROM", 4 },
		{ "TLSROM", 4 },
		{ "TQROM", 4 },
		{ "TR1ROM", 4 },
		{ "TSROM", 4 },
		{ "TVROM", 4 },
		{ "Transformer", MapperFactory::UnknownBoard },
		{ "UNROM", 2 },
		{ "UNROM-512-8", 30 },
		{ "UNROM-512-16", 30 },
		{ "UNROM-512-32", 30 },
		{ "UOROM", 2 },
		{ "VRC7", MapperFactory::UnknownBoard },
		{ "YOKO", MapperFactory::UnknownBoard },
		{ "SB-2000", MapperFactory::UnknownBoard },
		{ "COOLBOY", MapperFactory::UnknownBoard },
		{ "158B", MapperFactory::UnknownBoard },
		{ "DRAGONFIGHTER", MapperFactory::UnknownBoard },
		{ "EH8813A", MapperFactory::UnknownBoard },
		{ "HP898F", MapperFactory::UnknownBoard },
		{ "F-15", MapperFactory::UnknownBoard },
		{ "RT-01", MapperFactory::UnknownBoard },
		{ "81-01-31-C", MapperFactory::UnknownBoard },
		{ "8-IN-1", MapperFactory::UnknownBoard }
	};

	vector<uint8_t> _prgChunks[16];
	vector<uint8_t> _chrChunks[16];
	string _mapperName;

	void Read(uint8_t* &data, uint8_t& dest)
	{
		dest = data[0];
		data++;
	}

	void Read(uint8_t* &data, uint32_t& dest)
	{
		dest = data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24);
		data += 4;
	}

	void Read(uint8_t* &data, uint8_t* dest, size_t len)
	{
		memcpy(dest, data, len);
		data += len;
	}

	string ReadString(uint8_t* &data, uint8_t* chunkEnd)
	{
		stringstream ss;
		while(data < chunkEnd) {
			if(data[0] == 0) {
				//end of string
				data = chunkEnd;
				break;
			} else {
				ss << (char)data[0];
			}
			data++;
		}

		return ss.str();
	}

	string ReadFourCC(uint8_t* &data)
	{
		stringstream ss;
		for(int i = 0; i < 4; i++) {
			ss << (char)data[i];
		}
		data += 4;
		return ss.str();
	}

	bool ReadChunk(uint8_t* &data, uint8_t* dataEnd, RomData& romData)
	{
		if(data + 8 > dataEnd) {
			return false;
		}

		NsfHeader& header = romData.NsfHeader;

		string fourCC = ReadFourCC(data);

		uint32_t length;
		Read(data, length);

		uint8_t* chunkEnd = data + length;
		if(chunkEnd > dataEnd) {
			return false;
		}

		if(fourCC.compare("MAPR") == 0) {
			_mapperName = ReadString(data, chunkEnd);
			if(_mapperName.size() > 0) {
				romData.MapperID = GetMapperID(_mapperName);
			} else {
				romData.Error = true;
				return false;
			}
		} else if(fourCC.substr(0, 3).compare("PRG") == 0) {
			uint32_t chunkNumber;
			std::stringstream ss;
			ss << std::hex << fourCC[3];
			ss >> chunkNumber;

			_prgChunks[chunkNumber].resize(length);
			Read(data, _prgChunks[chunkNumber].data(), length);
		} else if(fourCC.substr(0, 3).compare("CHR") == 0) {
			uint32_t chunkNumber;
			std::stringstream ss;
			ss << std::hex << fourCC[3];
			ss >> chunkNumber;

			_chrChunks[chunkNumber].resize(length);
			Read(data, _chrChunks[chunkNumber].data(), length);
		} else if(fourCC.compare("TVCI") == 0) {
			uint8_t value;
			Read(data, value);
			romData.System = value == 1 ? GameSystem::NesPal : GameSystem::NesNtsc;
		} else if(fourCC.compare("CTRL") == 0) {
			//not supported
		} else if(fourCC.compare("BATR") == 0) {
			uint8_t value;
			Read(data, value);
			romData.HasBattery = value > 0;
		} else if(fourCC.compare("MIRR") == 0) {
			uint8_t value;
			Read(data, value);

			switch(value) {
				default:
				case 0: romData.MirroringType = MirroringType::Horizontal; break;
				case 1: romData.MirroringType = MirroringType::Vertical; break;
				case 2: romData.MirroringType = MirroringType::ScreenAOnly; break;
				case 3: romData.MirroringType = MirroringType::ScreenBOnly; break;
				case 4: romData.MirroringType = MirroringType::FourScreens; break;
			}
		} else {
			//Unsupported/unused FourCCs: PCKn, CCKn, NAME, WRTR, READ, DINF, VROR
		}

		data = chunkEnd;

		return true;
	}

	int32_t GetMapperID(string mapperName)
	{
		string prefix = mapperName.substr(0, 4);
		if(prefix.compare("NES-") == 0 || prefix.compare("UNL-") == 0 || prefix.compare("HVC-") == 0 || prefix.compare("BTL-") == 0 || prefix.compare("BMC-") == 0) {
			mapperName = mapperName.substr(4);
		}

		auto result = _boardMappings.find(mapperName);
		if(result != _boardMappings.end()) {
			return result->second;
		}

		MessageManager::Log("[UNIF] Error: Unknown board");

		return MapperFactory::UnknownBoard;
	}

public:
	RomData LoadRom(vector<uint8_t>& romFile)
	{
		RomData romData;

		//Skip header, version & null bytes, start reading at first chunk
		uint8_t* data = romFile.data() + 32;
		uint8_t* endOfFile = romFile.data() + romFile.size();

		while(ReadChunk(data, endOfFile, romData)) {
			//Read all chunks
		}

		for(int i = 0; i < 16; i++) {
			romData.PrgRom.insert(romData.PrgRom.end(), _prgChunks[i].begin(), _prgChunks[i].end());
			romData.ChrRom.insert(romData.ChrRom.end(), _chrChunks[i].begin(), _chrChunks[i].end());
		}

		if(romData.PrgRom.size() == 0 || _mapperName.empty()) {
			romData.Error = true;
		} else {
			vector<uint8_t> fullRom;
			fullRom.insert(fullRom.end(), romData.PrgRom.begin(), romData.PrgRom.end());
			fullRom.insert(fullRom.end(), romData.ChrRom.begin(), romData.ChrRom.end());

			romData.Crc32 = CRC32::GetCRC(fullRom.data(), fullRom.size());;
			romData.PrgCrc32 = CRC32::GetCRC(romData.PrgRom.data(), romData.PrgRom.size());

			stringstream crcHex;
			crcHex << std::hex << std::uppercase << std::setfill('0') << std::setw(8) << romData.Crc32;
			MessageManager::Log("PRG+CHR CRC32: 0x" + crcHex.str());

			MessageManager::Log("[UNIF] Board Name: " + _mapperName);
			MessageManager::Log("[UNIF] PRG ROM: " + std::to_string(romData.PrgRom.size() / 1024) + " KB");
			MessageManager::Log("[UNIF] CHR ROM: " + std::to_string(romData.ChrRom.size() / 1024) + " KB");
			if(romData.ChrRom.size() == 0) {
				MessageManager::Log("[UNIF] CHR RAM: 8 KB");
			}

			string mirroringType;
			switch(romData.MirroringType) {
				case MirroringType::Horizontal: mirroringType = "Horizontal"; break;
				case MirroringType::Vertical: mirroringType = "Vertical"; break;
				case MirroringType::ScreenAOnly: mirroringType = "1-Screen (A)"; break;
				case MirroringType::ScreenBOnly: mirroringType = "1-Screen (B)"; break;
				case MirroringType::FourScreens: mirroringType = "Four Screens"; break;
			}

			MessageManager::Log("[UNIF] Mirroring: " + mirroringType);
			MessageManager::Log("[UNIF] Battery: " + string(romData.HasBattery ? "Yes" : "No"));

			GameDatabase::SetGameInfo(romData.Crc32, romData, !EmulationSettings::CheckFlag(EmulationFlags::DisableGameDatabase));

			if(romData.MapperID == MapperFactory::UnknownBoard) {
				MessageManager::DisplayMessage("Error", "UnsupportedMapper", "UNIF: " + _mapperName);
				romData.Error = true;
			}
		}

		return romData;
	}
};