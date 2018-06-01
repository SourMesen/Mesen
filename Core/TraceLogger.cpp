#include "stdafx.h"
#include <regex>
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

template<typename T>
void TraceLogger::WriteValue(string &output, T value, RowPart& rowPart)
{
	string str = rowPart.DisplayInHex ? HexUtilities::ToHex(value) : std::to_string(value);
	output += str;
	if(rowPart.MinWidth > str.size()) {
		output += std::string(rowPart.MinWidth - str.size(), ' ');
	}
}

template<>
void TraceLogger::WriteValue(string &output, string value, RowPart& rowPart)
{
	output += value;
	if(rowPart.MinWidth > value.size()) {
		output += std::string(rowPart.MinWidth - value.size(), ' ');
	}
}

void TraceLogger::SetOptions(TraceLoggerOptions options)
{
	_options = options;
	string condition = _options.Condition;
	string format = _options.Format;
	
	auto lock = _lock.AcquireSafe();
	_conditionRpnList.clear();
	if(!condition.empty()) {
		vector<int> *rpnList = _expEvaluator->GetRpnList(condition);
		if(rpnList) {
			_conditionRpnList = *rpnList;
		}
	}

	_rowParts.clear();

	std::regex formatRegex = std::regex("(\\[\\s*([^[]*?)\\s*(,\\s*([\\d]*)\\s*(h){0,1}){0,1}\\s*\\])|([^[]*)", std::regex_constants::icase);
	std::sregex_iterator start = std::sregex_iterator(format.cbegin(), format.cend(), formatRegex);
	std::sregex_iterator end = std::sregex_iterator();

	for(std::sregex_iterator it = start; it != end; it++) {
		const std::smatch& match = *it;

		if(match.str(1) == "") {
			RowPart part = {};
			part.DataType = RowDataType::Text;
			part.Text = match.str(6);
			_rowParts.push_back(part);
		} else {
			RowPart part = {};
			
			string dataType = match.str(2);
			if(dataType == "ByteCode") {
				part.DataType = RowDataType::ByteCode;
			} else if(dataType == "Disassembly") {
				part.DataType = RowDataType::Disassembly;
			} else if(dataType == "EffectiveAddress") {
				part.DataType = RowDataType::EffectiveAddress;
			} else if(dataType == "MemoryValue") {
				part.DataType = RowDataType::MemoryValue;
			} else if(dataType == "Align") {
				part.DataType = RowDataType::Align;
			} else if(dataType == "PC") {
				part.DataType = RowDataType::PC;
			} else if(dataType == "A") {
				part.DataType = RowDataType::A;
			} else if(dataType == "X") {
				part.DataType = RowDataType::X;
			} else if(dataType == "Y") {
				part.DataType = RowDataType::Y;
			} else if(dataType == "P") {
				part.DataType = RowDataType::PS;
			} else if(dataType == "SP") {
				part.DataType = RowDataType::SP;
			} else if(dataType == "Cycle") {
				part.DataType = RowDataType::Cycle;
			} else if(dataType == "Scanline") {
				part.DataType = RowDataType::Scanline;
			} else if(dataType == "FrameCount") {
				part.DataType = RowDataType::FrameCount;
			} else if(dataType == "CycleCount") {
				part.DataType = RowDataType::CycleCount;
			} else {
				part.DataType = RowDataType::Text;
				part.Text = "[Invalid tag]";
			}

			if(!match.str(4).empty()) {
				try {
					part.MinWidth = std::stoi(match.str(4));
				} catch(std::exception) {
				}
			}
			part.DisplayInHex = match.str(5) == "h";

			_rowParts.push_back(part);
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
		_instance->_outputFile << "[" << log << " - Cycle: " << std::to_string(CPU::GetCycleCount()) << "]" << (_instance->_options.UseWindowsEol ? "\r\n" : "\n");
	}
}

void TraceLogger::GetStatusFlag(string &output, uint8_t ps, RowPart& part)
{
	if(part.DisplayInHex) {
		WriteValue(output, ps, part);
	} else {
		constexpr char activeStatusLetters[8] = { 'N', 'V', '-', '-', 'D', 'I', 'Z', 'C' };
		constexpr char inactiveStatusLetters[8] = { 'n', 'v', '-', '-', 'd', 'i', 'z', 'c' };
		string flags;
		for(int i = 0; i < 8; i++) {
			if(ps & 0x80) {
				flags += activeStatusLetters[i];
			} else if(part.MinWidth >= 8) {
				flags += inactiveStatusLetters[i];
			}
			ps <<= 1;
		}
		WriteValue(output, flags, part);
	}
}

void TraceLogger::GetTraceRow(string &output, State &cpuState, PPUDebugState &ppuState, DisassemblyInfo &disassemblyInfo)
{
	size_t originalSize = output.size();
	for(RowPart& rowPart : _rowParts) {
		switch(rowPart.DataType) {
			case RowDataType::Text: output += rowPart.Text; break;

			case RowDataType::ByteCode: {
				string byteCode;
				disassemblyInfo.GetByteCode(byteCode);
				if(!rowPart.DisplayInHex) {
					//Remove $ marks if not in "hex" mode (but still display the bytes as hex)
					byteCode.erase(std::remove(byteCode.begin(), byteCode.end(), '$'), byteCode.end());
				}
				WriteValue(output, byteCode, rowPart);
				break;
			}

			case RowDataType::Disassembly: {
				int indentLevel = 0;
				string code;
				
				if(_options.IndentCode) {
					indentLevel = 0xFF - cpuState.SP;
					code = std::string(indentLevel, ' ');
				}
				
				LabelManager* labelManager = _options.UseLabels ? _labelManager.get() : nullptr;
				disassemblyInfo.ToString(code, cpuState.DebugPC, _memoryManager.get(), labelManager, _options.ExtendZeroPage);
				WriteValue(output, code, rowPart);
				break;
			}
			
			case RowDataType::EffectiveAddress:{
				string effectiveAddress;
				disassemblyInfo.GetEffectiveAddressString(effectiveAddress, cpuState, _memoryManager.get(), _options.UseLabels ? _labelManager.get() : nullptr);
				WriteValue(output, effectiveAddress, rowPart);
				break;
			}

			case RowDataType::MemoryValue:{
				int32_t value = disassemblyInfo.GetMemoryValue(cpuState, _memoryManager.get());
				if(value >= 0) {
					output += rowPart.DisplayInHex ? "= $" : "= ";
					WriteValue(output, (uint8_t)value, rowPart);
				}
				break;
			}

			case RowDataType::Align:
				if(output.size() - originalSize < rowPart.MinWidth) {
					output += std::string(rowPart.MinWidth - (output.size() - originalSize), ' ');
				}
				break;

			case RowDataType::PC: WriteValue(output, cpuState.DebugPC, rowPart); break;
			case RowDataType::A: WriteValue(output, cpuState.A, rowPart); break;
			case RowDataType::X: WriteValue(output, cpuState.X, rowPart); break;
			case RowDataType::Y: WriteValue(output, cpuState.Y, rowPart); break;
			case RowDataType::SP: WriteValue(output, cpuState.SP, rowPart); break;
			case RowDataType::PS: GetStatusFlag(output, cpuState.PS, rowPart); break;
			case RowDataType::Cycle: WriteValue(output, ppuState.Cycle, rowPart); break;
			case RowDataType::Scanline: WriteValue(output, ppuState.Scanline, rowPart); break;
			case RowDataType::FrameCount: WriteValue(output, ppuState.FrameCount, rowPart); break;
			case RowDataType::CycleCount: WriteValue(output, cpuState.CycleCount, rowPart); break;
		}
	}
	output += _options.UseWindowsEol ? "\r\n" : "\n";
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
		GetTraceRow(_outputBuffer, state.CPU, state.PPU, disassemblyInfo);
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
		_executionTrace += HexUtilities::ToHex(_cpuStateCacheCopy[index].DebugPC) + "\x1";
		string byteCode;
		_disassemblyCacheCopy[index].GetByteCode(byteCode);
		_executionTrace += byteCode + "\x1";
		GetTraceRow(_executionTrace, _cpuStateCacheCopy[index], _ppuStateCacheCopy[index], _disassemblyCacheCopy[index]);
	}

	return _executionTrace.c_str();
}