#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Sunsoft89 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() override { return 0x4000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		SelectPRGPage(1, -1);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		SelectPRGPage(0, (value >> 4) & 0x07);
		SelectCHRPage(0, value & 0x07 | ((value & 0x80) >> 4));
		SetMirroringType((value & 0x08) == 0x08 ? MirroringType::ScreenBOnly : MirroringType::ScreenAOnly);
	}
};