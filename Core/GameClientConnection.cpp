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

GameClientConnection::GameClientConnection(shared_ptr<Socket> socket, shared_ptr<ClientConnectionData> connectionData) : GameConnection(socket, connectionData)
{
	MessageManager::DisplayMessage("Net Play", "Connected to server.");
	SendHandshake();
}

GameClientConnection::~GameClientConnection()
{
	_shutdown = true;
	DisableControllers();

	MessageManager::SendNotification(ConsoleNotificationType::DisconnectedFromServer);
	MessageManager::DisplayMessage("Net Play", "Connection to server lost.");
}

void GameClientConnection::SendHandshake()
{
	HandShakeMessage message(_connectionData->PlayerName, _connectionData->AvatarData, _connectionData->AvatarSize);
	SendNetMessage(message);
}

void GameClientConnection::ClearInputData()
{
	for(int i = 0; i < 4; i++) {
		_inputSize[i] = 0;
		_inputData[i].clear();
	}
}

void GameClientConnection::ProcessMessage(NetMessage* message)
{
	GameInformationMessage* gameInfo;

	switch(message->GetType()) {
		case MessageType::SaveState:
			if(_gameLoaded) {
				DisableControllers();
				Console::Pause();
				ClearInputData();
				((SaveStateMessage*)message)->LoadState();
				_enableControllers = true;
				switch(EmulationSettings::GetControllerType(_controllerPort)) {
					case ControllerType::StandardController: _controlDevice.reset(new StandardController(0)); break;
					case ControllerType::Zapper: _controlDevice = ControlManager::GetControlDevice(_controllerPort); break;
				}
				Console::Resume();

			}
			break;
		case MessageType::MovieData:
			if(_gameLoaded) {
				PushControllerState(((MovieDataMessage*)message)->GetPortNumber(), ((MovieDataMessage*)message)->GetInputState());
			}
			break;
		case MessageType::GameInformation:
			DisableControllers();
			Console::Pause();
			gameInfo = (GameInformationMessage*)message;
			if(gameInfo->GetPort() != _controllerPort) {
				_controllerPort = gameInfo->GetPort();
				MessageManager::DisplayMessage("Net Play", string("Connected as player ") + std::to_string(_controllerPort + 1));
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

void GameClientConnection::PushControllerState(uint8_t port, uint8_t state)
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
	for(int i = 0; i < 4; i++) {
		_waitForInput[i].Signal();
	}
}

uint8_t GameClientConnection::GetControllerState(uint8_t port)
{
	if(_enableControllers) {
		while(_inputSize[port] == 0) {
			_waitForInput[port].Wait();

			if(port == 0 && _minimumQueueSize < 10) {
				//Increase buffer size - reduces freezes at the cost of additional lag
				_minimumQueueSize++;
			}

			if(_shutdown || !_enableControllers) {
				return 0;
			}
		}

		LockHandler lock = _writeLock.AcquireSafe();
		uint8_t state = _inputData[port].front();
		_inputData[port].pop_front();
		_inputSize[port]--;

		if(_inputData[port].size() > _minimumQueueSize) {
			//Too much data, catch up
			EmulationSettings::SetEmulationSpeed(0);
		} else {
			EmulationSettings::SetEmulationSpeed(100);
		}
		return state;
	}
	return 0;
}
	
void GameClientConnection::SendInput()
{
	if(_gameLoaded) {
		uint32_t inputState = 0;
		if(std::dynamic_pointer_cast<Zapper>(_controlDevice)) {
			shared_ptr<Zapper> zapper = std::dynamic_pointer_cast<Zapper>(_controlDevice);
			inputState = zapper->GetZapperState();
		} else if(std::dynamic_pointer_cast<StandardController>(_controlDevice)) {
			shared_ptr<StandardController> controller = std::dynamic_pointer_cast<StandardController>(_controlDevice);
			inputState = controller->GetButtonState();
		}

		if(_lastInputSent != inputState) {
			InputDataMessage message(inputState);
			SendNetMessage(message);
			_lastInputSent = inputState;
		}
	}
}