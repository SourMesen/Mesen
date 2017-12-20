#include "stdafx.h"
#include "KeyManager.h"
#include "IKeyManager.h"
#include "Types.h"
#include "EmulationSettings.h"
#include "PPU.h"

unique_ptr<IKeyManager> KeyManager::_keyManager;
MousePosition KeyManager::_mousePosition;
atomic<int16_t> KeyManager::_xMouseMovement;
atomic<int16_t> KeyManager::_yMouseMovement;

void KeyManager::RegisterKeyManager(IKeyManager* keyManager)
{
	_keyManager.reset(keyManager);
}

void KeyManager::RefreshKeyState()
{
	if(_keyManager != nullptr) {
		return _keyManager->RefreshState();
	}
}

bool KeyManager::IsKeyPressed(uint32_t keyCode)
{
	if(_keyManager != nullptr) {
		return EmulationSettings::InputEnabled() && _keyManager->IsKeyPressed(keyCode);
	}
	return false;
}

bool KeyManager::IsMouseButtonPressed(MouseButton button)
{
	if(_keyManager != nullptr) {
		return EmulationSettings::InputEnabled() && _keyManager->IsMouseButtonPressed(button);
	}
	return false;
}

vector<uint32_t> KeyManager::GetPressedKeys()
{
	if(_keyManager != nullptr) {
		return _keyManager->GetPressedKeys();
	}
	return vector<uint32_t>();
}

string KeyManager::GetKeyName(uint32_t keyCode)
{
	if(_keyManager != nullptr) {
		return _keyManager->GetKeyName(keyCode);
	}
	return "";
}

uint32_t KeyManager::GetKeyCode(string keyName)
{
	if(_keyManager != nullptr) {
		return _keyManager->GetKeyCode(keyName);
	}
	return 0;
}

void KeyManager::UpdateDevices()
{
	if(_keyManager != nullptr) {
		_keyManager->UpdateDevices();
	}
}

void KeyManager::SetMouseMovement(int16_t x, int16_t y)
{
	_xMouseMovement += x;
	_yMouseMovement += y;
}

MouseMovement KeyManager::GetMouseMovement()
{
	double factor = EmulationSettings::GetVideoScale() * EmulationSettings::GetMouseSensitivity();
	MouseMovement mov;
	mov.dx = (int16_t)(_xMouseMovement / factor);
	mov.dy = (int16_t)(_yMouseMovement / factor);
	_xMouseMovement -= (int16_t)(mov.dx * factor);
	_yMouseMovement -= (int16_t)(mov.dy * factor);

	return mov;
}

void KeyManager::SetMousePosition(double x, double y)
{
	if(x < 0 || y < 0) {
		_mousePosition.X = -1;
		_mousePosition.Y = -1;
	} else {
		OverscanDimensions overscan = EmulationSettings::GetOverscanDimensions();
		_mousePosition.X = (int32_t)(x * (PPU::ScreenWidth - overscan.Left - overscan.Right) + overscan.Left);
		_mousePosition.Y = (int32_t)(y * (PPU::ScreenHeight - overscan.Top - overscan.Bottom) + overscan.Top);
	}
}

MousePosition KeyManager::GetMousePosition()
{
	return _mousePosition;
}