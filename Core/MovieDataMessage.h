#pragma once
#include "stdafx.h"
#include "NetMessage.h"

class MovieDataMessage : public NetMessage
{
private:
	uint8_t _portNumber;
	uint8_t _inputState;

protected:
	virtual void ProtectedStreamState()
	{
		Stream<uint8_t>(_portNumber);
		Stream<uint8_t>(_inputState);
	}

public:
	MovieDataMessage(void* buffer, uint32_t length) : NetMessage(buffer, length) { }

	MovieDataMessage(uint8_t state, uint8_t port) : NetMessage(MessageType::MovieData)
	{
		_portNumber = port;
		_inputState = state;
	}

	uint8_t GetPortNumber()
	{
		return _portNumber;
	}

	uint8_t GetInputState()
	{
		return _inputState;
	}
};