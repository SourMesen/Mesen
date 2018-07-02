#include "stdafx.h"
#include "../Lua/lua.hpp"
#include "LuaScriptingContext.h"
#include "LuaApi.h"
#include "LuaCallHelper.h"
#include "DebuggerTypes.h"
#include "Debugger.h"

LuaScriptingContext::LuaScriptingContext(Debugger* debugger) : ScriptingContext(debugger)
{
}

LuaScriptingContext::~LuaScriptingContext()
{
	if(_lua) {
		//Cleanup all references, this is required to prevent crashes that can occur when calling lua_close
		std::unordered_set<int> references;
		for(int i = (int)CallbackType::CpuRead; i <= (int)CallbackType::PpuWrite; i++) {
			for(int addr = 0; addr < 0x10000; addr++ ){
				for(int &ref : _callbacks[i][addr]) {
					references.emplace(ref);
				}
			}
		}

		for(int i = (int)EventType::Reset; i <= (int)EventType::StateSaved; i++) {
			for(int &ref : _eventCallbacks[i]) {
				references.emplace(ref);
			}
		}

		for(const int &ref : references) {
			luaL_unref(_lua, LUA_REGISTRYINDEX, ref);
		}

		lua_close(_lua);
		_lua = nullptr;
	}
}

bool LuaScriptingContext::LoadScript(string scriptName, string scriptContent, Debugger* debugger)
{
	_scriptName = scriptName;

	int iErr = 0;
	_lua = luaL_newstate();
	LuaApi::SetContext(this);

	luaL_openlibs(_lua);
	luaL_requiref(_lua, "emu", LuaApi::GetLibrary, 1);
	Log("Loading script...");
	if((iErr = luaL_loadbufferx(_lua, scriptContent.c_str(), scriptContent.size(), ("@" + scriptName).c_str(), nullptr)) == 0) {
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

void LuaScriptingContext::UnregisterMemoryCallback(CallbackType type, int startAddr, int endAddr, int reference)
{
	ScriptingContext::UnregisterMemoryCallback(type, startAddr, endAddr, reference);
	luaL_unref(_lua, LUA_REGISTRYINDEX, reference);
}

void LuaScriptingContext::UnregisterEventCallback(EventType type, int reference)
{
	ScriptingContext::UnregisterEventCallback(type, reference);
	luaL_unref(_lua, LUA_REGISTRYINDEX, reference);
}

void LuaScriptingContext::InternalCallMemoryCallback(uint16_t addr, uint8_t &value, CallbackType type)
{
	LuaApi::SetContext(this);
	for(int &ref : _callbacks[(int)type][addr]) {
		int top = lua_gettop(_lua);
		lua_rawgeti(_lua, LUA_REGISTRYINDEX, ref);
		lua_pushinteger(_lua, addr);
		lua_pushinteger(_lua, value);		
		if(lua_pcall(_lua, 2, LUA_MULTRET, 0) != 0) {
			Log(lua_tostring(_lua, -1));
		} else {
			int returnParamCount = lua_gettop(_lua) - top;
			if(returnParamCount && lua_isinteger(_lua, -1)) {
				int newValue = (int)lua_tointeger(_lua, -1);
				value = (uint8_t)newValue;
			}
			lua_settop(_lua, top);
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
