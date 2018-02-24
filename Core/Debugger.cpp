#include "stdafx.h"
#include <thread>
#include "../Utilities/FolderUtilities.h"
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
#include "LabelManager.h"
#include "MemoryDumper.h"
#include "MemoryAccessCounter.h"
#include "Profiler.h"
#include "Assembler.h"
#include "CodeRunner.h"
#include "DisassemblyInfo.h"
#include "PPU.h"
#include "MemoryManager.h"
#include "RewindManager.h"
#include "DebugBreakHelper.h"
#include "ScriptHost.h"
#include "DebugHud.h"
#include "StandardController.h"

#ifndef UINT32_MAX
#define UINT32_MAX  ((uint32_t)-1)
#endif

Debugger* Debugger::Instance = nullptr;
const int Debugger::BreakpointTypeCount;
string Debugger::_disassemblerOutput = "";

Debugger::Debugger(shared_ptr<Console> console, shared_ptr<CPU> cpu, shared_ptr<PPU> ppu, shared_ptr<APU> apu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper)
{
	_romName = Console::GetRomName();
	_console = console;
	_cpu = cpu;
	_ppu = ppu;
	_apu = apu;
	_memoryManager = memoryManager;
	_mapper = mapper;

	_labelManager.reset(new LabelManager(_mapper));
	_assembler.reset(new Assembler(_labelManager));
	_disassembler.reset(new Disassembler(memoryManager.get(), mapper.get(), this));
	_codeDataLogger.reset(new CodeDataLogger(this, mapper->GetMemorySize(DebugMemoryType::PrgRom), mapper->GetMemorySize(DebugMemoryType::ChrRom)));
	_memoryDumper.reset(new MemoryDumper(_ppu, _memoryManager, _mapper, _codeDataLogger, this, _disassembler));
	_memoryAccessCounter.reset(new MemoryAccessCounter(this));
	_profiler.reset(new Profiler(this));
	_traceLogger.reset(new TraceLogger(this, memoryManager, _labelManager));
	_debugHud.reset(new DebugHud());

	_stepOut = false;
	_stepCount = -1;
	_stepOverAddr = -1;
	_stepCycleCount = -1;
	_ppuStepCount = -1;
	_breakRequested = false;
	_breakOnScanline = -2;

	_preventResume = 0;
	_stopFlag = false;
	_suspendCount = 0;

	_lastInstruction = 0;

	_currentReadAddr = nullptr;
	_currentReadValue = nullptr;
	_nextReadAddr = -1;
	_returnToAddress = 0;

	_ppuScrollX = 0;
	_ppuScrollY = 0;

	_ppuViewerScanline = 241;
	_ppuViewerCycle = 0;

	_flags = 0;

	_runToCycle = 0;
	_prevInstructionCycle = 0;
	_curInstructionCycle = 0;
	_needRewind = false;

	_executionStopped = false;
	_hideTopOfCallstack = false;

	_disassemblerOutput = "";
	
	memset(_inputOverride, 0, sizeof(_inputOverride));

	_frozenAddresses.insert(_frozenAddresses.end(), 0x10000, 0);

	if(!LoadCdlFile(FolderUtilities::CombinePath(FolderUtilities::GetDebuggerFolder(), FolderUtilities::GetFilename(_romName, false) + ".cdl"))) {
		_disassembler->Reset();
	}

	_hasScript = false;
	_nextScriptId = 0;

	Debugger::Instance = this;
}

Debugger::~Debugger()
{
	_codeDataLogger->SaveCdlFile(FolderUtilities::CombinePath(FolderUtilities::GetDebuggerFolder(), FolderUtilities::GetFilename(_romName, false) + ".cdl"));

	_stopFlag = true;

	Console::Pause();
	if(Debugger::Instance == this) {
		Debugger::Instance = nullptr;
	}
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
	bool needUpdate = ((flags ^ _flags) & (int)DebuggerFlags::DisplayOpCodesInLowerCase) != 0;
	_flags = flags;
	if(needUpdate) {
		_disassembler->BuildOpCodeTables(CheckFlag(DebuggerFlags::DisplayOpCodesInLowerCase));
	}
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
	} else if(EmulationSettings::CheckFlag(EmulationFlags::BreakOnCrash)) {
		//When "Break on Crash" is enabled, open the debugger and break immediately if a crash occurs
		Console::GetInstance()->GetDebugger(true);
		Debugger::Instance->Step(1);
		Debugger::Instance->SleepUntilResume();
	}
}

bool Debugger::LoadCdlFile(string cdlFilepath)
{
	if(_codeDataLogger->LoadCdlFile(cdlFilepath)) {
		//Can't use DebugBreakHelper due to the fact this is called in the constructor
		bool isEmulationThread = Console::GetEmulationThreadId() == std::this_thread::get_id();
		if(!isEmulationThread) {
			Console::Pause();
		}
		UpdateCdlCache();
		if(!isEmulationThread) {
			Console::Resume();
		}
		return true;
	}
	return false;
}

void Debugger::SetCdlData(uint8_t* cdlData, uint32_t length)
{
	DebugBreakHelper helper(this);
	_codeDataLogger->SetCdlData(cdlData, length);
	UpdateCdlCache();
}

void Debugger::ResetCdl()
{
	DebugBreakHelper helper(this);
	_codeDataLogger->Reset();
	UpdateCdlCache();
}

