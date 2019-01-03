#pragma once
#include "stdafx.h"
#include <unordered_map>

class MemoryManager;
class LabelManager;
struct State;
enum class AddrMode;

class DisassemblyInfo
{
public:
	static string OPName[256];
	static AddrMode OPMode[256];
	static uint8_t OPSize[256];
	static bool IsUnofficialCode[256];

private:
	uint8_t _byteCode[3];
	bool _isJumpTarget = false;
	bool _isSubEntryPoint = false;
	bool _isSubExitPoint = false;
	uint32_t _opSize = 0;
	AddrMode _opMode;
	
public:
	DisassemblyInfo();
	DisassemblyInfo(uint8_t* opPointer, bool isSubEntryPoint, bool isJumpTarget);

	void Initialize(uint8_t * opPointer, bool isSubEntryPoint, bool isJumpTarget);

	void SetSubEntryPoint();

	void SetJumpTarget();

	int32_t GetEffectiveAddress(State& cpuState, MemoryManager* memoryManager);
	
	void GetEffectiveAddressString(string &out, State& cpuState, MemoryManager* memoryManager, LabelManager* labelManager);
	int32_t GetMemoryValue(State& cpuState, MemoryManager* memoryManager);
	uint16_t GetIndirectJumpDestination(MemoryManager* memoryManager);
	void ToString(string &out, uint32_t memoryAddr, MemoryManager* memoryManager, LabelManager* labelManager, bool extendZeroPage);
	void GetByteCode(string &out);
	uint32_t GetSize();
	uint16_t GetOpAddr(uint16_t memoryAddr);

	bool IsSubEntryPoint();
	bool IsSubExitPoint();
	bool IsJumpTarget();
};

