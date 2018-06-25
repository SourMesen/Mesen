#include "stdafx.h"
#include "../Utilities/HexUtilities.h"
#include "DisassemblyInfo.h"
#include "CPU.h"
#include "LabelManager.h"
#include "MemoryManager.h"

string DisassemblyInfo::OPName[256];
AddrMode DisassemblyInfo::OPMode[256];
bool DisassemblyInfo::IsUnofficialCode[256];
uint8_t DisassemblyInfo::OPSize[256];

static const char* hexTable[256] = {
		"00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F",
		"10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F",
		"20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F",
		"30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F",
		"40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F",
		"50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F",
		"60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F",
		"70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F",
		"80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F",
		"90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F",
		"A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF",
		"B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF",
		"C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF",
		"D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF",
		"E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF",
		"F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF"
};

DisassemblyInfo::DisassemblyInfo()
{
}

void DisassemblyInfo::ToString(string &out, uint32_t memoryAddr, MemoryManager* memoryManager, LabelManager* labelManager, bool extendZeroPage)
{
	char buffer[500];
	uint8_t opCode = _byteCode[0];
	uint16_t length = (uint16_t)DisassemblyInfo::OPName[opCode].size();

	memcpy(buffer, DisassemblyInfo::OPName[opCode].c_str(), length);

	uint16_t* ptrPos = &length;
	char* ptrBuf = buffer;

	uint16_t opAddr = GetOpAddr(memoryAddr);
	
	uint8_t operandLength = 0;
	char operandBuffer[7];
	char* opBuffer = operandBuffer;

	operandBuffer[0] = '$';
	if(_opSize == 2 && _opMode != AddrMode::Rel) {
		if(extendZeroPage && (_opMode == AddrMode::Zero || _opMode == AddrMode::ZeroX || _opMode == AddrMode::ZeroY ||
									 _opMode == AddrMode::IndY || _opMode == AddrMode::IndYW || _opMode == AddrMode::IndX)) {
			operandBuffer[1] = '0';
			operandBuffer[2] = '0';
			memcpy(operandBuffer + 3, hexTable[opAddr], 2);
			operandLength = 5;
		} else {
			memcpy(operandBuffer + 1, hexTable[opAddr], 2);
			operandLength = 3;
		}
	} else {
		memcpy(operandBuffer + 1, hexTable[opAddr >> 8], 2);
		memcpy(operandBuffer + 3, hexTable[opAddr & 0xFF], 2);
		operandLength = 5;
	}

	auto writeChar = [=](char c) -> void {
		ptrBuf[(*ptrPos)++] = c;
	};

	auto copyOperand = [=]() -> void { 
		if(labelManager && _opMode != AddrMode::Imm) {
			string label = labelManager->GetLabel(opAddr, true);
			if(!label.empty()) {
				memcpy(ptrBuf + (*ptrPos), label.c_str(), label.size());
				(*ptrPos) += (uint16_t)label.size();
				return;
			}
		}
		memcpy(ptrBuf + (*ptrPos), opBuffer, operandLength);
		(*ptrPos) += operandLength;
	};

	switch(_opMode) {
		case AddrMode::Acc: writeChar('A'); break;
		case AddrMode::Imm: writeChar('#'); copyOperand(); break;
		case AddrMode::Ind: writeChar('('); copyOperand(); writeChar(')'); break;
		case AddrMode::IndX: writeChar('('); copyOperand(); memcpy(ptrBuf + length, ",X)", 3); length += 3; break;

		case AddrMode::IndY:
		case AddrMode::IndYW:
			writeChar('(');
			copyOperand();
			memcpy(ptrBuf + length, "),Y", 3);
			length += 3;
			break;

		case AddrMode::Abs:
		case AddrMode::Rel:
		case AddrMode::Zero:
			copyOperand();
			break;

		case AddrMode::AbsX:
		case AddrMode::AbsXW:
		case AddrMode::ZeroX:
			copyOperand(); 
			memcpy(ptrBuf + length, ",X", 2);
			length += 2;
			break;

		case AddrMode::AbsY:
		case AddrMode::AbsYW:
		case AddrMode::ZeroY:
			copyOperand();
			memcpy(ptrBuf + length, ",Y", 2);
			length += 2;
			break;
		
		default: break;
	}
	ptrBuf[length] = 0;
	out.append(ptrBuf, length);
}

uint16_t DisassemblyInfo::GetOpAddr(uint16_t memoryAddr)
{
	uint16_t opAddr = 0;
	if(_opSize == 2) {
		opAddr = _byteCode[1];
	} else if(_opSize == 3) {
		opAddr = _byteCode[1] | (_byteCode[2] << 8);
	}

	if(_opMode == AddrMode::Rel) {
		opAddr = (int8_t)opAddr + memoryAddr + 2;
	}

	return opAddr;
}

