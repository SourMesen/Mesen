#pragma once

#include "stdafx.h"
#include <Xinput.h>

class GamePad
{
	private:
		vector<shared_ptr<XINPUT_STATE>> _gamePadStates;

	public:
		GamePad();

		void RefreshState();
		bool IsPressed(uint8_t gamepadPort, WORD button);
};
