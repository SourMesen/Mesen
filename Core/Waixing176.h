#pragma once
#include "BaseMapper.h"

class Waixing176 : public BaseMapper
{
private:
	bool _registersEnabled;

protected:
	uint16_t GetPRGPageSize() override { return 0x2000; }
	uint16_t GetCHRPageSize() override { return 0x2000; }
	uint16_t RegisterStartAddress() override { return 0x5000; }
	uint16_t RegisterEndAddress() override { return 0x5FFF; }

	void InitMapper() override
	{
		_registersEnabled = false;

		SelectPRGPage(0, 0);
		SelectPRGPage(1, 1);
		SelectPRGPage(2, (GetPRGPageCount() - 2) & 0x3F);
		SelectPRGPage(3, (GetPRGPageCount() - 1) & 0x3F);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_registersEnabled);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr) {
			case 0x5001:
				if(_registersEnabled) {
					SelectPrgPage4x(0, value*4);
				}
				break;

			case 0x5010:
				if(value == 0x24) {
					_registersEnabled = true;
				}
				break;

			case 0x5011:
				if(_registersEnabled) {
					SelectPrgPage4x(0, (value >> 1) * 4);
				}
				break;

			case 0x5FF1:
				SelectPrgPage4x(0, (value >> 1) * 4);
				break;

			case 0x5FF2:
				if(!HasChrRam()) {
					SelectCHRPage(0, value);
				}
				break;
		}
	}
};