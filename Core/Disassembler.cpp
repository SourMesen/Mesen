#include "stdafx.h"
#include <algorithm>
#include "Disassembler.h"
#include "DisassemblyInfo.h"
#include "BaseMapper.h"
#include "MemoryManager.h"
#include "CPU.h"
#include "LabelManager.h"
#include "../Utilities/HexUtilities.h"
#include "../Utilities/StringUtilities.h"
#include "Debugger.h"

Disassembler::Disassembler(uint8_t* internalRam, uint8_t* prgRom, uint32_t prgSize, uint8_t* prgRam, uint32_t prgRamSize, Debugger* debugger)
{
	_debugger = debugger;
	_internalRam = internalRam;
	_prgRom = prgRom;
	_prgRam = prgRam;
	_prgSize = prgSize;
	for(uint32_t i = 0; i < prgSize; i++) {
		_disassembleCache.push_back(shared_ptr<DisassemblyInfo>(nullptr));
	}
	for(uint32_t i = 0; i < prgRamSize; i++) {
		_disassembleRamCache.push_back(shared_ptr<DisassemblyInfo>(nullptr));
	}
	for(uint32_t i = 0; i < 0x800; i++) {
		_disassembleMemoryCache.push_back(shared_ptr<DisassemblyInfo>(nullptr));
	}

	BuildOpCodeTables(false);
}

Disassembler::~Disassembler()
{
}

