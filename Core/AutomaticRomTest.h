#pragma once
#include "stdafx.h"
#include "INotificationListener.h"
#include "../Utilities/AutoResetEvent.h"

class AutomaticRomTest : public INotificationListener
{
private:
	AutoResetEvent _signal;
	static bool _running;
	uint16_t _prevFrameBuffer[256 * 240];
	uint32_t _errorCode;

public:
	AutomaticRomTest();
	~AutomaticRomTest();

	void ProcessNotification(ConsoleNotificationType type, void* parameter) override;
	int32_t Run(string filename);

	static bool Running();
	static uint8_t GetControllerState(uint8_t port);
};