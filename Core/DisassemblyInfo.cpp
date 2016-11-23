#include "stdafx.h"
#include "DisassemblyInfo.h"
#include "CPU.h"
#include "LabelManager.h"

string DisassemblyInfo::OPName[256];
AddrMode DisassemblyInfo::OPMode[256];
uint32_t DisassemblyInfo::OPSize[256];

string DisassemblyInfo::ToString(uint32_t memoryAddr, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager)
{
	std::ostringstream output;
	uint8_t opCode = *_opPointer;

	output << DisassemblyInfo::OPName[opCode];
	if(DisassemblyInfo::OPName[opCode].empty()) {
		output << "invalid opcode";
	}

	std::ostringstream addrString;
	if(_opSize == 2) {
		_opAddr = *(_opPointer + 1);
	} else if(_opSize == 3) {
		_opAddr = *(_opPointer + 1) | (*(_opPointer + 2) << 8);
	}
	
	string operandValue;
	if(labelManager && _opMode != AddrMode::Imm) {
		operandValue = labelManager->GetLabel(_opAddr, true);
	}
	
	if(operandValue.empty()) {
		std::stringstream ss;
		ss << "$" << std::uppercase << std::hex << std::setw(_opSize == 2 ? 2 : 4) << std::setfill('0') << (short)_opAddr;
		operandValue = ss.str();
	}

	output << " ";

	switch(_opMode) {
		case AddrMode::Acc: output << "A"; break;
		case AddrMode::Imm: output << "#" << operandValue; break;
		case AddrMode::Ind: output << "(" << operandValue << ")"; break;
		case AddrMode::IndX: output << "(" << operandValue << ",X)"; break;

		case AddrMode::IndY:
		case AddrMode::IndYW:
			output << "(" << operandValue << "),Y";
			break;

		case AddrMode::Abs:
		case AddrMode::Zero:
			output << operandValue;
			break;

		case AddrMode::AbsX:
		case AddrMode::AbsXW:
		case AddrMode::ZeroX:
			output << operandValue << ",X";
			break;

		case AddrMode::AbsY:
		case AddrMode::AbsYW:
		case AddrMode::ZeroY:
			output << operandValue << ",Y";
			break;

		case AddrMode::Rel:
			//TODO (not correct when banks are switched around in memory)
			output << "$" << std::uppercase << std::hex << std::setw(2) << std::setfill('0') << ((int8_t)*(_opPointer + 1) + memoryAddr + 2);
			break;
		
		default: break;
	}

	return output.str();
}

DisassemblyInfo::DisassemblyInfo(uint8_t* opPointer, bool isSubEntryPoint)
{
	_opPointer = opPointer;
	_isSubEntryPoint = isSubEntryPoint;

	uint8_t opCode = *_opPointer;
	_opSize = DisassemblyInfo::OPSize[opCode];
	_opMode = DisassemblyInfo::OPMode[opCode];
	_isSubExitPoint = opCode == 0x40 || opCode == 0x60;


	//Raw byte code
	std::stringstream byteCodeOutput;
	for(uint32_t i = 0; i < 3; i++) {
		if(i < _opSize) {
			byteCodeOutput << "$" << std::hex << std::uppercase << std::setfill('0') << std::setw(2) << (short)*(_opPointer + i);
		} else {
			byteCodeOutput << "   ";
		}
		if(i != 2) {
			byteCodeOutput << " ";
		}
	}
	_byteCode = byteCodeOutput.str();

}

void DisassemblyInfo::SetSubEntryPoint()
{
	_isSubEntryPoint = true;
}

string DisassemblyInfo::GetEffectiveAddressString(State& cpuState, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager)
{
	int32_t effectiveAddress = GetEffectiveAddress(cpuState, memoryManager);
	if(effectiveAddress < 0) {
		return "";
	} else {
		bool empty = true;
		if(labelManager) {
			string label = labelManager->GetLabel(effectiveAddress, true);
			if(!label.empty()) {
				return " @ " + label;
			}
		}
		
		std::stringstream ss;
		ss << std::uppercase << std::setfill('0') << " @ $";
		if(_opMode == AddrMode::ZeroX || _opMode == AddrMode::ZeroY) {
			ss << std::setw(2) << std::hex << (uint16_t)effectiveAddress;
		} else {
			ss << std::setw(4) << std::hex << (uint16_t)effectiveAddress;
		}
		
		return ss.str();
	}
}

int32_t DisassemblyInfo::GetEffectiveAddress(State& cpuState, shared_ptr<MemoryManager> memoryManager)
{
	switch(_opMode) {
		case AddrMode::ZeroX: return (uint8_t)(*(_opPointer + 1) + cpuState.X); break;
		case AddrMode::ZeroY: return (uint8_t)(*(_opPointer + 1) + cpuState.Y); break;

		case AddrMode::IndX: {
			uint8_t zeroAddr = *(_opPointer + 1) + cpuState.X;
			return memoryManager->DebugRead(zeroAddr) | memoryManager->DebugRead((uint8_t)(zeroAddr + 1)) << 8;
		}

		case AddrMode::IndY:
		case AddrMode::IndYW: {
			uint8_t zeroAddr = *(_opPointer + 1);
			uint16_t addr = memoryManager->DebugRead(zeroAddr) | memoryManager->DebugRead((uint8_t)(zeroAddr + 1)) << 8;
			return addr + cpuState.Y;
		}

		case AddrMode::Ind: {
			uint8_t zeroAddr = *(_opPointer + 1);
			return memoryManager->DebugRead(zeroAddr) | memoryManager->DebugRead((uint8_t)(zeroAddr + 1)) << 8;
		}

		case AddrMode::AbsX:
		case AddrMode::AbsXW: {
			return (*(_opPointer + 1) | (*(_opPointer + 2) << 8)) + cpuState.X;
		}

		case AddrMode::AbsY:
		case AddrMode::AbsYW: {
			return (*(_opPointer + 1) | (*(_opPointer + 2) << 8)) + cpuState.Y;
		}
	}

	return -1;
}
		
string DisassemblyInfo::GetByteCode()
{
	return _byteCode;
}

uint32_t DisassemblyInfo::GetSize()
{
	return _opSize;
}

bool DisassemblyInfo::IsSubEntryPoint()
{
	return _isSubEntryPoint;
}

bool DisassemblyInfo::IsSubExitPoint()
{
	return _isSubExitPoint;
}