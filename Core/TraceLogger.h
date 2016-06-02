#pragma once
#include "stdafx.h"

class DisassemblyInfo;
struct DebugState;

struct TraceLoggerOptions
{

};

class TraceLogger
{
private:
	static TraceLogger *_instance;
	TraceLoggerOptions _options;
	string _outputFilepath;
	ofstream _outputFile;
	bool _firstLine;

public:
	TraceLogger(string outputFilepath, TraceLoggerOptions options);
	~TraceLogger();

	void Log(DebugState &state, shared_ptr<DisassemblyInfo> disassemblyInfo);

	static void LogStatic(string log);
};