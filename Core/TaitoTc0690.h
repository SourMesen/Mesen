#pragma once
#include "stdafx.h"
#include "TaitoTc0190.h"
#include "MMC3.h"

class TaitoTc0690 : public MMC3
{
private:
	uint8_t _irqDelay;

protected:
	virtual void InitMapper() override
	{
		_irqDelay = 0;
		SelectPRGPage(2, -2);
		SelectPRGPage(3, -1);
	}

	virtual void StreamState(bool saving) override
	{
		MMC3::StreamState(saving);
		Stream(_irqDelay);
	}

	virtual void TriggerIrq() override
	{
		//"The IRQ seems to trip a little later than it does on MMC3.  It looks like about a 4 CPU cycle delay from the normal MMC3 IRQ time."
		//A value of 5 removes the shaking from The Jetsons
		_irqDelay = 5;
	}

	void ProcessCpuClock() override
	{
		if(_irqDelay > 0) {
			_irqDelay--;
			if(_irqDelay == 0) {
				CPU::SetIRQSource(IRQSource::External);
			}
		}
	}

	virtual void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr & 0xE003) {
			case 0x8000:
				SelectPRGPage(0, value & 0x3F);
				break;
			case 0x8001:
				SelectPRGPage(1, value & 0x3F);
				break;
			case 0x8002:
				SelectCHRPage(0, value * 2);
				SelectCHRPage(1, value * 2 + 1);
				break;
			case 0x8003:
				SelectCHRPage(2, value * 2);
				SelectCHRPage(3, value * 2 + 1);
				break;
			case 0xA000: case 0xA001: case 0xA002: case 0xA003:
				SelectCHRPage(4 + (addr & 0x03), value);
				break;

			case 0xC000:
				_irqReloadValue = (0x100 - value) & 0xFF;
				break;
			case 0xC001:
				_irqCounter = 0;
				_irqReload = true;
				break;
			case 0xC002:
				_irqEnabled = true;
				break;
			case 0xC003:
				_irqEnabled = false;
				CPU::ClearIRQSource(IRQSource::External);
				break;

			case 0xE000:
				SetMirroringType((value & 0x40) == 0x40 ? MirroringType::Horizontal : MirroringType::Vertical);
				break;
		}
	}
};