void Debugger::UpdateCdlCache()
{
	_disassembler->Reset();
	for(int i = 0, len = _mapper->GetMemorySize(DebugMemoryType::PrgRom); i < len; i++) {
		if(_codeDataLogger->IsCode(i)) {
			AddressTypeInfo info = { i, AddressType::PrgRom };
			i = _disassembler->BuildCache(info, 0, _codeDataLogger->IsSubEntryPoint(i)) - 1;
		}
	}

	_functionEntryPoints.clear();
	for(int i = 0, len = _mapper->GetMemorySize(DebugMemoryType::PrgRom); i < len; i++) {
		if(_codeDataLogger->IsSubEntryPoint(i)) {
			_functionEntryPoints.emplace(i);
		}
	}
}

bool Debugger::IsMarkedAsCode(uint16_t relativeAddress)
{
	AddressTypeInfo info;
	GetAbsoluteAddressAndType(relativeAddress, &info);
	if(info.Address >= 0 && info.Type == AddressType::PrgRom) {
		return _codeDataLogger->IsCode(info.Address);
	} else {
		return false;
	}
}

shared_ptr<CodeDataLogger> Debugger::GetCodeDataLogger()
{
	return _codeDataLogger;
}

shared_ptr<LabelManager> Debugger::GetLabelManager()
{
	return _labelManager;
}

shared_ptr<Profiler> Debugger::GetProfiler()
{
	return _profiler;
}

void Debugger::SetBreakpoints(Breakpoint breakpoints[], uint32_t length)
{
	DebugBreakHelper helper(this);

	for(int i = 0; i < Debugger::BreakpointTypeCount; i++) {
		_breakpoints[i].clear();
		_breakpointRpnList[i].clear();
		_hasBreakpoint[i] = false;
	}

	ExpressionEvaluator expEval(this);
	for(uint32_t j = 0; j < length; j++) {
		Breakpoint &bp = breakpoints[j];
		for(int i = 0; i < Debugger::BreakpointTypeCount; i++) {
			bool isEnabled = bp.IsEnabled() && EmulationSettings::CheckFlag(EmulationFlags::DebuggerWindowEnabled);
			if((bp.IsMarked() || isEnabled) && bp.HasBreakpointType((BreakpointType)i)) {
				_breakpoints[i].push_back(bp);
				vector<int> *rpnList = expEval.GetRpnList(bp.GetCondition());
				_breakpointRpnList[i].push_back(rpnList ? *rpnList : vector<int>());
				_hasBreakpoint[i] = true;
			}
		}
	}
}

void Debugger::ProcessBreakpoints(BreakpointType type, OperationInfo &operationInfo, bool allowBreak)
{
	if(_runToCycle != 0) {
		//Disable all breakpoints while stepping backwards
		return;
	}

	uint32_t absoluteAddr;
	AddressTypeInfo info { -1, AddressType::InternalRam };
	PpuAddressTypeInfo ppuInfo { -1, PpuAddressType::None };
	bool isPpuBreakpoint = false;
	switch(type) {
		case BreakpointType::Execute:
		case BreakpointType::ReadRam:
		case BreakpointType::WriteRam:
			GetAbsoluteAddressAndType(operationInfo.Address, &info);
			absoluteAddr = info.Address;
			break;

		case BreakpointType::ReadVram:
		case BreakpointType::WriteVram:
			GetPpuAbsoluteAddressAndType(operationInfo.Address, &ppuInfo);
			absoluteAddr = ppuInfo.Address;
			isPpuBreakpoint = true;
			break;
	}

	vector<Breakpoint> &breakpoints = _breakpoints[(int)type];

	bool needBreak = false;
	bool needMark = false;
	bool needState = true;
	uint32_t markBreakpointId = 0;
	EvalResultType resultType;
	
	auto processBreakpoint = [&needMark, &needBreak, &markBreakpointId](Breakpoint &bp) {
		if(bp.IsMarked()) {
			needMark = true;
			markBreakpointId = bp.GetId();
		}
		needBreak |= bp.IsEnabled();
	};

	for(size_t i = 0, len = breakpoints.size(); i < len; i++) {
		Breakpoint &breakpoint = breakpoints[i];
		if(
			type == BreakpointType::Global ||
			!isPpuBreakpoint && breakpoint.Matches(operationInfo.Address, info) ||
			isPpuBreakpoint && breakpoint.Matches(operationInfo.Address, ppuInfo)
		) {
			if(!breakpoint.HasCondition()) {
				processBreakpoint(breakpoint);
			} else {
				if(needState) {
					GetState(&_debugState, false);
					needState = false;
				}
				if(_bpExpEval.Evaluate(_breakpointRpnList[(int)type][i], _debugState, resultType, operationInfo) != 0) {
					processBreakpoint(breakpoint);
				}
			}
		}

		if(needMark && needBreak) {
			//No need to process remaining breakpoints
			break;
		}
	}

	if(needMark) {
		AddDebugEvent(DebugEventType::Breakpoint, operationInfo.Address, (uint8_t)operationInfo.Value, markBreakpointId);
	}

	if(needBreak && allowBreak) {
		//Found a matching breakpoint, stop execution
		Step(1);
		SleepUntilResume();
	}
}

int32_t Debugger::EvaluateExpression(string expression, EvalResultType &resultType)
{
	DebugState state;
	OperationInfo operationInfo { 0, 0, MemoryOperationType::DummyRead };
	GetState(&state);
	return _watchExpEval.Evaluate(expression, state, resultType, operationInfo);
}

void Debugger::RemoveExcessCallstackEntries()
{
	while(_callstackRelative.size() >= 1022) {
		//Ensure callstack stays below 512 entries - some games never call RTI, causing an infinite stack
		_callstackRelative.pop_front();
		_callstackRelative.pop_front();
		_callstackAbsolute.pop_front();
		_callstackAbsolute.pop_front();
	}
}

