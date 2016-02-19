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
#include "StandardController.h"
#include "SelectControllerMessage.h"
#include "PlayerListMessage.h"
#include "GameServer.h"
#include "ForceDisconnectMessage.h"

GameServerConnection* GameServerConnection::_netPlayDevices[4] = { nullptr,nullptr,nullptr,nullptr };

GameServerConnection::GameServerConnection(shared_ptr<Socket> socket) : GameConnection(socket, nullptr)
{
	//Server-side connection
	_controllerPort = GameConnection::SpectatorPort;
	MessageManager::RegisterNotificationListener(this);
}

GameServerConnection::~GameServerConnection()
{
	if(_connectionData) {
		MessageManager::DisplayToast("Net Play", _connectionData->PlayerName + " (Player " + std::to_string(_controllerPort + 1) + ") disconnected.", _connectionData->AvatarData, _connectionData->AvatarSize);
	}

	UnregisterNetPlayDevice(this);
	MessageManager::UnregisterNotificationListener(this);
}

void GameServerConnection::SendGameInformation()
{
	Console::Pause();
	GameInformationMessage gameInfo(Console::GetROMPath(), Console::GetCrc32(), _controllerPort, EmulationSettings::CheckFlag(EmulationFlags::Paused));
	SendNetMessage(gameInfo);
	SaveStateMessage saveState;
	SendNetMessage(saveState);
	Console::Resume();
}

void GameServerConnection::SendMovieData(uint8_t state, uint8_t port)
{
	if(_handshakeCompleted) {
		MovieDataMessage message(state, port);
		SendNetMessage(message);
	}
}

void GameServerConnection::SendForceDisconnectMessage(string disconnectMessage)
{
	ForceDisconnectMessage message(disconnectMessage);
	SendNetMessage(message);
	Disconnect();
}

void GameServerConnection::PushState(uint32_t state)
{
	if(_inputData.size() == 0 || state != _inputData.back()) {
		_inputData.push_back(state);
	}
}

uint32_t GameServerConnection::GetState()
{
	size_t inputBufferSize = _inputData.size();
	uint32_t stateData = 0;
	if(inputBufferSize > 0) {
		stateData = _inputData.front();
		if(inputBufferSize > 1) {
			//Always keep the last one the client sent, it will be used until a new one is received
			_inputData.pop_front();
		}
	}
	return stateData;
}

void GameServerConnection::ProcessHandshakeResponse(HandShakeMessage* message)
{
	//Send the game's current state to the client and register the controller
	if(message->IsValid()) {
		Console::Pause();

		_controllerPort = message->IsSpectator() ? GameConnection::SpectatorPort : GetFirstFreeControllerPort();
		_connectionData.reset(new ClientConnectionData("", 0, message->GetPlayerName(), message->GetAvatarData(), message->GetAvatarSize(), false));

		string playerPortMessage = _controllerPort == GameConnection::SpectatorPort ? "Spectator" : "Player " + std::to_string(_controllerPort + 1);

		MessageManager::DisplayToast("Net Play", _connectionData->PlayerName + " (" + playerPortMessage + ") connected.", _connectionData->AvatarData, _connectionData->AvatarSize);

		if(Console::GetROMPath().size() > 0) {
			SendGameInformation();
		}

		_handshakeCompleted = true;
		RegisterNetPlayDevice(this, _controllerPort);
		GameServer::SendPlayerList();
		Console::Resume();
	} else {
		SendForceDisconnectMessage("Server is using a different version of Mesen (" + EmulationSettings::GetMesenVersionString() + ") - you have been disconnected.");
		MessageManager::DisplayMessage("NetPlay", + "NetplayVersionMismatch", message->GetPlayerName());
	}
}

void GameServerConnection::ProcessMessage(NetMessage* message)
{
	switch(message->GetType()) {
		case MessageType::HandShake:
			ProcessHandshakeResponse((HandShakeMessage*)message);
			break;

		case MessageType::InputData:
			PushState(((InputDataMessage*)message)->GetInputState());
			break;

		case MessageType::SelectController:
			SelectControllerPort(((SelectControllerMessage*)message)->GetPortNumber());
			break;

		default:
			break;
	}
}

void GameServerConnection::SelectControllerPort(uint8_t port)
{
	Console::Pause();
	if(port == GameConnection::SpectatorPort) {
		//Client wants to be a spectator, make sure we are not using any controller
		UnregisterNetPlayDevice(this);
		_controllerPort = port;
	} else {
		GameServerConnection* netPlayDevice = GetNetPlayDevice(port);
		if(netPlayDevice == this) {
			//Nothing to do, we're already this player
		} else if(netPlayDevice == nullptr) {
			//This port is free, we can switch
			UnregisterNetPlayDevice(this);
			RegisterNetPlayDevice(this, port);
			_controllerPort = port;
		} else {
			//Another player is using this port, we can't use it
		}
	}
	SendGameInformation();
	GameServer::SendPlayerList();
	Console::Resume();
}

void GameServerConnection::ProcessNotification(ConsoleNotificationType type, void* parameter)
{
	switch(type) {
		case ConsoleNotificationType::GamePaused:
		case ConsoleNotificationType::GameLoaded:
		case ConsoleNotificationType::GameResumed:
		case ConsoleNotificationType::GameReset:
		case ConsoleNotificationType::StateLoaded:
		case ConsoleNotificationType::CheatAdded:
		case ConsoleNotificationType::FdsDiskChanged:
		case ConsoleNotificationType::ConfigChanged:
			SendGameInformation();
			break;
		default:
			break;
	}
}

void GameServerConnection::RegisterNetPlayDevice(GameServerConnection* device, uint8_t port)
{
	GameServerConnection::_netPlayDevices[port] = device;
}

void GameServerConnection::UnregisterNetPlayDevice(GameServerConnection* device)
{
	if(device != nullptr) {
		for(int i = 0; i < 4; i++) {
			if(GameServerConnection::_netPlayDevices[i] == device) {
				GameServerConnection::_netPlayDevices[i] = nullptr;
				break;
			}
		}
	}
}

GameServerConnection* GameServerConnection::GetNetPlayDevice(uint8_t port)
{
	return GameServerConnection::_netPlayDevices[port];
}

uint8_t GameServerConnection::GetFirstFreeControllerPort()
{
	uint8_t hostPost = GameServer::GetHostControllerPort();
	for(int i = 0; i < 4; i++) {
		if(hostPost != i && GameServerConnection::_netPlayDevices[i] == nullptr) {
			return i;
		}
	}
	return GameConnection::SpectatorPort;
}

string GameServerConnection::GetPlayerName()
{
	if(_connectionData) {
		return _connectionData->PlayerName;
	} else {
		return "";
	}
}

uint8_t GameServerConnection::GetControllerPort()
{
	return _controllerPort;
}