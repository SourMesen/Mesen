#pragma once
#include "stdafx.h"
#include "NetMessage.h"

class MovieDataMessage : public NetMessage
{
protected:
	virtual uint32_t GetMessageLength()
	{
		return sizeof(PortNumber) + sizeof(InputState);
	}

	virtual void ProtectedSend(Socket &socket)
	{
		socket.BufferedSend((char*)&PortNumber, sizeof(PortNumber));
		socket.BufferedSend((char*)&InputState, sizeof(InputState));
	}

public:
	uint8_t PortNumber;
	uint8_t InputState;

	MovieDataMessage(char *readBuffer) : NetMessage(MessageType::MovieData)
	{
		PortNumber = readBuffer[0];
		InputState = readBuffer[1];
	}

	MovieDataMessage(uint8_t state, uint8_t port) : NetMessage(MessageType::MovieData)
	{
		PortNumber = port;
		InputState = state;
	}
};