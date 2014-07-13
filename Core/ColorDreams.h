#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class ColorDreams : public BaseMapper
{
	protected:
		virtual uint32_t GetPRGPageSize() { return 0x8000; }
		virtual uint32_t GetCHRPageSize() {	return 0x2000; }

		void InitMapper() 
		{
			SelectPRGPage(0, 0);
			SelectCHRPage(0, 0);
		}

		void WriteRegister(uint16_t addr, uint8_t value)
		{
			SelectPRGPage(0, value & 0x03);
			SelectCHRPage(0, (value >> 4) & 0x0F);
		}
};