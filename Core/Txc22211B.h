#pragma once
#include "stdafx.h"
#include "Txc22211A.h"

class Txc22211B : public Txc22211A
{
protected:
	virtual void UpdateState(uint8_t value)
	{
		SelectPRGPage(0, _regs[2] >> 2);
		SelectCHRPage(0, (((value ^ _regs[2]) >> 3) & 0x02) | (((value ^ _regs[2]) >> 5) & 0x01));
	}
};