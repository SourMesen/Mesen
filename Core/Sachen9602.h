#pragma once
#include "stdafx.h"
#include "MMC3.h"

class Sachen9602 : public MMC3
{
private:
	uint8_t _regs[2];

protected:
	bool ForceChrBattery() override { return true; }
	uint32_t GetChrRamSize() override { return 0x8000; }

	void InitMapper() override
	{
		_regs[0] = _regs[1] = 0;
		MMC3::InitMapper();
	}

	void StreamState(bool saving) override
	{
		MMC3::StreamState(saving);
		Stream(_regs[0], _regs[1]);
	}

	void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom) override
	{
		MMC3::SelectPRGPage(slot, (page & 0x3F) | (_regs[1] << 6));
		MMC3::SelectPRGPage(_prgMode ? 0 : 2, 0x3E);
		MMC3::SelectPRGPage(3, 0x3F);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr & 0xE001) {
			case 0x8000:
				_regs[0] = value;
				break;

			case 0x8001:
				if((_regs[0] & 0x07) < 6) {
					_regs[1] = value >> 6;
					value &= 0x1F;
					UpdatePrgMapping();
				}
				break;
		}
		MMC3::WriteRegister(addr, value);
	}
};