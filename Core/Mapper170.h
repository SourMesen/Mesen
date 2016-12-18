#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper170 : public BaseMapper
{
private:
	uint8_t _reg;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }
	virtual uint16_t RegisterStartAddress() override { return 0x7000; }
	virtual uint16_t RegisterEndAddress() override { return 0x7001; }
	virtual bool AllowRegisterRead() override { return true; }

	void InitMapper() override
	{
		_reg = 0;

		RemoveRegisterRange(0x7000, 0x7000, MemoryOperation::Read);
		RemoveRegisterRange(0x7001, 0x7001, MemoryOperation::Write);
		AddRegisterRange(0x6502, 0x6502, MemoryOperation::Write);
		AddRegisterRange(0x7777, 0x7777, MemoryOperation::Read);

		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void Reset(bool softReset) override
	{
		_reg = 0;
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_reg);
	}
	
	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		_reg = (value << 1) & 0x80;
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		return _reg | ((addr >> 8) & 0x7F);
	}
};