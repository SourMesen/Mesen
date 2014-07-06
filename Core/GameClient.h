#pragma once
#include "stdafx.h"
#include "GameClientConnection.h"

class GameClient
{
private:
	shared_ptr<Socket> _socket;
	unique_ptr<GameClientConnection> _connection;
	bool _connected = false;

public:
	GameClient()
	{
	}

	~GameClient()
	{
	}

	bool Connected()
	{
		return _connected;
	}

	void Connect(const char *host, u_short port)
	{
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

	void Disconnect()
	{
		if(_connected) {
			_connected = false;
			_socket.reset();
			_connection.reset();
		}
	}

	void Exec()
	{
		if(_connected) {
			if(!_connection->ConnectionError()) {
				_connection->ProcessMessages();
				_connection->SendInput();
			} else {
				_connected = false;
				_socket.reset();
				_connection.reset();
			}
		}
	}
};

