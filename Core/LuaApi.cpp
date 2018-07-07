#include "stdafx.h"
#include "LuaApi.h"
#include "../Utilities/HexUtilities.h"
#include "../Utilities/FolderUtilities.h"
#include "../Lua/lua.hpp"
#include "LuaCallHelper.h"
#include "Debugger.h"
#include "MemoryDumper.h"
#include "MessageManager.h"
#include "ScriptingContext.h"
#include "DebugHud.h"
#include "VideoDecoder.h"
#include "RewindManager.h"
#include "SaveStateManager.h"
#include "Console.h"
#include "IKeyManager.h"
#include "ControlManager.h"
#include "StandardController.h"
#include "PPU.h"
#include "CheatManager.h"
#include "KeyManager.h"
#include "MemoryAccessCounter.h"
#include "RomData.h"

#define lua_pushintvalue(name, value) lua_pushliteral(lua, #name); lua_pushinteger(lua, (int)value); lua_settable(lua, -3);
#define lua_pushdoublevalue(name, value) lua_pushliteral(lua, #name); lua_pushnumber(lua, (double)value); lua_settable(lua, -3);
#define lua_pushboolvalue(name, value) lua_pushliteral(lua, #name); lua_pushboolean(lua, (int)value); lua_settable(lua, -3);
#define lua_pushstringvalue(name, value) lua_pushliteral(lua, #name); lua_pushstring(lua, value.c_str()); lua_settable(lua, -3);
#define lua_starttable(name) lua_pushliteral(lua, name); lua_newtable(lua);
#define lua_endtable() lua_settable(lua, -3);
#define lua_readint(name, dest) lua_getfield(lua, -1, #name); dest = l.ReadInteger();
#define lua_readbool(name, dest) lua_getfield(lua, -1, #name); dest = l.ReadBool();
#define error(text) luaL_error(lua, text); return 0;
#define errorCond(cond, text) if(cond) { luaL_error(lua, text); return 0; }
#define checkparams() if(!l.CheckParamCount()) { return 0; }
#define checkminparams(x) if(!l.CheckParamCount(x)) { return 0; }
#define checksavestateconditions() if(!_context->CheckInStartFrameEvent() && !_context->CheckInExecOpEvent()) { error("This function must be called inside a StartFrame event callback or a CpuExec memory operation callback"); return 0; }

enum class ExecuteCountType
{
	CpuCycles = 0,
	PpuCycles = 1,
	CpuInstructions = 2
};

Debugger* LuaApi::_debugger = nullptr;
Console* LuaApi::_console = nullptr;
MemoryDumper* LuaApi::_memoryDumper = nullptr;
ScriptingContext* LuaApi::_context = nullptr;

void LuaApi::SetContext(ScriptingContext* context)
{
	_context = context;
	_debugger = _context->GetDebugger();
	_memoryDumper = _debugger->GetMemoryDumper().get();
	_console = _debugger->GetConsole();
}

