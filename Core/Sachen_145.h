#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Sachen_145 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	virtual uint16_t RegisterStartAddress() { return 0x4100; }
	virtual uint16_t RegisterEndAddress() { return 0x7FFF; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if((addr & 0x4100) == 0x4100) {
			SelectCHRPage(0, (value >> 7) & 0x01);
		}
	}
};