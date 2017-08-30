#pragma once
#include "stdafx.h"
#include "ScriptingContext.h"

struct lua_State;
class Debugger;

class LuaScriptingContext : public ScriptingContext
{
private:
	lua_State* _lua = nullptr;

public:
	LuaScriptingContext();
	~LuaScriptingContext();

	bool LoadScript(string scriptContent, Debugger* debugger);
	void CallMemoryCallback(int addr, int value, CallbackType type);
	int InternalCallEventCallback(EventType type);
};