void Debugger::UpdateCallstack(uint32_t addr)
{
	_hideTopOfCallstack = false;
	if((_lastInstruction == 0x60 || _lastInstruction == 0x40) && !_callstackRelative.empty()) {
		//RTS & RTI
		_callstackRelative.pop_back();
		_callstackRelative.pop_back();
		_callstackAbsolute.pop_back();
		_callstackAbsolute.pop_back();

		_profiler->UnstackFunction();
	} else if(_lastInstruction == 0x20) {
		//JSR
		RemoveExcessCallstackEntries();

		uint16_t targetAddr = _memoryManager->DebugRead(addr + 1) | (_memoryManager->DebugRead(addr + 2) << 8);
		_callstackRelative.push_back(addr);
		_callstackRelative.push_back(targetAddr);

		_callstackAbsolute.push_back(_mapper->ToAbsoluteAddress(addr));
		_callstackAbsolute.push_back(_mapper->ToAbsoluteAddress(targetAddr));

		_hideTopOfCallstack = true;

		_profiler->StackFunction(_mapper->ToAbsoluteAddress(addr), _mapper->ToAbsoluteAddress(targetAddr));
	}
}

void Debugger::PrivateProcessInterrupt(uint16_t cpuAddr, uint16_t destCpuAddr, bool forNmi)
{
	RemoveExcessCallstackEntries();

	_callstackRelative.push_back(cpuAddr | (forNmi ? 0x40000 : 0x20000));
	_callstackRelative.push_back(destCpuAddr);

	_callstackAbsolute.push_back(_mapper->ToAbsoluteAddress(cpuAddr));
	_callstackAbsolute.push_back(_mapper->ToAbsoluteAddress(destCpuAddr));

	_profiler->StackFunction(-1, _mapper->ToAbsoluteAddress(destCpuAddr));

	ProcessEvent(forNmi ? EventType::Nmi : EventType::Irq);
}

void Debugger::ProcessInterrupt(uint16_t cpuAddr, uint16_t destCpuAddr, bool forNmi)
{
	if(Debugger::Instance) {
		Debugger::Instance->PrivateProcessInterrupt(cpuAddr, destCpuAddr, forNmi);
	}
}

