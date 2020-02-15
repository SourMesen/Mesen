#include "stdafx.h"
#include "MemoryAccessCounter.h"
#include "Console.h"
#include "CPU.h"
#include "DebugBreakHelper.h"
#include "Debugger.h"
#include "MemoryDumper.h"
#include "PPU.h"
#include "BaseMapper.h"

MemoryAccessCounter::MemoryAccessCounter(Debugger* debugger)
{
	_debugger = debugger;
	
	uint32_t memorySizes[4] = {
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::InternalRam),
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::PrgRom),
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::WorkRam),
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::SaveRam)
	};

	for(int i = 0; i < 4; i++) {
		_counters[i].reserve(memorySizes[i]);
		for(int j = 0; j < memorySizes[i]; j++) {
			_counters[i].push_back({ (uint32_t)j });
		}
	}

	uint32_t ppuMemorySizes[4] = {
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::ChrRom),
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::ChrRam),
		_debugger->GetMemoryDumper()->GetMemorySize(DebugMemoryType::PaletteMemory),
		BaseMapper::NametableSize * BaseMapper::NametableCount,
	};

	for(int i = 0; i < 4; i++) {
		_ppuCounters[i].reserve(ppuMemorySizes[i]);
		for(int j = 0; j < ppuMemorySizes[i]; j++) {
			_ppuCounters[i].push_back({ (uint32_t)j });
		}
	}
}

bool MemoryAccessCounter::IsAddressUninitialized(AddressTypeInfo &addressInfo)
{
	if(addressInfo.Type == AddressType::InternalRam || addressInfo.Type == AddressType::WorkRam) {
		return _counters[(int)addressInfo.Type][addressInfo.Address].WriteCount == 0;
	}
	return false;
}

void MemoryAccessCounter::ProcessPpuMemoryRead(PpuAddressTypeInfo &addressInfo, uint64_t cpuCycle)
{
	if(addressInfo.Address < 0) {
		return;
	}

	AddressCounters& counts = _ppuCounters[(int)addressInfo.Type][addressInfo.Address];
	counts.ReadCount++;
	counts.ReadStamp = cpuCycle;
}

void MemoryAccessCounter::ProcessPpuMemoryWrite(PpuAddressTypeInfo& addressInfo, uint64_t cpuCycle)
{
	if(addressInfo.Address < 0) {
		return;
	}

	AddressCounters& counts = _ppuCounters[(int)addressInfo.Type][addressInfo.Address];
	counts.WriteCount++;
	counts.WriteStamp = cpuCycle;
}

bool MemoryAccessCounter::ProcessMemoryRead(AddressTypeInfo &addressInfo, uint64_t cpuCycle)
{
	if(addressInfo.Address < 0) {
		return false;
	}

	AddressCounters& counts = _counters[(int)addressInfo.Type][addressInfo.Address];
	counts.ReadCount++;
	counts.ReadStamp = cpuCycle;

	if(counts.WriteCount == 0 && (addressInfo.Type == AddressType::InternalRam || addressInfo.Type == AddressType::WorkRam)) {
		//Mark address as read before being written to (if trying to read/execute)
		counts.UninitRead = true;
		return true;
	}

	return false;
}

void MemoryAccessCounter::ProcessMemoryWrite(AddressTypeInfo& addressInfo, uint64_t cpuCycle)
{
	if(addressInfo.Address < 0) {
		return;
	}

	AddressCounters& counts = _counters[(int)addressInfo.Type][addressInfo.Address];
	counts.WriteCount++;
	counts.WriteStamp = cpuCycle;
}

void MemoryAccessCounter::ProcessMemoryExec(AddressTypeInfo& addressInfo, uint64_t cpuCycle)
{
	if(addressInfo.Address < 0) {
		return;
	}
	
	AddressCounters& counts = _counters[(int)addressInfo.Type][addressInfo.Address];
	counts.ExecCount++;
	counts.ExecStamp = cpuCycle;
}

