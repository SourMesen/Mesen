#include "stdafx.h"
#include <thread>
#include "MessageManager.h"
#include "Debugger.h"
#include "Console.h"
#include "BaseMapper.h"
#include "Disassembler.h"
#include "VideoDecoder.h"
#include "APU.h"
#include "SoundMixer.h"
#include "CodeDataLogger.h"
#include "ExpressionEvaluator.h"

Debugger* Debugger::Instance = nullptr;
const int Debugger::BreakpointTypeCount;

Debugger::Debugger(shared_ptr<Console> console, shared_ptr<CPU> cpu, shared_ptr<PPU> ppu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper)
{
	_romName = Console::GetRomName();
	_console = console;
	_cpu = cpu;
	_ppu = ppu;
	_memoryManager = memoryManager;
	_mapper = mapper;

	_disassembler.reset(new Disassembler(memoryManager->GetInternalRAM(), mapper->GetPrgRom(), mapper->GetPrgSize(), mapper->GetWorkRam(), mapper->GetPrgSize(true)));
	_codeDataLogger.reset(new CodeDataLogger(mapper->GetPrgSize(), mapper->GetChrSize()));
	_memoryDumper.reset(new MemoryDumper(_ppu, _memoryManager, _mapper, _codeDataLogger));

	_stepOut = false;
	_stepCount = -1;
	_stepOverAddr = -1;
	_stepCycleCount = -1;

	_flags = 0;

	_bpUpdateNeeded = false;
	_executionStopped = false;

	LoadCdlFile(FolderUtilities::CombinePath(FolderUtilities::GetDebuggerFolder(), FolderUtilities::GetFilename(_romName, false) + ".cdl"));
		
	Debugger::Instance = this;
}

Debugger::~Debugger()
{
	SaveCdlFile(FolderUtilities::CombinePath(FolderUtilities::GetDebuggerFolder(), FolderUtilities::GetFilename(_romName, false) + ".cdl"));

	_stopFlag = true;

	Console::Pause();
	Debugger::Instance = nullptr;
	_breakLock.Acquire();
	_breakLock.Release();
	Console::Resume();
}

void Debugger::Suspend()
{
	_suspendCount++;
	while(_executionStopped) {}
}

void Debugger::Resume()
{
	_suspendCount--;
	if(_suspendCount < 0) {
		_suspendCount = 0;
	}
}

void Debugger::SetFlags(uint32_t flags)
{
	_flags = flags;
}

bool Debugger::CheckFlag(DebuggerFlags flag)
{
	return (_flags & (uint32_t)flag) == (uint32_t)flag;
}

bool Debugger::IsEnabled()
{
	return Debugger::Instance != nullptr;
}

void Debugger::BreakIfDebugging()
{
	if(Debugger::Instance != nullptr) {
		Debugger::Instance->Step(1);
		Debugger::Instance->SleepUntilResume();
	}
}

bool Debugger::LoadCdlFile(string cdlFilepath)
{
	if(_codeDataLogger->LoadCdlFile(cdlFilepath)) {
		for(int i = 0, len = _mapper->GetPrgSize(); i < len; i++) {
			if(_codeDataLogger->IsCode(i)) {
				i = _disassembler->BuildCache(i, -1, 0xFFFF, false) - 1;
			}
		}
		return true;
	}
	return false;
}

bool Debugger::SaveCdlFile(string cdlFilepath)
{
	return _codeDataLogger->SaveCdlFile(cdlFilepath);
}

void Debugger::ResetCdlLog()
{
	_codeDataLogger->Reset();
}

CdlRatios Debugger::GetCdlRatios()
{
	return _codeDataLogger->GetRatios();
}

void Debugger::SetBreakpoints(Breakpoint breakpoints[], uint32_t length)
{
	_bpUpdateLock.AcquireSafe();

	_newBreakpoints.clear();
	_newBreakpoints.insert(_newBreakpoints.end(), breakpoints, breakpoints + length);	
	_bpUpdateNeeded = true;

	if(_executionStopped) {
		UpdateBreakpoints();
	} else {
		bool hasBreakpoint[Debugger::BreakpointTypeCount]{ false,false,false,false,false,false };
		for(Breakpoint &bp : _newBreakpoints) {
			for(int i = 0; i < Debugger::BreakpointTypeCount; i++) {
				hasBreakpoint[i] |= bp.HasBreakpointType((BreakpointType)i);
			}
		}

		for(int i = 0; i < Debugger::BreakpointTypeCount; i++) {
			_hasBreakpoint[i] = hasBreakpoint[i];
		}
	}
}

