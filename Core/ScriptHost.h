#pragma once
#include "stdafx.h"
#include "DebuggerTypes.h"

class ScriptingContext;
class Debugger;

class ScriptHost
{
private:
	shared_ptr<ScriptingContext> _context;
	int _scriptId;

public:
	ScriptHost(int scriptId);

	int GetScriptId();
	const char* GetLog();

	bool LoadScript(string scriptContent, Debugger* debugger);

	void ProcessCpuOperation(uint16_t addr, uint8_t &value, MemoryOperationType type);
	void ProcessPpuOperation(uint16_t addr, uint8_t &value, MemoryOperationType type);
	void ProcessEvent(EventType eventType);
};