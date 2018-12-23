#pragma once

#include "stdafx.h"
#include <atomic>
#include <deque>
#include <unordered_set>
using std::atomic;
using std::deque;
using std::unordered_set;

#include "../Utilities/SimpleLock.h"
#include "DebuggerTypes.h"

class CPU;
class APU;
class PPU;
class MemoryManager;
class Console;
class Assembler;
class Disassembler;
class LabelManager;
class MemoryDumper;
class MemoryAccessCounter;
class Profiler;
class CodeRunner;
class BaseMapper;
class ScriptHost;
class TraceLogger;
class Breakpoint;
class CodeDataLogger;
class ExpressionEvaluator;
struct ExpressionData;

enum EvalResultType : int32_t;
enum class CdlStripFlag;

class Debugger
{
private:
	static constexpr int BreakpointTypeCount = 6;

	//Must be static to be thread-safe when switching game
	static string _disassemblerOutput;

	shared_ptr<Disassembler> _disassembler;
	shared_ptr<Assembler> _assembler;
	shared_ptr<MemoryDumper> _memoryDumper;
	shared_ptr<CodeDataLogger> _codeDataLogger;
	shared_ptr<MemoryAccessCounter> _memoryAccessCounter;
	shared_ptr<LabelManager> _labelManager;
	shared_ptr<TraceLogger> _traceLogger;
	shared_ptr<Profiler> _profiler;
	unique_ptr<CodeRunner> _codeRunner;

	shared_ptr<Console> _console;
	shared_ptr<CPU> _cpu;
	shared_ptr<PPU> _ppu;
	shared_ptr<APU> _apu;
	shared_ptr<MemoryManager> _memoryManager;
	shared_ptr<BaseMapper> _mapper;

	bool _hasScript;
	SimpleLock _scriptLock;
	int _nextScriptId;
	vector<shared_ptr<ScriptHost>> _scripts;
	
	atomic<int32_t> _preventResume;
	atomic<bool> _stopFlag;
	atomic<bool> _executionStopped;
	atomic<int32_t> _suspendCount;
	vector<Breakpoint> _breakpoints[BreakpointTypeCount];
	vector<ExpressionData> _breakpointRpnList[BreakpointTypeCount];
	bool _hasBreakpoint[BreakpointTypeCount] = {};

	vector<uint8_t> _frozenAddresses;

	uint32_t _opCodeCycle;
	MemoryOperationType _memoryOperationType;

	deque<StackFrameInfo> _callstack;
	deque<int32_t> _subReturnAddresses;

	int32_t _stepOutReturnAddress;

	unordered_set<uint32_t> _functionEntryPoints;

	unique_ptr<ExpressionEvaluator> _watchExpEval;
	unique_ptr<ExpressionEvaluator> _bpExpEval;
	DebugState _debugState;

	SimpleLock _breakLock;

	//Used to alter the executing address via "Set Next Statement"
	uint16_t *_currentReadAddr;
	uint8_t *_currentReadValue;
	int32_t _nextReadAddr;
	uint16_t _returnToAddress;

	uint32_t _flags;

	string _romName;
	atomic<int32_t> _stepCount;
	atomic<int32_t> _ppuStepCount;
	atomic<int32_t> _stepCycleCount;
	atomic<uint8_t> _lastInstruction;
	atomic<bool> _stepOut;
	atomic<int32_t> _stepOverAddr;
	BreakSource _breakSource;
	
	atomic<bool> _released;
	SimpleLock _releaseLock;

	bool _enableBreakOnUninitRead;
	
	atomic<bool> _breakRequested;
	bool _pausedForDebugHelper;

	atomic<int32_t> _breakOnScanline;

	bool _proccessPpuCycle[341];
	std::unordered_map<int, int> _ppuViewerUpdateCycle;

	uint16_t _ppuScrollX;
	uint16_t _ppuScrollY;

	int32_t _prevInstructionCycle;
	int32_t _curInstructionCycle;
	int32_t _runToCycle;
	bool _needRewind;
	
	vector<stringstream> _rewindCache;
	vector<uint32_t> _rewindPrevInstructionCycleCache;

	uint32_t _inputOverride[4];

	vector<DebugEventInfo> _prevDebugEvents;
	vector<DebugEventInfo> _debugEvents;
	vector<vector<int>> _debugEventMarkerRpn;

private:
	void ProcessBreakpoints(BreakpointType type, OperationInfo &operationInfo, bool allowBreak = true);
	
	void AddCallstackFrame(uint16_t source, uint16_t target, StackFrameFlags flags);
	void UpdateCallstack(uint8_t currentInstruction, uint32_t addr);

	void ProcessStepConditions(uint16_t addr);
	bool SleepUntilResume(BreakSource source, uint32_t breakpointId = 0, BreakpointType bpType = BreakpointType::Global, uint16_t bpAddress = 0);

	void AddDebugEvent(DebugEventType type, uint16_t address = -1, uint8_t value = 0, int16_t breakpointId = -1, int8_t ppuLatch = -1);

	void UpdatePpuCyclesToProcess();

public:
	Debugger(shared_ptr<Console> console, shared_ptr<CPU> cpu, shared_ptr<PPU> ppu, shared_ptr<APU> apu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper);
	~Debugger();

