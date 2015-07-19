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

	DllExport void DebugAddBreakpoint(uint32_t type, uint32_t address, int isAbsoluteAddr) { _debugger->AddBreakpoint((BreakpointType)type, address, isAbsoluteAddr == 1); }
		
	DllExport void __stdcall DebugRun() { _debugger->Run(); }
	DllExport void __stdcall DebugStep(uint32_t count) { _debugger->Step(count); }
	DllExport void __stdcall DebugStepCycles(uint32_t count) { _debugger->StepCycles(count); }
	DllExport void __stdcall DebugStepOver() { _debugger->StepOver(); }
	DllExport void __stdcall DebugStepOut() { _debugger->StepOut(); }
	DllExport int __stdcall DebugIsCodeChanged() { return _debugger->IsCodeChanged(); }
	DllExport const char* __stdcall DebugGetCode() { return _debugger->GetCode()->c_str(); }

	DllExport uint8_t __stdcall DebugGetMemoryValue(uint32_t addr) { return _debugger->GetMemoryValue(addr); }
	DllExport uint32_t __stdcall DebugGetRelativeAddress(uint32_t addr) { return _debugger->GetRelativeAddress(addr); }
};
