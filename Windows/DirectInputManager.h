#pragma once
#include "stdafx.h"
#include <dinput.h>
#include "../Utilities/SimpleLock.h"

struct DirectInputData
{
	LPDIRECTINPUTDEVICE8 joystick;
	DIJOYSTATE2 state;
	DIJOYSTATE2 defaultState;
};

class DirectInputManager
{
private:
	HWND _hWnd;
	static LPDIRECTINPUT8 _directInput;
	static vector<DirectInputData> _joysticks;

	bool Initialize();
	bool UpdateInputState(DirectInputData& joystick);
	static int __stdcall EnumJoysticksCallback(const DIDEVICEINSTANCE* pdidInstance, void* pContext);
	static int __stdcall EnumObjectsCallback(const DIDEVICEOBJECTINSTANCE* pdidoi, void* pContext);

public:
	DirectInputManager(HWND window);
	~DirectInputManager();

	void RefreshState();
	bool UpdateDeviceList();
	int GetJoystickCount();
	bool IsPressed(int port, int button);
};
