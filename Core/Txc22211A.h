#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Txc22211A : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	virtual uint16_t RegisterStartAddress() { return 0x8000; }
	virtual uint16_t RegisterEndAddress() { return 0xFFFF; }
	virtual bool AllowRegisterRead() { return true; }

	uint8_t _regs[4];

	void InitMapper()
	{
		AddRegisterRange(0x4100, 0x4103, MemoryOperation::Any);
		RemoveRegisterRange(0x8000, 0xFFFF, MemoryOperation::Read);

		memset(_regs, 0, sizeof(_regs));

		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(_regs[0], _regs[1], _regs[2], _regs[3]);
	}

	virtual uint8_t ReadRegister(uint16_t addr) 
	{
		return (_regs[1] ^ _regs[2]) | 0x40;
	}

	virtual void UpdateState(uint8_t value)
	{
		SelectPRGPage(0, _regs[2] >> 2);
		SelectCHRPage(0, _regs[2]);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr < 0x8000) {
			_regs[addr & 0x03] = value;
		} else {
			UpdateState(value);
		}
		
	}
};