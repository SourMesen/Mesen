#pragma once
#include "stdafx.h"
#include "DebuggerTypes.h"
#include "IMemoryHandler.h"
#include <unordered_set>
class Debugger;

class MemoryAccessCounter
{
private:
	Debugger* _debugger;
	vector<int32_t> _readCounts[4];
	vector<int32_t> _writeCounts[4];
	vector<int32_t> _execCounts[4];

	vector<uint64_t> _readStamps[4];
	vector<uint64_t> _writeStamps[4];
	vector<uint64_t> _execStamps[4];

	vector<uint8_t> _uninitReads[4];

	vector<int32_t> _ppuReadCounts[4];
	vector<int32_t> _ppuWriteCounts[4];
	vector<uint64_t> _ppuReadStamps[4];
	vector<uint64_t> _ppuWriteStamps[4];

	vector<int32_t>& GetCountArray(MemoryOperationType operationType, AddressType addressType);
	vector<uint64_t>& GetStampArray(MemoryOperationType operationType, AddressType addressType);

	vector<int32_t>& GetPpuCountArray(MemoryOperationType operationType, PpuAddressType addressType);
	vector<uint64_t>& GetPpuStampArray(MemoryOperationType operationType, PpuAddressType addressType);

public:
	MemoryAccessCounter(Debugger* debugger);

	void ProcessPpuMemoryAccess(PpuAddressTypeInfo &addressInfo, MemoryOperationType operation, uint64_t cpuCycle);
	bool ProcessMemoryAccess(AddressTypeInfo &addressInfo, MemoryOperationType operation, uint64_t cpuCycle);
	void ResetCounts();

	bool IsAddressUninitialized(AddressTypeInfo &addressInfo);
	
	void GetUninitMemoryReads(DebugMemoryType memoryType, int32_t counts[]);
	void GetNametableChangedData(bool ntChangedData[]);
	void GetAccessCounts(uint32_t offset, uint32_t length, DebugMemoryType memoryType, MemoryOperationType operationType, int32_t counts[]);
	void GetAccessStamps(uint32_t offset, uint32_t length, DebugMemoryType memoryType, MemoryOperationType operationType, uint64_t stamps[]);
};