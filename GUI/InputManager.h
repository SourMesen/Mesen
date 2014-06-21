#pragma once

#include "stdafx.h"
#include "..\Core\IControlDevice.h"
#include "GamePad.h"

class InputManager : public IControlDevice
{
	private:
		GamePad _gamePad;

		bool IsKeyPressed(int key);

	public:
		ButtonState GetButtonState();
};