int LuaApi::GetLibrary(lua_State *lua)
{
	static const luaL_Reg apilib[] = {
		{ "read", LuaApi::ReadMemory },
		{ "write", LuaApi::WriteMemory },
		{ "readWord", LuaApi::ReadMemoryWord },
		{ "writeWord", LuaApi::WriteMemoryWord },
		{ "revertPrgChrChanges", LuaApi::RevertPrgChrChanges },
		{ "addMemoryCallback", LuaApi::RegisterMemoryCallback },
		{ "removeMemoryCallback", LuaApi::UnregisterMemoryCallback },
		{ "addEventCallback", LuaApi::RegisterEventCallback },
		{ "removeEventCallback", LuaApi::UnregisterEventCallback },
		{ "drawString", LuaApi::DrawString },
		{ "drawPixel", LuaApi::DrawPixel },
		{ "drawLine", LuaApi::DrawLine },
		{ "drawRectangle", LuaApi::DrawRectangle },
		{ "clearScreen", LuaApi::ClearScreen },
		{ "getScreenBuffer", LuaApi::GetScreenBuffer },
		{ "setScreenBuffer", LuaApi::SetScreenBuffer },
		{ "getPixel", LuaApi::GetPixel },
		{ "getMouseState", LuaApi::GetMouseState },
		{ "log", LuaApi::Log },
		{ "displayMessage", LuaApi::DisplayMessage },
		{ "reset", LuaApi::Reset },
	   { "stop", LuaApi::Stop },
		{ "breakExecution", LuaApi::Break },
		{ "resume", LuaApi::Resume },
		{ "execute", LuaApi::Execute },
		{ "rewind", LuaApi::Rewind },
		{ "takeScreenshot", LuaApi::TakeScreenshot },
		{ "saveSavestate", LuaApi::SaveSavestate },
		{ "loadSavestate", LuaApi::LoadSavestate },
		{ "saveSavestateAsync", LuaApi::SaveSavestateAsync },
		{ "loadSavestateAsync", LuaApi::LoadSavestateAsync },
		{ "getSavestateData", LuaApi::GetSavestateData },
		{ "clearSavestateData", LuaApi::ClearSavestateData },
		{ "isKeyPressed", LuaApi::IsKeyPressed },
		{ "getInput", LuaApi::GetInput },
		{ "setInput", LuaApi::SetInput },
		{ "addCheat", LuaApi::AddCheat },
		{ "clearCheats", LuaApi::ClearCheats },
		{ "getAccessCounters", LuaApi::GetAccessCounters },
		{ "resetAccessCounters", LuaApi::ResetAccessCounters },
		{ "setState", LuaApi::SetState },
		{ "getState", LuaApi::GetState },
		{ "getScriptDataFolder", LuaApi::GetScriptDataFolder },
		{ "getRomInfo", LuaApi::GetRomInfo },
		{ "getLogWindowLog", LuaApi::GetLogWindowLog },
		{ NULL,NULL }
	};

	luaL_newlib(lua, apilib);

	//Expose DebugMemoryType enum as "memory.type"
	lua_pushliteral(lua, "memType");
	lua_newtable(lua);
	lua_pushintvalue(cpu, DebugMemoryType::CpuMemory);
	lua_pushintvalue(ppu, DebugMemoryType::PpuMemory);
	lua_pushintvalue(palette, DebugMemoryType::PaletteMemory);
	lua_pushintvalue(oam, DebugMemoryType::SpriteMemory);
	lua_pushintvalue(secondaryOam, DebugMemoryType::SecondarySpriteMemory);
	lua_pushintvalue(prgRom, DebugMemoryType::PrgRom);
	lua_pushintvalue(chrRom, DebugMemoryType::ChrRom);
	lua_pushintvalue(chrRam, DebugMemoryType::ChrRam);
	lua_pushintvalue(workRam, DebugMemoryType::WorkRam);
	lua_pushintvalue(saveRam, DebugMemoryType::SaveRam);
	lua_pushintvalue(cpuDebug, DebugMemoryType::CpuMemory | 0x100);
	lua_pushintvalue(ppuDebug, DebugMemoryType::PpuMemory | 0x100);
	lua_settable(lua, -3);

	lua_pushliteral(lua, "memCallbackType");
	lua_newtable(lua);
	lua_pushintvalue(cpuRead, CallbackType::CpuRead);
	lua_pushintvalue(cpuWrite, CallbackType::CpuWrite);
	lua_pushintvalue(cpuExec, CallbackType::CpuExec);
	lua_pushintvalue(ppuRead, CallbackType::PpuRead);
	lua_pushintvalue(ppuWrite, CallbackType::PpuWrite);
	lua_settable(lua, -3);

	lua_pushliteral(lua, "counterMemType");
	lua_newtable(lua);
	lua_pushintvalue(nesRam, AddressType::InternalRam);
	lua_pushintvalue(prgRom, AddressType::PrgRom);
	lua_pushintvalue(workRam, AddressType::WorkRam);
	lua_pushintvalue(saveRam, AddressType::SaveRam);
	lua_settable(lua, -3);

	lua_pushliteral(lua, "counterOpType");
	lua_newtable(lua);
	lua_pushintvalue(read, MemoryOperationType::Read);
	lua_pushintvalue(write, MemoryOperationType::Write);
	lua_pushintvalue(exec, MemoryOperationType::ExecOpCode);
	lua_settable(lua, -3);

	lua_pushliteral(lua, "eventType");
	lua_newtable(lua);
	lua_pushintvalue(reset, EventType::Reset);
	lua_pushintvalue(nmi, EventType::Nmi);
	lua_pushintvalue(irq, EventType::Irq);
	lua_pushintvalue(startFrame, EventType::StartFrame);
	lua_pushintvalue(endFrame, EventType::EndFrame);
	lua_pushintvalue(codeBreak, EventType::CodeBreak);
	lua_pushintvalue(stateLoaded, EventType::StateLoaded);
	lua_pushintvalue(stateSaved, EventType::StateSaved);
	lua_pushintvalue(inputPolled, EventType::InputPolled);
	lua_pushintvalue(scriptEnded, EventType::ScriptEnded);
	lua_settable(lua, -3);

	lua_pushliteral(lua, "executeCountType");
	lua_newtable(lua);
	lua_pushintvalue(cpuCycles, ExecuteCountType::CpuCycles);
	lua_pushintvalue(cpuInstructions, ExecuteCountType::CpuInstructions);
	lua_pushintvalue(ppuCycles, ExecuteCountType::PpuCycles);
	lua_settable(lua, -3);

	return 1;
}

int LuaApi::ReadMemory(lua_State *lua)
{
	LuaCallHelper l(lua);
	l.ForceParamCount(3);
	bool returnSignedValue = l.ReadBool();
	int type = l.ReadInteger();
	bool disableSideEffects = (type & 0x100) == 0x100;
	DebugMemoryType memType = (DebugMemoryType)(type & 0xFF);
	int address = l.ReadInteger();
	checkminparams(2);
	errorCond(address < 0, "address must be >= 0");
	uint8_t value = _memoryDumper->GetMemoryValue(memType, address, disableSideEffects);
	l.Return(returnSignedValue ? (int8_t)value : value);
	return l.ReturnCount();
}

int LuaApi::WriteMemory(lua_State *lua)
{
	LuaCallHelper l(lua);
	int type = l.ReadInteger();
	bool disableSideEffects = (type & 0x100) == 0x100;
	DebugMemoryType memType = (DebugMemoryType)(type & 0xFF);
	int value = l.ReadInteger();
	int address = l.ReadInteger();
	checkparams();
	errorCond(value > 255 || value < -128, "value out of range");
	errorCond(address < 0, "address must be >= 0");
	_memoryDumper->SetMemoryValue(memType, address, value, false, disableSideEffects);
	return l.ReturnCount();
}

int LuaApi::ReadMemoryWord(lua_State *lua)
{
	LuaCallHelper l(lua);
	l.ForceParamCount(3);
	bool returnSignedValue = l.ReadBool();
	int type = l.ReadInteger();
	bool disableSideEffects = (type & 0x100) == 0x100;
	DebugMemoryType memType = (DebugMemoryType)(type & 0xFF);
	int address = l.ReadInteger();
	checkminparams(2);
	errorCond(address < 0, "address must be >= 0");
	uint16_t value = _memoryDumper->GetMemoryValueWord(memType, address, disableSideEffects);
	l.Return(returnSignedValue ? (int16_t)value : value);
	return l.ReturnCount();
}

