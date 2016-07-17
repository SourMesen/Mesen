#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Kaiser7058 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x1000; }
	virtual uint16_t RegisterStartAddress() { return 0xF000; }
	virtual uint16_t RegisterEndAddress() { return 0xFFFF; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xF080) {
			case 0xF000: SelectCHRPage(0, value); break;
			case 0xF080: SelectCHRPage(1, value); break;
		}
	}
};