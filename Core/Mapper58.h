#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper58 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
		SelectPRGPage(1, 1);
		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		uint8_t prgBank = addr & 0x07;
		if(addr & 0x40) {
			SelectPRGPage(0, prgBank);
			SelectPRGPage(1, prgBank);
		} else {
			SelectPRGPage(0, prgBank & 0xFE);
			SelectPRGPage(1, (prgBank & 0xFE) + 1);
		}
		SelectCHRPage(0, (addr >> 3) & 0x07);

		SetMirroringType(addr & 0x80 ? MirroringType::Horizontal : MirroringType::Vertical);
	}
};
