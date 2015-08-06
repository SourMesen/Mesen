#pragma once

#include "stdafx.h"
#include <atomic>
using std::atomic;

#include "CPU.h"
#include "PPU.h"
#include "Breakpoint.h"
#include "../Utilities/SimpleLock.h"

class MemoryManager;
class Console;
class Disassembler;

struct DebugState
{
	State CPU;
	PPUDebugState PPU;
};

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
	shared_ptr<Console> _console;
	shared_ptr<CPU> _cpu;
	shared_ptr<PPU> _ppu;
	shared_ptr<MemoryManager> _memoryManager;
	shared_ptr<BaseMapper> _mapper;
	vector<shared_ptr<Breakpoint>> _readBreakpoints;
	vector<shared_ptr<Breakpoint>> _writeBreakpoints;
	vector<shared_ptr<Breakpoint>> _execBreakpoints;

	SimpleLock _bpLock;
	SimpleLock _breakLock;

	string _outputCache;
	atomic<uint32_t> _stepCount;
	atomic<int32_t> _stepCycleCount;
	atomic<uint8_t> _lastInstruction;
	atomic<bool> _stepOut;
	atomic<int32_t> _stepOverAddr;

private:
	void PrivateCheckBreakpoint(BreakpointType type, uint32_t addr);
	shared_ptr<Breakpoint> GetMatchingBreakpoint(BreakpointType type, uint32_t addr);
	bool SleepUntilResume();

public:
	Debugger(shared_ptr<Console> console, shared_ptr<CPU> cpu, shared_ptr<PPU> ppu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper);
	~Debugger();

	void AddBreakpoint(BreakpointType type, uint32_t address, bool isAbsoluteAddr, bool enabled);
	void RemoveBreakpoint(BreakpointType type, uint32_t address, bool isAbsoluteAddr);
	vector<shared_ptr<Breakpoint>> GetBreakpoints();
	vector<uint32_t> GetExecBreakpointAddresses();

	uint32_t GetMemoryState(DebugMemoryType type, uint8_t *buffer);

	void GetState(DebugState *state);

	void Step(uint32_t count = 1);
	void StepCycles(uint32_t cycleCount = 1);
	void StepOver();
	void StepOut();
	void Run();

	bool IsCodeChanged();
	string GenerateOutput();
	string* GetCode();

	uint8_t GetMemoryValue(uint32_t addr);
	uint32_t GetRelativeAddress(uint32_t addr);
	
	static void CheckBreakpoint(BreakpointType type, uint32_t addr);
};