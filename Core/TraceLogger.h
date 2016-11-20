#pragma once
#include "stdafx.h"

class DisassemblyInfo;
class MemoryManager;
struct DebugState;

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
};

class TraceLogger
{
private:
	static TraceLogger *_instance;
	TraceLoggerOptions _options;
	string _outputFilepath;
	ofstream _outputFile;
	bool _firstLine;
	shared_ptr<MemoryManager> _memoryManager;

public:
	TraceLogger(string outputFilepath, shared_ptr<MemoryManager> memoryManager, TraceLoggerOptions options);
	~TraceLogger();

	void Log(DebugState &state, shared_ptr<DisassemblyInfo> disassemblyInfo);

	static void LogStatic(string log);
};