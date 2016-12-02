#include "stdafx.h"
#include "MemoryAccessCounter.h"
#include "Console.h"

std::unordered_map<int, int>& MemoryAccessCounter::GetCountMap(MemoryOperationType operationType, AddressType addressType)
{
	switch(operationType) {
		case MemoryOperationType::Read: return _readCounts[(int)addressType];
		case MemoryOperationType::Write: return _writeCounts[(int)addressType];

		default:
		case MemoryOperationType::ExecOpCode:
		case MemoryOperationType::ExecOperand: return _execCounts[(int)addressType];
	}
}

void MemoryAccessCounter::ProcessMemoryAccess(AddressTypeInfo &addressInfo, MemoryOperationType operation)
{
	int index = (int)addressInfo.Type;
	std::unordered_map<int, int> &countMap = GetCountMap(operation, addressInfo.Type);
	if(operation != MemoryOperationType::Write &&
		(addressInfo.Type == AddressType::InternalRam || addressInfo.Type == AddressType::WorkRam) &&
		_initWrites[index].find(addressInfo.Address) == _initWrites[index].end()) {
		//Mark address as read before being written to (if trying to read/execute)
		_uninitReads[index].emplace(addressInfo.Address);
	} else if(operation == MemoryOperationType::Write) {
		_initWrites[index].emplace(addressInfo.Address);
	}

	countMap[addressInfo.Address]++;
}

void MemoryAccessCounter::ResetCounts()
{
	Console::Pause();
	for(int i = 0; i < 4; i++) {
		_readCounts[i].clear();
		_writeCounts[i].clear();
		_execCounts[i].clear();
	}
	Console::Resume();
}

void MemoryAccessCounter::GetAccessCounts(AddressType memoryType, MemoryOperationType operationType, uint32_t counts[], bool forUninitReads)
{
	if(forUninitReads) {
		for(int address : _uninitReads[(int)memoryType]) {
			counts[address] = 1;
		}
	} else {
		for(auto kvp : GetCountMap(operationType, memoryType)) {
			counts[kvp.first] = kvp.second;
		}
	}
}