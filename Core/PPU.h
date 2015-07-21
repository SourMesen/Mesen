#pragma once

#include "stdafx.h"
#include "Snapshotable.h"
#include "MemoryManager.h"
#include "IVideoDevice.h"

enum PPURegisters
{
	Control = 0x00,
	Mask = 0x01,
	Status = 0x02,
	SpriteAddr = 0x03,
	SpriteData = 0x04,
	ScrollOffsets = 0x05,
	VideoMemoryAddr = 0x06,
	VideoMemoryData = 0x07,
	SpriteDMA = 0x4014,
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
};

struct SpriteInfo
{
	uint8_t LowByte;
	uint8_t HighByte;
	uint32_t PaletteOffset;
	bool HorizontalMirror;
	bool BackgroundPriority;
};

struct PPUDebugState
{
	PPUControlFlags ControlFlags;
	PPUStatusFlags StatusFlags;
	PPUState State;
	int32_t Scanline;
	uint32_t Cycle;
};

class PPU : public IMemoryHandler, public Snapshotable
{
	private:
		static PPU* Instance;
		static IVideoDevice *VideoDevice;

		MemoryManager *_memoryManager;

		PPUState _state;
		int32_t _scanline = 0;
		uint32_t _cycle = 0;
		uint32_t _frameCount = 0;
		uint8_t _memoryReadBuffer = 0;
		
		uint8_t _paletteRAM[0x100];

		uint8_t _spriteRAM[0x100];
		uint8_t _secondarySpriteRAM[0x20];

		uint32_t *_outputBuffer;

		PPUControlFlags _flags;
		PPUStatusFlags _statusFlags;

		bool _doNotSetVBFlag = false;

		TileInfo _currentTile;
		TileInfo _nextTile;
		TileInfo _previousTile;

		int32_t _spriteX[8];
		SpriteInfo _spriteTiles[8];
		uint32_t _spriteCount = 0;
		uint32_t _secondaryOAMAddr = 0;
		bool _sprite0Visible = false;

		uint8_t _oamCopybuffer;
		bool _writeOAMData;
		uint32_t _overflowCounter;
		bool _sprite0Added;

		void UpdateStatusFlag();

		void SetControlRegister(uint8_t value);
		void SetMaskRegister(uint8_t value);

		bool IsRenderingEnabled();

		void UpdateVideoRamAddr();
		void IncVerticalScrolling();
		void IncHorizontalScrolling();
		uint16_t GetNameTableAddr();
		uint16_t GetAttributeAddr();

		void ProcessPreVBlankScanline();
		void ProcessPrerenderScanline();
		void ProcessVisibleScanline();

		void CopyOAMData();

		void BeginVBlank();
		void EndVBlank();

		uint32_t GetBGPaletteEntry(uint32_t paletteOffset, uint32_t pixel);
		uint32_t GetSpritePaletteEntry(uint32_t paletteOffset, uint32_t pixel);

		uint8_t ReadPaletteRAM(uint16_t addr);
		void WritePaletteRAM(uint16_t addr, uint8_t value);

		void LoadTileInfo();
		void LoadSpriteTileInfo(uint8_t spriteIndex);
		void ShiftTileRegisters();
		void InitializeShiftRegisters();
		void LoadNextTile();
		void DrawPixel();

		void CopyFrame();

		PPURegisters GetRegisterID(uint16_t addr)
		{
			if(addr == 0x4014) {
				return PPURegisters::SpriteDMA;
			} else {
				return (PPURegisters)(addr & 0x07);
			}
		}

	protected:
		void StreamState(bool saving);

	public:
		PPU(MemoryManager *memoryManager);
		~PPU();

		void Reset();

		PPUDebugState GetState();

		void GetMemoryRanges(MemoryRanges &ranges)
		{
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Read, 0x2000, 0x3FFF);
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Read, 0x4014);
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Write, 0x2000, 0x3FFF);
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Write, 0x4014);
		}

		uint8_t ReadRAM(uint16_t addr);
		void WriteRAM(uint16_t addr, uint8_t value);

		void Exec();
		static void ExecStatic();

		static void RegisterVideoDevice(IVideoDevice *videoDevice)
		{
			PPU::VideoDevice = videoDevice;
		}

		static uint32_t GetFrameCount()
		{
			return PPU::Instance->_frameCount;
		}

		static uint32_t GetFrameCycle()
		{
			return ((PPU::Instance->_scanline + 1) * 341) + PPU::Instance->_cycle;
		}

		static uint32_t GetCurrentCycle()
		{
			return PPU::Instance->_cycle;
		}

		static uint32_t GetCurrentScanline()
		{
			return PPU::Instance->_scanline;
		}
};
