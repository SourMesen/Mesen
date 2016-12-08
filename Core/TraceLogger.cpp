#include "stdafx.h"
#include "TraceLogger.h"
#include "DisassemblyInfo.h"
#include "DebugState.h"
#include "Console.h"
#include "MemoryManager.h"
#include "../Utilities/HexUtilities.h"

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
		if(!_outputBuffer.empty()) {
			_outputFile << _outputBuffer;
		}
		_outputFile.close();
	}
	Console::Resume();
}

void TraceLogger::LogStatic(string log)
{
	if(_instance && _instance->_options.ShowExtraInfo) {
		//Flush current buffer
		_instance->_outputFile << _instance->_outputBuffer;
		_instance->_outputBuffer.clear();

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
			_outputBuffer += "\n";
		}

		_outputBuffer += HexUtilities::ToHex(cpuState.DebugPC) + "  ";

		if(_options.ShowByteCode) {
			_outputBuffer += disassemblyInfo->GetByteCode() + std::string(13 - disassemblyInfo->GetByteCode().size(), ' ');
		}

		int indentLevel = 0; 
		if(_options.IndentCode) {
			indentLevel = 0xFF - state.CPU.SP;
			_outputBuffer += std::string(indentLevel, ' ');
		}

		string codeString = disassembly + (_options.ShowEffectiveAddresses ? disassemblyInfo->GetEffectiveAddressString(state.CPU, _memoryManager, nullptr) : "");
		_outputBuffer += codeString + std::string(32 - codeString.size(), ' ');
						
		if(_options.ShowRegisters) {
			_outputBuffer += " A:" + HexUtilities::ToHex(cpuState.A) +
				" X:" + HexUtilities::ToHex(cpuState.X) +
				" Y:" + HexUtilities::ToHex(cpuState.Y) +
				" P:" + HexUtilities::ToHex(cpuState.PS) +
				" SP:" + HexUtilities::ToHex(cpuState.SP);
		}

		if(_options.ShowPpuCycles) {
			string str = std::to_string(ppuCycle);
			_outputBuffer += " CYC:" + std::string(3 - str.size(), ' ') + str;
		}

		if(_options.ShowPpuScanline) {
			string str = std::to_string(scanline);
			_outputBuffer += " SL:" + std::string(3 - str.size(), ' ') + str;
		}

		if(_options.ShowPpuFrames) {
			_outputBuffer += " FC:" + std::to_string(ppuState.FrameCount);
		}

		if(_options.ShowCpuCycles) {
			_outputBuffer += " CPU Cycle:" + std::to_string(cpuState.CycleCount);
		}

		if(_outputBuffer.size() > 32768) {
			_outputFile << _outputBuffer;
			_outputBuffer.clear();
		}

		_firstLine = false;
	}
}