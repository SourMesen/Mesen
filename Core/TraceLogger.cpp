#include "stdafx.h"
#include "TraceLogger.h"
#include "DisassemblyInfo.h"
#include "DebuggerTypes.h"
#include "Console.h"
#include "Debugger.h"
#include "MemoryManager.h"
#include "LabelManager.h"
#include "EmulationSettings.h"
#include "ExpressionEvaluator.h"
#include "../Utilities/HexUtilities.h"
#include "../Utilities/FolderUtilities.h"

TraceLogger *TraceLogger::_instance = nullptr;
string TraceLogger::_executionTrace = "";

TraceLogger::TraceLogger(Debugger* debugger, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager)
{
	_expEvaluator = shared_ptr<ExpressionEvaluator>(new ExpressionEvaluator(debugger));
	_memoryManager = memoryManager;
	_labelManager = labelManager;
	_instance = this;
	_currentPos = 0;
	_logCount = 0;
	_logToFile = false;
	_pendingLog = false;
}

TraceLogger::~TraceLogger()
{
	StopLogging();
	_instance = nullptr;
}

void TraceLogger::SetOptions(TraceLoggerOptions options)
{
	_options = options;
	string condition = _options.Condition;
	
	auto lock = _lock.AcquireSafe();
	_conditionRpnList.clear();
	if(!condition.empty()) {
		vector<int> *rpnList = _expEvaluator->GetRpnList(condition);
		if(rpnList) {
			_conditionRpnList = *rpnList;
		}
	}
}

void TraceLogger::StartLogging(string filename)
{
	_outputBuffer.clear();
	_outputFile.open(filename, ios::out | ios::binary);
	_logToFile = true;
}

void TraceLogger::StopLogging() 
{
	if(_logToFile) {
		_logToFile = false;
		if(_outputFile) {
			if(!_outputBuffer.empty()) {
				_outputFile << _outputBuffer;
			}
			_outputFile.close();
		}
	}
}

void TraceLogger::LogStatic(const char *log)
{
	if(_instance && _instance->_logToFile && _instance->_options.ShowExtraInfo) {
		LogStatic(string(log));
	}
}

void TraceLogger::LogStatic(string log)
{
	if(_instance && _instance->_logToFile && _instance->_options.ShowExtraInfo) {
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

void TraceLogger::GetTraceRow(string &output, State &cpuState, PPUDebugState &ppuState, DisassemblyInfo &disassemblyInfo, bool forceByteCode)
{
	output += HexUtilities::ToHex(cpuState.DebugPC) + "  ";

	if(_options.ShowByteCode || forceByteCode) {
		string byteCode;
		disassemblyInfo.GetByteCode(byteCode);
		output += byteCode + std::string(13 - byteCode.size(), ' ');
	}

	int indentLevel = 0;
	if(_options.IndentCode) {
		indentLevel = 0xFF - cpuState.SP;
		output += std::string(indentLevel, ' ');
	}

	string code;
	LabelManager* labelManager = _options.UseLabels ? _labelManager.get() : nullptr;
	disassemblyInfo.ToString(code, cpuState.DebugPC, _memoryManager.get(), labelManager);
	disassemblyInfo.GetEffectiveAddressString(code, cpuState, _memoryManager.get(), labelManager);

	int paddingSize = 32;
	if(_options.ShowMemoryValues) {
		int32_t value = disassemblyInfo.GetMemoryValue(cpuState, _memoryManager.get());
		if(value >= 0) {
			code += " = $" + HexUtilities::ToHex((uint8_t)value);
		}
		paddingSize += 6;
	}
	code += std::string(std::max(0, (int)(paddingSize - code.size())), ' ');
	output += code;

	if(_options.ShowRegisters) {
		output += " A:" + HexUtilities::ToHex(cpuState.A) +
			" X:" + HexUtilities::ToHex(cpuState.X) +
			" Y:" + HexUtilities::ToHex(cpuState.Y);

		GetStatusFlag(output, cpuState.PS);

		output += " SP:" + HexUtilities::ToHex(cpuState.SP);
	}

	if(_options.ShowPpuCycles) {
		string str = std::to_string(ppuState.Cycle);
		output += " CYC:" + std::string(3 - str.size(), ' ') + str;
	}

	if(_options.ShowPpuScanline) {
		string str = std::to_string(ppuState.Scanline);
		output += " SL:" + std::string(3 - str.size(), ' ') + str;
	}

	if(_options.ShowPpuFrames) {
		output += " FC:" + std::to_string(ppuState.FrameCount);
	}

	if(_options.ShowCpuCycles) {
		output += " CPU Cycle:" + std::to_string(cpuState.CycleCount);
	}
	output += "\n";
}

bool TraceLogger::ConditionMatches(DebugState &state, DisassemblyInfo &disassemblyInfo, OperationInfo &operationInfo)
{
	if(!_conditionRpnList.empty()) {
		EvalResultType type;
		if(!_expEvaluator->Evaluate(_conditionRpnList, state, type, operationInfo)) {
			if(operationInfo.OperationType == MemoryOperationType::ExecOpCode) {
				//Condition did not match, keep state/disassembly info for instruction's subsequent cycles
				_lastState = state;
				_lastDisassemblyInfo = disassemblyInfo;
				_pendingLog = true;
			}
			return false;
		}
	}
	return true;
}

void TraceLogger::AddRow(DisassemblyInfo &disassemblyInfo, DebugState &state)
{
	_disassemblyCache[_currentPos] = disassemblyInfo;
	_cpuStateCache[_currentPos] = state.CPU;
	_ppuStateCache[_currentPos] = state.PPU;
	_currentPos = (_currentPos + 1) % ExecutionLogSize;
	_pendingLog = false;

	if(_logCount < ExecutionLogSize) {
		_logCount++;
	}

	if(_logToFile) {
		GetTraceRow(_outputBuffer, state.CPU, state.PPU, disassemblyInfo, false);
		if(_outputBuffer.size() > 32768) {
			_outputFile << _outputBuffer;
			_outputBuffer.clear();
		}
	}
}

void TraceLogger::LogNonExec(OperationInfo& operationInfo)
{
	if(_pendingLog) {
		auto lock = _lock.AcquireSafe();
		if(ConditionMatches(_lastState, _lastDisassemblyInfo, operationInfo)) {
			AddRow(_lastDisassemblyInfo, _lastState);
		}
	}
}

void TraceLogger::Log(DebugState &state, DisassemblyInfo &disassemblyInfo, OperationInfo &operationInfo)
{
	auto lock = _lock.AcquireSafe();
	if(ConditionMatches(state, disassemblyInfo, operationInfo)) {
		AddRow(disassemblyInfo, state);
	}
}

const char* TraceLogger::GetExecutionTrace(uint32_t lineCount)
{
	int startPos;

	_executionTrace.clear();
	{
		auto lock = _lock.AcquireSafe();
		lineCount = std::min(lineCount, _logCount);
		memcpy(_cpuStateCacheCopy, _cpuStateCache, sizeof(_cpuStateCache));
		memcpy(_ppuStateCacheCopy, _ppuStateCache, sizeof(_ppuStateCache));
		memcpy(_disassemblyCacheCopy, _disassemblyCache, sizeof(_disassemblyCache));
		startPos = _currentPos + ExecutionLogSize - lineCount;
	}
	
	for(int i = 0; i < (int)lineCount; i++) {
		int index = (startPos + i) % ExecutionLogSize;
		GetTraceRow(_executionTrace, _cpuStateCacheCopy[index], _ppuStateCacheCopy[index], _disassemblyCacheCopy[index], true);
	}

	return _executionTrace.c_str();
}