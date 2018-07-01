#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Racermate : public BaseMapper
{
private:
	uint16_t _irqCounter;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x4000; }
	virtual uint16_t GetCHRPageSize() override { return 0x1000; }
	virtual uint32_t GetChrRamSize() override { return 0x10000; }
	virtual uint32_t GetSaveRamSize() override { return 0; }
	virtual bool ForceChrBattery() override { return true; }

	void InitMapper() override
	{
		_irqCounter = 0;

		SelectPRGPage(1, -1);
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_irqCounter);
	}

	void ProcessCpuClock() override
	{
		_irqCounter--;
		if(_irqCounter == 0) {
			_irqCounter = 1024;
			_console->GetCpu()->SetIrqSource(IRQSource::External);
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr & 0xC000) {
			case 0x8000: 
				SelectPRGPage(0, (value >> 6) & 0x03);
				SelectCHRPage(1, value & 0x0F);
				break;

			case 0xC000: 
				_irqCounter = 1024;
				_console->GetCpu()->ClearIrqSource(IRQSource::External);
				break;
		}

	}
};