#pragma once
#include "stdafx.h"
#include <thread>
#include "GameServerConnection.h"
#include "INotificationListener.h"

using std::thread;

class GameServer : public IGameBroadcaster
{
private:
	static unique_ptr<GameServer> Instance;
	unique_ptr<thread> _serverThread;
	atomic<bool> _stop;

	unique_ptr<Socket> _listener;
	uint16_t _port;
	list<shared_ptr<GameServerConnection>> _openConnections;
	bool _initialized = false;

	void AcceptConnections();
	void UpdateConnections();

	void Exec();
	void Stop();

public:
	GameServer();
	~GameServer();

	static void StartServer(uint16_t port);
	static void StopServer();
	static bool Started();

	virtual void BroadcastInput(uint8_t inputData, uint8_t port);
};