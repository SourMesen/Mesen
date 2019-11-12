#pragma once
#include "stdafx.h"
#include <unordered_map>
#include <stack>

class Debugger;

enum class ProfilerDataType
{
	FunctionExclusive = 0,
	FunctionInclusive = 1,
	Instructions = 2,
	FunctionCallCount = 3,
	MinCycles = 4,
	MaxCycles = 5,
};

class Profiler
{
private:
	Debugger* _debugger;

	vector<uint64_t> _cyclesByInstruction;
	vector<uint64_t> _cyclesByFunction;
	vector<uint64_t> _cyclesByFunctionInclusive;
	vector<uint64_t> _functionCallCount;
	vector<uint64_t> _minCycles;
	vector<uint64_t> _maxCycles;
	
	std::stack<int32_t> _functionStack;
	std::stack<int32_t> _jsrStack;
	std::stack<uint64_t> _cycleCountStack;

	uint64_t _currentCycleCount;

	int32_t _currentFunction;
	int32_t _currentInstruction;
	int32_t _nextFunctionAddr;

	uint32_t _resetFunctionIndex;
	uint32_t _inMemoryFunctionIndex;

	void InternalReset();

public:
	Profiler(Debugger* debugger);

	void ProcessInstructionStart(int32_t absoluteAddr);
	void ProcessCycle();
	void StackFunction(int32_t instructionAddr, int32_t functionAddr);
	void UnstackFunction();

	void Reset();
	void GetProfilerData(uint64_t* profilerData, ProfilerDataType type);
};