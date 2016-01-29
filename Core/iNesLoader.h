#pragma once
#include "stdafx.h"

class iNesLoader
{
public:
	RomData LoadRom(vector<uint8_t>& romFile)
	{
		RomData romData;

		uint8_t* buffer = romFile.data();
		NESHeader header;
		memcpy((char*)&header, buffer, sizeof(NESHeader));
		buffer += sizeof(NESHeader);

		header.SanitizeHeader(romFile.size());

		romData.MapperID = header.GetMapperID();
		romData.SubMapperID = header.GetSubMapper();
		romData.MirroringType = header.GetMirroringType();
		romData.HasBattery = header.HasBattery();
		romData.IsPalRom = header.IsPalRom();

		romData.PrgRom.insert(romData.PrgRom.end(), buffer, buffer + header.GetPrgSize());
		buffer += header.GetPrgSize();
		romData.ChrRom.insert(romData.ChrRom.end(), buffer, buffer + header.GetChrSize());

		return romData;
	}
};