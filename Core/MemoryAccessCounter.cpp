#include "stdafx.h"
#include "MemoryAccessCounter.h"
#include "Console.h"
#include "CPU.h"
#include "DebugBreakHelper.h"
#include "Debugger.h"
#include "MemoryDumper.h"
#include "BaseMapper.h"

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

		_uninitReads[i].insert(_uninitReads[i].end(), memorySizes[i], 0);
	}

	uint32_t ppuMemorySizes[4] = {
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::ChrRom),
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::ChrRam),
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::PaletteMemory),
		BaseMapper::NametableSize * BaseMapper::NametableCount,
	};

	for(int i = 0; i < 4; i++) {
		_ppuReadCounts[i].insert(_ppuReadCounts[i].end(), ppuMemorySizes[i], 0);
		_ppuWriteCounts[i].insert(_ppuWriteCounts[i].end(), ppuMemorySizes[i], 0);
		_ppuReadStamps[i].insert(_ppuReadStamps[i].end(), ppuMemorySizes[i], 0);
		_ppuWriteStamps[i].insert(_ppuWriteStamps[i].end(), ppuMemorySizes[i], 0);
	}
}

vector<int32_t>& MemoryAccessCounter::GetCountArray(MemoryOperationType operationType, AddressType addressType)
{
	switch(operationType) {
		case MemoryOperationType::Read: return _readCounts[(int)addressType];
		case MemoryOperationType::Write: return _writeCounts[(int)addressType];

		default:
		case MemoryOperationType::ExecOpCode:
		case MemoryOperationType::ExecOperand: return _execCounts[(int)addressType];
	}
}

vector<int32_t>& MemoryAccessCounter::GetStampArray(MemoryOperationType operationType, AddressType addressType)
{
	switch(operationType) {
		case MemoryOperationType::Read: return _readStamps[(int)addressType];
		case MemoryOperationType::Write: return _writeStamps[(int)addressType];

		default:
		case MemoryOperationType::ExecOpCode:
		case MemoryOperationType::ExecOperand: return _execStamps[(int)addressType];
	}
}

vector<int32_t>& MemoryAccessCounter::GetPpuCountArray(MemoryOperationType operationType, PpuAddressType addressType)
{
	return operationType == MemoryOperationType::Write ? _ppuWriteCounts[(int)addressType] : _ppuReadCounts[(int)addressType];
}

vector<int32_t>& MemoryAccessCounter::GetPpuStampArray(MemoryOperationType operationType, PpuAddressType addressType)
{
	return operationType == MemoryOperationType::Write ? _ppuWriteStamps[(int)addressType] : _ppuReadStamps[(int)addressType];
}

bool MemoryAccessCounter::IsAddressUninitialized(AddressTypeInfo &addressInfo)
{
	if(addressInfo.Type == AddressType::InternalRam || addressInfo.Type == AddressType::WorkRam) {
		int index = (int)addressInfo.Type;
		return _writeCounts[index][addressInfo.Address] == 0;
	}
	return false;
}

void MemoryAccessCounter::ProcessPpuMemoryAccess(PpuAddressTypeInfo &addressInfo, MemoryOperationType operation, int32_t cpuCycle)
{
	if(addressInfo.Address >= 0) {
		vector<int> &counts = GetPpuCountArray(operation, addressInfo.Type);
		counts.data()[addressInfo.Address]++;

		vector<int> &stamps = GetPpuStampArray(operation, addressInfo.Type);
		stamps.data()[addressInfo.Address] = cpuCycle;
	}
}

bool MemoryAccessCounter::ProcessMemoryAccess(AddressTypeInfo &addressInfo, MemoryOperationType operation, int32_t cpuCycle)
{
	vector<int> &counts = GetCountArray(operation, addressInfo.Type);
	counts.data()[addressInfo.Address]++;

	vector<int> &stamps = GetStampArray(operation, addressInfo.Type);
	stamps.data()[addressInfo.Address] = cpuCycle;

	if(operation == MemoryOperationType::Read && (addressInfo.Type == AddressType::InternalRam || addressInfo.Type == AddressType::WorkRam) && !_writeCounts[(int)addressInfo.Type][addressInfo.Address]) {
		//Mark address as read before being written to (if trying to read/execute)
		_uninitReads[(int)addressInfo.Type][addressInfo.Address] = true;
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

		memset(_ppuReadCounts[i].data(), 0, _ppuReadCounts[i].size() * sizeof(uint32_t));
		memset(_ppuWriteCounts[i].data(), 0, _ppuWriteCounts[i].size() * sizeof(uint32_t));
		memset(_ppuReadStamps[i].data(), 0, _ppuReadStamps[i].size() * sizeof(uint32_t));
		memset(_ppuWriteStamps[i].data(), 0, _ppuWriteStamps[i].size() * sizeof(uint32_t));
	}
}

