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

	vector<int32_t> _readStamps[4];
	vector<int32_t> _writeStamps[4];
	vector<int32_t> _execStamps[4];

	vector<uint8_t> _initWrites[4];
	vector<uint8_t> _uninitReads[4];

	vector<int32_t>& GetArray(MemoryOperationType operationType, AddressType addressType, bool stampArray);

public:
	MemoryAccessCounter(Debugger* debugger);

	bool ProcessMemoryAccess(AddressTypeInfo &addressInfo, MemoryOperationType operation, int32_t cpuCycle);
	void ResetCounts();

	bool IsAddressUninitialized(AddressTypeInfo &addressInfo);
	
	void GetAccessCounts(AddressType memoryType, MemoryOperationType operationType, uint32_t counts[], bool forUninitReads);
	void GetAccessCountsEx(uint32_t offset, uint32_t length, DebugMemoryType memoryType, MemoryOperationType operationType, int32_t counts[]);
	void GetAccessStamps(uint32_t offset, uint32_t length, DebugMemoryType memoryType, MemoryOperationType operationType, uint32_t stamps[]);
};