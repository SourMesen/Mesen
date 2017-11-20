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
	CheatRemoved = 8,
	PpuFrameDone = 9,
	MovieEnded = 10,
	ResolutionChanged = 11,
	FdsBiosNotFound = 12,
	ConfigChanged = 13,
	DisconnectedFromServer = 14,
	PpuViewerDisplayFrame = 15,
	ExecuteShortcut = 16,
	EmulationStopped = 17,
};

class INotificationListener
{
public:
	virtual void ProcessNotification(ConsoleNotificationType type, void* parameter) = 0;
};