#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "ControlManager.h"
#include "StandardController.h"

class BandaiKaraoke : public BaseMapper
{
protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	virtual bool AllowRegisterRead() { return true; }

	void InitMapper()
	{
		AddRegisterRange(0x6000, 0x7FFF, MemoryOperation::Read);
		RemoveRegisterRange(0x8000, 0xFFFF, MemoryOperation::Read);

		SelectPRGPage(0, 0);
		SelectPRGPage(1, 0x07);
		SelectCHRPage(0, 0);
	}

	uint8_t ReadRegister(uint16_t addr)
	{
		//Microphone not implemented - always return A/B buttons as not pressed
		return 0x03;
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(value & 0x10) {
			//Select internal rom
			SelectPRGPage(0, value & 0x07);
		} else {
			//Select expansion rom
			if(_prgSize >= 0x40000) {
				SelectPRGPage(0, (value & 0x07) | 0x08);
			} else {
				//Open bus for roms that don't contain the expansion rom
				RemoveCpuMemoryMapping(0x8000, 0xBFFF);
			}
		}

		SetMirroringType(value & 0x20 ? MirroringType::Horizontal : MirroringType::Vertical);
	}
};