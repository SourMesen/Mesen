#pragma once

#include "stdafx.h"

class Socket
{
private:
	uintptr_t _socket = ~0;
	bool _connectionError = false;
	bool _cleanupWSA = false;
	char* _sendBuffer;
	int _bufferPosition;
	int32_t _UPnPPort = -1;

public:
	Socket();
	Socket(uintptr_t socket);
	~Socket();

	void SetSocketOptions();
	void SetConnectionErrorFlag();

	void Close();
	bool ConnectionError();

	void Bind(uint16_t port);
	bool Connect(const char* hostname, uint16_t port);
	void Listen(int backlog);
	shared_ptr<Socket> Accept();

	int Send(char *buf, int len, int flags);
	void BufferedSend(char *buf, int len);
	void SendBuffer();
	int Recv(char *buf, int len, int flags);
};