void Disassembler::BuildOpCodeTables(bool useLowerCase)
{
	string opName[256] = {
	//	0			1			2			3			4			5			6			7			8			9			A			B			C			D			E			F
		"BRK",	"ORA",	"STP",	"SLO",	"NOP",	"ORA",	"ASL",	"SLO",	"PHP",	"ORA",	"ASL",	"ANC",	"NOP",	"ORA",	"ASL",	"SLO", //0
		"BPL",	"ORA",	"STP",	"SLO",	"NOP",	"ORA",	"ASL",	"SLO",	"CLC",	"ORA",	"NOP",	"SLO",	"NOP",	"ORA",	"ASL",	"SLO", //1
		"JSR",	"AND",	"STP",	"RLA",	"BIT",	"AND",	"ROL",	"RLA",	"PLP",	"AND",	"ROL",	"ANC",	"BIT",	"AND",	"ROL",	"RLA", //2
		"BMI",	"AND",	"STP",	"RLA",	"NOP",	"AND",	"ROL",	"RLA",	"SEC",	"AND",	"NOP",	"RLA",	"NOP",	"AND",	"ROL",	"RLA", //3
		"RTI",	"EOR",	"STP",	"SRE",	"NOP",	"EOR",	"LSR",	"SRE",	"PHA",	"EOR",	"LSR",	"ALR",	"JMP",	"EOR",	"LSR",	"SRE", //4
		"BVC",	"EOR",	"STP",	"SRE",	"NOP",	"EOR",	"LSR",	"SRE",	"CLI",	"EOR",	"NOP",	"SRE",	"NOP",	"EOR",	"LSR",	"SRE", //5
		"RTS",	"ADC",	"STP",	"RRA",	"NOP",	"ADC",	"ROR",	"RRA",	"PLA",	"ADC",	"ROR",	"ARR",	"JMP",	"ADC",	"ROR",	"RRA", //6
		"BVS",	"ADC",	"STP",	"RRA",	"NOP",	"ADC",	"ROR",	"RRA",	"SEI",	"ADC",	"NOP",	"RRA",	"NOP",	"ADC",	"ROR",	"RRA", //7
		"NOP",	"STA",	"NOP",	"SAX",	"STY",	"STA",	"STX",	"SAX",	"DEY",	"NOP",	"TXA",	"XAA",	"STY",	"STA",	"STX",	"SAX", //8
		"BCC",	"STA",	"STP",	"AHX",	"STY",	"STA",	"STX",	"SAX",	"TYA",	"STA",	"TXS",	"TAS",	"SHY",	"STA",	"SHX",	"AXA", //9
		"LDY",	"LDA",	"LDX",	"LAX",	"LDY",	"LDA",	"LDX",	"LAX",	"TAY",	"LDA",	"TAX",	"LAX",	"LDY",	"LDA",	"LDX",	"LAX", //A
		"BCS",	"LDA",	"STP",	"LAX",	"LDY",	"LDA",	"LDX",	"LAX",	"CLV",	"LDA",	"TSX",	"LAS",	"LDY",	"LDA",	"LDX",	"LAX", //B
		"CPY",	"CMP",	"NOP",	"DCP",	"CPY",	"CMP",	"DEC",	"DCP",	"INY",	"CMP",	"DEX",	"AXS",	"CPY",	"CMP",	"DEC",	"DCP", //C
		"BNE",	"CMP",	"STP",	"DCP",	"NOP",	"CMP",	"DEC",	"DCP",	"CLD",	"CMP",	"NOP",	"DCP",	"NOP",	"CMP",	"DEC",	"DCP", //D
		"CPX",	"SBC",	"NOP",	"ISC",	"CPX",	"SBC",	"INC",	"ISC",	"INX",	"SBC",	"NOP",	"SBC",	"CPX",	"SBC",	"INC",	"ISC", //E
		"BEQ",	"SBC",	"STP",	"ISC",	"NOP",	"SBC",	"INC",	"ISC",	"SED",	"SBC",	"NOP",	"ISC",	"NOP",	"SBC",	"INC",	"ISC"  //F
	};

	bool unofficial[256] = {
		//	0		1		2		3		4		5		6		7		8		9		A		B		C		D		E		F
			false,false,true, true, true, false,false,true, false,false,false,true, true, false,false,true, //0
			false,false,true, true, true, false,false,true, false,false,true, true, true, false,false,true, //1
			false,false,true, true, false,false,false,true, false,false,false,true, false,false,false,true, //2
			false,false,true, true, true, false,false,true, false,false,true, true, true, false,false,true, //3
			false,false,true, true, true, false,false,true, false,false,false,true, false,false,false,true, //4
			false,false,true, true, true, false,false,true, false,false,true, true, true, false,false,true, //5
			false,false,true, true, true, false,false,true, false,false,false,true, false,false,false,true, //6
			false,false,true, true, true, false,false,true, false,false,true, true, true, false,false,true, //7
			true, false,true, true, false,false,false,true, false,true, false,true, false,false,false,true, //8
			false,false,true, true, false,false,false,true, false,false,false,true, true, false,true, true, //9
			false,false,false,true, false,false,false,true, false,false,false,true, false,false,false,true, //A
			false,false,true, true, false,false,false,true, false,false,false,true, false,false,false,true, //B
			false,false,true, true, false,false,false,true, false,false,false,true, false,false,false,true, //C
			false,false,true, true, true, false,false,true, false,false,true, true, true, false,false,true, //D
			false,false,true, true, false,false,false,true, false,false,false,true, false,false,false,true, //E
			false,false,true, true, true, false,false,true, false,false,true, true, true, false,false,true  //F
	};

	AddrMode opMode[256] = {
		Imp,  IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Acc, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
		Abs,  IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Acc, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
		Imp,  IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Acc, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
		Imp,  IndX,  None, IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Acc, Imm,   Ind,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndYW, None, IndY,  ZeroX, ZeroX, ZeroY, ZeroY, Imp, AbsYW, Imp, AbsY,  AbsXW, AbsXW, AbsYW, AbsY,
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndY,  ZeroX, ZeroX, ZeroY, ZeroY, Imp, AbsY,  Imp, AbsY,  AbsX,  AbsX,  AbsY,  AbsY,
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
		Imm,  IndX,  Imm,  IndX,  Zero,  Zero,  Zero,  Zero,  Imp, Imm,   Imp, Imm,   Abs,   Abs,   Abs,   Abs,
		Rel,  IndY,  None, IndYW, ZeroX, ZeroX, ZeroX, ZeroX, Imp, AbsY,  Imp, AbsYW, AbsX,  AbsX,  AbsXW, AbsXW,
	};

	for(int i = 0; i < 256; i++) {
		if(useLowerCase) {
			string name = opName[i];
			std::transform(name.begin(), name.end(), name.begin(), ::tolower);
			DisassemblyInfo::OPName[i] = name + (unofficial[i] ? "* " : " ");
		} else {
			DisassemblyInfo::OPName[i] = opName[i] + (unofficial[i] ? "* " : " ");
		}

		DisassemblyInfo::IsUnofficialCode[i] = unofficial[i];
		DisassemblyInfo::OPMode[i] = opMode[i];
		switch(DisassemblyInfo::OPMode[i]) {
			case AddrMode::Abs:
			case AddrMode::AbsX:
			case AddrMode::AbsXW:
			case AddrMode::AbsY:
			case AddrMode::AbsYW:
			case AddrMode::Ind:
				DisassemblyInfo::OPSize[i] = 3;
				break;

			case AddrMode::Imm:
			case AddrMode::IndX:
			case AddrMode::IndY:
			case AddrMode::IndYW:
			case AddrMode::Rel:
			case AddrMode::Zero:
			case AddrMode::ZeroX:
			case AddrMode::ZeroY:
				DisassemblyInfo::OPSize[i] = 2;
				break;

			default:
				DisassemblyInfo::OPSize[i] = 1;
				break;
		}
	}
}

