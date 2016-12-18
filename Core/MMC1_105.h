#pragma once
#include "MMC1.h"

class MMC1_105 : public MMC1
{
private:
	uint8_t _initState;
	uint32_t _irqCounter;
	bool _irqEnabled;

protected:
	void InitMapper() override
	{
		MMC1::InitMapper();
		_initState = 0;
		_irqCounter = 0;
		_irqEnabled = false;
		_state.RegA000 |= 0x10;  //Set I bit to 1
	}

	void StreamState(bool saving) override
	{
		MMC1::StreamState(saving);
		Stream(_initState, _irqCounter, _irqEnabled);
	}

	void ProcessCpuClock() override
	{
		if(_irqEnabled) {
			_irqCounter++;
			//TODO: Counter hardcoded - should be based on dip switches
			if(_irqCounter == 0x28000000) {
				CPU::SetIRQSource(IRQSource::External);
				_irqEnabled = false;
			}
		}
	}

	void UpdateState() override
	{
		if(_initState == 0 && (_state.RegA000 & 0x10) == 0x00) {
			_initState = 1;
		} else if(_initState == 1 && _state.RegA000 & 0x10) {
			_initState = 2;
		}

		if(_state.RegA000 & 0x10) {
			_irqEnabled = false;
			_irqCounter = 0;
			CPU::ClearIRQSource(IRQSource::External);
		} else {
			_irqEnabled = true;
		}

		switch(_state.Reg8000 & 0x03) {
			case 0: SetMirroringType(MirroringType::ScreenAOnly); break;
			case 1: SetMirroringType(MirroringType::ScreenBOnly); break;
			case 2: SetMirroringType(MirroringType::Vertical); break;
			case 3: SetMirroringType(MirroringType::Horizontal); break;
		}

		if(_state.RegE000 & 0x10) {
			RemoveCpuMemoryMapping(0x6000, 0x7FFF);
		} else {
			SetCpuMemoryMapping(0x6000, 0x7FFF, 0, HasBattery() ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam);
		}

		if(_initState == 2) {
			if(_state.RegA000 & 0x08) {
				//MMC1 mode
				uint8_t prgReg = (_state.RegE000 & 0x07) | 0x08;
				if(_state.Reg8000 & 0x08) {
					if(_state.Reg8000 & 0x04) {
						SelectPRGPage(0, prgReg);
						SelectPRGPage(1, 0x0F);
					} else {
						SelectPRGPage(0, 0x08);
						SelectPRGPage(1, prgReg);
					}
				} else {
					SelectPrgPage2x(0, prgReg & 0xFE);
				}
			} else {
				SelectPrgPage2x(0, _state.RegA000 & 0x06);
			}
		} else {
			SelectPrgPage2x(0, 0);
		}
	}
};