int LuaApi::WriteMemoryWord(lua_State *lua)
{
	LuaCallHelper l(lua);
	int type = l.ReadInteger();
	bool disableSideEffects = (type & 0x100) == 0x100;
	DebugMemoryType memType = (DebugMemoryType)(type & 0xFF);
	int value = l.ReadInteger();
	int address = l.ReadInteger();
	checkparams();
	errorCond(value > 65535 || value < -32768, "value out of range");
	errorCond(address < 0, "address must be >= 0");
	_memoryDumper->SetMemoryValueWord(memType, address, value, false, disableSideEffects);
	return l.ReturnCount();
}

int LuaApi::RevertPrgChrChanges(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();
	_debugger->RevertPrgChrChanges();
	return l.ReturnCount();
}

int LuaApi::RegisterMemoryCallback(lua_State *lua)
{
	LuaCallHelper l(lua);
	l.ForceParamCount(4);
	int32_t endAddr = l.ReadInteger(-1);
	int32_t startAddr = l.ReadInteger();
	CallbackType type = (CallbackType)l.ReadInteger();
	int reference = l.GetReference();
	checkminparams(3);

	if(endAddr == -1) {
		endAddr = startAddr;
	}

	errorCond(startAddr > endAddr, "start address must be <= end address");
	errorCond(type < CallbackType::CpuRead || type > CallbackType::PpuWrite, "the specified type is invalid");
	errorCond(reference == LUA_NOREF, "the specified function could not be found");
	_context->RegisterMemoryCallback(type, startAddr, endAddr, reference);
	_context->Log("Registered memory callback from $" + HexUtilities::ToHex((uint32_t)startAddr) + " to $" + HexUtilities::ToHex((uint32_t)endAddr));
	l.Return(reference);
	return l.ReturnCount();
}

int LuaApi::UnregisterMemoryCallback(lua_State *lua)
{
	LuaCallHelper l(lua);
	l.ForceParamCount(4);

	int endAddr = l.ReadInteger(-1);
	int startAddr = l.ReadInteger();
	CallbackType type = (CallbackType)l.ReadInteger();
	int reference = l.ReadInteger();

	checkminparams(3);

	if(endAddr == -1) {
		endAddr = startAddr;
	}

	errorCond(startAddr > endAddr, "start address must be <= end address");
	errorCond(type < CallbackType::CpuRead || type > CallbackType::PpuWrite, "the specified type is invalid");
	errorCond(reference == LUA_NOREF, "function reference is invalid");
	_context->UnregisterMemoryCallback(type, startAddr, endAddr, reference);
	return l.ReturnCount();
}

int LuaApi::RegisterEventCallback(lua_State *lua)
{
	LuaCallHelper l(lua);
	EventType type = (EventType)l.ReadInteger();
	int reference = l.GetReference();
	checkparams();
	errorCond(type < EventType::Reset || type >= EventType::EventTypeSize, "the specified type is invalid");
	errorCond(reference == LUA_NOREF, "the specified function could not be found");
	_context->RegisterEventCallback(type, reference);
	l.Return(reference);
	return l.ReturnCount();
}

int LuaApi::UnregisterEventCallback(lua_State *lua)
{
	LuaCallHelper l(lua);
	EventType type = (EventType)l.ReadInteger();
	int reference = l.ReadInteger();
	checkparams();
	errorCond(type < EventType::Reset || type >= EventType::EventTypeSize, "the specified type is invalid");
	errorCond(reference == LUA_NOREF, "function reference is invalid");
	_context->UnregisterEventCallback(type, reference);
	return l.ReturnCount();
}

int LuaApi::DrawString(lua_State *lua)
{
	LuaCallHelper l(lua);
	l.ForceParamCount(6);
	int frameCount = l.ReadInteger(1);
	int backColor = l.ReadInteger(0);
	int color = l.ReadInteger(0xFFFFFF);
	string text = l.ReadString();
	int y = l.ReadInteger();
	int x = l.ReadInteger();
	checkminparams(3);

	int startFrame = _console->GetFrameCount();
	_console->GetDebugHud()->DrawString(x, y, text, color, backColor, frameCount, startFrame);

	return l.ReturnCount();
}

int LuaApi::DrawLine(lua_State *lua)
{
	LuaCallHelper l(lua);
	l.ForceParamCount(6);
	int frameCount = l.ReadInteger(1);
	int color = l.ReadInteger(0xFFFFFF);
	int y2 = l.ReadInteger();
	int x2 = l.ReadInteger();
	int y = l.ReadInteger();
	int x = l.ReadInteger();
	checkminparams(4);

	int startFrame = _console->GetFrameCount();
	_console->GetDebugHud()->DrawLine(x, y, x2, y2, color, frameCount, startFrame);

	return l.ReturnCount();
}

int LuaApi::DrawPixel(lua_State *lua)
{
	LuaCallHelper l(lua);
	l.ForceParamCount(4);
	int frameCount = l.ReadInteger(1);
	int color = l.ReadInteger();
	int y = l.ReadInteger();
	int x = l.ReadInteger();
	checkminparams(3);

	int startFrame = _console->GetFrameCount();
	_console->GetDebugHud()->DrawPixel(x, y, color, frameCount, startFrame);

	return l.ReturnCount();
}