void Debugger::StaticProcessEvent(EventType type)
{
	if(Debugger::Instance) {
		Debugger::Instance->ProcessEvent(type);
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
	if(PPU::GetCurrentCycle() == (uint32_t)_ppuViewerCycle && PPU::GetCurrentScanline() == _ppuViewerScanline) {
		MessageManager::SendNotification(ConsoleNotificationType::PpuViewerDisplayFrame);
	} 
	if(PPU::GetCurrentCycle() == 0) {
		if(_breakOnScanline == PPU::GetCurrentScanline()) {
			Step(1);
			SleepUntilResume(BreakSource::Pause);
		}
		if(PPU::GetCurrentScanline() == 241) {
			ProcessEvent(EventType::EndFrame);
		} else if(PPU::GetCurrentScanline() == -1) {
			ProcessEvent(EventType::StartFrame);
		}
	}
	
	OperationInfo operationInfo { 0, 0, MemoryOperationType::DummyRead };
	
	if(_hasBreakpoint[BreakpointType::Global]) {
		ProcessBreakpoints(BreakpointType::Global, operationInfo);
	}

	if(_ppuStepCount > 0) {
		_ppuStepCount--;
		if(_ppuStepCount == 0) {
			Step(1);
			SleepUntilResume();
		}
	}
}

bool Debugger::PrivateProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t &value)
{
	OperationInfo operationInfo { addr, (int16_t)value, type };

	bool isDmcRead = false;
	if(type == MemoryOperationType::DmcRead) {
		//Used to flag the data in the CDL file
		isDmcRead = true;
		type = MemoryOperationType::Read;
	}

	ProcessCpuOperation(addr, value, type);

	if(type == MemoryOperationType::ExecOpCode) {
		if(_runToCycle == 0) {
			_rewindCache.clear();
			_rewindPrevInstructionCycleCache.clear();
		}

		if(_nextReadAddr != -1) {
			//SetNextStatement (either from manual action or code runner)
			if(addr < 0x3000 || addr >= 0x4000) {
				_returnToAddress = addr;
			}

			addr = _nextReadAddr;
			value = _memoryManager->DebugRead(addr, true);
			_cpu->SetDebugPC(addr);
			_nextReadAddr = -1;
		} else if(_needRewind) {
			//Step back - Need to load a state, and then alter the current opcode based on the new program counter
			if(!_rewindCache.empty()) {
				//Restore the state, and the cycle number of the instruction that preceeded that state
				//Otherwise, the target cycle number when building the next cache will be incorrect
				Console::LoadState(_rewindCache.back());
				_curInstructionCycle = _rewindPrevInstructionCycleCache.back();
				
				_rewindCache.pop_back();
				_rewindPrevInstructionCycleCache.pop_back();
				
				//This state is for the instruction we want to stop on, break here.
				_runToCycle = 0;
				Step(1);
			} else {
				RewindManager::StartRewinding(true);
			}
			UpdateProgramCounter(addr, value);
			_needRewind = false;
		}
		ProcessScriptSaveState(addr, value);

		_currentReadAddr = &addr;
		_currentReadValue = &value;
	}

	//Check if a breakpoint has been hit and freeze execution if one has
	bool breakDone = false;
	AddressTypeInfo addressInfo;
	GetAbsoluteAddressAndType(addr, &addressInfo);
	int32_t absoluteAddr = addressInfo.Type == AddressType::PrgRom ? addressInfo.Address : -1;
	int32_t absoluteRamAddr = addressInfo.Type == AddressType::WorkRam ? addressInfo.Address : -1;

	if(addressInfo.Address >= 0 && type != MemoryOperationType::DummyRead) {
		if(type == MemoryOperationType::Write && CheckFlag(DebuggerFlags::IgnoreRedundantWrites)) {
			if(_memoryManager->DebugRead(addr) != value) {
				_memoryAccessCounter->ProcessMemoryAccess(addressInfo, type, _cpu->GetCycleCount());
			}
		} else {
			_memoryAccessCounter->ProcessMemoryAccess(addressInfo, type, _cpu->GetCycleCount());
		}
	}

	if(absoluteAddr >= 0) {
		if(type == MemoryOperationType::ExecOperand) {
			_codeDataLogger->SetFlag(absoluteAddr, CdlPrgFlags::Code);
		} else if(type == MemoryOperationType::Read) {
			_codeDataLogger->SetFlag(absoluteAddr, CdlPrgFlags::Data);
			if(isDmcRead) {
				_codeDataLogger->SetFlag(absoluteAddr, CdlPrgFlags::PcmData);
			}
		}
	} else if(addr < 0x2000 || absoluteRamAddr >= 0) {
		if(type == MemoryOperationType::Write) {
			_disassembler->InvalidateCache(addressInfo);
		}
	}

	if(type == MemoryOperationType::ExecOpCode) {
		_prevInstructionCycle = _curInstructionCycle;
		_curInstructionCycle = CPU::GetCycleCount();

		bool isSubEntryPoint = _lastInstruction == 0x20; //Previous instruction was a JSR
		if(absoluteAddr >= 0) {
			_codeDataLogger->SetFlag(absoluteAddr, CdlPrgFlags::Code);
			if(isSubEntryPoint) {
				_codeDataLogger->SetFlag(absoluteAddr, CdlPrgFlags::SubEntryPoint);
				_functionEntryPoints.emplace(absoluteAddr);
			}
		}

		_disassembler->BuildCache(addressInfo, addr, isSubEntryPoint);

		ProcessStepConditions(addr);

		_profiler->ProcessInstructionStart(absoluteAddr);

		if(value == 0 && CheckFlag(DebuggerFlags::BreakOnBrk)) {
			Step(1);
		} else if(CheckFlag(DebuggerFlags::BreakOnUnofficialOpCode) && _disassembler->IsUnofficialOpCode(value)) {
			Step(1);
		} 

		if(_runToCycle != 0) {
			if(CPU::GetCycleCount() >= _runToCycle) {
				//Step back operation is done, revert RewindManager's state & break debugger
				RewindManager::StopRewinding(true);
				_runToCycle = 0;
				Step(1);
			} else if(_runToCycle - CPU::GetCycleCount() < 500) {
				_rewindCache.push_back(stringstream());
				Console::SaveState(_rewindCache.back());
				_rewindPrevInstructionCycleCache.push_back(_prevInstructionCycle);
			}
		}

		_lastInstruction = value;
		UpdateCallstack(addr);

		breakDone = SleepUntilResume();

		if(_codeRunner && !_codeRunner->IsRunning()) {
			_codeRunner.reset();
		}

		GetState(&_debugState, false);

		DisassemblyInfo disassemblyInfo;
		if(_codeRunner && _codeRunner->IsRunning() && addr >= 0x3000 && addr < 0x4000) {
			disassemblyInfo = _codeRunner->GetDisassemblyInfo(addr);
		} else {
			disassemblyInfo = _disassembler->GetDisassemblyInfo(addressInfo);
		}
		_traceLogger->Log(_debugState, disassemblyInfo, operationInfo);
	} else {
		_traceLogger->LogNonExec(operationInfo);
		_profiler->ProcessCycle();
	}

	if(type != MemoryOperationType::DummyRead) {
		BreakpointType breakpointType = BreakpointType::Execute;
		switch(type) {
			case MemoryOperationType::Read: breakpointType = BreakpointType::ReadRam; break;
			case MemoryOperationType::Write: breakpointType = BreakpointType::WriteRam; break;
		}

		if(_hasBreakpoint[breakpointType]) {
			ProcessBreakpoints(breakpointType, operationInfo, !breakDone);
		}
	}

	_currentReadAddr = nullptr;
	_currentReadValue = nullptr;

	if(type == MemoryOperationType::Write) {
		if(addr >= 0x2000 && addr <= 0x3FFF) {
			if((addr & 0x07) == 5 || (addr & 0x07) == 6) {
				GetState(&_debugState, false);
				AddDebugEvent(DebugEventType::PpuRegisterWrite, addr, value, -1, _debugState.PPU.State.WriteToggle ? 1 : 0);
			} else {
				AddDebugEvent(DebugEventType::PpuRegisterWrite, addr, value);
			}
		} else if(addr >= 0x4018 && _mapper->IsWriteRegister(addr)) {
			AddDebugEvent(DebugEventType::MapperRegisterWrite, addr, value);
		}

		if(_frozenAddresses[addr]) {
			return false;
		}
	} else if(type == MemoryOperationType::Read) {
		if(addr >= 0x2000 && addr <= 0x3FFF) {
			AddDebugEvent(DebugEventType::PpuRegisterRead, addr, value);
		} else if(addr >= 0x4018 && _mapper->IsReadRegister(addr)) {
			AddDebugEvent(DebugEventType::MapperRegisterRead, addr, value);
		}
	}

	return true;
}

