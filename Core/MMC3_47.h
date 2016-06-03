#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_47 : public MMC3
{
private:
	uint8_t _selectedBlock = 0;

protected:
	virtual uint16_t RegisterStartAddress() { return 0x6000; }
	virtual uint16_t RegisterEndAddress() { return 0xFFFF; }

	virtual void StreamState(bool saving)
	{
		MMC3::StreamState(saving);
		Stream(_selectedBlock);
	}

	virtual void Reset(bool softReset)
	{
		_selectedBlock = 0;
		UpdateState();
	}

	virtual void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default)
	{
		page &= 0x7F;
		if(_selectedBlock == 1) {
			page |= 0x80;
		}

		MMC3::SelectCHRPage(slot, page, memoryType);
	}

	virtual void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom)
	{
		page &= 0x0F;
		if(_selectedBlock == 1) {
			page |= 0x10;
		}

		MMC3::SelectPRGPage(slot, page, memoryType);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr < 0x8000) {
			if(CanWriteToWorkRam()) {
				_selectedBlock = value & 0x01;
				UpdateState();
			}
		} else {
			MMC3::WriteRegister(addr, value);
		}
	}
};