#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_245 : public MMC3
{
protected:
	virtual void UpdateState()
	{
		uint8_t orValue = _registers[0] & 0x01 ? 0x40 : 0x00;

		_registers[6] = (_registers[6] & 0x3F) | orValue;
		_registers[7] = (_registers[7] & 0x3F) | orValue;

		MMC3::UpdateState();

		if(HasChrRam()) {
			if(_chrMode) {
				SelectChrPage4x(0, 4);
				SelectChrPage4x(1, 0);
			} else {
				SelectChrPage4x(0, 0);
				SelectChrPage4x(1, 4);
			}
		}
	}
};