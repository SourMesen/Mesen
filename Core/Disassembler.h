#pragma once
#include "stdafx.h"

class DisassemblyInfo;

class Disassembler
{
private:
	vector<shared_ptr<DisassemblyInfo>> _disassembleCache;
	vector<shared_ptr<DisassemblyInfo>> _disassembleMemoryCache;
	uint8_t* _internalRAM;
	uint8_t* _prgROM;
	uint32_t _prgSize;

public:
	Disassembler(uint8_t* internalRAM, uint8_t* prgROM, uint32_t prgSize);
	~Disassembler();
	
	void BuildCache(uint32_t absoluteAddr, uint16_t memoryAddr);
	string GetRAMCode();
	string GetCode(uint32_t startAddr, uint32_t endAddr, uint16_t &memoryAddr);
};
