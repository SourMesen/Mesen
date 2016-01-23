#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"

class SunSoft4 : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x800; }

	void InitMapper()
	{
		SelectPRGPage(1, -1);
	}

	virtual void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xF000) {
			case 0x8000: SelectCHRPage(0, value); break;
			case 0x9000: SelectCHRPage(1, value); break;
			case 0xA000: SelectCHRPage(2, value); break;
			case 0xB000: SelectCHRPage(3, value); break;
			case 0xC000:
				//TODO
				break;
			case 0xD000:
				//TODO
				break;
			case 0xE000:
				switch(value & 0x03) {
					case 0: SetMirroringType(MirroringType::Vertical); break;
					case 1: SetMirroringType(MirroringType::Horizontal); break;
					case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
					case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
				}
				break;
			case 0xF000: 
				SelectPRGPage(0, value & 0x0F); 
				//_prgRamEnabled = (value & 0x10) == 0x10;
				break;
		}
	}
};