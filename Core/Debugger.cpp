#pragma once

#include "stdafx.h"
#include <thread>
#include "MessageManager.h"
#include "Debugger.h"
#include "Console.h"
#include "BaseMapper.h"
#include "Disassembler.h"
#include "APU.h"

Debugger* Debugger::Instance = nullptr;

Debugger::Debugger(shared_ptr<Console> console, shared_ptr<CPU> cpu, shared_ptr<PPU> ppu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper)
{
	_console = console;
	_cpu = cpu;
	_ppu = ppu;
	_memoryManager = memoryManager;
	_mapper = mapper;

	uint8_t *prgBuffer;
	mapper->GetPrgCopy(&prgBuffer);
	_disassembler.reset(new Disassembler(memoryManager->GetInternalRAM(), prgBuffer, mapper->GetPrgSize()));

	_stepCount = -1;
		
	Debugger::Instance = this;
}

Debugger::~Debugger()
{
	Console::Pause();
	Debugger::Instance = nullptr;
	Run();
	_breakLock.Acquire();
	_breakLock.Release();
	Console::Resume();
}

void Debugger::AddBreakpoint(BreakpointType type, uint32_t address, bool isAbsoluteAddr, bool enabled)
{
	_bpLock.Acquire();

	if(isAbsoluteAddr) {
		address = _mapper->ToAbsoluteAddress(address);
	}

	shared_ptr<Breakpoint> breakpoint(new Breakpoint(type, address, isAbsoluteAddr));
	breakpoint->SetEnabled(enabled);
	switch(type) {
		case BreakpointType::Execute: _execBreakpoints.push_back(breakpoint); break;
		case BreakpointType::Read: _readBreakpoints.push_back(breakpoint); break;
		case BreakpointType::Write: _writeBreakpoints.push_back(breakpoint); break;
	}

	_bpLock.Release();
}

void Debugger::RemoveBreakpoint(BreakpointType type, uint32_t address, bool isAbsoluteAddr)
{
	_bpLock.Acquire();
	
	vector<shared_ptr<Breakpoint>> *breakpoints = nullptr;
	switch(type) {
		case BreakpointType::Execute: breakpoints = &_execBreakpoints; break;
		case BreakpointType::Read: breakpoints = &_readBreakpoints; break;
		case BreakpointType::Write: breakpoints = &_writeBreakpoints; break;
	}

	shared_ptr<Breakpoint> breakpoint = GetMatchingBreakpoint(type, address);
	for(size_t i = 0, len = breakpoints->size(); i < len; i++) {
		shared_ptr<Breakpoint> breakpoint = (*breakpoints)[i];
		if(breakpoint->GetAddr() == address && breakpoint->IsAbsoluteAddr() == isAbsoluteAddr) {
			breakpoints->erase(breakpoints->begin() + i);
			break;
		}
	}
	_bpLock.Release();
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
	for(size_t i = 0, len = breakpoints->size(); i < len; i++) {
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
	_breakLock.Acquire();

	//Check if a breakpoint has been hit and freeze execution if one has
	bool breakDone = false;
	if(type == BreakpointType::Execute) {
		_lastInstruction = _memoryManager->DebugRead(addr);
		if(_stepOut && _lastInstruction == 0x60) {
			//RTS found, set StepCount to 2 to break on the following instruction
			Step(2);
		} else if(_stepOverAddr != -1 && addr == _stepOverAddr) {
			Step(1);
		} else if(_stepCycleCount != -1 && abs(_cpu->GetCycleCount() - _stepCycleCount) < 100 && _cpu->GetCycleCount() >= _stepCycleCount) {
			Step(1);
		}
		_disassembler->BuildCache(_mapper->ToAbsoluteAddress(addr), addr);
		breakDone = SleepUntilResume();
	}

	if(!breakDone && GetMatchingBreakpoint(type, addr)) {
		//Found a matching breakpoint, stop execution
		Step(1);
		SleepUntilResume();
	}

	_breakLock.Release();
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
		APU::StopAudio();
		MessageManager::SendNotification(ConsoleNotificationType::CodeBreak);
		_stepOverAddr = -1;
		while(stepCount == 0) {
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(10));
			stepCount = _stepCount.load();
		}
		return true;
	}
	return false;
}

