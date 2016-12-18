#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Nina01 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override { return 0x1000; }
	virtual uint16_t RegisterStartAddress() override { return 0x7FFD; }
	virtual uint16_t RegisterEndAddress() override { return 0x7FFF; }

	void InitMapper() override
	{
		SelectPRGPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr) {
			case 0x7FFD: SelectPRGPage(0, value & 0x01); break;
			case 0x7FFE: SelectCHRPage(0, value & 0x0F); break;
			case 0x7FFF: SelectCHRPage(1, value & 0x0F); break;
		}

		WritePrgRam(addr, value);
	}
};