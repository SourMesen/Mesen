#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

//Missing flashing support + only tested on 1 game demo
class UnRom512 : public BaseMapper
{
private:
	bool _enableMirroringBit;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x4000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }
	virtual uint16_t RegisterStartAddress() override { return 0x8000; }
	virtual uint16_t RegisterEndAddress() override { return 0xFFFF; }
	virtual uint32_t GetChrRamSize() override { return 0x8000; }
	virtual bool HasBusConflicts() override { return !HasBattery(); }

	void InitMapper() override
	{
		SelectPRGPage(1, -1);
		if(IsNes20()) {
			_enableMirroringBit = GetMirroringType() == MirroringType::ScreenAOnly;			
		} else {
			_enableMirroringBit = GetMirroringType() == MirroringType::FourScreens;
		}
	}

	void SetDefaultNametables(uint8_t* nametableA, uint8_t* nametableB) override
	{
		BaseMapper::SetDefaultNametables(nametableA, nametableB);
		if(GetMirroringType() == MirroringType::FourScreens && _chrRam && _chrRamSize >= 0x8000) {
			//InfiniteNesLives four-screen mirroring variation, last 8kb of CHR RAM is always mapped to 0x2000-0x3FFF (0x3EFF due to palette)
			//This "breaks" the "UNROM512_4screen_test" test ROM - was the ROM actually tested on this board? Seems to contradict hardware specs
			SetPpuMemoryMapping(0x2000, 0x3FFF, _chrRam + 0x6000);
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(!HasBattery() || addr >= 0xC000) {
			SelectPRGPage(0, value & 0x1F);
			SelectCHRPage(0, (value >> 5) & 0x03);

			if(_enableMirroringBit) {
				SetMirroringType(value & 0x80 ? MirroringType::ScreenBOnly : MirroringType::ScreenAOnly);
			}
		} else {
			//Unimplemented (flash process)
		}
	}
};