#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_Super24in1Sc03 : public MMC3
{
private:
	const int _prgMask[8] = { 0x3F, 0x1F, 0x0F, 0x01, 0x03, 0, 0, 0 };
	uint8_t _exRegs[3];

protected:
	uint32_t GetChrRamSize() override { return 0x2000; }
	uint16_t GetChrRamPageSize() override { return 0x400; }

	void InitMapper() override
	{
		MMC3::InitMapper();
		AddRegisterRange(0x5FF0, 0x5FF2, MemoryOperation::Write);
	}

	void Reset(bool softReset) override
	{
		MMC3::ResetMmc3();

		_exRegs[0] = 0x24;
		_exRegs[1] = 0x9F;
		_exRegs[2] = 0;

		_registers[6] = 0;
		_registers[7] = 1;
		UpdateState();
	}

	void StreamState(bool saving) override
	{
		MMC3::StreamState(saving);
		Stream(_exRegs[0], _exRegs[1], _exRegs[2]);
		
		if(!saving) {
			UpdateState();
		}
	}

	void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default) override
	{
		MMC3::SelectCHRPage(slot, ((_exRegs[2] << 3) & 0xF00) | page, _exRegs[0] & 0x20 ? ChrMemoryType::ChrRam : ChrMemoryType::ChrRom);
	}

	void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom) override
	{
		MMC3::SelectPRGPage(slot, ((_exRegs[1] << 1) | (page & _prgMask[_exRegs[0] & 0x07])) & 0xFF);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr < 0x8000) {
			_exRegs[addr & 0x03] = value;
			UpdateState();
		} else {
			MMC3::WriteRegister(addr, value);
		}
	}
};