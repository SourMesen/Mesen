#include "stdafx.h"
#include "../Core/Console.h"
#include "../Core/Debugger.h"
#include "../Core/CodeDataLogger.h"
#include "../Core/LabelManager.h"
#include "../Core/MemoryDumper.h"
#include "../Core/MemoryAccessCounter.h"
#include "../Core/Profiler.h"
#include "../Core/Assembler.h"

shared_ptr<Debugger> GetDebugger()
{
	return Console::GetInstance()->GetDebugger();
}

extern "C"
{
	//Debugger wrapper
	DllExport void __stdcall DebugInitialize()
	{
		Console::GetInstance()->GetDebugger();
	}

	DllExport void __stdcall DebugRelease()
	{
		Console::GetInstance()->StopDebugger();
	}

	DllExport bool __stdcall DebugIsDebuggerRunning()
	{
		return Console::GetInstance()->GetDebugger(false).get() != nullptr;
	}

	DllExport void __stdcall DebugSetFlags(uint32_t flags) { GetDebugger()->SetFlags(flags); }

	DllExport void __stdcall DebugGetState(DebugState *state) { GetDebugger()->GetState(state); }
	DllExport void __stdcall DebugSetState(DebugState state) { GetDebugger()->SetState(state); }

	DllExport void __stdcall DebugSetBreakpoints(Breakpoint breakpoints[], uint32_t length) { GetDebugger()->SetBreakpoints(breakpoints, length); }
	DllExport void __stdcall DebugSetLabel(uint32_t address, AddressType addressType, char* label, char* comment) { GetDebugger()->GetLabelManager()->SetLabel(address, addressType, label, comment); }

	DllExport bool __stdcall DebugIsExecutionStopped() { return GetDebugger()->IsExecutionStopped(); }
	DllExport void __stdcall DebugRun() { GetDebugger()->Run(); }
	DllExport void __stdcall DebugStep(uint32_t count) { GetDebugger()->Step(count); }
	DllExport void __stdcall DebugStepCycles(uint32_t count) { GetDebugger()->StepCycles(count); }
	DllExport void __stdcall DebugStepOver() { GetDebugger()->StepOver(); }
	DllExport void __stdcall DebugStepOut() { GetDebugger()->StepOut(); }
	DllExport void __stdcall DebugPpuStep(uint32_t count) { GetDebugger()->PpuStep(count); }
	DllExport const char* __stdcall DebugGetCode(uint32_t &length) { return GetDebugger()->GetCode(length); }

	DllExport void __stdcall DebugSetPpuViewerScanlineCycle(int32_t scanline, int32_t cycle) { return GetDebugger()->SetPpuViewerScanlineCycle(scanline, cycle); }

	DllExport void __stdcall DebugSetNextStatement(uint16_t addr) { GetDebugger()->SetNextStatement(addr); }
	DllExport void __stdcall DebugSetMemoryState(uint32_t type, uint8_t *buffer) { GetDebugger()->GetMemoryDumper()->SetMemoryState((DebugMemoryType)type, buffer); }

	DllExport uint32_t __stdcall DebugGetMemoryState(uint32_t type, uint8_t *buffer) { return GetDebugger()->GetMemoryDumper()->GetMemoryState((DebugMemoryType)type, buffer); }
	DllExport void __stdcall DebugGetNametable(uint32_t nametableIndex, uint32_t *frameBuffer, uint8_t *tileData, uint8_t *attributeData) { GetDebugger()->GetMemoryDumper()->GetNametable(nametableIndex, frameBuffer, tileData, attributeData); }
	DllExport void __stdcall DebugGetChrBank(uint32_t bankIndex, uint32_t *frameBuffer, uint8_t palette, bool largeSprites, CdlHighlightType highlightType) { GetDebugger()->GetMemoryDumper()->GetChrBank(bankIndex, frameBuffer, palette, largeSprites, highlightType); }
	DllExport void __stdcall DebugGetSprites(uint32_t *frameBuffer) { GetDebugger()->GetMemoryDumper()->GetSprites(frameBuffer); }
	DllExport void __stdcall DebugGetPalette(uint32_t *frameBuffer) { GetDebugger()->GetMemoryDumper()->GetPalette(frameBuffer); }
	
	DllExport void __stdcall DebugGetCallstack(int32_t *callstackAbsolute, int32_t *callstackRelative) { GetDebugger()->GetCallstack(callstackAbsolute, callstackRelative); }
	DllExport int32_t __stdcall DebugGetFunctionEntryPointCount() { return GetDebugger()->GetFunctionEntryPointCount(); }
	DllExport void __stdcall DebugGetFunctionEntryPoints(int32_t *entryPoints, int32_t maxCount) { GetDebugger()->GetFunctionEntryPoints(entryPoints, maxCount); }
	
	DllExport int32_t __stdcall DebugGetRelativeAddress(uint32_t addr, AddressType type) { return GetDebugger()->GetRelativeAddress(addr, type); }
	DllExport int32_t __stdcall DebugGetAbsoluteAddress(uint32_t addr) { return GetDebugger()->GetAbsoluteAddress(addr); }
	DllExport void __stdcall DebugGetAbsoluteAddressAndType(uint32_t relativeAddr, AddressTypeInfo* info) { return GetDebugger()->GetAbsoluteAddressAndType(relativeAddr, info); }

	DllExport bool __stdcall DebugLoadCdlFile(char* cdlFilepath) { return GetDebugger()->LoadCdlFile(cdlFilepath); }
	DllExport bool __stdcall DebugSaveCdlFile(char* cdlFilepath) { return GetDebugger()->GetCodeDataLogger()->SaveCdlFile(cdlFilepath); }
	DllExport void __stdcall DebugGetCdlRatios(CdlRatios* cdlRatios) { *cdlRatios = GetDebugger()->GetCodeDataLogger()->GetRatios(); }
	DllExport void __stdcall DebugResetCdlLog() { GetDebugger()->GetCodeDataLogger()->Reset(); }

	DllExport int32_t __stdcall DebugEvaluateExpression(char* expression, EvalResultType *resultType) { return GetDebugger()->EvaluateExpression(expression, *resultType); }

	DllExport void __stdcall DebugSetTraceOptions(TraceLoggerOptions options) { GetDebugger()->GetTraceLogger()->SetOptions(options); }
	DllExport void __stdcall DebugStartTraceLogger(char* filename) { GetDebugger()->GetTraceLogger()->StartLogging(filename); }
	DllExport void __stdcall DebugStopTraceLogger() { GetDebugger()->GetTraceLogger()->StopLogging(); }
	DllExport const char* DebugGetExecutionTrace(uint32_t lineCount) { return GetDebugger()->GetTraceLogger()->GetExecutionTrace(lineCount); }

	DllExport uint8_t __stdcall DebugGetMemoryValue(DebugMemoryType type, uint32_t address) { return GetDebugger()->GetMemoryDumper()->GetMemoryValue(type, address); }
	DllExport void __stdcall DebugSetMemoryValue(DebugMemoryType type, uint32_t address, uint8_t value) { return GetDebugger()->GetMemoryDumper()->SetMemoryValue(type, address, value); }
	DllExport int32_t __stdcall DebugGetMemorySize(DebugMemoryType type) { return GetDebugger()->GetMemorySize(type); }
	DllExport void __stdcall DebugGetMemoryAccessCounts(AddressType memoryType, MemoryOperationType operationType, uint32_t* counts, bool forUninitReads) { GetDebugger()->GetMemoryAccessCounter()->GetAccessCounts(memoryType, operationType, counts, forUninitReads); }
	DllExport void __stdcall DebugResetMemoryAccessCounts() { GetDebugger()->GetMemoryAccessCounter()->ResetCounts(); }
	DllExport void __stdcall DebugGetMemoryAccessStamps(uint32_t offset, uint32_t length, DebugMemoryType memoryType, MemoryOperationType operationType, uint32_t* stamps) { GetDebugger()->GetMemoryAccessCounter()->GetAccessStamps(offset, length, memoryType, operationType, stamps); }
	DllExport void __stdcall DebugGetMemoryAccessCountsEx(uint32_t offset, uint32_t length, DebugMemoryType memoryType, MemoryOperationType operationType, int32_t* counts) { GetDebugger()->GetMemoryAccessCounter()->GetAccessCountsEx(offset, length, memoryType, operationType, counts); }

	DllExport void __stdcall DebugGetProfilerData(int64_t* profilerData, ProfilerDataType dataType) { GetDebugger()->GetProfiler()->GetProfilerData(profilerData, dataType); }
	DllExport void __stdcall DebugResetProfiler() { GetDebugger()->GetProfiler()->Reset(); }

	DllExport void __stdcall DebugSetFreezeState(uint16_t address, bool frozen) { GetDebugger()->SetFreezeState(address, frozen); }
	DllExport void __stdcall DebugGetFreezeState(uint16_t startAddress, uint16_t length, bool* freezeState) { GetDebugger()->GetFreezeState(startAddress, length, freezeState); }

	DllExport uint32_t __stdcall DebugGetPpuScroll() { return GetDebugger()->GetPpuScroll(); }

	DllExport uint32_t __stdcall DebugAssembleCode(char* code, uint16_t startAddress, int16_t* assembledOutput) { return GetDebugger()->GetAssembler()->AssembleCode(code, startAddress, assembledOutput); }

	DllExport void __stdcall DebugSaveRomToDisk(char* filename) { GetDebugger()->SaveRomToDisk(filename); }

	DllExport int32_t __stdcall DebugFindSubEntryPoint(uint16_t relativeAddress) { return GetDebugger()->FindSubEntryPoint(relativeAddress); }
};
