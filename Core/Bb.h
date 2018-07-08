#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Bb : public BaseMapper
{
private:
	uint8_t _prgReg;
	uint8_t _chrReg;

protected:
	uint16_t GetPRGPageSize() override { return 0x2000; }
	uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		_prgReg = -1;
		_chrReg = 0;

		SelectPrgPage4x(0, -4);
		UpdateState();
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_prgReg, _chrReg);
		if(!saving) {
			UpdateState();
		}
	}

	void UpdateState()
	{
		SetCpuMemoryMapping(0x6000, 0x7FFF, _prgReg, PrgMemoryType::PrgRom);
		SelectCHRPage(0, _chrReg);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if((addr & 0x9000) == 0x8000 || addr >= 0xF000){
			//A version of Bubble Bobble expects writes to $F000+ to switch the PRG banks
			_prgReg = _chrReg = value;
		} else {
			//For ProWres
			_chrReg = value & 0x01;
		}
		UpdateState();
	}
};