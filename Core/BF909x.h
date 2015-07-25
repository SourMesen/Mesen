#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class BF909x : public BaseMapper
{
private:
	bool _bf9097Mode = false;  //Auto-detect for firehawk

protected:
	virtual uint32_t GetPRGPageSize() { return 0x4000; }
	virtual uint32_t GetCHRPageSize() {	return 0x2000; }

	void InitMapper() 
	{
		//First and last PRG page
		SelectPRGPage(0, 0);
		SelectPRGPage(1, -1);

		SelectCHRPage(0, 0);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr == 0x9000) {
			//Firehawk uses $9000 to change mirroring
			_bf9097Mode = true;
		}

		if(addr >= 0xC000 || !_bf9097Mode) {
			SelectPRGPage(0, value);
		} else if(addr < 0xC000) {
			_mirroringType = (value & 0x10) ? MirroringType::ScreenAOnly : MirroringType::ScreenBOnly;
		}
	}
};