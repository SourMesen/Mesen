#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper120 : public BaseMapper
{
private:
	uint8_t _prgReg;
protected:
	virtual uint16_t RegisterStartAddress() { return 0x41FF; }
	virtual uint16_t RegisterEndAddress() { return 0x41FF; }
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		_prgReg = 0;
		UpdatePrg();
		SelectPrgPage4x(0, 8);
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(_prgReg);
		if(!saving) {
			UpdatePrg();
		}
	}

	void UpdatePrg()
	{
		SetCpuMemoryMapping(0x6000, 0x7FFF, _prgReg, PrgMemoryType::PrgRom);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		_prgReg = value;
		UpdatePrg();
	}
};
