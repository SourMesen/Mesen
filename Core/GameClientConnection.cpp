#include "stdafx.h"
#include "GameClientConnection.h"
#include "HandShakeMessage.h"
#include "InputDataMessage.h"
#include "MovieDataMessage.h"
#include "GameInformationMessage.h"
#include "SaveStateMessage.h"
#include "Console.h"
#include "EmulationSettings.h"
#include "ControlManager.h"
#include "ClientConnectionData.h"
#include "StandardController.h"
#include "Zapper.h"
#include "ArkanoidController.h"
#include "BandaiHyperShot.h"
#include "SelectControllerMessage.h"
#include "PlayerListMessage.h"
#include "ForceDisconnectMessage.h"
#include "ServerInformationMessage.h"

GameClientConnection::GameClientConnection(shared_ptr<Socket> socket, ClientConnectionData &connectionData) : GameConnection(socket)
{
	_connectionData = connectionData;
	_shutdown = false;
	_enableControllers = false;
	_minimumQueueSize = 3;

	MessageManager::DisplayMessage("NetPlay", "ConnectedToServer");
	ControlManager::RegisterInputProvider(this);
}

GameClientConnection::~GameClientConnection()
{
	Shutdown();
}

void GameClientConnection::Shutdown()
{
	if(!_shutdown) {
		_shutdown = true;
		DisableControllers();

		ControlManager::UnregisterInputProvider(this);
		MessageManager::SendNotification(ConsoleNotificationType::DisconnectedFromServer);
		MessageManager::DisplayMessage("NetPlay", "ConnectionLost");
		EmulationSettings::ClearFlags(EmulationFlags::ForceMaxSpeed);
	}
}

void GameClientConnection::SendHandshake()
{
	HandShakeMessage message(_connectionData.PlayerName, HandShakeMessage::GetPasswordHash(_connectionData.Password, _serverSalt), _connectionData.Spectator);
	SendNetMessage(message);
}

void GameClientConnection::SendControllerSelection(uint8_t port)
{
	SelectControllerMessage message(port);
	SendNetMessage(message);
}

void GameClientConnection::ClearInputData()
{
	LockHandler lock = _writeLock.AcquireSafe();
	for(int i = 0; i < BaseControlDevice::PortCount; i++) {
		_inputSize[i] = 0;
		_inputData[i].clear();
	}
}

void GameClientConnection::ProcessMessage(NetMessage* message)
{
	GameInformationMessage* gameInfo;

	switch(message->GetType()) {
		case MessageType::ServerInformation:
			_serverSalt = ((ServerInformationMessage*)message)->GetHashSalt();
			SendHandshake();
			break;

		case MessageType::SaveState:
			if(_gameLoaded) {
				DisableControllers();
				Console::Pause();
				ClearInputData();
				((SaveStateMessage*)message)->LoadState();
				_enableControllers = true;
				InitControlDevice();
				Console::Resume();
			}
			break;

		case MessageType::MovieData:
			if(_gameLoaded) {
				PushControllerState(((MovieDataMessage*)message)->GetPortNumber(), ((MovieDataMessage*)message)->GetInputState());
			}
			break;

		case MessageType::ForceDisconnect:
			MessageManager::DisplayMessage("NetPlay", ((ForceDisconnectMessage*)message)->GetMessage());
			break;

		case MessageType::PlayerList:
			_playerList = ((PlayerListMessage*)message)->GetPlayerList();
			break;

		case MessageType::GameInformation:
			DisableControllers();
			Console::Pause();
			gameInfo = (GameInformationMessage*)message;
			if(gameInfo->GetPort() != _controllerPort) {
				_controllerPort = gameInfo->GetPort();

				if(_controllerPort == GameConnection::SpectatorPort) {
					MessageManager::DisplayMessage("NetPlay", "ConnectedAsSpectator");
				} else {
					MessageManager::DisplayMessage("NetPlay", "ConnectedAsPlayer", std::to_string(_controllerPort + 1));
				}
			}

			ClearInputData();

			_gameLoaded = gameInfo->AttemptLoadGame();
			if(gameInfo->IsPaused()) {
				EmulationSettings::SetFlags(EmulationFlags::Paused);
			} else {
				EmulationSettings::ClearFlags(EmulationFlags::Paused);
			}
			Console::Resume();
			break;
		default:
			break;
	}
}

