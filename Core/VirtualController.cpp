#pragma once
#include "stdafx.h"
#include "VirtualController.h"
#include "ControlManager.h"
#include "Console.h"
#include "EmulationSettings.h"

VirtualController::VirtualController(uint8_t port)
{
	_port = port;
	_queueSize = 0;
	_waiting = false;
	_shutdown = false;
	ControlManager::RegisterControlDevice(this, _port);		
}

VirtualController::~VirtualController()
{
	_shutdown = true;
	while(_waiting.load());

	ControlManager::UnregisterControlDevice(this);
}

ButtonState VirtualController::GetButtonState()
{
	ButtonState state;
	if(_queueSize.load() == 0) {
		_waiting = true;
		while(_queueSize.load() <= _minimumBuffer) {
			if(_minimumBuffer < 10) {
				_minimumBuffer++;
			}
			if(_shutdown.load()) {
				_waiting = false;
				return state;
			}
		}
		_waiting = false;
	}

	_writeLock.Acquire();
	state.FromByte(_inputData.front());
	_inputData.pop_front();
	_queueSize--;

	if(_queueSize.load() > _minimumBuffer) {
		EmulationSettings::SetEmulationSpeed(0);
	} else {
		EmulationSettings::SetEmulationSpeed(100);
	}

	_writeLock.Release();

	return state;
}

void VirtualController::PushState(uint8_t state)
{
	_writeLock.Acquire();
	_inputData.push_back(state);
	_queueSize++;
	_writeLock.Release();
}