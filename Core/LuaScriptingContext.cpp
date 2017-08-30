#include "stdafx.h"
#include "../Lua/lua.hpp"
#include "LuaScriptingContext.h"
#include "LuaApi.h"
#include "LuaCallHelper.h"
#include "DebuggerTypes.h"
#include "Debugger.h"

LuaScriptingContext::LuaScriptingContext() { }

LuaScriptingContext::~LuaScriptingContext()
{
	if(_lua) {
		lua_close(_lua);
		_lua = nullptr;
	}
}

bool LuaScriptingContext::LoadScript(string scriptContent, Debugger* debugger)
{
	int iErr = 0;
	_lua = luaL_newstate();
	LuaApi::RegisterDebugger(debugger);
	LuaApi::SetContext(this);

	luaL_openlibs(_lua);
	luaL_requiref(_lua, "emu", LuaApi::GetLibrary, 1);
	Log("Loading script...");
	if((iErr = luaL_loadstring(_lua, scriptContent.c_str())) == 0) {
		if((iErr = lua_pcall(_lua, 0, LUA_MULTRET, 0)) == 0) {
			//Script loaded properly
			Log("Script loaded successfully.");
			return true;
		}
	}

	if(lua_isstring(_lua, -1)) {
		Log(lua_tostring(_lua, -1));
	}
	return false;
}

void LuaScriptingContext::CallMemoryCallback(int addr, int value, CallbackType type)
{
	LuaApi::SetContext(this);
	for(int &ref : _callbacks[(int)type][addr]) {
		lua_rawgeti(_lua, LUA_REGISTRYINDEX, ref);
		lua_pushinteger(_lua, addr);
		lua_pushinteger(_lua, value);
		if(lua_pcall(_lua, 2, 0, 0) != 0) {
			Log(lua_tostring(_lua, -1));
		}
	}
}

int LuaScriptingContext::InternalCallEventCallback(EventType type)
{
	LuaApi::SetContext(this);
	LuaCallHelper l(_lua);
	for(int &ref : _eventCallbacks[(int)type]) {
		lua_rawgeti(_lua, LUA_REGISTRYINDEX, ref);
		if(lua_pcall(_lua, 0, 0, 0) != 0) {
			Log(lua_tostring(_lua, -1));
		}
	}
	return l.ReturnCount();
}
