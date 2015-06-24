#pragma once

#include "stdafx.h"
#include "Debugger.h"
#include "Console.h"
#include "Disassembler.h"

Debugger* Debugger::Instance = nullptr;

Debugger::Debugger(shared_ptr<Console> console, shared_ptr<CPU> cpu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper)
{
	_console = console;
	_cpu = cpu;
	_memoryManager = memoryManager;
	_mapper = mapper;

	_disassembler.reset(new Disassembler(mapper->GetPRGCopy(), mapper->GetPRGSize()));

	_stepCount = -1;
		
	Debugger::Instance = this;
}

Debugger::~Debugger()
{
	if(Debugger::Instance == this) {
		Debugger::Instance = nullptr;
	}
}

void Debugger::AddBreakpoint(BreakpointType type, uint32_t address, bool isAbsoluteAddr)
{
	_bpLock.Acquire();

	if(isAbsoluteAddr) {
		address = _mapper->ToAbsoluteAddress(address);
	}

	shared_ptr<Breakpoint> breakpoint(new Breakpoint(type, address, isAbsoluteAddr));
	switch(type) {
		case BreakpointType::Execute: _execBreakpoints.push_back(breakpoint); break;
		case BreakpointType::Read: _readBreakpoints.push_back(breakpoint); break;
		case BreakpointType::Write: _writeBreakpoints.push_back(breakpoint); break;
	}

	_bpLock.Release();
}

vector<shared_ptr<Breakpoint>> Debugger::GetBreakpoints()
{
	vector<shared_ptr<Breakpoint>> breakpoints;
	
	breakpoints.insert(breakpoints.end(), _execBreakpoints.begin(), _execBreakpoints.end());
	breakpoints.insert(breakpoints.end(), _readBreakpoints.begin(), _readBreakpoints.end());
	breakpoints.insert(breakpoints.end(), _writeBreakpoints.begin(), _writeBreakpoints.end());

	return breakpoints;
}

vector<uint32_t> Debugger::GetExecBreakpointAddresses()
{
	_bpLock.Acquire();

	vector<uint32_t> result;

	for(int i = 0, len = _execBreakpoints.size(); i < len; i++) {
		shared_ptr<Breakpoint> breakpoint = _execBreakpoints[i];
		int32_t addr = breakpoint->GetAddr();
		if(breakpoint->IsAbsoluteAddr()) {
			addr = _mapper->FromAbsoluteAddress(addr);
		}

		if(addr >= 0) {
			result.push_back(addr);
		}
	}

	_bpLock.Release();

	return result;
}

void Debugger::RemoveBreakpoint(shared_ptr<Breakpoint> breakpoint)
{
	_bpLock.Acquire();

	for(int i = 0, len = _execBreakpoints.size(); i < len; i++) {
		if(_execBreakpoints[i] == breakpoint) {
			_execBreakpoints.erase(_execBreakpoints.begin()+i);
			break;
		}
	}

	_bpLock.Release();

	//_readBreakpoints.remove(breakpoint);
	//_writeBreakpoints.remove(breakpoint);
}

shared_ptr<Breakpoint> Debugger::GetMatchingBreakpoint(BreakpointType type, uint32_t addr)
{
	uint32_t absoluteAddr = _mapper->ToAbsoluteAddress(addr);
	vector<shared_ptr<Breakpoint>> *breakpoints = nullptr;

	switch(type) {
		case BreakpointType::Execute: breakpoints = &_execBreakpoints; break;
		case BreakpointType::Read: breakpoints = &_readBreakpoints; break;
		case BreakpointType::Write: breakpoints = &_writeBreakpoints; break;
	}
	
	_bpLock.Acquire();
	for(int i = 0, len = breakpoints->size(); i < len; i++) {
		shared_ptr<Breakpoint> breakpoint = (*breakpoints)[i];
		if(breakpoint->Matches(addr, absoluteAddr)) {
			_bpLock.Release();
			return breakpoint;
		}
	}

	_bpLock.Release();
	return shared_ptr<Breakpoint>();
}

void Debugger::PrivateCheckBreakpoint(BreakpointType type, uint32_t addr)
{
	//Check if a breakpoint has been hit and freeze execution if one has
	bool breakDone = false;
	if(type == BreakpointType::Execute) {
		_disassembler->BuildCache(_mapper->ToAbsoluteAddress(addr), addr);
		breakDone = SleepUntilResume();
	}

	if(!breakDone && GetMatchingBreakpoint(type, addr)) {
		//Found a matching breakpoint, stop execution
		Step(1);
		SleepUntilResume();
	}
}

bool Debugger::SleepUntilResume()
{
	uint32_t stepCount = _stepCount.load();
	if(stepCount > 0) {
		_stepCount--;
		stepCount = _stepCount.load();
	}

	if(stepCount == 0) {
		//Break
		_console->SendNotification(ConsoleNotificationType::CodeBreak);
		while(stepCount == 0) {
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(10));
			stepCount = _stepCount.load();
		}
		return true;
	}
	return false;
}

State Debugger::GetCPUState()
{
	return _cpu->GetState();
}

void Debugger::Step(uint32_t count)
{
	//Run CPU for [count] cycles and before breaking again
	_stepCount = count;
}

void Debugger::Run()
{
	//Resume execution after a breakpoint has been hit
	_stepCount = -1;
}

bool Debugger::IsCodeChanged()
{
	string output = GenerateOutput();
	if(_outputCache.compare(output) == 0) {
		return false;
	} else {
		_outputCache = output;
		return true;
	}
}

string Debugger::GenerateOutput()
{
	vector<uint32_t> memoryRanges = _mapper->GetPRGRanges();

	std::ostringstream output;
	uint16_t memoryAddr = 0x8000;
	for(int i = 0, size = memoryRanges.size(); i < size; i += 2) {
		output << _disassembler->GetCode(memoryRanges[i], memoryRanges[i+1], memoryAddr);
	}

	return output.str();
}

string Debugger::GetCode()
{
	return _outputCache;
}

uint8_t Debugger::GetMemoryValue(uint32_t addr)
{
	return _memoryManager->DebugRead(addr);
}

uint32_t Debugger::GetRelativeAddress(uint32_t addr)
{
	return _mapper->FromAbsoluteAddress(addr);
}

void Debugger::CheckBreakpoint(BreakpointType type, uint32_t addr)
{
	if(Debugger::Instance) {
		Debugger::Instance->PrivateCheckBreakpoint(type, addr);
	}
}