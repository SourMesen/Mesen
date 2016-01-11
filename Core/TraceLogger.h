#pragma once
#include "stdafx.h"
#include "DebugState.h"
#include "DisassemblyInfo.h"

struct TraceLoggerOptions
{

};

class TraceLogger
{
private:
	TraceLoggerOptions _options;
	string _outputFilepath;
	ofstream _outputFile;

public:
	TraceLogger(string outputFilepath, TraceLoggerOptions options);
	~TraceLogger();

	void Log(DebugState &state, shared_ptr<DisassemblyInfo> disassemblyInfo);
};