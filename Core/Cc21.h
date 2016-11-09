#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Cc21 : public BaseMapper
{
protected:
	uint16_t GetPRGPageSize() { return 0x8000; }
	uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper() override
	{
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		SelectCHRPage(0, addr & 0xFF);
		SetMirroringType(addr & 0x02 ? MirroringType::ScreenBOnly : MirroringType::ScreenAOnly);
	}
};