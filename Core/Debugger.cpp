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
#include "StandardController.h"
#include "TraceLogger.h"
#include "Breakpoint.h"
#include "CodeDataLogger.h"
#include "NotificationManager.h"
#include "DebugHud.h"
#include "DummyCpu.h"

const int Debugger::BreakpointTypeCount;
string Debugger::_disassemblerOutput = "";

Debugger::Debugger(shared_ptr<Console> console, shared_ptr<CPU> cpu, shared_ptr<PPU> ppu, shared_ptr<APU> apu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper)
{
	_romName = console->GetRomInfo().RomName;
	_console = console;
	_cpu = cpu;
	_apu = apu;
	_memoryManager = memoryManager;
	_mapper = mapper;

	_dummyCpu.reset(new DummyCpu(console));
	_breakOnFirstCycle = false;

	_labelManager.reset(new LabelManager(_mapper));
	_assembler.reset(new Assembler(_labelManager));
	_disassembler.reset(new Disassembler(memoryManager.get(), mapper.get(), this));
	_codeDataLogger.reset(new CodeDataLogger(this, mapper->GetMemorySize(DebugMemoryType::PrgRom), mapper->GetMemorySize(DebugMemoryType::ChrRom)));

	SetPpu(ppu);

	_memoryAccessCounter.reset(new MemoryAccessCounter(this));
	_profiler.reset(new Profiler(this));
	_traceLogger.reset(new TraceLogger(this, memoryManager, _labelManager));

	_bpExpEval.reset(new ExpressionEvaluator(this));
	_watchExpEval.reset(new ExpressionEvaluator(this));

	_stepOut = false;
	_stepCount = -1;
	_stepOverAddr = -1;
	_stepCycleCount = -1;
	_ppuStepCount = -1;
	_breakRequested = false;
	_pausedForDebugHelper = false;
	_breakOnScanline = -2;
	_breakSource = BreakSource::Unspecified;

	_preventResume = 0;
	_stopFlag = false;
	_suspendCount = 0;

	_opCodeCycle = 0;
	_lastInstruction = 0;

	_stepOutReturnAddress = -1;

	_currentReadAddr = nullptr;
	_currentReadValue = nullptr;
	_nextReadAddr = -1;
	_returnToAddress = 0;

	_ppuScrollX = 0;
	_ppuScrollY = 0;

	_flags = 0;

	_runToCycle = 0;
	_prevInstructionCycle = 0;
	_curInstructionCycle = 0;
	_needRewind = false;

	//Only enable break on uninitialized reads when debugger is opened at power on/reset
	_enableBreakOnUninitRead = _cpu->GetPC() == 0;

	_executionStopped = false;
	
	_disassemblerOutput = "";
	
	memset(_inputOverride, 0, sizeof(_inputOverride));

	_frozenAddresses.insert(_frozenAddresses.end(), 0x10000, 0);

	if(!LoadCdlFile(FolderUtilities::CombinePath(FolderUtilities::GetDebuggerFolder(), FolderUtilities::GetFilename(_romName, false) + ".cdl"))) {
		_disassembler->Reset();
	}

	_hasScript = false;
	_nextScriptId = 0;

	_released = false;

	UpdatePpuCyclesToProcess();
}

Debugger::~Debugger()
{
	if(!_released) {
		ReleaseDebugger();
	}
}

void Debugger::ReleaseDebugger()
{
	auto lock = _releaseLock.AcquireSafe();
	if(!_released) {
		_codeDataLogger->SaveCdlFile(FolderUtilities::CombinePath(FolderUtilities::GetDebuggerFolder(), FolderUtilities::GetFilename(_romName, false) + ".cdl"));

		_stopFlag = true;

		_console->Pause();

		{
			auto lock = _scriptLock.AcquireSafe();
			for(shared_ptr<ScriptHost> script : _scripts) {
				//Send a ScriptEnded event to all active scripts
				script->ProcessEvent(EventType::ScriptEnded);
			}
			_scripts.clear();
			_hasScript = false;
		}

		_breakLock.Acquire();
		_breakLock.Release();
		_console->Resume();

		_released = true;
	}
}

void Debugger::SetPpu(shared_ptr<PPU> ppu)
{
	_ppu = ppu;
	_memoryDumper.reset(new MemoryDumper(_ppu, _memoryManager, _mapper, _codeDataLogger, this, _disassembler));
}

Console* Debugger::GetConsole()
{
	return _console.get();
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
	_breakOnFirstCycle = CheckFlag(DebuggerFlags::BreakOnFirstCycle);
	if(needUpdate) {
		_disassembler->BuildOpCodeTables(CheckFlag(DebuggerFlags::DisplayOpCodesInLowerCase));
	}
}

