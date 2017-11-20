#pragma once
#include "stdafx.h"
#include "Zapper.h"

class VsZapper : public Zapper
{
private:
	uint32_t _stateBuffer = 0;

protected:
	void StreamState(bool saving) override
	{
		Zapper::StreamState(saving);
		Stream(_stateBuffer);
	}

	void RefreshStateBuffer() override
	{
		_stateBuffer = 0x10 | (IsLightFound() ? 0x40 : 0x00) | (IsPressed(Zapper::Buttons::Fire) ? 0x80 : 0x00);
	}

public:
	VsZapper(uint8_t port) : Zapper(port)
	{
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		if(IsCurrentPort(addr)) {
			uint8_t returnValue = _stateBuffer & 0x01;
			_stateBuffer >>= 1;
			StrobeProcessRead();
			return returnValue;
		}

		return 0;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		StrobeProcessWrite(value);
	}
};