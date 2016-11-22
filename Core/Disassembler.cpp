#include "stdafx.h"
#include "Disassembler.h"
#include "DisassemblyInfo.h"
#include "BaseMapper.h"
#include "MemoryManager.h"
#include "CPU.h"

Disassembler::Disassembler(uint8_t* internalRam, uint8_t* prgRom, uint32_t prgSize, uint8_t* prgRam, uint32_t prgRamSize)
{
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

string Disassembler::GetCode(uint32_t startAddr, uint32_t endAddr, uint16_t memoryAddr, PrgMemoryType memoryType, bool showEffectiveAddresses, bool showOnlyDiassembledCode, State& cpuState, shared_ptr<MemoryManager> memoryManager, unordered_map<uint32_t, string> &codeLabels, unordered_map<uint32_t, string> &codeComments) {
	std::ostringstream output;

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

	uint32_t addr = startAddr;
	uint32_t byteCount = 0;
	bool skippingCode = false;
	while(addr <= endAddr) {
		auto labelSearch = codeLabels.find(addr);
		auto commentSearch = codeComments.find(addr);
		string label = labelSearch != codeLabels.end() ? labelSearch->second : "";
		string labelString = label.empty() ? "" : ("\x1\x1\x1" + labelSearch->second + ":\n");
		string commentString = commentSearch != codeComments.end() ? commentSearch->second : "";
		bool multilineComment = commentString.find_first_of('\n') != string::npos;
		string singleLineComment = "";
		string multiLineComment = "";
		if(multilineComment) {
			for(string &str : SplitComment(commentString)) {
				multiLineComment += "\x1\x1\x1\x2\x2;" + str + "\n";
			}
		} else if(!commentString.empty()) {
			singleLineComment = "\x2;" + commentString;
		}

		shared_ptr<DisassemblyInfo> info = (*cache)[addr&mask];
		if(info) {
			if(byteCount > 0) {
				output << "\n";
				byteCount = 0;
			} 
			
			if(skippingCode) {
				output << std::hex << std::uppercase << (memoryAddr - 1) << "\x1" << (addr - 1) << "\x1\x1";
				if(showOnlyDiassembledCode) {
					output << "----\n";
				} else {
					output << "__unknown block__\n";
				}
				skippingCode = false;
			}

			string effectiveAddress = showEffectiveAddresses ? info->GetEffectiveAddressString(cpuState, memoryManager, &codeLabels) : "";

			if(info->IsSubEntryPoint()) {
				if(label.empty()) {
					output << "\x1\x1\x1\n\x1\x1\x1--sub start--\n";
				} else {
					output << "\x1\x1\x1\n\x1\x1\x1--" + label + "()--\n";
				}
			} else if(memoryAddr == resetVector) {
				output << "\x1\x1\x1\n\x1\x1\x1--reset--\n";
			} else if(memoryAddr == irqVector) {
				output << "\x1\x1\x1\n\x1\x1\x1--irq--\n";
			} else if(memoryAddr == nmiVector) {
				output << "\x1\x1\x1\n\x1\x1\x1--nmi--\n";
			}

			output << multiLineComment;
			output << labelString;
			output << std::hex << std::uppercase << memoryAddr << "\x1" << addr << "\x1" << info->GetByteCode() << "\x1  " << info->ToString(memoryAddr, memoryManager, &codeLabels) << "\x2" << effectiveAddress;
			output << singleLineComment;

			output << "\n";

			if(info->IsSubExitPoint()) {
				output << "\x1\x1\x1__sub end__\n\x1\x1\x1\n";
			}

			addr += info->GetSize();
			memoryAddr += info->GetSize();
		} else {
			if(!skippingCode) {
				output << std::hex << std::uppercase << memoryAddr << "\x1" << addr << "\x1\x1";
				if(showOnlyDiassembledCode) {
					output << "____\n\x1\x1\x1";
					if(label.empty()) {
						output << "[[unknown block]]\n";
					} else {
						output << "[[" << label << "]]\n";
						if(!singleLineComment.empty()) {
							output << "\x1\x1\x1" << singleLineComment << "\n";
						} else {
							output << multiLineComment;
						}
					}
				} else {
					output << "--unknown block--\n";
				}
				skippingCode = true;
			}

			if(!showOnlyDiassembledCode) {
				if(byteCount >= 8 || !label.empty() || !commentString.empty()) {
					output << "\n";
					byteCount = 0;
				}
				if(byteCount == 0) {
					output << multiLineComment;
					output << labelString;
					output << std::hex << std::uppercase << memoryAddr << "\x1" << addr << "\x1\x1" << ".db";
					output << singleLineComment;

					if(!label.empty() || !commentString.empty()) {
						byteCount = 7;
					}
				}
				output << std::hex << " $" << std::setfill('0') << std::setw(2) << (short)source[addr&mask];

				byteCount++;
			}
			addr++;
			memoryAddr++;
		}
	}

	if(skippingCode) {
		if(byteCount != 0) {
			output << "\n";
		}

		output << std::hex << std::uppercase << (memoryAddr - 1) << "\x1" << (addr - 1) << "\x1\x1";
		if(showOnlyDiassembledCode) {
			output << "----\n";
		} else {
			output << "__unknown block__\n";
		}
	}

	output << "\n";
		
	return output.str();
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