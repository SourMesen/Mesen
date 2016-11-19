#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class DisassemblyInfo;

class Disassembler
{
private:
	vector<shared_ptr<DisassemblyInfo>> _disassembleCache;
	vector<shared_ptr<DisassemblyInfo>> _disassembleRamCache;
	vector<shared_ptr<DisassemblyInfo>> _disassembleMemoryCache;
	uint8_t* _internalRam;
	uint8_t* _prgRom;
	uint8_t* _prgRam;
	uint32_t _prgSize;

	bool IsUnconditionalJump(uint8_t opCode);

public:
	Disassembler(uint8_t* internalRam, uint8_t* prgRom, uint32_t prgSize, uint8_t* prgRam, uint32_t prgRamSize);
	~Disassembler();
	
	uint32_t BuildCache(int32_t absoluteAddr, int32_t absoluteRamAddr, uint16_t memoryAddr, bool isSubEntryPoint);
	void InvalidateCache(uint16_t memoryAddr, int32_t absoluteRamAddr);

	string GetCode(uint32_t startAddr, uint32_t endAddr, uint16_t memoryAddr, PrgMemoryType memoryType);

	shared_ptr<DisassemblyInfo> GetDisassemblyInfo(int32_t absoluteAddress, int32_t absoluteRamAddress, uint16_t memoryAddress);
};
