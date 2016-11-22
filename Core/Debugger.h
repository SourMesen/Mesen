#pragma once

#include "stdafx.h"
#include <atomic>
#include <deque>
#include <unordered_set>
#include <unordered_map>
using std::atomic;
using std::deque;
using std::unordered_set;
using std::unordered_map;

#include "DebugState.h"
#include "Breakpoint.h"
#include "TraceLogger.h"
#include "../Utilities/SimpleLock.h"
#include "CodeDataLogger.h"
#include "MemoryDumper.h"

class CPU;
class PPU;
class MemoryManager;
class Console;
class Disassembler;

enum class DebuggerFlags
{
	PpuPartialDraw = 1,
	ShowEffectiveAddresses = 2,
	ShowOnlyDisassembledCode = 4
};

class Debugger
{
private:
	static Debugger* Instance;

	const static int BreakpointTypeCount = 6;

	unique_ptr<Disassembler> _disassembler;
	shared_ptr<MemoryDumper> _memoryDumper;
	shared_ptr<CodeDataLogger> _codeDataLogger;

	shared_ptr<Console> _console;
	shared_ptr<CPU> _cpu;
	shared_ptr<PPU> _ppu;
	shared_ptr<MemoryManager> _memoryManager;
	shared_ptr<BaseMapper> _mapper;
	
	bool _bpUpdateNeeded;
	SimpleLock _bpUpdateLock;

	atomic<bool> _stopFlag;
	atomic<bool> _executionStopped;
	atomic<int32_t> _suspendCount;
	vector<Breakpoint> _newBreakpoints;
	vector<Breakpoint> _breakpoints[BreakpointTypeCount];
	bool _hasBreakpoint[BreakpointTypeCount];

	unordered_map<uint32_t, string> _codeLabels;
	unordered_map<uint32_t, string> _codeComments;

	deque<uint32_t> _callstackAbsolute;
	deque<uint32_t> _callstackRelative;

	unordered_set<uint32_t> _functionEntryPoints;

	DebugState _debugState;

	SimpleLock _breakLock;

	shared_ptr<TraceLogger> _traceLogger;

	//Used to alter the executing address via "Set Next Statement"
	uint16_t *_currentReadAddr;
	uint8_t *_currentReadValue;

	uint32_t _flags;

	string _romName;
	string _outputCache;
	atomic<int32_t> _stepCount;
	atomic<int32_t> _ppuStepCount;
	atomic<int32_t> _stepCycleCount;
	atomic<uint8_t> _lastInstruction;
	atomic<bool> _stepOut;
	atomic<int32_t> _stepOverAddr;

private:
	void UpdateBreakpoints();

	void PrivateProcessPpuCycle();
	void PrivateProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t &value);
	void PrivateProcessVramOperation(MemoryOperationType type, uint16_t addr, uint8_t value);
	bool HasMatchingBreakpoint(BreakpointType type, uint32_t addr, int16_t value);
	
	void UpdateCallstack(uint32_t addr);
	void PrivateProcessInterrupt(uint16_t cpuAddr, uint16_t destCpuAddr, bool forNmi);

	void ProcessStepConditions(uint32_t addr);
	bool SleepUntilResume();

public:
	Debugger(shared_ptr<Console> console, shared_ptr<CPU> cpu, shared_ptr<PPU> ppu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper);
	~Debugger();

	void SetFlags(uint32_t flags);
	bool CheckFlag(DebuggerFlags flag);
	
	void SetBreakpoints(Breakpoint breakpoints[], uint32_t length);
	void SetLabel(uint32_t address, string label, string comment);

	void GetFunctionEntryPoints(int32_t* entryPoints);
	void GetCallstack(int32_t* callstackAbsolute, int32_t* callstackRelative);
	void GetState(DebugState *state);

	void Suspend();
	void Resume();

	void PpuStep(uint32_t count = 1);
	void Step(uint32_t count = 1);
	void StepCycles(uint32_t cycleCount = 1);
	void StepOver();
	void StepOut();
	void Run();

	bool LoadCdlFile(string cdlFilepath);
	bool SaveCdlFile(string cdlFilepath);
	CdlRatios GetCdlRatios();
	void ResetCdlLog();

	void SetNextStatement(uint16_t addr);

	bool IsCodeChanged();
	string GenerateOutput();
	string* GetCode();
	
	uint8_t GetMemoryValue(uint32_t addr);
	int32_t GetRelativeAddress(uint32_t addr);
	int32_t GetAbsoluteAddress(uint32_t addr);

	void StartTraceLogger(TraceLoggerOptions options);
	void StopTraceLogger();

	shared_ptr<MemoryDumper> GetMemoryDumper();

	int32_t EvaluateExpression(string expression, EvalResultType &resultType);
	
	static void ProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t &value);
	static void ProcessVramOperation(MemoryOperationType type, uint16_t addr, uint8_t value);
	static void ProcessPpuCycle();

	static void ProcessInterrupt(uint16_t cpuAddr, uint16_t destCpuAddr, bool forNmi);

	static bool IsEnabled();
	static void BreakIfDebugging();
};