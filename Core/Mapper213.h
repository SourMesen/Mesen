#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper213 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		WriteRegister(0x8000, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		SelectCHRPage(0, (addr >> 3) & 0x07);
		SelectPRGPage(0, (addr >> 1) & 0x03);
	}
};