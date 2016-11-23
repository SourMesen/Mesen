#pragma once
#include "stdafx.h"
#include <unordered_map>
#include "CPU.h"

class LabelManager;

class DisassemblyInfo
{
public:
	static string OPName[256];
	static AddrMode OPMode[256];
	static uint32_t OPSize[256];

private:
	string _byteCode;
	uint8_t *_opPointer = nullptr;
	bool _isSubEntryPoint = false;
	bool _isSubExitPoint = false;
	uint32_t _opSize = 0;
	AddrMode _opMode;

	uint16_t _opAddr;

public:
	DisassemblyInfo(uint8_t* opPointer, bool isSubEntryPoint);

	void SetSubEntryPoint();

	int32_t GetEffectiveAddress(State& cpuState, shared_ptr<MemoryManager> memoryManager);
	string GetEffectiveAddressString(State& cpuState, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager);

	string ToString(uint32_t memoryAddr, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager);
	string GetByteCode();
	uint32_t GetSize();

	bool IsSubEntryPoint();
	bool IsSubExitPoint();
};

