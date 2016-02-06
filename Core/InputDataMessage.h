#pragma once
#include "stdafx.h"
#include "NetMessage.h"

class InputDataMessage : public NetMessage
{
private:
	uint32_t _inputState;

protected:	
	virtual void ProtectedStreamState()
	{
		Stream<uint32_t>(_inputState);
	}

public:
	InputDataMessage(void* buffer, uint32_t length) : NetMessage(buffer, length) { }

	InputDataMessage(uint32_t inputState) : NetMessage(MessageType::InputData)
	{
		_inputState = inputState;
	}

	uint32_t GetInputState()
	{
		return _inputState;
	}
};