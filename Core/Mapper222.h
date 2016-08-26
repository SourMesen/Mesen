#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"
#include "A12Watcher.h"

class Mapper222 : public BaseMapper
{
private:
	uint16_t _irqCounter;
	A12Watcher _a12Watcher;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x400; }

	void InitMapper()
	{
		_irqCounter = 0;

		SelectPrgPage2x(1, -2);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		SnapshotInfo a12Watcher{ &_a12Watcher };
		Stream(_irqCounter, a12Watcher);
	}

	virtual void NotifyVRAMAddressChange(uint16_t addr)
	{
		if(_a12Watcher.UpdateVramAddress(addr) == A12StateChange::Rise) {
			if(_irqCounter) {
				_irqCounter++;
				if(_irqCounter >= 240) {
					CPU::SetIRQSource(IRQSource::External);
					_irqCounter = 0;
				}
			}
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xF003) {
			case 0x8000: SelectPRGPage(0, value); break;
			case 0x9000: SetMirroringType(value & 0x01 ? MirroringType::Horizontal : MirroringType::Vertical); break;
			case 0xA000: SelectPRGPage(1, value); break;
			case 0xB000: SelectCHRPage(0, value); break;
			case 0xB002: SelectCHRPage(1, value); break;
			case 0xC000: SelectCHRPage(2, value); break;
			case 0xC002: SelectCHRPage(3, value); break;
			case 0xD000: SelectCHRPage(4, value); break;
			case 0xD002: SelectCHRPage(5, value); break;
			case 0xE000: SelectCHRPage(6, value); break;
			case 0xE002: SelectCHRPage(7, value); break;
			case 0xF000: 
				_irqCounter = value;
				CPU::ClearIRQSource(IRQSource::External);
				break;
		}
	}
};