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
	FdsDiskChanged = 12,
	FdsBiosNotFound = 13,
	ConfigChanged = 14,
	DisconnectedFromServer = 15,
	PpuViewerDisplayFrame = 16,
	RequestExit = 17,
	ToggleCheats = 18,
	ToggleAudio = 19,
};

class INotificationListener
{
public:
	virtual void ProcessNotification(ConsoleNotificationType type, void* parameter) = 0;
};