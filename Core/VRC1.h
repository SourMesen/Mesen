#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class VRC1 : public BaseMapper
{
private:
	uint8_t _chrBanks[2];
	
	void UpdateChrBanks()
	{
		SelectCHRPage(0, _chrBanks[0]);
		SelectCHRPage(1, _chrBanks[1]);
	}

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x1000; }

	void InitMapper()
	{
		SelectPRGPage(3, -1);
	}

	virtual void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(ArrayInfo<uint8_t>{ _chrBanks, 2 });
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xF000) {
			case 0x8000: SelectPRGPage(0, value & 0x0F); break;
			
			case 0x9000:
				if(GetMirroringType() != MirroringType::FourScreens) {
					//"The mirroring bit is ignored if the cartridge is wired for 4-screen VRAM, as is typical for Vs. System games using the VRC1."
					SetMirroringType((value & 0x01) == 0x01 ? MirroringType::Horizontal : MirroringType::Vertical);
				}
				_chrBanks[0] = (_chrBanks[0] & 0x0F) | ((value & 0x02) << 3);
				_chrBanks[1] = (_chrBanks[1] & 0x0F) | ((value & 0x04) << 2);
				UpdateChrBanks();
				break;

			case 0xA000: SelectPRGPage(1, value & 0x0F); break;
			case 0xC000: SelectPRGPage(2, value & 0x0F); break;

			case 0xE000:
				_chrBanks[0] = (_chrBanks[0] & 0x10) | (value & 0x0F);
				UpdateChrBanks();
				break;

			case 0xF000:
				_chrBanks[1] = (_chrBanks[1] & 0x10) | (value & 0x0F);
				UpdateChrBanks();
				break;
		}
	}
};