void MemoryAccessCounter::GetAccessStamps(uint32_t offset, uint32_t length, DebugMemoryType memoryType, MemoryOperationType operationType, uint32_t stamps[])
{
	switch(memoryType) {
		default: break;

		case DebugMemoryType::InternalRam:
			memcpy(stamps, GetStampArray(operationType, AddressType::InternalRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::WorkRam:
			memcpy(stamps, GetStampArray(operationType, AddressType::WorkRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::SaveRam:
			memcpy(stamps, GetStampArray(operationType, AddressType::SaveRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::PrgRom:
			memcpy(stamps, GetStampArray(operationType, AddressType::PrgRom).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::ChrRom:
			memcpy(stamps, GetPpuStampArray(operationType, PpuAddressType::ChrRom).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::ChrRam:
			memcpy(stamps, GetPpuStampArray(operationType, PpuAddressType::ChrRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::NametableRam:
			memcpy(stamps, GetPpuStampArray(operationType, PpuAddressType::NametableRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::PaletteMemory:
			memcpy(stamps, GetPpuStampArray(operationType, PpuAddressType::PaletteRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::CpuMemory:
			for(uint32_t i = 0; i < length; i++) {
				AddressTypeInfo info;
				_debugger->GetAbsoluteAddressAndType(offset + i, &info);
				stamps[i] = GetStampArray(operationType, info.Type).data()[info.Address];
			}			
			break;

		case DebugMemoryType::PpuMemory:
			for(uint32_t i = 0; i < length; i++) {
				PpuAddressTypeInfo info;
				_debugger->GetPpuAbsoluteAddressAndType(offset + i, &info);
				stamps[i] = GetPpuStampArray(operationType, info.Type).data()[info.Address];
			}
			break;
	}
}

void MemoryAccessCounter::GetNametableChangedData(bool ntChangedData[])
{
	PpuAddressTypeInfo addressInfo;
	int32_t cpuCycle = _debugger->GetConsole()->GetCpu()->GetCycleCount();
	int32_t cyclesPerFrame = _debugger->GetConsole()->GetCpu()->GetClockRate(_debugger->GetConsole()->GetModel()) / 60;
	for(int i = 0; i < 0x1000; i++) {
		_debugger->GetPpuAbsoluteAddressAndType(0x2000+i, &addressInfo);
		if(addressInfo.Type != PpuAddressType::None) {
			ntChangedData[i] = (cpuCycle - _ppuWriteStamps[(int)addressInfo.Type][addressInfo.Address]) < cyclesPerFrame;
		} else {
			ntChangedData[i] = false;
		}
	}
}

void MemoryAccessCounter::GetUninitMemoryReads(DebugMemoryType memoryType, int32_t counts[])
{
	AddressType addressType;
	switch(memoryType) {
		default: return;
		case DebugMemoryType::InternalRam: addressType = AddressType::InternalRam; break;
		case DebugMemoryType::WorkRam: addressType = AddressType::WorkRam; break;
		case DebugMemoryType::SaveRam: addressType = AddressType::SaveRam; break;
		case DebugMemoryType::PrgRom: addressType = AddressType::PrgRom; break;
	}

	for(size_t i = 0, len = _uninitReads[(int)addressType].size(); i < len; i++) {
		counts[i] = _uninitReads[(int)addressType][i];
	}
}

void MemoryAccessCounter::GetAccessCounts(uint32_t offset, uint32_t length, DebugMemoryType memoryType, MemoryOperationType operationType, int32_t counts[])
{
	switch(memoryType) {
		default: break;

		case DebugMemoryType::InternalRam:
			memcpy(counts, GetCountArray(operationType, AddressType::InternalRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::WorkRam:
			memcpy(counts, GetCountArray(operationType, AddressType::WorkRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::SaveRam:
			memcpy(counts, GetCountArray(operationType, AddressType::SaveRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::PrgRom:
			memcpy(counts, GetCountArray(operationType, AddressType::PrgRom).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::ChrRom:
			memcpy(counts, GetPpuCountArray(operationType, PpuAddressType::ChrRom).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::ChrRam:
			memcpy(counts, GetPpuCountArray(operationType, PpuAddressType::ChrRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::NametableRam:
			memcpy(counts, GetPpuCountArray(operationType, PpuAddressType::NametableRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::PaletteMemory:
			memcpy(counts, GetPpuCountArray(operationType, PpuAddressType::PaletteRam).data() + offset, length * sizeof(uint32_t));
			break;

		case DebugMemoryType::CpuMemory:
			for(uint32_t i = 0; i < length; i++) {
				AddressTypeInfo info;
				_debugger->GetAbsoluteAddressAndType(offset + i, &info);
				counts[i] = GetCountArray(operationType, info.Type).data()[info.Address];
			}
			break;

		case DebugMemoryType::PpuMemory:
			for(uint32_t i = 0; i < length; i++) {
				PpuAddressTypeInfo info;
				_debugger->GetPpuAbsoluteAddressAndType(offset + i, &info);
				counts[i] = GetPpuCountArray(operationType, info.Type).data()[info.Address];
			}
			break;
	}
}