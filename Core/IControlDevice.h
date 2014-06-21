#pragma once

#include "stdafx.h"

struct ButtonState
{
	bool Up;
	bool Down;
	bool Left;
	bool Right;

	bool A;
	bool B;

	bool Select;
	bool Start;
};

class IControlDevice
{
	public:
		virtual ButtonState GetButtonState() = 0;
};