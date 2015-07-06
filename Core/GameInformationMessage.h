#pragma once
#include "stdafx.h"
#include "MessageManager.h"
#include "NetMessage.h"
#include "Console.h"
#include "ROMLoader.h"
#include "../Utilities/FolderUtilities.h"

class GameInformationMessage : public NetMessage
{
private:
	wchar_t *_romFilename = nullptr;
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

	GameInformationMessage(wstring filepath, uint8_t port, bool paused) : NetMessage(MessageType::GameInformation)
	{
		CopyString(&_romFilename, _romFilenameLength, FolderUtilities::GetFilename(filepath, true));
		_crc32Hash = ROMLoader::GetCRC32(filepath);
		_controllerPort = port;
		_paused = paused;
	}
	
	bool AttemptLoadGame()
	{
		wstring filename = _romFilename;
		if(filename.size() > 0) {
			if(Console::LoadROM(filename, _crc32Hash)) {
				return true;
			} else {
				MessageManager::DisplayMessage(L"Net Play", L"Could not find matching game ROM.");
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