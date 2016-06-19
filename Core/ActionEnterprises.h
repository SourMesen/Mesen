#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class ActionEnterprises : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		WriteRegister(0x8000, 0);
	}

	virtual void Reset(bool softReset)
	{
		WriteRegister(0x8000, 0);
	}


	void WriteRegister(uint16_t addr, uint8_t value)
	{
		uint8_t chipSelect = (addr >> 11) & 0x03;

		if(chipSelect == 3) {
			chipSelect = 2;
		}

		uint8_t prgPage = ((addr >> 6) & 0x1F) | (chipSelect << 5);
		if(addr & 0x20) {
			SelectPRGPage(0, prgPage);
			SelectPRGPage(1, prgPage);
		} else {
			SelectPRGPage(0, prgPage & 0xFE);
			SelectPRGPage(1, (prgPage & 0xFE) + 1);
		}

		SelectCHRPage(0, ((addr & 0x0F) << 2) | (value & 0x03));

		SetMirroringType(addr & 0x2000 ? MirroringType::Horizontal : MirroringType::Vertical);
	}
};
