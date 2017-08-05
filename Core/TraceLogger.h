#pragma once
#include "stdafx.h"
#include "DebuggerTypes.h"
#include "../Utilities/SimpleLock.h"

class DisassemblyInfo;
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
	TraceLoggerOptions _options;
	string _outputFilepath;
	string _outputBuffer;
	ofstream _outputFile;
	bool _firstLine;
	shared_ptr<MemoryManager> _memoryManager;
	shared_ptr<LabelManager> _labelManager;
	
	shared_ptr<ExpressionEvaluator> _expEvaluator;
	vector<int> _conditionRpnList;
	DebugState _lastState;
	shared_ptr<DisassemblyInfo> _lastDisassemblyInfo;

	constexpr static int ExecutionLogSize = 30000;
	bool _logToFile;
	uint16_t _currentPos;
	State _cpuStateCache[ExecutionLogSize] = {};
	PPUDebugState _ppuStateCache[ExecutionLogSize] = {};
	shared_ptr<DisassemblyInfo> _disassemblyCache[ExecutionLogSize] = {};

	SimpleLock _lock;
	string _executionTrace;
	
	void GetStatusFlag(string &output, uint8_t ps);
	void AddRow(shared_ptr<DisassemblyInfo> &disassemblyInfo, DebugState &state);
	bool ConditionMatches(DebugState &state, shared_ptr<DisassemblyInfo> &disassemblyInfo, OperationInfo &operationInfo);

public:
	TraceLogger(Debugger* debugger, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager);
	~TraceLogger();

	
	void Log(DebugState &state, shared_ptr<DisassemblyInfo> disassemblyInfo, OperationInfo &operationInfo);
	void LogNonExec(OperationInfo& operationInfo);
	void SetOptions(TraceLoggerOptions options);
	void StartLogging(string filename);
	void StopLogging();

	void GetTraceRow(string &output, State &cpuState, PPUDebugState &ppuState, shared_ptr<DisassemblyInfo> &disassemblyInfo, bool firstLine);
	const char* GetExecutionTrace(uint32_t lineCount);

	static void LogStatic(string log);

};