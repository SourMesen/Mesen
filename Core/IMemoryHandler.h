#pragma once

#include "stdafx.h"

enum class MemoryType
{
	RAM = 0,
	VRAM = 1,
};

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
		vector<uint16_t> _vramReadAddresses;
		vector<uint16_t> _vramWriteAddresses;

	public:
		vector<uint16_t>* GetRAMReadAddresses() { return &_ramReadAddresses;	}
		vector<uint16_t>* GetRAMWriteAddresses() { return &_ramWriteAddresses;	}
		vector<uint16_t>* GetVRAMReadAddresses() { return &_vramReadAddresses;	}
		vector<uint16_t>* GetVRAMWriteAddresses() { return &_vramWriteAddresses;	}

		void AddHandler(MemoryType type, MemoryOperation operation, uint16_t start, uint16_t end = 0)
		{
			if(end == 0) {
				end = start;
			}

			vector<uint16_t> *addresses;			
			if(type == MemoryType::RAM) {
				if(operation == MemoryOperation::Read) {
					addresses = &_ramReadAddresses;
				} else {
					addresses = &_ramWriteAddresses;
				}
			} else {
				if(operation == MemoryOperation::Read) {
					addresses = &_vramReadAddresses;
				} else {
					addresses = &_vramWriteAddresses;
				}
			}
			for(int i = start; i <= end; i++) {
				addresses->push_back(i);
			}
		}
};

class IMemoryHandler
{
public:
	virtual void GetMemoryRanges(MemoryRanges &ranges) = 0;
	virtual uint8_t ReadRAM(uint16_t addr) = 0;
	virtual void WriteRAM(uint16_t addr, uint8_t value) = 0;
	virtual uint8_t ReadVRAM(uint16_t addr) { throw exception("Operation not implemented"); }
	virtual void WriteVRAM(uint16_t addr, uint8_t value) { throw exception("Operation not implemented"); }
};