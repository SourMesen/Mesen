#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"

class Sunsoft4 : public BaseMapper
{
private:
	uint8_t _ntRegs[2];
	bool _useChrForNametables;
	bool _prgRamEnabled;
	uint32_t _licensingTimer;
	bool _usingExternalRom;
	uint8_t _externalPage = 0;

	void UpdateNametables()
	{
		AddNametable(4, _chrRom + _ntRegs[0] * 0x400);
		AddNametable(5, _chrRom + _ntRegs[1] * 0x400);

		if(_useChrForNametables) {
			switch(GetMirroringType()) {
				case MirroringType::Vertical: SetNametables(4, 5, 4, 5); break;
				case MirroringType::Horizontal: SetNametables(4, 4, 5, 5); break;
				case MirroringType::ScreenAOnly: SetNametables(4, 4, 4, 4);	break;
				case MirroringType::ScreenBOnly: SetNametables(5, 5, 5, 5);	break;
			}
		} else {
			//Reset to default mirroring
			SetMirroringType(GetMirroringType());
		}
	}

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x4000; }
	virtual uint16_t GetCHRPageSize() override { return 0x800; }

	void InitMapper() override
	{
		_useChrForNametables = false;
		_ntRegs[0] = _ntRegs[1] = 0;
		
		_licensingTimer = 0;
		_usingExternalRom = false;
		_prgRamEnabled = false;

		//Bank 0's initial state is undefined, but some roms expect it to be the first page
		SelectPRGPage(0, 0);
		SelectPRGPage(1, 7);

		UpdateState();
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		
		Stream(_ntRegs[0], _ntRegs[1], _useChrForNametables, _prgRamEnabled, _usingExternalRom, _externalPage);

		if(!saving) {
			UpdateNametables();
			UpdateState();
		}
	}

	void UpdateState()
	{
		if(!_prgRamEnabled) {
			RemoveCpuMemoryMapping(0x6000, 0x7FFF);
		} else {
			SetupDefaultWorkRam();
		}
		
		if(_usingExternalRom) { 
			if(_licensingTimer == 0) {
				RemoveCpuMemoryMapping(0x8000, 0xBFFF);
			} else {
				SelectPRGPage(0, _externalPage);
			}
		}
	}

	void ProcessCpuClock() override
	{
		if(_licensingTimer) {
			_licensingTimer--;
			if(_licensingTimer == 0) {
				UpdateState();
			}
		}
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		if(addr >= 0x6000 && addr <= 0x7FFF) {
			_licensingTimer = 1024 * 105;
			UpdateState();
		}
		BaseMapper::WriteRAM(addr, value);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr & 0xF000) {
			case 0x8000: SelectCHRPage(0, value); break;
			case 0x9000: SelectCHRPage(1, value); break;
			case 0xA000: SelectCHRPage(2, value); break;
			case 0xB000: SelectCHRPage(3, value); break;
			case 0xC000: 
				_ntRegs[0] = value | 0x80;
				UpdateNametables();
				break;
			case 0xD000:
				_ntRegs[1] = value | 0x80;
				UpdateNametables();
				break;
			case 0xE000:
				switch(value & 0x03) {
					case 0: SetMirroringType(MirroringType::Vertical); break;
					case 1: SetMirroringType(MirroringType::Horizontal); break;
					case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
					case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
				}
				_useChrForNametables = (value & 0x10) == 0x10;
				UpdateNametables();
				break;
			case 0xF000: 
				bool externalPrg = (value & 0x08) == 0;
				if(externalPrg && GetPRGPageCount() > 8) {
					_usingExternalRom = true;
					_externalPage = 0x08 | ((value & 0x07) % (GetPRGPageCount() - 0x08));
					SelectPRGPage(0, _externalPage);
				} else {
					_usingExternalRom = false;
					SelectPRGPage(0, value & 0x07);
				}

				_prgRamEnabled = (value & 0x10) == 0x10;
				UpdateState();

				break;
		}
	}
};