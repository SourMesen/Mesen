#pragma once

#include "stdafx.h"
#include <atomic>
using std::atomic;

#include "IControlDevice.h"
#include "../Utilities/SimpleLock.h"

class VirtualController : public IControlDevice
{
	list<uint8_t> _inputData;
	bool _paused = false;
	atomic<uint32_t> _queueSize;
	atomic<bool> _waiting;
	atomic<bool> _shutdown;
	SimpleLock _writeLock;
	uint8_t _port;
	uint32_t _minimumBuffer = 3;

public:
	VirtualController(uint8_t port);
	~VirtualController();

	ButtonState GetButtonState();
	void PushState(uint8_t state);
};
