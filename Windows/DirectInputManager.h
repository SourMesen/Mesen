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
	static bool _needToUpdate;
	static LPDIRECTINPUT8 _directInput;
	static vector<DirectInputData> _joysticks;
	static std::vector<GUID> _xinputDeviceGuids;
	static std::vector<GUID> _directInputDeviceGuids;

	bool Initialize();
	bool UpdateInputState(DirectInputData& joystick);
	static bool ProcessDevice(const DIDEVICEINSTANCE* pdidInstance, bool checkOnly);
	static bool IsXInputDevice(const GUID* pGuidProductFromDirectInput);
	static int __stdcall NeedToUpdateCallback(const DIDEVICEINSTANCE* pdidInstance, void* pContext);
	static int __stdcall EnumJoysticksCallback(const DIDEVICEINSTANCE* pdidInstance, void* pContext);
	static int __stdcall EnumObjectsCallback(const DIDEVICEOBJECTINSTANCE* pdidoi, void* pContext);

public:
	DirectInputManager(HWND window);
	~DirectInputManager();

	void RefreshState();
	bool NeedToUpdate();
	bool UpdateDeviceList();
	int GetJoystickCount();
	bool IsPressed(int port, int button);
};
