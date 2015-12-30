#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class NROM : public BaseMapper
{
	protected:
		virtual uint16_t GetPRGPageSize() { return 0x4000; }
		virtual uint16_t GetCHRPageSize() {	return 0x2000; }

		virtual void InitMapper()
		{
			SelectPRGPage(0, 0);
			SelectPRGPage(1, 1);

			SelectCHRPage(0, 0);
		}
};
