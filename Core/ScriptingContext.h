#pragma once
#include "stdafx.h"
#include <deque>
#include "../Utilities/SimpleLock.h"
#include "DebuggerTypes.h"

class Debugger;

enum class CallbackType
{
	CpuRead = 0,
	CpuWrite = 1,
	CpuExec = 2,
	PpuRead = 3,
	PpuWrite = 4
};

class ScriptingContext
{
private:
	std::deque<string> _logRows;
	string _log;
	SimpleLock _logLock;
	bool _inStartFrameEvent = false;

protected:
	vector<int> _callbacks[5][0x10000];
	vector<int> _eventCallbacks[7];

public:
	virtual bool LoadScript(string scriptContent, Debugger* debugger) = 0;

	void Log(string message);
	const char* GetLog();

	virtual void CallMemoryCallback(int addr, int value, CallbackType type) = 0;
	virtual int InternalCallEventCallback(EventType type) = 0;

	int CallEventCallback(EventType type);
	bool CheckInStartFrameEvent();

	void RegisterMemoryCallback(CallbackType type, int startAddr, int endAddr, int reference);
	void UnregisterMemoryCallback(CallbackType type, int startAddr, int endAddr, int reference);
	void RegisterEventCallback(EventType type, int reference);
	void UnregisterEventCallback(EventType type, int reference);
};