DisassemblyInfo::DisassemblyInfo(uint8_t* opPointer, bool isSubEntryPoint)
{
	_isSubEntryPoint = isSubEntryPoint;

	uint8_t opCode = *opPointer;
	_opSize = DisassemblyInfo::OPSize[opCode];
	_opMode = DisassemblyInfo::OPMode[opCode];

	for(uint32_t i = 0; i < _opSize; i++) {
		_byteCode[i] = *(opPointer + i);
	}

	_isSubExitPoint = opCode == 0x40 || opCode == 0x60;
}

void DisassemblyInfo::SetSubEntryPoint()
{
	_isSubEntryPoint = true;
}

int32_t DisassemblyInfo::GetMemoryValue(State& cpuState, MemoryManager* memoryManager)
{
	int32_t address = -1;
	if(_opMode <= AddrMode::Abs) {
		if(_opMode == AddrMode::Rel || _opMode == AddrMode::Abs || _opMode == AddrMode::Zero) {
			address = GetOpAddr(cpuState.DebugPC);
		}
	} else {
		address = GetEffectiveAddress(cpuState, memoryManager);
	}

	if(address >= 0 && address <= 0xFFFF) {
		return memoryManager->DebugRead(address);
	} else {
		return -1;
	}
}

void DisassemblyInfo::GetEffectiveAddressString(string &out, State& cpuState, MemoryManager* memoryManager, LabelManager* labelManager)
{
	if(_opMode <= AddrMode::Abs) {
		return;
	} else {
		int32_t effectiveAddress = GetEffectiveAddress(cpuState, memoryManager);
		char buffer[500];

		int length = 0;
		buffer[0] = ' ';
		buffer[1] = '@';
		buffer[2] = ' ';

		if(labelManager) {
			string label = labelManager->GetLabel(effectiveAddress, true);
			if(!label.empty()) {
				memcpy(buffer + 3, label.c_str(), label.size());
				length = (uint16_t)label.size() + 3;
				buffer[length] = 0;

				out.append(buffer, length);
				return;
			}
		}

		buffer[3] = '$';

		if(_opMode == AddrMode::ZeroX || _opMode == AddrMode::ZeroY) {
			memcpy(buffer + 4, hexTable[effectiveAddress], 2);
			buffer[6] = 0;
			length = 6;
		} else {
			memcpy(buffer + 4, hexTable[effectiveAddress >> 8], 2);
			memcpy(buffer + 6, hexTable[effectiveAddress & 0xFF], 2);
			buffer[8] = 0;
			length = 8;
		}
		
		out.append(buffer, length);
	}
}

int32_t DisassemblyInfo::GetEffectiveAddress(State& cpuState, MemoryManager* memoryManager)
{
	switch(_opMode) {
		default: break;

		case AddrMode::ZeroX: return (uint8_t)(_byteCode[1] + cpuState.X); break;
		case AddrMode::ZeroY: return (uint8_t)(_byteCode[1] + cpuState.Y); break;

		case AddrMode::IndX: {
			uint8_t zeroAddr = _byteCode[1] + cpuState.X;
			return memoryManager->DebugRead(zeroAddr) | memoryManager->DebugRead((uint8_t)(zeroAddr + 1)) << 8;
		}

		case AddrMode::IndY:
		case AddrMode::IndYW: {
			uint8_t zeroAddr = _byteCode[1];
			uint16_t addr = memoryManager->DebugRead(zeroAddr) | memoryManager->DebugRead((uint8_t)(zeroAddr + 1)) << 8;
			return (uint16_t)(addr + cpuState.Y);
		}

		case AddrMode::Ind: {
			uint8_t zeroAddr = _byteCode[1];
			return memoryManager->DebugRead(zeroAddr) | memoryManager->DebugRead((uint8_t)(zeroAddr + 1)) << 8;
		}

		case AddrMode::AbsX:
		case AddrMode::AbsXW: {
			return (uint16_t)((_byteCode[1] | (_byteCode[2] << 8)) + cpuState.X) & 0xFFFF;
		}

		case AddrMode::AbsY:
		case AddrMode::AbsYW: {
			return (uint16_t)((_byteCode[1] | (_byteCode[2] << 8)) + cpuState.Y) & 0xFFFF;
		}
	}

	return -1;
}
		
void DisassemblyInfo::GetByteCode(string &out)
{
	//Raw byte code
	char byteCode[12];
	int pos = 1;
	byteCode[0] = '$';
	for(uint32_t i = 0; i < _opSize; i++) {
		if(i != 0) {
			byteCode[pos++] = ' ';
			byteCode[pos++] = '$';
		}

		byteCode[pos++] = hexTable[_byteCode[i]][0];
		byteCode[pos++] = hexTable[_byteCode[i]][1];
	}

	byteCode[pos] = 0;
	out.append(byteCode, pos);
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