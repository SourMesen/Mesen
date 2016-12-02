#pragma once
#include "stdafx.h"
#include "DebuggerTypes.h"
#include "IMemoryHandler.h"
#include <unordered_map>
#include <unordered_set>

class MemoryAccessCounter
{
private:
	std::unordered_map<int, int> _readCounts[4];
	std::unordered_map<int, int> _writeCounts[4];
	std::unordered_map<int, int> _execCounts[4];

	std::unordered_set<int> _initWrites[4];
	std::unordered_set<int> _uninitReads[4];

	std::unordered_map<int, int>& GetCountMap(MemoryOperationType operationType, AddressType addressType);

public:
	void ProcessMemoryAccess(AddressTypeInfo &addressInfo, MemoryOperationType operation);
	void ResetCounts();
	
	void GetAccessCounts(AddressType memoryType, MemoryOperationType operationType, uint32_t counts[], bool forUninitReads);
};