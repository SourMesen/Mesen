#include "stdafx.h"
#include "GamePad.h"

GamePad::GamePad()
{
	for(int i = 0; i < XUSER_MAX_COUNT; i++) {
		_gamePadStates.push_back(shared_ptr<XINPUT_STATE>(new XINPUT_STATE()));
	}
}

void GamePad::RefreshState()
{
	for(DWORD i = 0; i < XUSER_MAX_COUNT; i++) {
		if(_gamePadStates[i] != nullptr) {
			ZeroMemory(_gamePadStates[i].get(), sizeof(XINPUT_STATE));
			if(XInputGetState(i, _gamePadStates[i].get()) != ERROR_SUCCESS) {
				//XInputGetState is incredibly slow when no controller is plugged in
				//TODO: Periodically detect if a controller has been plugged in to allow controllers to be plugged in after the emu is started
				_gamePadStates[i] = nullptr;
			}
		}
	}
}

bool GamePad::IsPressed(uint8_t gamepadPort, WORD button)
{
	if(_gamePadStates[gamepadPort] != nullptr) {
		if(button == XINPUT_GAMEPAD_DPAD_LEFT && _gamePadStates[gamepadPort]->Gamepad.sThumbLX < -XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
			_gamePadStates[gamepadPort]->Gamepad.wButtons |= button;
		} else if(button == XINPUT_GAMEPAD_DPAD_RIGHT && _gamePadStates[gamepadPort]->Gamepad.sThumbLX > XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
			_gamePadStates[gamepadPort]->Gamepad.wButtons |= button;
		} else if(button == XINPUT_GAMEPAD_DPAD_UP && _gamePadStates[gamepadPort]->Gamepad.sThumbLY > XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
			_gamePadStates[gamepadPort]->Gamepad.wButtons |= button;
		} else if(button == XINPUT_GAMEPAD_DPAD_DOWN && _gamePadStates[gamepadPort]->Gamepad.sThumbLY < -XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
			_gamePadStates[gamepadPort]->Gamepad.wButtons |= button;
		}
		return (_gamePadStates[gamepadPort]->Gamepad.wButtons & button) != 0;
	} else {
		return false;
	}
}
