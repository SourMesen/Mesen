#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_250 : public MMC3
{
protected:
	virtual void WriteRegister(uint16_t addr, uint8_t value)
	{
		MMC3::WriteRegister((addr & 0xE000) | ((addr & 0x0400) >> 10), addr & 0xFF);
	}
};