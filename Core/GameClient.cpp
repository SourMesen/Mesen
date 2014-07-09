#pragma once
#include "stdafx.h"
#include "GameClient.h"
#include "Console.h"

unique_ptr<GameClient> GameClient::Instance;

GameClient::GameClient()
{
	Console::RegisterNotificationListener(this);
}

GameClient::~GameClient()
{
	_stop = true;
	_clientThread->join();
	Console::UnregisterNotificationListener(this);
}

bool GameClient::Connected()
{
	if(Instance) {
		return Instance->_connected;
	} else {
		return false;
	}
}

void GameClient::Connect(const char *host, u_short port)
{
	Instance.reset(new GameClient());
	Instance->PrivateConnect(host, port);
	Instance->_clientThread.reset(new thread(&GameClient::Exec, Instance.get()));
}

void GameClient::Disconnect()
{
	Instance.reset();
}

void GameClient::PrivateConnect(const char *host, u_short port)
{
	_stop = false;
	_socket.reset(new Socket());
	if(_socket->Connect(host, port)) {
		_connection.reset(new GameClientConnection(_socket));
		_connected = true;
	} else {
		Console::DisplayMessage(L"Could not connect to server.");
		_connected = false;
		_socket.reset();
	}
}

void GameClient::PrivateDisconnect()
{
	if(_connected) {
		_connected = false;
		_socket.reset();
		_connection.reset();
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
		}
	}
}

void GameClient::ProcessNotification(ConsoleNotificationType type)
{
	if(type == ConsoleNotificationType::GameLoaded && std::this_thread::get_id() != _clientThread->get_id()) {
		GameClient::Disconnect();		
	}
}