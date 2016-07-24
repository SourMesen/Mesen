#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper212 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	virtual bool AllowRegisterRead() { return true; }
	
	void InitMapper()
	{
		WriteRegister(0x8000, 0);
	}

	uint8_t ReadRegister(uint16_t addr)
	{
		uint8_t value = InternalReadRam(addr);

		if((addr & 0xE010) == 0x6000) {
			value |= 0x80;
		}

		return value;
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr & 0x4000) {
			SelectPrgPage2x(0, addr & 0x06);
		} else {
			SelectPRGPage(0, addr & 0x07);
			SelectPRGPage(1, addr & 0x07);
		}
		SelectCHRPage(0, addr & 0x07);
		SetMirroringType(addr & 0x08 ? MirroringType::Horizontal : MirroringType::Vertical);
	}
};