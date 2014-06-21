#include "stdafx.h"
#include "InputManager.h"

bool InputManager::IsKeyPressed(int key)
{
	return (GetAsyncKeyState(key) & 0x8000) == 0x8000;
}

ButtonState InputManager::GetButtonState()
{
	ButtonState state;
	state.A = IsKeyPressed('A') || _gamePad.IsPressed(XINPUT_GAMEPAD_X);
	state.B = IsKeyPressed('S') || _gamePad.IsPressed(XINPUT_GAMEPAD_A);
	state.Select = IsKeyPressed('W') || _gamePad.IsPressed(XINPUT_GAMEPAD_BACK);
	state.Start = IsKeyPressed('Q') || _gamePad.IsPressed(XINPUT_GAMEPAD_START);
	state.Up = IsKeyPressed(VK_UP) || _gamePad.IsPressed(XINPUT_GAMEPAD_DPAD_UP);
	state.Down = IsKeyPressed(VK_DOWN) || _gamePad.IsPressed(XINPUT_GAMEPAD_DPAD_DOWN);
	state.Left = IsKeyPressed(VK_LEFT) || _gamePad.IsPressed(XINPUT_GAMEPAD_DPAD_LEFT);
	state.Right = IsKeyPressed(VK_RIGHT) || _gamePad.IsPressed(XINPUT_GAMEPAD_DPAD_RIGHT);

	return state;
}
