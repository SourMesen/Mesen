#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_238 : public MMC3
{
private:
	const uint8_t _securityLut[4] = { 0x00, 0x02, 0x02, 0x03 };
	uint8_t _exReg;

protected:
	virtual bool AllowRegisterRead() { return true; }

	void InitMapper()
	{
		MMC3::InitMapper();
		_exReg = 0;
		AddRegisterRange(0x4020, 0x7FFF, MemoryOperation::Any);
		RemoveRegisterRange(0x8000, 0xFFFF, MemoryOperation::Read);
	}

	void StreamState(bool saving)
	{
		MMC3::StreamState(saving);
		Stream(_exReg);
	}

	uint8_t ReadRegister(uint16_t addr)
	{
		return _exReg;
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr < 0x8000) {
			_exReg = _securityLut[value & 0x03];
		} else {
			MMC3::WriteRegister(addr, value);
		}
	}
};