#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper201 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		SelectCHRPage(0, addr & 0xFF);
		SelectPRGPage(0, addr & 0xFF);
	}
};