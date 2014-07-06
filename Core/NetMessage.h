#pragma once
#include "stdafx.h"
#include "Console.h"

enum class MessageType : uint8_t
{
	HandShake = 0,
	SaveState = 1,
	InputData = 2,
	MovieData = 3,
};

class NetMessage
{
public:
	MessageType Type;

	NetMessage(MessageType type)
	{
		Type = type;
	}

	virtual void Send(Socket &socket) = 0;
};

class HandShakeMessage : public NetMessage
{
private:
	const int CurrentVersion = 1;

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

	virtual void Send(Socket &socket)
	{
		uint32_t messageLength = sizeof(Type) + sizeof(ProtocolVersion);
		socket.BufferedSend((char*)&messageLength, sizeof(messageLength));
		socket.BufferedSend((char*)&Type, sizeof(Type));
		socket.BufferedSend((char*)&ProtocolVersion, sizeof(ProtocolVersion));
		socket.SendBuffer();
	}
};

class InputDataMessage : public NetMessage
{
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

	virtual void Send(Socket &socket)
	{
		uint32_t messageLength = sizeof(Type) + sizeof(InputState);
		socket.BufferedSend((char*)&messageLength, sizeof(messageLength));
		socket.BufferedSend((char*)&Type, sizeof(Type));
		socket.BufferedSend((char*)&InputState, sizeof(InputState));
		socket.SendBuffer();
	}
};

class SaveStateMessage : public NetMessage
{
private:
	char* _stateData;
	uint32_t _dataSize;

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

	virtual void Send(Socket &socket)
	{
		uint32_t messageLength = _dataSize + sizeof(Type);
		socket.BufferedSend((char*)&messageLength, sizeof(messageLength));
		socket.BufferedSend((char*)&Type, sizeof(Type));
		socket.BufferedSend(_stateData, _dataSize);
		socket.SendBuffer();
	}

	void LoadState()
	{
		Console::LoadState((uint8_t*)_stateData, _dataSize);
	}
};

class MovieDataMessage : public NetMessage
{
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

	virtual void Send(Socket &socket)
	{
		uint32_t messageLength = sizeof(Type) + sizeof(PortNumber) + sizeof(InputState);
		uint8_t type = (uint8_t)Type;
		uint8_t portNumber = PortNumber;
		uint8_t inputState = InputState;

		socket.BufferedSend((char*)&messageLength, sizeof(messageLength));
		socket.BufferedSend((char*)&type, sizeof(type));
		socket.BufferedSend((char*)&portNumber, sizeof(portNumber));
		socket.BufferedSend((char*)&inputState, sizeof(inputState));
		socket.SendBuffer();
	}

};
