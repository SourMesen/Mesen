#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "VsControlManager.h"

class VsSystem : public BaseMapper
{
private:
	uint8_t _prgChrSelectBit = false;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	virtual void InitMapper()
	{
		if(_prgSize > 0x6000) {
			SelectPRGPage(0, 0);
		}
		SelectPRGPage(1, 1);
		SelectPRGPage(2, 2);
		SelectPRGPage(3, 3);

		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);

		Stream(_prgChrSelectBit);
	}

	uint8_t ReadVRAM(uint16_t addr)
	{
		if(_prgChrSelectBit != VsControlManager::GetInstance()->GetPrgChrSelectBit()) {
			_prgChrSelectBit = VsControlManager::GetInstance()->GetPrgChrSelectBit();
			SelectCHRPage(0, _prgChrSelectBit);
		}

		return BaseMapper::ReadVRAM(addr);
	}
};
