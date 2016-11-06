#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class DreamTech01 : public BaseMapper
{
protected:
	uint16_t GetPRGPageSize() { return 0x4000; }
	uint16_t GetCHRPageSize() { return 0x2000; }
	uint16_t RegisterStartAddress() { return 0x5020; }
	uint16_t RegisterEndAddress() { return 0x5020; }

	void InitMapper() override
	{
		SelectPRGPage(0, 0);
		SelectPRGPage(1, 8);
		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		SelectPRGPage(0, value & 0x07);
	}
};