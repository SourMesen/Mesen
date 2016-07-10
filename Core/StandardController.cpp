#include "stdafx.h"
#include "StandardController.h"
#include "ControlManager.h"
#include "PPU.h"
#include "EmulationSettings.h"
#include "ArkanoidController.h"

void StandardController::StreamState(bool saving)
{
	BaseControlDevice::StreamState(saving);
	
	SnapshotInfo additionalController{ _additionalController.get() };
	Stream(_stateBuffer, _stateBufferFamicom, additionalController);
}

uint8_t StandardController::GetButtonState()
{
	ButtonState state;

	if(!EmulationSettings::CheckFlag(EmulationFlags::InBackground) || EmulationSettings::CheckFlag(EmulationFlags::AllowBackgroundInput)) {
		for(size_t i = 0, len = _keyMappings.size(); i < len; i++) {
			KeyMapping keyMapping = _keyMappings[i];

			state.A |= ControlManager::IsKeyPressed(keyMapping.A);
			state.B |= ControlManager::IsKeyPressed(keyMapping.B);
			state.Select |= ControlManager::IsKeyPressed(keyMapping.Select);
			state.Start |= ControlManager::IsKeyPressed(keyMapping.Start);
			state.Up |= ControlManager::IsKeyPressed(keyMapping.Up);
			state.Down |= ControlManager::IsKeyPressed(keyMapping.Down);
			state.Left |= ControlManager::IsKeyPressed(keyMapping.Left);
			state.Right |= ControlManager::IsKeyPressed(keyMapping.Right);

			//Turbo buttons - need to be applied for at least 2 reads in a row (some games require this)
			uint8_t turboFreq = 1 << (4 - _turboSpeed);
			bool turboOn = (uint8_t)(PPU::GetFrameCount() % turboFreq) < turboFreq / 2;
			if(turboOn) {
				state.A |= ControlManager::IsKeyPressed(keyMapping.TurboA);
				state.B |= ControlManager::IsKeyPressed(keyMapping.TurboB);
				state.Start |= ControlManager::IsKeyPressed(keyMapping.TurboStart);
				state.Select |= ControlManager::IsKeyPressed(keyMapping.TurboSelect);
			}
		}

		if(!EmulationSettings::CheckFlag(EmulationFlags::AllowInvalidInput)) {
			if(state.Up && state.Down) {
				state.Down = false;
			}
			if(state.Left && state.Right) {
				state.Right = false;
			}
		}
	}

	return state.ToByte();
}

uint32_t StandardController::GetNetPlayState()
{
	return GetButtonState();
}

uint8_t StandardController::GetPortOutput()
{
	uint8_t returnValue = _stateBuffer & 0x01;
	_stateBuffer >>= 1;

	if(_famiconDevice && _additionalController) {
		if(_hasZapper) {
			returnValue |= _additionalController->GetPortOutput();
		} else if(_hasArkanoidController) {
			returnValue |= std::dynamic_pointer_cast<ArkanoidController>(_additionalController)->GetExpansionPortOutput(_port);
		} else {
			returnValue |= (_stateBufferFamicom & 0x01) << 1;
			_stateBufferFamicom >>= 1;
			_stateBuffer |= 0x800000;
		}
	}

	//"All subsequent reads will return D=1 on an authentic controller but may return D=0 on third party controllers."
	_stateBuffer |= 0x800000;

	return returnValue;
}

void StandardController::RefreshStateBuffer()
{
	_stateBuffer = GetControlState();
	if(_additionalController && !_hasZapper) {
		//Next 8 bits = Gamepad 3/4
		if(_famiconDevice) {
			//Four player adapter (Famicom)
			if(_hasArkanoidController) {
				_additionalController->RefreshStateBuffer();
			} else {
				_stateBufferFamicom = _additionalController->GetControlState();
				_stateBufferFamicom |= 0xFFFF00;
			}
		} else {
			//Four-score adapter (NES)
			_stateBuffer |= _additionalController->GetControlState() << 8;

			//Last 8 bits = signature
			//Signature for port 0 = 0x10, reversed bit order => 0x08
			//Signature for port 1 = 0x20, reversed bit order => 0x04
			_stateBuffer |= (GetPort() == 0 ? 0x08 : 0x04) << 16;
		}
	} else {
		//"All subsequent reads will return D=1 on an authentic controller but may return D=0 on third party controllers."
		_stateBuffer |= 0xFFFF00;
	}
}

uint8_t StandardController::RefreshState()
{
	return GetButtonState();
}

void StandardController::AddAdditionalController(shared_ptr<BaseControlDevice> controller)
{
	_hasZapper = false;
	_hasArkanoidController = false;

	if(std::dynamic_pointer_cast<Zapper>(controller)) {
		_hasZapper = true;
	} else if(std::dynamic_pointer_cast<ArkanoidController>(controller)) {
		_hasArkanoidController = true;
	}
	_additionalController = controller;
}

uint32_t StandardController::GetInternalState()
{
	return _stateBuffer;
}

void StandardController::SetInternalState(uint32_t state)
{
	_stateBuffer = state;
}