#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Sachen_149 : public BaseMapper
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
		SelectCHRPage(0, (value >> 7) & 0x01);
	}
};