#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Bs5 : public BaseMapper
{
private:
	uint8_t _dipSwitch = 0;

protected:
	uint16_t GetPRGPageSize() { return 0x2000; }
	uint16_t GetCHRPageSize() { return 0x800; }

	void InitMapper() override
	{
		for(int i = 0; i < 4; i++) {
			SelectPRGPage(i, -1);
			SelectCHRPage(i, -1);
		}
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(_dipSwitch);
	}

	void Reset(bool softReset)
	{
		if(softReset) {
			_dipSwitch = (_dipSwitch + 1) & 0x03;
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		int bank = (addr >> 10) & 0x03;
		switch(addr & 0xF000) {
			case 0x8000: 
				SelectCHRPage(bank, addr & 0x1F); 
				break;

			case 0xA000:
				if(addr & (1 << (_dipSwitch + 4))) {
					SelectPRGPage(bank, addr & 0x0F);
				}
				break;
		}
	}
};