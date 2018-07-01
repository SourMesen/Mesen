#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Sachen_136 : public BaseMapper
{
private:
	uint8_t _chrReg;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }
	virtual uint16_t RegisterStartAddress() override { return 0x4100; }
	virtual uint16_t RegisterEndAddress() override { return 0xFFFF; }
	virtual bool AllowRegisterRead() override { return true; }

	void InitMapper() override
	{
		_chrReg = 0;
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);

		RemoveRegisterRange(0x4101, 0xFFFF, MemoryOperation::Read);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_chrReg);
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		return (_chrReg & 0x3F) | (_console->GetMemoryManager()->GetOpenBus() & 0xC0);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if((addr & 0x0103) == 0x0102) {
			_chrReg = value + 3;
			SelectCHRPage(0, _chrReg & 0x03);
		}
	}
};