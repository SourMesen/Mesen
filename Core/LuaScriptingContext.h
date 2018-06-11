#pragma once
#include "stdafx.h"
#include "ScriptingContext.h"

struct lua_State;
class Debugger;

class LuaScriptingContext : public ScriptingContext
{
private:
	lua_State* _lua = nullptr;

protected:
	void InternalCallMemoryCallback(uint16_t addr, uint8_t &value, CallbackType type) override;
	int InternalCallEventCallback(EventType type) override;

public:
	LuaScriptingContext();
	virtual ~LuaScriptingContext();

	bool LoadScript(string scriptName, string scriptContent, Debugger* debugger) override;
	
	void UnregisterMemoryCallback(CallbackType type, int startAddr, int endAddr, int reference) override;
	void UnregisterEventCallback(EventType type, int reference) override;
};
