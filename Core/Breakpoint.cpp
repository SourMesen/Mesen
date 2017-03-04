#include "stdafx.h"

#include "Breakpoint.h"

Breakpoint::Breakpoint()
{
}

Breakpoint::~Breakpoint()
{
}

bool Breakpoint::Matches(uint32_t memoryAddr, uint32_t absoluteAddr)
{
	if(_endAddr == -1) {
		return _startAddr == -1 || ((int32_t)memoryAddr == _startAddr && !_isAbsoluteAddr) || ((int32_t)absoluteAddr == _startAddr && _isAbsoluteAddr);
	} else {
		if(_isAbsoluteAddr) {
			if((int32_t)absoluteAddr >= _startAddr && (int32_t)absoluteAddr <= _endAddr) {
				return true;
			}
		} else {
			if((int32_t)memoryAddr >= _startAddr && (int32_t)memoryAddr <= _endAddr) {
				return true;
			}
		}
		return false;
	}
}

bool Breakpoint::HasBreakpointType(BreakpointType type)
{
	switch(type) {
		case BreakpointType::Global: return (_type == BreakpointTypeFlags::Global);
		case BreakpointType::Execute: return (_type & BreakpointTypeFlags::Execute) == BreakpointTypeFlags::Execute;
		case BreakpointType::ReadRam: return (_type & BreakpointTypeFlags::ReadRam) == BreakpointTypeFlags::ReadRam;
		case BreakpointType::WriteRam: return (_type & BreakpointTypeFlags::WriteRam) == BreakpointTypeFlags::WriteRam;
		case BreakpointType::ReadVram: return (_type & BreakpointTypeFlags::ReadVram) == BreakpointTypeFlags::ReadVram;
		case BreakpointType::WriteVram: return (_type & BreakpointTypeFlags::WriteVram) == BreakpointTypeFlags::WriteVram;
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