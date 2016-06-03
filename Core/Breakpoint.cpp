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
	return _addr == -1 || ((int32_t)memoryAddr == _addr && !_isAbsoluteAddr) || ((int32_t)absoluteAddr == _addr && _isAbsoluteAddr);
}

BreakpointType Breakpoint::GetType()
{
	return _type;
}

string Breakpoint::GetCondition()
{
	return _condition;
}

void Breakpoint::ClearCondition()
{
	memset(_condition, 0, sizeof(_condition));
}