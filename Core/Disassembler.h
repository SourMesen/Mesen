#pragma once
#include "stdafx.h"

class DisassemblyInfo;

class Disassembler
{
private:
	vector<shared_ptr<DisassemblyInfo>> _disassembleCache;
	uint8_t* _prgROM;
	uint32_t _prgSize;

public:
	Disassembler(uint8_t* prgROM, uint32_t prgSize);
	
	void BuildCache(uint32_t absoluteAddr, uint16_t memoryAddr);
	string GetCode(uint32_t startAddr, uint32_t endAddr, uint16_t &memoryAddr);
};
