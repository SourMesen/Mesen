#include "stdafx.h"
#include "TraceLogger.h"
#include "DisassemblyInfo.h"
#include "DebugState.h"
#include "Console.h"
#include "MemoryManager.h"

TraceLogger *TraceLogger::_instance = nullptr;

TraceLogger::TraceLogger(string outputFilepath, shared_ptr<MemoryManager> memoryManager, TraceLoggerOptions options)
{
	_memoryManager = memoryManager;
	_outputFile.open(outputFilepath, ios::out | ios::binary);
	_options = options;
	_firstLine = true;
	_instance = this;
}

TraceLogger::~TraceLogger()
{
	Console::Pause();
	if(_instance == this) {
		_instance = nullptr;
	}

	if(_outputFile) {
		_outputFile.close();
	}
	Console::Resume();
}

void TraceLogger::LogStatic(string log)
{
	if(_instance && _instance->_options.ShowExtraInfo) {
		_instance->_outputFile << " - [" << log << " - Cycle: " << std::to_string(CPU::GetCycleCount()) << "]";
	}
}

void TraceLogger::Log(DebugState &state, shared_ptr<DisassemblyInfo> disassemblyInfo)
{
	if(disassemblyInfo) {
		State &cpuState = state.CPU;
		PPUDebugState &ppuState = state.PPU;

		string disassembly = disassemblyInfo->ToString(cpuState.DebugPC, _memoryManager, nullptr);

		//Roughly adjust PPU cycle & scanline to take into account the PPU already ran 3 cycles by the time we get here
		short ppuCycle = (short)ppuState.Cycle - 3;
		short scanline = (short)ppuState.Scanline;
		if(ppuCycle < 0) {
			ppuCycle = 341 + ppuCycle;
			scanline--;
			if(scanline < -1) {
				scanline = EmulationSettings::GetNesModel() == NesModel::NTSC ? 260 : 310;
			}
		}

		if(!_firstLine) {
			_outputFile << std::endl;
		}

		_outputFile << std::uppercase << std::hex << std::setfill('0') << std::setw(4) << std::right << (short)cpuState.DebugPC << "  ";

		if(_options.ShowByteCode) {
			_outputFile << std::setfill(' ') << std::setw(10) << std::left << disassemblyInfo->GetByteCode();
		}

		int indentLevel = 0; 
		if(_options.IndentCode) {
			indentLevel = 0xFF - state.CPU.SP;
			_outputFile << std::string(indentLevel, ' ');
		}

		string codeString = disassembly + (_options.ShowEffectiveAddresses ? disassemblyInfo->GetEffectiveAddressString(state.CPU, _memoryManager, nullptr) : "");
		_outputFile << std::setfill(' ') << std::setw(32 - indentLevel) << std::left << codeString;
						
		if(_options.ShowRegisters) {
			_outputFile << std::setfill('0')
				<< "A:" << std::right << std::setw(2) << (short)cpuState.A
				<< " X:" << std::setw(2) << (short)cpuState.X
				<< " Y:" << std::setw(2) << (short)cpuState.Y
				<< " P:" << std::setw(2) << (short)cpuState.PS
				<< " SP:" << std::setw(2) << (short)cpuState.SP;
		}
		
		_outputFile << std::dec << std::setfill(' ');

		if(_options.ShowPpuCycles) {
			_outputFile << std::right << " CYC:" << std::setw(3) << ppuCycle;
		}

		if(_options.ShowPpuScanline) {
			_outputFile << std::left << " SL:" << std::setw(3) << scanline;
		}

		if(_options.ShowPpuFrames) {
			_outputFile << " FC:" << ppuState.FrameCount;
		}

		if(_options.ShowCpuCycles) {
			_outputFile << " CPU Cycle:" << cpuState.CycleCount;
		}
		
		_firstLine = false;
	}
}