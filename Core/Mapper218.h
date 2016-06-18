#pragma once
#include "BaseMapper.h"

class Mapper218 : public BaseMapper
{
protected:
	uint16_t GetPRGPageSize() { return 0x8000; }
	uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);

		if(GetMirroringType() == MirroringType::FourScreens) {
			SetMirroringType(_nesHeader.Byte6 & 0x01 ? MirroringType::ScreenBOnly : MirroringType::ScreenAOnly);
		}
	}

	void SetDefaultNametables(uint8_t* nametableA, uint8_t* nametableB)
	{
		BaseMapper::SetDefaultNametables(nametableA, nametableB);

		uint16_t mask = 0;
		switch(GetMirroringType()) {
			case MirroringType::Vertical: mask = 0x400; break;
			case MirroringType::Horizontal: mask = 0x800; break;
			case MirroringType::ScreenAOnly: mask = 0x1000; break;
			case MirroringType::ScreenBOnly: mask = 0x2000; break;
		}

		for(int i = 0; i < 8; i++) {
			SetPpuMemoryMapping(i * 0x400, i * 0x400 + 0x3FF, (i * 0x400) & mask ? GetNametable(1) : GetNametable(0));
		}
	}
};