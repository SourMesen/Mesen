#include "stdafx.h"
#include "DisassemblyInfo.h"
#include "CPU.h"

string DisassemblyInfo::OPName[256];
AddrMode DisassemblyInfo::OPMode[256];
uint32_t DisassemblyInfo::OPSize[256];

void DisassemblyInfo::Initialize(uint32_t memoryAddr)
{
	_lastAddr = memoryAddr;

	std::ostringstream output;
	uint8_t opCode = *_opPointer;
	_opSize = DisassemblyInfo::OPSize[opCode];
	_opMode = DisassemblyInfo::OPMode[opCode];

	//Output raw byte code
	for(uint32_t i = 0; i < 3; i++) {
		if(i < _opSize) {
			output << "$" << std::hex << std::uppercase << std::setfill('0') << std::setw(2) << (short)*(_opPointer + i);
		} else {
			output << "   ";
		}
		if(i != 2) {
			output << " ";
		}
	}
	output << ":";

	output << DisassemblyInfo::OPName[opCode];
	if(opCode == 0x40 || opCode == 0x60) {
		//Make end of function/interrupt routines more obvious
		output << " ---->";
	}

	if(DisassemblyInfo::OPName[opCode].empty()) {
		output << "invalid opcode";
	}

	std::ostringstream nextByte;
	std::ostringstream nextWord;
	if(_opSize == 2) {
		nextByte << std::uppercase << std::hex << std::setw(2) << std::setfill('0') << (short)(*(_opPointer + 1));
	} else if(_opSize == 3) {
		nextWord << std::uppercase << std::hex << std::setw(4) << std::setfill('0') << (*(_opPointer + 1) | (*(_opPointer + 2) << 8));
	}

	switch(_opMode) {
		case AddrMode::Abs:
			output << " $" << nextWord.str();
			break;

		case AddrMode::AbsX:
		case AddrMode::AbsXW:
			output << " $" << nextWord.str() << ",X";
			break;

		case AddrMode::AbsY:
		case AddrMode::AbsYW:
			output << " $" << nextWord.str() << ",Y";
			break;

		case AddrMode::Imm:
			output << " #$" << nextByte.str();
			break;

		case AddrMode::Ind:
			output << " ($" << nextWord.str() << ")";
			break;

		case AddrMode::IndX:
			output << " ($" << nextByte.str() << ",X)";
			break;

		case AddrMode::IndY:
		case AddrMode::IndYW:
			output << " ($" << nextByte.str() << "),Y";
			break;

		case AddrMode::Rel:
			//TODO (not correct when banks are switched around in memory)
			output << " $" << std::uppercase << std::hex << std::setw(2) << std::setfill('0') << ((int8_t)*(_opPointer + 1) + memoryAddr + 2);
			break;

		case AddrMode::Zero:
			output << " $" << nextByte.str();
			break;

		case AddrMode::ZeroX:
			output << " $" << nextByte.str() << ",X";
			break;

		case AddrMode::ZeroY:
			output << " $" << nextByte.str() << ",Y";
			break;

		case AddrMode::Acc:
			output << " A";
			break;

		default:
			break;
	}

	if(_isSubEntryPoint) {
		output << " <----";
	}

	_disassembly = output.str();
}

DisassemblyInfo::DisassemblyInfo(uint8_t* opPointer, bool isSubEntryPoint)
{
	_opPointer = opPointer;
	_isSubEntryPoint = isSubEntryPoint;

	Initialize();
}

void DisassemblyInfo::SetSubEntryPoint()
{
	if(!_isSubEntryPoint) {
		_isSubEntryPoint = true;
		Initialize();
	}
}
		
string DisassemblyInfo::ToString(uint32_t memoryAddr)
{
	if(memoryAddr != _lastAddr && _opMode == AddrMode::Rel) {
		Initialize(memoryAddr);
	}
	return _disassembly;
}

uint32_t DisassemblyInfo::GetSize()
{
	return _opSize;
}

