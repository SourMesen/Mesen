#pragma once
#include "stdafx.h"
#include "MMC3.h"

class ResetTxrom : public MMC3
{
private:
	uint8_t _resetCounter;

protected:
	void Reset(bool softReset) override
	{
		MMC3::Reset(softReset);
		if(softReset) {
			_resetCounter = (_resetCounter + 1) & 0x03;
			UpdateState();
		} else {
			_resetCounter = 0;
		}
	}

	void StreamState(bool saving) override
	{
		MMC3::StreamState(saving);
		Stream(_resetCounter);
	}

	void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType) override
	{
		page = (_resetCounter << 7) | (page & 0x7F);
		MMC3::SelectCHRPage(slot, page, memoryType);
	}

	void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType) override
	{
		page = (_resetCounter << 4) | (page & 0x0F);
		MMC3::SelectPRGPage(slot, page, memoryType);
	}
};