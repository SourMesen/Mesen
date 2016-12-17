#include "stdafx.h"
#include <thread>
using std::thread;

#include "MessageManager.h"
#include "GameServer.h"
#include "Console.h"
#include "../Utilities/Socket.h"
#include "PlayerListMessage.h"

unique_ptr<GameServer> GameServer::Instance;

GameServer::GameServer(uint16_t listenPort, string hostPlayerName)
{
	_stop = false;
	_port = listenPort;
	_hostPlayerName = hostPlayerName;
	_hostControllerPort = 0;
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
			_openConnections.push_back(shared_ptr<GameServerConnection>(new GameServerConnection(socket)));
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

list<shared_ptr<GameServerConnection>> GameServer::GetConnectionList()
{
	if(GameServer::Started()) {
		return Instance->_openConnections;
	} else {
		return list<shared_ptr<GameServerConnection>>();
	}
}

void GameServer::Exec()
{
	_listener.reset(new Socket());
	_listener->Bind(_port);
	_listener->Listen(10);
	_stop = false;
	_initialized = true;
	MessageManager::DisplayMessage("NetPlay" , "ServerStarted", std::to_string(_port));

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
	MessageManager::DisplayMessage("NetPlay", "ServerStopped");
}

void GameServer::StartServer(uint16_t port, string hostPlayerName)
{
	Instance.reset(new GameServer(port, hostPlayerName));
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

string GameServer::GetHostPlayerName()
{
	if(GameServer::Started()) {
		return Instance->_hostPlayerName;
	}
	return "";
}

uint8_t GameServer::GetHostControllerPort()
{
	if(GameServer::Started()) {
		return Instance->_hostControllerPort;
	}
	return GameConnection::SpectatorPort;
}

void GameServer::SetHostControllerPort(uint8_t port)
{
	if(GameServer::Started()) {
		Console::Pause();
		if(port == GameConnection::SpectatorPort || GetAvailableControllers() & (1 << port)) {
			//Port is available
			Instance->_hostControllerPort = port;
			SendPlayerList();
		}
		Console::Resume();
	}
}

uint8_t GameServer::GetAvailableControllers()
{
	uint8_t availablePorts = 0x0F;
	for(PlayerInfo &playerInfo : GetPlayerList()) {
		if(playerInfo.ControllerPort < 4) {
			availablePorts &= ~(1 << playerInfo.ControllerPort);
		}
	}
	return availablePorts;
}

vector<PlayerInfo> GameServer::GetPlayerList()
{
	vector<PlayerInfo> playerList;

	PlayerInfo playerInfo;
	playerInfo.Name = GetHostPlayerName();
	playerInfo.ControllerPort = GetHostControllerPort();
	playerInfo.IsHost = true;
	playerList.push_back(playerInfo);

	for(shared_ptr<GameServerConnection> &connection : GetConnectionList()) {
		playerInfo.Name = connection->GetPlayerName();
		playerInfo.ControllerPort = connection->GetControllerPort();
		playerInfo.IsHost = false;
		playerList.push_back(playerInfo);
	}

	return playerList;
}

void GameServer::SendPlayerList()
{
	vector<PlayerInfo> playerList = GetPlayerList();

	for(shared_ptr<GameServerConnection> &connection : GetConnectionList()) {
		//Send player list update to all connections
		PlayerListMessage message(playerList);
		connection->SendNetMessage(message);
	}
}