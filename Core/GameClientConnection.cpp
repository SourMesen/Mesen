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
#include "VirtualController.h"
#include "ClientConnectionData.h"

GameClientConnection::GameClientConnection(shared_ptr<Socket> socket, shared_ptr<ClientConnectionData> connectionData) : GameConnection(socket, connectionData)
{
	_controlDevice = ControlManager::GetControlDevice(0);
	ControlManager::BackupControlDevices();

	MessageManager::DisplayMessage("Net Play", "Connected to server.");

	SendHandshake();
}

GameClientConnection::~GameClientConnection()
{
	_virtualControllers.clear();
	ControlManager::RestoreControlDevices();
	MessageManager::DisplayMessage("Net Play", "Connection to server lost.");
}

void GameClientConnection::SendHandshake()
{
	HandShakeMessage message(_connectionData->PlayerName, _connectionData->AvatarData, _connectionData->AvatarSize);
	SendNetMessage(message);
}

void GameClientConnection::InitializeVirtualControllers()
{
	for(int i = 0; i < 4; i++) {
		_virtualControllers.push_back(unique_ptr<VirtualController>(new VirtualController(i)));
	}
}

void GameClientConnection::DisposeVirtualControllers()
{
	_virtualControllers.clear();
}

void GameClientConnection::ProcessMessage(NetMessage* message)
{
	uint8_t port;
	uint8_t state;
	GameInformationMessage* gameInfo;

	switch(message->GetType()) {
		case MessageType::SaveState:
			if(_gameLoaded) {
				DisposeVirtualControllers();

				Console::Pause();

				((SaveStateMessage*)message)->LoadState();

				Console::Resume();

				InitializeVirtualControllers();
			}
			break;
		case MessageType::MovieData:
			if(_gameLoaded) {
				port = ((MovieDataMessage*)message)->GetPortNumber();
				state = ((MovieDataMessage*)message)->GetInputState();

				_virtualControllers[port]->PushState(state);
			}
			break;
		case MessageType::GameInformation:
			gameInfo = (GameInformationMessage*)message;
			if(gameInfo->GetPort() != _controllerPort) {
				_controllerPort = gameInfo->GetPort();
				MessageManager::DisplayMessage("Net Play", string("Connected as player ") + std::to_string(_controllerPort + 1));
			}

			DisposeVirtualControllers();

			_gameLoaded = gameInfo->AttemptLoadGame();
			if(gameInfo->IsPaused()) {
				EmulationSettings::SetFlags(EmulationFlags::Paused);
			} else {
				EmulationSettings::ClearFlags(EmulationFlags::Paused);
			}

			break;
		default:
			break;
	}
}
	
void GameClientConnection::SendInput()
{
	if(_gameLoaded) {
		uint8_t inputState = _controlDevice->GetButtonState().ToByte();
		if(_lastInputSent != inputState) {
			InputDataMessage message(inputState);
			SendNetMessage(message);
			_lastInputSent = inputState;
		}
	}
}