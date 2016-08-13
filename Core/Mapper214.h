#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper214 : public BaseMapper
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
		SelectCHRPage(0, addr & 0x03);
		SelectPRGPage(0, (addr >> 2) & 0x03);
		SelectPRGPage(1, (addr >> 2) & 0x03);
	}
};