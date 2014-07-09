#pragma once
#include "stdafx.h"
#include "NetMessage.h"
#include "Console.h"

class SaveStateMessage : public NetMessage
{
private:
	char* _stateData;
	uint32_t _dataSize;

protected:
	virtual uint32_t GetMessageLength()
	{
		return _dataSize;
	}

	virtual void ProtectedSend(Socket &socket)
	{
		socket.BufferedSend(_stateData, _dataSize);
	}

public:
	SaveStateMessage(char *readBuffer, uint32_t bufferLength) : NetMessage(MessageType::SaveState)
	{
		_stateData = new char[bufferLength];
		_dataSize = bufferLength;
		memcpy(_stateData, readBuffer, bufferLength);
	}

	~SaveStateMessage()
	{
		delete[] _stateData;
	}

	void LoadState()
	{
		Console::LoadState((uint8_t*)_stateData, _dataSize);
	}
};