bool Debugger::SleepUntilResume(BreakSource source)
{
	int32_t stepCount = _stepCount.load();
	if(stepCount > 0) {
		_stepCount--;
		stepCount = _stepCount.load();
	}

	if((stepCount == 0 || _breakRequested) && !_stopFlag && _suspendCount == 0) {
		//Break
		auto lock = _breakLock.AcquireSafe();
				
		if(_preventResume == 0) {
			SoundMixer::StopAudio();
			MessageManager::SendNotification(ConsoleNotificationType::CodeBreak, (void*)(uint64_t)source);
			ProcessEvent(EventType::CodeBreak);
			_stepOverAddr = -1;
			if(CheckFlag(DebuggerFlags::PpuPartialDraw)) {
				_ppu->DebugSendFrame();
			}
		}

		_executionStopped = true;
		while(((stepCount == 0 || _breakRequested) && !_stopFlag && _suspendCount == 0) || _preventResume > 0) {
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(10));
			stepCount = _stepCount.load();
		}
		_executionStopped = false;
		return true;
	}
	return false;
}

void Debugger::PrivateProcessVramReadOperation(MemoryOperationType type, uint16_t addr, uint8_t &value)
{
	int32_t absoluteAddr = _mapper->ToAbsoluteChrAddress(addr);
	_codeDataLogger->SetFlag(absoluteAddr, type == MemoryOperationType::Read ? CdlChrFlags::Read : CdlChrFlags::Drawn);

	if(_hasBreakpoint[BreakpointType::ReadVram]) {
		OperationInfo operationInfo{ addr, value, type };
		ProcessBreakpoints(BreakpointType::ReadVram, operationInfo);
	}

	ProcessPpuOperation(addr, value, MemoryOperationType::Read);
}

void Debugger::PrivateProcessVramWriteOperation(uint16_t addr, uint8_t &value)
{
	if(_hasBreakpoint[BreakpointType::WriteVram]) {
		OperationInfo operationInfo{ addr, value, MemoryOperationType::Write };
		ProcessBreakpoints(BreakpointType::WriteVram, operationInfo);
	}

	ProcessPpuOperation(addr, value, MemoryOperationType::Write);
}

void Debugger::GetApuState(ApuState *state)
{
	//Pause the emulation
	DebugBreakHelper helper(this);

	//Force APU to catch up before we retrieve its state
	APU::StaticRun();

	*state = _apu->GetState();
}

void Debugger::GetState(DebugState *state, bool includeMapperInfo)
{
	state->Model = _console->GetModel();
	_cpu->GetState(state->CPU);
	_ppu->GetState(state->PPU);
	if(includeMapperInfo) {
		state->Cartridge = _mapper->GetState();
		state->APU = _apu->GetState();
	}
}

void Debugger::SetState(DebugState state)
{
	_cpu->SetState(state.CPU);
	_ppu->SetState(state.PPU);
	if(state.CPU.PC != _cpu->GetPC()) {
		SetNextStatement(state.CPU.PC);
	}
}

void Debugger::Break()
{
	_breakRequested = true;
}

void Debugger::ResumeFromBreak()
{
	_breakRequested = false;
}

void Debugger::PpuStep(uint32_t count)
{
	_ppuStepCount = count;
	_stepOverAddr = -1;
	_stepCycleCount = -1;
	_stepCount = -1;
	_breakOnScanline = -2;
}

void Debugger::Step(uint32_t count)
{
	//Run CPU for [count] INSTRUCTIONS before breaking again
	_stepOut = false;
	_stepOverAddr = -1;
	_stepCycleCount = -1;
	_ppuStepCount = -1;
	_stepCount = count;
	_breakOnScanline = -2;
}

void Debugger::StepCycles(uint32_t count)
{
	//Run CPU for [count] CYCLES before breaking again
	PpuStep((uint32_t)(count * (_console->GetModel() == NesModel::NTSC ? 3 : 3.2)));
}

void Debugger::StepOut()
{
	_stepOut = true;
	_stepOverAddr = -1;
	_stepCycleCount = -1;
	_stepCount = -1;
	_breakOnScanline = -2;
}

void Debugger::StepOver()
{
	if(_lastInstruction == 0x20 || _lastInstruction == 0x00) {
		//We are on a JSR/BRK instruction, need to continue until the following instruction
		_stepOverAddr = _cpu->GetPC() + (_lastInstruction == 0x20 ? 3 : 1);
		Run();
	} else {
		//Except for JSR & BRK, StepOver behaves the same as StepTnto
		Step(1);
	}
}

void Debugger::StepBack()
{
	if(_runToCycle == 0) {
		_runToCycle = _prevInstructionCycle;
		_needRewind = true;
		Run();
	}
}

void Debugger::Run()
{
	//Resume execution after a breakpoint has been hit
	_ppuStepCount = -1;
	_stepCount = -1;
	_breakOnScanline = -2;
}

void Debugger::BreakOnScanline(int16_t scanline)
{
	Run();
	_breakOnScanline = scanline;
}

void Debugger::GenerateCodeOutput()
{
	State cpuState;
	_cpu->GetState(cpuState);

	_disassemblerOutput.clear();
	_disassemblerOutput.reserve(10000);

	for(uint32_t i = 0; i < 0x10000; i += 0x100) {
		//Merge all sequential ranges into 1 chunk
		AddressTypeInfo startInfo, currentInfo, endInfo;
		GetAbsoluteAddressAndType(i, &startInfo);
		currentInfo = startInfo;
		GetAbsoluteAddressAndType(i+0x100, &endInfo);

		uint32_t startMemoryAddr = i;
		int32_t startAddr, endAddr;

		if(startInfo.Address >= 0) {
			startAddr = startInfo.Address;
			endAddr = startAddr + 0xFF;
			while(currentInfo.Type == endInfo.Type && currentInfo.Address + 0x100 == endInfo.Address && i < 0x10000) {
				endAddr += 0x100;
				currentInfo = endInfo;
				i+=0x100;
				GetAbsoluteAddressAndType(i + 0x100, &endInfo);
			}
			_disassemblerOutput += _disassembler->GetCode(startInfo, endAddr, startMemoryAddr, cpuState, _memoryManager, _labelManager);
		}
	}
}

