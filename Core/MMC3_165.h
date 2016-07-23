#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_165 : public MMC3
{
private:
	bool _chrLatch[2] = { false, false };
	bool _needUpdate = false;

protected:
	virtual uint16_t GetCHRPageSize() { return 0x1000; }
	virtual uint32_t GetChrRamSize() { return 0x1000; }
	virtual uint16_t GetChrRamPageSize() { return 0x1000; }	
	
	virtual void StreamState(bool saving)
	{
		MMC3::StreamState(saving);
		Stream(_chrLatch[0], _chrLatch[1], _needUpdate);
	}

	virtual void UpdateChrMapping()
	{
		uint16_t page;
		
		for(int i = 0; i < 2; i++) {
			page = _registers[i == 0 ? (_chrLatch[0] ? 1 : 0) : (_chrLatch[1] ? 4 : 2)];
			if(page == 0) {
				SelectCHRPage(i, 0, ChrMemoryType::ChrRam);
			} else {
				SelectCHRPage(i, page >> 2, ChrMemoryType::ChrRom);
			}
		}

		_needUpdate = false;
	}

	virtual void NotifyVRAMAddressChange(uint16_t addr)
	{
		if(_needUpdate) {
			UpdateChrMapping();
		}

		//MMC2 style latch
		switch(addr & 0x2FF8) {
			case 0xFD0: case 0xFE8:
				_chrLatch[(addr >> 12) & 0x01] = ((addr & 0x08) == 0x08);
				_needUpdate = true;
				break;
		}
	}
};