void Debugger::UpdateBreakpoints()
{
	if(_bpUpdateNeeded) {
		_bpUpdateLock.AcquireSafe();

		for(int i = 0; i < Debugger::BreakpointTypeCount; i++) {
			_breakpoints[i].clear();
			_hasBreakpoint[i] = false;
		}

		ExpressionEvaluator expEval;
		for(Breakpoint &bp : _newBreakpoints) {
			if(!expEval.Validate(bp.GetCondition())) {
				//Remove any invalid condition (syntax-wise)
				bp.ClearCondition();
			}

			for(int i = 0; i < Debugger::BreakpointTypeCount; i++) {
				if(bp.HasBreakpointType((BreakpointType)i)) {
					_breakpoints[i].push_back(bp);
					_hasBreakpoint[i] = true;
				}
			}
		}

		_bpUpdateNeeded = false;
	}
}

bool Debugger::HasMatchingBreakpoint(BreakpointType type, uint32_t addr, int16_t value)
{
	UpdateBreakpoints();

	uint32_t absoluteAddr = _mapper->ToAbsoluteAddress(addr);
	vector<Breakpoint> &breakpoints = _breakpoints[(int)type];

	bool needState = true;
	for(size_t i = 0, len = breakpoints.size(); i < len; i++) {
		Breakpoint &breakpoint = breakpoints[i];
		if(type == BreakpointType::Global || breakpoint.Matches(addr, absoluteAddr)) {
			string condition = breakpoint.GetCondition();
			if(condition.empty()) {
				return true;
			} else {
				ExpressionEvaluator expEval;
				if(needState) {
					GetState(&_debugState);
					needState = false;
				}
				if(expEval.Evaluate(condition, _debugState, value, addr) != 0) {
					return true;
				}
			}
		}
	}

	return false;
}

int32_t Debugger::EvaluateExpression(string expression, EvalResultType &resultType)
{
	ExpressionEvaluator expEval;

	DebugState state;
	GetState(&state);

	return expEval.Evaluate(expression, state, resultType);
}

void Debugger::UpdateCallstack(uint32_t addr)
{
	if(_lastInstruction == 0x60 && !_callstackRelative.empty()) {
		//RTS
		_callstackRelative.pop_back();
		_callstackRelative.pop_back();
		_callstackAbsolute.pop_back();
		_callstackAbsolute.pop_back();
	} else if(_lastInstruction == 0x20 && _callstackRelative.size() < 1022) {
		//JSR
		uint16_t targetAddr = _memoryManager->DebugRead(addr + 1) | (_memoryManager->DebugRead(addr + 2) << 8);
		_callstackRelative.push_back(addr);
		_callstackRelative.push_back(targetAddr);

		_callstackAbsolute.push_back(_mapper->ToAbsoluteAddress(addr));
		_callstackAbsolute.push_back(_mapper->ToAbsoluteAddress(targetAddr));
	}
}

void Debugger::ProcessStepConditions(uint32_t addr)
{
	if(_stepOut && _lastInstruction == 0x60) {
		//RTS found, set StepCount to 2 to break on the following instruction
		Step(2);
	} else if(_stepOverAddr != -1 && addr == (uint32_t)_stepOverAddr) {
		Step(1);
	} else if(_stepCycleCount != -1 && abs(_cpu->GetCycleCount() - _stepCycleCount) < 100 && _cpu->GetCycleCount() >= _stepCycleCount) {
		Step(1);
	}
}

void Debugger::PrivateProcessPpuCycle()
{
	if(_hasBreakpoint[BreakpointType::Global] && HasMatchingBreakpoint(BreakpointType::Global, 0, -1)) {
		//Found a matching breakpoint, stop execution
		Step(1);
		SleepUntilResume();
	} else if(_ppuStepCount > 0) {
		_ppuStepCount--;
		if(_ppuStepCount == 0) {
			Step(1);
			SleepUntilResume();
		}
	}
}

