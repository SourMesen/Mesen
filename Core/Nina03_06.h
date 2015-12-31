#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Nina03_06 : public BaseMapper
{
private: 
	bool _multicartMode;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	virtual uint16_t RegisterStartAddress() { return 0x4100; }
	virtual uint16_t RegisterEndAddress() { return 0x5FFF; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if((addr & 0x5100) != 0) {
			if(_multicartMode) {
				//Mapper 113
				SelectPRGPage(0, (value >> 3) & 0x07);
				SelectCHRPage(0, (value & 0x07) | ((value >> 6) & 0x01));
				SetMirroringType((value & 0x80) == 0x80 ? MirroringType::Vertical : MirroringType::Horizontal);
			} else {
				SelectPRGPage(0, (value >> 3) & 0x01);
				SelectCHRPage(0, value & 0x07);
			}
		}
	}

public:
	Nina03_06(bool multicartMode) : _multicartMode(multicartMode)
	{
	}
};