#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Bmc63 : public BaseMapper
{
private:
	bool _openBus;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		WriteRegister(0x8000, 0);
	}

	void Reset(bool softReset)
	{
		_openBus = false;
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(_openBus);
		if(!saving && _openBus) {
			RemoveCpuMemoryMapping(0x8000, 0xBFFF);
		}
	}
	
	void WriteRegister(uint16_t addr, uint8_t value)
	{
		_openBus = ((addr & 0x0300) == 0x0300);

		if(_openBus) {
			RemoveCpuMemoryMapping(0x8000, 0xBFFF);
		} else {
			SelectPRGPage(0, (addr >> 1 & 0x1FC) | ((addr & 0x2) ? 0x0 : (addr >> 1 & 0x2) | 0x0));
			SelectPRGPage(1, (addr >> 1 & 0x1FC) | ((addr & 0x2) ? 0x1 : (addr >> 1 & 0x2) | 0x1));
		}
		SelectPRGPage(2, (addr >> 1 & 0x1FC) | ((addr & 0x2) ? 0x2 : (addr >> 1 & 0x2) | 0x0));
		SelectPRGPage(3, (addr & 0x800) ? ((addr & 0x07C) | ((addr & 0x06) ? 0x03 : 0x01)) : ((addr >> 1 & 0x01FC) | ((addr & 0x02) ? 0x03 : ((addr >> 1 & 0x02) | 0x01))));

		SetMirroringType(addr & 0x01 ? MirroringType::Horizontal : MirroringType::Vertical);
	}
};