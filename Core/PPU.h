#pragma once

#include "stdafx.h"
#include "Snapshotable.h"
#include "EmulationSettings.h"
#include "Types.h"
#include "DebuggerTypes.h"
#include "IMemoryHandler.h"

enum class NesModel;

class BaseMapper;

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

class PPU : public IMemoryHandler, public Snapshotable
{
	protected:
		static PPU* Instance;
		static constexpr uint16_t _palSpriteEvalScanline = 264;
	
		BaseMapper *_mapper;

		PPUState _state;
		int32_t _scanline;
		uint32_t _cycle;
		uint32_t _frameCount;
		uint8_t _memoryReadBuffer;

		uint8_t _paletteRAM[0x20];

		uint8_t _spriteRAM[0x100];
		uint8_t _secondarySpriteRAM[0x20];
		bool _hasSprite[257];

		uint16_t *_currentOutputBuffer;
		uint16_t *_outputBuffers[2];

		NesModel _nesModel;
		uint16_t _vblankEnd;
		uint16_t _nmiScanline;

		PPUControlFlags _flags;
		PPUStatusFlags _statusFlags;

		uint16_t _intensifyColorBits;
		uint8_t _paletteRamMask;

		SpriteInfo *_lastSprite; //used by HD ppu

		TileInfo _currentTile;
		TileInfo _nextTile;
		TileInfo _previousTile;

		SpriteInfo _spriteTiles[64];
		uint32_t _spriteCount;
		uint32_t _secondaryOAMAddr;
		bool _sprite0Visible;

		uint32_t _overflowSpriteAddr;
		uint32_t _spriteIndex;

		uint8_t _openBus;
		int32_t _openBusDecayStamp[8];
		uint32_t _ignoreVramRead;

		uint8_t _oamCopybuffer;
		bool _spriteInRange;
		bool _sprite0Added;
		uint8_t _spriteAddrH;
		uint8_t _spriteAddrL;
		bool _oamCopyDone;
		uint8_t _overflowBugCounter;

		bool _renderingEnabled;
		bool _prevRenderingEnabled;

		double _cyclesNeeded;

		uint16_t _updateVramAddr;
		uint8_t _updateVramAddrDelay;

		uint32_t _minimumDrawBgCycle;
		uint32_t _minimumDrawSpriteCycle;
		uint32_t _minimumDrawSpriteStandardCycle;

		int32_t _oamDecayCycles[0x40];
		bool _enableOamDecay;
		
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

		__forceinline void ProcessScanline();
		__forceinline void ProcessSpriteEvaluation();

		void BeginVBlank();
		void TriggerNmi();

		void LoadTileInfo();
		void LoadSprite(uint8_t spriteY, uint8_t tileIndex, uint8_t attributes, uint8_t spriteX, bool extraSprite);
		void LoadSpriteTileInfo();
		void LoadExtraSprites();
		__forceinline void ShiftTileRegisters();
		void InitializeShiftRegisters();
		void LoadNextTile();

		__forceinline uint8_t ReadSpriteRam(uint8_t addr);
		__forceinline void WriteSpriteRam(uint8_t addr, uint8_t value);

		void UpdateMinimumDrawCycles();

		__forceinline uint32_t GetPixelColor();
		__forceinline virtual void DrawPixel();
		virtual void SendFrame();

		PPURegisters GetRegisterID(uint16_t addr)
		{
			if(addr == 0x4014) {
				return PPURegisters::SpriteDMA;
			} else {
				return (PPURegisters)(addr & 0x07);
			}
		}

		void StreamState(bool saving) override;

	public:
		static const uint32_t ScreenWidth = 256;
		static const uint32_t ScreenHeight = 240;
		static const uint32_t PixelCount = 256*240;
		static const uint32_t OutputBufferSize = 256*240*2;

		PPU(BaseMapper *mapper);
		virtual ~PPU();

		void Reset();

		void DebugSendFrame();
		PPUDebugState GetState();
		void SetState(PPUDebugState state);

		void GetMemoryRanges(MemoryRanges &ranges) override
		{
			ranges.AddHandler(MemoryOperation::Read, 0x2000, 0x3FFF);
			ranges.AddHandler(MemoryOperation::Write, 0x2000, 0x3FFF);
			ranges.AddHandler(MemoryOperation::Write, 0x4014);
		}

		__forceinline uint8_t ReadPaletteRAM(uint16_t addr);
		void WritePaletteRAM(uint16_t addr, uint8_t value);

		uint8_t ReadRAM(uint16_t addr) override;
		void WriteRAM(uint16_t addr, uint8_t value) override;

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
