#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class NROM : public BaseMapper
{
	protected:
		virtual uint32_t GetPRGPageSize() { return 0x4000; }
		virtual uint32_t GetCHRPageSize() {	return 0x2000; }

		virtual void InitMapper()
		{
			if(_prgSize == 0x4000) {
				SelectPRGPage(0, 0);
				SelectPRGPage(1, 0);
			} else {
				SelectPRGPage(0, 0);
				SelectPRGPage(1, 1);
			}

			SelectCHRPage(0, 0);
		}
};
