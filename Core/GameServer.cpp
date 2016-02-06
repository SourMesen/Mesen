#include "stdafx.h"
#include <thread>
using std::thread;

#include "MessageManager.h"
#include "GameServer.h"
#include "Console.h"
#include "../Utilities/Socket.h"

unique_ptr<GameServer> GameServer::Instance;

GameServer::GameServer()
{
	ControlManager::RegisterBroadcaster(this);
}

GameServer::~GameServer()
{
	_stop = true;
	_serverThread->join();

	Stop();

	ControlManager::UnregisterBroadcaster(this);
}

void GameServer::AcceptConnections()
{
	while(true) {
		shared_ptr<Socket> socket = _listener->Accept();
		if(!socket->ConnectionError()) {
			_openConnections.push_back(shared_ptr<GameServerConnection>(new GameServerConnection(socket, 1)));
		} else {
			break;
		}
	}
	_listener->Listen(10);
}

void GameServer::UpdateConnections()
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

void GameServer::Exec()
{
	_listener.reset(new Socket());
	_listener->Bind(_port);
	_listener->Listen(10);
	_stop = false;
	_initialized = true;
	MessageManager::DisplayMessage("Net Play" , "Server started (Port: " + std::to_string(_port) + ")");

	while(!_stop) {
		AcceptConnections();
		UpdateConnections();

		std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(1));
	}
}

void GameServer::Stop()
{
	_initialized = false;
	_listener.reset();
	MessageManager::DisplayMessage("Net Play", "Server stopped");
}

void GameServer::StartServer(uint16_t port)
{
	Instance.reset(new GameServer());
	Instance->_port = port;
	Instance->_serverThread.reset(new thread(&GameServer::Exec, Instance.get()));
}

void GameServer::StopServer()
{
	if(Instance) {
		Instance.reset();
	}
}

bool GameServer::Started()
{
	if(Instance) {
		return Instance->_initialized;
	} else {
		return false;
	}
}

void GameServer::BroadcastInput(uint8_t inputData, uint8_t port)
{
	for(shared_ptr<GameServerConnection> connection : _openConnections) {
		if(!connection->ConnectionError()) {
			//Send movie stream
			connection->SendMovieData(inputData, port);
		}
	}
}