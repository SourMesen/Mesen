#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_44 : public MMC3
{
private:
	uint8_t _selectedBlock = 0;

protected:
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
		page &= _selectedBlock <= 5 ? 0x7F : 0xFF;
		page |= _selectedBlock * 0x80;

		MMC3::SelectCHRPage(slot, page, memoryType);
	}

	virtual void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom)
	{
		page &= _selectedBlock <= 5 ? 0x0F : 0x1F;
		page |= _selectedBlock * 0x10;

		MMC3::SelectPRGPage(slot, page, memoryType);
	}

	virtual void WriteRegister(uint16_t addr, uint8_t value)
	{
		if((addr & 0xE001) == 0xA001) {
			_selectedBlock = value & 0x07;
			if(_selectedBlock == 7) {
				_selectedBlock = 6;
			}
		}
		MMC3::WriteRegister(addr, value);
	}
};