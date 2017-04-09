#include "stdafx.h"
#include "TraceLogger.h"
#include "DisassemblyInfo.h"
#include "DebuggerTypes.h"
#include "Console.h"
#include "MemoryManager.h"
#include "LabelManager.h"
#include "../Utilities/HexUtilities.h"
#include "../Utilities/FolderUtilities.h"

TraceLogger *TraceLogger::_instance = nullptr;

TraceLogger::TraceLogger(shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager)
{
	_memoryManager = memoryManager;
	_labelManager = labelManager;
	_instance = this;
	_currentPos = 0;
	_logToFile = false;
}

TraceLogger::~TraceLogger()
{
	StopLogging();
	_instance = nullptr;
}

void TraceLogger::SetOptions(TraceLoggerOptions options)
{
	_options = options;
}

void TraceLogger::StartLogging(string filename)
{
	_outputFile.open(filename, ios::out | ios::binary);
	_logToFile = true;
	_firstLine = true;
}

void TraceLogger::StopLogging() 
{
	if(_logToFile) {
		Console::Pause();
		if(_outputFile) {
			if(!_outputBuffer.empty()) {
				_outputFile << _outputBuffer;
			}
			_outputFile.close();
		}
		Console::Resume();
		_logToFile = false;
	}
}


void TraceLogger::LogStatic(string log)
{
	if(_instance && _instance->_logToFile && _instance->_options.ShowExtraInfo && !_instance->_firstLine) {
		//Flush current buffer
		_instance->_outputFile << _instance->_outputBuffer;
		_instance->_outputBuffer.clear();

		_instance->_outputFile << " - [" << log << " - Cycle: " << std::to_string(CPU::GetCycleCount()) << "]";
	}
}

void TraceLogger::GetStatusFlag(string &output, uint8_t ps)
{
	output += " P:";
	if(_options.StatusFormat == StatusFlagFormat::Hexadecimal) {
		output.append(HexUtilities::ToHex(ps));
	} else {
		constexpr char activeStatusLetters[8] = { 'N', 'V', 'B', '-', 'D', 'I', 'Z', 'C' };
		constexpr char inactiveStatusLetters[8] = { 'n', 'v', 'b', '-', 'd', 'i', 'z', 'c' };
		int padding = 6;
		for(int i = 0; i < 8; i++) {
			if(ps & 0x80) {
				output += activeStatusLetters[i];
				padding--;
			} else if(_options.StatusFormat == StatusFlagFormat::Text) {
				output += inactiveStatusLetters[i];
				padding--;
			}
			ps <<= 1;
		}
		if(padding > 0) {
			output += string(padding, ' ');
		}
	}
}

void TraceLogger::GetTraceRow(string &output, State &cpuState, PPUDebugState &ppuState, shared_ptr<DisassemblyInfo> &disassemblyInfo, bool firstLine)
{
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

	if(!firstLine) {
		output += "\n";
	}

	output += HexUtilities::ToHex(cpuState.DebugPC) + "  ";

	if(_options.ShowByteCode) {
		string byteCode;
		disassemblyInfo->GetByteCode(byteCode);
		output += byteCode + std::string(13 - byteCode.size(), ' ');
	}

	int indentLevel = 0;
	if(_options.IndentCode) {
		indentLevel = 0xFF - cpuState.SP;
		output += std::string(indentLevel, ' ');
	}

	string code;
	LabelManager* labelManager = _options.UseLabels ? _labelManager.get() : nullptr;
	disassemblyInfo->ToString(code, cpuState.DebugPC, _memoryManager.get(), labelManager);
	disassemblyInfo->GetEffectiveAddressString(code, cpuState, _memoryManager.get(), labelManager);
	code += std::string(std::max(0, (int)(32 - code.size())), ' ');
	output += code;

	if(_options.ShowRegisters) {
		output += " A:" + HexUtilities::ToHex(cpuState.A) +
			" X:" + HexUtilities::ToHex(cpuState.X) +
			" Y:" + HexUtilities::ToHex(cpuState.Y);

		GetStatusFlag(output, cpuState.PS);

		output += " SP:" + HexUtilities::ToHex(cpuState.SP);
	}

	if(_options.ShowPpuCycles) {
		string str = std::to_string(ppuCycle);
		output += " CYC:" + std::string(3 - str.size(), ' ') + str;
	}

	if(_options.ShowPpuScanline) {
		string str = std::to_string(scanline);
		output += " SL:" + std::string(3 - str.size(), ' ') + str;
	}

	if(_options.ShowPpuFrames) {
		output += " FC:" + std::to_string(ppuState.FrameCount);
	}

	if(_options.ShowCpuCycles) {
		output += " CPU Cycle:" + std::to_string(cpuState.CycleCount);
	}
}

SimpleLock _lock;
void TraceLogger::Log(State &cpuState, PPUDebugState &ppuState, shared_ptr<DisassemblyInfo> disassemblyInfo)
{
	if(disassemblyInfo) {
		auto lock = _lock.AcquireSafe();
		_disassemblyCache[_currentPos] = disassemblyInfo;
		_cpuStateCache[_currentPos] = cpuState;
		_ppuStateCache[_currentPos] = ppuState;
		_currentPos = (_currentPos + 1) % ExecutionLogSize;
		
		if(_logToFile) {
			GetTraceRow(_outputBuffer, cpuState, ppuState, disassemblyInfo, _firstLine);
			if(_outputBuffer.size() > 32768) {
				_outputFile << _outputBuffer;
				_outputBuffer.clear();
			}

			_firstLine = false;
		}
	}
}

const char* TraceLogger::GetExecutionTrace(uint32_t lineCount)
{
	_executionTrace.clear();
	auto lock = _lock.AcquireSafe();
	int startPos = _currentPos + ExecutionLogSize - lineCount;
	bool firstLine = true;
	for(int i = 0; i < (int)lineCount; i++) {
		int index = (startPos + i) % ExecutionLogSize;
		if(_disassemblyCache[index]) {
			GetTraceRow(_executionTrace, _cpuStateCache[index], _ppuStateCache[index], _disassemblyCache[index], firstLine);
			firstLine = false;
		}
	}
	return _executionTrace.c_str();
}