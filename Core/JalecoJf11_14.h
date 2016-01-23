#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class JalecoJf11_14 : public BaseMapper
{
protected:
	virtual uint16_t RegisterStartAddress() { return 0x6000; }
	virtual uint16_t RegisterEndAddress() { return 0x7FFF; }

	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		SelectPRGPage(0, (value >> 4) & 0x03);
		SelectCHRPage(0, value & 0x0F);
	}
};