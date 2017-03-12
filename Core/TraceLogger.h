#pragma once
#include "stdafx.h"
#include "CpuState.h"
#include "PpuState.h"

class DisassemblyInfo;
class MemoryManager;
class LabelManager;

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

	constexpr static int ExecutionLogSize = 30000;
	bool _logToFile;
	uint16_t _currentPos;
	State _cpuStateCache[ExecutionLogSize] = {};
	PPUDebugState _ppuStateCache[ExecutionLogSize] = {};
	shared_ptr<DisassemblyInfo> _disassemblyCache[ExecutionLogSize] = {};

	string _executionTrace;

public:
	TraceLogger(shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager);
	~TraceLogger();

	void Log(State &cpuState, PPUDebugState &ppuState, shared_ptr<DisassemblyInfo> disassemblyInfo);
	void SetOptions(TraceLoggerOptions options);
	void StartLogging(string filename);
	void StopLogging();

	void GetTraceRow(string &output, State &cpuState, PPUDebugState &ppuState, shared_ptr<DisassemblyInfo> &disassemblyInfo, bool firstLine);
	const char* GetExecutionTrace(uint32_t lineCount);

	static void LogStatic(string log);

};