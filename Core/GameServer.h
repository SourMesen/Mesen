#pragma once
#include "stdafx.h"
#include "GameServerConnection.h"

class GameServer : public IGameBroadcaster
{
private:
	shared_ptr<Socket> _listener;
	list<shared_ptr<GameServerConnection>> _openConnections;
	bool _initialized = false;

	void AcceptConnections()
	{
		while(true) {
			shared_ptr<Socket> socket = _listener->Accept(NULL, NULL);
			if(!socket->ConnectionError()) {
				_openConnections.push_back(shared_ptr<GameServerConnection>(new GameServerConnection(socket, 1, this)));
				std::cout << "Client connected." << std::endl;
			} else {
				break;
			}
		}
		_listener->Listen(10);
	}

	void UpdateConnections()
	{
		vector<shared_ptr<GameServerConnection>> connectionsToRemove;
		for(shared_ptr<GameServerConnection> connection : _openConnections) {
			if(connection->ConnectionError()) {
				connectionsToRemove.push_back(connection);
			} else {
				connection->ProcessMessages();
			}
		}

		for(shared_ptr<GameServerConnection> gameConnection : connectionsToRemove) {
			_openConnections.remove(gameConnection);
		}
	}

public:
	GameServer()
	{
		ControlManager::RegisterBroadcaster(this);
	}

	~GameServer()
	{
		ControlManager::UnregisterBroadcaster(this);
	}

	void Start()
	{
		Console::DisplayMessage(L"Server started.");
		_listener.reset(new Socket());
		_listener->Bind(8888);
		_listener->Listen(10);
		_initialized = true;
	}

	void Exec()
	{
		if(_initialized) {
			AcceptConnections();
			UpdateConnections();
		}
	}

	void Stop()
	{
		_initialized = false;
		_listener.reset();
		Console::DisplayMessage(L"Server stopped.");
	}

	bool Started()
	{
		return _initialized;
	}

	void BroadcastInput(uint8_t inputData, uint8_t port)
	{
		for(shared_ptr<GameServerConnection> connection : _openConnections) {
			if(!connection->ConnectionError()) {
				//Send movie stream
				connection->SendMovieData(inputData, port);
			}
		}
	}
};