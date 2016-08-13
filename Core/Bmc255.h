#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Bmc255 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		WriteRegister(0x8000, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		uint8_t prgBit = (addr & 0x1000) ? 0 : 1;
		uint8_t bank = ((addr >> 8) & 0x40) | ((addr >> 6) & 0x3F);

		SelectPRGPage(0, bank & ~prgBit);
		SelectPRGPage(1, bank | prgBit);
		SelectCHRPage(0, ((addr >> 8) & 0x40) | (addr & 0x3F));
		SetMirroringType(addr & 0x2000 ? MirroringType::Horizontal : MirroringType::Vertical);
	}
};