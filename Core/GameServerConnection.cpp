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
#include "EmulationSettings.h"

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
		MessageManager::DisplayToast("Net Play", _connectionData->PlayerName + " (Player " + std::to_string(_controllerPort + 1) + ") disconnected.", _connectionData->AvatarData, _connectionData->AvatarSize);
	} else {
		MessageManager::DisplayMessage("Net Play", "Player " + std::to_string(_controllerPort + 1) + " disconnected.");
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
	SaveStateMessage message(buffer, size, true);
	SendNetMessage(message);
	delete[] buffer;
}

void GameServerConnection::SendGameInformation()
{
	GameInformationMessage message(Console::GetROMPath(), _controllerPort, EmulationSettings::CheckFlag(EmulationFlags::Paused));
	SendNetMessage(message);
}

void GameServerConnection::SendMovieData(uint8_t state, uint8_t port)
{
	if(_handshakeCompleted) {
		MovieDataMessage message(state, port);
		SendNetMessage(message);
	}
}

ButtonState GameServerConnection::GetButtonState()
{
	ButtonState state;
	size_t inputBufferSize = _inputData.size();
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

				MessageManager::DisplayToast("Net Play", _connectionData->PlayerName + " (Player " + std::to_string(_controllerPort + 1) + ") connected.", _connectionData->AvatarData, _connectionData->AvatarSize);
				SendGameInformation();
				SendGameState();
			}
			break;
		case MessageType::InputData:
		{
			uint8_t state = ((InputDataMessage*)message)->GetInputState();
			if(_inputData.size() == 0 || state != _inputData.back()) {
				_inputData.push_back(state);
			}
			break;
		}
		default:
			break;
	}
}

void GameServerConnection::ProcessNotification(ConsoleNotificationType type, void* parameter)
{
	switch(type) {
		case ConsoleNotificationType::GamePaused:
			SendGameInformation();
			break;
		case ConsoleNotificationType::GameLoaded:
		case ConsoleNotificationType::GameResumed:
		case ConsoleNotificationType::GameReset:
		case ConsoleNotificationType::StateLoaded:
			SendGameInformation();
			SendGameState();
			break;
		default:
			break;
	}
}