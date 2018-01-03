#pragma once
#include "stdafx.h"
#include "DebuggerTypes.h"

struct State;
class MemoryManager;
class DisassemblyInfo;
class LabelManager;
class Debugger;
class BaseMapper;

enum class DataType
{
	VerifiedCode,
	VerifiedData,
	UnidentifiedData,
};

class Disassembler
{
private:
	Debugger* _debugger;
	MemoryManager* _memoryManager;
	BaseMapper *_mapper;

	vector<shared_ptr<DisassemblyInfo>> _disassembleCache;
	vector<shared_ptr<DisassemblyInfo>> _disassembleWorkRamCache;
	vector<shared_ptr<DisassemblyInfo>> _disassembleSaveRamCache;
	vector<shared_ptr<DisassemblyInfo>> _disassembleMemoryCache;

	bool IsJump(uint8_t opCode);
	bool IsUnconditionalJump(uint8_t opCode);
	void GetLine(string &out, string code = "", string comment = string(), int32_t cpuAddress = -1, int32_t absoluteAddress = -1, DataType dataType = DataType::VerifiedCode);
	void GetCodeLine(string &out, string &code, string &comment, int32_t cpuAddress, int32_t absoluteAddress, string &byteCode, string &addressing, DataType dataType, bool isIndented);
	void GetSubHeader(string &out, DisassemblyInfo *info, string &label, uint16_t relativeAddr, uint16_t resetVector, uint16_t nmiVector, uint16_t irqVector);
	
	void GetInfo(AddressTypeInfo &info, uint8_t** source, uint32_t &size, vector<shared_ptr<DisassemblyInfo>> **cache);

public:
	Disassembler(MemoryManager* memoryManager, BaseMapper* mapper, Debugger* debugger);
	~Disassembler();

	void BuildOpCodeTables(bool useLowerCase);
	void Reset();
	
	uint32_t BuildCache(AddressTypeInfo &info, uint16_t memoryAddr, bool isSubEntryPoint);
	void InvalidateCache(AddressTypeInfo &info);

	bool IsUnofficialOpCode(uint8_t opCode);

	string GetCode(AddressTypeInfo &addressInfo, uint32_t endAddr, uint16_t memoryAddr, State& cpuState, shared_ptr<MemoryManager> memoryManager, shared_ptr<LabelManager> labelManager);

	DisassemblyInfo GetDisassemblyInfo(AddressTypeInfo &info);

	void RebuildPrgRomCache(uint32_t absoluteAddr, int32_t length);
};
