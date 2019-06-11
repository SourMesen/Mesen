#pragma once
#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Bmc8157 : public BaseMapper
{
private:
	uint16_t _lastAddr;

protected:
	uint32_t GetDipSwitchCount() override { return 1; }
	uint16_t GetPRGPageSize() override { return 0x4000; }
	uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		_lastAddr = 0;
		UpdateState();
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_lastAddr);

		if(!saving) {
			UpdateState();
		}
	}

	void UpdateState()
	{
		uint8_t innerPrg0 = (_lastAddr >> 2) & 0x07;
		uint8_t innerPrg1 = ((_lastAddr >> 7) & 0x01) | ((_lastAddr >> 8) & 0x02);
		uint8_t outer128Prg = (_lastAddr >> 5) & 0x03;
		uint8_t outer512Prg = (_lastAddr >> 8) & 0x01;

		int baseBank;
		if(innerPrg1 == 0) {
			baseBank = 0;
		} else if(innerPrg1 == 1) {
			baseBank = innerPrg0;
		} else {
			baseBank = 7;
		}

		if(outer512Prg && _prgSize <= 1024 * 512 && GetDipSwitches() != 0) {
			RemoveCpuMemoryMapping(0x8000, 0xFFFF);
		} else {
			SelectPRGPage(0, (outer512Prg << 6) | (outer128Prg << 3) | innerPrg0);
			SelectPRGPage(1, (outer512Prg << 6) | (outer128Prg << 3) | baseBank);
			SetMirroringType(_lastAddr & 0x02 ? MirroringType::Horizontal : MirroringType::Vertical);
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		_lastAddr = addr;
		UpdateState();
	}
};