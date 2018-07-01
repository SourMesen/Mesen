#pragma once
#include "stdafx.h"
#include <thread>
#include <unordered_set>
#include "../Utilities/SimpleLock.h"
#include "EmulationSettings.h"

class Console;

class ShortcutKeyHandler
{
private:
	shared_ptr<Console> _console;

	std::thread _thread;
	atomic<bool> _stopThread;
	SimpleLock _lock;
	
	int _keySetIndex;
	vector<uint32_t> _pressedKeys;
	vector<uint32_t> _lastPressedKeys;
	bool _isKeyUp;
	bool _keyboardMode;

	std::unordered_set<uint32_t> _keysDown[2];
	std::unordered_set<uint32_t> _prevKeysDown[2];
	
	void CheckMappedKeys();
	
	bool IsKeyPressed(EmulatorShortcut key);
	bool IsKeyPressed(KeyCombination comb);
	bool IsKeyPressed(uint32_t keyCode);

	bool DetectKeyPress(EmulatorShortcut key);
	bool DetectKeyRelease(EmulatorShortcut key);

public:
	ShortcutKeyHandler(shared_ptr<Console> console);
	~ShortcutKeyHandler();

	void ProcessKeys();
};