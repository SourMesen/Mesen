#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper200 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
		SelectPRGPage(1, 0);
		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		uint8_t bank = addr & 0x07;
		SelectPRGPage(0, bank);
		SelectPRGPage(1, bank);
		SelectCHRPage(0, bank);

		SetMirroringType(addr & 0x08 ? MirroringType::Vertical : MirroringType::Horizontal);
	}
};
