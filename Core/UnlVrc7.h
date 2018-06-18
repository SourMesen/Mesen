#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "VrcIrq.h"

class UnlVrc7 : public BaseMapper
{
private:
	VrcIrq _irq;
	uint8_t _chrRegisters[8];

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x2000; }
	virtual uint16_t GetCHRPageSize() override { return 0x0400; }

	void InitMapper() override
	{
		_irq.Reset();
		memset(_chrRegisters, 0, sizeof(_chrRegisters));
		SelectPRGPage(3, -1);
	}

	virtual void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		SnapshotInfo irq { &_irq };
		ArrayInfo<uint8_t> chrRegisters = { _chrRegisters, 8 };

		Stream(chrRegisters, irq);
	}

	void ProcessCpuClock() override
	{
		_irq.ProcessCpuClock();
	}
	
	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr & 0xF038) {
			case 0x8000: SelectPRGPage(0, value & 0x3F); break;
			case 0x8008: SelectPRGPage(1, value & 0x3F); break;
			case 0x9000: SelectPRGPage(2, value & 0x3F); break;

			case 0xA000: SelectCHRPage(0, value);  break;
			case 0xA008: SelectCHRPage(1, value);  break;
			case 0xB000: SelectCHRPage(2, value);  break;
			case 0xB008: SelectCHRPage(3, value);  break;
			case 0xC000: SelectCHRPage(4, value);  break;
			case 0xC008: SelectCHRPage(5, value);  break;
			case 0xD000: SelectCHRPage(6, value);  break;
			case 0xD008: SelectCHRPage(7, value);  break;

			case 0xE000: 
				switch(value & 0x03) {
					case 0: SetMirroringType(MirroringType::Vertical); break;
					case 1: SetMirroringType(MirroringType::Horizontal); break;
					case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
					case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
				}
				break;

			case 0xE008: _irq.SetReloadValue(value + 8); break;
			case 0xF000: _irq.SetControlValue(value & 0x03); break;
			case 0xF008: _irq.AcknowledgeIrq(); break;
		}
	}
};