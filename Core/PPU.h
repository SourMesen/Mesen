#pragma once

#include "stdafx.h"
#include "Snapshotable.h"
#include "MemoryManager.h"
#include "EmulationSettings.h"

enum class NesModel;

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
};

class PPU : public IMemoryHandler, public Snapshotable
{
	protected:
		static PPU* Instance;

		MemoryManager *_memoryManager;

		PPUState _state;
		int32_t _scanline;
		uint32_t _cycle;
		uint32_t _frameCount;
		uint8_t _memoryReadBuffer;

		uint8_t _paletteRAM[0x20];

		uint8_t _spriteRAM[0x100];
		uint8_t _secondarySpriteRAM[0x20];

		uint16_t *_currentOutputBuffer;
		uint16_t *_outputBuffers[2];

		NesModel _nesModel;
		uint16_t _vblankEnd;

		PPUControlFlags _flags;
		PPUStatusFlags _statusFlags;

		uint16_t _intensifyColorBits;
		uint8_t _paletteRamMask;

		SpriteInfo *_lastSprite; //used by HD ppu

		TileInfo _currentTile;
		TileInfo _nextTile;
		TileInfo _previousTile;

		SpriteInfo _spriteTiles[64];
		uint32_t _spriteCount = 0;
		uint32_t _secondaryOAMAddr = 0;
		bool _sprite0Visible = false;

		uint32_t _overflowSpriteAddr = 0;
		uint32_t _spriteIndex = 0;

		uint8_t _openBus = 0;
		int32_t _openBusDecayStamp[8];
		uint32_t _ignoreVramRead = 0;

		uint16_t _spriteDmaCounter = 0;
		uint16_t _spriteDmaAddr = 0;

		uint8_t _oamCopybuffer;
		bool _spriteInRange;
		bool _sprite0Added;
		uint8_t _spriteAddrH;
		uint8_t _spriteAddrL;
		bool _oamCopyDone;

		bool _renderingEnabled;
		bool _prevRenderingEnabled;
		
		void UpdateStatusFlag();

		void SetControlRegister(uint8_t value);
		void SetMaskRegister(uint8_t value);

		bool IsRenderingEnabled();

		void SetOpenBus(uint8_t mask, uint8_t value);
		uint8_t ApplyOpenBus(uint8_t mask, uint8_t value);

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
		void TriggerNmi();
		void EndVBlank();

		uint32_t GetBGPaletteEntry(uint32_t paletteOffset, uint32_t pixel);
		uint32_t GetSpritePaletteEntry(uint32_t paletteOffset, uint32_t pixel);

		void WritePaletteRAM(uint16_t addr, uint8_t value);

		void LoadTileInfo();
		void LoadSprite(uint8_t spriteY, uint8_t tileIndex, uint8_t attributes, uint8_t spriteX, bool extraSprite);
		void LoadSpriteTileInfo();
		void LoadExtraSprites();
		void ShiftTileRegisters();
		void InitializeShiftRegisters();
		void LoadNextTile();

		uint32_t GetPixelColor(uint32_t &paletteOffset);
		virtual void DrawPixel();
		virtual void SendFrame();

		PPURegisters GetRegisterID(uint16_t addr)
		{
			if(addr == 0x4014) {
				return PPURegisters::SpriteDMA;
			} else {
				return (PPURegisters)(addr & 0x07);
			}
		}

		void StreamState(bool saving);

	public:
		static const uint32_t ScreenWidth = 256;
		static const uint32_t ScreenHeight = 240;
		static const uint32_t PixelCount = 256*240;
		static const uint32_t OutputBufferSize = 256*240*2;

		PPU(MemoryManager *memoryManager);
		virtual ~PPU();

		void Reset();

		PPUDebugState GetState();

		void GetMemoryRanges(MemoryRanges &ranges)
		{
			ranges.AddHandler(MemoryOperation::Read, 0x2000, 0x3FFF);
			ranges.AddHandler(MemoryOperation::Write, 0x2000, 0x3FFF);
			ranges.AddHandler(MemoryOperation::Write, 0x4014);
		}

		uint8_t ReadPaletteRAM(uint16_t addr);

		uint8_t ReadRAM(uint16_t addr);
		void WriteRAM(uint16_t addr, uint8_t value);

		void SetNesModel(NesModel model);
		
		void Exec();
		static void ExecStatic();

		static uint32_t GetFrameCount()
		{
			return PPU::Instance->_frameCount;
		}

		static uint32_t GetFrameCycle()
		{
			return ((PPU::Instance->_scanline + 1) * 341) + PPU::Instance->_cycle;
		}

		static PPUControlFlags GetControlFlags()
		{
			return PPU::Instance->_flags;
		}

		static uint32_t GetCurrentCycle()
		{
			return PPU::Instance->_cycle;
		}

		static int32_t GetCurrentScanline()
		{
			return PPU::Instance->_scanline;
		}
		
		uint8_t* GetSpriteRam()
		{
			return _spriteRAM;
		}

		uint8_t* GetSecondarySpriteRam()
		{
			return _secondarySpriteRAM;
		}

		static uint32_t GetPixelBrightness(uint8_t x, uint8_t y)
		{
			//Used by Zapper, gives a rough approximation of the brightness level of the specific pixel
			uint16_t pixelData = PPU::Instance->_currentOutputBuffer[y << 8 | x];
			uint32_t argbColor = EmulationSettings::GetRgbPalette()[pixelData & 0x3F];
			return (argbColor & 0xFF) + ((argbColor >> 8) & 0xFF) + ((argbColor >> 16) & 0xFF);
		}
};
