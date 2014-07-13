#pragma once

#include "stdafx.h"
#include "..\Core\IControlDevice.h"
#include "GamePad.h"

class InputManager : public IControlDevice
{
	private:
		GamePad _gamePad;
		HWND _hWnd;
		uint8_t _port;

		bool IsKeyPressed(int key);
		bool WindowHasFocus();

	public:
		InputManager(HWND hWnd, uint8_t port);
		ButtonState GetButtonState();
};