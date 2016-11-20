#pragma once
#include "stdafx.h"
#include "CPU.h"

class DisassemblyInfo
{
public:
	static string OPName[256];
	static AddrMode OPMode[256];
	static uint32_t OPSize[256];

private:
	string _disassembly;
	uint8_t *_opPointer = nullptr;
	bool _isSubEntryPoint = false;
	uint32_t _opSize = 0;
	AddrMode _opMode;
	uint32_t _lastAddr = 0;

private:
	void Initialize(uint32_t memoryAddr = 0);

public:
	DisassemblyInfo(uint8_t* opPointer, bool isSubEntryPoint);

	void SetSubEntryPoint();
	string GetEffectiveAddress(State& cpuState, shared_ptr<MemoryManager> memoryManager);
	string ToString(uint32_t memoryAddr);
	uint32_t GetSize();
};

