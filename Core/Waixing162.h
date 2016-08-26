#pragma once
#include "BaseMapper.h"

class Waixing162 : public BaseMapper
{
private:
	uint8_t _regs[4];

protected:
	uint16_t GetPRGPageSize() { return 0x8000; }
	uint16_t GetCHRPageSize() { return 0x2000; }
	uint16_t RegisterStartAddress() { return 0x5000; }
	uint16_t RegisterEndAddress() { return 0x5FFF; }

	void InitMapper()
	{
		_regs[0] = 3;
		_regs[1] = 0;
		_regs[2] = 0;
		_regs[3] = 7;

		SelectCHRPage(0, 0);
		UpdateState();
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		ArrayInfo<uint8_t> regs{ _regs, 4 };
		Stream(regs);
	}

	void UpdateState()
	{
		switch(_regs[3] & 0x5) {
			case 0: SelectPRGPage(0, (_regs[0] & 0x0C) | (_regs[1] & 0x02) | ((_regs[2] & 0x0F) << 4)); break;
			case 1: SelectPRGPage(0, (_regs[0] & 0x0C) | (_regs[2] & 0x0F) << 4); break;
			case 4: SelectPRGPage(0, (_regs[0] & 0x0E) | ((_regs[1] >> 1) & 0x01) | ((_regs[2] & 0x0F) << 4)); break;
			case 5: SelectPRGPage(0, (_regs[0] & 0x0F) | ((_regs[2] & 0x0F) << 4)); break;
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		_regs[(addr >> 8) & 0x03] = value;
		UpdateState();
	}
};