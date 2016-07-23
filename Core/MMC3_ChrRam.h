#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_ChrRam : public MMC3
{
private:
	uint16_t _firstRamBank;
	uint16_t _lastRamBank;
	uint16_t _chrRamSize;

protected:
	virtual uint16_t GetChrRamPageSize() { return 0x400; }
	virtual uint32_t GetChrRamSize() { return _chrRamSize * 0x400; }

	virtual void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default)
	{
		if(page >= _firstRamBank && page <= _lastRamBank) {
			memoryType = ChrMemoryType::ChrRam;
			page -= _firstRamBank;
		}

		MMC3::SelectCHRPage(slot, page, memoryType);
	}

	virtual void StreamState(bool saving)
	{
		MMC3::StreamState(saving);
		Stream(_firstRamBank, _lastRamBank, _chrRamSize);
	}

public:
	MMC3_ChrRam(uint16_t firstRamBank, uint16_t lastRamBank, uint16_t chrRamSize) : _firstRamBank(firstRamBank), _lastRamBank(lastRamBank), _chrRamSize(chrRamSize)
	{
	}
};
