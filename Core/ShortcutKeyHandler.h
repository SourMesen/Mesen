#pragma once
#include "stdafx.h"
#include <thread>
#include <unordered_set>
#include "../Utilities/SimpleLock.h"
#include "EmulationSettings.h"

class ShortcutKeyHandler
{
private:
	std::thread _thread;
	atomic<bool> _stopThread;
	SimpleLock _lock;
	
	int _keySetIndex;
	vector<uint32_t> _pressedKeys;

	std::unordered_set<uint32_t> _keysDown[2];
	std::unordered_set<uint32_t> _prevKeysDown[2];
	
	void CheckMappedKeys();
	
	bool IsKeyPressed(EmulatorShortcut key);

	bool DetectKeyPress(EmulatorShortcut key);
	bool DetectKeyRelease(EmulatorShortcut key);

public:
	ShortcutKeyHandler();
	~ShortcutKeyHandler();

	void ProcessKeys();
};