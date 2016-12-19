#pragma once
#include <unordered_map>
#include <vector>
#include <thread>
#include "../Core/IKeyManager.h"

class LinuxGameController;

struct KeyDefinition {
	string name;
	uint32_t keyCode;
	string description;
	string extDescription;
};

class LinuxKeyManager : public IKeyManager
{
private:
	std::vector<shared_ptr<LinuxGameController>> _controllers;
	bool _keyState[0x200];
	bool _mouseState[0x03];
	std::unordered_map<uint32_t, string> _keyNames;
	std::unordered_map<string, uint32_t> _keyCodes;	

	std::thread _updateDeviceThread;
	atomic<bool> _stopUpdateDeviceThread; 

	void StartUpdateDeviceThread();

public:
	LinuxKeyManager();
	virtual ~LinuxKeyManager();

	void RefreshState();
	bool IsKeyPressed(uint32_t key);
	bool IsMouseButtonPressed(MouseButton button);
	uint32_t GetPressedKey();
	string GetKeyName(uint32_t key);
	uint32_t GetKeyCode(string keyName);

	void UpdateDevices();
	void SetKeyState(uint16_t scanCode, bool state);
	void ResetKeyState();
};
