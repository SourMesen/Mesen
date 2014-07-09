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

protected:
	void ProcessMessage(NetMessage* message);
	
public:
	GameServerConnection(shared_ptr<Socket> socket, int controllerPort, IGameBroadcaster* gameBroadcaster);
	~GameServerConnection();

	void SendGameState();
	void SendGameInformation();
	void SendMovieData(uint8_t state, uint8_t port);

	ButtonState GetButtonState();
};
