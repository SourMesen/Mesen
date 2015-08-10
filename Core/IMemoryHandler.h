#pragma once

#include "stdafx.h"

enum class MemoryOperation
{
	Read = 0,
	Write = 1,
};

class MemoryRanges
{
	private:
		vector<uint16_t> _ramReadAddresses;
		vector<uint16_t> _ramWriteAddresses;

	public:
		vector<uint16_t>* GetRAMReadAddresses() { return &_ramReadAddresses;	}
		vector<uint16_t>* GetRAMWriteAddresses() { return &_ramWriteAddresses;	}

		void AddHandler(MemoryOperation operation, uint16_t start, uint16_t end = 0)
		{
			if(end == 0) {
				end = start;
			}

			vector<uint16_t> *addresses;			
			if(operation == MemoryOperation::Read) {
				addresses = &_ramReadAddresses;
			} else {
				addresses = &_ramWriteAddresses;
			}

			for(uint32_t i = start; i <= end; i++) {
				addresses->push_back((uint16_t)i);
			}
		}
};

class IMemoryHandler
{
public:
	virtual void GetMemoryRanges(MemoryRanges &ranges) = 0;
	virtual uint8_t ReadRAM(uint16_t addr) = 0;
	virtual void WriteRAM(uint16_t addr, uint8_t value) = 0;
};