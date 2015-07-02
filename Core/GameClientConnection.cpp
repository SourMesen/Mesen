#pragma once
#include "stdafx.h"
#include "GameClientConnection.h"
#include "HandShakeMessage.h"
#include "InputDataMessage.h"
#include "MovieDataMessage.h"
#include "GameInformationMessage.h"
#include "SaveStateMessage.h"
#include "Console.h"
#include "ControlManager.h"
#include "VirtualController.h"
#include "ClientConnectionData.h"

GameClientConnection::GameClientConnection(shared_ptr<Socket> socket, shared_ptr<ClientConnectionData> connectionData) : GameConnection(socket, connectionData)
{
	_controlDevice = ControlManager::GetControlDevice(0);
	ControlManager::BackupControlDevices();

	MessageManager::DisplayMessage(L"Net Play", L"Connected to server.");

	SendHandshake();
}

GameClientConnection::~GameClientConnection()
{
	_virtualControllers.clear();
	ControlManager::RestoreControlDevices();
	MessageManager::DisplayMessage(L"Net Play", L"Connection to server lost.");
}

void GameClientConnection::SendHandshake()
{
	SendNetMessage(HandShakeMessage(_connectionData->PlayerName, _connectionData->AvatarData, _connectionData->AvatarSize));
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
				MessageManager::DisplayMessage(wstring(L"Connected as player ") + std::to_wstring(_controllerPort + 1));
			}

			DisposeVirtualControllers();

			_gameLoaded = gameInfo->AttemptLoadGame();
			if(gameInfo->IsPaused()) {
				Console::SetFlags(EmulationFlags::Paused);
			} else {
				Console::ClearFlags(EmulationFlags::Paused);
			}

			break;
	}
}
	
void GameClientConnection::SendInput()
{
	uint8_t inputState = _controlDevice->GetButtonState().ToByte();
	if(_lastInputSent != inputState) {
		SendNetMessage(InputDataMessage(inputState));
		_lastInputSent = inputState;
	}
}