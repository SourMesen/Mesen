#pragma once
#include "stdafx.h"

enum class ConsoleNotificationType
{
	GameLoaded = 0,
	StateLoaded = 1,
	GameReset = 2,
	GamePaused = 3,
	GameResumed = 4,
	GameStopped = 5,
	CodeBreak = 6,
	CheatAdded = 7,
	CheatRemoved = 8
};

class INotificationListener
{
public:
	virtual void ProcessNotification(ConsoleNotificationType type) = 0;
};