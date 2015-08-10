#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class UNROM : public BaseMapper
{
	protected:
		virtual uint16_t GetPRGPageSize() { return 0x4000; }
		virtual uint16_t GetCHRPageSize() {	return 0x2000; }

		void InitMapper() 
		{
			//First and last PRG page
			SelectPRGPage(0, 0);
			SelectPRGPage(1, -1);

			SelectCHRPage(0, 0);
		}

		void WriteRegister(uint16_t addr, uint8_t value)
		{
			SelectPRGPage(0, value);
		}
};