#pragma once
#include "stdafx.h"

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

	uint16_t TileAddr; //used by HD ppu
	uint8_t OffsetY; //used by HD ppu
};

struct SpriteInfo : TileInfo
{
	bool HorizontalMirror;
	bool BackgroundPriority;
	uint8_t SpriteX;

	bool VerticalMirror; //used by HD ppu
};

struct PPUDebugState
{
	PPUControlFlags ControlFlags;
	PPUStatusFlags StatusFlags;
	PPUState State;
	int32_t Scanline;
	uint32_t Cycle;
	uint32_t FrameCount;
};