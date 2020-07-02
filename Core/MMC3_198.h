#pragma once

#include "stdafx.h"
#include "MMC3.h"

//Most likely incorrect/incomplete, but works (with minor glitches) with the 2 games marked as mapper 198 that I am aware of.
//Game 1: 吞食天地2  (CHR RAM, but uses chr banking?, has save ram at 6000-7FFF)
//        吞食天地2  (English name is doae2) doae2 uses ram 5000-7FFF (Add Chinese character process program in 5000-5FFF)
//        This rom has two version. One's size is 640KB(The version published by unknown in Hongkang)
//        Another rom size is 1024KB(The version published by WaiXingKeji(Chinese named 外星科技) in China Mainland)
//Game 2: Cheng Ji Si Han (ES-1110) (Ch)  (CHR RAM, work ram mirrored from 5000-7FFF?, doesn't use chr banking)
//These games may actually use different mappers.
class MMC3_198 : public MMC3
{
private:
	uint8_t _exRegs[4];

protected:
	uint32_t GetWorkRamSize() override { return 0x1000; }
	uint32_t GetWorkRamPageSize() override { return 0x1000; }
	uint16_t GetChrRamPageSize() override { return 0x400; }
	bool ForceWorkRamSize() override { return true; }

	void InitMapper() override
	{
		_exRegs[0] = 0;
		_exRegs[1] = 1;
		_exRegs[2] = GetPRGPageCount() - 2;
		_exRegs[3] = GetPRGPageCount() - 1;

		//Set 4kb of work ram at $5000, mirrored
		SetCpuMemoryMapping(0x5000, 0x7FFF, 0, PrgMemoryType::WorkRam);

		MMC3::InitMapper();

		if(GetSaveRamSize() == 0) {
			SetCpuMemoryMapping(0x5000, 0x7FFF, 0, PrgMemoryType::WorkRam);
		}
	}

	void StreamState(bool saving) override
	{
		MMC3::StreamState(saving);
		Stream(_exRegs[0], _exRegs[1], _exRegs[2], _exRegs[3]);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr == 0x8001 && (GetState().Reg8000 & 0x07) >= 6) {
			_exRegs[(GetState().Reg8000 & 0x07) - 6] = value & 0x7F;
		}
		MMC3::WriteRegister(addr, value);
	}
	
	void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType) override
	{
		MMC3::SelectPRGPage(slot, _exRegs[slot], memoryType);
	}

	void UpdateChrMapping() override
	{
		if(_chrRamSize > 0 && _registers[0] == 0 && _registers[1] == 0 && _registers[2] == 0 && _registers[3] == 0 && _registers[4] == 0 && _registers[5] == 0) {
			SelectChrPage8x(0, 0, ChrMemoryType::ChrRam);
		} else {
			MMC3::UpdateChrMapping();
		}
	}
};
