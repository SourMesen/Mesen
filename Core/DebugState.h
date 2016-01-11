#pragma once
#include "stdafx.h"
#include "CPU.h"
#include "PPU.h"

struct DebugState
{
	State CPU;
	PPUDebugState PPU;
};
