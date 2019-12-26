#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"
#include "Console.h"

class StandardController : public BaseControlDevice
{
private:
	bool _microphoneEnabled = false;
	uint32_t _turboSpeed = 0;

protected:
	uint32_t _stateBuffer = 0;

	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);
		Stream(_stateBuffer, _microphoneEnabled);
	}

	string GetKeyNames() override
	{
		string keys = "UDLRSsBA";
		if(_microphoneEnabled) {
			keys += "M";
		}
		return keys;
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

			uint8_t turboFreq = 1 << (4 - _turboSpeed);
			bool turboOn = (uint8_t)(_console->GetFrameCount() % turboFreq) < turboFreq / 2;
			if(turboOn) {
				SetPressedState(Buttons::A, keyMapping.TurboA);
				SetPressedState(Buttons::B, keyMapping.TurboB);
			}

			if(_microphoneEnabled && (_console->GetFrameCount() % 3) == 0) {
				SetPressedState(Buttons::Microphone, keyMapping.Microphone);
			}

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

	void RefreshStateBuffer() override
	{
		if(_console->GetSettings()->GetConsoleType() == ConsoleType::Nes && _console->GetSettings()->CheckFlag(EmulationFlags::HasFourScore)) {
			if(_port >= 2) {
				_stateBuffer = ToByte() << 8;
			} else {
				//Add some 0 bit padding to allow P3/P4 controller bits + signature bits
				_stateBuffer = (_port == 0 ? 0xFF000000 : 0xFF000000) | ToByte();
			}
		} else {
			_stateBuffer = 0xFFFFFF00 | ToByte();
		}
	}

public:
	enum Buttons { Up = 0, Down, Left, Right, Start, Select, B, A, Microphone };

	StandardController(shared_ptr<Console> console, uint8_t port, KeyMappingSet keyMappings) : BaseControlDevice(console, port, keyMappings)
	{
		_turboSpeed = keyMappings.TurboSpeed;
		_microphoneEnabled = port == 1 && _console->GetSettings()->GetConsoleType() == ConsoleType::Famicom;
	}
	
	uint8_t ToByte()
	{
		//"Button status for each controller is returned as an 8-bit report in the following order: A, B, Select, Start, Up, Down, Left, Right."
		return
			(uint8_t)IsPressed(Buttons::A) |
			((uint8_t)IsPressed(Buttons::B) << 1) |
			((uint8_t)IsPressed(Buttons::Select) << 2) |
			((uint8_t)IsPressed(Buttons::Start) << 3) |
			((uint8_t)IsPressed(Buttons::Up) << 4) |
			((uint8_t)IsPressed(Buttons::Down) << 5) |
			((uint8_t)IsPressed(Buttons::Left) << 6) |
			((uint8_t)IsPressed(Buttons::Right) << 7);
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		if(_port >= 2 && _console->IsDualSystem()) {
			//Ignore P3/P4 controllers for VS DualSystem - those are used by the slave CPU
			return 0;
		}

		uint8_t output = 0;

		if((addr == 0x4016 && (_port & 0x01) == 0) || (addr == 0x4017 && (_port & 0x01) == 1)) {
			StrobeProcessRead();
			
			output = _stateBuffer & 0x01;
			if(_port >= 2 && _console->GetSettings()->GetConsoleType() == ConsoleType::Famicom) {
				//Famicom outputs P3 & P4 on bit 1
				output <<= 1;
			}
			_stateBuffer >>= 1;

			//"All subsequent reads will return D=1 on an authentic controller but may return D=0 on third party controllers."
			_stateBuffer |= 0x80000000;
		}

		if(addr == 0x4016 && IsPressed(StandardController::Buttons::Microphone)) {
			output |= 0x04;
		}

		return output;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		StrobeProcessWrite(value);
	}
};