int LuaApi::DrawRectangle(lua_State *lua)
{
	LuaCallHelper l(lua);
	l.ForceParamCount(7);
	int frameCount = l.ReadInteger(1);
	bool fill = l.ReadBool(false);
	int color = l.ReadInteger(0xFFFFFF);
	int height = l.ReadInteger();
	int width = l.ReadInteger();
	int y = l.ReadInteger();
	int x = l.ReadInteger();
	checkminparams(4);

	int startFrame = _console->GetFrameCount();
	_console->GetDebugHud()->DrawRectangle(x, y, width, height, color, fill, frameCount, startFrame);

	return l.ReturnCount();
}

int LuaApi::ClearScreen(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();

	_console->GetDebugHud()->ClearScreen();
	return l.ReturnCount();
}

int LuaApi::GetScreenBuffer(lua_State *lua)
{
	LuaCallHelper l(lua);
	
	uint32_t *palette = EmulationSettings::GetRgbPalette();
	lua_newtable(lua);
	for(int y = 0; y < PPU::ScreenHeight; y++) {
		for(int x = 0; x < PPU::ScreenWidth; x++) {
			lua_pushinteger(lua, palette[_debugger->GetScreenPixel(x, y) & 0x3F] & 0xFFFFFF);
			lua_rawseti(lua, -2, (y << 8) + x);
		}
	}

	return 1;
}

int LuaApi::SetScreenBuffer(lua_State *lua)
{
	LuaCallHelper l(lua);

	uint32_t pixels[PPU::PixelCount] = {};
	luaL_checktype(lua, 1, LUA_TTABLE);
	for(int i = 0; i < PPU::PixelCount; i++) {
		lua_rawgeti(lua, 1, i);
		pixels[i] = l.ReadInteger() ^ 0xFF000000;
	}
	
	int startFrame = _console->GetFrameCount();
	_console->GetDebugHud()->DrawScreenBuffer(pixels, startFrame);

	return l.ReturnCount();
}

int LuaApi::GetPixel(lua_State *lua)
{
	LuaCallHelper l(lua);
	int y = l.ReadInteger();
	int x = l.ReadInteger();
	checkparams();
	errorCond(x < 0 || x > 255 || y < 0 || y > 239, "invalid x,y coordinates (must be between 0-255, 0-239)");

	//Ignores intensify & grayscale bits
	l.Return(EmulationSettings::GetRgbPalette()[_debugger->GetScreenPixel(x, y) & 0x3F] & 0xFFFFFF);
	return l.ReturnCount();
}

int LuaApi::GetMouseState(lua_State *lua)
{
	LuaCallHelper l(lua);
	MousePosition pos = KeyManager::GetMousePosition();
	checkparams();
	lua_newtable(lua);
	lua_pushintvalue(x, pos.X);
	lua_pushintvalue(y, pos.Y);
	lua_pushboolvalue(left, KeyManager::IsMouseButtonPressed(MouseButton::LeftButton));
	lua_pushboolvalue(middle, KeyManager::IsMouseButtonPressed(MouseButton::MiddleButton));
	lua_pushboolvalue(right, KeyManager::IsMouseButtonPressed(MouseButton::RightButton));
	return 1;
}

int LuaApi::Log(lua_State *lua)
{
	LuaCallHelper l(lua);
	string text = l.ReadString();
	checkparams();
	_context->Log(text);
	return l.ReturnCount();
}

int LuaApi::DisplayMessage(lua_State *lua)
{
	LuaCallHelper l(lua);
	string text = l.ReadString();
	string category = l.ReadString();
	checkparams();
	MessageManager::DisplayMessage(category, text);
	return l.ReturnCount();
}

int LuaApi::Reset(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();
	_console->Reset(true);
	return l.ReturnCount();
}

int LuaApi::Stop(lua_State *lua)
{
	LuaCallHelper l(lua);
	int32_t stopCode = l.ReadInteger(0);
	checkminparams(0);
	_console->Stop(stopCode);
	return l.ReturnCount();
}

int LuaApi::Break(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();
	_debugger->Step(1);
	return l.ReturnCount();
}

int LuaApi::Resume(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();
	_debugger->Run();
	return l.ReturnCount();
}

int LuaApi::Execute(lua_State *lua)
{
	LuaCallHelper l(lua);
	ExecuteCountType type = (ExecuteCountType)l.ReadInteger();
	int count = l.ReadInteger();
	checkparams();
	errorCond(count <= 0, "count must be >= 1");
	errorCond(type < ExecuteCountType::CpuCycles || type > ExecuteCountType::CpuInstructions, "type is invalid");

	switch(type) {
		case ExecuteCountType::CpuCycles: _debugger->StepCycles(count);  break;
		case ExecuteCountType::PpuCycles: _debugger->PpuStep(count); break;
		case ExecuteCountType::CpuInstructions: _debugger->Step(count); break;
	}
	return l.ReturnCount();
}

int LuaApi::Rewind(lua_State *lua)
{
	LuaCallHelper l(lua);
	int seconds = l.ReadInteger();
	checkparams();
	checksavestateconditions();
	errorCond(seconds <= 0, "seconds must be >= 1");
	_console->GetRewindManager()->RewindSeconds(seconds);
	return l.ReturnCount();
}

int LuaApi::TakeScreenshot(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();
	stringstream ss;
	_console->GetVideoDecoder()->TakeScreenshot(ss);
	l.Return(ss.str());
	return l.ReturnCount();
}

int LuaApi::SaveSavestate(lua_State *lua)
{
	LuaCallHelper l(lua);
	checksavestateconditions();
	stringstream ss;
	_console->GetSaveStateManager()->SaveState(ss);
	l.Return(ss.str());
	return l.ReturnCount();
}

int LuaApi::LoadSavestate(lua_State *lua)
{
	LuaCallHelper l(lua);
	string savestate = l.ReadString();
	checkparams();
	checksavestateconditions();
	l.Return(_context->LoadState(savestate));
	return l.ReturnCount();
}

