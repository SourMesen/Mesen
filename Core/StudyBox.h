#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class StudyBox : public BaseMapper
{
private:
	uint8_t _prgRamReg;
	bool _tapeReady = false;
	uint16_t _tapeReadyDelay = 0;
	uint8_t _reg4202 = 0;

protected:
	virtual uint16_t RegisterStartAddress() { return 0x4200; }
	virtual uint16_t RegisterEndAddress() { return 0x43FF; }
	virtual bool AllowRegisterRead() { return true; }

	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	
	virtual uint32_t GetSaveRamSize() { return 0xC00; }
	virtual uint32_t GetSaveRamPageSize() { return 0xC00; }

	virtual uint32_t GetWorkRamSize() { return 0x8000; }
	virtual uint32_t GetWorkRamPageSize() { return 0x2000; }
	
	void InitMapper()
	{
		_prgRamReg = 0;

		SelectPRGPage(1, 0);
		SelectCHRPage(0, 0);
		SetCpuMemoryMapping(0x4400, 0x4FFF, 0, PrgMemoryType::SaveRam);

		UpdateState();
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(_prgRamReg);
	}

	uint8_t ReadRegister(uint16_t addr)
	{
		switch(addr) {
			case 0x4200: case 0x4203: return 0x00;
			case 0x4201: 
				return 0x10; /* | (EmulationSettings::CheckFlag(EmulationFlags::ShowFPS) ? 0x20 : 0x00)
					| (EmulationSettings::CheckFlag(EmulationFlags::Turbo) ? 0x40 : 0x00); */
				/*(EmulationSettings::CheckFlag(EmulationFlags::ShowFPS) ? 0x00 : 0x20) |
				(EmulationSettings::CheckFlag(EmulationFlags::Turbo) ? 0x00 : 0x50);*/

			case 0x4202: return _tapeReady ? 0x40 : 0x00;
			default: return 0xFF;
		}
	}

	void UpdateState()
	{
		SetCpuMemoryMapping(0x6000, 0x7FFF, _prgRamReg, PrgMemoryType::WorkRam);
	}

	void ProcessCpuClock()
	{
		if(_tapeReadyDelay > 0) {
			_tapeReadyDelay--;
			if(_tapeReadyDelay == 0) {
				_tapeReady = (_reg4202 & 0x10) == 0x10;
			}
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr & 0x4203) {
			switch(addr & 0x03) {
				case 0: _prgRamReg = value >> 6; UpdateState(); break;
				case 1: SelectPRGPage(0, value); break;
				case 2:
					_reg4202 = value;
					_tapeReadyDelay = 100;
					break;
				case 3:
					break;
			}
		}
	}
};