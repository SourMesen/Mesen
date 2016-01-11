#include "stdafx.h"
#include "TraceLogger.h"
#include "DisassemblyInfo.h"
#include "DebugState.h"

TraceLogger::TraceLogger(string outputFilepath, TraceLoggerOptions options)
{
	_outputFile.open(outputFilepath, ios::out | ios::binary);
	_options = options;
}

TraceLogger::~TraceLogger()
{
	if(_outputFile) {
		_outputFile.close();
	}
}

void TraceLogger::Log(DebugState &state, shared_ptr<DisassemblyInfo> disassemblyInfo)
{
	State &cpuState = state.CPU;
	PPUDebugState &ppuState = state.PPU;

	string disassembly = disassemblyInfo->ToString(cpuState.DebugPC);
	while(disassembly.size() < 30) {
		disassembly += " ";
	}

	_outputFile << std::uppercase << std::hex << (short)cpuState.DebugPC << ":  " << disassembly << "  " 
					<< "A:" << (short)cpuState.A << " X:" << (short)cpuState.X << " Y:" << (short)cpuState.Y << " P:" << (short)cpuState.PS << " SP:" << (short)cpuState.SP 
					<< std::dec 
					<< " CYC:" << (short)ppuState.Cycle << " SL:" << (short)ppuState.Scanline << std::endl;
}