#pragma once
#include "BaseMapper.h"

class Waixing178 : public BaseMapper
{
private:
	uint8_t _regs[4];

protected:
	uint16_t GetPRGPageSize() { return 0x4000; }
	uint16_t GetCHRPageSize() { return 0x2000; }
	uint16_t RegisterStartAddress() { return 0x4800; }
	uint16_t RegisterEndAddress() { return 0x4FFF; }
	uint32_t GetWorkRamSize() { return 0x8000; }

	void InitMapper()
	{
		memset(_regs, 0, sizeof(_regs));
		UpdateState();
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(_regs[0], _regs[1], _regs[2], _regs[3]);
		if(!saving) {
			UpdateState();
		}
	}

	void UpdateState()
	{
		uint16_t sbank = _regs[1] & 0x07;
		uint16_t bbank = _regs[2];
		if(_regs[0] & 0x02) {
			SelectPRGPage(0, (bbank << 3) | sbank);
			if(_regs[0] & 0x04) {
				SelectPRGPage(1, (bbank << 3) | 0x06 | (_regs[1] & 0x01));
			} else {
				SelectPRGPage(1, (bbank << 3) | 0x07);
			}
		} else {
			uint16_t bank = (bbank << 3) | sbank;
			if(_regs[0] & 0x04) {
				SelectPRGPage(0, bank);
				SelectPRGPage(1, bank);
			} else {
				SelectPrgPage2x(0, bank);
			}
		}

		SetCpuMemoryMapping(0x6000, 0x7FFF, _regs[3] & 0x03, PrgMemoryType::WorkRam, MemoryAccessType::ReadWrite);
		SetMirroringType(_regs[0] & 0x01 ? MirroringType::Horizontal : MirroringType::Vertical);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		_regs[addr & 0x03] = value;
		UpdateState();
	}
};