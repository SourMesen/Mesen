#pragma once

#include "stdafx.h"
#include "Types.h"

class MemoryRanges
{
	private:
		vector<uint16_t> _ramReadAddresses;
		vector<uint16_t> _ramWriteAddresses;
		bool _allowOverride = false;

	public:
		vector<uint16_t>* GetRAMReadAddresses() { return &_ramReadAddresses;	}
		vector<uint16_t>* GetRAMWriteAddresses() { return &_ramWriteAddresses;	}

		bool GetAllowOverride()
		{
			return _allowOverride;
		}

		void SetAllowOverride()
		{
			_allowOverride = true;
		}

		void AddHandler(MemoryOperation operation, uint16_t start, uint16_t end = 0)
		{
			if(end == 0) {
				end = start;
			}

			if(operation == MemoryOperation::Read || operation == MemoryOperation::Any) {
				for(uint32_t i = start; i <= end; i++) {
					_ramReadAddresses.push_back((uint16_t)i);
				}
			}
			
			if(operation == MemoryOperation::Write || operation == MemoryOperation::Any) {
				for(uint32_t i = start; i <= end; i++) {
					_ramWriteAddresses.push_back((uint16_t)i);
				}
			}
		}
};

class IMemoryHandler
{
public:
	virtual void GetMemoryRanges(MemoryRanges &ranges) = 0;
	virtual uint8_t ReadRAM(uint16_t addr) = 0;
	virtual void WriteRAM(uint16_t addr, uint8_t value) = 0;
	virtual uint8_t PeekRAM(uint16_t addr) { return 0; }

	virtual ~IMemoryHandler() {}
};