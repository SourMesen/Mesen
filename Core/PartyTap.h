#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"

class PartyTap : public BaseControlDevice
{
private:
	uint8_t _stateBuffer = 0;
	uint8_t _readCount = 0;
	bool _enabled = false;

protected:
	enum Buttons { B1 = 0, B2, B3, B4, B5, B6 };

	string GetKeyNames() override
	{
		return "123456";
	}

	void InternalSetStateFromInput() override
	{
		for(KeyMapping keyMapping : _keyMappings) {
			for(int i = 0; i < 6; i++) {
				SetPressedState(i, keyMapping.PartyTapButtons[i]);
			}
		}
	}

	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);
		Stream(_stateBuffer, _readCount, _enabled);
	}

public:
	PartyTap(shared_ptr<Console> console, KeyMappingSet keyMappings) : BaseControlDevice(console, BaseControlDevice::ExpDevicePort, keyMappings)
	{
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		if(addr == 0x4017) {
			if(_readCount < 2) {
				uint8_t value = (_stateBuffer & 0x7) << 2;
				_stateBuffer >>= 3;
				StrobeProcessRead();
				_readCount++;
				return value;
			} else {
				//"After 1st/2nd reads,	a detection value can be read : $4017 & $1C == $14"
				return 0x14;
			}
		}
		return 0;
	}

	void RefreshStateBuffer() override
	{
		_readCount = 0;
		_stateBuffer =
			IsPressed(PartyTap::Buttons::B1) ? 1 : 0 |
			IsPressed(PartyTap::Buttons::B2) ? 2 : 0 |
			IsPressed(PartyTap::Buttons::B3) ? 4 : 0 |
			IsPressed(PartyTap::Buttons::B4) ? 8 : 0 |
			IsPressed(PartyTap::Buttons::B5) ? 16 : 0 |
			IsPressed(PartyTap::Buttons::B6) ? 32 : 0;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		StrobeProcessWrite(value);
	}
};