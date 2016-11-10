#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"

class Mapper43 : public BaseMapper
{
private:
	uint8_t _reg;
	bool _swap;
	uint16_t _irqCounter;
	bool _irqEnabled;

protected:
	uint16_t GetPRGPageSize() override { return 0x2000; }
	uint16_t GetCHRPageSize() override { return 0x2000; }
	uint16_t RegisterStartAddress() override { return 0x4020; }
	uint16_t RegisterEndAddress() override { return 0xFFFF; }

	void InitMapper() override
	{
		_irqCounter = 0;
		_irqEnabled = false;
		_swap = false;
		_reg = 0;

		UpdateState();
		SetCpuMemoryMapping(0x5000, 0x5FFF, 8, PrgMemoryType::PrgRom);
		SelectPRGPage(0, 1);
		SelectPRGPage(1, 0);
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_irqCounter, _irqEnabled, _reg, _swap);

		if(!saving) {
			UpdateState();
		}
	}

	void ProcessCpuClock() override
	{
		if(_irqEnabled) {
			_irqCounter++;
			if(_irqCounter >= 4096) {
				_irqEnabled = false;
				CPU::SetIRQSource(IRQSource::External);
			}
		}
	}

	void UpdateState()
	{
		SetCpuMemoryMapping(0x6000, 0x7FFF, _swap ? 0 : 2, PrgMemoryType::PrgRom);
		SelectPRGPage(2, _reg);
		SelectPRGPage(3, _swap ? 8 : 9);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		int lut[8] = { 4, 3, 5, 3, 6, 3, 7, 3 };
		switch(addr & 0xF1FF) {
			case 0x4022: _reg = lut[value & 0x07]; UpdateState(); break;
			case 0x4120: _swap = value & 0x01; UpdateState(); break;
			
			case 0x8122:
			case 0x4122:
				_irqEnabled = (value & 0x01) == 0x01;
				CPU::ClearIRQSource(IRQSource::External);
				_irqCounter = 0;
				break;
		}
	}
};
