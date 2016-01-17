#include "stdafx.h"
#include "../Core/Console.h"
#include "../Core/Debugger.h"
#include "../Core/CodeDataLogger.h"

static std::weak_ptr<Debugger> _debugger;

shared_ptr<Debugger> GetDebugger()
{
	if(_debugger.expired()) {
		_debugger = Console::GetInstance()->GetDebugger();
	}
	return _debugger.lock();
}

extern "C"
{
	//Debugger wrapper
	DllExport void __stdcall DebugInitialize()
	{
		_debugger = Console::GetInstance()->GetDebugger();
	}

	DllExport void __stdcall DebugRelease()
	{
		Console::GetInstance()->StopDebugger();
	}

	DllExport void __stdcall DebugGetState(DebugState *state) { GetDebugger()->GetState(state); }

	DllExport void __stdcall DebugSetBreakpoints(Breakpoint breakpoints[], uint32_t length) { GetDebugger()->SetBreakpoints(breakpoints, length); }
		
	DllExport void __stdcall DebugRun() { GetDebugger()->Run(); }
	DllExport void __stdcall DebugStep(uint32_t count) { GetDebugger()->Step(count); }
	DllExport void __stdcall DebugStepCycles(uint32_t count) { GetDebugger()->StepCycles(count); }
	DllExport void __stdcall DebugStepOver() { GetDebugger()->StepOver(); }
	DllExport void __stdcall DebugStepOut() { GetDebugger()->StepOut(); }
	DllExport bool __stdcall DebugIsCodeChanged() { return GetDebugger()->IsCodeChanged(); }
	DllExport const char* __stdcall DebugGetCode() { return GetDebugger()->GetCode()->c_str(); }

	DllExport void __stdcall DebugSetNextStatement(uint16_t addr) { GetDebugger()->SetNextStatement(addr); }

	DllExport uint32_t __stdcall DebugGetMemoryState(uint32_t type, uint8_t *buffer) { return GetDebugger()->GetMemoryState((DebugMemoryType)type, buffer); }
	DllExport void __stdcall DebugGetNametable(uint32_t nametableIndex, uint32_t *frameBuffer, uint8_t *tileData, uint8_t *attributeData) { GetDebugger()->GetNametable(nametableIndex, frameBuffer, tileData, attributeData); }
	DllExport void __stdcall DebugGetChrBank(uint32_t bankIndex, uint32_t *frameBuffer, uint8_t palette) { GetDebugger()->GetChrBank(bankIndex, frameBuffer, palette); }
	DllExport void __stdcall DebugGetSprites(uint32_t *frameBuffer) { GetDebugger()->GetSprites(frameBuffer); }
	DllExport void __stdcall DebugGetPalette(uint32_t *frameBuffer) { GetDebugger()->GetPalette(frameBuffer); }
	
	DllExport void __stdcall DebugGetCallstack(int32_t *callstackAbsolute, int32_t *callstackRelative) { GetDebugger()->GetCallstack(callstackAbsolute, callstackRelative); }
	
	DllExport uint8_t __stdcall DebugGetMemoryValue(uint32_t addr) { return GetDebugger()->GetMemoryValue(addr); }
	DllExport uint32_t __stdcall DebugGetRelativeAddress(uint32_t addr) { return GetDebugger()->GetRelativeAddress(addr); }

	DllExport bool __stdcall DebugLoadCdlFile(char* cdlFilepath) { return GetDebugger()->LoadCdlFile(cdlFilepath); }
	DllExport bool __stdcall DebugSaveCdlFile(char* cdlFilepath) { return GetDebugger()->SaveCdlFile(cdlFilepath); }
	DllExport void __stdcall DebugGetCdlRatios(CdlRatios* cdlRatios) { *cdlRatios = GetDebugger()->GetCdlRatios(); }
	DllExport void __stdcall DebugResetCdlLog() { GetDebugger()->ResetCdlLog(); }

	DllExport int32_t __stdcall DebugEvaluateExpression(char* expression, EvalResultType *resultType) { return GetDebugger()->EvaluateExpression(expression, *resultType); }

	DllExport void __stdcall DebugStartTraceLogger(TraceLoggerOptions options) { GetDebugger()->StartTraceLogger(options); }
	DllExport void __stdcall DebugStopTraceLogger() { GetDebugger()->StopTraceLogger(); }
};
