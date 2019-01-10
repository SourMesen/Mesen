#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

//Missing Flash rom support, and only tested via a test rom
class Cheapocabra : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }
	virtual uint16_t RegisterStartAddress() override { return 0x5000; }
	virtual uint16_t RegisterEndAddress() override { return 0x5FFF; }
	virtual uint32_t GetChrRamSize() override { return 0x8000; }

	void InitMapper() override
	{
		AddRegisterRange(0x7000, 0x7FFF, MemoryOperation::Write);
		WriteRegister(0x5000, GetPowerOnByte());
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		SelectPRGPage(0, value & 0x0F);
		SelectCHRPage(0, (value >> 4) & 0x01);
		if(value & 0x20) {
			SetNametables(4, 5, 6, 7);
		} else {
			SetNametables(0, 1, 2, 3);
		}
	}
};