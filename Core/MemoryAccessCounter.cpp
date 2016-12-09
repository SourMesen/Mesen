#include "stdafx.h"
#include "MemoryAccessCounter.h"
#include "Console.h"
#include "DebugBreakHelper.h"
#include "Debugger.h"

MemoryAccessCounter::MemoryAccessCounter(Debugger* debugger)
{
	_debugger = debugger;
	
	int memorySizes[4] = { 0x2000, _debugger->GetMemorySize(DebugMemoryType::PrgRom), _debugger->GetMemorySize(DebugMemoryType::WorkRam), _debugger->GetMemorySize(DebugMemoryType::SaveRam) };
	for(int i = 0; i < 4; i++) {
		_readCounts[i].insert(_readCounts[i].end(), memorySizes[i], 0);
		_writeCounts[i].insert(_writeCounts[i].end(), memorySizes[i], 0);
		_execCounts[i].insert(_execCounts[i].end(), memorySizes[i], 0);
	}
}

vector<int>& MemoryAccessCounter::GetArray(MemoryOperationType operationType, AddressType addressType)
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
	vector<int> &counts = GetArray(operation, addressInfo.Type);
	if(operation != MemoryOperationType::Write &&
		(addressInfo.Type == AddressType::InternalRam || addressInfo.Type == AddressType::WorkRam) &&
		_initWrites[index].find(addressInfo.Address) == _initWrites[index].end()) {
		//Mark address as read before being written to (if trying to read/execute)
		_uninitReads[index].emplace(addressInfo.Address);
	} else if(operation == MemoryOperationType::Write) {
		_initWrites[index].emplace(addressInfo.Address);
	}

	counts.data()[addressInfo.Address]++;
}

void MemoryAccessCounter::ResetCounts()
{
	DebugBreakHelper helper(_debugger);
	for(int i = 0; i < 4; i++) {
		memset(_readCounts[i].data(), 0, _readCounts[i].size() * sizeof(uint32_t));
		memset(_writeCounts[i].data(), 0, _writeCounts[i].size() * sizeof(uint32_t));
		memset(_execCounts[i].data(), 0, _execCounts[i].size() * sizeof(uint32_t));
	}
}

void MemoryAccessCounter::GetAccessCounts(AddressType memoryType, MemoryOperationType operationType, uint32_t counts[], bool forUninitReads)
{
	if(forUninitReads) {
		for(int address : _uninitReads[(int)memoryType]) {
			counts[address] = 1;
		}
	} else {
		memcpy(counts, GetArray(operationType, memoryType).data(), GetArray(operationType, memoryType).size() * sizeof(uint32_t));
	}
}