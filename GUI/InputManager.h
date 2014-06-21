#pragma once

#include "stdafx.h"
#include "..\Core\IControlDevice.h"

class InputManager : public IControlDevice
{
	private:
		bool IsKeyPressed(int key);

	public:
		ButtonState GetButtonState();
};