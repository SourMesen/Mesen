#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"
#include "Console.h"

class MMC5MemoryHandler : public IMemoryHandler
{
	Console* _console;
	uint8_t _ppuRegs[8];

public:
	MMC5MemoryHandler(Console* console)
	{
		_console = console;
		memset(_ppuRegs, 0, sizeof(_ppuRegs));
	}

	uint8_t GetReg(uint16_t addr)
	{
		return _ppuRegs[addr & 0x07];
	}

	void GetMemoryRanges(MemoryRanges& ranges) override {}
	uint8_t ReadRAM(uint16_t addr) override { return 0; }

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		_console->GetPpu()->WriteRAM(addr, value);
		_ppuRegs[addr & 0x07] = value;
	}
};
