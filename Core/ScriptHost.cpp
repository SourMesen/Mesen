#include "stdafx.h"
#include "ScriptHost.h"
#include "ScriptingContext.h"
#include "LuaScriptingContext.h"

ScriptHost::ScriptHost(int scriptId)
{
	_scriptId = scriptId;
}

int ScriptHost::GetScriptId()
{
	return _scriptId;
}

const char* ScriptHost::GetLog()
{
	return _context ? _context->GetLog() : "";
}

bool ScriptHost::LoadScript(string scriptContent, Debugger* debugger)
{
	_context.reset();
	if(scriptContent.size() > 0) {
		_context.reset(new LuaScriptingContext());

		if(!_context->LoadScript(scriptContent, debugger)) {
			return false;
		}
	}
	return true;
}

void ScriptHost::ProcessCpuOperation(uint16_t addr, uint8_t value, MemoryOperationType type)
{
	if(_context) {
		switch(type) {
			case MemoryOperationType::Read: _context->CallMemoryCallback(addr, value, CallbackType::CpuRead); break;
			case MemoryOperationType::Write: _context->CallMemoryCallback(addr, value, CallbackType::CpuWrite); break;
			case MemoryOperationType::ExecOpCode: _context->CallMemoryCallback(addr, value, CallbackType::CpuExec); break;
		}
	}
}

void ScriptHost::ProcessPpuOperation(uint16_t addr, uint8_t value, MemoryOperationType type)
{
	if(_context) {
		switch(type) {
			case MemoryOperationType::Read: _context->CallMemoryCallback(addr, value, CallbackType::PpuRead); break;
			case MemoryOperationType::Write: _context->CallMemoryCallback(addr, value, CallbackType::PpuWrite); break;
		}
	}
}

void ScriptHost::ProcessEvent(EventType eventType)
{
	if(_context) {
		_context->CallEventCallback(eventType);
	}
}
