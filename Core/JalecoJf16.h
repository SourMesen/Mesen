#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class JalecoJf16 : public BaseMapper
{
private:
	bool _isIremHolyDiver;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	virtual bool HasBusConflicts() { return true; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
		SelectPRGPage(1, -1);

		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		SelectPRGPage(0, value & 0x07);
		SelectCHRPage(0, (value >> 4) & 0x0F);
		if(_isIremHolyDiver) {
			SetMirroringType(value & 0x08 ? MirroringType::Vertical : MirroringType::Horizontal);
		} else {
			SetMirroringType(value & 0x08 ? MirroringType::ScreenBOnly : MirroringType::ScreenAOnly);
		}
	}

public:
	JalecoJf16(bool isIremHolyDiver) : _isIremHolyDiver(isIremHolyDiver)
	{
	}
};