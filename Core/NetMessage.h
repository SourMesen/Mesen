#pragma once
#include "stdafx.h"
#include "MessageType.h"
#include "../Utilities/Socket.h"

class NetMessage
{
public:
	MessageType Type;

	NetMessage(MessageType type)
	{
		Type = type;
	}

	void Send(Socket &socket)
	{
		uint32_t messageLength = GetMessageLength() + sizeof(Type);
		socket.BufferedSend((char*)&messageLength, sizeof(messageLength));
		socket.BufferedSend((char*)&Type, sizeof(Type));
		ProtectedSend(socket);
		socket.SendBuffer();
	}

protected:
	virtual uint32_t GetMessageLength() = 0;
	virtual void ProtectedSend(Socket &socket) = 0;
};
