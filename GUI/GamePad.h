#pragma once

#include "stdafx.h"
#include <Xinput.h>

class GamePad
{
	private:
		XINPUT_STATE _state;

		bool Initialize();

	public:
		GamePad();

		bool IsPressed(WORD button);
};
