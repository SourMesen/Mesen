#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class DaouInfosys : public BaseMapper
{
private:
	uint8_t _chrLow[8];
	uint8_t _chrHigh[8];

protected:
	virtual uint16_t RegisterStartAddress() override { return 0xC000; }
	virtual uint16_t RegisterEndAddress() override { return 0xC014; }
	virtual uint16_t GetPRGPageSize() override { return 0x4000; }
	virtual uint16_t GetCHRPageSize() override { return 0x400; }

	void InitMapper() override
	{
		memset(_chrLow, 0, sizeof(_chrLow));
		memset(_chrHigh, 0, sizeof(_chrHigh));
		SelectPRGPage(1, -1);
		SetMirroringType(MirroringType::ScreenAOnly);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);

		ArrayInfo<uint8_t> chrLow{ _chrLow, 8 };
		ArrayInfo<uint8_t> chrHigh{ _chrHigh, 8 };
		Stream(chrLow, chrHigh);

		if(!saving) {
			UpdateChrBanks();
		}
	}

	void UpdateChrBanks()
	{
		for(int i = 0; i < 8; i++) {
			SelectCHRPage(i, (_chrHigh[i] << 8) | _chrLow[i]);
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr) {
			case 0xC000: case 0xC001: case 0xC002: case 0xC003:
			case 0xC004: case 0xC005: case 0xC006: case 0xC007:
			case 0xC008: case 0xC009: case 0xC00A: case 0xC00B:
			case 0xC00C: case 0xC00D: case 0xC00E: case 0xC00F:
			{
				uint8_t bank = (addr & 0x03) + ((addr >= 0xC008) ? 4 : 0);
				uint8_t* arr = (addr & 0x04) ? _chrHigh : _chrLow;
				arr[bank] = value;
				UpdateChrBanks();
				break;
			}

			case 0xC010:
				SelectPRGPage(0, value);
				break;

			case 0xC014:
				SetMirroringType((value & 0x01) == 0x01 ? MirroringType::Horizontal : MirroringType::Vertical);
				break;
		}

	}
};