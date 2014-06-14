#pragma once

#include "stdafx.h"

struct NESHeader
{
	char NES[4];
	uint8_t ROMCount;
	uint8_t VROMCount;
	uint8_t Flags1;
	uint8_t Flags2;
	uint8_t RAMCount;
	uint8_t CartType;
	uint8_t Reserved[6];

	uint8_t GetMapperID()
	{
		return (Flags2 & 0xF0) | (Flags1 >> 4);
	}
};

typedef vector<uint8_t> MemoryBank;
class ROMLoader
{
	private:
		const int ROMBankSize = 0x4000;
		const int VROMBankSize = 0x2000;
		NESHeader _header;
		vector<MemoryBank> _romBanks;
		vector<MemoryBank> _vromBanks;

	public:
		ROMLoader(const char* filename)
		{
			_romBanks.clear();
			_vromBanks.clear();

			ifstream romFile(filename, ios::in | ios::binary);

			if(!romFile) {
				return;
			}

			romFile.read((char*)&_header, sizeof(NESHeader));

			uint8_t *buffer = new uint8_t[max(ROMBankSize, VROMBankSize)];
			for(int i = 0; i < _header.ROMCount; i++) {
				romFile.read((char*)buffer, ROMBankSize);
				_romBanks.push_back(MemoryBank(buffer, buffer + ROMBankSize));
			}

			for(int i = 0; i < _header.VROMCount; i++) {
				romFile.read((char*)buffer, VROMBankSize);
				_vromBanks.push_back(MemoryBank(buffer, buffer + VROMBankSize));
			}
			
			delete[] buffer;

			romFile.close();
		}

		vector<MemoryBank> GetROMBanks()
		{
			return _romBanks;
		}

		vector<MemoryBank> GetVROMBanks()
		{
			return _vromBanks;
		}

		NESHeader GetHeader()
		{
			return _header;
		}
};

