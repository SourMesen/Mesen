#pragma once
#include "stdafx.h"
#include "Sachen74LS374N.h"

class Sachen74LS374NB : public BaseMapper
{
private:
	uint8_t _counter;
	uint8_t _currentRegister;
	uint8_t _regs[8];

protected:
	uint16_t RegisterStartAddress() { return 0x4100; }
	uint16_t RegisterEndAddress() { return 0x7FFF; }
	uint16_t GetPRGPageSize() { return 0x8000; }
	uint16_t GetCHRPageSize() { return 0x2000; }
	bool AllowRegisterRead() { return true; }

	void InitMapper()
	{
		_counter = 0;
		_currentRegister = 0;
		memset(_regs, 0, sizeof(_regs));

		SelectPRGPage(0, 0);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		ArrayInfo<uint8_t> regs{ _regs, 8 };
		Stream(_currentRegister, regs, _counter);
	}

	void Reset(bool softReset)
	{
		if(softReset) {
			_counter++;
		}
	}

	uint8_t ReadRegister(uint16_t addr) 
	{
		switch(addr & 0xC101) {
			case 0x4000: return (~_currentRegister) ^ (_counter & 1);
		}

		return 0;
	}

	void UpdateState()
	{
		uint8_t chrPage = ((_regs[2] & 0x01) << 3) | ((_regs[4] & 0x01) << 2) | (_regs[6] & 0x03);
		SelectCHRPage(0, chrPage);
		if(_currentRegister == 2) {
			SelectPRGPage(0, _regs[2] & 0x01);
		} else {
			SelectPRGPage(0, _regs[5] & 0x07);
		}

		switch((_regs[7] >> 1) & 0x02) {
			case 0: SetMirroringType(MirroringType::Horizontal); break;
			case 1: SetMirroringType(MirroringType::Vertical); break;
			case 2: SetNametables(0, 1, 1, 1); break;
			case 3: SetMirroringType(MirroringType::ScreenAOnly); break;
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xC101) {
			case 0x4100: _currentRegister = value & 0x07; break;
			case 0x4101: _regs[_currentRegister] = value; UpdateState(); break;
		}
	}
};
