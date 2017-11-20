#pragma once
#include "stdafx.h"
#include "Types.h"

class IKeyManager;
enum class MouseButton;

class KeyManager
{
private:
	static unique_ptr<IKeyManager> _keyManager;
	static MousePosition _mousePosition;
	static atomic<int16_t> _xMouseMovement;
	static atomic<int16_t> _yMouseMovement;

public:
	static void RegisterKeyManager(IKeyManager* keyManager);
	static void RefreshKeyState();
	static bool IsKeyPressed(uint32_t keyCode);
	static bool IsMouseButtonPressed(MouseButton button);
	static vector<uint32_t> GetPressedKeys();
	static string GetKeyName(uint32_t keyCode);
	static uint32_t GetKeyCode(string keyName);

	static void UpdateDevices();
	
	static void SetMouseMovement(int16_t x, int16_t y);
	static MouseMovement GetMouseMovement();
	
	static void SetMousePosition(double x, double y);
	static MousePosition GetMousePosition();
};