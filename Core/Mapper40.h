#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"

class Mapper40 : public BaseMapper
{
private:
	uint16_t _irqCounter;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		_irqCounter = 0;

		SetCpuMemoryMapping(0x6000, 0x7FFF, 6, PrgMemoryType::PrgRom);
		SelectPRGPage(0, 4);
		SelectPRGPage(1, 5);
		SelectPRGPage(3, 7);
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);

		Stream(_irqCounter);
	}

	void ProcessCpuClock()
	{
		if(_irqCounter > 0) {
			_irqCounter--;
			if(_irqCounter == 0) {
				CPU::SetIRQSource(IRQSource::External);
			}
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xE000) {
			case 0x8000:
				_irqCounter = 0;
				CPU::ClearIRQSource(IRQSource::External);
				break;
			case 0xA000:
				_irqCounter = 4096;
				break;
			case 0xE000:
				SelectPRGPage(2, value);
				break;
		}
	}
};