const char* Debugger::GetCode(uint32_t &length)
{
	string previousCode = _disassemblerOutput;
	GenerateCodeOutput();
	bool forceRefresh = length == UINT32_MAX;
	length = (uint32_t)_disassemblerOutput.size();
	if(!forceRefresh && previousCode.compare(_disassemblerOutput) == 0) {
		//Return null pointer if the code is identical to last call
		//This avois the UTF8->UTF16 conversion that the UI needs to do
		//before comparing the strings
		return nullptr;
	} else {
		return _disassemblerOutput.c_str();
	}
}

int32_t Debugger::GetRelativeAddress(uint32_t addr, AddressType type)
{
	switch(type) {
		case AddressType::InternalRam: 
		case AddressType::Register:
			return addr;
		
		case AddressType::PrgRom: 
		case AddressType::WorkRam: 
		case AddressType::SaveRam: 
			return _mapper->FromAbsoluteAddress(addr, type);
	}

	return -1;
}

int32_t Debugger::GetAbsoluteAddress(uint32_t addr)
{
	return _mapper->ToAbsoluteAddress(addr);
}

int32_t Debugger::GetAbsoluteChrAddress(uint32_t addr)
{
	return _mapper->ToAbsoluteChrAddress(addr);
}

int32_t Debugger::GetRelativeChrAddress(uint32_t absoluteAddr)
{
	return _mapper->FromAbsoluteChrAddress(absoluteAddr);
}

void Debugger::SetNextStatement(uint16_t addr)
{
	if(_currentReadAddr) {
		_cpu->SetDebugPC(addr);
		*_currentReadAddr = addr;
		*_currentReadValue = _memoryManager->DebugRead(addr, true);
	} else {
		//Can't change the address right away (CPU is in the middle of an instruction)
		//Address will change after current instruction is done executing
		_nextReadAddr = addr;
	}
}

void Debugger::ProcessPpuCycle()
{
#ifndef LIBRETRO
	if(Debugger::Instance) {
		Debugger::Instance->PrivateProcessPpuCycle();
	}
#endif
}

bool Debugger::ProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t &value)
{
#ifndef LIBRETRO
	if(Debugger::Instance) {
		return Debugger::Instance->PrivateProcessRamOperation(type, addr, value);
	}
	return true;
#else
	return true;
#endif
}

void Debugger::ProcessVramReadOperation(MemoryOperationType type, uint16_t addr, uint8_t &value)
{
#ifndef LIBRETRO
	if(Debugger::Instance) {
		Debugger::Instance->PrivateProcessVramReadOperation(type, addr, value);
	}
#endif
}

void Debugger::ProcessVramWriteOperation(uint16_t addr, uint8_t &value)
{
#ifndef LIBRETRO
	if(Debugger::Instance) {
		Debugger::Instance->PrivateProcessVramWriteOperation(addr, value);
	}
#endif
}

void Debugger::GetCallstack(int32_t* callstackAbsolute, int32_t* callstackRelative)
{
	int callstackSize = std::min(1022, (int)_callstackRelative.size() - (_hideTopOfCallstack ? 2 : 0));
	for(int i = 0; i < callstackSize; i++) {
		callstackAbsolute[i] = _callstackAbsolute[i];
		
		int32_t relativeAddr = _callstackRelative[i];
		if(_mapper->FromAbsoluteAddress(_callstackAbsolute[i]) == -1) {
			//Mark address as an unmapped memory addr
			relativeAddr |= 0x10000;
		}
		callstackRelative[i] = relativeAddr;
	}
	callstackAbsolute[callstackSize] = -2;
	callstackRelative[callstackSize] = -2;
}

int32_t Debugger::GetFunctionEntryPointCount()
{
	return (int32_t)_functionEntryPoints.size();
}

void Debugger::GetFunctionEntryPoints(int32_t* entryPoints, int32_t maxCount)
{
	int32_t i = 0;
	for(auto itt = _functionEntryPoints.begin(); itt != _functionEntryPoints.end(); itt++) {
		entryPoints[i] = *itt;
		i++;
		if(i == maxCount - 1) {
			break;
		}
	}
	entryPoints[i] = -1;
}

shared_ptr<Assembler> Debugger::GetAssembler()
{
	return _assembler;
}

shared_ptr<TraceLogger> Debugger::GetTraceLogger()
{
	return _traceLogger;
}

shared_ptr<MemoryDumper> Debugger::GetMemoryDumper()
{
	return _memoryDumper;
}

shared_ptr<MemoryAccessCounter> Debugger::GetMemoryAccessCounter()
{
	return _memoryAccessCounter;
}

bool Debugger::IsExecutionStopped()
{
	return _executionStopped || _console->IsPaused();
}

void Debugger::PreventResume()
{
	_preventResume++;
}

void Debugger::AllowResume()
{
	_preventResume--;
}

void Debugger::GetAbsoluteAddressAndType(uint32_t relativeAddr, AddressTypeInfo* info)
{
	if(relativeAddr < 0x2000) {
		info->Address = relativeAddr;
		info->Type = AddressType::InternalRam;
		return;
	}

	int32_t addr = _mapper->ToAbsoluteAddress(relativeAddr);
	if(addr >= 0) {
		info->Address = addr;
		info->Type = AddressType::PrgRom;
		return;
	}
	
	addr = _mapper->ToAbsoluteWorkRamAddress(relativeAddr);
	if(addr >= 0) {
		info->Address = addr;
		info->Type = AddressType::WorkRam;
		return;
	}

	addr = _mapper->ToAbsoluteSaveRamAddress(relativeAddr);
	if(addr >= 0) {
		info->Address = addr;
		info->Type = AddressType::SaveRam;
		return;
	}

	info->Address = -1;
	info->Type = AddressType::InternalRam;
}

