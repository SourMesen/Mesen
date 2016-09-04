#pragma once
#include "stdafx.h"
#include "CPU.h"
#include "PPU.h"
#include "BaseMapper.h"

struct DebugState
{
	State CPU;
	PPUDebugState PPU;
	CartridgeState Cartridge;
};
