#pragma once
#include "stdafx.h"
#include <thread>
#include "../Utilities/Timer.h"

class AutoSaveManager
{
private:
	const uint32_t _autoSaveSlot = 8;
	std::thread _autoSaveThread;
	atomic<bool> _stopThread;
	Timer _timer;

public:
	AutoSaveManager();
	~AutoSaveManager();
};