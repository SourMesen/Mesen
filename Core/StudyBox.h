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
	virtual uint16_t RegisterStartAddress() override { return 0x4200; }
	virtual uint16_t RegisterEndAddress() override { return 0x43FF; }
	virtual bool AllowRegisterRead() override { return true; }

	virtual uint16_t GetPRGPageSize() override { return 0x4000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }
	
	virtual uint32_t GetSaveRamSize() override { return 0xC00; }
	virtual uint32_t GetSaveRamPageSize() override { return 0xC00; }

	virtual uint32_t GetWorkRamSize() override { return 0x8000; }
	virtual uint32_t GetWorkRamPageSize() override { return 0x2000; }
	
	void InitMapper() override
	{
		_prgRamReg = 0;

		SelectPRGPage(1, 0);
		SelectCHRPage(0, 0);
		SetCpuMemoryMapping(0x4400, 0x4FFF, 0, PrgMemoryType::SaveRam);

		UpdateState();
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_prgRamReg);
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		switch(addr) {
			case 0x4200: case 0x4203: return 0x00;
			case 0x4201: return 0x10;
			case 0x4202: return _tapeReady ? 0x40 : 0x00;
			default: return 0xFF;
		}
	}

	void UpdateState()
	{
		SetCpuMemoryMapping(0x6000, 0x7FFF, _prgRamReg, PrgMemoryType::WorkRam);
	}

	void ProcessCpuClock() override
	{
		if(_tapeReadyDelay > 0) {
			_tapeReadyDelay--;
			if(_tapeReadyDelay == 0) {
				_tapeReady = (_reg4202 & 0x10) == 0x10;
			}
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
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