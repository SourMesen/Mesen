#pragma once
#include "stdafx.h"
#include "NetMessage.h"
#include "Console.h"

class SaveStateMessage : public NetMessage
{
private:
	void* _stateData;
	uint32_t _dataSize;

protected:
	virtual void ProtectedStreamState()
	{
		StreamArray(&_stateData, _dataSize);
	}

public:
	SaveStateMessage(void* buffer, uint32_t length) : NetMessage(buffer, length) { }
	
	SaveStateMessage(void* stateData, uint32_t dataSize, bool forSend) : NetMessage(MessageType::SaveState)
	{ 
		_stateData = stateData;
		_dataSize = dataSize;
	}
	
	void LoadState()
	{
		Console::LoadState((uint8_t*)_stateData, _dataSize);
	}
};