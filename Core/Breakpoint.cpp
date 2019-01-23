#include "stdafx.h"

#include "Breakpoint.h"

Breakpoint::Breakpoint()
{
}

Breakpoint::~Breakpoint()
{
}

bool Breakpoint::Matches(uint32_t memoryAddr, AddressTypeInfo &info)
{
	if(_memoryType == DebugMemoryType::CpuMemory) {
		if(_startAddr == -1) {
			return true;
		} else if(_endAddr == -1) {
			return (int32_t)memoryAddr == _startAddr;
		} else {
			return (int32_t)memoryAddr >= _startAddr && (int32_t)memoryAddr <= _endAddr;
		}
	} else if(
		(_memoryType == DebugMemoryType::PrgRom && info.Type == AddressType::PrgRom) ||
		(_memoryType == DebugMemoryType::WorkRam && info.Type == AddressType::WorkRam) ||
		(_memoryType == DebugMemoryType::SaveRam && info.Type == AddressType::SaveRam)
	) {
		if(_startAddr == -1) {
			return true;
		} else if(_endAddr == -1) {
			return info.Address == _startAddr;
		} else {
			return info.Address >= _startAddr && info.Address <= _endAddr;
		}
	}

	return false;
}

bool Breakpoint::Matches(uint32_t memoryAddr, PpuAddressTypeInfo &info)
{
	if(_memoryType == DebugMemoryType::PpuMemory) {
		if(_startAddr == -1) {
			return true;
		} else if(_endAddr == -1) {
			return (int32_t)memoryAddr == _startAddr;
		} else {
			return (int32_t)memoryAddr >= _startAddr && (int32_t)memoryAddr <= _endAddr;
		}
	} else if(
		(_memoryType == DebugMemoryType::ChrRam && info.Type == PpuAddressType::ChrRam) ||
		(_memoryType == DebugMemoryType::ChrRom && info.Type == PpuAddressType::ChrRom) ||
		(_memoryType == DebugMemoryType::PaletteMemory && info.Type == PpuAddressType::PaletteRam) ||
		(_memoryType == DebugMemoryType::NametableRam && info.Type == PpuAddressType::NametableRam)
	) {
		if(_startAddr == -1) {
			return true;
		} else if(_endAddr == -1) {
			return info.Address == _startAddr;
		} else {
			return info.Address >= _startAddr && info.Address <= _endAddr;
		}
	}

	return false;
}

bool Breakpoint::HasBreakpointType(BreakpointType type)
{
	switch(type) {
		case BreakpointType::Global: return (_type == BreakpointTypeFlags::Global);
		case BreakpointType::Execute: return (_type & BreakpointTypeFlags::Execute) != 0;
		case BreakpointType::ReadRam: return (_type & BreakpointTypeFlags::ReadRam) != 0;
		case BreakpointType::WriteRam: return (_type & BreakpointTypeFlags::WriteRam) != 0;
		case BreakpointType::ReadVram: return (_type & BreakpointTypeFlags::ReadVram) != 0;
		case BreakpointType::WriteVram: return (_type & BreakpointTypeFlags::WriteVram) != 0;
		case BreakpointType::DummyReadRam: return (_type & BreakpointTypeFlags::ReadRam) != 0 && _processDummyReadWrites;
		case BreakpointType::DummyWriteRam: return (_type & BreakpointTypeFlags::WriteRam) != 0 && _processDummyReadWrites;
	}
	return false;
}

string Breakpoint::GetCondition()
{
	return _condition;
}

bool Breakpoint::HasCondition()
{
	return _condition[0] != 0;
}

void Breakpoint::ClearCondition()
{
	memset(_condition, 0, sizeof(_condition));
}

uint32_t Breakpoint::GetId()
{
	return _id;
}

bool Breakpoint::IsEnabled()
{
	return _enabled;
}

bool Breakpoint::IsMarked()
{
	return _markEvent;
}
