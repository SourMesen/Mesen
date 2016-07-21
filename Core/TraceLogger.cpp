#include "stdafx.h"
#include "TraceLogger.h"
#include "DisassemblyInfo.h"
#include "DebugState.h"
#include "Console.h"

TraceLogger *TraceLogger::_instance = nullptr;

TraceLogger::TraceLogger(string outputFilepath, TraceLoggerOptions options)
{
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
	if(_instance) {
		_instance->_outputFile << " - [" << log << " - Cycle: " << std::to_string(CPU::GetCycleCount()) << "]";
	}
}

void TraceLogger::Log(DebugState &state, shared_ptr<DisassemblyInfo> disassemblyInfo)
{
	if(disassemblyInfo) {
		State &cpuState = state.CPU;
		PPUDebugState &ppuState = state.PPU;

		string disassembly = disassemblyInfo->ToString(cpuState.DebugPC);
		auto separatorPosition = disassembly.begin() + disassembly.find_first_of(':', 0);
		string byteCode(disassembly.begin(), separatorPosition);
		byteCode.erase(std::remove(byteCode.begin(), byteCode.end(), '$'), byteCode.end());
		string assemblyCode(separatorPosition + 1, disassembly.end());

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

		_outputFile << std::uppercase << std::hex 
						<< std::setfill('0') << std::setw(4) << std::right << (short)cpuState.DebugPC << "  "
						<< std::setfill(' ') << std::setw(10) << std::left << byteCode 
						<< std::setfill(' ') << std::setw(32) << std::left << assemblyCode
						<< std::setfill('0') 
						<< "A:" << std::right << std::setw(2) << (short)cpuState.A
						<< " X:" << std::setw(2) << (short)cpuState.X
						<< " Y:" << std::setw(2) << (short)cpuState.Y
						<< " P:" << std::setw(2) << (short)cpuState.PS
						<< " SP:" << std::setw(2) << (short)cpuState.SP
						<< std::dec << std::setfill(' ') << std::right
						<< " CYC:" << std::setw(3) << ppuCycle << std::left
						<< " SL:" << std::setw(3) << scanline 
						<< " CPU Cycle:" << cpuState.CycleCount;

		_firstLine = false;
	}
}