void Debugger::GetState(DebugState *state)
{
	state->CPU = _cpu->GetState();
	state->PPU = _ppu->GetState();
}

void Debugger::Step(uint32_t count)
{
	//Run CPU for [count] INSTRUCTIONS and before breaking again
	_stepOut = false;
	_stepCount = count;
	_stepOverAddr = -1;
	_stepCycleCount = -1;
}

void Debugger::StepCycles(uint32_t count)
{
	//Run CPU for [count] CYCLES and before breaking again
	_stepCycleCount = _cpu->GetCycleCount() + count;
	Run();
}

void Debugger::StepOut()
{
	_stepOut = true;
	_stepCount = -1;
	_stepOverAddr = -1;
	_stepCycleCount = -1;
}

void Debugger::StepOver()
{
	if(_lastInstruction == 0x20 || _lastInstruction == 0x00) {
		//We are on a JSR/BRK instruction, need to continue until the following instruction
		_stepOverAddr = _cpu->GetState().PC + (_lastInstruction == 0x20 ? 3 : 1);
		Run();
	} else {
		//Except for JSR & BRK, StepOver behaves the same as StepTnto
		Step(1);
	}
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
	std::ostringstream output;
	vector<uint32_t> memoryRanges = _mapper->GetPRGRanges();

	//RAM code viewer doesn't work well yet
	//output << _disassembler->GetRAMCode();

	uint16_t memoryAddr = 0x8000;
	for(size_t i = 0, size = memoryRanges.size(); i < size; i += 2) {
		uint32_t startRange = memoryRanges[i];
		//Merge all sequential ranges into 1 chunk
		for(size_t j = i+1; j < size - 1; j+=2) {
			if(memoryRanges[j] + 1 == memoryRanges[j + 1]) {
				i+=2;
			} else {
				break;
			}
		}
		output << _disassembler->GetCode(startRange, memoryRanges[i+1], memoryAddr);
	}

	return output.str();
}

string* Debugger::GetCode()
{
	return &_outputCache;
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

uint32_t Debugger::GetMemoryState(DebugMemoryType type, uint8_t *buffer)
{
	switch(type) {
		case DebugMemoryType::CpuMemory:
			for(int i = 0; i <= 0xFFFF; i++) {
				buffer[i] = _memoryManager->DebugRead(i);
			}
			return 0x10000;

		case DebugMemoryType::PpuMemory:
			for(int i = 0; i <= 0x3FFF; i++) {
				buffer[i] = _memoryManager->DebugReadVRAM(i);
			}
			return 0x4000;

		case DebugMemoryType::PaletteMemory:
			for(int i = 0; i <= 0x1F; i++) {
				buffer[i] = _ppu->ReadPaletteRAM(i);
			}
			return 0x20;

		case DebugMemoryType::SpriteMemory:
			memcpy(buffer, _ppu->GetSpriteRam(), 0x100);
			return 0x100;

		case DebugMemoryType::SecondarySpriteMemory:
			memcpy(buffer, _ppu->GetSecondarySpriteRam(), 0x20);
			return 0x20;

		case DebugMemoryType::PrgRom:
			uint8_t *prgRom;
			_mapper->GetPrgCopy(&prgRom);
			memcpy(buffer, prgRom, _mapper->GetPrgSize());
			delete[] prgRom;
			return _mapper->GetPrgSize();

		case DebugMemoryType::ChrRom:
			uint8_t *chrRom;
			_mapper->GetChrCopy(&chrRom);
			memcpy(buffer, chrRom, _mapper->GetChrSize());
			delete[] chrRom;
			return _mapper->GetChrSize();
	}
	return 0;
}