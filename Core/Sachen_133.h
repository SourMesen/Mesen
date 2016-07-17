#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Sachen_133 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	virtual uint16_t RegisterStartAddress() { return 0x4100; }
	virtual uint16_t RegisterEndAddress() { return 0xFFFF; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if((addr & 0x6100) == 0x4100) {
			SelectPRGPage(0, (value >> 2) & 0x01);
			SelectCHRPage(0, value & 0x03);
		}
	}
};