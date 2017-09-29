#pragma once
#include "stdafx.h"
#include "DebuggerTypes.h"
#include "../Utilities/SimpleLock.h"
#include "DisassemblyInfo.h"

class MemoryManager;
class LabelManager;
class ExpressionEvaluator;
class Debugger;

enum class StatusFlagFormat
{
	Hexadecimal = 0,
	Text = 1,
	CompactText = 2
};

struct TraceLoggerOptions
{
	bool ShowByteCode;
	bool ShowRegisters;
	bool ShowCpuCycles;
	bool ShowPpuCycles;
	bool ShowPpuScanline;
	bool ShowPpuFrames;
	bool ShowExtraInfo;
	bool IndentCode;
	bool ShowEffectiveAddresses;
	bool UseLabels;
	StatusFlagFormat StatusFormat;

	char Condition[1000];
};

class TraceLogger
{
private:
	static TraceLogger *_instance;

	//Must be static to be thread-safe when switching game
	static string _executionTrace;
	
	TraceLoggerOptions _options;
	string _outputFilepath;
	string _outputBuffer;
	ofstream _outputFile;
	shared_ptr<MemoryManager> _memoryManager;
	shared_ptr<LabelManager> _labelManager;
	
	shared_ptr<ExpressionEvaluator> _expEvaluator;
	vector<int> _conditionRpnList;

	bool _pendingLog;
	DebugState _lastState;
	DisassemblyInfo _lastDisassemblyInfo;

	constexpr static int ExecutionLogSize = 30000;
	bool _logToFile;
	uint16_t _currentPos;
	uint32_t _logCount;
	State _cpuStateCache[ExecutionLogSize] = {};
	PPUDebugState _ppuStateCache[ExecutionLogSize] = {};
	DisassemblyInfo _disassemblyCache[ExecutionLogSize];

	State _cpuStateCacheCopy[ExecutionLogSize] = {};
	PPUDebugState _ppuStateCacheCopy[ExecutionLogSize] = {};
	DisassemblyInfo _disassemblyCacheCopy[ExecutionLogSize];

	SimpleLock _lock;
	
	void GetStatusFlag(string &output, uint8_t ps);
	void AddRow(DisassemblyInfo &disassemblyInfo, DebugState &state);
	bool ConditionMatches(DebugState &state, DisassemblyInfo &disassemblyInfo, OperationInfo &operationInfo);
	
	void GetTraceRow(string &output, State &cpuState, PPUDebugState &ppuState, DisassemblyInfo &disassemblyInfo, bool forceByteCode);

public:
	TraceLogger(Debugger* debugger, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager);
	~TraceLogger();

	
	void Log(DebugState &state, DisassemblyInfo &disassemblyInfo, OperationInfo &operationInfo);
	void LogNonExec(OperationInfo& operationInfo);
	void SetOptions(TraceLoggerOptions options);
	void StartLogging(string filename);
	void StopLogging();

	const char* GetExecutionTrace(uint32_t lineCount);

	static void LogStatic(string log);

};