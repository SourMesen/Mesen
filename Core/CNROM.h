#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class CNROM : public BaseMapper
{
	protected:
		virtual uint32_t GetPRGPageSize() { return 0x8000; }
		virtual uint32_t GetCHRPageSize() {	return 0x2000; }

		void InitMapper() 
		{
			SelectPRGPage(0, 0);
			SelectCHRPage(0, 0);
		}

	public:		
		void WriteRAM(uint16_t addr, uint8_t value)
		{
			SelectCHRPage(0, value);
		}
};