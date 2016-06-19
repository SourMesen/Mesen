#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Sachen74LS374N : public BaseMapper
{
private:
	uint8_t _currentRegister;
	uint8_t _regs[8];

protected:
	uint16_t RegisterStartAddress() { return 0x4100; }
	uint16_t RegisterEndAddress() { return 0x7FFF; }
	uint16_t GetPRGPageSize() { return 0x8000; }
	uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		_currentRegister = 0;
		memset(_regs, 0, sizeof(_regs));
		SelectPRGPage(0, 0);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);

		ArrayInfo<uint8_t> regs{ _regs, 8 };
		Stream(_currentRegister, regs);
	}

	virtual void UpdateState()
	{
		uint8_t chrPage = ((_regs[2] & 0x01) << 3) | ((_regs[6] & 0x03) << 1) | (_regs[4] & 0x01);
		SelectCHRPage(0, chrPage);
		SelectPRGPage(0, _regs[5] & 0x01);

		SetMirroringType(_regs[7] & 0x01 ? MirroringType::Vertical : MirroringType::Horizontal);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xC101) {
			case 0x4100: _currentRegister = value & 0x07; break;
			case 0x4101: 
				_regs[_currentRegister] = value; 
				if(_currentRegister == 0) {
					SelectCHRPage(0, 3);
					SelectPRGPage(0, 0);
				} else {
					UpdateState();
				}
				break;
		}
	}
};
