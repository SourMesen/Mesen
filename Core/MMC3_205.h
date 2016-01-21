#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_205 : public MMC3
{
private:
	uint8_t _selectedBlock = 0;

protected:
	virtual uint16_t RegisterStartAddress() { return 0x6000; }
	virtual uint16_t RegisterEndAddress() { return 0xFFFF; }

	virtual void StreamState(bool saving)
	{
		Stream<uint8_t>(_selectedBlock);
		MMC3::StreamState(saving);
	}

	virtual void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default)
	{
		if(_selectedBlock >= 2) {
			page &= 0x7F;
			page |= 0x100;
		}
		if(_selectedBlock == 1 || _selectedBlock == 3) {
			page |= 0x80;
		}

		MMC3::SelectCHRPage(slot, page, memoryType);
	}

	virtual void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom)
	{
		page &= _selectedBlock <= 1 ? 0x1F : 0x0F;
		page |= (_selectedBlock * 0x10);

		MMC3::SelectPRGPage(slot, page, memoryType);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr < 0x8000) {
			_selectedBlock = value & 0x03;
			UpdateState();
		} else {
			MMC3::WriteRegister(addr, value);
		}
	}
};