void MemoryAccessCounter::ResetCounts()
{
	DebugBreakHelper helper(_debugger);
	for(int i = 0; i < 4; i++) {
		for(int j = 0; j < _counters[i].size(); j++) {
			_counters[i][j] = { (uint32_t)j };
		}
		for(int j = 0; j < _ppuCounters[i].size(); j++) {
			_ppuCounters[i][j] = { (uint32_t)j };
		}
	}
}

void MemoryAccessCounter::GetNametableChangedData(bool ntChangedData[])
{
	PpuAddressTypeInfo addressInfo;
	uint64_t cpuCycle = _debugger->GetConsole()->GetCpu()->GetCycleCount();
	NesModel model = _debugger->GetConsole()->GetModel();
	double frameRate = model == NesModel::NTSC ? 60.1 : 50.01;
	double overclockRate = _debugger->GetConsole()->GetPpu()->GetOverclockRate() * 100;
	uint32_t cyclesPerFrame = (uint32_t)(_debugger->GetConsole()->GetCpu()->GetClockRate(model) / frameRate * overclockRate);

	for(int i = 0; i < 0x1000; i++) {
		_debugger->GetPpuAbsoluteAddressAndType(0x2000+i, &addressInfo);
		if(addressInfo.Type != PpuAddressType::None) {
			ntChangedData[i] = (cpuCycle - _ppuCounters[(int)addressInfo.Type][addressInfo.Address].WriteStamp) < cyclesPerFrame;
		} else {
			ntChangedData[i] = false;
		}
	}
}

void MemoryAccessCounter::GetAccessCounts(uint32_t offset, uint32_t length, DebugMemoryType memoryType, AddressCounters counts[])
{
	switch(memoryType) {
		default: break;

		case DebugMemoryType::PrgRom: memcpy(counts, _counters[(int)AddressType::PrgRom].data() + offset, length * sizeof(AddressCounters)); break;
		case DebugMemoryType::WorkRam: memcpy(counts, _counters[(int)AddressType::WorkRam].data() + offset, length * sizeof(AddressCounters)); break;
		case DebugMemoryType::SaveRam: memcpy(counts, _counters[(int)AddressType::SaveRam].data() + offset, length * sizeof(AddressCounters)); break;
		case DebugMemoryType::InternalRam: memcpy(counts, _counters[(int)AddressType::InternalRam].data() + offset, length * sizeof(AddressCounters)); break;

		case DebugMemoryType::ChrRom: memcpy(counts, _ppuCounters[(int)PpuAddressType::ChrRom].data() + offset, length * sizeof(AddressCounters)); break;
		case DebugMemoryType::ChrRam: memcpy(counts, _ppuCounters[(int)PpuAddressType::ChrRam].data() + offset, length * sizeof(AddressCounters)); break;
		case DebugMemoryType::NametableRam: memcpy(counts, _ppuCounters[(int)PpuAddressType::NametableRam].data() + offset, length * sizeof(AddressCounters)); break;
		case DebugMemoryType::PaletteMemory: memcpy(counts, _ppuCounters[(int)PpuAddressType::PaletteRam].data() + offset, length * sizeof(AddressCounters)); break;

		case DebugMemoryType::CpuMemory:
			for(uint32_t i = 0; i < length; i++) {
				AddressTypeInfo info;
				_debugger->GetAbsoluteAddressAndType(offset + i, &info);
				if(info.Address >= 0) {
					counts[i] = _counters[(int)info.Type][info.Address];
				} else {
					counts[i] = {};
				}
			}
			break;

		case DebugMemoryType::PpuMemory:
			for(uint32_t i = 0; i < length; i++) {
				PpuAddressTypeInfo info;
				_debugger->GetPpuAbsoluteAddressAndType(offset + i, &info);
				if(info.Address >= 0) {
					counts[i] = _ppuCounters[(int)info.Type][info.Address];
				} else {
					counts[i] = {};
				}
			}
			break;
	}
}