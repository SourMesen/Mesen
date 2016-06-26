#pragma once
#include "stdafx.h"
#include "RomData.h"

class NsfLoader
{
private:
	void Read(uint8_t* &data, uint8_t& dest)
	{
		dest = data[0];
		data++;
	}

	void Read(uint8_t* &data, uint16_t& dest)
	{
		dest = data[0] | (data[1] << 8);
		data += 2;
	}

	void Read(uint8_t* &data, char* dest, size_t len)
	{
		memcpy(dest, data, len);
		data += len;
	}
protected:
	void InitializeFromHeader(RomData& romData)
	{
		NsfHeader &header = romData.NsfHeader;

		romData.MapperID = MapperFactory::NsfMapperID;

		if(header.LoadAddress < 0x6000) {
			romData.Error = true;
		}

		if(header.Flags == 0x01) {
			romData.System = GameSystem::NesPal;
		}

		//Log window output
		MessageManager::Log("[NSF] Region: " + string(header.Flags == 0x00 ? "NTSC" : (header.Flags == 0x01 ? "PAL" : "NTSC & PAL")));
		if(header.PlaySpeedNtsc > 0) {
			MessageManager::Log("[NSF] Play speed (NTSC): " + std::to_string(1000000.0 / (double)header.PlaySpeedNtsc) + " Hz");
		}
		if(header.PlaySpeedPal > 0) {
			MessageManager::Log("[NSF] Play speed (PAL): " + std::to_string(1000000.0 / (double)header.PlaySpeedPal) + " Hz");
		}
		MessageManager::Log("[NSF] Title: " + string(header.SongName));
		MessageManager::Log("[NSF] Artist: " + string(header.ArtistName));
		MessageManager::Log("[NSF] Copyright: " + string(header.CopyrightHolder));
		MessageManager::Log("[NSF] Ripper: " + string(header.RipperName));

		stringstream ss;
		ss << "[NSF] Load Address: 0x" << std::hex << std::uppercase << std::setfill('0') << std::setw(4) << header.LoadAddress;
		MessageManager::Log(ss.str());
		ss = stringstream();
		ss << "[NSF] Init Address: 0x" << std::hex << std::uppercase << std::setfill('0') << std::setw(4) << header.InitAddress;
		MessageManager::Log(ss.str());
		ss = stringstream();
		ss << "[NSF] Play Address: 0x" << std::hex << std::uppercase << std::setfill('0') << std::setw(4) << header.PlayAddress;
		MessageManager::Log(ss.str());

		vector<string> chips;
		if(header.SoundChips & 0x01) {
			chips.push_back("VRC6");
		}
		if(header.SoundChips & 0x02) {
			chips.push_back("VRC7");
		}
		if(header.SoundChips & 0x04) {
			chips.push_back("FDS");
		}
		if(header.SoundChips & 0x08) {
			chips.push_back("MMC5");
		}
		if(header.SoundChips & 0x10) {
			chips.push_back("Namco 163");
		}
		if(header.SoundChips & 0x20) {
			chips.push_back("Sunsoft 5B");
		}
		if(chips.empty()) {
			chips.push_back("<none>");
		}

		ss = stringstream();
		for(size_t i = 0; i < chips.size(); i++) {
			if(i > 0) {
				ss << ", ";
			}
			ss << chips[i];
		}

		MessageManager::Log("[NSF] Sound Chips: " + ss.str());
		MessageManager::Log("[NSF] ROM size: " + std::to_string(romData.PrgRom.size() / 1024) + " KB");
	}

	void InitHeader(NsfHeader& header)
	{
		memset(&header, 0, sizeof(NsfHeader));
		for(int i = 0; i < 256; i++) {
			//Used by NSFE
			header.TrackLength[i] = -1;
			header.TrackFade[i] = -1;
		}
	}

public:
	RomData LoadRom(vector<uint8_t>& romFile)
	{
		RomData romData;
		NsfHeader &header = romData.NsfHeader;

		InitHeader(header);

		uint8_t* data = romFile.data();
		Read(data, header.Header, 5);
		Read(data, header.Version);
		Read(data, header.TotalSongs);
		Read(data, header.StartingSong);
		Read(data, header.LoadAddress);
		Read(data, header.InitAddress);
		Read(data, header.PlayAddress);
		Read(data, header.SongName, 32);
		Read(data, header.ArtistName, 32);
		Read(data, header.CopyrightHolder, 32);
		Read(data, header.PlaySpeedNtsc);
		Read(data, (char*)header.BankSetup, 8);
		Read(data, header.PlaySpeedPal);
		Read(data, header.Flags);
		Read(data, header.SoundChips);
		Read(data, (char*)header.Padding, 4);

		//Ensure strings are null terminated
		header.SongName[31] = 0;
		header.ArtistName[31] = 0;
		header.CopyrightHolder[31] = 0;

		if(header.PlaySpeedNtsc == 16666 || header.PlaySpeedNtsc == 16667) {
			header.PlaySpeedNtsc = 16639;
		}
		if(header.PlaySpeedPal == 20000) {
			header.PlaySpeedPal = 19997;
		}

		//Pad start of file to make the first block start at a multiple of 4k
		romData.PrgRom.insert(romData.PrgRom.end(), header.LoadAddress % 4096, 0);

		romData.PrgRom.insert(romData.PrgRom.end(), data, data + romFile.size() - 0x80);

		//Pad out the last block to be a multiple of 4k
		if(romData.PrgRom.size() % 4096 != 0) {
			romData.PrgRom.insert(romData.PrgRom.end(), 4096 - (romData.PrgRom.size() % 4096), 0);
		}

		InitializeFromHeader(romData);
		
		return romData;
	}
};