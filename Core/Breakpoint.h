#pragma once
#include "stdafx.h"
#include "DebuggerTypes.h"

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

	bool Matches(uint32_t memoryAddr, AddressTypeInfo &info);
	bool Matches(uint32_t memoryAddr, PpuAddressTypeInfo &info);
	bool HasBreakpointType(BreakpointType type);
	string GetCondition();
	bool HasCondition();
	void ClearCondition();

	uint32_t GetId();
	bool IsEnabled();
	bool IsMarked();
	
private:
	uint32_t _id;
	DebugMemoryType _memoryType;
	BreakpointTypeFlags _type;
	int32_t _startAddr;
	int32_t _endAddr;
	bool _enabled;
	bool _markEvent;
	char _condition[1000];
};