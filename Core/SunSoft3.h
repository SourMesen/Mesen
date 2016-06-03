#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"

class SunSoft3 : public BaseMapper
{
private:
	bool _irqLatch = false;
	bool _irqEnabled = false;
	uint16_t _irqCounter = 0;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x800; }

	void InitMapper()
	{
		SelectPRGPage(1, -1);
	}

	virtual void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(_irqLatch, _irqEnabled, _irqCounter);
	}

	virtual void ProcessCpuClock()
	{
		if(_irqEnabled) {
			_irqCounter--;
			if(_irqCounter == 0xFFFF) {
				_irqEnabled = false;
				CPU::SetIRQSource(IRQSource::External);
			}
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xF800) {
			case 0x8800: SelectCHRPage(0, value); break;
			case 0x9800: SelectCHRPage(1, value); break;
			case 0xA800: SelectCHRPage(2, value); break;
			case 0xB800: SelectCHRPage(3, value); break;
			case 0xC800: 
				_irqCounter &= _irqLatch ? 0xFF00 : 0x00FF;
				_irqCounter |= _irqLatch ? value : (value << 8);
				_irqLatch = !_irqLatch;
				break;
			case 0xD800:
				_irqEnabled = (value & 0x10) == 0x10;
				_irqLatch = false;
				CPU::ClearIRQSource(IRQSource::External);
				break;
			case 0xE800:
				switch(value & 0x03) {
					case 0: SetMirroringType(MirroringType::Vertical); break;
					case 1: SetMirroringType(MirroringType::Horizontal); break;
					case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
					case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
				}
				break;
			case 0xF800: SelectPRGPage(0, value); break;
		}
	}
};
