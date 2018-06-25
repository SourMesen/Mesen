#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"

template<size_t Mask>
class InternalRamHandler : public IMemoryHandler
{
private:
	uint8_t *_internalRam;

public:
	void SetInternalRam(uint8_t* internalRam)
	{
		_internalRam = internalRam;
	}

	void GetMemoryRanges(MemoryRanges &ranges) override
	{
		ranges.SetAllowOverride();
		ranges.AddHandler(MemoryOperation::Any, 0, 0x1FFF);
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		return _internalRam[addr & Mask];
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		_internalRam[addr & Mask] = value;
	}
};
