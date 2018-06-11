#pragma once
#include "stdafx.h"
#include <thread>
#include "INotificationListener.h"
using std::thread;
class Socket;
class GameClientConnection;
class ClientConnectionData;

class GameClient : public INotificationListener
{
private:
	static shared_ptr<GameClient> _instance;
	unique_ptr<thread> _clientThread;
	atomic<bool> _stop;

	shared_ptr<GameClientConnection> _connection;
	bool _connected = false;

	static shared_ptr<GameClientConnection> GetConnection();

	void PrivateConnect(shared_ptr<ClientConnectionData> connectionData);
	void Exec();

public:
	GameClient();
	virtual ~GameClient();

	static bool Connected();
	static void Connect(shared_ptr<ClientConnectionData> connectionData);
	static void Disconnect();

	static void SelectController(uint8_t port);
	static uint8_t GetControllerPort();
	static uint8_t GetAvailableControllers();

	void ProcessNotification(ConsoleNotificationType type, void* parameter) override;
};