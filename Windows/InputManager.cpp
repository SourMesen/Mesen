#include "stdafx.h"
#include "InputManager.h"
#include "../Core/ControlManager.h"

InputManager::InputManager(HWND hWnd, uint8_t port)
{
	_hWnd = hWnd;
	_port = port;
	ControlManager::RegisterControlDevice(this, port);
}

InputManager::~InputManager()
{
	ControlManager::UnregisterControlDevice(this);
}

bool InputManager::IsKeyPressed(int key)
{
	return (GetAsyncKeyState(key) & 0x8000) == 0x8000;
}

bool InputManager::WindowHasFocus()
{
	return GetForegroundWindow() == _hWnd;
}

ButtonState InputManager::GetButtonState()
{
	ButtonState state;

	if(WindowHasFocus()) {
		if(_port == 0) {
			state.A = IsKeyPressed('A') || _gamePad.IsPressed(XINPUT_GAMEPAD_A);
			state.B = IsKeyPressed('S') || _gamePad.IsPressed(XINPUT_GAMEPAD_X);
			state.Select = IsKeyPressed('W') || _gamePad.IsPressed(XINPUT_GAMEPAD_BACK);
			state.Start = IsKeyPressed('Q') || _gamePad.IsPressed(XINPUT_GAMEPAD_START);
			state.Up = IsKeyPressed(VK_UP) || _gamePad.IsPressed(XINPUT_GAMEPAD_DPAD_UP);
			state.Down = IsKeyPressed(VK_DOWN) || _gamePad.IsPressed(XINPUT_GAMEPAD_DPAD_DOWN);
			state.Left = IsKeyPressed(VK_LEFT) || _gamePad.IsPressed(XINPUT_GAMEPAD_DPAD_LEFT);
			state.Right = IsKeyPressed(VK_RIGHT) || _gamePad.IsPressed(XINPUT_GAMEPAD_DPAD_RIGHT);
		} else {
			state.A = IsKeyPressed('D');
			state.B = IsKeyPressed('F');
			state.Select = IsKeyPressed('R');
			state.Start = IsKeyPressed('E');
			state.Up = IsKeyPressed('U');
			state.Down = IsKeyPressed('J');
			state.Left = IsKeyPressed('H');
			state.Right = IsKeyPressed('K');
		}
	}

	return state;
}