void GameClientConnection::PushControllerState(uint8_t port, ControlDeviceState state)
{
	LockHandler lock = _writeLock.AcquireSafe();
	_inputData[port].push_back(state);
	_inputSize[port]++;

	if(_inputData[port].size() >= _minimumQueueSize) {
		_waitForInput[port].Signal();
	}
}

void GameClientConnection::DisableControllers()
{
	//Used to prevent deadlocks when client is trying to fill its buffer while the host changes the current game/settings/etc. (i.e situations where we need to call Console::Pause())
	ClearInputData();
	_enableControllers = false;
	for(int i = 0; i < BaseControlDevice::PortCount; i++) {
		_waitForInput[i].Signal();
	}
}

bool GameClientConnection::SetInput(BaseControlDevice *device)
{
	if(_enableControllers) {
		uint8_t port = device->GetPort();
		while(_inputSize[port] == 0) {
			_waitForInput[port].Wait();

			if(port == 0 && _minimumQueueSize < 10) {
				//Increase buffer size - reduces freezes at the cost of additional lag
				_minimumQueueSize++;
			}

			if(_shutdown || !_enableControllers) {
				return true;
			}
		}

		LockHandler lock = _writeLock.AcquireSafe();
		ControlDeviceState state = _inputData[port].front();
		_inputData[port].pop_front();
		_inputSize[port]--;

		if(_inputData[port].size() > _minimumQueueSize) {
			//Too much data, catch up
			EmulationSettings::SetFlags(EmulationFlags::ForceMaxSpeed);
		} else {
			EmulationSettings::ClearFlags(EmulationFlags::ForceMaxSpeed);
			EmulationSettings::SetEmulationSpeed(100);
		}

		device->SetRawState(state);
	}
	return true;
}

void GameClientConnection::InitControlDevice()
{
	if(_controllerPort == BaseControlDevice::ExpDevicePort) {
		_newControlDevice = ControlManager::CreateExpansionDevice(EmulationSettings::GetExpansionDevice());
	} else {
		//Pretend we are using port 0 (to use player 1's keybindings during netplay)
		_newControlDevice = ControlManager::CreateControllerDevice(EmulationSettings::GetControllerType(_controllerPort), 0);
	}
}

void GameClientConnection::ProcessNotification(ConsoleNotificationType type, void* parameter)
{
	if(type == ConsoleNotificationType::ConfigChanged) {
		InitControlDevice();
	}
}

void GameClientConnection::SendInput()
{
	if(_gameLoaded) {
		if(_newControlDevice) {
			_controlDevice = _newControlDevice;
			_newControlDevice.reset();
		}

		ControlDeviceState inputState;
		if(_controlDevice) {
			_controlDevice->SetStateFromInput();
			inputState = _controlDevice->GetRawState();
		}
		
		if(_lastInputSent != inputState) {
			InputDataMessage message(inputState);
			SendNetMessage(message);
			_lastInputSent = inputState;
		}
	}
}

void GameClientConnection::SelectController(uint8_t port)
{
	SendControllerSelection(port);
}

uint8_t GameClientConnection::GetAvailableControllers()
{
	uint8_t availablePorts = (1 << BaseControlDevice::PortCount) - 1;
	for(PlayerInfo &playerInfo : _playerList) {
		if(playerInfo.ControllerPort < BaseControlDevice::PortCount) {
			availablePorts &= ~(1 << playerInfo.ControllerPort);
		}
	}
	return availablePorts;
}

uint8_t GameClientConnection::GetControllerPort()
{
	return _controllerPort;
}