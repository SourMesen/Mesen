#pragma once

#include "stdafx.h"

class IMemoryHandler
{
public:
	virtual vector<std::array<uint16_t, 2>> GetRAMAddresses() = 0;
	virtual vector<std::array<uint16_t, 2>> GetVRAMAddresses() { return{}; }
	virtual uint8_t ReadRAM(uint16_t addr) = 0;
	virtual void WriteRAM(uint16_t addr, uint8_t value) = 0;
	virtual uint8_t ReadVRAM(uint16_t addr) { return 0; }
	virtual void WriteVRAM(uint16_t addr, uint8_t value) { }
};