int LuaApi::SaveSavestateAsync(lua_State *lua)
{
	LuaCallHelper l(lua);
	int32_t slot = l.ReadInteger();
	checkparams();
	errorCond(slot < 0, "Slot must be >= 0");
	_context->RequestSaveState(slot);
	return l.ReturnCount();
}

int LuaApi::LoadSavestateAsync(lua_State *lua)
{
	LuaCallHelper l(lua);
	int32_t slot = l.ReadInteger();
	checkparams();
	errorCond(slot < 0, "Slot must be >= 0");
	l.Return(_context->RequestLoadState(slot));
	return l.ReturnCount();
}

int LuaApi::GetSavestateData(lua_State *lua)
{
	LuaCallHelper l(lua);
	int32_t slot = l.ReadInteger();
	checkparams();
	errorCond(slot < 0, "Slot must be >= 0");
	l.Return(_context->GetSavestateData(slot));
	return l.ReturnCount();
}

int LuaApi::ClearSavestateData(lua_State *lua)
{
	LuaCallHelper l(lua);
	int32_t slot = l.ReadInteger();
	checkparams();
	errorCond(slot < 0, "Slot must be >= 0");
	_context->ClearSavestateData(slot);
	return l.ReturnCount();
}

int LuaApi::IsKeyPressed(lua_State *lua)
{
	LuaCallHelper l(lua);
	string keyName = l.ReadString();
	checkparams();
	uint32_t keyCode = KeyManager::GetKeyCode(keyName);
	errorCond(keyCode == 0, "Invalid key name");
	l.Return(KeyManager::IsKeyPressed(keyCode));
	return l.ReturnCount();
}

int LuaApi::GetInput(lua_State *lua)
{
	LuaCallHelper l(lua);
	int port = l.ReadInteger();
	checkparams();
	errorCond(port < 0 || port > 3, "Invalid port number - must be between 0 to 3");

	shared_ptr<StandardController> controller = std::dynamic_pointer_cast<StandardController>(_console->GetControlManager()->GetControlDevice(port));
	errorCond(controller == nullptr, "Input port must be connected to a standard controller");

	lua_newtable(lua);
	lua_pushboolvalue(a, controller->IsPressed(StandardController::Buttons::A));
	lua_pushboolvalue(b, controller->IsPressed(StandardController::Buttons::B));
	lua_pushboolvalue(start, controller->IsPressed(StandardController::Buttons::Start));
	lua_pushboolvalue(select, controller->IsPressed(StandardController::Buttons::Select));
	lua_pushboolvalue(up, controller->IsPressed(StandardController::Buttons::Up));
	lua_pushboolvalue(down, controller->IsPressed(StandardController::Buttons::Down));
	lua_pushboolvalue(left, controller->IsPressed(StandardController::Buttons::Left));
	lua_pushboolvalue(right, controller->IsPressed(StandardController::Buttons::Right));
	return 1;
}

int LuaApi::SetInput(lua_State *lua)
{
	LuaCallHelper l(lua);
	lua_settop(lua, 3);
	
	bool allowUserInput = l.ReadBool();

	luaL_checktype(lua, 2, LUA_TTABLE);
	lua_getfield(lua, 2, "a");
	lua_getfield(lua, 2, "b");
	lua_getfield(lua, 2, "start");
	lua_getfield(lua, 2, "select");
	lua_getfield(lua, 2, "up");
	lua_getfield(lua, 2, "down");
	lua_getfield(lua, 2, "left");
	lua_getfield(lua, 2, "right");

	Nullable<bool> right = l.ReadOptionalBool();
	Nullable<bool> left = l.ReadOptionalBool();
	Nullable<bool> down = l.ReadOptionalBool();
	Nullable<bool> up = l.ReadOptionalBool();
	Nullable<bool> select = l.ReadOptionalBool();
	Nullable<bool> start = l.ReadOptionalBool();
	Nullable<bool> b = l.ReadOptionalBool();
	Nullable<bool> a = l.ReadOptionalBool();

	lua_pop(lua, 1);
	int port = l.ReadInteger();

	errorCond(port < 0 || port > 3, "Invalid port number - must be between 0 to 3");

	shared_ptr<StandardController> controller = std::dynamic_pointer_cast<StandardController>(_console->GetControlManager()->GetControlDevice(port));
	errorCond(controller == nullptr, "Input port must be connected to a standard controller");

	if(right.HasValue || !allowUserInput) controller->SetBitValue(StandardController::Buttons::Right, right.Value);
	if(left.HasValue || !allowUserInput) controller->SetBitValue(StandardController::Buttons::Left, left.Value);
	if(down.HasValue || !allowUserInput) controller->SetBitValue(StandardController::Buttons::Down, down.Value);
	if(up.HasValue || !allowUserInput) controller->SetBitValue(StandardController::Buttons::Up, up.Value);
	if(select.HasValue || !allowUserInput) controller->SetBitValue(StandardController::Buttons::Select, select.Value);
	if(start.HasValue || !allowUserInput) controller->SetBitValue(StandardController::Buttons::Start, start.Value);
	if(b.HasValue || !allowUserInput) controller->SetBitValue(StandardController::Buttons::B, b.Value);
	if(a.HasValue || !allowUserInput) controller->SetBitValue(StandardController::Buttons::A, a.Value);

	return l.ReturnCount();
}

int LuaApi::AddCheat(lua_State *lua)
{
	LuaCallHelper l(lua);
	string gamegenieCode = l.ReadString();
	checkparams();
	errorCond(gamegenieCode.length() != 6 && gamegenieCode.length() != 8, "Game genie code must be 6 or 8 characters long");
	errorCond(gamegenieCode.find_first_not_of("APZLGITYEOXUKSVN", 0) != string::npos, "Game genie code may only contain these characters: AEGIKLNOPSTUVXYZ");
	_console->GetCheatManager()->AddGameGenieCode(gamegenieCode);
	return l.ReturnCount();
}

