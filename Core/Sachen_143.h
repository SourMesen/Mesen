#pragma once
#include "NROM.h"

class Sachen_143 : public NROM
{
protected:
	uint16_t RegisterStartAddress() { return 0x4100; }
	uint16_t RegisterEndAddress() { return 0x5FFF; }
	bool AllowRegisterRead() { return true; }
	
	uint8_t ReadRegister(uint16_t addr)
	{
		return (~addr & 0x3F) | 0x40;
	}
};