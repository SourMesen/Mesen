#pragma once
#include "stdafx.h"
#include <thread>
#include <unordered_set>
#include "EmulationSettings.h"

class ShortcutKeyHandler
{
private:
	std::thread _thread;
	atomic<bool> _stopThread;
	
	std::unordered_set<uint32_t> _keysDown;
	std::unordered_set<uint32_t> _prevKeysDown;
	bool _turboEnabled;

	void CheckMappedKeys(EmulatorKeyMappings mappings);
	void ProcessKeys(EmulatorKeyMappingSet mappings);
	bool DetectKeyPress(uint32_t keyCode);

public:
	ShortcutKeyHandler();
	~ShortcutKeyHandler();
};