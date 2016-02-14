#include "stdafx.h"
#include "WindowsKeyManager.h"
#include "../Core/ControlManager.h"

WindowsKeyManager::WindowsKeyManager(HWND hWnd)
{
	_hWnd = hWnd;
}

WindowsKeyManager::~WindowsKeyManager()
{
}

void WindowsKeyManager::RefreshState()
{
	_gamePad.RefreshState();
}

bool WindowsKeyManager::IsKeyPressed(uint32_t key)
{
	if(key >= 0x10000) {
		//XInput key
		uint8_t gamepadPort = (key - 0xFFFF) / 0x100;
		uint8_t gamepadButton = (key - 0xFFFF) % 0x100;
		return _gamePad.IsPressed(gamepadPort, gamepadButton);
	} else {
		return (GetAsyncKeyState(key) & 0x8000) == 0x8000;
	}
	return false;
}

bool WindowsKeyManager::IsMouseButtonPressed(MouseButton button)
{
	uint32_t key = 0;
	switch(button) {
		case MouseButton::LeftButton: key = VK_LBUTTON; break;
		case MouseButton::RightButton: key = VK_RBUTTON; break;
		case MouseButton::MiddleButton: key = VK_MBUTTON; break;
	}

	return (GetAsyncKeyState(key) & 0x8000) == 0x8000;
}

uint32_t WindowsKeyManager::GetPressedKey()
{
	_gamePad.RefreshState();

	for(int i = 0; i < XUSER_MAX_COUNT; i++) {
		for(int j = 1; j <= 22; j++) {
			if(_gamePad.IsPressed(i, j)) {
				return 0xFFFF + i * 0x100 + j;
			}
		}
	}

	for(int i = 0; i < 0xFF; i++) {
		if((GetAsyncKeyState(i) & 0x8000) == 0x8000) {
			return i;
		}
	}
	return 0;
}

string WindowsKeyManager::GetKeyName(uint32_t key)
{
	for(KeyDefinition keyDef : _keyDefinitions) {
		if(keyDef.keyCode == key) {
			return keyDef.description;
		}
	}
	return "";
}

uint32_t WindowsKeyManager::GetKeyCode(string keyName)
{
	for(KeyDefinition keyDef : _keyDefinitions) {
		if(keyName.compare(keyDef.description) == 0) {
			return keyDef.keyCode;
		}
	}
	return 0;
}