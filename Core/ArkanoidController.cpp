#include "stdafx.h"
#include "ArkanoidController.h"
#include "ControlManager.h"
#include "PPU.h"
#include "GameServerConnection.h"

void ArkanoidController::StreamState(bool saving)
{
	BaseControlDevice::StreamState(saving);
	Stream<uint32_t>(_stateBuffer);
	Stream<bool>(_buttonPressed);
	Stream<int32_t>(_xPosition);
}

uint8_t ArkanoidController::GetPortOutput()
{
	return GetControlState();
}

bool ArkanoidController::IsButtonPressed()
{
	if(!EmulationSettings::CheckFlag(EmulationFlags::InBackground) || EmulationSettings::CheckFlag(EmulationFlags::AllowBackgroundInput)) {
		if(ControlManager::IsMouseButtonPressed(MouseButton::LeftButton)) {
			return true;
		}
	}

	return false;
}

uint32_t ArkanoidController::GetNetPlayState()
{
	//Used by netplay
	uint32_t state = ControlManager::GetMousePosition().X;

	if(IsButtonPressed()) {
		state |= 0x40000000;
	}

	return state;
}

uint8_t ArkanoidController::ProcessNetPlayState(uint32_t netplayState)
{
	_xPosition = netplayState & 0xFF;
	_buttonPressed = ((netplayState >> 30) & 0x01) == 0x01;

	return RefreshState();
}

void ArkanoidController::RefreshStateBuffer()
{
	const uint8_t validRange = 0xF4 - 0x54;	
	if(!GameServerConnection::GetNetPlayDevice(_port)) {
		_xPosition = ControlManager::GetMousePosition().X;
	}

	_xPosition -= 48;
	if(_xPosition < 0) {
		_xPosition = 0;
	} else if(_xPosition >= 160) {
		_xPosition = 159;
	}

	_stateBuffer = 0x54 + (uint32_t)(((double)_xPosition / 159) * validRange);
}

uint8_t ArkanoidController::RefreshState()
{
	if(!GameServerConnection::GetNetPlayDevice(_port)) {
		_buttonPressed = IsButtonPressed();
	}

	uint8_t output = ((~_stateBuffer) >> 3) & 0x10;
	_stateBuffer <<= 1;

	if(_buttonPressed) {
		output |= 0x08;
	}
	
	return output;
}

uint8_t ArkanoidController::GetExpansionPortOutput(uint8_t port)
{
	uint8_t output = 0;
	if(port == 0) {
		//Fire button is on port 1
		if(!GameServerConnection::GetNetPlayDevice(_port)) {
			_buttonPressed = IsButtonPressed();
		}

		if(_buttonPressed) {
			output |= 0x02;
		}
	} else if(port == 1) {
		//Serial data is on port 2
		uint8_t arkanoidBits = GetPortOutput();
		output |= (arkanoidBits >> 3) & 0x02;
	}
	return output;
}