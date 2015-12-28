#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class CpRom : public BaseMapper
{
	protected:
		virtual uint16_t GetPRGPageSize() { return 0x8000; }
		virtual uint16_t GetCHRPageSize() {	return 0x1000; }
		virtual uint32_t GetChrRamSize() { return 0x4000; }

		void InitMapper() 
		{
			SelectPRGPage(0, 0);
			SelectCHRPage(0, 0);
			SetMirroringType(MirroringType::Vertical);
		}

		void WriteRegister(uint16_t addr, uint8_t value)
		{
			if(addr >= 0x8000) {
				SelectCHRPage(1, value & 0x03);
			}
		}
};