#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Sachen_148 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		SelectPRGPage(0, (value >> 3) & 0x01);
		SelectCHRPage(0, value & 0x07);
	}
};