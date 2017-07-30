#pragma once
#include "stdafx.h"
#include "MessageManager.h"
#include "NetMessage.h"
#include "Console.h"
#include "RomLoader.h"
#include "../Utilities/FolderUtilities.h"

class GameInformationMessage : public NetMessage
{
private:
	char* _romFilename = nullptr;
	uint32_t _romFilenameLength = 0;
	uint32_t _crc32Hash = 0;
	uint8_t _controllerPort = 0;
	bool _paused = false;

protected:
	virtual void ProtectedStreamState()
	{
		StreamArray((void**)&_romFilename, _romFilenameLength);
		Stream<uint32_t>(_crc32Hash);
		Stream<uint8_t>(_controllerPort);
		Stream<bool>(_paused);
	}

public:
	GameInformationMessage(void* buffer, uint32_t length) : NetMessage(buffer, length) { }

	GameInformationMessage(string filepath, uint32_t crc32Hash, uint8_t port, bool paused) : NetMessage(MessageType::GameInformation)
	{
		CopyString(&_romFilename, _romFilenameLength, FolderUtilities::GetFilename(filepath, true));
		_crc32Hash = crc32Hash;
		_controllerPort = port;
		_paused = paused;
	}
	
	bool AttemptLoadGame()
	{
		string filename = _romFilename;
		if(filename.size() > 0) {
			HashInfo hashInfo;
			hashInfo.Crc32Hash = _crc32Hash;
			if(Console::LoadROM(filename, hashInfo)) {
				return true;
			} else {
				MessageManager::DisplayMessage("NetPlay", "CouldNotFindRom");
				return false;
			}
		}
		return false;
	}

	uint8_t GetPort()
	{
		return _controllerPort;
	}

	bool IsPaused()
	{
		return _paused;
	}

};