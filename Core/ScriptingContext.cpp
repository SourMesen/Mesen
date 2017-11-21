#include "stdafx.h"
#include <algorithm>
#include "ScriptingContext.h"
#include "DebuggerTypes.h"
#include "SaveStateManager.h"

string ScriptingContext::_log = "";

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

string ScriptingContext::GetScriptName()
{
	return _scriptName;
}

void ScriptingContext::CallMemoryCallback(uint16_t addr, uint8_t &value, CallbackType type)
{
	_inExecOpEvent = type == CallbackType::CpuExec;
	InternalCallMemoryCallback(addr, value, type);
	_inExecOpEvent = false;
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

bool ScriptingContext::CheckInExecOpEvent()
{
	return _inExecOpEvent;
}

bool ScriptingContext::CheckStateLoadedFlag()
{
	bool stateLoaded = _stateLoaded;
	_stateLoaded = false;
	return stateLoaded;
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

void ScriptingContext::RequestSaveState(int slot)
{
	_saveSlot = slot;
	if(_inExecOpEvent) {
		SaveState();
	} else {
		_saveSlotData.erase(slot);
	}
}

bool ScriptingContext::RequestLoadState(int slot)
{
	if(_saveSlotData.find(slot) != _saveSlotData.end()) {
		_loadSlot = slot;
		if(_inExecOpEvent) {
			return LoadState();
		} else {
			return true;
		}
	}
	return false;
}

void ScriptingContext::SaveState()
{
	if(_saveSlot >= 0) {
		stringstream ss;
		SaveStateManager::SaveState(ss);
		_saveSlotData[_saveSlot] = ss.str();
		_saveSlot = -1;
	}
}

bool ScriptingContext::LoadState()
{
	if(_loadSlot >= 0 && _saveSlotData.find(_loadSlot) != _saveSlotData.end()) {
		stringstream ss;
		ss << _saveSlotData[_loadSlot];
		bool result = SaveStateManager::LoadState(ss);
		_loadSlot = -1;
		if(result) {
			_stateLoaded = true;
		}
		return result;
	}
	return false;
}

bool ScriptingContext::LoadState(string stateData)
{
	stringstream ss;
	ss << stateData;
	bool result = SaveStateManager::LoadState(ss);
	if(result) {
		_stateLoaded = true;
	}
	return result;
}

bool ScriptingContext::ProcessSavestate()
{
	SaveState();
	return LoadState();
}

string ScriptingContext::GetSavestateData(int slot)
{
	if(slot >= 0) {
		auto result = _saveSlotData.find(slot);
		if(result != _saveSlotData.end()) {
			return result->second;
		}
	}

	return "";
}

void ScriptingContext::ClearSavestateData(int slot)
{
	if(slot >= 0) {
		_saveSlotData.erase(slot);
	}
}