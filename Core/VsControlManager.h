#pragma once
#include "stdafx.h"
#include "ControlManager.h"
#include "CPU.h"
#include <assert.h>

class VsControlManager : public ControlManager
{
private:
	static VsControlManager *_instance;
	uint8_t _prgChrSelectBit;
	uint8_t _dipSwitches = 0;
	bool _serviceButton = false;
	bool _coinInserted[2] = { };
	int32_t _coinInsertCycle[2] = { };

private:
	void UpdateCoinInsertedFlags()
	{
		int32_t cycle = CPU::GetCycleCount();
		for(int i = 0; i < 2; i++) {
			if(_coinInserted[i] && cycle - _coinInsertCycle[i] > 120000) {
				_coinInserted[i] = false;
			}
		}
	}

public:
	VsControlManager()
	{
		_instance = this;
	}

	~VsControlManager()
	{
		if(_instance == this) {
			_instance = nullptr;
		}
	}

	static VsControlManager* GetInstance()
	{
		return _instance;
	}

	void InsertCoin(uint8_t port)
	{
		assert(port < 2);

		_coinInsertCycle[port] = CPU::GetCycleCount();
		_coinInserted[port] = true;
	}

	void SetDipSwitches(uint8_t dipSwitches)
	{
		_dipSwitches = dipSwitches;
	}

	void SetServiceButtonState(bool pushed)
	{
		_serviceButton = pushed;
	}

	void GetMemoryRanges(MemoryRanges &ranges)
	{
		ControlManager::GetMemoryRanges(ranges);
		ranges.AddHandler(MemoryOperation::Read, 0x4020, 0x5FFF);
		ranges.AddHandler(MemoryOperation::Write, 0x4020, 0x5FFF);
	}

	uint8_t GetPrgChrSelectBit()
	{
		return _prgChrSelectBit;
	}

	uint8_t ReadRAM(uint16_t addr)
	{
		UpdateCoinInsertedFlags();

		uint8_t value = 0;
		switch(addr) {
			case 0x4016: value = GetPortValue(1); break;
			case 0x4017: value = GetPortValue(0); break;
		}

		value &= 0x01;

		if(addr == 0x4016) {
			if(_coinInserted[0]) {
				value |= 0x20;
			}
			if(_coinInserted[1]) {
				value |= 0x40;
			}
			if(_serviceButton) {
				value |= 0x04;
			}

			value |= ((_dipSwitches & 0x01) ? 0x08 : 0x00);
			value |= ((_dipSwitches & 0x02) ? 0x10 : 0x00);
		} else if(addr == 0x4017) {
			value |= ((_dipSwitches & 0x04) ? 0x04 : 0x00);
			value |= ((_dipSwitches & 0x08) ? 0x08 : 0x00);
			value |= ((_dipSwitches & 0x10) ? 0x10 : 0x00);
			value |= ((_dipSwitches & 0x20) ? 0x20 : 0x00);
			value |= ((_dipSwitches & 0x40) ? 0x40 : 0x00);
			value |= ((_dipSwitches & 0x80) ? 0x80 : 0x00);
		}

		return value;
	}
	
	void WriteRAM(uint16_t addr, uint8_t value)
	{
		ControlManager::WriteRAM(addr, value);

		if(addr == 0x4016) {
			_prgChrSelectBit = (value >> 2) & 0x01;
		}
	}
};