int LuaApi::ClearCheats(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();
	_console->GetCheatManager()->ClearCodes();
	return l.ReturnCount();
}

int LuaApi::GetAccessCounters(lua_State *lua)
{
	LuaCallHelper l(lua);
	l.ForceParamCount(2);
	MemoryOperationType operationType = (MemoryOperationType)l.ReadInteger();
	AddressType memoryType = (AddressType)l.ReadInteger();
	errorCond(operationType >= MemoryOperationType::ExecOperand, "Invalid operation type");
	errorCond(memoryType >= AddressType::Register, "Invalid memory type");
	checkparams();

	uint32_t size = 0;
	switch(memoryType) {
		case AddressType::Register: error("Invalid memory type"); break;
		case AddressType::InternalRam: size = 0x2000; break;
		case AddressType::PrgRom: size = _memoryDumper->GetMemorySize(DebugMemoryType::PrgRom); break;
		case AddressType::WorkRam: size = _memoryDumper->GetMemorySize(DebugMemoryType::WorkRam); break;
		case AddressType::SaveRam: size = _memoryDumper->GetMemorySize(DebugMemoryType::SaveRam); break;
	}

	vector<uint32_t> counts;
	counts.resize(size, 0);
	_debugger->GetMemoryAccessCounter()->GetAccessCounts(memoryType, operationType, counts.data(), false);

	lua_newtable(lua);
	for(uint32_t i = 0; i < size; i++) {
		lua_pushinteger(lua, counts[i]);
		lua_rawseti(lua, -2, i);
	}

	return 1;
}

int LuaApi::ResetAccessCounters(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();
	_debugger->GetMemoryAccessCounter()->ResetCounts();
	return l.ReturnCount();
}

int LuaApi::GetScriptDataFolder(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();
	string baseFolder = FolderUtilities::CombinePath(FolderUtilities::GetHomeFolder(), "LuaScriptData");
	FolderUtilities::CreateFolder(baseFolder);
	string scriptFolder = FolderUtilities::CombinePath(baseFolder, FolderUtilities::GetFilename(_context->GetScriptName(), false));
	FolderUtilities::CreateFolder(scriptFolder);
	l.Return(scriptFolder);
	return l.ReturnCount();
}

int LuaApi::GetRomInfo(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();

	RomInfo romInfo = _console->GetRomInfo();
	string romPath = _console->GetRomPath();

	lua_newtable(lua);
	lua_pushstringvalue(name, romInfo.RomName);
	lua_pushstringvalue(path, romPath);

	lua_pushintvalue(fileCrc32Hash, romInfo.Hash.Crc32);
	lua_pushstringvalue(fileSha1Hash, romInfo.Hash.Sha1);
	lua_pushintvalue(prgChrCrc32Hash, romInfo.Hash.PrgCrc32);
	lua_pushstringvalue(prgChrMd5Hash, romInfo.Hash.PrgChrMd5);
	lua_pushintvalue(format, romInfo.Format);
	lua_pushboolvalue(isChrRam, romInfo.HasChrRam);
	return 1;
}

int LuaApi::GetLogWindowLog(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();
	
	l.Return(MessageManager::GetLog());
	return l.ReturnCount();
}

int LuaApi::SetState(lua_State *lua)
{
	LuaCallHelper l(lua);
	lua_settop(lua, 1);
	luaL_checktype(lua, -1, LUA_TTABLE);

	DebugState state;

	lua_getfield(lua, -1, "cpu");
	luaL_checktype(lua, -1, LUA_TTABLE);
	lua_readint(a, state.CPU.A);
	lua_readint(cycleCount, state.CPU.CycleCount);
	lua_readint(irqFlag, state.CPU.IRQFlag);
	lua_readbool(nmiFlag, state.CPU.NMIFlag);
	lua_readint(pc, state.CPU.PC);
	lua_readint(ps, state.CPU.PS);
	lua_readint(sp, state.CPU.SP);
	lua_readint(x, state.CPU.X);
	lua_readint(y, state.CPU.Y);
	lua_pop(lua, 1);

	lua_getfield(lua, -1, "ppu");
	luaL_checktype(lua, -1, LUA_TTABLE);
	lua_readint(cycle, state.PPU.Cycle);
	lua_readint(frameCount, state.PPU.FrameCount);
	lua_readint(scanline, state.PPU.Scanline);

	lua_getfield(lua, -1, "control");
	luaL_checktype(lua, -1, LUA_TTABLE);
	lua_readbool(backgroundEnabled, state.PPU.ControlFlags.BackgroundEnabled);
	lua_readbool(backgroundMask, state.PPU.ControlFlags.BackgroundMask);
	lua_readint(backgroundPatternAddr, state.PPU.ControlFlags.BackgroundPatternAddr);
	lua_readbool(grayscale, state.PPU.ControlFlags.Grayscale);
	lua_readbool(intensifyBlue, state.PPU.ControlFlags.IntensifyBlue);
	lua_readbool(intensifyGreen, state.PPU.ControlFlags.IntensifyGreen);
	lua_readbool(intensifyRed, state.PPU.ControlFlags.IntensifyRed);
	lua_readbool(largeSprites, state.PPU.ControlFlags.LargeSprites);
	lua_readbool(spriteMask, state.PPU.ControlFlags.SpriteMask);
	lua_readint(spritePatternAddr, state.PPU.ControlFlags.SpritePatternAddr);
	lua_readbool(spritesEnabled, state.PPU.ControlFlags.SpritesEnabled);
	lua_readbool(nmiOnVBlank, state.PPU.ControlFlags.VBlank);
	lua_readbool(verticalWrite, state.PPU.ControlFlags.VerticalWrite);
	lua_pop(lua, 1);

	lua_getfield(lua, -1, "status");
	luaL_checktype(lua, -1, LUA_TTABLE);
	lua_readbool(sprite0Hit, state.PPU.StatusFlags.Sprite0Hit);
	lua_readbool(spriteOverflow, state.PPU.StatusFlags.SpriteOverflow);
	lua_readbool(verticalBlank, state.PPU.StatusFlags.VerticalBlank);
	lua_pop(lua, 1);

	lua_getfield(lua, -1, "state");
	luaL_checktype(lua, -1, LUA_TTABLE);
	lua_readint(control, state.PPU.State.Control);
	lua_readint(highBitShift, state.PPU.State.HighBitShift);
	lua_readint(lowBitShift, state.PPU.State.LowBitShift);
	lua_readint(mask, state.PPU.State.Mask);
	lua_readint(spriteRamAddr, state.PPU.State.SpriteRamAddr);
	lua_readint(status, state.PPU.State.Status);
	lua_readint(tmpVideoRamAddr, state.PPU.State.TmpVideoRamAddr);
	lua_readint(videoRamAddr, state.PPU.State.VideoRamAddr);
	lua_readbool(writeToggle, state.PPU.State.WriteToggle);
	lua_readint(xScroll, state.PPU.State.XScroll);
	lua_pop(lua, 1);

	lua_pop(lua, 1);

	_debugger->SetState(state);

	return 0;
}

