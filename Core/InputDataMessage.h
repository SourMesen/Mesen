#pragma once
#include "stdafx.h"
#include "NetMessage.h"

class InputDataMessage : public NetMessage
{
protected:	
	virtual uint32_t GetMessageLength()
	{
		return sizeof(InputState);
	}

	virtual void ProtectedSend(Socket &socket)
	{
		socket.BufferedSend((char*)&InputState, sizeof(InputState));
	}

public:
	uint8_t InputState;

	InputDataMessage(char *readBuffer) : NetMessage(MessageType::InputData)
	{
		InputState = readBuffer[0];
	}

	InputDataMessage(uint8_t inputState) : NetMessage(MessageType::InputData)
	{
		InputState = inputState;
	}
};