#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"

class SnesController : public BaseControlDevice
{
private:
	uint32_t _stateBuffer = 0;

protected:
	enum Buttons { A = 0, B, Select, Start, Up, Down, Left, Right, Y, X, L, R };

	string GetKeyNames() override
	{
		return "ABSTUDLRYXLR";
	}

	void InternalSetStateFromInput() override
	{
		for(KeyMapping keyMapping : _keyMappings) {
			SetPressedState(Buttons::A, keyMapping.A);
			SetPressedState(Buttons::B, keyMapping.B);
			SetPressedState(Buttons::Start, keyMapping.Start);
			SetPressedState(Buttons::Select, keyMapping.Select);
			SetPressedState(Buttons::Up, keyMapping.Up);
			SetPressedState(Buttons::Down, keyMapping.Down);
			SetPressedState(Buttons::Left, keyMapping.Left);
			SetPressedState(Buttons::Right, keyMapping.Right);
			SetPressedState(Buttons::X, keyMapping.TurboA);
			SetPressedState(Buttons::Y, keyMapping.TurboB);
			SetPressedState(Buttons::L, keyMapping.LButton);
			SetPressedState(Buttons::R, keyMapping.RButton);

			if(!_console->GetSettings()->CheckFlag(EmulationFlags::AllowInvalidInput)) {
				//If both U+D or L+R are pressed at the same time, act as if neither is pressed
				if(IsPressed(Buttons::Up) && IsPressed(Buttons::Down)) {
					ClearBit(Buttons::Down);
					ClearBit(Buttons::Up);
				}
				if(IsPressed(Buttons::Left) && IsPressed(Buttons::Right)) {
					ClearBit(Buttons::Left);
					ClearBit(Buttons::Right);
				}
			}
		}
	}

	uint16_t ToByte()
	{
		//"A Super NES controller returns a 16-bit report in a similar order: B, Y, Select, Start, Up, Down, Left, Right, A, X, L, R, then four 0 bits."

		return
			(uint8_t)IsPressed(Buttons::B) |
			((uint8_t)IsPressed(Buttons::Y) << 1) |
			((uint8_t)IsPressed(Buttons::Select) << 2) |
			((uint8_t)IsPressed(Buttons::Start) << 3) |
			((uint8_t)IsPressed(Buttons::Up) << 4) |
			((uint8_t)IsPressed(Buttons::Down) << 5) |
			((uint8_t)IsPressed(Buttons::Left) << 6) |
			((uint8_t)IsPressed(Buttons::Right) << 7) |
			((uint8_t)IsPressed(Buttons::A) << 8) |
			((uint8_t)IsPressed(Buttons::X) << 9) |
			((uint8_t)IsPressed(Buttons::L) << 10) |
			((uint8_t)IsPressed(Buttons::R) << 11);
	}

	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);
		Stream(_stateBuffer);
	}

	void RefreshStateBuffer() override
	{
		_stateBuffer = (uint32_t)ToByte();
	}

public:
	SnesController(shared_ptr<Console> console, uint8_t port, KeyMappingSet keyMappings) : BaseControlDevice(console, port, keyMappings)
	{
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		uint8_t output = 0;

		if(IsCurrentPort(addr)) {
			StrobeProcessRead();

			output = _stateBuffer & 0x01;
			_stateBuffer >>= 1;

			//"All subsequent reads will return D=1 on an authentic controller but may return D=0 on third party controllers."
			_stateBuffer |= 0x8000;
		}

		return output;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		StrobeProcessWrite(value);
	}
};