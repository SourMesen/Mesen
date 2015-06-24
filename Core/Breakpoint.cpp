#include "stdafx.h"

#include "Breakpoint.h"

Breakpoint::Breakpoint(BreakpointType type, uint32_t addr, bool isAbsoluteAddr)
{
	UpdateBreakpoint(type, addr, isAbsoluteAddr, true);
}

Breakpoint::~Breakpoint()
{
}
