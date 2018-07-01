#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"

class Mapper50 : public BaseMapper
{
private:
	uint16_t _irqCounter;
	bool _irqEnabled;

protected:
	virtual uint16_t RegisterStartAddress() override { return 0x4020; }
	virtual uint16_t RegisterEndAddress() override { return 0x5FFF; }
	virtual uint16_t GetPRGPageSize() override { return 0x2000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		_irqCounter = 0;
		_irqEnabled = false;

		SetCpuMemoryMapping(0x6000, 0x7FFF, 0x0F, PrgMemoryType::PrgRom);
		SelectPRGPage(0, 0x08);
		SelectPRGPage(1, 0x09);
		SelectPRGPage(3, 0x0B);
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);

		Stream(_irqCounter, _irqEnabled);
	}

	void ProcessCpuClock() override
	{
		if(_irqEnabled) {
			_irqCounter++;
			if(_irqCounter == 0x1000) {
				_console->GetCpu()->SetIrqSource(IRQSource::External);
				_irqEnabled = false;
			}
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr & 0x4120) {
			case 0x4020:
				SelectPRGPage(2, (value & 0x08) | ((value & 0x01) << 2) | ((value & 0x06) >> 1));
				break;

			case 0x4120:
				if(value & 0x01) {
					_irqEnabled = true;
				} else {
					_console->GetCpu()->ClearIrqSource(IRQSource::External);
					_irqCounter = 0;
					_irqEnabled = false;
				}
				break;
		}
	}
};
