#pragma once

#include "stdafx.h"
#include "../Core/IKeyManager.h"
#include "../Utilities/Timer.h"
#include "XInputManager.h"
#include "DirectInputManager.h"

struct KeyDefinition {
	string name;
	uint32_t keyCode;
	string description;
};

class WindowsKeyManager : public IKeyManager
{
	private:
		Timer _timer;
		HWND _hWnd;
		unique_ptr<DirectInputManager> _directInput;
		unique_ptr<XInputManager> _xInput;

	public:
		WindowsKeyManager(HWND hWnd);
		~WindowsKeyManager();

		void RefreshState();
		bool IsKeyPressed(uint32_t key);
		bool IsMouseButtonPressed(MouseButton button);
		uint32_t GetPressedKey();
		string GetKeyName(uint32_t key);
		uint32_t GetKeyCode(string keyName);

		void UpdateDevices();
};
