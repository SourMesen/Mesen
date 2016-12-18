#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "VrcIrq.h"
#include "Vrc7Audio.h"

//incomplete - missing audio
class VRC7 : public BaseMapper
{
private:
	Vrc7Audio _audio;
	VrcIrq _irq;
	uint8_t _controlFlags;
	uint8_t _chrRegisters[8];

	void UpdatePrgRamAccess()
	{
		SetCpuMemoryMapping(0x6000, 0x7FFF, 0, HasBattery() ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam, (_controlFlags & 0x80) ? MemoryAccessType::ReadWrite : MemoryAccessType::NoAccess);
	}

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x2000; }
	virtual uint16_t GetCHRPageSize() override { return 0x0400; }

	void InitMapper() override
	{
		_irq.Reset();
		_controlFlags = 0;
		memset(_chrRegisters, 0, sizeof(_chrRegisters));
		SelectPRGPage(3, -1);
	}

	virtual void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		SnapshotInfo irq{ &_irq };
		SnapshotInfo audio{ &_audio };
		ArrayInfo<uint8_t> chrRegisters = { _chrRegisters, 8 };

		Stream(_controlFlags, chrRegisters, irq, audio);

		if(!saving) {
			UpdatePrgRamAccess();
		}
	}

	void ProcessCpuClock() override
	{
		_irq.ProcessCpuClock();
		_audio.Clock();
	}

	void UpdateState()
	{
		switch(_controlFlags & 0x03) {
			case 0: SetMirroringType(MirroringType::Vertical); break;
			case 1: SetMirroringType(MirroringType::Horizontal); break;
			case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
			case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
		}

		UpdatePrgRamAccess();
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr & 0x10 && (addr & 0xF010) != 0x9010) {
			addr |= 0x08;
			addr &= ~0x10;
		}

		switch(addr & 0xF038) {
			case 0x8000: SelectPRGPage(0, value & 0x3F); break;
			case 0x8008: SelectPRGPage(1, value & 0x3F); break;
			case 0x9000: SelectPRGPage(2, value & 0x3F); break;
				
			case 0x9010: case 0x9030: _audio.WriteReg(addr, value); break;
			 
			case 0xA000: SelectCHRPage(0, value);  break;
			case 0xA008: SelectCHRPage(1, value);  break;
			case 0xB000: SelectCHRPage(2, value);  break;
			case 0xB008: SelectCHRPage(3, value);  break;
			case 0xC000: SelectCHRPage(4, value);  break;
			case 0xC008: SelectCHRPage(5, value);  break;
			case 0xD000: SelectCHRPage(6, value);  break;
			case 0xD008: SelectCHRPage(7, value);  break;

			case 0xE000: _controlFlags = value; UpdateState(); break;				

			case 0xE008: _irq.SetReloadValue(value); break;
			case 0xF000: _irq.SetControlValue(value); break;
			case 0xF008: _irq.AcknowledgeIrq(); break;
		}
	}
};