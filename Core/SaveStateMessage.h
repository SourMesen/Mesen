#pragma once
#include "stdafx.h"
#include "NetMessage.h"
#include "Console.h"
#include "CheatManager.h"

class SaveStateMessage : public NetMessage
{
private:
	uint8_t* _stateData = nullptr;
	uint32_t _dataSize = 0;

	CodeInfo* _cheats;
	uint32_t _cheatArraySize = 0;

protected:
	virtual void ProtectedStreamState()
	{
		StreamArray((void**)&_stateData, _dataSize);

		if(_sending) {
			vector<CodeInfo> cheats;
			cheats = CheatManager::GetCheats();
			_cheats = cheats.size() > 0 ? &cheats[0] : nullptr;
			_cheatArraySize = (uint32_t)cheats.size() * sizeof(CodeInfo);
			StreamArray((void**)&_cheats, _cheatArraySize);
			delete[] _stateData;
		} else {
			StreamArray((void**)&_cheats, _cheatArraySize);
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

		vector<CodeInfo> cheats;
		for(uint32_t i = 0; i < _cheatArraySize / sizeof(CodeInfo); i++) {
			cheats.push_back(((CodeInfo*)_cheats)[i]);
		}
		CheatManager::SetCheats(cheats);
	}
};