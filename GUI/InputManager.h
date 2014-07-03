#pragma once

#include "stdafx.h"
#include "..\Core\IControlDevice.h"
#include "GamePad.h"

class InputManager : public IControlDevice
{
	private:
		GamePad _gamePad;
		HWND _hWnd;

		bool IsKeyPressed(int key);
		bool WindowHasFocus();

	public:
		InputManager(HWND hWnd);
		ButtonState GetButtonState();
};