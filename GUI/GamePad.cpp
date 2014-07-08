#include "stdafx.h"
#include "GamePad.h"

GamePad::GamePad()
{
}

bool GamePad::RefreshState()
{
	int controllerId = -1;

	for(DWORD i = 0; i < XUSER_MAX_COUNT && controllerId == -1; i++) {
		ZeroMemory(&_state, sizeof(XINPUT_STATE));

		if(XInputGetState(i, &_state) == ERROR_SUCCESS) {
			controllerId = i;
		}
	}

	return controllerId != -1;
}

bool GamePad::IsPressed(WORD button)
{
	RefreshState();
	if(button == XINPUT_GAMEPAD_DPAD_LEFT && _state.Gamepad.sThumbLX < -XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
		_state.Gamepad.wButtons |= button;
	} else if(button == XINPUT_GAMEPAD_DPAD_RIGHT && _state.Gamepad.sThumbLX > XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
		_state.Gamepad.wButtons |= button;
	} else if(button == XINPUT_GAMEPAD_DPAD_UP && _state.Gamepad.sThumbLY > XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
		_state.Gamepad.wButtons |= button;
	} else if(button == XINPUT_GAMEPAD_DPAD_DOWN && _state.Gamepad.sThumbLY < -XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
		_state.Gamepad.wButtons |= button;
	}
	return (_state.Gamepad.wButtons & button) != 0;
}
