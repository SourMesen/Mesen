#pragma once
#include "stdafx.h"
#include "NetMessage.h"
#include "Console.h"
#include "ROMLoader.h"
#include "../Utilities/FolderUtilities.h"

class GameInformationMessage : public NetMessage
{
protected:
	virtual uint32_t GetMessageLength()
	{
		return sizeof(ROMFilename) + sizeof(CRC32Hash) + sizeof(ControllerPort) + sizeof(Paused);
	}

	virtual void ProtectedSend(Socket &socket)
	{
		socket.BufferedSend((char*)&ROMFilename, sizeof(ROMFilename));
		socket.BufferedSend((char*)&CRC32Hash, sizeof(CRC32Hash));
		socket.BufferedSend((char*)&ControllerPort, sizeof(ControllerPort));
		socket.BufferedSend((char*)&Paused, sizeof(Paused));
	}

public:
	wchar_t ROMFilename[255];
	uint32_t CRC32Hash;
	uint8_t ControllerPort;
	bool Paused;

	GameInformationMessage(char *readBuffer) : NetMessage(MessageType::GameInformation)
	{
		memcpy((char*)ROMFilename, readBuffer, sizeof(ROMFilename));
		memcpy((char*)&CRC32Hash, readBuffer + sizeof(ROMFilename), sizeof(CRC32Hash));
		ControllerPort = readBuffer[sizeof(ROMFilename) + sizeof(CRC32Hash)];
		Paused = readBuffer[sizeof(ROMFilename) + sizeof(CRC32Hash) + sizeof(ControllerPort)] == 1;
	}

	GameInformationMessage(wstring filepath, uint8_t port, bool paused) : NetMessage(MessageType::GameInformation)
	{
		memset(ROMFilename, 0, sizeof(ROMFilename));
		wcscpy_s(ROMFilename, FolderUtilities::GetFilename(filepath, true).c_str());
		CRC32Hash = ROMLoader::GetCRC32(filepath);
		ControllerPort = port;
		Paused = paused;
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