#include "stdafx.h"
#include "BaseControlDevice.h"
#include "ControlManager.h"
#include "MovieManager.h"
#include "EmulationSettings.h"
#include "GameClient.h"
#include "GameServerConnection.h"

BaseControlDevice::BaseControlDevice(uint8_t port)
{
	_port = port;
	_famiconDevice = EmulationSettings::GetConsoleType() == ConsoleType::Famicom;
	if(EmulationSettings::GetControllerType(port) == ControllerType::StandardController) {
		AddKeyMappings(EmulationSettings::GetControllerKeys(port));
	}
}

BaseControlDevice::~BaseControlDevice()
{
}

void BaseControlDevice::StreamState(bool saving)
{
	Stream(_currentState);
}

uint8_t BaseControlDevice::GetPort()
{
	return _port;
}

void BaseControlDevice::AddKeyMappings(KeyMappingSet keyMappings)
{
	if(keyMappings.Mapping1.HasKeySet()) {
		_keyMappings.push_back(keyMappings.Mapping1);
	}
	if(keyMappings.Mapping2.HasKeySet()) {
		_keyMappings.push_back(keyMappings.Mapping2);
	}
	if(keyMappings.Mapping3.HasKeySet()) {
		_keyMappings.push_back(keyMappings.Mapping3);
	}
	if(keyMappings.Mapping4.HasKeySet()) {
		_keyMappings.push_back(keyMappings.Mapping4);
	}
	_turboSpeed = keyMappings.TurboSpeed;
}

void BaseControlDevice::RefreshStateBuffer()
{
	//Do nothing by default - used by standard controllers and some others
}

uint8_t BaseControlDevice::ProcessNetPlayState(uint32_t netplayState)
{
	return netplayState;
}

uint8_t BaseControlDevice::GetControlState()
{
	GameServerConnection* netPlayDevice = GameServerConnection::GetNetPlayDevice(_port);
	if(MovieManager::Playing()) {
		_currentState = MovieManager::GetState(_port);
	} else if(GameClient::Connected()) {
		_currentState = GameClient::GetControllerState(_port);
	} else if(netPlayDevice) {
		_currentState = ProcessNetPlayState(netPlayDevice->GetState());
	} else {
		_currentState = RefreshState();
	}

	if(MovieManager::Recording()) {
		MovieManager::RecordState(_port, _currentState);
	}

	//For NetPlay
	ControlManager::BroadcastInput(_port, _currentState);

	return _currentState;
}