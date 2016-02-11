#pragma once
#include "stdafx.h"
#include <deque>
#include "GameConnection.h"
#include "StandardController.h"
#include "IGameBroadcaster.h"
#include "INotificationListener.h"

class HandShakeMessage;

class GameServerConnection : public GameConnection, public INotificationListener
{
private:
	static GameServerConnection* _netPlayDevices[4];

	list<uint32_t> _inputData;
	int _controllerPort;	
	bool _handshakeCompleted = false;
	void PushState(uint32_t state);
	void SendGameInformation();
	void SelectControllerPort(uint8_t port);

	void SendForceDisconnectMessage(string disconnectMessage);

	void ProcessHandshakeResponse(HandShakeMessage* message);

	static void RegisterNetPlayDevice(GameServerConnection* connection, uint8_t port);
	static void UnregisterNetPlayDevice(GameServerConnection* device);
	static uint8_t GetFirstFreeControllerPort();

protected:
	void ProcessMessage(NetMessage* message);
	
public:
	GameServerConnection(shared_ptr<Socket> socket);
	~GameServerConnection();

	uint32_t GetState();
	void SendMovieData(uint8_t state, uint8_t port);

	string GetPlayerName();
	uint8_t GetControllerPort();

	virtual void ProcessNotification(ConsoleNotificationType type, void* parameter);

	static GameServerConnection* GetNetPlayDevice(uint8_t port);
};
