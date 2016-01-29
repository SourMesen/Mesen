#pragma once
#include "stdafx.h"
#include "../Utilities/SimpleLock.h"

class Socket;
class NetMessage;
class ClientConnectionData;

class GameConnection
{
protected:
	shared_ptr<Socket> _socket;
	shared_ptr<ClientConnectionData> _connectionData;
	uint8_t _readBuffer[0x40000];
	uint8_t _messageBuffer[0x40000];
	int _readPosition = 0;
	SimpleLock _socketLock;

private:
	void ReadSocket();

	bool ExtractMessage(void *buffer, uint32_t &messageLength);
	NetMessage* ReadMessage();

	virtual void ProcessMessage(NetMessage* message) = 0;

protected:
	void SendNetMessage(NetMessage &message);

public:
	GameConnection(shared_ptr<Socket> socket, shared_ptr<ClientConnectionData> connectionData);

	bool ConnectionError();

	void ProcessMessages();

};