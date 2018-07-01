#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"

class Mapper42 : public BaseMapper
{
private:
	uint16_t _irqCounter;
	bool _irqEnabled;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x2000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		_irqCounter = 0;
		_irqEnabled = false;

		SelectPRGPage(0, 0x0C);
		SelectPRGPage(1, 0x0D);
		SelectPRGPage(2, 0x0E);
		SelectPRGPage(3, 0x0F);
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
			if(_irqCounter >= 0x8000) {
				_irqCounter -= 0x8000;
			}
			if(_irqCounter >= 0x6000) {
				_console->GetCpu()->SetIrqSource(IRQSource::External);
			} else {
				_console->GetCpu()->ClearIrqSource(IRQSource::External);
			}
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr & 0xE003) {
			case 0x8000:
				if(_chrRomSize > 0) {
					SelectCHRPage(0, value & 0x0F);
				}
				break;

			case 0xE000:
				SetCpuMemoryMapping(0x6000, 0x7FFF, value & 0x0F, PrgMemoryType::PrgRom);
				break;

			case 0xE001:
				SetMirroringType(value & 0x08 ? MirroringType::Horizontal : MirroringType::Vertical);
				break;

			case 0xE002:
				_irqEnabled = (value == 0x02);

				if(!_irqEnabled) {
					_console->GetCpu()->ClearIrqSource(IRQSource::External);
					_irqCounter = 0;
				}
				break;
		}
	}
};
