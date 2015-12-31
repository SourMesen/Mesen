#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class BnRom : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		//"While the original BNROM board connects only 2 bits, it is recommended that emulators implement this as an 8-bit register allowing selection of up to 8 MB PRG ROM if present."
		SelectPRGPage(0, value);
	}
};