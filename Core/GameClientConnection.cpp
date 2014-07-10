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

GameClientConnection::GameClientConnection(shared_ptr<Socket> socket) : GameConnection(socket)
{
	_controlDevice = ControlManager::GetControlDevice(0);
	ControlManager::BackupControlDevices();

	Console::DisplayMessage(L"Connected to server.");

	SendHandshake();
}

GameClientConnection::~GameClientConnection()
{
	_virtualControllers.clear();
	ControlManager::RestoreControlDevices();
	Console::DisplayMessage(L"Connection to server lost.");
}

void GameClientConnection::SendHandshake()
{
	SendNetMessage(HandShakeMessage());
}

void GameClientConnection::ProcessMessage(NetMessage* message)
{
	uint8_t port;
	uint8_t state;
	GameInformationMessage* gameInfo;

	switch(message->Type) {
		case MessageType::SaveState:
			if(_gameLoaded) {
				_virtualControllers.clear();

				Console::Pause();

				((SaveStateMessage*)message)->LoadState();

				for(int i = 0; i < 4; i++) {
					_virtualControllers.push_back(unique_ptr<VirtualController>(new VirtualController(i)));
				}

				Console::Resume();
			}
			break;
		case MessageType::MovieData:
			if(_gameLoaded) {
				port = ((MovieDataMessage*)message)->PortNumber;
				state = ((MovieDataMessage*)message)->InputState;

				_virtualControllers[port]->PushState(state);
			}
			break;
		case MessageType::GameInformation:
			gameInfo = (GameInformationMessage*)message;
			if(gameInfo->ControllerPort != _controllerPort) {
				_controllerPort = gameInfo->ControllerPort;
				Console::DisplayMessage(wstring(L"Connected as player ") + std::to_wstring(_controllerPort + 1));
			}
			_virtualControllers.clear();
			_gameLoaded = gameInfo->AttemptLoadGame();
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