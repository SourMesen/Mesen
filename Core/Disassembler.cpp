#include "stdafx.h"
#include "Disassembler.h"
#include "DisassemblyInfo.h"
#include "BaseMapper.h"

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
			if(!cache[absoluteAddr]) {
				while(absoluteAddr < (int32_t)_prgSize && !cache[absoluteAddr]) {
					shared_ptr<DisassemblyInfo> disInfo(new DisassemblyInfo(&source[absoluteAddr], isSubEntryPoint));
					isSubEntryPoint = false;
					cache[absoluteAddr] = disInfo;

					uint8_t opCode = source[absoluteAddr];
					absoluteAddr += disInfo->GetSize();
					if(opCode == 0x10 || opCode == 0x20 || opCode == 0x30 || opCode == 0x40 || opCode == 0x50 || opCode == 0x60 || opCode == 0x70 || opCode == 0x90 || opCode == 0xB0 || opCode == 0xD0 || opCode == 0xF0 || opCode == 0x4C || opCode == 0x6C) {
						//Hit a jump/return instruction, can't assume that what follows is actual code, stop disassembling
						break;
					}
				}
			} else {
				if(isSubEntryPoint) {
					cache[absoluteAddr]->SetSubEntryPoint();
				}
				absoluteAddr += cache[absoluteAddr]->GetSize();
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

string Disassembler::GetCode(uint32_t startAddr, uint32_t endAddr, uint16_t memoryAddr, PrgMemoryType memoryType)
{
	std::ostringstream output;
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
	while(addr <= endAddr) {
		shared_ptr<DisassemblyInfo> info;
		if(info = (*cache)[addr&mask]) {
			if(byteCount > 0) {
				output << "\n";
				byteCount = 0;
			}
			output << std::hex << std::uppercase << memoryAddr << ":" << addr << ":" << info->ToString(memoryAddr) << "\n";
			addr += info->GetSize();
			memoryAddr += info->GetSize();
		} else {
			if(byteCount >= 8) {
				output << "\n";
				byteCount = 0;
			}
			if(byteCount == 0) {
				output << std::hex << std::uppercase << memoryAddr << ":" << addr << "::" << ".db";
			}
			output << std::hex << " $" << std::setfill('0') << std::setw(2) << (short)source[addr&mask];

			byteCount++;
			addr++;
			memoryAddr++;
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