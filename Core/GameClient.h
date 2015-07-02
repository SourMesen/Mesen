#pragma once

#include "stdafx.h"
#include "GameClientConnection.h"
#include "INotificationListener.h"

class ClientConnectionData;
class thread;

class GameClient : public INotificationListener
{
private:
	static unique_ptr<GameClient> Instance;
	unique_ptr<thread> _clientThread;
	atomic<bool> _stop;

	shared_ptr<Socket> _socket;
	unique_ptr<GameClientConnection> _connection;
	bool _connected = false;

	void PrivateConnect(shared_ptr<ClientConnectionData> connectionData);
	void Exec();
	void PrivateDisconnect();

public:
	GameClient();
	~GameClient();

	static bool Connected();
	static void Connect(shared_ptr<ClientConnectionData> connectionData);
	static void Disconnect();

	void ProcessNotification(ConsoleNotificationType type);
};