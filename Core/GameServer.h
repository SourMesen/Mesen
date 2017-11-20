#pragma once
#include "stdafx.h"
#include <thread>
#include "GameServerConnection.h"
#include "INotificationListener.h"
#include "IInputProvider.h"
#include "IInputRecorder.h"

using std::thread;

class GameServer : public IInputRecorder, public IInputProvider
{
private:
	static unique_ptr<GameServer> Instance;
	unique_ptr<thread> _serverThread;
	atomic<bool> _stop;

	unique_ptr<Socket> _listener;
	uint16_t _port;
	list<shared_ptr<GameServerConnection>> _openConnections;
	bool _initialized = false;

	string _hostPlayerName;
	uint8_t _hostControllerPort;

	void AcceptConnections();
	void UpdateConnections();

	void Exec();
	void Stop();

public:
	GameServer(uint16_t port, string hostPlayerName);
	~GameServer();

	static void StartServer(uint16_t port, string hostPlayerName);
	static void StopServer();
	static bool Started();

	static string GetHostPlayerName();
	static uint8_t GetHostControllerPort();
	static void SetHostControllerPort(uint8_t port);
	static uint8_t GetAvailableControllers();
	static vector<PlayerInfo> GetPlayerList();
	static void SendPlayerList();

	static list<shared_ptr<GameServerConnection>> GetConnectionList();

	bool SetInput(BaseControlDevice *device) override;
	void RecordInput(BaseControlDevice *device) override;
};