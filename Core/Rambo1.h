#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"

class Rambo1 : public BaseMapper
{
private:	
	const uint8_t PpuIrqDelay = 3;
	const uint8_t CpuIrqDelay = 1;
	bool _irqEnabled = false;
	bool _irqCycleMode = false;
	bool _needReload = false;
	uint8_t _irqCounter = 0;
	uint8_t _irqReloadValue = 0;
	uint32_t _lastCycle = 0;
	uint32_t _cyclesDown = 0;
	uint8_t _cpuClockCounter = 0;

	uint8_t _currentRegister = 0;
	uint8_t _registers[16];
	uint8_t _needIrqDelay = 0;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x400; }

	void InitMapper()
	{
		memset(_registers, 0, sizeof(_registers));
		SelectPRGPage(3, -1);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream<bool>(_irqEnabled);
		Stream<bool>(_irqCycleMode);
		Stream<bool>(_needReload);
		Stream<uint8_t>(_needIrqDelay);
		Stream<uint8_t>(_irqCounter);
		Stream<uint8_t>(_irqReloadValue);
		Stream<uint32_t>(_lastCycle);
		Stream<uint32_t>(_cyclesDown);
		Stream<uint8_t>(_cpuClockCounter);

		Stream<uint8_t>(_currentRegister);
		StreamArray<uint8_t>(_registers, 16);
	}

	virtual void ProcessCpuClock()
	{
		if(_needIrqDelay) {
			_needIrqDelay--;
			if(_needIrqDelay == 0) {
				CPU::SetIRQSource(IRQSource::External);
			}
		}
		if(_irqCycleMode) {
			_cpuClockCounter = (_cpuClockCounter + 1) & 0x03;
			if(_cpuClockCounter == 0) {
				ClockIrqCounter(Rambo1::CpuIrqDelay);
			}
		}
	}

	void ClockIrqCounter(const uint8_t delay)
	{
		if(_needReload) {
			_irqCounter = _irqReloadValue + 1;
			_cpuClockCounter = 0;
			_needReload = false;
		} else if(_irqCounter == 0) {
			_irqCounter = _irqReloadValue;
			_cpuClockCounter = 0;
		} else {
			_irqCounter--;
			if(_irqCounter == 0 && _irqEnabled) {
				_needIrqDelay = delay;
			}
		}
	}

	void UpdateState()
	{
		if(_currentRegister & 0x40) {
			SelectPRGPage(0, _registers[15]);
			SelectPRGPage(1, _registers[6]);
			SelectPRGPage(2, _registers[7]);
		} else {
			SelectPRGPage(0, _registers[6]);
			SelectPRGPage(1, _registers[7]);
			SelectPRGPage(2, _registers[15]);
		}

		uint8_t a12Inversion = _currentRegister & 0x80 ? 0x04 : 0x00;
		if(a12Inversion) {
			std::cout << "test";
		}
		SelectCHRPage(0 ^ a12Inversion, _registers[0]);
		SelectCHRPage(2 ^ a12Inversion, _registers[1]);
		SelectCHRPage(4 ^ a12Inversion, _registers[2]);
		SelectCHRPage(5 ^ a12Inversion, _registers[3]);
		SelectCHRPage(6 ^ a12Inversion, _registers[4]);
		SelectCHRPage(7 ^ a12Inversion, _registers[5]);

		if(_currentRegister & 0x20) {
			SelectCHRPage(1 ^ a12Inversion, _registers[8]);
			SelectCHRPage(3 ^ a12Inversion, _registers[9]);
		} else {
			SelectCHRPage(1 ^ a12Inversion, _registers[0]+1);
			SelectCHRPage(3 ^ a12Inversion, _registers[1]+1);
		}		
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xE001) {
			case 0x8000: 
				_currentRegister = value;
				break;

			case 0x8001:
				_registers[_currentRegister & 0x0F] = value;
				UpdateState();
				break;
			
			case 0xA000: 
				SetMirroringType(value & 0x01 ? MirroringType::Horizontal : MirroringType::Vertical); 
				break;

			case 0xC000: 
				_irqReloadValue = value; 
				break;

			case 0xC001:
				_irqCycleMode = (value & 0x01) == 0x01;
				_needReload = true;
				break;

			case 0xE000:
				_irqEnabled = false;
				CPU::ClearIRQSource(IRQSource::External);
				break;
			
			case 0xE001:
				_irqEnabled = true;
				break;
		}
	}

public:
	virtual void NotifyVRAMAddressChange(uint16_t addr)
	{
		uint32_t cycle = PPU::GetFrameCycle();

		if((addr & 0x1000) == 0) {
			if(_cyclesDown == 0) {
				_cyclesDown = 1;
			} else {
				if(_lastCycle > cycle) {
					//We changed frames
					_cyclesDown += (89342 - _lastCycle) + cycle;
				} else {
					_cyclesDown += (cycle - _lastCycle);
				}
			}
		} else if(addr & 0x1000) {
			if(_cyclesDown > 8) {
				ClockIrqCounter(Rambo1::PpuIrqDelay);
			}
			_cyclesDown = 0;
		}
		_lastCycle = cycle;
	}
};