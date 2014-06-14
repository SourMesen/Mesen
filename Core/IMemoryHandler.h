#pragma once

#include "stdafx.h"

class IMemoryHandler
{
	public:
		virtual std::array<int, 2> GetIOAddresses() = 0;
		virtual uint8_t MemoryRead(uint16_t addr) = 0;
		virtual void MemoryWrite(uint16_t addr, uint8_t value) = 0;
};