bool Disassembler::IsUnofficialOpCode(uint8_t opCode)
{
	return DisassemblyInfo::IsUnofficialCode[opCode];
}

bool Disassembler::IsJump(uint8_t opCode)
{
	return opCode == 0x10 || opCode == 0x30|| opCode == 0x50 || opCode == 0x70 || opCode == 0x90 || opCode == 0xB0 || opCode == 0xD0 || opCode == 0xF0 || opCode == 0x4C || opCode == 0x20;
}

bool Disassembler::IsUnconditionalJump(uint8_t opCode)
{
	return opCode == 0x40 || opCode == 0x60 || opCode == 0x6C || opCode == 0x4C || opCode == 0x20;
}

uint32_t Disassembler::BuildCache(int32_t absoluteAddr, int32_t absoluteRamAddr, uint16_t memoryAddr, bool isSubEntryPoint)
{
	if(memoryAddr < 0x2000) {
		memoryAddr = memoryAddr & 0x7FF;
		if(!_disassembleMemoryCache[memoryAddr]) {
			shared_ptr<DisassemblyInfo> disInfo(new DisassemblyInfo(&_internalRam[memoryAddr], isSubEntryPoint));
			_disassembleMemoryCache[memoryAddr] = disInfo;
			memoryAddr += disInfo->GetSize();
		} else if(isSubEntryPoint) {
			_disassembleMemoryCache[memoryAddr]->SetSubEntryPoint();
		}
		return memoryAddr;
	} else {
		vector<shared_ptr<DisassemblyInfo>> &cache = absoluteRamAddr >= 0 ? _disassembleRamCache : _disassembleCache;
		uint8_t *source = absoluteRamAddr >= 0 ? _prgRam : _prgRom;
		if(absoluteRamAddr >= 0) {
			absoluteAddr = absoluteRamAddr;
		}

		if(absoluteAddr >= 0) {
			shared_ptr<DisassemblyInfo> disInfo = cache[absoluteAddr];
			if(!disInfo) {
				while(absoluteAddr < (int32_t)_prgSize && !cache[absoluteAddr]) {
					bool isJump = IsUnconditionalJump(source[absoluteAddr]);
					disInfo = shared_ptr<DisassemblyInfo>(new DisassemblyInfo(&source[absoluteAddr], isSubEntryPoint));
					isSubEntryPoint = false;

					cache[absoluteAddr] = disInfo;

					absoluteAddr += disInfo->GetSize();
					if(isJump) {
						//Hit a jump/return instruction, can't assume that what follows is actual code, stop disassembling
						break;
					}
				}
			} else {
				if(isSubEntryPoint) {
					disInfo->SetSubEntryPoint();
				}

				uint8_t opCode = source[absoluteAddr];
				if(IsJump(opCode)) {
					uint16_t jumpDest = disInfo->GetOpAddr(memoryAddr);
					AddressTypeInfo info;
					_debugger->GetAbsoluteAddressAndType(jumpDest, &info);

					const uint8_t jsrCode = 0x20;
					if(info.Address >= 0) {
						if(info.Type == AddressType::PrgRom && !_disassembleCache[info.Address]) {
							BuildCache(info.Address, -1, jumpDest, opCode == jsrCode);
						} else if(info.Type == AddressType::WorkRam && !_disassembleRamCache[info.Address]) {
							BuildCache(-1, info.Address, jumpDest, opCode == jsrCode);
						} else if(info.Type == AddressType::InternalRam && !_disassembleMemoryCache[jumpDest]) {
							BuildCache(-1, -1, jumpDest, opCode == jsrCode);
						}
					}
				}

				absoluteAddr += disInfo->GetSize();
			}
		}
		return absoluteAddr;
	}
}

