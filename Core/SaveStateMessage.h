#pragma once
#include "stdafx.h"
#include "NetMessage.h"
#include "Console.h"
#include "CheatManager.h"

class SaveStateMessage : public NetMessage
{
private:
	uint8_t* _stateData;
	uint32_t _dataSize;

	CodeInfo* _cheats;
	uint32_t _cheatArraySize;

protected:
	virtual void ProtectedStreamState()
	{
		vector<CodeInfo> cheats;

		StreamArray((void**)&_stateData, _dataSize);

		if(_sending) {
			cheats = CheatManager::GetCheats();
			_cheats = &cheats[0];
			_cheatArraySize = (uint32_t)cheats.size() * sizeof(CodeInfo);
			StreamArray((void**)&_cheats, _cheatArraySize);
			delete[] _stateData;
		} else {
			StreamArray((void**)&_cheats, _cheatArraySize);
			for(uint32_t i = 0; i < _cheatArraySize / sizeof(CodeInfo); i++) {
				cheats.push_back(((CodeInfo*)_cheats)[i]);
			}

			CheatManager::SetCheats(cheats);
		}
	}

public:
	SaveStateMessage(void* buffer, uint32_t length) : NetMessage(buffer, length) { }
	
	SaveStateMessage() : NetMessage(MessageType::SaveState)
	{
		//Used when sending state to clients
		Console::Pause();
		stringstream state;
		Console::SaveState(state);
		Console::Resume();

		_dataSize = (uint32_t)state.tellp();
		_stateData = new uint8_t[_dataSize];
		state.read((char*)_stateData, _dataSize);
	}
	
	void LoadState()
	{
		Console::LoadState((uint8_t*)_stateData, _dataSize);
	}
};