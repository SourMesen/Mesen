#pragma once

#include "stdafx.h"
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
	void StreamState(bool saving) override
	{
		Stream(_lastCycle, _cyclesDown);
	}

	A12StateChange UpdateVramAddress(uint16_t addr, uint32_t frameCycle)
	{
		A12StateChange result = A12StateChange::None;

		if(_cyclesDown > 0) {
			if(_lastCycle > frameCycle) {
				//We changed frames
				_cyclesDown += (89342 - _lastCycle) + frameCycle;
			} else {
				_cyclesDown += (frameCycle - _lastCycle);
			}
		}

		if((addr & 0x1000) == 0) {
			if(_cyclesDown == 0) {
				_cyclesDown = 1;
				result = A12StateChange::Fall;
			}
		} else if(addr & 0x1000) {
			if(_cyclesDown > 10) {
				result = A12StateChange::Rise;
			}
			_cyclesDown = 0;
		}
		_lastCycle = frameCycle;

		return result;
	}
};