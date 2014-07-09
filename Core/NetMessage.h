#pragma once
#include "stdafx.h"
#include "Console.h"
#include "../Utilities/CRC32.h"

enum class MessageType : uint8_t
{
	HandShake = 0,
	SaveState = 1,
	InputData = 2,
	MovieData = 3,
	GameInformation = 4,
};

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

class GameInformationMessage : public NetMessage
{
protected:
	virtual uint32_t GetMessageLength()
	{
		return sizeof(ROMFilename) + sizeof(CRC32Hash) + sizeof(ControllerPort);
	}

	virtual void ProtectedSend(Socket &socket)
	{
		socket.BufferedSend((char*)&ROMFilename, sizeof(ROMFilename));
		socket.BufferedSend((char*)&CRC32Hash, sizeof(CRC32Hash));
		socket.BufferedSend((char*)&ControllerPort, sizeof(ControllerPort));
	}

public:
	wchar_t ROMFilename[255];
	uint32_t CRC32Hash;
	uint8_t ControllerPort;

	GameInformationMessage(char *readBuffer) : NetMessage(MessageType::GameInformation)
	{
		memcpy((char*)ROMFilename, readBuffer, sizeof(ROMFilename));
		memcpy((char*)&CRC32Hash, readBuffer + sizeof(ROMFilename), sizeof(CRC32Hash));
		ControllerPort = readBuffer[sizeof(ROMFilename) + sizeof(CRC32Hash)];
	}

	GameInformationMessage(wstring filepath, uint8_t port) : NetMessage(MessageType::GameInformation)
	{
		memset(ROMFilename, 0, sizeof(ROMFilename));
		wcscpy_s(ROMFilename, FolderUtilities::GetFilename(filepath, true).c_str());
		CRC32Hash = CRC32::GetCRC(filepath);
		ControllerPort = port;
	}

	bool AttemptLoadGame()
	{
		wstring filename = ROMFilename;
		if(filename.size() > 0) {
			if(Console::AttemptLoadROM(filename, CRC32Hash)) {
				return true;
			} else {
				Console::DisplayMessage(L"Could not find matching game ROM.");
				return false;
			}
		}
		return false;
	}
};