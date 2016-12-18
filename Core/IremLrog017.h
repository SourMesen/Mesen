#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class IremLrog017 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override { return 0x0800; }
	virtual uint32_t GetChrRamSize() override { return 0x1800; }
	virtual uint16_t GetChrRamPageSize() override { return 0x0800; }
	virtual bool HasBusConflicts() override { return true; }

	void InitMapper() override
	{
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
		SetMirroringType(MirroringType::FourScreens);

		SelectCHRPage(1, 0, ChrMemoryType::ChrRam);
		SelectCHRPage(2, 1, ChrMemoryType::ChrRam);
		SelectCHRPage(3, 2, ChrMemoryType::ChrRam);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		SelectPRGPage(0, value & 0x0F);
		SelectCHRPage(0, (value >> 4) & 0x0F);
	}
};