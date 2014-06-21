#include "stdafx.h"
#include "InputManager.h"

bool InputManager::IsKeyPressed(int key)
{
	return (GetAsyncKeyState(key) & 0x8000) == 0x8000;
}

ButtonState InputManager::GetButtonState()
{
	ButtonState state;
	state.A = IsKeyPressed('A');
	state.B = IsKeyPressed('S');
	state.Select = IsKeyPressed('W');
	state.Start = IsKeyPressed('Q');
	state.Up = IsKeyPressed(VK_UP);
	state.Down = IsKeyPressed(VK_DOWN);
	state.Left = IsKeyPressed(VK_LEFT);
	state.Right = IsKeyPressed(VK_RIGHT);

	return state;
}