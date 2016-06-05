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

Debugger::Debugger(shared_ptr<Console> console, shared_ptr<CPU> cpu, shared_ptr<PPU> ppu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper)
{
	_romFilepath = Console::GetROMPath();
	_console = console;
	_cpu = cpu;
	_ppu = ppu;
	_memoryManager = memoryManager;
	_mapper = mapper;

	_disassembler.reset(new Disassembler(memoryManager->GetInternalRAM(), mapper->GetPrgRom(), mapper->GetPrgSize(), mapper->GetWorkRam(), mapper->GetPrgSize(true)));
	_codeDataLogger.reset(new CodeDataLogger(mapper->GetPrgSize(), mapper->GetChrSize()));

	_stepOut = false;
	_stepCount = -1;
	_stepOverAddr = -1;
	_stepCycleCount = -1;

	_flags = 0;

	_hasBreakpoint = false;
	_bpUpdateNeeded = false;
	_executionStopped = false;

	LoadCdlFile(FolderUtilities::CombinePath(FolderUtilities::GetDebuggerFolder(), FolderUtilities::GetFilename(_romFilepath, false) + ".cdl"));
		
	Debugger::Instance = this;
}

Debugger::~Debugger()
{
	SaveCdlFile(FolderUtilities::CombinePath(FolderUtilities::GetDebuggerFolder(), FolderUtilities::GetFilename(_romFilepath, false) + ".cdl"));

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
	_hasBreakpoint = length > 0;
	
	while(_updatingBreakpoints) { }
	_newBreakpoints.clear();
	_newBreakpoints.insert(_newBreakpoints.end(), breakpoints, breakpoints + length);	
	_bpUpdateNeeded = true;
	if(_executionStopped) {
		UpdateBreakpoints();
	}
}

void Debugger::UpdateBreakpoints()
{
	_updatingBreakpoints = true;
	if(_bpUpdateNeeded) {
		_globalBreakpoints.clear();
		_execBreakpoints.clear();
		_readBreakpoints.clear();
		_writeBreakpoints.clear();
		_readVramBreakpoints.clear();
		_writeVramBreakpoints.clear();

		ExpressionEvaluator expEval;
		for(Breakpoint &bp : _newBreakpoints) {
			if(!expEval.Validate(bp.GetCondition())) {
				//Remove any invalid condition (syntax-wise)
				bp.ClearCondition();
			}

			BreakpointType type = bp.GetType();
			if(type & BreakpointType::Execute) {
				_execBreakpoints.push_back(bp);
			}
			if(type & BreakpointType::ReadRam) {
				_readBreakpoints.push_back(bp);
			}
			if(type & BreakpointType::ReadVram) {
				_readVramBreakpoints.push_back(bp);
			}
			if(type & BreakpointType::WriteRam) {
				_writeBreakpoints.push_back(bp);
			}
			if(type & BreakpointType::WriteVram) {
				_writeVramBreakpoints.push_back(bp);
			}
			if(type == BreakpointType::Global) {
				_globalBreakpoints.push_back(bp);
			}
		}

		_bpUpdateNeeded = false;
	}

	_updatingBreakpoints = false;
}

