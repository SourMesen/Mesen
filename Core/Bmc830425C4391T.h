#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Bmc830425C4391T : public BaseMapper
{
private:
	uint8_t _innerReg;
	uint8_t _outerReg;
	uint8_t _prgMode;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x4000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		_innerReg = 0;
		_outerReg = 0;
		_prgMode = 0;

		SelectCHRPage(0, 0);
		UpdateState();
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_innerReg, _outerReg, _prgMode);
	}

	void UpdateState()
	{
		if(_prgMode) {
			//UNROM mode
			SelectPRGPage(0, (_innerReg & 0x07) | (_outerReg << 3));
			SelectPRGPage(1, 0x07 | (_outerReg << 3));
		} else {
			//UOROM mode
			SelectPRGPage(0, _innerReg | (_outerReg << 3));
			SelectPRGPage(1, 0x0F | (_outerReg << 3));
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		_innerReg = value & 0x0F;
		if((addr & 0xFFE0) == 0xF0E0) {
			_outerReg = addr & 0x0F;
			_prgMode = (addr >> 4) & 0x01;
		}
		UpdateState();
	}
};
