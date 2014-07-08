#pragma once

#include "stdafx.h"
#include <Xinput.h>

class GamePad
{
	private:
		XINPUT_STATE _state;

		bool RefreshState();

	public:
		GamePad();

		bool IsPressed(WORD button);
};
