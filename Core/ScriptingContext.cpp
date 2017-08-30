#include "stdafx.h"
#include <algorithm>
#include "ScriptingContext.h"
#include "DebuggerTypes.h"

void ScriptingContext::Log(string message)
{
	auto lock = _logLock.AcquireSafe();
	_logRows.push_back(message);
	if(_logRows.size() > 500) {
		_logRows.pop_front();
	}
}

const char* ScriptingContext::GetLog()
{
	auto lock = _logLock.AcquireSafe();
	stringstream ss;
	for(string &msg : _logRows) {
		ss << msg << "\n";
	}
	_log = ss.str();
	return _log.c_str();
}

int ScriptingContext::CallEventCallback(EventType type)
{
	_inStartFrameEvent = type == EventType::StartFrame;
	int returnValue = InternalCallEventCallback(type);
	_inStartFrameEvent = false;

	return returnValue;
}

bool ScriptingContext::CheckInStartFrameEvent()
{
	return _inStartFrameEvent;
}

void ScriptingContext::RegisterMemoryCallback(CallbackType type, int startAddr, int endAddr, int reference)
{
	if(endAddr < startAddr) {
		return;
	}

	if(startAddr == 0 && endAddr == 0) {
		if(type <= CallbackType::CpuExec) {
			endAddr = 0xFFFF;
		} else {
			endAddr = 0x3FFF;
		}
	}

	for(int i = startAddr; i < endAddr; i++) {
		_callbacks[(int)type][i].push_back(reference);
	}
}

void ScriptingContext::UnregisterMemoryCallback(CallbackType type, int startAddr, int endAddr, int reference)
{
	if(endAddr < startAddr) {
		return;
	}

	if(startAddr == 0 && endAddr == 0) {
		if(type <= CallbackType::CpuExec) {
			endAddr = 0xFFFF;
		} else {
			endAddr = 0x3FFF;
		}
	}

	for(int i = startAddr; i < endAddr; i++) {
		vector<int> &refs = _callbacks[(int)type][i];
		refs.erase(std::remove(refs.begin(), refs.end(), reference), refs.end());
	}
}

void ScriptingContext::RegisterEventCallback(EventType type, int reference)
{
	_eventCallbacks[(int)type].push_back(reference);
}

void ScriptingContext::UnregisterEventCallback(EventType type, int reference)
{
	vector<int> &callbacks = _eventCallbacks[(int)type];
	callbacks.erase(std::remove(callbacks.begin(), callbacks.end(), reference), callbacks.end());
}
