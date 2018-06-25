#include "stdafx.h"
#include "MemoryAccessCounter.h"
#include "Console.h"
#include "DebugBreakHelper.h"
#include "Debugger.h"
#include "MemoryDumper.h"

MemoryAccessCounter::MemoryAccessCounter(Debugger* debugger)
{
	_debugger = debugger;
	
	uint32_t memorySizes[4] = { 
		0x2000, 
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::PrgRom), 
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::WorkRam), 
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::SaveRam)
	};

	for(int i = 0; i < 4; i++) {
		_readCounts[i].insert(_readCounts[i].end(), memorySizes[i], 0);
		_writeCounts[i].insert(_writeCounts[i].end(), memorySizes[i], 0);
		_execCounts[i].insert(_execCounts[i].end(), memorySizes[i], 0);

		_readStamps[i].insert(_readStamps[i].end(), memorySizes[i], 0);
		_writeStamps[i].insert(_writeStamps[i].end(), memorySizes[i], 0);
		_execStamps[i].insert(_execStamps[i].end(), memorySizes[i], 0);

		_initWrites[i].insert(_initWrites[i].end(), memorySizes[i], 0);
		_uninitReads[i].insert(_uninitReads[i].end(), memorySizes[i], 0);
	}
}

vector<int32_t>& MemoryAccessCounter::GetArray(MemoryOperationType operationType, AddressType addressType, bool stampArray)
{
	switch(operationType) {
		case MemoryOperationType::Read: return stampArray ? _readStamps[(int)addressType] : _readCounts[(int)addressType];
		case MemoryOperationType::Write: return stampArray ? _writeStamps[(int)addressType] : _writeCounts[(int)addressType];

		default:
		case MemoryOperationType::ExecOpCode:
		case MemoryOperationType::ExecOperand: return stampArray ? _execStamps[(int)addressType] : _execCounts[(int)addressType];
	}
}

bool MemoryAccessCounter::ProcessMemoryAccess(AddressTypeInfo &addressInfo, MemoryOperationType operation, int32_t cpuCycle)
{
	int index = (int)addressInfo.Type;
	vector<int> &counts = GetArray(operation, addressInfo.Type, false);
	counts.data()[addressInfo.Address]++;

	vector<int> &stamps = GetArray(operation, addressInfo.Type, true);
	stamps.data()[addressInfo.Address] = cpuCycle;

	if(operation == MemoryOperationType::Write) {
		_initWrites[index][addressInfo.Address] = true;
	} else if((addressInfo.Type == AddressType::InternalRam || addressInfo.Type == AddressType::WorkRam) && !_initWrites[index][addressInfo.Address]) {
		//Mark address as read before being written to (if trying to read/execute)
		_uninitReads[index][addressInfo.Address] = true;
		return true;
	}

	return false;
}

void MemoryAccessCounter::ResetCounts()
{
	DebugBreakHelper helper(_debugger);
	for(int i = 0; i < 4; i++) {
		memset(_readCounts[i].data(), 0, _readCounts[i].size() * sizeof(uint32_t));
		memset(_writeCounts[i].data(), 0, _writeCounts[i].size() * sizeof(uint32_t));
		memset(_execCounts[i].data(), 0, _execCounts[i].size() * sizeof(uint32_t));

		memset(_readStamps[i].data(), 0, _readStamps[i].size() * sizeof(uint32_t));
		memset(_writeStamps[i].data(), 0, _writeStamps[i].size() * sizeof(uint32_t));
		memset(_execStamps[i].data(), 0, _execStamps[i].size() * sizeof(uint32_t));
	}
}

void MemoryAccessCounter::GetAccessCounts(AddressType memoryType, MemoryOperationType operationType, uint32_t counts[], bool forUninitReads)
{
	if(forUninitReads) {
		for(size_t i = 0, len = _uninitReads[(int)memoryType].size(); i < len; i++) {
			counts[i] = _uninitReads[(int)memoryType][i];
		}
	} else {
		memcpy(counts, GetArray(operationType, memoryType, false).data(), GetArray(operationType, memoryType, false).size() * sizeof(uint32_t));
	}
}

void MemoryAccessCounter::GetAccessStamps(uint32_t offset, uint32_t length, DebugMemoryType memoryType, MemoryOperationType operationType, uint32_t stamps[])
{
	switch(memoryType) {
		default: break;

		case DebugMemoryType::InternalRam:
			memcpy(stamps, GetArray(operationType, AddressType::InternalRam, true).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::WorkRam:
			memcpy(stamps, GetArray(operationType, AddressType::WorkRam, true).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::SaveRam:
			memcpy(stamps, GetArray(operationType, AddressType::SaveRam, true).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::PrgRom:
			memcpy(stamps, GetArray(operationType, AddressType::PrgRom, true).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::CpuMemory:
			for(uint32_t i = 0; i < length; i++) {
				AddressTypeInfo info;
				_debugger->GetAbsoluteAddressAndType(offset + i, &info);
				stamps[i] = GetArray(operationType, info.Type, true).data()[info.Address];
			}			
			break;
	}
}

void MemoryAccessCounter::GetAccessCountsEx(uint32_t offset, uint32_t length, DebugMemoryType memoryType, MemoryOperationType operationType, int32_t counts[])
{
	switch(memoryType) {
		default: break;

		case DebugMemoryType::InternalRam:
			memcpy(counts, GetArray(operationType, AddressType::InternalRam, false).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::WorkRam:
			memcpy(counts, GetArray(operationType, AddressType::WorkRam, false).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::SaveRam:
			memcpy(counts, GetArray(operationType, AddressType::SaveRam, false).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::PrgRom:
			memcpy(counts, GetArray(operationType, AddressType::PrgRom, false).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::CpuMemory:
			for(uint32_t i = 0; i < length; i++) {
				AddressTypeInfo info;
				_debugger->GetAbsoluteAddressAndType(offset + i, &info);
				counts[i] = GetArray(operationType, info.Type, false).data()[info.Address];
			}
			break;
	}
}