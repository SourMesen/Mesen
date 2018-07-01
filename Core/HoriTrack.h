#pragma once
#include "stdafx.h"
#include "StandardController.h"
#include "KeyManager.h"

class HoriTrack : public StandardController
{
protected:
	bool HasCoordinates() override { return true; }
	
	void InternalSetStateFromInput() override
	{
		StandardController::InternalSetStateFromInput();
		SetPressedState(StandardController::Buttons::A, KeyManager::IsMouseButtonPressed(MouseButton::LeftButton));
		SetPressedState(StandardController::Buttons::B, KeyManager::IsMouseButtonPressed(MouseButton::RightButton));
		SetMovement(KeyManager::GetMouseMovement(EmulationSettings::GetMouseSensitivity(MouseDevice::HoriTrack)));
	}

public:
	HoriTrack(shared_ptr<Console> console,  KeyMappingSet keyMappings) : StandardController(console, BaseControlDevice::ExpDevicePort, keyMappings)
	{
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		uint8_t output = 0;
		if(addr == 0x4016) {
			output = (_stateBuffer & 0x01) << 1;
			_stateBuffer >>= 1;
			StrobeProcessRead();
		}
		return output;
	}
	
	void RefreshStateBuffer() override
	{
		MouseMovement mov = GetMovement();

		mov.dx = std::max(-8, std::min((int)mov.dx, 7));
		mov.dy = std::max(-8, std::min((int)mov.dy, 7));

		mov.dx = ((mov.dx & 0x08) >> 3) | ((mov.dx & 0x04) >> 1) | ((mov.dx & 0x02) << 1) | ((mov.dx & 0x01) << 3);
		mov.dy = ((mov.dy & 0x08) >> 3) | ((mov.dy & 0x04) >> 1) | ((mov.dy & 0x02) << 1) | ((mov.dy & 0x01) << 3);
		
		uint8_t byte1 = (~mov.dy & 0x0F) | ((~mov.dx & 0x0F) << 4);
		uint8_t byte2 = 0x09;

		StandardController::RefreshStateBuffer();
		_stateBuffer = (_stateBuffer & 0xFF) | (byte1 << 8) | (byte2 << 16);
	}
};