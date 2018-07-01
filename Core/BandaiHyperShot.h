#pragma once
#include "stdafx.h"
#include "StandardController.h"
#include "Zapper.h"
#include "IKeyManager.h"
#include "KeyManager.h"

class BandaiHyperShot : public StandardController
{
private:
	uint32_t _stateBuffer = 0;
	shared_ptr<Console> _console;

protected:
	enum ZapperButtons { Fire = 9 };
	
	bool HasCoordinates() override { return true; }

	string GetKeyNames() override
	{
		return StandardController::GetKeyNames() + "F";
	}

	void InternalSetStateFromInput() override
	{
		StandardController::InternalSetStateFromInput();

		if(EmulationSettings::InputEnabled()) {
			SetPressedState(ZapperButtons::Fire, KeyManager::IsMouseButtonPressed(MouseButton::LeftButton));

			MousePosition pos = KeyManager::GetMousePosition();
			if(KeyManager::IsMouseButtonPressed(MouseButton::RightButton)) {
				pos.X = -1;
				pos.Y = -1;
			}
			SetCoordinates(pos);
		}
	}

	bool IsLightFound()
	{
		return Zapper::StaticIsLightFound(GetCoordinates(), _console);
	}

	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);
		Stream(_stateBuffer);
	}

public:
	BandaiHyperShot(shared_ptr<Console> console, KeyMappingSet keyMappings) : StandardController(console, BaseControlDevice::ExpDevicePort, keyMappings)
	{
		_console = console;
	}

	void RefreshStateBuffer() override
	{
		_stateBuffer = (uint32_t)ToByte();
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		if(addr == 0x4016) {
			uint8_t output = (_stateBuffer & 0x01) << 1;
			_stateBuffer >>= 1;
			StrobeProcessRead();
			return output;
		} else {
			return (IsLightFound() ? 0 : 0x08) | (IsPressed(BandaiHyperShot::ZapperButtons::Fire) ? 0x10 : 0x00);
		}
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		StrobeProcessWrite(value);
	}
};