#pragma once

#include "stdafx.h"
#include "CPU.h"
#include "PPU.h"
#include "Snapshotable.h"

enum class A12StateChange
{
	None = 0,
	Rise = 1,
	Fall = 2
};

class A12Watcher : public Snapshotable
{
private:
	uint32_t _lastCycle = 0;
	uint32_t _cyclesDown = 0;

public:
	void StreamState(bool saving)
	{
		Stream(_lastCycle, _cyclesDown);
	}

	A12StateChange UpdateVramAddress(uint16_t addr)
	{
		A12StateChange result = A12StateChange::None;
		uint32_t cycle = PPU::GetFrameCycle();

		if((addr & 0x1000) == 0) {
			if(_cyclesDown == 0) {
				_cyclesDown = 1;
			} else {
				if(_lastCycle > cycle) {
					//We changed frames
					_cyclesDown += (89342 - _lastCycle) + cycle;
				} else {
					_cyclesDown += (cycle - _lastCycle);
				}
			}
			result = A12StateChange::Fall;
		} else if(addr & 0x1000) {
			if(_cyclesDown > 8) {
				result = A12StateChange::Rise;
			}
			_cyclesDown = 0;
		}
		_lastCycle = cycle;

		return result;
	}
};