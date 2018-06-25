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

Disassembler::Disassembler(MemoryManager* memoryManager, BaseMapper* mapper, Debugger* debugger)
{
	_debugger = debugger;
	_memoryManager = memoryManager;
	_mapper = mapper;

	BuildOpCodeTables(false);
}

Disassembler::~Disassembler()
{
}

void Disassembler::Reset()
{
	_disassembleCache.clear();
	_disassembleWorkRamCache.clear();
	_disassembleSaveRamCache.clear();
	_disassembleMemoryCache.clear();

	_disassembleCache.insert(_disassembleCache.end(), _mapper->GetMemorySize(DebugMemoryType::PrgRom), shared_ptr<DisassemblyInfo>(nullptr));
	_disassembleWorkRamCache.insert(_disassembleWorkRamCache.end(), _mapper->GetMemorySize(DebugMemoryType::WorkRam), shared_ptr<DisassemblyInfo>(nullptr));
	_disassembleSaveRamCache.insert(_disassembleSaveRamCache.end(), _mapper->GetMemorySize(DebugMemoryType::SaveRam), shared_ptr<DisassemblyInfo>(nullptr));
	_disassembleMemoryCache.insert(_disassembleMemoryCache.end(), 0x800, shared_ptr<DisassemblyInfo>(nullptr));
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

	typedef AddrMode M;
	AddrMode opMode[] = {
		//	0			1				2			3				4				5				6				7				8			9			A			B			C			D			E			F
		M::Imp,	M::IndX,		M::None,	M::IndX,		M::Zero,		M::Zero,		M::Zero,		M::Zero,		M::Imp,	M::Imm,	M::Acc,	M::Imm,	M::Abs,	M::Abs,	M::Abs,	M::Abs,	//0
		M::Rel,	M::IndY,		M::None,	M::IndYW,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::Imp,	M::AbsY,	M::Imp,	M::AbsYW,M::AbsX,	M::AbsX,	M::AbsXW,M::AbsXW,//1
		M::Abs,	M::IndX,		M::None,	M::IndX,		M::Zero,		M::Zero,		M::Zero,		M::Zero,		M::Imp,	M::Imm,	M::Acc,	M::Imm,	M::Abs,	M::Abs,	M::Abs,	M::Abs,	//2
		M::Rel,	M::IndY,		M::None,	M::IndYW,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::Imp,	M::AbsY,	M::Imp,	M::AbsYW,M::AbsX,	M::AbsX,	M::AbsXW,M::AbsXW,//3
		M::Imp,	M::IndX,		M::None,	M::IndX,		M::Zero,		M::Zero,		M::Zero,		M::Zero,		M::Imp,	M::Imm,	M::Acc,	M::Imm,	M::Abs,	M::Abs,	M::Abs,	M::Abs,	//4
		M::Rel,	M::IndY,		M::None,	M::IndYW,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::Imp,	M::AbsY,	M::Imp,	M::AbsYW,M::AbsX,	M::AbsX,	M::AbsXW,M::AbsXW,//5
		M::Imp,	M::IndX,		M::None,	M::IndX,		M::Zero,		M::Zero,		M::Zero,		M::Zero,		M::Imp,	M::Imm,	M::Acc,	M::Imm,	M::Ind,	M::Abs,	M::Abs,	M::Abs,	//6
		M::Rel,	M::IndY,		M::None,	M::IndYW,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::Imp,	M::AbsY,	M::Imp,	M::AbsYW,M::AbsX,	M::AbsX,	M::AbsXW,M::AbsXW,//7
		M::Imm,	M::IndX,		M::Imm,	M::IndX,		M::Zero,		M::Zero,		M::Zero,		M::Zero,		M::Imp,	M::Imm,	M::Imp,	M::Imm,	M::Abs,	M::Abs,	M::Abs,	M::Abs,	//8
		M::Rel,	M::IndYW,	M::None,	M::IndYW,	M::ZeroX,	M::ZeroX,	M::ZeroY,	M::ZeroY,	M::Imp,	M::AbsYW,M::Imp,	M::AbsYW,M::AbsXW,M::AbsXW,M::AbsYW,M::AbsYW,//9
		M::Imm,	M::IndX,		M::Imm,	M::IndX,		M::Zero,		M::Zero,		M::Zero,		M::Zero,		M::Imp,	M::Imm,	M::Imp,	M::Imm,	M::Abs,	M::Abs,	M::Abs,	M::Abs,	//A
		M::Rel,	M::IndY,		M::None,	M::IndY,		M::ZeroX,	M::ZeroX,	M::ZeroY,	M::ZeroY,	M::Imp,	M::AbsY,	M::Imp,	M::AbsY,	M::AbsX,	M::AbsX,	M::AbsY,	M::AbsY,	//B
		M::Imm,	M::IndX,		M::Imm,	M::IndX,		M::Zero,		M::Zero,		M::Zero,		M::Zero,		M::Imp,	M::Imm,	M::Imp,	M::Imm,	M::Abs,	M::Abs,	M::Abs,	M::Abs,	//C
		M::Rel,	M::IndY,		M::None,	M::IndYW,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::Imp,	M::AbsY,	M::Imp,	M::AbsYW,M::AbsX,	M::AbsX,	M::AbsXW,M::AbsXW,//D
		M::Imm,	M::IndX,		M::Imm,	M::IndX,		M::Zero,		M::Zero,		M::Zero,		M::Zero,		M::Imp,	M::Imm,	M::Imp,	M::Imm,	M::Abs,	M::Abs,	M::Abs,	M::Abs,	//E
		M::Rel,	M::IndY,		M::None,	M::IndYW,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::ZeroX,	M::Imp,	M::AbsY,	M::Imp,	M::AbsYW,M::AbsX,	M::AbsX,	M::AbsXW,M::AbsXW,//F
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

void Disassembler::GetInfo(AddressTypeInfo &info, uint8_t** source, uint32_t &size, vector<shared_ptr<DisassemblyInfo>> **cache)
{
	switch(info.Type) {
		case AddressType::Register:
			//AddressType::Register should never be used here
			break;

		case AddressType::InternalRam: 
			*source = _memoryManager->GetInternalRAM();
			*cache = &_disassembleMemoryCache;
			size = 0x800;
			break;

		case AddressType::PrgRom: 
			*source = _mapper->GetPrgRom();
			*cache = &_disassembleCache;
			size = _mapper->GetMemorySize(DebugMemoryType::PrgRom);
			break;

		case AddressType::WorkRam: 
			*source = _mapper->GetWorkRam();
			*cache = &_disassembleWorkRamCache;
			size = _mapper->GetMemorySize(DebugMemoryType::WorkRam);
			break;

		case AddressType::SaveRam: 
			*source = _mapper->GetSaveRam();
			*cache = &_disassembleSaveRamCache;
			size = _mapper->GetMemorySize(DebugMemoryType::SaveRam);
			break;
	}
}


uint32_t Disassembler::BuildCache(AddressTypeInfo &info, uint16_t cpuAddress, bool isSubEntryPoint)
{
	if(info.Type == AddressType::InternalRam) {
		uint16_t memoryAddr = info.Address & 0x7FF;
		if(!_disassembleMemoryCache[memoryAddr]) {
			shared_ptr<DisassemblyInfo> disInfo(new DisassemblyInfo(_memoryManager->GetInternalRAM()+memoryAddr, isSubEntryPoint));
			_disassembleMemoryCache[memoryAddr] = disInfo;
			memoryAddr += disInfo->GetSize();
		} else if(isSubEntryPoint) {
			_disassembleMemoryCache[memoryAddr]->SetSubEntryPoint();
		}
		return memoryAddr;
	} else {
		vector<shared_ptr<DisassemblyInfo>> *cache;
		uint8_t *source;
		uint32_t size;
		GetInfo(info, &source, size, &cache);
		int32_t absoluteAddr = info.Address;

		if(info.Address >= 0) {
			DisassemblyInfo *disInfo = (*cache)[info.Address].get();
			if(!disInfo) {
				while(absoluteAddr < (int32_t)size && !(*cache)[absoluteAddr]) {
					bool isJump = IsUnconditionalJump(source[absoluteAddr]);
					disInfo = new DisassemblyInfo(source+absoluteAddr, isSubEntryPoint);
					isSubEntryPoint = false;

					(*cache)[absoluteAddr] = shared_ptr<DisassemblyInfo>(disInfo);

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

				uint8_t opCode = source[info.Address];
				if(IsJump(opCode)) {
					uint16_t jumpDest = disInfo->GetOpAddr(cpuAddress);
					if(jumpDest != cpuAddress) {
						AddressTypeInfo addressInfo;
						_debugger->GetAbsoluteAddressAndType(jumpDest, &addressInfo);

						const uint8_t jsrCode = 0x20;
						if(addressInfo.Address >= 0) {
							BuildCache(addressInfo, jumpDest, opCode == jsrCode);
						}
					}
				}

				absoluteAddr += disInfo->GetSize();
			}
		}
		return absoluteAddr;
	}
}

void Disassembler::InvalidateCache(AddressTypeInfo &info)
{
	int32_t addr;
	vector<shared_ptr<DisassemblyInfo>> *cache = nullptr;

	switch(info.Type) {
		case AddressType::InternalRam:
			addr = info.Address & 0x7FF;
			cache = &_disassembleMemoryCache;
			break;

		case AddressType::WorkRam:
			addr = info.Address;
			cache = &_disassembleWorkRamCache;
			break;

		case AddressType::SaveRam:
			addr = info.Address;
			cache = &_disassembleSaveRamCache;
			break;

		default:
			//No need to invalidate PRG ROM cache (since it's not RAM)
			break;
	}

	if(cache && addr >= 0) {
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
	AddressTypeInfo info = { (int32_t)absoluteAddr, AddressType::PrgRom };
	BuildCache(info, memoryAddr, isSubEntryPoint);
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
void Disassembler::GetLine(string &out, string code, string comment, int32_t cpuAddress, int32_t absoluteAddress, DataType dataType, char memoryType)
{
	GetCodeLine(out, code, comment, cpuAddress, absoluteAddress, emptyString, emptyString, dataType, false, memoryType);
}

void Disassembler::GetCodeLine(string &out, string &code, string &comment, int32_t cpuAddress, int32_t absoluteAddress, string &byteCode, string &addressing, DataType dataType, bool isIndented, char memoryType)
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
	//
	//Flags:
	//1: Executed code
	//2: Unidentified code/data
	//4: Indented line
	//8: Verified data
	if(cpuAddress >= 0) {
		if(dataType == DataType::UnidentifiedData) {
			writeChar(isIndented ? '6' : '2');
			writeChar('\x1');
		} else if(dataType == DataType::VerifiedData) {
			writeChar(isIndented ? '9' : '8');
			writeChar('\x1');
		} else {
			writeChar((_debugger->IsMarkedAsCode(cpuAddress) || absoluteAddress == -1) ? (isIndented ? '5' : '1') : (isIndented ? '4' : '0'));
			writeChar('\x1');
		}
		writeHex(hexTable[(cpuAddress >> 8) & 0xFF]);
		writeHex(hexTable[cpuAddress & 0xFF]);
		writeChar('\x1');
	} else {
		if(dataType == DataType::VerifiedData) {
			writeChar('8');
		} else {
			writeChar(dataType == DataType::UnidentifiedData ? '2' : '0');
		}
		writeChar('\x1');
		writeChar('\x1');
	}

	writeChar(memoryType);
	writeChar('\x1');

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
			GetLine(out, "__sub start__");
		} else {
			GetLine(out, "__" + label + "()__");
		}
	} else if(relativeAddr == resetVector) {
		GetLine(out, "__reset__");
	} else if(relativeAddr == irqVector) {
		GetLine(out, "__irq__");
	} else if(relativeAddr == nmiVector) {
		GetLine(out, "__nmi__");
	}
}

string Disassembler::GetCode(AddressTypeInfo &addressInfo, uint32_t endAddr, uint16_t memoryAddr, State& cpuState, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager) 
{
	string output;
	output.reserve(10000000);

	int32_t dbRelativeAddr = 0;
	int32_t dbAbsoluteAddr = 0;
	string dbBuffer;

	bool showEffectiveAddresses = _debugger->CheckFlag(DebuggerFlags::ShowEffectiveAddresses);
	bool disassembleVerifiedData = _debugger->CheckFlag(DebuggerFlags::DisassembleVerifiedData);
	bool disassembleUnidentifiedData = _debugger->CheckFlag(DebuggerFlags::DisassembleUnidentifiedData);
	bool showUnidentifiedData = _debugger->CheckFlag(DebuggerFlags::ShowUnidentifiedData);
	bool showVerifiedData = _debugger->CheckFlag(DebuggerFlags::ShowVerifiedData);

	uint16_t resetVector = memoryManager->DebugReadWord(CPU::ResetVector);
	uint16_t nmiVector = memoryManager->DebugReadWord(CPU::NMIVector);
	uint16_t irqVector = memoryManager->DebugReadWord(CPU::IRQVector);

	vector<shared_ptr<DisassemblyInfo>> *cache;
	uint8_t *source;
	char memoryType = 'P';
	switch(addressInfo.Type) {
		case AddressType::Register: break; //Should never happen
		case AddressType::InternalRam: memoryType = 'N'; break;
		case AddressType::PrgRom: memoryType = 'P'; break;
		case AddressType::WorkRam: memoryType = 'W'; break;
		case AddressType::SaveRam: memoryType = 'S'; break;
	}
	uint32_t mask = addressInfo.Type == AddressType::InternalRam ? 0x7FF : 0xFFFFFFFF;
	uint32_t size;

	GetInfo(addressInfo, &source, size, &cache);

	uint32_t addr = addressInfo.Address;
	uint32_t byteCount = 0;
	bool insideDataBlock = false;
	shared_ptr<CodeDataLogger> cdl = _debugger->GetCodeDataLogger();
	string label;
	string commentString;
	string commentLines;
	shared_ptr<DisassemblyInfo> infoRef;
	DisassemblyInfo* info;
	DataType dataType = DataType::UnidentifiedData;
	string spaces = "  ";
	string effAddress;
	string code;
	string byteCode;
	bool isVerifiedData;
	bool inVerifiedDataBlock = false;
	bool emptyBlock = false;
	bool showEitherDataType = showVerifiedData || showUnidentifiedData;
	
	auto outputBytes = [this, &showEitherDataType, &addressInfo, &inVerifiedDataBlock, &output, &dbBuffer, &dbRelativeAddr, &dbAbsoluteAddr, &byteCount, memoryType]() {
		DataType type = addressInfo.Type == AddressType::PrgRom ? (showEitherDataType && inVerifiedDataBlock ? DataType::VerifiedData : DataType::UnidentifiedData) : DataType::VerifiedCode;
		if(byteCount > 0) {
			GetLine(output, dbBuffer, "", dbRelativeAddr, dbAbsoluteAddr, type, memoryType);
			byteCount = 0;
		}
	};

	auto endDataBlock = [this, outputBytes, &addressInfo, &showEitherDataType, &inVerifiedDataBlock, &emptyBlock, &output, &addr, &insideDataBlock, &memoryAddr, memoryType]() {
		outputBytes();
		DataType type = addressInfo.Type == AddressType::PrgRom ? (showEitherDataType && inVerifiedDataBlock ? DataType::VerifiedData : DataType::UnidentifiedData) : DataType::VerifiedCode;
		if(emptyBlock) {
			GetLine(output, "", "", -1, -1, type, memoryType);
		}
		GetLine(output, "----", "", emptyBlock ? (uint16_t)(memoryAddr - 1) : -1, emptyBlock ? addr - 1 : -1, type, memoryType);
		insideDataBlock = false;
	};

	while(addr <= endAddr) {
		labelManager->GetLabelAndComment(memoryAddr, label, commentString);
		commentLines.clear();
		
		if(commentString.find_first_of('\n') != string::npos) {
			for(string &str : StringUtilities::Split(commentString, '\n')) {
				GetLine(commentLines, "", str, -1, -1, dataType, memoryType);
			}
			commentString.clear();
		}
		
		infoRef = (*cache)[addr&mask];

		isVerifiedData = addressInfo.Type == AddressType::PrgRom && cdl->IsData(addr&mask);
		info = infoRef.get();
		if(!info && ((disassembleUnidentifiedData && !isVerifiedData) || (disassembleVerifiedData && isVerifiedData))) {
			dataType = isVerifiedData ? DataType::VerifiedData : (addressInfo.Type == AddressType::PrgRom ? DataType::UnidentifiedData : DataType::VerifiedCode);
			info = new DisassemblyInfo(source + (addr & mask), false);
		} else if(info) {
			dataType = DataType::VerifiedCode;
		}
		
		if(info && addr + info->GetSize() <= endAddr + 1) {
			if(insideDataBlock) {
				endDataBlock();
			}

			GetSubHeader(output, info, label, memoryAddr, resetVector, nmiVector, irqVector);
			output += commentLines;
			if(!label.empty()) {
				GetLine(output, label + ":", emptyString, -1, -1, dataType, memoryType);
			}
			
			byteCode.clear();
			code.clear();
			effAddress.clear();
			if(showEffectiveAddresses) {
				info->GetEffectiveAddressString(effAddress, cpuState, memoryManager.get(), labelManager.get());
			}
			info->ToString(code, memoryAddr, memoryManager.get(), labelManager.get(), false);
			info->GetByteCode(byteCode);

			GetCodeLine(output, code, commentString, memoryAddr, addr & mask, byteCode, effAddress, dataType, true, memoryType);

			if(info->IsSubExitPoint()) {
				GetLine(output, "----");
			}

			if(dataType == DataType::UnidentifiedData) {
				//For unverified code, check if a verified instruction starts between the start of this instruction and its end.
				//If so, we need to realign the disassembler to the start of the next verified instruction
				for(uint32_t i = 0; i < info->GetSize(); i++) {
					addr++;
					memoryAddr++;
					if(addr > endAddr || (*cache)[addr&mask] || (addressInfo.Type == AddressType::PrgRom && cdl->IsData(addr))) {
						//Verified code or verified data found, stop incrementing address counters
						break;
					}
				}		
				delete info;
			} else {
				addr += info->GetSize();
				memoryAddr += info->GetSize();
			}
		} else {
			//This byte should be interpreted as data
			bool showData = (isVerifiedData && showVerifiedData) || (!isVerifiedData && showUnidentifiedData);

			if(inVerifiedDataBlock != isVerifiedData && insideDataBlock && showEitherDataType) {
				//End of block (switching between verified data & unidentified data, while only either of them is set to be displayed)
				endDataBlock();
			} else if(inVerifiedDataBlock != isVerifiedData) {
				outputBytes();
			}

			inVerifiedDataBlock = isVerifiedData;
			dataType = addressInfo.Type == AddressType::PrgRom ? (showEitherDataType && inVerifiedDataBlock ? DataType::VerifiedData : DataType::UnidentifiedData) : DataType::VerifiedCode;

			if(!insideDataBlock) {
				//Output block header 
				if(label.empty()) {
					string blockName;
					if(addressInfo.Type == AddressType::InternalRam) {
						blockName = "__NES RAM__";
					} else if(addressInfo.Type == AddressType::SaveRam) {
						blockName = "__Save RAM__";
					} else if(addressInfo.Type == AddressType::WorkRam) {
						blockName = "__Work RAM__";
					} else {
						blockName = showEitherDataType && inVerifiedDataBlock ? "__data block__" : "__unidentified block__";
					}

					GetLine(output, blockName, "", showData ? -1 : memoryAddr, showData ? -1 : addr, dataType, memoryType);
				} else {
					GetLine(output, "__" + label + "__", "", showData ? -1 : memoryAddr, showData ? -1 : addr, dataType, memoryType);
				}
				insideDataBlock = true;
				emptyBlock = true;
			}

			if(showData) {
				//Output bytes in ".db" statements
				if(byteCount >= 8 || ((!label.empty() || !commentString.empty()) && byteCount > 0)) {
					outputBytes();
				}

				if(byteCount == 0) {
					dbBuffer = ".db";
					output += commentLines;
					if(!label.empty()) {
						GetLine(output, label + ":", "", -1, -1, dataType, memoryType);
					}

					dbRelativeAddr = memoryAddr;
					dbAbsoluteAddr = addr;
				}

				dbBuffer += " $" + HexUtilities::ToHex(source[addr&mask]);

				if(!commentString.empty()) {
					GetLine(output, dbBuffer, commentString, dbRelativeAddr, dbAbsoluteAddr, dataType, memoryType);
					byteCount = 0;
				} else {
					byteCount++;
				}

				emptyBlock = false;
			}
			addr++;
			memoryAddr++;
		}
	}

	if(insideDataBlock) {
		//End the current data block if needed
		endDataBlock();
	}
		
	return output;
}

DisassemblyInfo Disassembler::GetDisassemblyInfo(AddressTypeInfo &info)
{
	DisassemblyInfo* disassemblyInfo = nullptr;
	switch(info.Type) {
		case AddressType::Register: break; //Should never happen
		case AddressType::InternalRam: disassemblyInfo = _disassembleMemoryCache[info.Address & 0x7FF].get(); break;
		case AddressType::PrgRom: disassemblyInfo = _disassembleCache[info.Address].get(); break;
		case AddressType::WorkRam: disassemblyInfo = _disassembleWorkRamCache[info.Address].get(); break;
		case AddressType::SaveRam: disassemblyInfo = _disassembleSaveRamCache[info.Address].get(); break;
	}

	if(disassemblyInfo) {
		return *disassemblyInfo;
	} else {
		return DisassemblyInfo();
	}
}