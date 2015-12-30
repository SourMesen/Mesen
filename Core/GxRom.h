#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class GxRom : public BaseMapper
{
	protected:
		virtual uint16_t GetPRGPageSize() { return 0x8000; }
		virtual uint16_t GetCHRPageSize() {	return 0x2000; }
		virtual uint32_t GetChrRamSize() { return 0x4000; }

		void InitMapper() 
		{
			SelectPRGPage(0, 0);
			SelectCHRPage(0, 0);
		}

		void WriteRegister(uint16_t addr, uint8_t value)
		{
			SelectPRGPage(0, (value >> 4) & 0x03);
			SelectCHRPage(0, value & 0x03);
		}
};