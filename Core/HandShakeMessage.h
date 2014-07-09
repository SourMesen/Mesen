#pragma once
#include "stdafx.h"
#include "NetMessage.h"

class HandShakeMessage : public NetMessage
{
private:
	const int CurrentVersion = 1;
	
protected:
	virtual uint32_t GetMessageLength()
	{
		return sizeof(ProtocolVersion);
	}

	virtual void ProtectedSend(Socket &socket)
	{
		socket.BufferedSend((char*)&ProtocolVersion, sizeof(ProtocolVersion));
	}

public:
	uint32_t ProtocolVersion;

	HandShakeMessage(char *readBuffer) : NetMessage(MessageType::HandShake)
	{
		ProtocolVersion = *(uint32_t*)readBuffer;
	}

	HandShakeMessage() : NetMessage(MessageType::HandShake)
	{
		ProtocolVersion = 1;
	}

	bool IsValid()
	{
		return ProtocolVersion == CurrentVersion;
	}
};
