#pragma once
#include "stdafx.h"
#include "NetMessage.h"
#include "Console.h"
#include "CheatManager.h"

class SaveStateMessage : public NetMessage
{
private:
	vector<CodeInfo> _activeCheats;

	uint8_t* _stateData = nullptr;
	uint32_t _dataSize = 0;

	CodeInfo* _cheats = nullptr;
	uint32_t _cheatArraySize = 0;

protected:
	virtual void ProtectedStreamState()
	{
		StreamArray((void**)&_stateData, _dataSize);

		if(_sending) {
			_cheats = _activeCheats.size() > 0 ? &_activeCheats[0] : nullptr;
			_cheatArraySize = (uint32_t)_activeCheats.size() * sizeof(CodeInfo);
			StreamArray((void**)&_cheats, _cheatArraySize);
			delete[] _stateData;
		} else {
			StreamArray((void**)&_cheats, _cheatArraySize);
		}
	}

public:
	SaveStateMessage(void* buffer, uint32_t length) : NetMessage(buffer, length) { }
	
	SaveStateMessage(shared_ptr<Console> console) : NetMessage(MessageType::SaveState)
	{
		//Used when sending state to clients
		console->Pause();
		_activeCheats = console->GetCheatManager()->GetCheats();
		stringstream state;
		console->SaveState(state);
		console->Resume();

		_dataSize = (uint32_t)state.tellp();
		_stateData = new uint8_t[_dataSize];
		state.read((char*)_stateData, _dataSize);
	}
	
	void LoadState(shared_ptr<Console> console)
	{
		console->LoadState((uint8_t*)_stateData, _dataSize);

		vector<CodeInfo> cheats;
		for(uint32_t i = 0; i < _cheatArraySize / sizeof(CodeInfo); i++) {
			cheats.push_back(((CodeInfo*)_cheats)[i]);
		}
		console->GetCheatManager()->SetCheats(cheats);
	}
};