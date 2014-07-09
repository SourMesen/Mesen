#pragma once
#include "stdafx.h"
#include "GameConnection.h"
#include "VirtualController.h"

class GameClientConnection : public GameConnection
{
private:
	vector<unique_ptr<VirtualController>> _virtualControllers;
	IControlDevice* _controlDevice;
	uint8_t _lastInputSent = 0x00;
	bool _gameLoaded = false;

private:
	void SendHandshake()
	{
		SendNetMessage(HandShakeMessage());
	}

protected:
	void ProcessMessage(NetMessage* message)
	{
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
					uint8_t port;
					uint8_t state;
					port = ((MovieDataMessage*)message)->PortNumber;
					state = ((MovieDataMessage*)message)->InputState;

					_virtualControllers[port]->PushState(state);
				}
				break;
			case MessageType::GameInformation:
				GameInformationMessage* gameInfo;
				gameInfo = (GameInformationMessage*)message;
				Console::DisplayMessage(wstring(L"Connected as player ") + std::to_wstring(((GameInformationMessage*)message)->ControllerPort + 1) + L".");
				_virtualControllers.clear();
				_gameLoaded = gameInfo->AttemptLoadGame();
				break;
		}
	}

public:
	GameClientConnection(shared_ptr<Socket> socket) : GameConnection(socket)
	{
		_controlDevice = ControlManager::GetControlDevice(0);
		ControlManager::BackupControlDevices();

		Console::DisplayMessage(L"Connected to server.");

		SendHandshake();
	}

	~GameClientConnection()
	{
		_virtualControllers.clear();
		ControlManager::RestoreControlDevices();
		Console::DisplayMessage(L"Connection to server lost.");
	}
	
	void SendInput()
	{
		uint8_t inputState = _controlDevice->GetButtonState().ToByte();
		if(_lastInputSent != inputState) {
			SendNetMessage(InputDataMessage(inputState));
			_lastInputSent = inputState;
		}
	}
};