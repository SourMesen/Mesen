#pragma once

#include "stdafx.h"
#include "MMC3.h"

class MMC3_187: public MMC3
{
private:
	uint8_t _exRegs[2];

protected:
	virtual bool AllowRegisterRead() { return true; }

	virtual void InitMapper()
	{
		MMC3::InitMapper();

		memset(_exRegs, 0, sizeof(_exRegs));
		AddRegisterRange(0x5000, 0x5FFF, MemoryOperation::Any);
		AddRegisterRange(0x6000, 0x6FFF, MemoryOperation::Write);
		RemoveRegisterRange(0x8000, 0xFFFF, MemoryOperation::Read);
	}

	virtual void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default)
	{
		if((_chrMode && slot >= 4) || (!_chrMode && slot < 4)) {
			page |= 0x100;
		}
		BaseMapper::SelectCHRPage(slot, page);
	}

	virtual void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom)
	{
		if(!(_exRegs[0] & 0x80)) {
			BaseMapper::SelectPRGPage(slot, page & 0x3F, memoryType);
		} else {
			uint16_t page = _exRegs[0] & 0x1F;

			if(_exRegs[0] & 0x20) {
				if(_exRegs[0] & 0x40) {
					page &= 0xFC;
					BaseMapper::SelectPRGPage(0, page);
					BaseMapper::SelectPRGPage(1, page+1);
					BaseMapper::SelectPRGPage(2, page+2);
					BaseMapper::SelectPRGPage(3, page+3);
				} else {
					page &= 0xFE;
					BaseMapper::SelectPRGPage(0, (page << 1));
					BaseMapper::SelectPRGPage(1, (page << 1) + 1);
					BaseMapper::SelectPRGPage(2, (page << 1) + 2);
					BaseMapper::SelectPRGPage(3, (page << 1) + 3);
				}
			} else {
				BaseMapper::SelectPRGPage(0, (page << 1));
				BaseMapper::SelectPRGPage(1, (page << 1) + 1);
				BaseMapper::SelectPRGPage(2, (page << 1));
				BaseMapper::SelectPRGPage(3, (page << 1) + 1);
			}
		}
	}

	virtual uint8_t ReadRegister(uint16_t addr)
	{
		uint8_t security[4] = { 0x83,0x83,0x42,0x00 };
		return security[_exRegs[1] & 0x03];
	}

	virtual void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr < 0x8000) {
			if(addr == 0x5000 || addr == 0x6000) {
				_exRegs[0] = value;
				MMC3::UpdatePrgMapping();
			}
		} else {
			if(addr == 0x8000) {
				_exRegs[1] = 1;
				MMC3::WriteRegister(addr, value);
			} else if(addr == 0x8001) {
				if(_exRegs[1] == 1) {
					MMC3::WriteRegister(addr, value);
				}
			} else {
				MMC3::WriteRegister(addr, value);
			}
		}
	}

	virtual void StreamState(bool saving)
	{
		MMC3::StreamState(saving);
		Stream(_exRegs[0], _exRegs[1]);
	}
};