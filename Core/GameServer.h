#pragma once
#include "stdafx.h"
#include "GameServerConnection.h"

class GameServer : public IGameBroadcaster, public INotificationListener
{
private:
	static unique_ptr<GameServer> Instance;
	unique_ptr<thread> _serverThread;
	atomic<bool> _stop;

	unique_ptr<Socket> _listener;
	list<shared_ptr<GameServerConnection>> _openConnections;
	bool _initialized = false;

	void AcceptConnections();
	void UpdateConnections();

	void Start();
	void Exec();
	void Stop();

public:
	GameServer();
	~GameServer();

	static void StartServer();
	static void StopServer();
	static bool Started();

	virtual void BroadcastInput(uint8_t inputData, uint8_t port);
	virtual void ProcessNotification(ConsoleNotificationType type);
};