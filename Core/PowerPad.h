#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"

class PowerPad : public BaseControlDevice
{
private:
	uint8_t _stateBufferL;
	uint8_t _stateBufferH;
	bool _useSideA = false;

protected:
	string GetKeyNames() override
	{
		return "123456789ABC";
	}

	void InternalSetStateFromInput() override
	{
		for(KeyMapping keyMapping : _keyMappings) {
			for(int i = 0; i < 3; i++) {
				for(int j = 0; j < 4; j++) {
					if(_useSideA) {
						//Invert the order of each row
						SetPressedState(i*4+j, keyMapping.PowerPadButtons[i*4+(3-j)]);
					} else {
						SetPressedState(i*4+j, keyMapping.PowerPadButtons[i*4+j]);
					}
				}
			}
		}
	}

	void RefreshStateBuffer() override
	{
		uint8_t pressedKeys[12] = {};
		for(int i = 0; i < 12; i++) {
			pressedKeys[i] |= IsPressed(i) ? 1 : 0;
		}

		//"Serial data from buttons 2, 1, 5, 9, 6, 10, 11, 7"
		_stateBufferL = pressedKeys[1] | (pressedKeys[0] << 1) | (pressedKeys[4] << 2) | (pressedKeys[8] << 3) | (pressedKeys[5] << 4) | (pressedKeys[9] << 5) | (pressedKeys[10] << 6) | (pressedKeys[6] << 7);

		//"Serial data from buttons 4, 3, 12, 8 (following 4 bits read as H=1)"
		_stateBufferH = pressedKeys[3] | (pressedKeys[2] << 1) | (pressedKeys[11] << 2) | (pressedKeys[7] << 3) | 0xF0;
	}

	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);
		Stream(_stateBufferL, _stateBufferH);
	}

public:
	PowerPad(shared_ptr<Console> console, uint8_t port, KeyMappingSet keyMappings) : BaseControlDevice(console, port, keyMappings)
	{
		_useSideA = keyMappings.PowerpadUseSideA;
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		uint8_t output = 0;
		if(IsCurrentPort(addr)) {
			StrobeProcessRead();

			output = ((_stateBufferH & 0x01) << 4) | ((_stateBufferL & 0x01) << 3);
			_stateBufferL >>= 1;
			_stateBufferH >>= 1;

			_stateBufferL |= 0x80;
			_stateBufferH |= 0x80;
		}
		return output;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		StrobeProcessWrite(value);
	}
};