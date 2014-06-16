#pragma once

#include "stdafx.h"
#include "MemoryManager.h"

enum PPURegisters
{
	Control = 0x00,
	Control2 = 0x01,
	Status = 0x02,
	SpriteAddr = 0x03,
	SpriteData = 0x04,
	ScrollOffsets = 0x05,
	VideoMemoryAddr = 0x06,
	VideoMemoryData = 0x07
};

struct PPUControlFlags
{
	uint16_t NameTableAddr;
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
	uint8_t Control2;
	uint8_t Status;
	uint8_t SpriteRamAddr;
	uint16_t VideoRamAddr;
};

class PPU : public IMemoryHandler
{
	private:
		MemoryManager *_memoryManager;

		PPUState _state;
		uint64_t _cycleCount;

		uint8_t _memoryReadBuffer;
		uint8_t _spriteRAM[0x100];

		uint8_t *_outputBuffer;

		int16_t _scanline = 0;
		uint16_t _cycle = 0;
		uint32_t _frameCount = 0;

		bool _writeLow = false;

		PPUControlFlags _flags;
		PPUStatusFlags _statusFlags;

		void PPU::UpdateStatusFlag();

		void PPU::UpdateFlags();
		bool PPU::CheckFlag(PPUControlFlags flag);

		PPURegisters GetRegisterID(uint16_t addr)
		{
			return (PPURegisters)(addr & 0x07);
		}

	public:
		PPU(MemoryManager *memoryManager);
		~PPU();

		vector<std::array<uint16_t, 2>> GetRAMAddresses()
		{
			return{ { { 0x2000, 0x3FFF } }, { {0x4014, 0x4014 } } };
		}

		uint8_t ReadRAM(uint16_t addr);
		void WriteRAM(uint16_t addr, uint8_t value);

		void Exec();
};