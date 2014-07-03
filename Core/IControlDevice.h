#pragma once

#include "stdafx.h"

struct ButtonState
{
	bool Up = false;
	bool Down = false;
	bool Left = false;
	bool Right = false;

	bool A = false;
	bool B = false;

	bool Select = false;
	bool Start = false;
};

class IControlDevice
{
	public:
		virtual ButtonState GetButtonState() = 0;
};