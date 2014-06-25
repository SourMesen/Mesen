#pragma once

#include "stdafx.h"

enum class MirroringType
{
	Horizontal,
	Vertical,
	ScreenAOnly,
	ScreenBOnly,
	FourScreens,
};

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

	bool HasBattery()
	{
		return (Flags1 & 0x02) == 0x02;
	}

	bool HasTrainer()
	{
		return (Flags1 & 0x04) == 0x04;
	}

	MirroringType GetMirroringType()
	{
		if(Flags1 & 0x08) {
			return MirroringType::FourScreens;
		} else {
			return Flags1 & 0x01 ? MirroringType::Vertical : MirroringType::Horizontal;
		}
	}
};

class ROMLoader
{
	private:
		NESHeader _header;
		wstring _filename;
		uint8_t* _prgRAM;
		uint8_t* _chrRAM;

	public:
		ROMLoader(wstring filename)
		{
			_filename = filename;

			ifstream romFile(filename, ios::in | ios::binary);

			if(!romFile) {
				throw std::exception("File could not be read");
			}

			romFile.read((char*)&_header, sizeof(NESHeader));

			uint8_t* prgBuffer = new uint8_t[0x4000 * _header.ROMCount];
			for(int i = 0; i < _header.ROMCount; i++) {
				romFile.read((char*)prgBuffer+i*0x4000, 0x4000);
			}
			_prgRAM = prgBuffer;

			uint8_t* chrBuffer = new uint8_t[0x2000 * _header.VROMCount];
			for(int i = 0; i < _header.VROMCount; i++) {
				romFile.read((char*)chrBuffer+i*0x2000, 0x2000);
			}
			_chrRAM = chrBuffer;

			romFile.close();
		}

		uint8_t* GetPRGRam()
		{
			return _prgRAM;
		}

		uint8_t* GetCHRRam()
		{
			return _chrRAM;
		}

		uint32_t GetPRGSize()
		{
			return _header.ROMCount * 0x4000;
		}

		uint32_t GetCHRSize()
		{
			return _header.VROMCount * 0x2000;
		}

		MirroringType GetMirroringType()
		{
			return _header.GetMirroringType();
		}

		uint8_t GetMapperID()
		{
			return _header.GetMapperID();
		}

		bool HasBattery()
		{
			return _header.HasBattery();
		}

		wstring GetFilename()
		{
			return _filename;
		}
};

