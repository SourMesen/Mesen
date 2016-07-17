#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Kaiser7022 : public BaseMapper
{
private:
	uint8_t _reg;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	virtual uint16_t RegisterStartAddress() { return 0x8000; }
	virtual uint16_t RegisterEndAddress() { return 0xFFFF; }
	virtual bool AllowRegisterRead() { return true; }

	void InitMapper()
	{
		_reg = 0;
		RemoveRegisterRange(0x8000, 0xFFFF, MemoryOperation::Read);
		AddRegisterRange(0xFFFC, 0xFFFC, MemoryOperation::Any);
		SelectPRGPage(0, 0);
	}

	void Reset(bool softReset)
	{
		_reg = 0;
		ReadRegister(0xFFFC);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(_reg);
	}

	uint8_t ReadRegister(uint16_t addr)
	{
		SelectCHRPage(0, _reg);
		SelectPRGPage(0, _reg);
		SelectPRGPage(1, _reg);
		
		return InternalReadRam(addr);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr) {
			case 0x8000: SetMirroringType(value & 0x04 ? MirroringType::Horizontal : MirroringType::Vertical); break;
			case 0xA000: _reg = value & 0x0F; break;
		}
	}
};