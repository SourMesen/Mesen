#pragma once
#include "stdafx.h"
#include "ScriptingContext.h"
#include "../Utilities/Timer.h"

struct lua_State;
struct lua_Debug;
class Debugger;

class LuaScriptingContext : public ScriptingContext
{
private:
	static LuaScriptingContext* _context;
	static uint32_t _timeout;
	lua_State* _lua = nullptr;
	Timer _timer;

	static void ExecutionCountHook(lua_State* lua, lua_Debug* ar);

protected:
	void InternalCallMemoryCallback(uint16_t addr, uint8_t &value, CallbackType type) override;
	int InternalCallEventCallback(EventType type) override;

public:
	LuaScriptingContext(Debugger* debugger);
	virtual ~LuaScriptingContext();

	static void SetScriptTimeout(uint32_t timeout);

	bool LoadScript(string scriptName, string scriptContent, Debugger* debugger) override;
	
	void UnregisterMemoryCallback(CallbackType type, int startAddr, int endAddr, int reference) override;
	void UnregisterEventCallback(EventType type, int reference) override;
};
