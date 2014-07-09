#pragma once
#include "stdafx.h"

enum class ConsoleNotificationType
{
	GameLoaded = 0,
	StateLoaded = 1,
	GameReset = 2,
	GamePaused = 3,
	GameStopped = 4,
};

class INotificationListener
{
public:
	virtual void ProcessNotification(ConsoleNotificationType type) = 0;
};