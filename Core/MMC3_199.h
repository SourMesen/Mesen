#pragma once

#include "stdafx.h"
#include "MMC3.h"

class MMC3_199 : public MMC3
{
private:
	uint8_t _exRegs[4];

protected:
	uint32_t GetChrRamSize() { return 0x2000; }
	uint16_t GetChrRamPageSize() { return 0x400; }

	void InitMapper() override
	{
		_exRegs[0] = 0xFE;
		_exRegs[1] = 0xFF;
		_exRegs[2] = 1;
		_exRegs[3] = 3;

		MMC3::InitMapper();
	}

	void StreamState(bool saving) override
	{
		MMC3::StreamState(saving);
		Stream(_exRegs[0], _exRegs[1], _exRegs[2], _exRegs[3]);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr == 0x8001 && (GetState().Reg8000 & 0x08)) {
			_exRegs[GetState().Reg8000 & 0x03] = value;
			UpdatePrgMapping();
			UpdateChrMapping();
		} else {
			MMC3::WriteRegister(addr, value);
		}
	}

	void UpdateMirroring() override
	{
		switch(GetState().RegA000 & 0x03) {
			case 0: SetMirroringType(MirroringType::Vertical); break;
			case 1: SetMirroringType(MirroringType::Horizontal); break;
			case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
			case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
		}
	}

	void UpdatePrgMapping() override
	{
		MMC3::UpdatePrgMapping();
		SelectPRGPage(2, _exRegs[0]);
		SelectPRGPage(3, _exRegs[1]);
	}

	void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType) override
	{
		MMC3::SelectCHRPage(slot, page, page < 8 ? ChrMemoryType::ChrRam : ChrMemoryType::ChrRom);

		MMC3::SelectCHRPage(0, _registers[0], _registers[0] < 8 ? ChrMemoryType::ChrRam : ChrMemoryType::ChrRom);
		MMC3::SelectCHRPage(1, _exRegs[2], _exRegs[2] < 8 ? ChrMemoryType::ChrRam : ChrMemoryType::ChrRom);
		MMC3::SelectCHRPage(2, _registers[1], _registers[1] < 8 ? ChrMemoryType::ChrRam : ChrMemoryType::ChrRom);
		MMC3::SelectCHRPage(3, _exRegs[3], _exRegs[3] < 8 ? ChrMemoryType::ChrRam : ChrMemoryType::ChrRom);
	}
};