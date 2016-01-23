#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper61 : public BaseMapper
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
		uint8_t prgPage = ((addr & 0x0F) << 1) | ((addr >> 5) & 0x01);
		if(addr & 0x10) {
			SelectPRGPage(0, prgPage);
			SelectPRGPage(1, prgPage);
		} else {
			SelectPRGPage(0, prgPage & 0xFE);
			SelectPRGPage(1, (prgPage & 0xFE) + 1);
		}

		SetMirroringType(addr & 0x80 ? MirroringType::Horizontal : MirroringType::Vertical);
	}
};