void Debugger::GetPpuAbsoluteAddressAndType(uint32_t relativeAddr, PpuAddressTypeInfo* info)
{
	if(relativeAddr >= 0x3F00) {
		info->Address = relativeAddr & 0x1F;
		info->Type = PpuAddressType::PaletteRam;
		return;
	}

	int32_t addr = _mapper->ToAbsoluteChrRomAddress(relativeAddr);
	if(addr >= 0) {
		info->Address = addr;
		info->Type = PpuAddressType::ChrRom;
		return;
	}

	addr = _mapper->ToAbsoluteChrRamAddress(relativeAddr);
	if(addr >= 0) {
		info->Address = addr;
		info->Type = PpuAddressType::ChrRam;
		return;
	}

	info->Address = -1;
	info->Type = PpuAddressType::None;
}

void Debugger::SetPpuViewerScanlineCycle(int32_t scanline, int32_t cycle)
{
	_ppuViewerScanline = scanline;
	_ppuViewerCycle = cycle;
}

void Debugger::SetLastFramePpuScroll(uint16_t addr, uint8_t xScroll, bool updateHorizontalScrollOnly)
{
	if(Debugger::Instance) {
		Debugger::Instance->_ppuScrollX = ((addr & 0x1F) << 3) | xScroll | ((addr & 0x400) ? 0x100 : 0);
		if(!updateHorizontalScrollOnly) {
			Debugger::Instance->_ppuScrollY = (((addr & 0x3E0) >> 2) | ((addr & 0x7000) >> 12)) + ((addr & 0x800) ? 240 : 0);
		}
	}
}

uint32_t Debugger::GetPpuScroll()
{
	return (_ppuScrollY << 16) | _ppuScrollX;
}

void Debugger::SetFreezeState(uint16_t address, bool frozen)
{
	_frozenAddresses[address] = frozen ? 1 : 0;
}

void Debugger::GetFreezeState(uint16_t startAddress, uint16_t length, bool* freezeState)
{
	for(uint16_t i = 0; i < length; i++) {
		freezeState[i] = _frozenAddresses[startAddress + i] ? true : false;
	}	
}

void Debugger::StartCodeRunner(uint8_t *byteCode, uint32_t codeLength)
{
	_codeRunner.reset(new CodeRunner(vector<uint8_t>(byteCode, byteCode + codeLength), this));
	_memoryManager->RegisterIODevice(_codeRunner.get());
	_returnToAddress = _cpu->GetDebugPC();
	SetNextStatement(CodeRunner::BaseAddress);
}

void Debugger::StopCodeRunner()
{
	_memoryManager->UnregisterIODevice(_codeRunner.get());
	_memoryManager->RegisterIODevice(_ppu.get());
	
	//Break debugger when code has finished executing
	SetNextStatement(_returnToAddress);

	if(EmulationSettings::CheckFlag(EmulationFlags::DebuggerWindowEnabled)) {
		Step(1);
	} else {
		Run();
	}
}

void Debugger::GetNesHeader(uint8_t* header)
{
	NESHeader nesHeader = _mapper->GetNesHeader();
	memcpy(header, &nesHeader, sizeof(NESHeader));
}

void Debugger::SaveRomToDisk(string filename, bool saveAsIps, uint8_t* header, CdlStripFlag cdlStripflag)
{
	vector<uint8_t> fileData;
	_mapper->GetRomFileData(fileData, saveAsIps, header);

	_codeDataLogger->StripData(fileData.data() + sizeof(NESHeader), cdlStripflag);

	ofstream file(filename, ios::out | ios::binary);
	if(file.good()) {
		file.write((char*)fileData.data(), fileData.size());
		file.close();
	}
}

void Debugger::RevertPrgChrChanges()
{
	DebugBreakHelper helper(this);
	_mapper->RevertPrgChrChanges();
	_disassembler->Reset();
	UpdateCdlCache();
}

bool Debugger::HasPrgChrChanges()
{
	return _mapper->HasPrgChrChanges();
}

int32_t Debugger::FindSubEntryPoint(uint16_t relativeAddress)
{
	AddressTypeInfo info;
	int32_t address = relativeAddress;
	do {
		GetAbsoluteAddressAndType(address, &info);
		if(info.Address < 0 || info.Type != AddressType::PrgRom || _codeDataLogger->IsData(info.Address)) {
			break;
		}
		address--;
		if(_codeDataLogger->IsSubEntryPoint(info.Address)) {
			break;
		}
	} while(address >= 0);

	return address > relativeAddress ? relativeAddress : (address + 1);
}

void Debugger::SetInputOverride(uint8_t port, uint32_t state)
{
	_inputOverride[port] = state;
}

int Debugger::LoadScript(string name, string content, int32_t scriptId)
{
	DebugBreakHelper helper(this);
	auto lock = _scriptLock.AcquireSafe();
	
	if(scriptId < 0) {
		shared_ptr<ScriptHost> script(new ScriptHost(_nextScriptId++));
		script->LoadScript(name, content, this);
		_scripts.push_back(script);
		_hasScript = true;
		return script->GetScriptId();
	} else {
		auto result = std::find_if(_scripts.begin(), _scripts.end(), [=](shared_ptr<ScriptHost> &script) {
			return script->GetScriptId() == scriptId;
		});
		if(result != _scripts.end()) {
			(*result)->LoadScript(name, content, this);
			return scriptId;
		}
	}

	return -1;
}