void Debugger::PrivateProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t &value)
{
	_currentReadAddr = &addr;
	_currentReadValue = &value;

	//Check if a breakpoint has been hit and freeze execution if one has
	bool breakDone = false;
	int32_t absoluteAddr = _mapper->ToAbsoluteAddress(addr);
	int32_t absoluteRamAddr = _mapper->ToAbsoluteRamAddress(addr);

	if(absoluteAddr >= 0) {
		if(type == MemoryOperationType::ExecOperand) {
			_codeDataLogger->SetFlag(absoluteAddr, CdlPrgFlags::Code);
		} else {
			_codeDataLogger->SetFlag(absoluteAddr, CdlPrgFlags::Data);
		}
	} else if(addr < 0x2000 || absoluteRamAddr >= 0) {
		if(type == MemoryOperationType::Write) {
			_disassembler->InvalidateCache(addr, absoluteRamAddr);
		}
	}

	if(type == MemoryOperationType::ExecOpCode) {
		if(absoluteAddr >= 0) {
			_codeDataLogger->SetFlag(absoluteAddr, CdlPrgFlags::Code);
		}
		
		bool isSubEntryPoint = _lastInstruction == 0x20; //Previous instruction was a JSR
		_disassembler->BuildCache(absoluteAddr, absoluteRamAddr, addr, isSubEntryPoint);
		_lastInstruction = _memoryManager->DebugRead(addr);

		UpdateCallstack(addr);
		ProcessStepConditions(addr);

		breakDone = SleepUntilResume();

		shared_ptr<TraceLogger> logger = _traceLogger;
		if(logger) {
			DebugState state;
			GetState(&state);
			logger->Log(state, _disassembler->GetDisassemblyInfo(absoluteAddr, absoluteRamAddr, addr));
		}
	}

	if(!breakDone) {
		BreakpointType breakpointType = BreakpointType::Execute;
		switch(type) {
			case MemoryOperationType::Read: breakpointType = BreakpointType::ReadRam; break;
			case MemoryOperationType::Write: breakpointType = BreakpointType::WriteRam; break;
		}

		if(_hasBreakpoint[breakpointType] && HasMatchingBreakpoint(breakpointType, addr, (type == MemoryOperationType::ExecOperand) ? -1 : value)) {
			//Found a matching breakpoint, stop execution
			Step(1);
			SleepUntilResume();
		}
	}

	_currentReadAddr = nullptr;
}

bool Debugger::SleepUntilResume()
{
	int32_t stepCount = _stepCount.load();
	if(stepCount > 0) {
		_stepCount--;
		stepCount = _stepCount.load();
	}

	if(stepCount == 0 && !_stopFlag && _suspendCount == 0) {
		//Break
		auto lock = _breakLock.AcquireSafe();
		_executionStopped = true;
		SoundMixer::StopAudio();
		MessageManager::SendNotification(ConsoleNotificationType::CodeBreak);
		_stepOverAddr = -1;
		if(CheckFlag(DebuggerFlags::PpuPartialDraw)) {
			_ppu->DebugSendFrame();
		}
		while(stepCount == 0 && !_stopFlag && _suspendCount == 0) {
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(10));
			stepCount = _stepCount.load();
		}
		_executionStopped = false;
		return true;
	}
	return false;
}

void Debugger::PrivateProcessVramOperation(MemoryOperationType type, uint16_t addr, uint8_t value)
{
	if(type != MemoryOperationType::Write) {
		int32_t absoluteAddr = _mapper->ToAbsoluteChrAddress(addr);
		_codeDataLogger->SetFlag(absoluteAddr, type == MemoryOperationType::Read ? CdlChrFlags::Read : CdlChrFlags::Drawn);
	}

	BreakpointType bpType = type == MemoryOperationType::Write ? BreakpointType::WriteVram : BreakpointType::ReadVram;
	if(_hasBreakpoint[bpType] && HasMatchingBreakpoint(bpType, addr, value)) {
		//Found a matching breakpoint, stop execution
		Step(1);
		SleepUntilResume();
	}
}

void Debugger::GetState(DebugState *state)
{
	state->CPU = _cpu->GetState();
	state->PPU = _ppu->GetState();
	state->Cartridge = _mapper->GetState();
}

