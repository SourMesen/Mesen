#pragma once

#include "stdafx.h"
#include <atomic>
#include <deque>
using std::atomic;
using std::deque;

#include "CPU.h"
#include "PPU.h"
#include "DebugState.h"
#include "Breakpoint.h"
#include "TraceLogger.h"
#include "../Utilities/SimpleLock.h"
#include "CodeDataLogger.h"

class MemoryManager;
class Console;
class Disassembler;

enum class DebugMemoryType
{
	CpuMemory = 0,
	PpuMemory = 1,
	PaletteMemory = 2,
	SpriteMemory = 3,
	SecondarySpriteMemory = 4,
	PrgRom = 5,
	ChrRom = 6,
};

class Debugger
{
private:
	static Debugger* Instance;

	unique_ptr<Disassembler> _disassembler;
	unique_ptr<CodeDataLogger> _codeDataLogger;
	shared_ptr<Console> _console;
	shared_ptr<CPU> _cpu;
	shared_ptr<PPU> _ppu;
	shared_ptr<MemoryManager> _memoryManager;
	shared_ptr<BaseMapper> _mapper;
	
	vector<Breakpoint> _readBreakpoints;
	vector<Breakpoint> _writeBreakpoints;
	vector<Breakpoint> _execBreakpoints;
	vector<Breakpoint> _globalBreakpoints;
	vector<Breakpoint> _readVramBreakpoints;
	vector<Breakpoint> _writeVramBreakpoints;

	deque<uint32_t> _callstackAbsolute;
	deque<uint32_t> _callstackRelative;

	SimpleLock _bpLock;
	SimpleLock _breakLock;

	unique_ptr<TraceLogger> _traceLogger;

	uint16_t *_currentReadAddr; //Used to alter the executing address via "Set Next Statement"

	string _romFilepath;
	string _outputCache;
	atomic<int32_t> _stepCount;
	atomic<int32_t> _stepCycleCount;
	atomic<uint8_t> _lastInstruction;
	atomic<bool> _stepOut;
	atomic<int32_t> _stepOverAddr;

private:
	void PrivateProcessPpuCycle();
	void PrivateProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t value);
	void PrivateProcessVramOperation(MemoryOperationType type, uint16_t addr, uint8_t value);
	bool HasMatchingBreakpoint(BreakpointType type, uint32_t addr, int16_t value);
	void UpdateCallstack(uint32_t addr);
	void ProcessStepConditions(uint32_t addr);
	void BreakOnBreakpoint(MemoryOperationType type, uint32_t addr, uint8_t value);
	bool SleepUntilResume();

public:
	Debugger(shared_ptr<Console> console, shared_ptr<CPU> cpu, shared_ptr<PPU> ppu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper);
	~Debugger();

	void SetBreakpoints(Breakpoint breakpoints[], uint32_t length);

	uint32_t GetMemoryState(DebugMemoryType type, uint8_t *buffer);
	void GetNametable(int nametableIndex, uint32_t* frameBuffer, uint8_t* tileData, uint8_t* paletteData);
	void GetChrBank(int bankIndex, uint32_t* frameBuffer, uint8_t palette);
	void GetSprites(uint32_t* frameBuffer);
	void GetPalette(uint32_t* frameBuffer);

	void GetCallstack(int32_t* callstackAbsolute, int32_t* callstackRelative);

	void GetState(DebugState *state);

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
	uint32_t GetRelativeAddress(uint32_t addr);

	void StartTraceLogger(TraceLoggerOptions options);
	void StopTraceLogger();

	int32_t EvaluateExpression(string expression, EvalResultType &resultType);
	
	static void ProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t value);
	static void ProcessVramOperation(MemoryOperationType type, uint16_t addr, uint8_t value);
	static void ProcessPpuCycle();

	static void BreakIfDebugging();
};