#pragma once
#include "stdafx.h"

struct State
{
	uint16_t PC = 0;
	uint8_t SP = 0;
	uint8_t A = 0;
	uint8_t X = 0;
	uint8_t Y = 0;
	uint8_t PS = 0;
	uint32_t IRQFlag = 0;
	int32_t CycleCount = 0;
	bool NMIFlag = false;

	//Used by debugger
	uint16_t DebugPC = 0;
};