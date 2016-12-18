#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"

class ArkanoidController : public BaseControlDevice
{
private:
	uint32_t _stateBuffer = 0;
	bool _buttonPressed = false;
	int32_t _xPosition = 0;

	bool IsButtonPressed();

protected:
	uint8_t RefreshState() override;
	virtual void StreamState(bool saving) override;

public:
	using BaseControlDevice::BaseControlDevice;

	uint8_t GetPortOutput() override;
	void RefreshStateBuffer() override;

	virtual uint32_t GetNetPlayState() override;
	uint8_t ProcessNetPlayState(uint32_t netplayState) override;

	uint8_t GetExpansionPortOutput(uint8_t port);
};