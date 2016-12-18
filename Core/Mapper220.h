#pragma once
#include "BaseMapper.h"

//Only seems to be for Gyruss (mapper 220) - other games marked as mapper 220 are not supported by any emulator that I could find (or remapped to another board type).
class Mapper220 : public BaseMapper
{
private:
	uint8_t _regs[8];

protected:
	uint16_t GetPRGPageSize() override { return 0x0800; }
	uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		memset(_regs, 0, sizeof(_regs));
		SelectCHRPage(0, 0);
		UpdateState();
	}
	
	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		
		ArrayInfo<uint8_t> regs{ _regs, 8 };
		Stream(regs);
		
		if(!saving) {
			UpdateState();
		}
	}
	
	void UpdateState()
	{
		const int regOrder[8] = { 4, 5, 6, 7, 0, 1, 2, 3 };
		for(int i = 0; i < 8; i++) {
			SetCpuMemoryMapping(0x6000 + i * 0x800, 0x67FF + i * 0x800, _regs[regOrder[i]], PrgMemoryType::PrgRom);
		}

		SelectPrgPage4x(1, 0x34);
		SelectPrgPage4x(2, 0x38);
		SelectPrgPage4x(3, 0x3C);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr < 0xA000) {
			SetMirroringType(value & 0x01 ? MirroringType::Vertical : MirroringType::Horizontal);
		} else if(addr >= 0xB000 && addr < 0xF000) {
			uint32_t regIndex = ((addr - 0xB000) >> 11) | ((addr >> 1) & 0x01);
			if(addr & 0x01) {
				_regs[regIndex] = (_regs[regIndex] & 0x0F) | (value << 4);
			} else {
				_regs[regIndex] = (_regs[regIndex] & 0xF0) | (value & 0x0F);
			}
			UpdateState();
		}
	}
};