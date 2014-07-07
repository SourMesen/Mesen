#pragma once
#include "stdafx.h"
#include "GameConnection.h"
#include "VirtualController.h"

class GameClientConnection : public GameConnection
{
private:
	vector<unique_ptr<VirtualController>> _virtualControllers;
	IControlDevice* _controlDevice;

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
				Console::Pause();
						
				((SaveStateMessage*)message)->LoadState();
						
				_virtualControllers.clear();
				for(int i = 0; i < 4; i++) {
					_virtualControllers.push_back(unique_ptr<VirtualController>(new VirtualController(i)));
				}

				Console::Resume();
				break;
			case MessageType::MovieData:
				uint8_t port = ((MovieDataMessage*)message)->PortNumber;
				uint8_t state = ((MovieDataMessage*)message)->InputState;

				_virtualControllers[port]->PushState(state);
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
		SendNetMessage(InputDataMessage(_controlDevice->GetButtonState().ToByte()));
	}
};