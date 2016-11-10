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

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(_prgReg, _chrReg);
		if(!saving) {
			UpdateState();
		}
	}

	void UpdateState()
	{
		SetCpuMemoryMapping(0x6000, 0x7FFF, _prgReg & 0x03, PrgMemoryType::PrgRom);
		SelectCHRPage(0, _chrReg);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if((addr & 0x9000) == 0x8000){
			_prgReg = _chrReg = value;
		} else {
			//For ProWres
			_chrReg = value & 0x01;
		}
		UpdateState();
	}
};