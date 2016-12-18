#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Sachen_145 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }
	virtual uint16_t RegisterStartAddress() override { return 0x4100; }
	virtual uint16_t RegisterEndAddress() override { return 0x7FFF; }

	void InitMapper() override
	{
		SelectPRGPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if((addr & 0x4100) == 0x4100) {
			SelectCHRPage(0, (value >> 7) & 0x01);
		}
	}
};