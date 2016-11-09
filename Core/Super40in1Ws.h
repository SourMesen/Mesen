#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Super40in1Ws : public BaseMapper
{
private:
	bool _regLock;

protected:
	uint16_t GetPRGPageSize() { return 0x4000; }
	uint16_t GetCHRPageSize() { return 0x2000; }
	uint16_t RegisterStartAddress() { return 0x6000; }
	uint16_t RegisterEndAddress() { return 0x6FFF; }

	void InitMapper() override
	{
		_regLock = false;
		WriteRegister(0x6000, 0);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_regLock);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(!_regLock) {
			if(addr & 0x01) {
				SelectCHRPage(0, value);
			} else {
				_regLock = (value & 0x20) == 0x20;

				SelectPRGPage(0, value & ~(~value >> 3 & 0x01));
				SelectPRGPage(1, value | (~value >> 3 & 0x01));
				SetMirroringType((value & 0x10) ? MirroringType::Horizontal : MirroringType::Vertical);
			}
		}
	}
};