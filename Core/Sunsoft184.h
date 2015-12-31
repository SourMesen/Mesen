#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Sunsoft184 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x1000; }
	virtual uint16_t RegisterStartAddress() { return 0x6000; }
	virtual uint16_t RegisterEndAddress() { return 0x7FFF; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		SelectCHRPage(0, value & 0x07);

		//"The most significant bit of H is always set in hardware."
		SelectCHRPage(1, 0x80 | ((value >> 4) & 0x07));
	}
};