void Disassembler::InvalidateCache(uint16_t memoryAddr, int32_t absoluteRamAddr)
{
	int32_t addr;
	vector<shared_ptr<DisassemblyInfo>> *cache;
	if(memoryAddr < 0x2000) {
		addr = memoryAddr & 0x7FF;
		cache = &_disassembleMemoryCache;
	} else {
		addr = absoluteRamAddr;
		cache = &_disassembleRamCache;
	}

	if(addr >= 0) {
		for(int i = 1; i <= 2; i++) {
			int offsetAddr = (int)addr - i;
			if(offsetAddr >= 0) {
				if((*cache)[offsetAddr] != nullptr) {
					if((*cache)[offsetAddr]->GetSize() >= (uint32_t)i + 1) {
						//Invalidate any instruction that overlapped this address
						(*cache)[offsetAddr] = nullptr;
					}
				}
			}
		}
		(*cache)[addr] = nullptr;
	}
}

void Disassembler::RebuildPrgRomCache(uint32_t absoluteAddr, int32_t length)
{
	for(int i = 1; i <= 2; i++) {
		int offsetAddr = (int)absoluteAddr - i;
		if(offsetAddr >= 0) {
			if(_disassembleCache[offsetAddr] != nullptr) {
				if(_disassembleCache[offsetAddr]->GetSize() >= (uint32_t)i + 1) {
					//Invalidate any instruction that overlapped this address
					_disassembleCache[offsetAddr] = nullptr;
				}
			}
		}
	}

	bool isSubEntryPoint = false;
	if(_disassembleCache[absoluteAddr]) {
		isSubEntryPoint = _disassembleCache[absoluteAddr]->IsSubEntryPoint();
	}

	for(int i = absoluteAddr, end = absoluteAddr + length; i < end; i++) {
		_disassembleCache[i] = nullptr;
	}

	uint16_t memoryAddr = _debugger->GetRelativeAddress(absoluteAddr, AddressType::PrgRom);
	BuildCache(absoluteAddr, -1, memoryAddr, isSubEntryPoint);
}

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

static string emptyString;
void Disassembler::GetLine(string &out, string code, string comment, int32_t cpuAddress, int32_t absoluteAddress)
{
	GetCodeLine(out, code, comment, cpuAddress, absoluteAddress, emptyString, emptyString, false, false);
}

void Disassembler::GetCodeLine(string &out, string &code, string &comment, int32_t cpuAddress, int32_t absoluteAddress, string &byteCode, string &addressing, bool speculativeCode, bool isIndented)
{
	char buffer[1000];
	int pos = 0;
	char* ptrBuf = buffer;
	int* ptrPos = &pos;

	auto writeChar = [=](char c) -> void {
		if(*ptrPos < 999) {
			ptrBuf[(*ptrPos)++] = c;
		}
	};

	auto writeHex = [=](const char* hex) -> void {
		if(*ptrPos < 950) {
			ptrBuf[(*ptrPos)++] = hex[0];
			ptrBuf[(*ptrPos)++] = hex[1];
		}
	};

	auto writeStr = [=](string &str) -> void {
		uint32_t len = (uint32_t)str.size();
		if(*ptrPos + len < 950) {
			memcpy(ptrBuf + (*ptrPos), str.c_str(), len);
			(*ptrPos) += len;
		} else {
			len = 950 - *ptrPos;
			memcpy(ptrBuf + (*ptrPos), str.c_str(), len);
			(*ptrPos) += len;
		}
	};
	
	//Fields:
	//Flags | CpuAddress | AbsAddr | ByteCode | Code | Addressing | Comment
	if(cpuAddress >= 0) {
		if(speculativeCode) {
			writeChar(isIndented ? '6' : '2');
			writeChar('\x1');
		} else {
			writeChar((_debugger->IsMarkedAsCode(cpuAddress) || absoluteAddress == -1) ? (isIndented ? '5' : '1') : (isIndented ? '4' : '0'));
			writeChar('\x1');
		}
		writeHex(hexTable[(cpuAddress >> 8) & 0xFF]);
		writeHex(hexTable[cpuAddress & 0xFF]);
		writeChar('\x1');
	} else {
		writeChar('1');
		writeChar('\x1');
		writeChar('\x1');
	}

	if(absoluteAddress >= 0) {
		if(absoluteAddress > 0xFFFFFF) {
			writeHex(hexTable[(absoluteAddress >> 24) & 0xFF]);
		}
		if(absoluteAddress > 0xFFFF) {
			writeHex(hexTable[(absoluteAddress >> 16) & 0xFF]);
		}			
		writeHex(hexTable[(absoluteAddress >> 8) & 0xFF]);
		writeHex(hexTable[absoluteAddress & 0xFF]);
	}

	writeChar('\x1');
	writeStr(byteCode);
	writeChar('\x1');
	writeStr(code);
	writeChar('\x1');
	writeStr(addressing);
	writeChar('\x1');
	if(!comment.empty()) {
		writeChar(';');
		writeStr(comment);
	}
	writeChar('\x1');
	ptrBuf[(*ptrPos)++] = 0;

	out.append(buffer, pos - 1);
}

