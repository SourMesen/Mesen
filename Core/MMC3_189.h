#pragma once

#include "stdafx.h"
#include "MMC3.h"

class MMC3_189 : public MMC3
{
private:
	uint8_t _prgReg = 0;

	virtual uint16_t RegisterStartAddress() { return 0x4120; }
	
	virtual void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr <= 0x4FFF) {
			_prgReg = value;
			UpdateState();
		} else {
			MMC3::WriteRegister(addr, value);
		}
	}

	virtual void UpdateState()
	{
		MMC3::UpdateState();

		//"$4120-7FFF:  [AAAA BBBB]"
		//" 'A' and 'B' bits of the $4120 reg seem to be effectively OR'd."
		uint8_t prgPage = (((_prgReg) | (_prgReg >> 4)) & 0x07) * 4;
		SelectPRGPage(0, prgPage);
		SelectPRGPage(1, prgPage+1);
		SelectPRGPage(2, prgPage+2);
		SelectPRGPage(3, prgPage+3);
	}

	virtual void StreamState(bool saving)
	{
		MMC3::StreamState(saving);
		Stream(_prgReg);
	}
};