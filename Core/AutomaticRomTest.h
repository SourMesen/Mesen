#pragma once
#include "stdafx.h"
#include "INotificationListener.h"
#include "IInputProvider.h"
#include "../Utilities/AutoResetEvent.h"

class Console;

class AutomaticRomTest : public INotificationListener, public IInputProvider, public std::enable_shared_from_this<AutomaticRomTest>
{
private:
	shared_ptr<Console> _console;

	AutoResetEvent _signal;
	uint16_t _prevFrameBuffer[256 * 240];
	uint32_t _errorCode;

public:
	AutomaticRomTest();
	virtual ~AutomaticRomTest();

	void ProcessNotification(ConsoleNotificationType type, void* parameter) override;
	int32_t Run(string filename);

	// Inherited via IInputProvider
	virtual bool SetInput(BaseControlDevice * device) override;
};