#pragma once
#include "stdafx.h"
#include "Txc22211A.h"

class Txc22211C : public Txc22211A
{
protected:
	virtual uint8_t ReadRegister(uint16_t addr) override
	{
		return (_regs[1] ^ _regs[2]) | 0x41;
	}
};