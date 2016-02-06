#pragma once
#include "stdafx.h"
#include <deque>
#include "GameConnection.h"
#include "StandardController.h"
#include "IGameBroadcaster.h"
#include "INotificationListener.h"

class GameServerConnection : public GameConnection, public INotificationListener
{
private:
	static GameServerConnection* _netPlayDevices[4];
	list<uint32_t> _inputData;
	int _controllerPort;	
	bool _handshakeCompleted = false;
	void PushState(uint32_t state);
	void SendGameInformation();

	static void RegisterNetPlayDevice(GameServerConnection* connection, uint8_t port);
	static void UnregisterNetPlayDevice(GameServerConnection* device);

protected:
	void ProcessMessage(NetMessage* message);
	
public:
	GameServerConnection(shared_ptr<Socket> socket, int controllerPort);
	~GameServerConnection();

	uint32_t GetState();
	void SendMovieData(uint8_t state, uint8_t port);

	virtual void ProcessNotification(ConsoleNotificationType type, void* parameter);

	static GameServerConnection* GetNetPlayDevice(uint8_t port);
};
