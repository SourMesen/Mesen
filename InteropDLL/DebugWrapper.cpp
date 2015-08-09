#include "stdafx.h"
#include "../Core/Console.h"
#include "../Core/Debugger.h"

static shared_ptr<Debugger> _debugger = nullptr;

extern "C"
{
	//Debugger wrapper
	DllExport void __stdcall DebugInitialize()
	{
		_debugger = Console::GetInstance()->GetDebugger();
	}

	DllExport void __stdcall DebugRelease()
	{
		if(_debugger != nullptr) {
			_debugger->Run();
			_debugger.reset();
		}
	}

	DllExport void __stdcall DebugGetState(DebugState *state) { _debugger->GetState(state); }

	DllExport void __stdcall DebugAddBreakpoint(uint32_t type, uint32_t address, bool isAbsoluteAddr, bool enabled) { _debugger->AddBreakpoint((BreakpointType)type, address, isAbsoluteAddr, enabled); }
	DllExport void __stdcall DebugRemoveBreakpoint(uint32_t type, uint32_t address, bool isAbsoluteAddr) { _debugger->RemoveBreakpoint((BreakpointType)type, address, isAbsoluteAddr); }
		
	DllExport void __stdcall DebugRun() { _debugger->Run(); }
	DllExport void __stdcall DebugStep(uint32_t count) { _debugger->Step(count); }
	DllExport void __stdcall DebugStepCycles(uint32_t count) { _debugger->StepCycles(count); }
	DllExport void __stdcall DebugStepOver() { _debugger->StepOver(); }
	DllExport void __stdcall DebugStepOut() { _debugger->StepOut(); }
	DllExport int __stdcall DebugIsCodeChanged() { return _debugger->IsCodeChanged(); }
	DllExport const char* __stdcall DebugGetCode() { return _debugger->GetCode()->c_str(); }

	DllExport uint32_t __stdcall DebugGetMemoryState(uint32_t type, uint8_t *buffer) { return _debugger->GetMemoryState((DebugMemoryType)type, buffer); }
	DllExport void __stdcall DebugGetNametable(uint32_t nametableIndex, uint32_t *frameBuffer, uint8_t *tileData, uint8_t *attributeData) { _debugger->GetNametable(nametableIndex, frameBuffer, tileData, attributeData); }
	DllExport void __stdcall DebugGetChrBank(uint32_t bankIndex, uint32_t *frameBuffer, uint8_t palette) { _debugger->GetChrBank(bankIndex, frameBuffer, palette); }
	DllExport void __stdcall DebugGetSprites(uint32_t *frameBuffer) { _debugger->GetSprites(frameBuffer); }
	DllExport void __stdcall DebugGetPalette(uint32_t *frameBuffer) { _debugger->GetPalette(frameBuffer); }
	
	DllExport uint8_t __stdcall DebugGetMemoryValue(uint32_t addr) { return _debugger->GetMemoryValue(addr); }
	DllExport uint32_t __stdcall DebugGetRelativeAddress(uint32_t addr) { return _debugger->GetRelativeAddress(addr); }
};
