#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "VrcIrq.h"
enum class VRCVariant;

//incomplete - missing audio and more
class VRC6 : public BaseMapper
{
private:
	VrcIrq _irq;
	VRCVariant _model;
	uint8_t _bankingMode;
	uint8_t _chrRegisters[8];

	void UpdatePrgRamAccess()
	{
		SetCpuMemoryMapping(0x6000, 0x7FFF, 0, HasBattery() ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam, (_bankingMode & 0x80) ? MemoryAccessType::ReadWrite : MemoryAccessType::NoAccess);
	}

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x0400; }
	
	void InitMapper()
	{
		_irq.Reset();
		_bankingMode = 0;
		memset(_chrRegisters, 0, sizeof(_chrRegisters));
		SelectPRGPage(3, -1);
	}

	virtual void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(_irq);
		Stream(_bankingMode, ArrayInfo<uint8_t>{_chrRegisters, 8});

		if(!saving) {
			UpdatePrgRamAccess();
		}
	}

	void ProcessCpuClock()
	{
		_irq.ProcessCpuClock();
	}

	void UpdatePpuBanking()
	{
		switch(_bankingMode & 0x03) {
			case 0:
				SelectCHRPage(0, _chrRegisters[0]);
				SelectCHRPage(1, _chrRegisters[1]);
				SelectCHRPage(2, _chrRegisters[2]);
				SelectCHRPage(3, _chrRegisters[3]);
				SelectCHRPage(4, _chrRegisters[4]);
				SelectCHRPage(5, _chrRegisters[5]);
				SelectCHRPage(6, _chrRegisters[6]);
				SelectCHRPage(7, _chrRegisters[7]);
				break;

			case 1:
				SelectChrPage2x(0, _chrRegisters[0]);
				SelectChrPage2x(1, _chrRegisters[1]);
				SelectChrPage2x(2, _chrRegisters[2]);
				SelectChrPage2x(3, _chrRegisters[3]);
				break;

			case 2: case 3:
				SelectCHRPage(0, _chrRegisters[0]);
				SelectCHRPage(1, _chrRegisters[1]);
				SelectCHRPage(2, _chrRegisters[2]);
				SelectCHRPage(3, _chrRegisters[3]);
				SelectChrPage2x(2, _chrRegisters[4]);
				SelectChrPage2x(3, _chrRegisters[5]);
				break;
		}
		
		//This is incorrect, but seems ok for all commercial games? (Based on old Disch documents)
		switch((_bankingMode >> 2) & 0x03) {
			case 0: SetMirroringType(MirroringType::Vertical); break;
			case 1: SetMirroringType(MirroringType::Horizontal); break;
			case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
			case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
		}

		UpdatePrgRamAccess();
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(_model == VRCVariant::VRC6b) {
			addr = (addr & 0xFFFC) | ((addr & 0x01) << 1) | ((addr & 0x02) >> 1);
		}

		switch(addr & 0xF003) {
			case 0x8000: case 0x8001: case 0x8002: case 0x8003: 
				SelectPrgPage2x(0, (value & 0x0F) << 1); 
				break;

			case 0xB003:
				_bankingMode = value;
				UpdatePpuBanking();
				break;

			case 0xC000: case 0xC001: case 0xC002: case 0xC003:
				SelectPRGPage(2, value & 0x1F);
				break;

			case 0xD000: case 0xD001: case 0xD002: case 0xD003:
				_chrRegisters[addr & 0x03] = value;
				UpdatePpuBanking();
				break;

			case 0xE000: case 0xE001: case 0xE002: case 0xE003:
				_chrRegisters[4 + (addr & 0x03)] = value;
				UpdatePpuBanking();
				break;

			case 0xF000:
				_irq.SetReloadValue(value);
				break;

			case 0xF001:
				_irq.SetControlValue(value);
				break;

			case 0xF002:
				_irq.AcknowledgeIrq();
				break;
		}
	}

public:
	VRC6(VRCVariant model) : _model(model)
	{
	}
};