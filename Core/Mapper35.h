#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "A12Watcher.h"
#include "CPU.h"

class Mapper35 : public BaseMapper
{
private:
	uint8_t _irqCounter;
	bool _irqEnabled;
	A12Watcher _a12Watcher;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x0400; }

	void InitMapper()
	{
		_irqEnabled = false;
		_irqCounter = 0;

		SelectPRGPage(3, -1);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		SnapshotInfo a12Watcher{ &_a12Watcher };
		Stream(_irqCounter, _irqEnabled, a12Watcher);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xF007) {
			case 0x8000: case 0x8001: case 0x8002: case 0x8003:
				SelectPRGPage(addr & 0x03, value);
				break;

			case 0x9000: case 0x9001: case 0x9002: case 0x9003:
			case 0x9004: case 0x9005: case 0x9006: case 0x9007:
				SelectCHRPage(addr & 0x07, value);
				break;

			case 0xC002: 
				_irqEnabled = false;
				CPU::ClearIRQSource(IRQSource::External);
				break;

			case 0xC003: _irqEnabled = true; break;
			case 0xC005: _irqCounter = value; break;
			
			case 0xD001:
				SetMirroringType(value & 0x01 ? MirroringType::Horizontal : MirroringType::Vertical);
				break;
		}
	}
	
	virtual void NotifyVRAMAddressChange(uint16_t addr)
	{
		//MMC3-style A12 IRQ counter
		if(_a12Watcher.UpdateVramAddress(addr) == A12StateChange::Rise) {
			if(_irqEnabled) {
				_irqCounter--;
				if(_irqCounter == 0) {
					_irqEnabled = false;
					CPU::SetIRQSource(IRQSource::External);
				}
			}
		}
	}
};
