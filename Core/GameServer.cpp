#pragma once
#include "stdafx.h"
#include "GameServer.h"

unique_ptr<GameServer> GameServer::Instance;

void GameServer::AcceptConnections()
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

void GameServer::Start()
{
	Console::DisplayMessage(L"Server started.");
	_listener.reset(new Socket());
	_listener->Bind(8888);
	_listener->Listen(10);
	_stop = false;
	_initialized = true;
}

void GameServer::Exec()
{
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
	Console::DisplayMessage(L"Server stopped.");
}

GameServer::GameServer()
{
	ControlManager::RegisterBroadcaster(this);
	Console::RegisterNotificationListener(this);
}

GameServer::~GameServer()
{
	_stop = true;
	_serverThread->join();

	ControlManager::UnregisterBroadcaster(this);
	Console::UnregisterNotificationListener(this);
}

void GameServer::StartServer()
{
	Instance.reset(new GameServer());
	Instance->Start();
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

void GameServer::ProcessNotification(ConsoleNotificationType type)
{
	if(type == ConsoleNotificationType::StateLoaded) {
		for(shared_ptr<GameServerConnection> connection : _openConnections) {
			if(!connection->ConnectionError()) {
				connection->SendGameState();
			}
		}
	} else if(type == ConsoleNotificationType::GameLoaded) {
		for(shared_ptr<GameServerConnection> connection : _openConnections) {
			if(!connection->ConnectionError()) {
				connection->SendGameInformation();
				connection->SendGameState();
			}
		}
	}
}