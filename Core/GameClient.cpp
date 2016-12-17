#include "stdafx.h"
#include <thread>
using std::thread;

#include "MessageManager.h"
#include "GameClient.h"
#include "Console.h"
#include "../Utilities/Socket.h"
#include "ClientConnectionData.h"
#include "GameClientConnection.h"

unique_ptr<GameClient> GameClient::Instance;

GameClient::GameClient()
{
	_stop = false;
	
	MessageManager::RegisterNotificationListener(this);
}

GameClient::~GameClient()
{
	_stop = true;
	if(_clientThread) {
		_clientThread->join();
	}
	MessageManager::UnregisterNotificationListener(this);
}

bool GameClient::Connected()
{
	if(Instance) {
		return Instance->_connected;
	} else {
		return false;
	}
}

void GameClient::Connect(shared_ptr<ClientConnectionData> connectionData)
{
	Instance.reset(new GameClient());
	Instance->PrivateConnect(connectionData);
	Instance->_clientThread.reset(new thread(&GameClient::Exec, Instance.get()));
}

void GameClient::Disconnect()
{
	Instance.reset();
}

void GameClient::PrivateConnect(shared_ptr<ClientConnectionData> connectionData)
{
	_stop = false;
	_socket.reset(new Socket());
	if(_socket->Connect(connectionData->Host.c_str(), connectionData->Port)) {
		_connection.reset(new GameClientConnection(_socket, connectionData));
		_connected = true;
	} else {
		MessageManager::DisplayMessage("NetPlay", "CouldNotConnect");
		_connected = false;
		_socket.reset();
	}
}

void GameClient::Exec()
{
	if(_connected) {
		while(!_stop) {
			if(!_connection->ConnectionError()) {
				_connection->ProcessMessages();
				_connection->SendInput();
			} else {
				_connected = false;
				_socket.reset();
				_connection.reset();
				break;
			}
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(1));
		}
	}
}

void GameClient::ProcessNotification(ConsoleNotificationType type, void* parameter)
{
	if(type == ConsoleNotificationType::GameLoaded && std::this_thread::get_id() != _clientThread->get_id()) {
		GameClient::Disconnect();		
	}
}

uint8_t GameClient::GetControllerState(uint8_t port)
{
	if(Instance && Instance->_connection) {
		return Instance->_connection->GetControllerState(port);
	}

	return 0;
}

void GameClient::SelectController(uint8_t port)
{
	if(Instance && Instance->_connection) {
		Instance->_connection->SelectController(port);
	}
}

uint8_t GameClient::GetAvailableControllers()
{
	if(Instance && Instance->_connection) {
		return Instance->_connection->GetAvailableControllers();
	}

	return 0;
}

uint8_t GameClient::GetControllerPort()
{
	if(Instance && Instance->_connection) {
		return Instance->_connection->GetControllerPort();
	}

	return GameConnection::SpectatorPort;
}