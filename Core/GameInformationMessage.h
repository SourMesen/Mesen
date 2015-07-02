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
			if(AttemptLoadROM(filename, _crc32Hash)) {
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

	bool AttemptLoadROM(wstring filename, uint32_t crc32Hash)
	{
		if(!Console::GetROMPath().empty()) {
			if(ROMLoader::GetCRC32(Console::GetROMPath()) == crc32Hash) {
				//Current game matches, no need to do anything
				return true;
			}
		}

		vector<wstring> romFiles = FolderUtilities::GetFilesInFolder(L"D:\\Users\\Saitoh Hajime\\Desktop\\CPPApp\\NES\\Games", L"*.nes", true);
		for(wstring zipFile : FolderUtilities::GetFilesInFolder(L"D:\\Users\\Saitoh Hajime\\Desktop\\CPPApp\\NES\\Games", L"*.zip", true)) {
			romFiles.push_back(zipFile);
		}
		for(wstring romFile : romFiles) {
			//Quick search by filename
			if(FolderUtilities::GetFilename(romFile, true).compare(filename) == 0) {
				if(ROMLoader::GetCRC32(romFile) == crc32Hash) {
					//Matching ROM found
					Console::LoadROM(romFile);
					return true;
				}
			}
		}

		for(wstring romFile : romFiles) {
			//Slower search by CRC value
			if(ROMLoader::GetCRC32(romFile) == crc32Hash) {
				//Matching ROM found
				Console::LoadROM(romFile);
				return true;
			}
		}

		return false;
	}
};