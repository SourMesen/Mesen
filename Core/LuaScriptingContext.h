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
	void InternalCallMemoryCallback(uint16_t addr, uint8_t &value, CallbackType type);
	int InternalCallEventCallback(EventType type);

public:
	LuaScriptingContext();
	~LuaScriptingContext();

	bool LoadScript(string scriptName, string scriptContent, Debugger* debugger);
};