int LuaApi::GetState(lua_State *lua)
{
	LuaCallHelper l(lua);
	checkparams();
	DebugState state;
	_debugger->GetState(&state, true);

	lua_newtable(lua);
	lua_pushintvalue(region, state.Model);
	lua_pushintvalue(clockRate, state.ClockRate);

	lua_starttable("cpu");
	lua_pushintvalue(a, state.CPU.A);
	lua_pushintvalue(cycleCount, state.CPU.CycleCount);
	lua_pushintvalue(irqFlag, state.CPU.IRQFlag);
	lua_pushboolvalue(nmiFlag, state.CPU.NMIFlag);
	lua_pushintvalue(pc, state.CPU.PC);
	lua_pushintvalue(status, state.CPU.PS);
	lua_pushintvalue(sp, state.CPU.SP);
	lua_pushintvalue(x, state.CPU.X);
	lua_pushintvalue(y, state.CPU.Y);
	lua_endtable();

	lua_starttable("ppu");
	lua_pushintvalue(cycle, state.PPU.Cycle);
	lua_pushintvalue(frameCount, state.PPU.FrameCount);
	lua_pushintvalue(scanline, state.PPU.Scanline);

	lua_starttable("control");
	lua_pushboolvalue(backgroundEnabled, state.PPU.ControlFlags.BackgroundEnabled);
	lua_pushboolvalue(backgroundMask, state.PPU.ControlFlags.BackgroundMask);
	lua_pushintvalue(backgroundPatternAddr, state.PPU.ControlFlags.BackgroundPatternAddr);
	lua_pushboolvalue(grayscale, state.PPU.ControlFlags.Grayscale);
	lua_pushboolvalue(intensifyBlue, state.PPU.ControlFlags.IntensifyBlue);
	lua_pushboolvalue(intensifyGreen, state.PPU.ControlFlags.IntensifyGreen);
	lua_pushboolvalue(intensifyRed, state.PPU.ControlFlags.IntensifyRed);
	lua_pushboolvalue(largeSprites, state.PPU.ControlFlags.LargeSprites);
	lua_pushboolvalue(spriteMask, state.PPU.ControlFlags.SpriteMask);
	lua_pushintvalue(spritePatternAddr, state.PPU.ControlFlags.SpritePatternAddr);
	lua_pushboolvalue(spritesEnabled, state.PPU.ControlFlags.SpritesEnabled);
	lua_pushboolvalue(nmiOnVBlank, state.PPU.ControlFlags.VBlank);
	lua_pushboolvalue(verticalWrite, state.PPU.ControlFlags.VerticalWrite);
	lua_endtable();

	lua_starttable("status");
	lua_pushboolvalue(sprite0Hit, state.PPU.StatusFlags.Sprite0Hit);
	lua_pushboolvalue(spriteOverflow, state.PPU.StatusFlags.SpriteOverflow);
	lua_pushboolvalue(verticalBlank, state.PPU.StatusFlags.VerticalBlank);
	lua_endtable();

	lua_starttable("state");
	lua_pushintvalue(control, state.PPU.State.Control);
	lua_pushintvalue(highBitShift, state.PPU.State.HighBitShift);
	lua_pushintvalue(lowBitShift, state.PPU.State.LowBitShift);
	lua_pushintvalue(mask, state.PPU.State.Mask);
	lua_pushintvalue(spriteRamAddr, state.PPU.State.SpriteRamAddr);
	lua_pushintvalue(status, state.PPU.State.Status);
	lua_pushintvalue(tmpVideoRamAddr, state.PPU.State.TmpVideoRamAddr);
	lua_pushintvalue(videoRamAddr, state.PPU.State.VideoRamAddr);
	lua_pushboolvalue(writeToggle, state.PPU.State.WriteToggle);
	lua_pushintvalue(xScroll, state.PPU.State.XScroll);
	lua_endtable();

	lua_endtable(); //end ppu

	lua_starttable("cart");
	lua_pushintvalue(chrPageCount, state.Cartridge.ChrPageCount);
	lua_pushintvalue(chrPageSize, state.Cartridge.ChrPageSize);
	lua_pushintvalue(chrRamSize, state.Cartridge.ChrRamSize);
	lua_pushintvalue(chrRomSize, state.Cartridge.ChrRomSize);
	lua_pushintvalue(prgPageCount, state.Cartridge.PrgPageCount);
	lua_pushintvalue(prgPageSize, state.Cartridge.PrgPageSize);
	lua_pushintvalue(prgRomSize, state.Cartridge.PrgRomSize);

	lua_starttable("selectedPrgPages");
	for(int i = 0, max = 0x8000 / state.Cartridge.PrgPageSize; i < max; i++) {
		lua_pushinteger(lua, i + 1);
		lua_pushinteger(lua, (int32_t)state.Cartridge.PrgSelectedPages[i]);
		lua_settable(lua, -3);
	}
	lua_endtable();

	lua_starttable("selectedChrPages");
	for(int i = 0, max = 0x2000 / state.Cartridge.ChrPageSize; i < max; i++) {
		lua_pushinteger(lua, i + 1);
		lua_pushinteger(lua, (int32_t)state.Cartridge.ChrSelectedPages[i]);
		lua_settable(lua, -3);
	}
	lua_endtable();

	lua_endtable();

	lua_starttable("apu");

	lua_starttable("square1");
	PushSquareState(lua, state.APU.Square1);
	lua_endtable();

	lua_starttable("square2");
	PushSquareState(lua, state.APU.Square2);
	lua_endtable();

	lua_starttable("triangle");
	lua_pushboolvalue(enabled, state.APU.Triangle.Enabled);
	lua_pushdoublevalue(frequency, state.APU.Triangle.Frequency);
	PushLengthCounterState(lua, state.APU.Triangle.LengthCounter);
	lua_pushintvalue(outputVolume, state.APU.Triangle.OutputVolume);
	lua_pushintvalue(period, state.APU.Triangle.Period);
	lua_pushintvalue(sequencePosition, state.APU.Triangle.SequencePosition);
	lua_endtable();

	lua_starttable("noise");
	lua_pushboolvalue(enabled, state.APU.Noise.Enabled);
	lua_pushdoublevalue(frequency, state.APU.Noise.Frequency);
	PushEnvelopeState(lua, state.APU.Noise.Envelope);
	PushLengthCounterState(lua, state.APU.Noise.LengthCounter);
	lua_pushboolvalue(modeFlag, state.APU.Noise.ModeFlag);
	lua_pushintvalue(outputVolume, state.APU.Noise.OutputVolume);
	lua_pushintvalue(period, state.APU.Noise.Period);
	lua_pushintvalue(shiftRegister, state.APU.Noise.ShiftRegister);
	lua_endtable();

	lua_starttable("dmc");
	lua_pushintvalue(bytesRemaining, state.APU.Dmc.BytesRemaining);
	lua_pushdoublevalue(sampleRate, state.APU.Dmc.SampleRate);
	lua_pushboolvalue(irqEnabled, state.APU.Dmc.IrqEnabled);
	lua_pushboolvalue(loop, state.APU.Dmc.Loop);
	lua_pushintvalue(outputVolume, state.APU.Dmc.OutputVolume);
	lua_pushintvalue(period, state.APU.Dmc.Period);
	lua_pushintvalue(sampleAddr, state.APU.Dmc.SampleAddr);
	lua_pushintvalue(sampleLength, state.APU.Dmc.SampleLength);
	lua_endtable();

	lua_starttable("frameCounter");
	lua_pushintvalue(fiveStepMode, state.APU.FrameCounter.FiveStepMode);
	lua_pushboolvalue(irqEnabled, state.APU.FrameCounter.IrqEnabled);
	lua_pushintvalue(sequencePosition, state.APU.FrameCounter.SequencePosition);
	lua_endtable();

	lua_endtable(); //end apu

	return 1;
}

