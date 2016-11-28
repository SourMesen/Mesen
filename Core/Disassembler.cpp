#include "stdafx.h"
#include "Disassembler.h"
#include "DisassemblyInfo.h"
#include "BaseMapper.h"
#include "MemoryManager.h"
#include "CPU.h"
#include "LabelManager.h"
#include "../Utilities/HexUtilities.h"
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

	string opName[256] = {
	//	0			1			2			3			4			5			6			7			8			9			A			B			C			D			E			F
		"BRK",	"ORA",	"",		"SLO*",	"NOP",	"ORA",	"ASL",	"SLO*",	"PHP",	"ORA",	"ASL",	"AAC*",	"NOP",	"ORA",	"ASL",	"SLO*", //0
		"BPL",	"ORA",	"",		"SLO*",	"NOP",	"ORA",	"ASL",	"SLO*",	"CLC",	"ORA",	"NOP*",	"SLO*",	"NOP",	"ORA",	"ASL",	"SLO*", //1
		"JSR",	"AND",	"",		"RLA*",	"BIT",	"AND",	"ROL",	"RLA*",	"PLP",	"AND",	"ROL",	"AAC*",	"BIT",	"AND",	"ROL",	"RLA*", //2
		"BMI",	"AND",	"",		"RLA*",	"NOP",	"AND",	"ROL",	"RLA*",	"SEC",	"AND",	"NOP*",	"RLA*",	"NOP",	"AND",	"ROL",	"RLA*", //3
		"RTI",	"EOR",	"",		"SRE*",	"NOP",	"EOR",	"LSR",	"SRE*",	"PHA",	"EOR",	"LSR",	"ASR*",	"JMP",	"EOR",	"LSR",	"SRE*", //4
		"BVC",	"EOR",	"",		"SRE*",	"NOP",	"EOR",	"LSR",	"SRE*",	"CLI",	"EOR",	"NOP*",	"SRE*",	"NOP",	"EOR",	"LSR",	"SRE*", //5
		"RTS",	"ADC",	"",		"RRA*",	"NOP",	"ADC",	"ROR",	"RRA*",	"PLA",	"ADC",	"ROR",	"ARR*",	"JMP",	"ADC",	"ROR",	"RRA*", //6
		"BVS",	"ADC",	"",		"RRA*",	"NOP",	"ADC",	"ROR",	"RRA*",	"SEI",	"ADC",	"NOP*",	"RRA*",	"NOP",	"ADC",	"ROR",	"RRA*", //7
		"NOP",	"STA",	"NOP",	"SAX*",	"STY",	"STA",	"STX",	"SAX*",	"DEY",	"NOP",	"TXA",	"",		"STY",	"STA",	"STX",	"SAX*", //8
		"BCC",	"STA",	"",		"AXA*",	"STY",	"STA",	"STX",	"SAX*",	"TYA",	"STA",	"TXS",	"TAS*",	"SYA*",	"STA",	"SXA",	"AXA*", //9
		"LDY",	"LDA",	"LDX",	"LAX*",	"LDY",	"LDA",	"LDX",	"LAX*",	"TAY",	"LDA",	"TAX",	"ATX*",	"LDY",	"LDA",	"LDX",	"LAX*", //A
		"BCS",	"LDA",	"",		"LAX*",	"LDY",	"LDA",	"LDX",	"LAX*",	"CLV",	"LDA",	"TSX",	"LAS*",	"LDY",	"LDA",	"LDX",	"LAX*", //B
		"CPY",	"CPA",	"NOP",	"DCP*",	"CPY",	"CPA",	"DEC",	"DCP*",	"INY",	"CPA",	"DEX",	"AXS*",	"CPY",	"CPA",	"DEC",	"DCP*", //C
		"BNE",	"CPA",	"",		"DCP*",	"NOP",	"CPA",	"DEC",	"DCP*",	"CLD",	"CPA",	"NOP*",	"DCP*",	"NOP",	"CPA",	"DEC",	"DCP*", //D
		"CPX",	"SBC",	"NOP",	"ISB*",	"CPX",	"SBC",	"INC",	"ISB*",	"INX",	"SBC",	"NOP",	"SBC*",	"CPX",	"SBC",	"INC",	"ISB*", //E
		"BEQ",	"SBC",	"",		"ISB*",	"NOP",	"SBC",	"INC",	"ISB*",	"SED",	"SBC",	"NOP*",	"ISB*",	"NOP",	"SBC",	"INC",	"ISB*"  //F
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
		DisassemblyInfo::OPName[i] = opName[i];
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

Disassembler::~Disassembler()
{
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
	uint32_t addr;
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

vector<string> Disassembler::SplitComment(string input)
{
	vector<string> result;
	size_t index;
	while((index = input.find('\n')) != string::npos) {
		result.push_back(input.substr(0, index));
		input = input.substr(index + 1, input.size() - index - 1);
	}
	result.push_back(input);
	return result;
}

string Disassembler::GetLine(string code, string comment, int32_t cpuAddress, int32_t absoluteAddress, string byteCode, string addressing)
{
	string out;
	out.reserve(100);
	if(cpuAddress >= 0) {
		out += (_debugger->IsMarkedAsCode(cpuAddress) || absoluteAddress == -1) ? "1\x1" : "0\x1";
		out += HexUtilities::ToHex((uint16_t)cpuAddress);		
	} else {
		out += "1\x1";
	}
	out += "\x1";
	if(absoluteAddress >= 0) {
		out += HexUtilities::ToHex((uint32_t)absoluteAddress);
	}
	out += "\x1" + byteCode + "\x1" + code + "\x2" + addressing + "\x2" + (comment.empty() ? "" : (";" + comment)) + "\n";
	return out;
}

string Disassembler::GetSubHeader(DisassemblyInfo *info, string &label, uint16_t relativeAddr, uint16_t resetVector, uint16_t nmiVector, uint16_t irqVector)
{
	if(info->IsSubEntryPoint()) {
		if(label.empty()) {
			return GetLine() + GetLine("__sub start__");
		} else {
			return GetLine() + GetLine("__" + label + "()__");
		}
	} else if(relativeAddr == resetVector) {
		return GetLine() + GetLine("--reset--");
	} else if(relativeAddr == irqVector) {
		return GetLine() + GetLine("--irq--");
	} else if(relativeAddr == nmiVector) {
		return GetLine() + GetLine("--nmi--");
	}
	return "";
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
	while(addr <= endAddr) {
		string label = labelManager->GetLabel(memoryAddr, false);
		string commentString = labelManager->GetComment(memoryAddr);
		string labelLine = label.empty() ? "" : GetLine(label + ":");
		string commentLines = "";
		if(commentString.find_first_of('\n') != string::npos) {
			for(string &str : SplitComment(commentString)) {
				commentLines += GetLine("", str);
			}
			commentString = "";
		}

		shared_ptr<DisassemblyInfo> info = (*cache)[addr&mask];

		if(info) {
			if(byteCount > 0) {
				output += GetLine(dbBuffer, "", dbRelativeAddr, dbAbsoluteAddr);
				byteCount = 0;
			} 
			
			if(skippingCode) {
				output += GetLine(unknownBlockHeader, "", (uint16_t)(memoryAddr - 1), addr - 1);
				skippingCode = false;
			}

			output += GetSubHeader(info.get(), label, memoryAddr, resetVector, nmiVector, irqVector);
			output += commentLines;
			output += labelLine;

			string effectiveAddress = showEffectiveAddresses ? info->GetEffectiveAddressString(cpuState, memoryManager, labelManager) : "";
			output += GetLine("  " + info->ToString(memoryAddr, memoryManager, labelManager), commentString, memoryAddr, addr, info->GetByteCode(), effectiveAddress);

			if(info->IsSubExitPoint()) {
				output += GetLine("__sub end__") + GetLine();
			}

			addr += info->GetSize();
			memoryAddr += info->GetSize();
		} else {
			if((!label.empty() || !commentString.empty()) && skippingCode) {
				output += GetLine(unknownBlockHeader, "", (uint16_t)(memoryAddr - 1), addr - 1);
				skippingCode = false;
			} 
			
			if(!skippingCode && showOnlyDiassembledCode) {
				if(label.empty()) {
					output += GetLine("__unknown block__", "", memoryAddr, addr);
					if(!commentString.empty()) {
						output += GetLine("", commentString);
					}
				} else {
					output += GetLine("__" + label + "__", "", memoryAddr, addr);
					if(!commentString.empty()) {
						output += GetLine("", commentString);
					}
					output += commentLines;
				}
				skippingCode = true;
			}

			if(!showOnlyDiassembledCode) {
				if(byteCount >= 8 || ((!label.empty() || !commentString.empty()) && byteCount > 0)) {
					output += GetLine(dbBuffer, "", dbRelativeAddr, dbAbsoluteAddr);
					byteCount = 0;
				}

				if(byteCount == 0) {
					dbBuffer = ".db";
					output += commentLines;
					output += labelLine;

					dbRelativeAddr = memoryAddr;
					dbAbsoluteAddr = addr;
				}

				dbBuffer += " $" + HexUtilities::ToHex(source[addr&mask]);

				if(!label.empty() || !commentString.empty()) {
					output += GetLine(dbBuffer, commentString, dbRelativeAddr, dbAbsoluteAddr);
					byteCount = 0;
				} else {
					byteCount++;
				}
			}
			addr++;
			memoryAddr++;
		}
	}

	if(byteCount > 0) {
		output += GetLine(dbBuffer, "", dbRelativeAddr, dbAbsoluteAddr);
	}

	if(skippingCode) {
		output += GetLine("----", "", (uint16_t)(memoryAddr - 1), addr - 1);
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