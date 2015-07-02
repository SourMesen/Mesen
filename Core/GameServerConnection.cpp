#pragma once
#include "stdafx.h"
#include "MessageManager.h"
#include "GameServerConnection.h"
#include "HandShakeMessage.h"
#include "InputDataMessage.h"
#include "MovieDataMessage.h"
#include "GameInformationMessage.h"
#include "SaveStateMessage.h"
#include "Console.h"
#include "ControlManager.h"
#include "ClientConnectionData.h"

GameServerConnection::GameServerConnection(shared_ptr<Socket> socket, int controllerPort, IGameBroadcaster* gameBroadcaster) : GameConnection(socket, nullptr)
{
	//Server-side connection
	_gameBroadcaster = gameBroadcaster;
	_controllerPort = controllerPort;
		
	ControlManager::BackupControlDevices();

	MessageManager::RegisterNotificationListener(this);
}

GameServerConnection::~GameServerConnection()
{
	if(_connectionData) {
		MessageManager::DisplayToast(L"Net Play", _connectionData->PlayerName + L" (Player " + std::to_wstring(_controllerPort + 1) + L") disconnected.", _connectionData->AvatarData, _connectionData->AvatarSize);
	} else {
		MessageManager::DisplayMessage(L"Net Play", L"Player " + std::to_wstring(_controllerPort + 1) + L" disconnected.");
	}

	ControlManager::RestoreControlDevices();

	MessageManager::UnregisterNotificationListener(this);
}

void GameServerConnection::SendGameState()
{
	Console::Pause();
	stringstream state;
	Console::SaveState(state);
	_handshakeCompleted = true;
	ControlManager::RegisterControlDevice(this, _controllerPort);
	Console::Resume();

	uint32_t size = (uint32_t)state.tellp();
		
	char* buffer = new char[size];
	state.read(buffer, size);
	SendNetMessage(SaveStateMessage(buffer, size, true));
	delete[] buffer;
}

void GameServerConnection::SendGameInformation()
{
	SendNetMessage(GameInformationMessage(Console::GetROMPath(), _controllerPort, Console::CheckFlag(EmulationFlags::Paused)));
}

void GameServerConnection::SendMovieData(uint8_t state, uint8_t port)
{
	if(_handshakeCompleted) {
		SendNetMessage(MovieDataMessage(state, port));
	}
}

ButtonState GameServerConnection::GetButtonState()
{
	ButtonState state;
	uint32_t inputBufferSize = _inputData.size();
	uint8_t stateData = 0;
	if(inputBufferSize > 0) {
		stateData = _inputData.front();
		if(inputBufferSize > 1) {
			//Always keep the last one the client sent, it will be used until a new one is received
			_inputData.pop_front();
		}
	}
	state.FromByte(stateData);
	return state;
}

void GameServerConnection::ProcessMessage(NetMessage* message)
{
	switch(message->GetType()) {
		case MessageType::HandShake:
			//Send the game's current state to the client and register the controller
			if(((HandShakeMessage*)message)->IsValid()) {
				_connectionData.reset(new ClientConnectionData("", 0, ((HandShakeMessage*)message)->GetPlayerName(), ((HandShakeMessage*)message)->GetAvatarData(), ((HandShakeMessage*)message)->GetAvatarSize()));

				MessageManager::DisplayToast(L"Net Play", _connectionData->PlayerName + L" (Player " + std::to_wstring(_controllerPort + 1) + L") connected.", _connectionData->AvatarData, _connectionData->AvatarSize);
				SendGameInformation();
				SendGameState();
			}
			break;
		case MessageType::InputData:
			uint8_t state = ((InputDataMessage*)message)->GetInputState();
			if(_inputData.size() == 0 || state != _inputData.back()) {
				_inputData.push_back(state);
			}
			break;
	}
}

void GameServerConnection::ProcessNotification(ConsoleNotificationType type)
{
	switch(type) {
		case ConsoleNotificationType::GamePaused:
			SendGameInformation();
			break;
		case ConsoleNotificationType::GameLoaded:
		case ConsoleNotificationType::GameResumed:
		case ConsoleNotificationType::GameReset:
			SendGameInformation();
			SendGameState();
			break;
	}
}