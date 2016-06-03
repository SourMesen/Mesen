#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class BF9096 : public BaseMapper
{
private:
	uint8_t _prgBlock;
	uint8_t _prgPage;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x4000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }

	void InitMapper()
	{
		_prgPage = 0;
		_prgBlock = 0;
		
		SelectPRGPage(0, 0);
		SelectPRGPage(1, 3);

		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr >= 0xC000) {
			_prgPage = value & 0x03;
		} else if(addr < 0xC000) {
			//Some cartridges appear to expect the bits for the block selection to be inverted.
			//Different wiring on board?
			_prgBlock = (value >> 3) & 0x03;
		}
		
		SelectPRGPage(0, (_prgBlock << 2) | _prgPage);
		SelectPRGPage(1, (_prgBlock << 2) | 3);
	}

	virtual void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);
		Stream(_prgBlock, _prgPage);
	}
};