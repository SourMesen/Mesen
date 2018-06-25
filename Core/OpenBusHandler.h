#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"

class OpenBusHandler : public IMemoryHandler
{
private:
	static uint8_t _lastReadValue;

public:
	OpenBusHandler()
	{
		_lastReadValue = 0;
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		return _lastReadValue;
	}

	__forceinline static uint8_t GetOpenBus()
	{
		return _lastReadValue;
	}

	__forceinline static void SetOpenBus(uint8_t value)
	{
		_lastReadValue = value;
	}

	void GetMemoryRanges(MemoryRanges & ranges) override
	{
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
	}
};