#pragma once

#include "stdafx.h"
#include "MMC3.h"

class Mapper112 : public BaseMapper
{
private:
	uint8_t _currentReg;
	uint8_t _outerChrBank;
	uint8_t _registers[8];

protected:
	uint16_t RegisterStartAddress() override { return 0x8000; }
	uint16_t RegisterEndAddress() override { return 0xFFFF; }
	uint16_t GetPRGPageSize() override { return 0x2000; }
	uint16_t GetCHRPageSize() override { return 0x400; }

	void InitMapper() override
	{
		_currentReg = 0;
		_outerChrBank = 0;
		memset(_registers, 0, sizeof(_registers));

		SetMirroringType(MirroringType::Vertical);
		AddRegisterRange(0x4020, 0x5FFF, MemoryOperation::Write);

		SelectPRGPage(2, -2);
		SelectPRGPage(3, -1);
		UpdateState();
	}
	
	void StreamState(bool saving) override
	{
		ArrayInfo<uint8_t> registers { _registers, 8 };
		Stream(registers, _currentReg, _outerChrBank);
	}

	void UpdateState()
	{
		SelectPRGPage(0, _registers[0]);
		SelectPRGPage(1, _registers[1]);

		SelectChrPage2x(0, _registers[2]);
		SelectChrPage2x(1, _registers[3]);
		SelectCHRPage(4, _registers[4]);
		SelectCHRPage(5, _registers[5]);
		SelectCHRPage(6, _registers[6]);
		SelectCHRPage(7, _registers[7]);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr & 0xE001) {
			case 0x8000: _currentReg = value & 0x07; break;
			case 0xA000: _registers[_currentReg] = value; break;
			case 0xC000: _outerChrBank = value; break;
			case 0xE000: SetMirroringType(value & 0x01 ? MirroringType::Horizontal : MirroringType::Vertical); break;
		}
	
		UpdateState();
	}
};