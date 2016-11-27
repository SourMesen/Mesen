#include "stdafx.h"
#include "DisassemblyInfo.h"
#include "CPU.h"
#include "LabelManager.h"
#include "../Utilities/HexUtilities.h"

string DisassemblyInfo::OPName[256];
AddrMode DisassemblyInfo::OPMode[256];
uint32_t DisassemblyInfo::OPSize[256];

string DisassemblyInfo::ToString(uint32_t memoryAddr, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager)
{
	string out;
	out.reserve(50);

	uint8_t opCode = *_opPointer;

	if(DisassemblyInfo::OPName[opCode].empty()) {
		out = "invalid opcode";
	} else {
		out = DisassemblyInfo::OPName[opCode];
	}

	if(_opSize == 2) {
		_opAddr = *(_opPointer + 1);
	} else if(_opSize == 3) {
		_opAddr = *(_opPointer + 1) | (*(_opPointer + 2) << 8);
	}

	if(_opMode == AddrMode::Rel) {
		_opAddr = (int8_t)_opAddr + memoryAddr + 2;
	}
	
	string operandValue;
	if(labelManager && _opMode != AddrMode::Imm) {
		operandValue = labelManager->GetLabel(_opAddr, true);
	}
	
	if(operandValue.empty()) {
		if(_opSize == 2 && _opMode != AddrMode::Rel) {
			operandValue += "$" + HexUtilities::ToHex((uint8_t)_opAddr);
		} else {
			operandValue += "$" + HexUtilities::ToHex((uint16_t)_opAddr);
		}
	}

	out += " ";

	switch(_opMode) {
		case AddrMode::Acc: out += "A"; break;
		case AddrMode::Imm: out += "#" + operandValue; break;
		case AddrMode::Ind: out += "(" + operandValue + ")"; break;
		case AddrMode::IndX: out += "(" + operandValue + ",X)"; break;

		case AddrMode::IndY:
		case AddrMode::IndYW:
			out += "(" + operandValue + "),Y";
			break;

		case AddrMode::Abs:
		case AddrMode::Rel:
		case AddrMode::Zero:
			out += operandValue;
			break;

		case AddrMode::AbsX:
		case AddrMode::AbsXW:
		case AddrMode::ZeroX:
			out += operandValue + ",X";
			break;

		case AddrMode::AbsY:
		case AddrMode::AbsYW:
		case AddrMode::ZeroY:
			out += operandValue + ",Y";
			break;
		
		default: break;
	}

	return out;
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
	string byteCodeOutput;
	byteCodeOutput.reserve(10);
	for(uint32_t i = 0; i < 3; i++) {
		if(i < _opSize) {
			byteCodeOutput += "$" + HexUtilities::ToHex((uint8_t)*(_opPointer + i));
		} else {
			byteCodeOutput += "   ";
		}
		if(i != 2) {
			byteCodeOutput += " ";
		}
	}
	_byteCode = byteCodeOutput;
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
		
		string output;
		output = " @ $";
		if(_opMode == AddrMode::ZeroX || _opMode == AddrMode::ZeroY) {
			output += HexUtilities::ToHex((uint8_t)effectiveAddress);
		} else {
			output += HexUtilities::ToHex((uint16_t)effectiveAddress);
		}
		
		return output;
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