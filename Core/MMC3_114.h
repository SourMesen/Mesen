#pragma once

#include "stdafx.h"
#include "MMC3.h"

class MMC3_114 : public MMC3
{
private:
	const uint8_t _security[8] = { 0,3,1,5,6,7,2,4 };
	uint8_t _exRegs[2];

protected:
	virtual uint16_t RegisterStartAddress() { return 0x5000; }
	virtual bool ForceMmc3RevAIrqs() { return true; }

	void InitMapper()
	{
		MMC3::InitMapper();
		_exRegs[0] = _exRegs[1] = 0;
	}

	virtual void StreamState(bool saving)
	{
		MMC3::StreamState(saving);
		Stream(_exRegs[0], _exRegs[1]);
	}

	virtual void UpdatePrgMapping()
	{
		if(_exRegs[0] & 0x80) {
			SelectPrgPage2x(0, _exRegs[0] & 0x1F);
			SelectPrgPage2x(1, _exRegs[0] & 0x1F);
		} else {
			MMC3::UpdatePrgMapping();
		}
	}

	virtual void UpdateMirroring()
	{
		//See $8000 writes below
	}

	virtual void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xE000) {
			case 0x4000: case 0x6000:
				if((addr & 0x07) == 0x00) {
					_exRegs[0] = value;
					UpdatePrgMapping();
				}
				break;

			case 0x8000:
				SetMirroringType(value & 0x01 ? MirroringType::Horizontal : MirroringType::Vertical);
				break;

			case 0xA000:
				value = (value & 0xC0) | _security[value & 0x07];
				_exRegs[1] = 1;
				MMC3::WriteRegister(0x8000, value);
				break;

			case 0xC000:
				if(_exRegs[1]) {
					_exRegs[1] = 0;
					MMC3::WriteRegister(0x8001, value);
				}
				break;

			case 0xE000:
				if(value > 0) {
					MMC3::WriteRegister(0xE001, value);
					MMC3::WriteRegister(0xC000, value);
					MMC3::WriteRegister(0xC001, value);
				} else {
					MMC3::WriteRegister(0xE000, value);
				}
				break;
		}
	}
};