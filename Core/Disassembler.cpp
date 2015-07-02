#include "stdafx.h"
#include "Disassembler.h"
#include "DisassemblyInfo.h"
#include "CPU.h"

Disassembler::Disassembler(uint8_t* internalRAM, uint8_t* prgROM, uint32_t prgSize)
{
	_internalRAM = internalRAM;
	_prgROM = prgROM;
	_prgSize = prgSize;
	for(uint32_t i = 0; i < prgSize; i++) {
		_disassembleCache.push_back(shared_ptr<DisassemblyInfo>(nullptr));
	}
	for(uint32_t i = 0; i < 0x2000; i++) {
		_disassembleMemoryCache.push_back(shared_ptr<DisassemblyInfo>(nullptr));
	}


	string opName[256] = {
	//	0			1			2			3			4			5			6			7			8			9			A					B			C					D			E				F
		"BRK",	"ORA",	"",		"",		"NOP",	"ORA",	"ASL",	"",		"PHP",	"ORA",	"ASL",			"",		"NOP",			"ORA",	"ASL",		"", //0
		"BPL",	"ORA",	"",		"",		"NOP",	"ORA",	"ASL",	"",		"CLC",	"ORA",	"",				"",		"NOP",			"ORA",	"ASL",		"", //1
		"JSR",	"AND",	"",		"",		"BIT",	"AND",	"ROL",	"",		"PLP",	"AND",	"ROL",			"",		"BIT",			"AND",	"ROL",		"", //2
		"BMI",	"AND",	"",		"",		"NOP",	"AND",	"ROL",	"",		"SEC",	"AND",	"",				"",		"NOP",			"AND",	"ROL",		"", //3
		"RTI",	"EOR",	"",		"",		"NOP",	"EOR",	"LSR",	"",		"PHA",	"EOR",	"LSR",			"",		"JMP",			"EOR",	"LSR",		"", //4
		"BVC",	"EOR",	"",		"",		"NOP",	"EOR",	"LSR",	"",		"CLI",	"EOR",	"",				"",		"NOP",			"EOR",	"LSR",		"", //5
		"RTS",	"ADC",	"",		"",		"NOP",	"ADC",	"ROR",	"",		"PLA",	"ADC",	"ROR",			"",		"JMP",			"ADC",	"ROR",		"", //6
		"BVS",	"ADC",	"",		"",		"NOP",	"ADC",	"ROR",	"",		"SEI",	"ADC",	"",				"",		"NOP",			"ADC",	"ROR",		"", //7
		"NOP",	"STA",	"NOP",	"",		"STY",	"STA",	"STX",	"",		"DEY",	"NOP",	"TXA",			"",		"STY",			"STA",	"STX",		"", //8
		"BCC",	"STA",	"",		"",		"STY",	"STA",	"STX",	"",		"TYA",	"STA",	"TXS",			"",		"",				"STA",	"",			"", //9
		"LDY",	"LDA",	"LDX",	"",		"LDY",	"LDA",	"LDX",	"",		"TAY",	"LDA",	"TAX",			"",		"LDY",			"LDA",	"LDX",		"", //A
		"BCS",	"LDA",	"",		"",		"LDY",	"LDA",	"LDX",	"",		"CLV",	"LDA",	"TSX",			"",		"LDY",			"LDA",	"LDX",		"", //B
		"CPY",	"CPA",	"NOP",	"",		"CPY",	"CPA",	"DEC",	"",		"INY",	"CPA",	"DEX",			"",		"CPY",			"CPA",	"DEC",		"", //C
		"BNE",	"CPA",	"",		"",		"NOP",	"CPA",	"DEC",	"",		"CLD",	"CPA",	"",				"",		"NOP",			"CPA",	"DEC",		"", //D
		"CPX",	"SBC",	"NOP",	"",		"CPX",	"SBC",	"INC",	"",		"INX",	"SBC",	"NOP",			"",		"CPX",			"SBC",	"INC",		"", //E
		"BEQ",	"SBC",	"",		"",		"NOP",	"SBC",	"INC",	"",		"SED",	"SBC",	"",				"",		"NOP",			"SBC",	"INC",		""  //F
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
	if(_prgROM) {
		delete[] _prgROM;
	}
}

void Disassembler::BuildCache(uint32_t absoluteAddr, uint16_t memoryAddr)
{
	if(memoryAddr < 0x2000) {
		memoryAddr = memoryAddr & 0x7FF;
		if(!_disassembleMemoryCache[memoryAddr]) {
			shared_ptr<DisassemblyInfo> disInfo(new DisassemblyInfo(&_internalRAM[memoryAddr]));
			_disassembleMemoryCache[memoryAddr] = disInfo;
		}
	} else {
		while(!_disassembleCache[absoluteAddr]) {
			shared_ptr<DisassemblyInfo> disInfo(new DisassemblyInfo(&_prgROM[absoluteAddr]));
			_disassembleCache[absoluteAddr] = disInfo;

			uint8_t opCode = _prgROM[absoluteAddr];

			if(opCode == 0x10 || opCode == 0x20 || opCode == 0x30 || opCode == 0x40 || opCode == 0x50 || opCode == 0x60 || opCode == 0x70 || opCode == 0x90 || opCode == 0xB0 || opCode == 0xD0 || opCode == 0xF0 || opCode == 0x4C || opCode == 0x6C) {
				//Hit a jump/return instruction, can't assume that what follows is actual code, stop disassembling
				break;
			}

			absoluteAddr += disInfo->GetSize();
			memoryAddr += disInfo->GetSize();
		}
	}
}

string Disassembler::GetRAMCode()
{
	std::ostringstream output;

	uint32_t addr = 0x0000;
	uint32_t byteCount = 0;
	while(addr < 0x2000) {
		shared_ptr<DisassemblyInfo> info;
		if(info = _disassembleMemoryCache[addr&0x7FF]) {
			if(byteCount > 0) {
				output << "\n";
				byteCount = 0;
			}
			output << std::hex << std::uppercase << addr << ":" << info->ToString(addr) << "\n";
			addr += info->GetSize();
		} else {
			if(byteCount >= 8) {
				output << "\n";
				byteCount = 0;
			}
			if(byteCount == 0) {
				output << std::hex << std::uppercase << addr << ":" << ".db";
			}
			output << std::hex << " $" << std::setfill('0') << std::setw(2) << (short)_internalRAM[addr];

			byteCount++;
			addr++;
		}
	}
	
	output << "\n1FFF:--END OF INTERNAL RAM--\n";
		
	return output.str();
}

string Disassembler::GetCode(uint32_t startAddr, uint32_t endAddr, uint16_t &memoryAddr)
{
	std::ostringstream output;

	uint32_t addr = startAddr;
	uint32_t byteCount = 0;
	while(addr < endAddr) {
		shared_ptr<DisassemblyInfo> info;
		if(info = _disassembleCache[addr]) {
			if(byteCount > 0) {
				output << "\n";
				byteCount = 0;
			}
			output << std::hex << std::uppercase << memoryAddr << ":" << info->ToString(memoryAddr) << "\n";
			addr += info->GetSize();
			memoryAddr += info->GetSize();
		} else {
			if(byteCount >= 8) {
				output << "\n";
				byteCount = 0;
			}
			if(byteCount == 0) {
				output << std::hex << std::uppercase << memoryAddr << ":" << ".db";
			}
			output << std::hex << " $" << std::setfill('0') << std::setw(2) << (short)_prgROM[addr];

			byteCount++;
			addr++;
			memoryAddr++;
		}
	}
	
	output << "\n";
		
	return output.str();
}
