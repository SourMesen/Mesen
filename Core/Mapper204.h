#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper204 : public BaseMapper
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
		uint8_t bitMask = addr & 0x06;
		uint8_t page = bitMask + ((bitMask == 0x06) ? 0 : (addr & 0x01));
		SelectPRGPage(0, page);
		SelectPRGPage(1, bitMask + ((bitMask == 0x06) ? 1 : (addr & 0x01)));
		SelectCHRPage(0, page);
		SetMirroringType(addr & 0x10 ? MirroringType::Horizontal : MirroringType::Vertical);
	}
};