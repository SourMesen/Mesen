#include "stdafx.h"
#include "GamePad.h"

GamePad::GamePad()
{
	for(int i = 0; i < XUSER_MAX_COUNT; i++) {
		_gamePadStates.push_back(shared_ptr<XINPUT_STATE>(new XINPUT_STATE()));
	}
}

void GamePad::RefreshState()
{
	for(DWORD i = 0; i < XUSER_MAX_COUNT; i++) {
		if(_gamePadStates[i] != nullptr) {
			ZeroMemory(_gamePadStates[i].get(), sizeof(XINPUT_STATE));
			if(XInputGetState(i, _gamePadStates[i].get()) != ERROR_SUCCESS) {
				//XInputGetState is incredibly slow when no controller is plugged in
				//TODO: Periodically detect if a controller has been plugged in to allow controllers to be plugged in after the emu is started
				_gamePadStates[i] = nullptr;
			}
		}
	}
}

bool GamePad::IsPressed(uint8_t gamepadPort, uint8_t button)
{
	if(_gamePadStates[gamepadPort] != nullptr) {
		XINPUT_GAMEPAD &gamepad = _gamePadStates[gamepadPort]->Gamepad;
		if(button <= 16) {
			WORD xinputButton = 1 << (button - 1);
			if(xinputButton == XINPUT_GAMEPAD_DPAD_LEFT && gamepad.sThumbLX < -XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
				gamepad.wButtons |= xinputButton;
			} else if(xinputButton == XINPUT_GAMEPAD_DPAD_RIGHT && gamepad.sThumbLX > XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
				gamepad.wButtons |= xinputButton;
			} else if(xinputButton == XINPUT_GAMEPAD_DPAD_UP && gamepad.sThumbLY > XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
				gamepad.wButtons |= xinputButton;
			} else if(xinputButton == XINPUT_GAMEPAD_DPAD_DOWN && gamepad.sThumbLY < -XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE) {
				gamepad.wButtons |= xinputButton;
			}

			return (_gamePadStates[gamepadPort]->Gamepad.wButtons & xinputButton) != 0;
		} else {
			switch(button) {
				case 17: return gamepad.bLeftTrigger > XINPUT_GAMEPAD_TRIGGER_THRESHOLD;
				case 18: return gamepad.bRightTrigger > XINPUT_GAMEPAD_TRIGGER_THRESHOLD;
				case 19: return gamepad.sThumbRY > XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE;
				case 20: return gamepad.sThumbRY < -XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE;
				case 21: return gamepad.sThumbRX < -XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE;
				case 22: return gamepad.sThumbRX > XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE;
			}
		}
	}
	return false;
}
