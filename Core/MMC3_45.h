#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_45 : public MMC3
{
private:
	uint8_t _regIndex;
	uint8_t _reg[4];

protected:
	virtual uint16_t RegisterStartAddress() { return 0x8000; }
	virtual uint16_t RegisterEndAddress() { return 0xFFFF; }

	virtual void StreamState(bool saving)
	{
		MMC3::StreamState(saving);
		Stream(_regIndex, ArrayInfo<uint8_t>{_reg, 4});

		if(_reg[3] & 0x40) {
			RemoveRegisterRange(0x6000, 0x7FFF);
		}
	}

	virtual void Reset(bool softReset)
	{
		AddRegisterRange(0x6000, 0x7FFF);
		_regIndex = 0;
		memset(_reg, 0, sizeof(_reg));
		_reg[2] = 0x0F;
		UpdateState();
	}

	virtual void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default)
	{
		page &= 0xFF >> (0x0F - (_reg[2] & 0x0F));
		page |= _reg[0] | ((_reg[2] & 0xF0) << 4);
		MMC3::SelectCHRPage(slot, page, memoryType);
	}

	virtual void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom)
	{
		page &= 0x3F ^ (_reg[3] & 0x3F);
		page |= _reg[1];
		MMC3::SelectPRGPage(slot, page, memoryType);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr < 0x8000) {
			if(!(_reg[3] & 0x40)) {
				_reg[_regIndex] = value;
				_regIndex = (_regIndex + 1) & 0x03;
			}
			
			if(_reg[3] & 0x40) {
				RemoveRegisterRange(0x6000, 0x7FFF);
			}
			UpdateState();
		} else {
			MMC3::WriteRegister(addr, value);
		}
	}
};