#pragma once
#include "stdafx.h"
#include "ExpressionEvaluator.h"

enum BreakpointType
{
	Global = 0,
	Execute = 1,
	ReadRam = 2,
	WriteRam = 3,
	ReadVram = 4,
	WriteVram = 5,
};

class Breakpoint
{
private:
	enum BreakpointTypeFlags
	{
		Global = 0,
		Execute = 1,
		ReadRam = 2,
		WriteRam = 4,
		ReadVram = 8,
		WriteVram = 16,
	};

public:
	Breakpoint();
	~Breakpoint();

	bool Matches(uint32_t memoryAddr, uint32_t absoluteAddr);
	bool HasBreakpointType(BreakpointType type);
	string GetCondition();
	void ClearCondition();
	
private:
	BreakpointTypeFlags _type;
	int32_t _addr;
	bool _isAbsoluteAddr;
	char _condition[1000];
};