void Debugger::PpuStep(uint32_t count)
{
	_stepCount = -1;
	_ppuStepCount = count;
	_stepOverAddr = -1;
	_stepCycleCount = -1;
}

void Debugger::Step(uint32_t count)
{
	//Run CPU for [count] INSTRUCTIONS before breaking again
	_stepOut = false;
	_stepCount = count;
	_stepOverAddr = -1;
	_stepCycleCount = -1;
	_ppuStepCount = -1;
}

void Debugger::StepCycles(uint32_t count)
{
	//Run CPU for [count] CYCLES before breaking again
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
	_ppuStepCount = -1;
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

	//Get code in internal RAM
	output << _disassembler->GetCode(0x0000, 0x1FFF, 0x0000, PrgMemoryType::PrgRom);
	output << "2000:::--END OF INTERNAL RAM--\n";

	for(uint32_t i = 0x2000; i < 0x10000; i += 0x100) {
		//Merge all sequential ranges into 1 chunk
		int32_t romAddr = _mapper->ToAbsoluteAddress(i);
		int32_t ramAddr = _mapper->ToAbsoluteRamAddress(i);
		uint32_t startMemoryAddr = i;
		int32_t startAddr, endAddr;

		if(romAddr >= 0) {
			startAddr = romAddr;
			endAddr = startAddr + 0xFF;
			while(romAddr + 0x100 == _mapper->ToAbsoluteAddress(i + 0x100) && i < 0x10000) {
				endAddr += 0x100;
				romAddr += 0x100;
				i+=0x100;
			}
			output << _disassembler->GetCode(startAddr, endAddr, startMemoryAddr, PrgMemoryType::PrgRom);
		} else if(ramAddr >= 0) {
			startAddr = ramAddr;
			endAddr = startAddr + 0xFF;
			while(ramAddr + 0x100 == _mapper->ToAbsoluteRamAddress(i + 0x100) && i < 0x10000) {
				endAddr += 0x100;
				ramAddr += 0x100;
				i += 0x100;
			}
			output << _disassembler->GetCode(startAddr, endAddr, startMemoryAddr, PrgMemoryType::WorkRam);
		}
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

uint32_t Debugger::GetAbsoluteAddress(uint32_t addr)
{
	return _mapper->ToAbsoluteAddress(addr);
}

void Debugger::SetNextStatement(uint16_t addr)
{
	if(_currentReadAddr) {
		_cpu->SetDebugPC(addr);
		*_currentReadAddr = addr;
		*_currentReadValue = _memoryManager->DebugRead(addr);
	}
}

void Debugger::StartTraceLogger(TraceLoggerOptions options)
{
	string traceFilepath = FolderUtilities::CombinePath(FolderUtilities::GetDebuggerFolder(), "Trace - " + FolderUtilities::GetFilename(_romName, false) + ".log");
	_traceLogger.reset(new TraceLogger(traceFilepath, options));
}

void Debugger::StopTraceLogger()
{
	_traceLogger.reset();
}

void Debugger::ProcessPpuCycle()
{
	if(Debugger::Instance) {
		Debugger::Instance->PrivateProcessPpuCycle();
	}
}

void Debugger::ProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t &value)
{
	if(Debugger::Instance) {
		Debugger::Instance->PrivateProcessRamOperation(type, addr, value);
	}
}

void Debugger::ProcessVramOperation(MemoryOperationType type, uint16_t addr, uint8_t value)
{
	if(Debugger::Instance) {
		Debugger::Instance->PrivateProcessVramOperation(type, addr, value);
	}
}

void Debugger::GetCallstack(int32_t* callstackAbsolute, int32_t* callstackRelative)
{
	for(size_t i = 0, len = _callstackRelative.size(); i < len; i++) {
		callstackAbsolute[i] = _callstackAbsolute[i];
		
		int32_t relativeAddr = _callstackRelative[i];
		if(_mapper->FromAbsoluteAddress(_callstackAbsolute[i]) == -1) {
			//Mark address as an unmapped memory addr
			relativeAddr |= 0x10000;
		}
		callstackRelative[i] = relativeAddr;
	}
	callstackAbsolute[_callstackRelative.size()] = -2;
	callstackRelative[_callstackRelative.size()] = -2;
}

shared_ptr<MemoryDumper> Debugger::GetMemoryDumper()
{
	return _memoryDumper;
}