#include "stdafx.h"
#include "Profiler.h"
#include "DebugBreakHelper.h"
#include "Debugger.h"
#include "MemoryDumper.h"

Profiler::Profiler(Debugger * debugger)
{
	_debugger = debugger;
	InternalReset();
}

void Profiler::ProcessCycle()
{
	_cyclesByFunction[_currentFunction]++;
	_cyclesByFunctionInclusive[_currentFunction]++;
	_cyclesByInstruction[_currentInstruction]++;
	_currentCycleCount++;
}

void Profiler::StackFunction(int32_t instructionAddr, int32_t functionAddr)
{
	if(functionAddr >= 0) {
		_nextFunctionAddr = functionAddr;
		_jsrStack.push(instructionAddr);
	}
}

void Profiler::UnstackFunction()
{
	if(!_functionStack.empty()) {
		//Return to the previous function
		_currentFunction = _functionStack.top();
		_functionStack.pop();

		int32_t jsrAddr = _jsrStack.top();
		_jsrStack.pop();

		if(jsrAddr >= 0) {
			//Prevent IRQ/NMI from adding cycles to the calling function

			//Add the subroutine's cycle count to the JSR instruction
			_cyclesByInstruction[jsrAddr] += _currentCycleCount;

			if(_currentFunction >= 0) {
				//Add the subroutine's cycle count to the function's inclusive cycle count
				_cyclesByFunctionInclusive[_currentFunction] += _currentCycleCount;
			}
		}

		//Add the subroutine's cycle count to the current routine's cycle count
		_currentCycleCount = _cycleCountStack.top() + _currentCycleCount;
		_cycleCountStack.pop();
	}
}

void Profiler::ProcessInstructionStart(int32_t absoluteAddr)
{
	if(_nextFunctionAddr >= 0) {
		_cycleCountStack.push(_currentCycleCount);
		_functionStack.push(_currentFunction);

		_currentFunction = _nextFunctionAddr;
		_currentCycleCount = 0;
		_functionCallCount[_nextFunctionAddr]++;

		_nextFunctionAddr = -1;
	}

	if(absoluteAddr >= 0) {
		_currentInstruction = absoluteAddr;
		ProcessCycle();
	} else {
		_currentFunction = _inMemoryFunctionIndex;
	}
}

void Profiler::Reset()
{
	DebugBreakHelper helper(_debugger);
	InternalReset();
}

void Profiler::InternalReset()
{
	_nextFunctionAddr = -1;
	_currentCycleCount = 0;
	_currentInstruction = 0;

	int size = _debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::PrgRom);
	_resetFunctionIndex = size;
	_inMemoryFunctionIndex = size + 1;
	_currentFunction = _resetFunctionIndex;

	_cyclesByInstruction.clear();
	_cyclesByFunction.clear();
	_cyclesByFunctionInclusive.clear();
	_functionCallCount.clear();

	_cyclesByInstruction.insert(_cyclesByInstruction.end(), size + 2, 0);
	_cyclesByFunction.insert(_cyclesByFunction.end(), size + 2, 0);
	_cyclesByFunctionInclusive.insert(_cyclesByFunctionInclusive.end(), size + 2, 0);
	_functionCallCount.insert(_functionCallCount.end(), size + 2, 0);
}

void Profiler::GetProfilerData(int64_t * profilerData, ProfilerDataType type)
{
	vector<uint64_t> *dataArray = nullptr;

	switch(type) {
		default:
		case ProfilerDataType::FunctionExclusive: dataArray = &_cyclesByFunction; break;
		case ProfilerDataType::FunctionInclusive: dataArray = &_cyclesByFunctionInclusive; break;
		case ProfilerDataType::Instructions: dataArray = &_cyclesByInstruction; break;
		case ProfilerDataType::FunctionCallCount: dataArray = &_functionCallCount; break;
	}

	memcpy(profilerData, (*dataArray).data(), (*dataArray).size() * sizeof(uint64_t));
}
