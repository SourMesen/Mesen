#pragma once

#include "stdafx.h"
#include <Xinput.h>

class XInputManager
{
	private:
		vector<shared_ptr<XINPUT_STATE>> _gamePadStates;

	public:
		XInputManager();

		void UpdateDeviceList();
		void RefreshState();
		bool IsPressed(uint8_t gamepadPort, uint8_t button);
};