void LuaApi::PushSquareState(lua_State *lua, ApuSquareState & state)
{
	lua_pushintvalue(duty, state.Duty);
	lua_pushintvalue(dutyPosition, state.DutyPosition);
	lua_pushdoublevalue(frequency, state.Frequency);
	lua_pushintvalue(period, state.Period);
	lua_pushboolvalue(sweepEnabled, state.SweepEnabled);
	lua_pushboolvalue(sweepNegate, state.SweepNegate);
	lua_pushintvalue(sweepPeriod, state.SweepPeriod);
	lua_pushintvalue(sweepShift, state.SweepShift);
	lua_pushboolvalue(enabled, state.Enabled);
	lua_pushintvalue(outputVolume, state.OutputVolume);
	PushEnvelopeState(lua, state.Envelope);
	PushLengthCounterState(lua, state.LengthCounter);
}

void LuaApi::PushEnvelopeState(lua_State *lua, ApuEnvelopeState & state)
{
	lua_starttable("envelope");
	lua_pushboolvalue(startFlag, state.StartFlag);
	lua_pushboolvalue(loop, state.Loop);
	lua_pushboolvalue(constantVolume, state.ConstantVolume);
	lua_pushintvalue(divider, state.Divider);
	lua_pushintvalue(counter, state.Counter);
	lua_pushintvalue(volume, state.Volume);
	lua_endtable();
}

void LuaApi::PushLengthCounterState(lua_State *lua, ApuLengthCounterState & state)
{
	lua_starttable("lengthCounter");
	lua_pushboolvalue(halt, state.Halt);
	lua_pushintvalue(counter, state.Counter);
	lua_pushintvalue(reloadValue, state.ReloadValue);
	lua_endtable();
}