bool Debugger::CheckFlag(DebuggerFlags flag)
{
	return (_flags & (uint32_t)flag) == (uint32_t)flag;
}

bool Debugger::LoadCdlFile(string cdlFilepath)
{
	if(_codeDataLogger->LoadCdlFile(cdlFilepath)) {
		//Can't use DebugBreakHelper due to the fact this is called in the constructor
		bool isEmulationThread = _console->GetEmulationThreadId() == std::this_thread::get_id();
		if(!isEmulationThread) {
			_console->Pause();
		}
		UpdateCdlCache();
		if(!isEmulationThread) {
			_console->Resume();
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
	DebugBreakHelper helper(this);

	_disassembler->Reset();
	for(int i = 0, len = _mapper->GetMemorySize(DebugMemoryType::PrgRom); i < len; i++) {
		if(_codeDataLogger->IsCode(i)) {
			AddressTypeInfo info = { i, AddressType::PrgRom };
			i = _disassembler->BuildCache(info, 0, false, false) - 1;
		}
	}

	_functionEntryPoints.clear();
	for(int i = 0, len = _mapper->GetMemorySize(DebugMemoryType::PrgRom); i < len; i++) {
		if(_codeDataLogger->IsSubEntryPoint(i)) {
			_functionEntryPoints.emplace(i);

			//After resetting the cache, set the entry point flags in the disassembly cache
			AddressTypeInfo info = { i, AddressType::PrgRom };
			_disassembler->BuildCache(info, 0, true, false);
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

	_bpDummyCpuRequired = false;

	_bpExpEval.reset(new ExpressionEvaluator(this));
	for(uint32_t j = 0; j < length; j++) {
		Breakpoint &bp = breakpoints[j];
		for(int i = 0; i < Debugger::BreakpointTypeCount; i++) {
			bool isEnabled = bp.IsEnabled() && _console->GetSettings()->CheckFlag(EmulationFlags::DebuggerWindowEnabled);
			if((bp.IsMarked() || isEnabled) && bp.HasBreakpointType((BreakpointType)i)) {
				_breakpoints[i].push_back(bp);

				bool success = true;
				if(bp.HasCondition()) {
					ExpressionData data = _bpExpEval->GetRpnList(bp.GetCondition(), success);
					_breakpointRpnList[i].push_back(success ? data : ExpressionData());
				}

				if(bp.IsEnabled()) {
					bool isReadWriteBp = i == BreakpointType::ReadVram || i == BreakpointType::ReadRam || i == BreakpointType::WriteVram || i == BreakpointType::WriteRam || i == BreakpointType::DummyReadRam || i == BreakpointType::DummyWriteRam;
					_bpDummyCpuRequired |= isReadWriteBp;
				}

				_hasBreakpoint[i] = true;
			}
		}
	}
}

bool Debugger::ProcessBreakpoints(BreakpointType type, OperationInfo &operationInfo, bool allowBreak, bool allowMark)
{
	if(_runToCycle != 0) {
		//Disable all breakpoints while stepping backwards
		return false;
	}

	AddressTypeInfo info { -1, AddressType::InternalRam };
	PpuAddressTypeInfo ppuInfo { -1, PpuAddressType::None };
	bool isPpuBreakpoint = false;
	switch(type) {
		case BreakpointType::Global:
			break;

		case BreakpointType::Execute:
		case BreakpointType::ReadRam:
		case BreakpointType::WriteRam:
		case BreakpointType::DummyReadRam:
		case BreakpointType::DummyWriteRam:
			GetAbsoluteAddressAndType(operationInfo.Address, &info);
			break;

		case BreakpointType::ReadVram:
		case BreakpointType::WriteVram:
			GetPpuAbsoluteAddressAndType(operationInfo.Address, &ppuInfo);
			isPpuBreakpoint = true;
			break;
	}

	vector<Breakpoint> &breakpoints = _breakpoints[(int)type];

	bool needBreak = false;
	bool needMark = false;
	bool needState = true;
	uint32_t markBreakpointId = 0;
	uint32_t breakpointId = 0;
	EvalResultType resultType;
	
	auto processBreakpoint = [&needMark, &needBreak, &markBreakpointId, &breakpointId](Breakpoint &bp) {
		if(bp.IsMarked()) {
			needMark = true;
			markBreakpointId = bp.GetId();
		}
		if(bp.IsEnabled()) {
			needBreak = true;
			breakpointId = bp.GetId();
		}
	};

	for(size_t i = 0, len = breakpoints.size(); i < len; i++) {
		Breakpoint &breakpoint = breakpoints[i];
		if(!((breakpoint.IsEnabled() && allowBreak) || (breakpoint.IsMarked() && allowMark))) {
			//Skip breakpoints we don't need to process
			continue;
		}

		if(
			type == BreakpointType::Global ||
			(!isPpuBreakpoint && breakpoint.Matches(operationInfo.Address, info)) ||
			(isPpuBreakpoint && breakpoint.Matches(operationInfo.Address, ppuInfo))
		) {
			if(!breakpoint.HasCondition()) {
				processBreakpoint(breakpoint);
			} else {
				if(needState) {
					GetState(&_debugState, false);
					needState = false;
				}
				if(_bpExpEval->Evaluate(_breakpointRpnList[(int)type][i], _debugState, resultType, operationInfo) != 0) {
					processBreakpoint(breakpoint);
				}
			}
		}

		if((needMark || !allowMark) && (needBreak || !allowBreak)) {
			//No need to process remaining breakpoints
			break;
		}
	}

	if(needMark && allowMark) {
		AddDebugEvent(DebugEventType::Breakpoint, operationInfo.Address, (uint8_t)operationInfo.Value, markBreakpointId);
	}

	if(needBreak && allowBreak) {
		//Found a matching breakpoint, stop execution
		Step(1);
		SleepUntilResume(BreakSource::Breakpoint, breakpointId, type, operationInfo.Address, operationInfo.OperationType);
		return true;
	} else {
		return false;
	}
}

void Debugger::ProcessAllBreakpoints(OperationInfo &operationInfo, AddressTypeInfo &addressInfo)
{
	if(_hasBreakpoint[BreakpointType::Execute]) {
		ProcessBreakpoints(BreakpointType::Execute, operationInfo, true, true);
	}

	bool checkUninitReads = _enableBreakOnUninitRead && CheckFlag(DebuggerFlags::BreakOnUninitMemoryRead);

	if(!checkUninitReads && !_bpDummyCpuRequired) {
		//Nothing to do, no read/write breakpoints are active and don't need to check uninit reads
		return;
	}

	_dummyCpu->SetDummyState(_cpu.get());
	_dummyCpu->Exec();

	DebugState &state = _debugState;
	uint32_t readCount = _dummyCpu->GetReadCount();
	if(readCount > 0) {
		uint16_t addr;
		uint8_t value;
		bool isDummyRead;
		for(uint32_t i = 0; i < readCount; i++) {
			_dummyCpu->GetReadAddr(i, addr, value, isDummyRead);

			OperationInfo info;

			if(addr >= 0x2000 && addr < 0x4000 && (addr & 0x07) == 0x07) {
				//Reads to $2007 will trigger a PPU read
				if(_hasBreakpoint[BreakpointType::ReadVram]) {
					OperationInfo ppuInfo;
					ppuInfo.OperationType = MemoryOperationType::Read;
					if((state.PPU.State.VideoRamAddr & 0x3FFF) >= 0x3F00) {
						ppuInfo.Address = state.PPU.State.VideoRamAddr;
						ppuInfo.Value = _ppu->ReadPaletteRAM(ppuInfo.Address);
					} else {
						ppuInfo.Address = state.PPU.BusAddress;
						ppuInfo.Value = _mapper->DebugReadVRAM(ppuInfo.Address);
					}
					if(ProcessBreakpoints(BreakpointType::ReadVram, ppuInfo, true, false)) {
						return;
					}
				}

				info.Value = state.PPU.MemoryReadBuffer;
			} else {
				if(!isDummyRead && checkUninitReads) {
					//Break on uninit memory read
					if(_memoryAccessCounter->IsAddressUninitialized(addressInfo)) {
						Step(1);
						SleepUntilResume(BreakSource::BreakOnUninitMemoryRead, 0, BreakpointType::ReadRam, addr);
						return;
					}
				}

				info.Value = value;
			}

			info.Address = addr;
			if(isDummyRead) {
				if(_hasBreakpoint[BreakpointType::DummyReadRam]) {
					info.OperationType = MemoryOperationType::DummyRead;
					if(ProcessBreakpoints(BreakpointType::DummyReadRam, info, true, false)) {
						return;
					}
				}
			} else {
				if(_hasBreakpoint[BreakpointType::ReadRam]) {
					info.OperationType = MemoryOperationType::Read;
					if(ProcessBreakpoints(BreakpointType::ReadRam, info, true, false)) {
						return;
					}
				}
			}
		}
	}
	
	uint32_t writeCount = _dummyCpu->GetWriteCount();
	if(writeCount > 0) {
		uint16_t addr;
		uint8_t value;
		bool isDummyWrite;
		for(uint32_t i = 0; i < writeCount; i++) {
			_dummyCpu->GetWriteAddrValue(i, addr, value, isDummyWrite);

			OperationInfo info;
			info.Address = addr;
			info.Value = value;
			if(isDummyWrite) {
				if(_hasBreakpoint[BreakpointType::DummyWriteRam]) {
					info.OperationType = MemoryOperationType::DummyWrite;
					if(ProcessBreakpoints(BreakpointType::DummyWriteRam, info, true, false)) {
						return;
					}
				}
			} else {
				if(_hasBreakpoint[BreakpointType::WriteRam]) {
					info.OperationType = MemoryOperationType::Write;
					if(ProcessBreakpoints(BreakpointType::WriteRam, info, true, false)) {
						return;
					}
				}
			}

			if(_hasBreakpoint[BreakpointType::WriteVram]) {
				if(addr >= 0x2000 && addr < 0x4000 && (addr & 0x07) == 0x07) {
					//Write to $2007 will trigger a PPU write
					OperationInfo ppuInfo;
					ppuInfo.Address = state.PPU.BusAddress;
					ppuInfo.Value = value;
					ppuInfo.OperationType = MemoryOperationType::Write;
					if(ProcessBreakpoints(BreakpointType::WriteVram, ppuInfo, true, false)) {
						return;
					}
				}
			}
		}
	}
}

int32_t Debugger::EvaluateExpression(string expression, EvalResultType &resultType, bool useCache)
{
	DebugState state;
	OperationInfo operationInfo { 0, 0, MemoryOperationType::DummyRead };
	GetState(&state);
	if(useCache) {
		return _watchExpEval->Evaluate(expression, state, resultType, operationInfo);
	} else {
		ExpressionEvaluator expEval(this);
		return expEval.Evaluate(expression, state, resultType, operationInfo);
	}
}

void Debugger::UpdateCallstack(uint8_t instruction, uint32_t addr)
{
	if((instruction == 0x60 || instruction == 0x40) && !_callstack.empty()) {
		//RTS & RTI		
		uint16_t expectedReturnAddress = _callstack[_callstack.size() - 1].JumpSource;

		_callstack.pop_back();
		_subReturnAddresses.pop_back();

		int spOffset = instruction == 0x40 ? 2 : 1; //RTI has an extra byte on the stack (flags)

		uint16_t targetAddr = _memoryManager->DebugReadWord(0x100 + ((_debugState.CPU.SP + spOffset) & 0xFF));
		if(targetAddr < expectedReturnAddress || targetAddr - expectedReturnAddress > 3) {
			//Mismatch, pop that stack frame and add the new one
			if(!_callstack.empty()) {
				bool foundMatch = false;
				for(int i = (int)_callstack.size() - 1; i >= 0; i--) {
					if(targetAddr > _callstack[i].JumpSource && targetAddr < _callstack[i].JumpSource + 3) {
						//Found a matching stack frame, unstack until that point
						foundMatch = true;
						for(int j = (int)_callstack.size() - i - 1; j >= 0; j--) {
							_callstack.pop_back();
							_subReturnAddresses.pop_back();
						}
						break;
					}
				}
				if(!foundMatch) {
					//Couldn't find a matching frame, replace the current one
					AddCallstackFrame(expectedReturnAddress, targetAddr, StackFrameFlags::None);
					_subReturnAddresses.push_back(expectedReturnAddress + 3);
				}
			}
		}

		_profiler->UnstackFunction();
	} else if(instruction == 0x20) {
		//JSR
		uint16_t targetAddr = _memoryManager->DebugRead(addr + 1) | (_memoryManager->DebugRead(addr + 2) << 8);
		AddCallstackFrame(addr, targetAddr, StackFrameFlags::None);
		_subReturnAddresses.push_back(addr + 3);
		
		_profiler->StackFunction(_mapper->ToAbsoluteAddress(addr), _mapper->ToAbsoluteAddress(targetAddr));
	}
}

void Debugger::AddCallstackFrame(uint16_t source, uint16_t target, StackFrameFlags flags)
{
	if(_callstack.size() >= 511) {
		//Ensure callstack stays below 512 entries - games can use various tricks that could keep making the callstack grow
		_callstack.pop_front();
		_subReturnAddresses.pop_front();
	}

	StackFrameInfo stackFrame;
	stackFrame.JumpSource = source;
	stackFrame.JumpSourceAbsolute = _mapper->ToAbsoluteAddress(source);

	stackFrame.JumpTarget = target;
	stackFrame.JumpTargetAbsolute = _mapper->ToAbsoluteAddress(target);

	stackFrame.Flags = flags;

	_callstack.push_back(stackFrame);
}

void Debugger::ProcessInterrupt(uint16_t cpuAddr, uint16_t destCpuAddr, bool forNmi)
{
	AddCallstackFrame(cpuAddr, destCpuAddr, forNmi ? StackFrameFlags::Nmi : StackFrameFlags::Irq);
	_subReturnAddresses.push_back(cpuAddr);

	_profiler->StackFunction(-1, _mapper->ToAbsoluteAddress(destCpuAddr));

	ProcessEvent(forNmi ? EventType::Nmi : EventType::Irq);
}

void Debugger::ProcessStepConditions(uint16_t addr)
{
	if(_stepOut && (_lastInstruction == 0x60 || _lastInstruction == 0x40) && _stepOutReturnAddress == addr) {
		//RTS/RTI found, if we're on the expected return address, break immediately
		Step(1);
	} else if(_stepOverAddr != -1 && addr == (uint32_t)_stepOverAddr) {
		Step(1);
	} else if(_stepCycleCount != -1 && abs(_cpu->GetCycleCount() - _stepCycleCount) < 100 && _cpu->GetCycleCount() >= _stepCycleCount) {
		Step(1);
	}
}

void Debugger::ProcessPpuCycle()
{
	if(_proccessPpuCycle[_ppu->GetCurrentCycle()]) {
		int32_t currentCycle = (_ppu->GetCurrentCycle() << 9) + _ppu->GetCurrentScanline();
		for(auto updateCycle : _ppuViewerUpdateCycle) {
			if(updateCycle.second == currentCycle) {
				_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::PpuViewerDisplayFrame, (void*)(uint64_t)updateCycle.first);
			}
		}

		if(_ppu->GetCurrentCycle() == 0) {
			if(_breakOnScanline == _ppu->GetCurrentScanline()) {
				Step(1);
				SleepUntilResume(BreakSource::Pause);
			}
			if(_ppu->GetCurrentScanline() == 240) {
				ProcessEvent(EventType::EndFrame);
			} else if(_ppu->GetCurrentScanline() == -1) {
				ProcessEvent(EventType::StartFrame);
			}
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
			SleepUntilResume(BreakSource::PpuStep);
		}
	}
}

bool Debugger::ProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t &value)
{
	OperationInfo operationInfo { addr, (int16_t)value, type };

	_memoryOperationType = type;

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
				_console->LoadState(_rewindCache.back());
				_curInstructionCycle = _rewindPrevInstructionCycleCache.back();
				
				_rewindCache.pop_back();
				_rewindPrevInstructionCycleCache.pop_back();
				
				//This state is for the instruction we want to stop on, break here.
				_runToCycle = 0;
				Step(1);
			} else {
				_console->GetRewindManager()->StartRewinding(true);
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

	if(addressInfo.Address >= 0 && type != MemoryOperationType::DummyRead && type != MemoryOperationType::DummyWrite) {
		if(type == MemoryOperationType::Write && CheckFlag(DebuggerFlags::IgnoreRedundantWrites)) {
			if(_memoryManager->DebugRead(addr) != value) {
				_memoryAccessCounter->ProcessMemoryAccess(addressInfo, type, _cpu->GetCycleCount());
			}
		} else {
			if(_memoryAccessCounter->ProcessMemoryAccess(addressInfo, type, _cpu->GetCycleCount())) {
				if(!_breakOnFirstCycle && _enableBreakOnUninitRead && CheckFlag(DebuggerFlags::BreakOnUninitMemoryRead)) {
					//Break on uninit memory read
					Step(1);
					breakDone = SleepUntilResume(BreakSource::BreakOnUninitMemoryRead);
				}
			}
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
		_opCodeCycle = 0;
		_prevInstructionCycle = _curInstructionCycle;
		_curInstructionCycle = _cpu->GetCycleCount();

		bool isSubEntryPoint = _lastInstruction == 0x20; //Previous instruction was a JSR
		if(absoluteAddr >= 0) {
			_codeDataLogger->SetFlag(absoluteAddr, CdlPrgFlags::Code);
			if(isSubEntryPoint) {
				_codeDataLogger->SetFlag(absoluteAddr, CdlPrgFlags::SubEntryPoint);
				_functionEntryPoints.emplace(absoluteAddr);
			}
		}

		_disassembler->BuildCache(addressInfo, addr, isSubEntryPoint, false);

		ProcessStepConditions(addr);

		_profiler->ProcessInstructionStart(absoluteAddr);

		BreakSource breakSource = BreakSource::Unspecified;
		if(value == 0 && CheckFlag(DebuggerFlags::BreakOnBrk)) {
			Step(1);
			breakSource = BreakSource::BreakOnBrk;
		} else if(CheckFlag(DebuggerFlags::BreakOnUnofficialOpCode) && _disassembler->IsUnofficialOpCode(value)) {
			Step(1);
			breakSource = BreakSource::BreakOnUnofficialOpCode;
		}

		if(_runToCycle != 0) {
			if(_cpu->GetCycleCount() >= _runToCycle) {
				//Step back operation is done, revert RewindManager's state & break debugger
				_console->GetRewindManager()->StopRewinding(true);
				_runToCycle = 0;
				Step(1);
			} else if(_runToCycle - _cpu->GetCycleCount() < 500) {
				_rewindCache.push_back(stringstream());
				_console->SaveState(_rewindCache.back());
				_rewindPrevInstructionCycleCache.push_back(_prevInstructionCycle);
			}
		}

		_lastInstruction = value;
		breakDone = SleepUntilResume(breakSource);

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
		_opCodeCycle++;
		_traceLogger->LogNonExec(operationInfo);
		_profiler->ProcessCycle();
	}

	BreakpointType breakpointType;
	switch(type) {
		default: breakpointType = BreakpointType::Execute; break;

		case MemoryOperationType::DummyRead:
		case MemoryOperationType::Read: breakpointType = BreakpointType::ReadRam; break;

		case MemoryOperationType::DummyWrite:
		case MemoryOperationType::Write: breakpointType = BreakpointType::WriteRam; break;
	}

	if(_breakOnFirstCycle) {
		if(type == MemoryOperationType::ExecOpCode && !breakDone) {
			ProcessAllBreakpoints(operationInfo, addressInfo);
		} else if(_hasBreakpoint[breakpointType]) {
			//Process marked breakpoints
			ProcessBreakpoints(breakpointType, operationInfo, false, true);
		}
	} else {
		if(_hasBreakpoint[breakpointType]) {
			ProcessBreakpoints(breakpointType, operationInfo, !breakDone, true);
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
	} else if(type == MemoryOperationType::ExecOpCode) {
		UpdateCallstack(_lastInstruction, addr);
	}

	return true;
}

bool Debugger::SleepUntilResume(BreakSource source, uint32_t breakpointId, BreakpointType bpType, uint16_t bpAddress, MemoryOperationType bpMemOpType)
{
	int32_t stepCount = _stepCount.load();
	if(stepCount > 0) {
		_stepCount--;
		stepCount = _stepCount.load();
	} else if(stepCount == 0) {
		//If stepCount was already 0 when we enter the function, it means
		//Debugger::Suspend() and Debugger::Resume() were called by another thread
		source = BreakSource::BreakAfterSuspend;
	}

	//Read both values here since they might change while executing the code below
	int32_t preventResume = _preventResume;
	bool breakRequested = _breakRequested;

	if((stepCount == 0 || breakRequested) && !_stopFlag && _suspendCount == 0) {
		//Break
		auto lock = _breakLock.AcquireSafe();
				
		if(preventResume == 0) {
			_console->GetSoundMixer()->StopAudio();
			if(source == BreakSource::Unspecified) {
				source = _breakSource;
			}
			_breakSource = BreakSource::Unspecified;

			uint64_t param = (
				((uint64_t)breakpointId << 32) | 
				((uint64_t)(bpAddress & 0xFFFF) << 16) | 
				((uint64_t)((int)bpMemOpType & 0x0F) << 12) |
				((uint64_t)(bpType & 0x0F) << 8) |
				((uint64_t)source & 0xFF)
			);

			_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::CodeBreak, (void*)(uint64_t)param);

			ProcessEvent(EventType::CodeBreak);
			_stepOverAddr = -1;
			if(CheckFlag(DebuggerFlags::PpuPartialDraw)) {
				_ppu->DebugSendFrame();
			}
		}

		_executionStopped = true;
		_pausedForDebugHelper = breakRequested;
		while((((stepCount == 0 || _breakRequested) && _suspendCount == 0) || _preventResume > 0) && !_stopFlag) {
			std::this_thread::sleep_for(std::chrono::duration<int, std::milli>(10));
			if(stepCount == 0) {
				_console->ResetRunTimers();
			}
			stepCount = _stepCount.load();
		}
		_pausedForDebugHelper = false;
		_executionStopped = false;
		return true;
	}
	return false;
}

void Debugger::ProcessVramReadOperation(MemoryOperationType type, uint16_t addr, uint8_t &value)
{
	int32_t absoluteAddr = _mapper->ToAbsoluteChrAddress(addr);
	_codeDataLogger->SetFlag(absoluteAddr, type == MemoryOperationType::Read ? CdlChrFlags::Read : CdlChrFlags::Drawn);

	if(_hasBreakpoint[BreakpointType::ReadVram]) {
		OperationInfo operationInfo{ addr, value, type };
		ProcessBreakpoints(BreakpointType::ReadVram, operationInfo, !_breakOnFirstCycle, true);
	}

	ProcessPpuOperation(addr, value, MemoryOperationType::Read);
}

void Debugger::ProcessVramWriteOperation(uint16_t addr, uint8_t &value)
{
	if(_hasBreakpoint[BreakpointType::WriteVram]) {
		OperationInfo operationInfo{ addr, value, MemoryOperationType::Write };
		ProcessBreakpoints(BreakpointType::WriteVram, operationInfo, !_breakOnFirstCycle, true);
	}

	ProcessPpuOperation(addr, value, MemoryOperationType::Write);
}

void Debugger::GetInstructionProgress(InstructionProgress &state)
{
	state.OpCode = _lastInstruction;
	state.OpCycle = _opCodeCycle;
	state.OpMemoryOperationType = _memoryOperationType;
}

void Debugger::GetApuState(ApuState *state)
{
	//Pause the emulation
	DebugBreakHelper helper(this);

	//Force APU to catch up before we retrieve its state
	_apu->Run();

	*state = _apu->GetState();
}

void Debugger::GetState(DebugState *state, bool includeMapperInfo)
{
	state->Model = _console->GetModel();
	state->ClockRate = _cpu->GetClockRate(_console->GetModel());
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
	_stepOut = false;
}

void Debugger::Step(uint32_t count, BreakSource source)
{
	//Run CPU for [count] INSTRUCTIONS before breaking again
	_stepOut = false;
	_stepOverAddr = -1;
	_stepCycleCount = -1;
	_ppuStepCount = -1;
	_stepCount = count;
	_breakOnScanline = -2;
	_breakSource = source;
}

void Debugger::StepCycles(uint32_t count)
{
	//Run CPU for [count] CYCLES before breaking again
	PpuStep((uint32_t)(count * (_console->GetModel() == NesModel::NTSC ? 3 : 3.2)));
}

void Debugger::StepOut()
{
	if(_subReturnAddresses.empty()) {
		return;
	}

	_stepOut = true;
	_stepOutReturnAddress = _subReturnAddresses.back();
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
	_stepOut = false;
}

void Debugger::BreakImmediately(BreakSource source)
{
	Step(1);
	SleepUntilResume(source);
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
	bool forceRefresh = length == (uint32_t)-1;
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

void Debugger::GetJumpTargets(bool* jumpTargets)
{
	_disassembler->GetJumpTargets(jumpTargets);
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
		*_currentReadValue = _memoryManager->DebugRead(addr, false);
	} else {
		//Can't change the address right away (CPU is in the middle of an instruction)
		//Address will change after current instruction is done executing
		_nextReadAddr = addr;

		//Force the current instruction to finish
		Step(1);
	}
}

void Debugger::GetCallstack(StackFrameInfo* callstackArray, uint32_t &callstackSize)
{
	DebugBreakHelper helper(this);
	int i = 0;
	for(StackFrameInfo &info : _callstack) {
		callstackArray[i] = info;
		i++;
	}
	callstackSize = i;
}

int32_t Debugger::GetFunctionEntryPointCount()
{
	DebugBreakHelper helper(this);
	return (int32_t)_functionEntryPoints.size();
}

void Debugger::GetFunctionEntryPoints(int32_t* entryPoints, int32_t maxCount)
{
	DebugBreakHelper helper(this);
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
	return _executionStopped || _console->IsExecutionStopped();
}

bool Debugger::IsPauseIconShown()
{
	return (_executionStopped || _console->IsPaused()) && !CheckFlag(DebuggerFlags::HidePauseIcon) && _preventResume == 0 && !_pausedForDebugHelper;
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
	return _mapper->GetAbsoluteAddressAndType(relativeAddr, info);
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

void Debugger::UpdatePpuCyclesToProcess()
{
	memset(_proccessPpuCycle, 0, sizeof(_proccessPpuCycle));
	for(auto updateCycle : _ppuViewerUpdateCycle) {
		int16_t cycle = updateCycle.second >> 9;
		if(cycle < 341) {
			_proccessPpuCycle[cycle] = true;
		}
	}
	_proccessPpuCycle[0] = true;
}

void Debugger::SetPpuViewerScanlineCycle(int32_t ppuViewerId, int32_t scanline, int32_t cycle)
{
	DebugBreakHelper helper(this);
	_ppuViewerUpdateCycle[ppuViewerId] = (cycle << 9) + scanline;
	UpdatePpuCyclesToProcess();
}

void Debugger::ClearPpuViewerSettings(int32_t ppuViewer)
{
	DebugBreakHelper helper(this);
	_ppuViewerUpdateCycle.erase(ppuViewer);
	UpdatePpuCyclesToProcess();
}

void Debugger::SetLastFramePpuScroll(uint16_t addr, uint8_t xScroll, bool updateHorizontalScrollOnly)
{
	_ppuScrollX = ((addr & 0x1F) << 3) | xScroll | ((addr & 0x400) ? 0x100 : 0);
	if(!updateHorizontalScrollOnly) {
		_ppuScrollY = (((addr & 0x3E0) >> 2) | ((addr & 0x7000) >> 12)) + ((addr & 0x800) ? 240 : 0);
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

	if(_console->GetSettings()->CheckFlag(EmulationFlags::DebuggerWindowEnabled)) {
		Step(1);
	} else {
		Run();
	}
}

void Debugger::GetNesHeader(uint8_t* header)
{
	NESHeader nesHeader = _mapper->GetRomInfo().NesHeader;
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
			//Send a ScriptEnded event before reloading the code
			(*result)->ProcessEvent(EventType::ScriptEnded);

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
	_scripts.erase(std::remove_if(_scripts.begin(), _scripts.end(), [=](const shared_ptr<ScriptHost>& script) {
		if(script->GetScriptId() == scriptId) {
			//Send a ScriptEnded event before unloading the script
			script->ProcessEvent(EventType::ScriptEnded);
			_console->GetDebugHud()->ClearScreen();
			return true;
		}
		return false;
	}), _scripts.end());
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
				shared_ptr<StandardController> controller = std::dynamic_pointer_cast<StandardController>(_console->GetControlManager()->GetControlDevice(i));
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
		_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::EventViewerDisplayFrame);

		//Clear the current frame/event log
		if(CheckFlag(DebuggerFlags::PpuPartialDraw)) {
			_ppu->DebugUpdateFrameBuffer(CheckFlag(DebuggerFlags::PpuShowPreviousFrame));
		}
		_prevDebugEvents = _debugEvents;
		_debugEvents.clear();
	} else if(type == EventType::Nmi) {
		AddDebugEvent(DebugEventType::Nmi);
	} else if(type == EventType::Irq) {
		AddDebugEvent(DebugEventType::Irq);
	} else if(type == EventType::SpriteZeroHit) {
		AddDebugEvent(DebugEventType::SpriteZeroHit);
	} else if(type == EventType::Reset) {
		_enableBreakOnUninitRead = true;
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

void Debugger::GetDebugEvents(uint32_t* pictureBuffer, DebugEventInfo *infoArray, uint32_t &maxEventCount, bool returnPreviousFrameData)
{
	DebugBreakHelper helper(this);

	uint16_t *ppuBuffer = new uint16_t[PPU::PixelCount];
	uint32_t *palette = _console->GetSettings()->GetRgbPalette();
	_ppu->DebugCopyOutputBuffer(ppuBuffer);

	for(int i = 0; i < PPU::PixelCount; i++) {
		pictureBuffer[i] = palette[ppuBuffer[i] & 0x3F];
	}

	delete[] ppuBuffer;

	vector<DebugEventInfo> &events = returnPreviousFrameData ? _prevDebugEvents : _debugEvents;
	uint32_t eventCount = std::min(maxEventCount, (uint32_t)events.size());
	memcpy(infoArray, events.data(), eventCount * sizeof(DebugEventInfo));
	maxEventCount = eventCount;
}

uint32_t Debugger::GetDebugEventCount(bool returnPreviousFrameData)
{
	return (uint32_t)(returnPreviousFrameData ? _prevDebugEvents.size() : _debugEvents.size());
}

uint32_t Debugger::GetScreenPixel(uint8_t x, uint8_t y)
{
	return _ppu->GetPixel(x, y);
}

void Debugger::AddTrace(const char* log)
{
	_traceLogger->LogExtraInfo(log, _cpu->GetCycleCount());
}