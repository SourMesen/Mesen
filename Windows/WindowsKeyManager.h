#pragma once

#include "stdafx.h"
#include <unordered_map>
#include "../Core/IKeyManager.h"
#include "../Utilities/Timer.h"
#include "XInputManager.h"
#include "DirectInputManager.h"

struct KeyDefinition {
	string name;
	uint32_t keyCode;
	string description;
	string extDescription;
};

class WindowsKeyManager : public IKeyManager
{
	private:
		HWND _hWnd;
		vector<uint32_t> _keyState;
		unique_ptr<DirectInputManager> _directInput;
		unique_ptr<XInputManager> _xInput;
		std::unordered_map<uint32_t, string> _keyNames;
		std::unordered_map<uint32_t, string> _keyExtendedNames;
		std::unordered_map<string, uint32_t> _keyCodes;
		
		std::thread _updateDeviceThread;
		atomic<bool> _stopUpdateDeviceThread = false;

		void StartUpdateDeviceThread();

	public:
		WindowsKeyManager(HWND hWnd);
		~WindowsKeyManager();

		void RefreshState();
		bool IsKeyPressed(uint32_t key);
		bool IsMouseButtonPressed(MouseButton button);
		uint32_t GetPressedKey();
		string GetKeyName(uint32_t key);
		uint32_t GetKeyCode(string keyName);

		void SetKeyState(uint16_t scanCode, bool state);
		void ResetKeyState();

		void UpdateDevices();
};
