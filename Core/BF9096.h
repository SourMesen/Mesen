#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class BF9096 : public BaseMapper
{
private:
	uint8_t _prgBlock;
	uint8_t _prgPage;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x4000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		_prgPage = 0;
		_prgBlock = 0;
		
		SelectPRGPage(0, 0);
		SelectPRGPage(1, 3);

		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr >= 0xC000) {
			_prgPage = value & 0x03;
		} else if(addr < 0xC000) {
			if(_romInfo.SubMapperID == 1) {
				//"232: 1 Aladdin Deck Enhancer"
				//"Aladdin Deck Enhancer variation.Swap the bits of the outer bank number."
				//But this seems to match the Pegasus 4-in-1 behavior?  Wiki wrong?
				_prgBlock = ((value >> 4) & 0x01) | ((value >> 2) & 0x02);
			} else {
				_prgBlock = (value >> 3) & 0x03;
			}

		}
		
		SelectPRGPage(0, (_prgBlock << 2) | _prgPage);
		SelectPRGPage(1, (_prgBlock << 2) | 3);
	}

	virtual void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_prgBlock, _prgPage);
	}
};