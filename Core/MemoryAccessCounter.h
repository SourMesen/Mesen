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
	vector<int> _readCounts[4];
	vector<int> _writeCounts[4];
	vector<int> _execCounts[4];

	std::unordered_set<int> _initWrites[4];
	std::unordered_set<int> _uninitReads[4];

	vector<int>& GetArray(MemoryOperationType operationType, AddressType addressType);
	
public:
	MemoryAccessCounter(Debugger* debugger);

	void ProcessMemoryAccess(AddressTypeInfo &addressInfo, MemoryOperationType operation);
	void ResetCounts();
	
	void GetAccessCounts(AddressType memoryType, MemoryOperationType operationType, uint32_t counts[], bool forUninitReads);
};