#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class JalecoJf16 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() override { return 0x4000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }
	virtual bool HasBusConflicts() override { return true; }

	void InitMapper() override
	{
		SelectPRGPage(0, GetPowerOnByte());
		SelectPRGPage(1, -1);

		SelectCHRPage(0, GetPowerOnByte());
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		SelectPRGPage(0, value & 0x07);
		SelectCHRPage(0, (value >> 4) & 0x0F);
		if(_subMapperID == 3) {
			//078: 3 Holy Diver
			SetMirroringType(value & 0x08 ? MirroringType::Vertical : MirroringType::Horizontal);
		} else {
			SetMirroringType(value & 0x08 ? MirroringType::ScreenBOnly : MirroringType::ScreenAOnly);
		}
	}
};