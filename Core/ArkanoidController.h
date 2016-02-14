#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"

class ArkanoidController : public BaseControlDevice
{
private:
	uint32_t _stateBuffer;
	bool _buttonPressed;
	int32_t _xPosition;

	bool IsButtonPressed();

protected:
	uint8_t RefreshState();
	virtual void StreamState(bool saving);

public:
	using BaseControlDevice::BaseControlDevice;

	uint8_t GetPortOutput();
	void RefreshStateBuffer();

	virtual uint32_t GetNetPlayState();
	uint8_t ProcessNetPlayState(uint32_t netplayState);

	uint8_t GetExpansionPortOutput(uint8_t port);
};