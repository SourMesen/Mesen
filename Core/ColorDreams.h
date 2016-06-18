#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class ColorDreams : public BaseMapper
{
	protected:
		virtual uint16_t GetPRGPageSize() { return 0x8000; }
		virtual uint16_t GetCHRPageSize() {	return 0x2000; }
		virtual bool HasBusConflicts() { return true; }

		void InitMapper() 
		{
			SelectPRGPage(0, 0);
			SelectCHRPage(0, 0);
		}

		void WriteRegister(uint16_t addr, uint8_t value)
		{
			if(_mapperID == 144) {
				//"This addition means that only the ROM's least significant bit always wins bus conflicts."
				value |= (ReadRAM(addr) & 0x01);
			}

			SelectPRGPage(0, value & 0x03);
			SelectCHRPage(0, (value >> 4) & 0x0F);
		}
};