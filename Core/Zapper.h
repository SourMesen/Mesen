#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"

class Zapper : public BaseControlDevice
{
private:
	bool _pulled;
	int32_t _xPosition;
	int32_t _yPosition;

protected:
	uint8_t RefreshState();
	uint8_t ProcessNetPlayState(uint32_t netplayState);
	void StreamState(bool saving);

public:
	using BaseControlDevice::BaseControlDevice;

	uint8_t GetPortOutput();

	uint32_t GetZapperState();
	void SetPosition(double x, double y);
	void SetTriggerState(bool pulled);
};