void Disassembler::GetSubHeader(string &out, DisassemblyInfo *info, string &label, uint16_t relativeAddr, uint16_t resetVector, uint16_t nmiVector, uint16_t irqVector)
{
	if(info->IsSubEntryPoint()) {
		if(label.empty()) {
			GetLine(out);
			GetLine(out, "__sub start__");
		} else {
			GetLine(out);
			GetLine(out, "__" + label + "()__");
		}
	} else if(relativeAddr == resetVector) {
		GetLine(out);
		GetLine(out, "--reset--");
	} else if(relativeAddr == irqVector) {
		GetLine(out);
		GetLine(out, "--irq--");
	} else if(relativeAddr == nmiVector) {
		GetLine(out);
		GetLine(out, "--nmi--");
	}
}

string Disassembler::GetCode(uint32_t startAddr, uint32_t endAddr, uint16_t memoryAddr, PrgMemoryType memoryType, bool showEffectiveAddresses, bool showOnlyDiassembledCode, State& cpuState, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager) 
{
	string output;
	output.reserve(10000000);

	int32_t dbRelativeAddr = 0;
	int32_t dbAbsoluteAddr = 0;
	string dbBuffer;

	uint16_t resetVector = memoryManager->DebugReadWord(CPU::ResetVector);
	uint16_t nmiVector = memoryManager->DebugReadWord(CPU::NMIVector);
	uint16_t irqVector = memoryManager->DebugReadWord(CPU::IRQVector);

	vector<shared_ptr<DisassemblyInfo>> *cache;
	uint8_t *source;
	uint32_t mask = 0xFFFFFFFF;
	if(memoryAddr < 0x2000) {
		cache = &_disassembleMemoryCache;
		source = _internalRam;
		mask = 0x7FF;
	} else if(memoryType == PrgMemoryType::WorkRam) {
		cache = &_disassembleRamCache;
		source = _prgRam;
	} else {
		cache = &_disassembleCache;
		source = _prgRom;
	}

	string unknownBlockHeader = showOnlyDiassembledCode ? "----" : "__unknown block__";
	uint32_t addr = startAddr;
	uint32_t byteCount = 0;
	bool skippingCode = false;
	shared_ptr<CodeDataLogger> cdl = _debugger->GetCodeDataLogger();
	string label;
	string commentString;
	string commentLines;
	shared_ptr<DisassemblyInfo> infoRef;
	DisassemblyInfo* info;
	bool speculativeCode;
	string spaces = "  ";
	string effAddress;
	string code;
	string byteCode;
	while(addr <= endAddr) {
		labelManager->GetLabelAndComment(memoryAddr, label, commentString);
		commentLines.clear();
		speculativeCode = false;

		if(commentString.find_first_of('\n') != string::npos) {
			for(string &str : StringUtilities::Split(commentString, '\n')) {
				GetLine(commentLines, "", str);
			}
			commentString.clear();
		}
		
		infoRef = (*cache)[addr&mask];

		info = infoRef.get();
		if(!info && source == _prgRom) {
			if(_debugger->CheckFlag(DebuggerFlags::DisassembleEverything) || _debugger->CheckFlag(DebuggerFlags::DisassembleEverythingButData) && !cdl->IsData(addr)) {
				speculativeCode = true;
				info = new DisassemblyInfo(source + addr, false);
			}
		}

		if(info && addr + info->GetSize() <= endAddr) {
			if(byteCount > 0) {
				GetLine(output, dbBuffer, "", dbRelativeAddr, dbAbsoluteAddr);
				byteCount = 0;
			} 
			
			if(skippingCode) {
				GetLine(output, unknownBlockHeader, "", (uint16_t)(memoryAddr - 1), addr - 1);
				skippingCode = false;
			}

			GetSubHeader(output, info, label, memoryAddr, resetVector, nmiVector, irqVector);
			output += commentLines;
			if(!label.empty()) {
				GetLine(output, label + ":");
			}
			
			byteCode.clear();
			code.clear();
			effAddress.clear();
			info->GetEffectiveAddressString(effAddress, cpuState, memoryManager.get(), labelManager.get());
			info->ToString(code, memoryAddr, memoryManager.get(), labelManager.get());
			info->GetByteCode(byteCode);

			GetCodeLine(output, code, commentString, memoryAddr, source != _internalRam ? addr : -1, byteCode, effAddress, speculativeCode, true);

			if(info->IsSubExitPoint()) {
				GetLine(output, "__sub end__");
				GetLine(output);
			}

			if(speculativeCode) {
				//For unverified code, check if a verified instruction starts between the start of this instruction and its end.
				//If so, we need to realign the disassembler to the start of the next verified instruction
				for(uint32_t i = 0; i < info->GetSize(); i++) {
					addr++;
					memoryAddr++;
					if(addr > endAddr || (*cache)[addr&mask]) {
						//Verified code found, stop incrementing address counters
						break;
					}
				}				
			} else {
				addr += info->GetSize();
				memoryAddr += info->GetSize();
			}
		} else {
			if((!label.empty() || !commentString.empty()) && skippingCode) {
				GetLine(output, unknownBlockHeader, "", (uint16_t)(memoryAddr - 1), addr - 1);
				skippingCode = false;
			} 
			
			if(!skippingCode && showOnlyDiassembledCode) {
				if(label.empty()) {
					GetLine(output, "__unknown block__", "", memoryAddr, addr);
					if(!commentString.empty()) {
						GetLine(output, "", commentString);
					}
				} else {
					GetLine(output, "__" + label + "__", "", memoryAddr, addr);
					if(!commentString.empty()) {
						GetLine(output, "", commentString);
					}
					output += commentLines;
				}
				skippingCode = true;
			}

			if(!showOnlyDiassembledCode) {
				if(byteCount >= 8 || ((!label.empty() || !commentString.empty()) && byteCount > 0)) {
					GetLine(output, dbBuffer, "", dbRelativeAddr, dbAbsoluteAddr);
					byteCount = 0;
				}

				if(byteCount == 0) {
					dbBuffer = ".db";
					output += commentLines;
					if(!label.empty()) {
						GetLine(output, label + ":");
					}

					dbRelativeAddr = memoryAddr;
					dbAbsoluteAddr = addr;
				}

				dbBuffer += " $" + HexUtilities::ToHex(source[addr&mask]);

				if(!label.empty() || !commentString.empty()) {
					GetLine(output, dbBuffer, commentString, dbRelativeAddr, dbAbsoluteAddr);
					byteCount = 0;
				} else {
					byteCount++;
				}
			}
			addr++;
			memoryAddr++;
		}

		if(speculativeCode) {
			delete info;
		}
	}

	if(byteCount > 0) {
		GetLine(output, dbBuffer, "", dbRelativeAddr, dbAbsoluteAddr);
	}

	if(skippingCode) {
		GetLine(output, "----", "", (uint16_t)(memoryAddr - 1), addr - 1);
	}
		
	return output;
}

shared_ptr<DisassemblyInfo> Disassembler::GetDisassemblyInfo(int32_t absoluteAddress, int32_t absoluteRamAddress, uint16_t memoryAddress)
{
	if(memoryAddress < 0x2000) {
		return _disassembleMemoryCache[memoryAddress & 0x7FF];
	} else if(absoluteAddress >= 0) {
		return _disassembleCache[absoluteAddress];
	} else if(absoluteRamAddress >= 0) {
		return _disassembleRamCache[absoluteRamAddress];
	}

	return nullptr;
}