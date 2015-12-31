#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Nina01 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x1000; }
	virtual uint16_t RegisterStartAddress() { return 0x7FFD; }
	virtual uint16_t RegisterEndAddress() { return 0x7FFF; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr) {
			case 0x7FFD: SelectPRGPage(0, value & 0x01); break;
			case 0x7FFE: SelectCHRPage(0, value & 0x0F); break;
			case 0x7FFF: SelectCHRPage(1, value & 0x0F); break;
		}

		WritePrgRam(addr, value);
	}
};