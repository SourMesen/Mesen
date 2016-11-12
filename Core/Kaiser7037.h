#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Kaiser7037 : public BaseMapper
{
private:
	uint8_t _currentRegister;
	uint8_t _regs[8];

protected:
	uint32_t GetWorkRamPageSize() { return 0x1000; }
	uint16_t GetPRGPageSize() { return 0x1000; }
	uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper() override
	{
		memset(_regs, 0, sizeof(_regs));
		_currentRegister = 0;

		SelectCHRPage(0, 0);
		RemoveRegisterRange(0xA000, 0xBFFF);

		UpdateState();
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		ArrayInfo<uint8_t> regs{ _regs, 8 };
		Stream(_currentRegister, regs);

		if(!saving) {
			UpdateState();
		}
	}

	void UpdateState()
	{
		SetCpuMemoryMapping(0x6000, 0x6FFF, 0, PrgMemoryType::WorkRam);
		SetCpuMemoryMapping(0x7000, 0x7FFF, 15, PrgMemoryType::PrgRom);
		SelectPrgPage2x(0, _regs[6] << 1);
		SelectPRGPage(2, -4);
		SelectPRGPage(3, 1, PrgMemoryType::WorkRam);
		SelectPrgPage2x(2, _regs[7] << 1);
		SelectPrgPage2x(3, -2);
		SetNametables(_regs[2] & 1, _regs[4] & 1, _regs[3] & 1, _regs[5] & 1);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr & 0xE001) {
			case 0x8000: _currentRegister = value & 0x07; break;
			case 0x8001: _regs[_currentRegister] = value; UpdateState(); break;
		}
	}
};