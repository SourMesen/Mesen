#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Sunsoft93 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		SelectPRGPage(1, -1);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		SelectPRGPage(0, (value >> 4) & 0x07);
		if((value & 0x01) == 0x01) {
			SelectCHRPage(0, 0);
		} else {
			RemovePpuMemoryMapping(0x0000, 0x1FFF);
		}
	}
};