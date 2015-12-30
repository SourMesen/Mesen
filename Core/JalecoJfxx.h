#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class JalecoJfxx : public BaseMapper
{
private:
	bool _orderedBits;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x8000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	virtual uint16_t RegisterStartAddress() { return 0x6000; }
	virtual uint16_t RegisterEndAddress() { return 0x7FFF; }

	void InitMapper()
	{
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(_orderedBits) {
			//Mapper 101
			SelectCHRPage(0, value);
		} else {
			//Mapper 87
			SelectCHRPage(0, ((value & 0x01) << 1) | ((value & 0x02) >> 1));
		}
	}

public:
	JalecoJfxx(bool orderedBits) : _orderedBits(orderedBits)
	{
	}
};