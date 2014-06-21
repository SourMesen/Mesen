#include "stdafx.h"
#include "GamePad.h"

GamePad::GamePad()
{
}

bool GamePad::Initialize()
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
	Initialize();
	return (_state.Gamepad.wButtons & button) != 0;
}