bool Debugger::HasMatchingBreakpoint(BreakpointType type, uint32_t addr, int16_t value)
{
	UpdateBreakpoints();

	uint32_t absoluteAddr = _mapper->ToAbsoluteAddress(addr);
	vector<Breakpoint> *breakpoints = nullptr;

	switch(type) {
		case BreakpointType::Global: breakpoints = &_globalBreakpoints; break;
		case BreakpointType::Execute: breakpoints = &_execBreakpoints; break;
		case BreakpointType::ReadRam: breakpoints = &_readBreakpoints; break;
		case BreakpointType::WriteRam: breakpoints = &_writeBreakpoints; break;
		case BreakpointType::ReadVram: breakpoints = &_readVramBreakpoints; break;
		case BreakpointType::WriteVram: breakpoints = &_writeVramBreakpoints; break;
	}

	bool needState = true;
	for(size_t i = 0, len = breakpoints->size(); i < len; i++) {
		Breakpoint &breakpoint = (*breakpoints)[i];
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
				if(expEval.Evaluate(condition, _debugState, value, absoluteAddr) != 0) {
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
	if(_hasBreakpoint && HasMatchingBreakpoint(BreakpointType::Global, 0, -1)) {
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

void Debugger::PrivateProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t value)
{
	_currentReadAddr = &addr;

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

		if(_traceLogger) {
			DebugState state;
			GetState(&state);
			_traceLogger->Log(state, _disassembler->GetDisassemblyInfo(absoluteAddr, absoluteRamAddr, addr));
		}
	}

	if(!breakDone && _hasBreakpoint) {
		BreakOnBreakpoint(type, addr, value);
	}

	_currentReadAddr = nullptr;
}

void Debugger::BreakOnBreakpoint(MemoryOperationType type, uint32_t addr, uint8_t value)
{
	if(_hasBreakpoint) {
		BreakpointType breakpointType;
		switch(type) {
			case MemoryOperationType::Read: breakpointType = BreakpointType::ReadRam; break;
			case MemoryOperationType::Write: breakpointType = BreakpointType::WriteRam; break;

			default:
			case MemoryOperationType::ExecOpCode:
			case MemoryOperationType::ExecOperand: breakpointType = BreakpointType::Execute; break;
		}

		if(HasMatchingBreakpoint(breakpointType, addr, (type == MemoryOperationType::ExecOperand) ? -1 : value)) {
			//Found a matching breakpoint, stop execution
			Step(1);
			SleepUntilResume();
		}
	}
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
		_breakLock.AcquireSafe();
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

	if(_hasBreakpoint && HasMatchingBreakpoint(type == MemoryOperationType::Write ? BreakpointType::WriteVram : BreakpointType::ReadVram, addr, value)) {
		//Found a matching breakpoint, stop execution
		Step(1);
		SleepUntilResume();
	}
}

void Debugger::GetState(DebugState *state)
{
	state->CPU = _cpu->GetState();
	state->PPU = _ppu->GetState();
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

	for(uint32_t i = 0x4100; i < 0x10000; i += 0x100) {
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

void Debugger::SetNextStatement(uint16_t addr)
{
	if(_currentReadAddr) {
		_cpu->SetDebugPC(addr);
		*_currentReadAddr = addr;
	}
}

void Debugger::StartTraceLogger(TraceLoggerOptions options)
{
	string traceFilepath = FolderUtilities::CombinePath(FolderUtilities::GetDebuggerFolder(), "Trace.txt");
	_traceLogger.reset(new TraceLogger(traceFilepath, options));
}

void Debugger::StopTraceLogger()
{
	_traceLogger.release();
}

void Debugger::ProcessPpuCycle()
{
	if(Debugger::Instance) {
		Debugger::Instance->PrivateProcessPpuCycle();
	}
}

void Debugger::ProcessRamOperation(MemoryOperationType type, uint16_t &addr, uint8_t value)
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
			_mapper->GetChrRomCopy(&chrRom);
			memcpy(buffer, chrRom, _mapper->GetChrSize());
			delete[] chrRom;
			return _mapper->GetChrSize();

		case DebugMemoryType::ChrRam:
			uint8_t *chrRam;
			_mapper->GetChrRamCopy(&chrRam);
			memcpy(buffer, chrRam, _mapper->GetChrSize(true));
			delete[] chrRam;
			return _mapper->GetChrSize(true);
	}
	return 0;
}

void Debugger::GetNametable(int nametableIndex, uint32_t* frameBuffer, uint8_t* tileData, uint8_t* paletteData)
{
	uint16_t *screenBuffer = new uint16_t[256 * 240];
	uint16_t bgAddr = _ppu->GetState().ControlFlags.BackgroundPatternAddr;
	uint16_t baseAddr = 0x2000 + nametableIndex * 0x400;
	uint16_t baseAttributeAddr = baseAddr + 960;
	for(uint8_t y = 0; y < 30; y++) {
		for(uint8_t x = 0; x < 32; x++) {
			uint8_t tileIndex = _mapper->ReadVRAM(baseAddr + (y << 5) + x);
			uint8_t attribute = _mapper->ReadVRAM(baseAttributeAddr + ((y & 0xFC) << 1) + (x >> 2));
			tileData[y * 32 + x] = tileIndex;
			paletteData[y * 32 + x] = attribute;
			uint8_t shift = (x & 0x02) | ((y & 0x02) << 1);

			uint8_t paletteBaseAddr = ((attribute >> shift) & 0x03) << 2;
			uint16_t tileAddr = bgAddr + (tileIndex << 4);
			for(uint8_t i = 0; i < 8; i++) {
				uint8_t lowByte = _mapper->ReadVRAM(tileAddr + i);
				uint8_t highByte = _mapper->ReadVRAM(tileAddr + i + 8);
				for(uint8_t j = 0; j < 8; j++) {
					uint8_t color = ((lowByte >> (7 - j)) & 0x01) | (((highByte >> (7 - j)) & 0x01) << 1);
					screenBuffer[(y<<11)+(x<<3)+(i<<8)+j] = color == 0 ? _ppu->ReadPaletteRAM(0) : _ppu->ReadPaletteRAM(paletteBaseAddr + color);
				}
			}
		}
	}
	
	VideoDecoder::GetInstance()->DebugDecodeFrame(screenBuffer, frameBuffer, 256*240);

	delete[] screenBuffer;
}

void Debugger::GetChrBank(int bankIndex, uint32_t* frameBuffer, uint8_t palette)
{
	uint16_t *screenBuffer = new uint16_t[128 * 128];
	uint16_t bgAddr = bankIndex == 0 ? 0x0000 : 0x1000;
	for(uint8_t y = 0; y < 16; y++) {
		for(uint8_t x = 0; x < 16; x++) {
			uint8_t tileIndex = y * 16 + x;
			uint8_t paletteBaseAddr = palette << 2;
			uint16_t tileAddr = bgAddr + (tileIndex << 4);
			for(uint8_t i = 0; i < 8; i++) {
				uint8_t lowByte = _mapper->ReadVRAM(tileAddr + i);
				uint8_t highByte = _mapper->ReadVRAM(tileAddr + i + 8);
				for(uint8_t j = 0; j < 8; j++) {
					uint8_t color = ((lowByte >> (7 - j)) & 0x01) | (((highByte >> (7 - j)) & 0x01) << 1);
					screenBuffer[(y<<10)+(x<<3)+(i<<7)+j] = color == 0 ? _ppu->ReadPaletteRAM(0) : _ppu->ReadPaletteRAM(paletteBaseAddr + color);
				}
			}
		}
	}
	
	VideoDecoder::GetInstance()->DebugDecodeFrame(screenBuffer, frameBuffer, 128*128);

	delete[] screenBuffer;
}

void Debugger::GetSprites(uint32_t* frameBuffer)
{
	uint16_t *screenBuffer = new uint16_t[64 * 128];
	memset(screenBuffer, 0, 64*128*sizeof(uint16_t));
	uint8_t *spriteRam = _ppu->GetSpriteRam();

	uint16_t spriteAddr = _ppu->GetState().ControlFlags.SpritePatternAddr;
	bool largeSprites = _ppu->GetState().ControlFlags.LargeSprites;

	for(uint8_t y = 0; y < 8; y++) {
		for(uint8_t x = 0; x < 8; x++) {
			uint8_t ramAddr = ((y << 3) + x) << 2;
			uint8_t tileIndex = spriteRam[ramAddr + 1];
			uint8_t attributes = spriteRam[ramAddr + 2];

			bool verticalMirror = (attributes & 0x80) == 0x80;
			bool horizontalMirror = (attributes & 0x40) == 0x40;

			uint16_t tileAddr;
			if(largeSprites) {
				tileAddr = (tileIndex & 0x01 ? 0x1000 : 0x0000) + ((tileIndex & 0xFE) << 4);
			} else {
				tileAddr = spriteAddr + (tileIndex << 4);
			}

			uint8_t palette = 0x10 + ((attributes & 0x03) << 2);

			for(uint8_t i = 0, iMax = largeSprites ? 16 : 8; i < iMax; i++) {
				if(i == 8) {
					//Move to next tile for 2nd tile of 8x16 sprites
					tileAddr += 8;
				}

				uint8_t lowByte = _mapper->ReadVRAM(tileAddr + i);
				uint8_t highByte = _mapper->ReadVRAM(tileAddr + i + 8);
				for(uint8_t j = 0; j < 8; j++) {
					uint8_t color;
					if(horizontalMirror) {
						color	= ((lowByte >> j) & 0x01) | (((highByte >> j) & 0x01) << 1);
					} else {
						color	= ((lowByte >> (7 - j)) & 0x01) | (((highByte >> (7 - j)) & 0x01) << 1);
					}

					uint16_t destAddr;
					if(verticalMirror) {
						destAddr = (y << 10) + (x << 3) + (((largeSprites ? 15 : 7) - i) << 6) + j;
					} else {
						destAddr = (y << 10) + (x << 3) + (i << 6) + j;
					}

					screenBuffer[destAddr] = color == 0 ? _ppu->ReadPaletteRAM(0) : _ppu->ReadPaletteRAM(palette + color);
				}
			}
		}
	}
	
	VideoDecoder::GetInstance()->DebugDecodeFrame(screenBuffer, frameBuffer, 64*128);

	delete[] screenBuffer;
}

void Debugger::GetPalette(uint32_t* frameBuffer)
{
	uint16_t *screenBuffer = new uint16_t[4*8];
	for(uint8_t i = 0; i < 32; i++) {
		screenBuffer[i] = _ppu->ReadPaletteRAM(i);
	}
	VideoDecoder::GetInstance()->DebugDecodeFrame(screenBuffer, frameBuffer, 4*8);
	delete[] screenBuffer;
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