void Debugger::RemoveScript(int32_t scriptId)
{
	DebugBreakHelper helper(this);
	auto lock = _scriptLock.AcquireSafe();
	_scripts.erase(std::remove_if(_scripts.begin(), _scripts.end(), [=](const shared_ptr<ScriptHost>& script) { return script->GetScriptId() == scriptId; }), _scripts.end());
	_hasScript = _scripts.size() > 0;
}

const char* Debugger::GetScriptLog(int32_t scriptId)
{
	auto lock = _scriptLock.AcquireSafe();
	for(shared_ptr<ScriptHost> &script : _scripts) {
		if(script->GetScriptId() == scriptId) {
			return script->GetLog();
		}
	}
	return "";
}

void Debugger::ResetCounters()
{
	_memoryAccessCounter->ResetCounts();
	_profiler->Reset();
}

void Debugger::UpdateProgramCounter(uint16_t &addr, uint8_t &value)
{
	addr = _cpu->GetPC();
	value = _memoryManager->DebugRead(addr, true);
	_cpu->SetDebugPC(addr);
}

void Debugger::ProcessScriptSaveState(uint16_t &addr, uint8_t &value)
{
	if(_hasScript) {
		for(shared_ptr<ScriptHost> &script : _scripts) {
			if(script->ProcessSavestate()) {
				//Adjust PC and current addr/value if a state was loaded due to a call to loadSavestateAsync
				UpdateProgramCounter(addr, value);
			}
		}
	}
}

void Debugger::ProcessCpuOperation(uint16_t &addr, uint8_t &value, MemoryOperationType type)
{
	if(_hasScript) {
		for(shared_ptr<ScriptHost> &script : _scripts) {
			script->ProcessCpuOperation(addr, value, type);
			if(type == MemoryOperationType::ExecOpCode && script->CheckStateLoadedFlag()) {
				//Adjust PC and current addr/value when a state was loaded during a CpuExec callback
				UpdateProgramCounter(addr, value);
			}
		}
	}
}

void Debugger::ProcessPpuOperation(uint16_t addr, uint8_t &value, MemoryOperationType type)
{
	if(_hasScript) {
		for(shared_ptr<ScriptHost> &script : _scripts) {
			script->ProcessPpuOperation(addr, value, type);
		}
	}
}

void Debugger::ProcessEvent(EventType type)
{
	if(_hasScript) {
		for(shared_ptr<ScriptHost> &script : _scripts) {
			script->ProcessEvent(type);
		}
	}

	if(type == EventType::InputPolled) {
		for(int i = 0; i < 4; i++) {
			if(_inputOverride[i] != 0) {
				shared_ptr<StandardController> controller = std::dynamic_pointer_cast<StandardController>(ControlManager::GetControlDevice(i));
				if(controller) {
					controller->SetBitValue(StandardController::Buttons::A, (_inputOverride[i] & 0x01) != 0);
					controller->SetBitValue(StandardController::Buttons::B, (_inputOverride[i] & 0x02) != 0);
					controller->SetBitValue(StandardController::Buttons::Select, (_inputOverride[i] & 0x04) != 0);
					controller->SetBitValue(StandardController::Buttons::Start, (_inputOverride[i] & 0x08) != 0);
					controller->SetBitValue(StandardController::Buttons::Up, (_inputOverride[i] & 0x10) != 0);
					controller->SetBitValue(StandardController::Buttons::Down, (_inputOverride[i] & 0x20) != 0);
					controller->SetBitValue(StandardController::Buttons::Left, (_inputOverride[i] & 0x40) != 0);
					controller->SetBitValue(StandardController::Buttons::Right, (_inputOverride[i] & 0x80) != 0);
				}
			}
		}
	} else if(type == EventType::EndFrame) {
		_memoryDumper->GatherChrPaletteInfo();
	} else if(type == EventType::StartFrame) {
		//Update the event viewer
		MessageManager::SendNotification(ConsoleNotificationType::EventViewerDisplayFrame);

		//Clear the current frame/event log
		if(CheckFlag(DebuggerFlags::PpuPartialDraw)) {
			_ppu->DebugUpdateFrameBuffer(CheckFlag(DebuggerFlags::PpuShowPreviousFrame));
		}
		_debugEvents.clear();
	} else if(type == EventType::Nmi) {
		AddDebugEvent(DebugEventType::Nmi);
	} else if(type == EventType::Irq) {
		AddDebugEvent(DebugEventType::Irq);
	} else if(type == EventType::SpriteZeroHit) {
		AddDebugEvent(DebugEventType::SpriteZeroHit);
	}
}

void Debugger::AddDebugEvent(DebugEventType type, uint16_t address, uint8_t value, int16_t breakpointId, int8_t ppuLatch)
{
	_debugEvents.push_back({
		(uint16_t)_ppu->GetCurrentCycle(),
		(int16_t)_ppu->GetCurrentScanline(),
		_cpu->GetDebugPC(),
		address,
		breakpointId,
		type,
		value,
		ppuLatch,
	});
}

void Debugger::GetDebugEvents(uint32_t* pictureBuffer, DebugEventInfo *infoArray, uint32_t &maxEventCount)
{
	DebugBreakHelper helper(this);

	uint16_t *buffer = new uint16_t[PPU::PixelCount];
	uint32_t *palette = EmulationSettings::GetRgbPalette();
	_ppu->DebugCopyOutputBuffer(buffer);

	for(int i = 0; i < PPU::PixelCount; i++) {
		pictureBuffer[i] = palette[buffer[i] & 0x3F];
	}
	uint32_t eventCount = std::min(maxEventCount, (uint32_t)_debugEvents.size());
	memcpy(infoArray, _debugEvents.data(), eventCount * sizeof(DebugEventInfo));
	maxEventCount = eventCount;
}

uint32_t Debugger::GetDebugEventCount()
{
	return (uint32_t)_debugEvents.size();
}
