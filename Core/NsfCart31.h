#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class NsfCart31 : public BaseMapper
{
protected:
	virtual uint16_t RegisterStartAddress() { return 0x5000; }
	virtual uint16_t RegisterEndAddress() { return 0x5FFF; }

	virtual uint16_t GetPRGPageSize() { return 0x1000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		WriteRegister(0x5FFF, 0xFF);

		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		SelectPRGPage(addr & 0x07, value);
	}
};
