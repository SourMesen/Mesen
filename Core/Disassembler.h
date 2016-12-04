#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

struct State;
class MemoryManager;
class DisassemblyInfo;
class LabelManager;
class Debugger;

class Disassembler
{
private:
	Debugger* _debugger;
	vector<shared_ptr<DisassemblyInfo>> _disassembleCache;
	vector<shared_ptr<DisassemblyInfo>> _disassembleRamCache;
	vector<shared_ptr<DisassemblyInfo>> _disassembleMemoryCache;
	uint8_t* _internalRam;
	uint8_t* _prgRom;
	uint8_t* _prgRam;
	uint32_t _prgSize;

	bool IsJump(uint8_t opCode);
	bool IsUnconditionalJump(uint8_t opCode);
	vector<string> SplitComment(string input);
	string GetLine(string code = "", string comment = "", int32_t cpuAddress = -1, int32_t absoluteAddress = -1, string byteCode = "", string addressing = "");
	string GetSubHeader(DisassemblyInfo *info, string &label, uint16_t relativeAddr, uint16_t resetVector, uint16_t nmiVector, uint16_t irqVector);

public:
	Disassembler(uint8_t* internalRam, uint8_t* prgRom, uint32_t prgSize, uint8_t* prgRam, uint32_t prgRamSize, Debugger* debugger);
	~Disassembler();

	void BuildOpCodeTables(bool useLowerCase);
	
	uint32_t BuildCache(int32_t absoluteAddr, int32_t absoluteRamAddr, uint16_t memoryAddr, bool isSubEntryPoint);
	void InvalidateCache(uint16_t memoryAddr, int32_t absoluteRamAddr);

	string GetCode(uint32_t startAddr, uint32_t endAddr, uint16_t memoryAddr, PrgMemoryType memoryType, bool showEffectiveAddresses, bool showOnlyDiassembledCode, State& cpuState, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager);

	shared_ptr<DisassemblyInfo> GetDisassemblyInfo(int32_t absoluteAddress, int32_t absoluteRamAddress, uint16_t memoryAddress);
};
