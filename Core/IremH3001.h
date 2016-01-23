#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"

class IremH3001 : public BaseMapper
{
private:
	bool _irqEnabled = false;
	uint16_t _irqCounter = 0;
	uint16_t _irqReloadValue = 0;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x400; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
		SelectPRGPage(1, 1);
		SelectPRGPage(2, 0xFE);
		SelectPRGPage(3, -1);
	}
	
	virtual void ProcessCpuClock()
	{
		if(_irqEnabled) {
			_irqCounter--;
			if(_irqCounter == 0) {
				_irqEnabled = false;
				CPU::SetIRQSource(IRQSource::External);
			}
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr) {
			case 0x8000: SelectPRGPage(0, value); break;

			case 0x9001: SetMirroringType(value & 0x80 ? MirroringType::Horizontal : MirroringType::Vertical); break;
			case 0x9003: 
				_irqEnabled = (value & 0x80) == 0x80; 
				CPU::ClearIRQSource(IRQSource::External);
				break;

			case 0x9004: 
				_irqCounter = _irqReloadValue;
				CPU::ClearIRQSource(IRQSource::External);
				break;

			case 0x9005: _irqReloadValue = (_irqReloadValue & 0x00FF) | (value << 8); break;
			case 0x9006: _irqReloadValue = (_irqReloadValue & 0xFF00) | value; break;

			case 0xA000: SelectPRGPage(1, value); break;
			
			case 0xB000: SelectCHRPage(0, value); break;
			case 0xB001: SelectCHRPage(1, value); break;
			case 0xB002: SelectCHRPage(2, value); break;
			case 0xB003: SelectCHRPage(3, value); break;
			case 0xB004: SelectCHRPage(4, value); break;
			case 0xB005: SelectCHRPage(5, value); break;
			case 0xB006: SelectCHRPage(6, value); break;
			case 0xB007: SelectCHRPage(7, value); break;

			case 0xC000: SelectPRGPage(2, value); break;
		}
	}
};