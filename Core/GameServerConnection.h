#pragma once
#include "stdafx.h"
#include "GameConnection.h"
#include "IControlDevice.h"
#include "IGameBroadcaster.h"

class GameServerConnection : public GameConnection, public IControlDevice
{
private:
	int _controllerPort;
	list<uint8_t> _inputData;
	IGameBroadcaster* _gameBroadcaster;
	bool _handshakeCompleted = false;

private:
	void SendGameState()
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
		SendNetMessage(SaveStateMessage(buffer, size));
		delete[] buffer;
	}

protected:
	void ProcessMessage(NetMessage* message)
	{
		switch(message->Type) {
			case MessageType::HandShake:
				//Send the game's current state to the client and register the controller
				if(((HandShakeMessage*)message)->IsValid()) {
					//SendPlayerNumber();
					SendGameState();
				}
				break;
			case MessageType::InputData:
				uint8_t state = ((InputDataMessage*)message)->InputState;
				if(_inputData.size() == 0 || state != _inputData.back()) {
					_inputData.push_back(state);
				}
				break;
		}
	}
	
public:
	GameServerConnection(shared_ptr<Socket> socket, int controllerPort, IGameBroadcaster* gameBroadcaster) : GameConnection(socket)
	{
		//Server-side connection
		_gameBroadcaster = gameBroadcaster;

		_controllerPort = controllerPort;
		
		Console::DisplayMessage(L"Player " + std::to_wstring(_controllerPort) + L" connected.");

		ControlManager::BackupControlDevices();
	}

	~GameServerConnection()
	{
		Console::DisplayMessage(L"Player " + std::to_wstring(_controllerPort) + L" disconnected.");

		ControlManager::RestoreControlDevices();
	}

	void SendMovieData(uint8_t state, uint8_t port)
	{
		if(_handshakeCompleted) {
			SendNetMessage(MovieDataMessage(state, port));
		}
	}

	ButtonState GetButtonState()
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
};
