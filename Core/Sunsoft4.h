#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"

class Sunsoft4 : public BaseMapper
{
private:
	uint8_t _ntRegs[2];
	bool _useChrForNametables;

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
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x800; }

	void InitMapper()
	{
		_useChrForNametables = false;
		_ntRegs[0] = _ntRegs[1] = 0;

		//Bank 0's initial state is undefined, but some roms expect it to be the first page
		SelectPRGPage(0, 0);

		SelectPRGPage(1, -1);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		
		Stream(_ntRegs[0], _ntRegs[1], _useChrForNametables);

		if(!saving) {
			UpdateNametables();
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value)
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
				SelectPRGPage(0, value & 0x0F); 
				//_prgRamEnabled = (value & 0x10) == 0x10;
				break;
		}
	}
};