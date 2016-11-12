#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Unl43272 : public BaseMapper
{
private:
	uint16_t _lastAddr;

protected:
	uint16_t GetPRGPageSize() override { return 0x8000; }
	uint16_t GetCHRPageSize() override { return 0x2000; }
	bool AllowRegisterRead() override { return true; }

	void InitMapper() override
	{
		SelectCHRPage(0, 0);
		SetMirroringType(MirroringType::Horizontal);
	}

	void Reset(bool softReset) override
	{
		BaseMapper::Reset(softReset);
		WriteRegister(0x8081, 0);
	}

	void StreamState(bool saving) override 
	{
		BaseMapper::StreamState(saving);
		Stream(_lastAddr);
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		return InternalReadRam(_lastAddr & 0x400 ? (addr & 0xFE) : addr);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		_lastAddr = addr;

		if((addr & 0x81) == 0x81) {
			SelectPRGPage(0, (addr & 0x38) >> 3);
		}
	}
};