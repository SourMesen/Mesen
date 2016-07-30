#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"

class OekaKidsTablet : public BaseControlDevice
{
private:
	bool _strobe = false;
	bool _shift = false;
	uint32_t _stateBuffer = 0;

	bool _click = false;
	bool _touch = false;
	int32_t _xPosition = 0;
	int32_t _yPosition = 0;

protected:
	virtual uint8_t RefreshState();
	uint8_t ProcessNetPlayState(uint32_t netplayState);
	void StreamState(bool saving);

public:
	using BaseControlDevice::BaseControlDevice;

	virtual uint8_t GetPortOutput();
	virtual uint32_t GetNetPlayState();

	void WriteRam(uint8_t value);
};