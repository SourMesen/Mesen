#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper240 : public BaseMapper
{
protected:
	virtual uint16_t RegisterStartAddress() override { return 0x4020; }
	virtual uint16_t RegisterEndAddress() override { return 0x5FFF; }
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		SelectPRGPage(0, (value >> 4) & 0x0F);
		SelectCHRPage(0, value & 0x0F);
	}
};