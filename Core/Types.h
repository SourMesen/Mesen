#pragma once
#include "stdafx.h"

enum class MemoryOperation
{
	Read = 1,
	Write = 2,
	Any = 3
};

enum class MemoryOperationType
{
	Read = 0,
	Write = 1,
	ExecOpCode = 2,
	ExecOperand = 3,
	PpuRenderingRead = 4,
	DummyRead = 5
};

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

enum class PrgMemoryType
{
	PrgRom,
	SaveRam,
	WorkRam,
};

enum class ChrMemoryType
{
	Default,
	ChrRom,
	ChrRam
};

enum MemoryAccessType
{
	Unspecified = -1,
	NoAccess = 0x00,
	Read = 0x01,
	Write = 0x02,
	ReadWrite = 0x03
};

enum ChrSpecialPage
{
	NametableA = 0x7000,
	NametableB = 0x7001
};

struct CartridgeState
{
	uint32_t PrgRomSize;
	uint32_t ChrRomSize;
	uint32_t ChrRamSize;

	uint32_t PrgPageCount;
	uint32_t PrgPageSize;
	uint32_t PrgSelectedPages[64];
	uint32_t ChrPageCount;
	uint32_t ChrPageSize;
	uint32_t ChrSelectedPages[64];
	uint32_t Nametables[8];
};


struct PPUControlFlags
{
	bool VerticalWrite;
	uint16_t SpritePatternAddr;
	uint16_t BackgroundPatternAddr;
	bool LargeSprites;
	bool VBlank;

	bool Grayscale;
	bool BackgroundMask;
	bool SpriteMask;
	bool BackgroundEnabled;
	bool SpritesEnabled;
	bool IntensifyRed;
	bool IntensifyGreen;
	bool IntensifyBlue;
};

struct PPUStatusFlags
{
	bool SpriteOverflow;
	bool Sprite0Hit;
	bool VerticalBlank;
};

struct PPUState
{
	uint8_t Control;
	uint8_t Mask;
	uint8_t Status;
	uint32_t SpriteRamAddr;
	uint16_t VideoRamAddr;
	uint8_t XScroll;
	uint16_t TmpVideoRamAddr;
	bool WriteToggle;

	uint16_t HighBitShift;
	uint16_t LowBitShift;
};

struct TileInfo
{
	uint8_t LowByte;
	uint8_t HighByte;
	uint32_t PaletteOffset;
	uint16_t TileAddr;
	
	int32_t AbsoluteTileAddr; //used by HD ppu
	uint8_t OffsetY; //used by HD ppu
};

struct SpriteInfo : TileInfo
{
	bool HorizontalMirror;
	bool BackgroundPriority;
	uint8_t SpriteX;

	bool VerticalMirror; //used by HD ppu
};