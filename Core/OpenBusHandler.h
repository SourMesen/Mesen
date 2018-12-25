#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"

class OpenBusHandler : public IMemoryHandler
{
private:
	uint8_t _lastReadValue;

public:
	OpenBusHandler()
	{
		_lastReadValue = 0;
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		return _lastReadValue;
	}

	uint8_t PeekRAM(uint16_t addr) override
	{
		return addr >> 8; //Fake open bus for debugger
	}

	__forceinline uint8_t GetOpenBus()
	{
		return _lastReadValue;
	}

	__forceinline void SetOpenBus(uint8_t value)
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