	void ReleaseDebugger();

	void SetPpu(shared_ptr<PPU> ppu);
	Console* GetConsole();

	void SetFlags(uint32_t flags);
	bool CheckFlag(DebuggerFlags flag);
	
	void SetBreakpoints(Breakpoint breakpoints[], uint32_t length);
	
	shared_ptr<LabelManager> GetLabelManager();

	void GetFunctionEntryPoints(int32_t* entryPoints, int32_t maxCount);
	int32_t GetFunctionEntryPointCount();

	void GetCallstack(StackFrameInfo* callstackArray, uint32_t &callstackSize);
	
	void GetInstructionProgress(InstructionProgress &state);
	void GetApuState(ApuState *state);
	__forceinline void GetState(DebugState *state, bool includeMapperInfo = true);
	void SetState(DebugState state);

	void Suspend();
	void Resume();

	void Break();
	void ResumeFromBreak();

	void PpuStep(uint32_t count = 1);
	void Step(uint32_t count = 1, BreakSource source = BreakSource::CpuStep);
	void StepCycles(uint32_t cycleCount = 1);
	void StepOver();
	void StepOut();
	void StepBack();
	void Run();

	void BreakImmediately(BreakSource source);
	void BreakOnScanline(int16_t scanline);

	bool LoadCdlFile(string cdlFilepath);
	void SetCdlData(uint8_t* cdlData, uint32_t length);
	void ResetCdl();
	void UpdateCdlCache();
	bool IsMarkedAsCode(uint16_t relativeAddress);
	shared_ptr<CodeDataLogger> GetCodeDataLogger();

	void SetNextStatement(uint16_t addr);

	void SetPpuViewerScanlineCycle(int32_t ppuViewerId, int32_t scanline, int32_t cycle);
	void ClearPpuViewerSettings(int32_t ppuViewer);

	bool IsExecutionStopped();

	bool IsPauseIconShown();
	void PreventResume();
	void AllowResume();

	void GenerateCodeOutput();
	const char* GetCode(uint32_t &length);

	void GetJumpTargets(bool* jumpTargets);
	
	int32_t GetRelativeAddress(uint32_t addr, AddressType type);
	int32_t GetAbsoluteAddress(uint32_t addr);	
	int32_t GetAbsoluteChrAddress(uint32_t addr);
	int32_t GetRelativeChrAddress(uint32_t addr);
	
	void GetAbsoluteAddressAndType(uint32_t relativeAddr, AddressTypeInfo* info);
	void GetPpuAbsoluteAddressAndType(uint32_t relativeAddr, PpuAddressTypeInfo* info);

	shared_ptr<Profiler> GetProfiler();
	shared_ptr<Assembler> GetAssembler();
	shared_ptr<TraceLogger> GetTraceLogger();
	shared_ptr<MemoryDumper> GetMemoryDumper();
	shared_ptr<MemoryAccessCounter> GetMemoryAccessCounter();

	int32_t EvaluateExpression(string expression, EvalResultType &resultType, bool useCache);
	
	void ProcessPpuCycle();
	bool ProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t &value);
	void ProcessVramReadOperation(MemoryOperationType type, uint16_t addr, uint8_t &value);
	void ProcessVramWriteOperation(uint16_t addr, uint8_t &value);
	
	void SetLastFramePpuScroll(uint16_t addr, uint8_t xScroll, bool updateHorizontalScrollOnly);
	uint32_t GetPpuScroll();

	void ProcessInterrupt(uint16_t cpuAddr, uint16_t destCpuAddr, bool forNmi);
	
	void AddTrace(const char *log);

	void SetFreezeState(uint16_t address, bool frozen);
	void GetFreezeState(uint16_t startAddress, uint16_t length, bool* freezeState);

	void StartCodeRunner(uint8_t *byteCode, uint32_t codeLength);
	void StopCodeRunner();

	void GetNesHeader(uint8_t* header);
	void SaveRomToDisk(string filename, bool saveAsIps, uint8_t* header, CdlStripFlag cdlStripflag);
	void RevertPrgChrChanges();
	bool HasPrgChrChanges();

	int32_t FindSubEntryPoint(uint16_t relativeAddress);
	
	void SetInputOverride(uint8_t port, uint32_t state);

	int32_t LoadScript(string name, string content, int32_t scriptId);
	void RemoveScript(int32_t scriptId);
	const char* GetScriptLog(int32_t scriptId);

	void ResetCounters();

	void UpdateProgramCounter(uint16_t &addr, uint8_t &value);

	void ProcessScriptSaveState(uint16_t &addr, uint8_t &value);
	void ProcessCpuOperation(uint16_t &addr, uint8_t &value, MemoryOperationType type);
	void ProcessPpuOperation(uint16_t addr, uint8_t &value, MemoryOperationType type);
	void ProcessEvent(EventType type);

	void GetDebugEvents(uint32_t* pictureBuffer, DebugEventInfo *infoArray, uint32_t &maxEventCount, bool returnPreviousFrameData);
	uint32_t GetDebugEventCount(bool returnPreviousFrameData);

	uint32_t GetScreenPixel(uint8_t x, uint8_t y);
};