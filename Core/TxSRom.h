#pragma once
#include "stdafx.h"
#include "MMC3.h"

//TKSROM and TLSROM
class TxSRom : public MMC3
{
protected:
	virtual void StreamState(bool saving)
	{
		MMC3::StreamState(saving);
	}

	void UpdateMirroring()
	{
		//This is disabled, 8001 writes are used to setup mirroring instead
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if((addr & 0xE001) == 0x8001) {
			uint8_t* nametable = GetNametable(value >> 7);
				
			if(GetChrMode() == 0) {
				switch(GetCurrentRegister()) {
					case 0:
						SetPpuMemoryMapping(0x2000, 0x23FF, nametable);
						SetPpuMemoryMapping(0x2400, 0x27FF, nametable);
						break;

					case 1:
						SetPpuMemoryMapping(0x2800, 0x2BFF, nametable);
						SetPpuMemoryMapping(0x2C00, 0x2FFF, nametable);
						break;
				}
			} else {
				switch(GetCurrentRegister()) {
					case 2: SetPpuMemoryMapping(0x2000, 0x23FF, nametable); break;
					case 3: SetPpuMemoryMapping(0x2400, 0x27FF, nametable); break;
					case 4: SetPpuMemoryMapping(0x2800, 0x2BFF, nametable); break;
					case 5: SetPpuMemoryMapping(0x2C00, 0x2FFF, nametable); break;
				}
			}
		}
		MMC3::WriteRegister(addr, value);
	}
};