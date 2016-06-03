#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_49 : public MMC3
{
private:
	uint8_t _selectedBlock = 0;
	uint8_t _prgReg = 0;
	uint8_t _prgMode = 0;

protected:
	virtual uint16_t RegisterStartAddress() { return 0x6000; }
	virtual uint16_t RegisterEndAddress() { return 0xFFFF; }

	virtual void StreamState(bool saving)
	{
		MMC3::StreamState(saving);
		Stream(_selectedBlock, _prgReg, _prgMode);
	}

	virtual void Reset(bool softReset)
	{
		_prgReg = 0;
		_prgMode = false;
		_selectedBlock = 0;
		UpdateState();
	}

	virtual void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default)
	{
		page &= 0x7F;
		page |= 0x80 * _selectedBlock;
		MMC3::SelectCHRPage(slot, page, memoryType);
	}

	virtual void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom)
	{
		if(_prgMode) {
			page &= 0x0F;
			page |= 0x10 * _selectedBlock;
		} else {
			page = _prgReg * 4 + slot;
		}
		MMC3::SelectPRGPage(slot, page, memoryType);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr < 0x8000) {
			if(CanWriteToWorkRam()) {
				_selectedBlock = (value >> 6) & 0x03;
				_prgReg = (value >> 4) & 0x03;
				_prgMode = value & 0x01;
				UpdateState();
			}
		} else {
			MMC3::WriteRegister(addr, value);
		}
	}
};