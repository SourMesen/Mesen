#pragma once

#include "stdafx.h"
#include "Snapshotable.h"
#include "EmulationSettings.h"
#include "Types.h"
#include "DebuggerTypes.h"
#include "IMemoryHandler.h"

enum class NesModel;

class BaseMapper;
class ControlManager;
class Console;

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
		shared_ptr<Console> _console;
		EmulationSettings* _settings;

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
		uint16_t _standardVblankEnd;
		uint16_t _standardNmiScanline;
		uint16_t _vblankEnd;
		uint16_t _nmiScanline;
		uint16_t _palSpriteEvalScanline;
		
		PPUControlFlags _flags;
		PPUStatusFlags _statusFlags;

		uint16_t _intensifyColorBits;
		uint8_t _paletteRamMask;
		int32_t _lastUpdatedPixel;

		SpriteInfo *_lastSprite; //used by HD ppu

		uint16_t _ppuBusAddress;
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

		bool _needStateUpdate;
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

		void ProcessStatusRegOpenBus(uint8_t & openBusMask, uint8_t & returnValue);

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

		__forceinline uint8_t ReadSpriteRam(uint8_t addr);
		__forceinline void WriteSpriteRam(uint8_t addr, uint8_t value);

		void UpdateMinimumDrawCycles();

		__forceinline uint8_t GetPixelColor();
		__forceinline virtual void DrawPixel();
		void UpdateGrayscaleAndIntensifyBits();
		virtual void SendFrame();

		void UpdateState();

		void UpdateApuStatus();

		PPURegisters GetRegisterID(uint16_t addr)
		{
			if(addr == 0x4014) {
				return PPURegisters::SpriteDMA;
			} else {
				return (PPURegisters)(addr & 0x07);
			}
		}

		__forceinline void SetBusAddress(uint16_t addr);
		__forceinline uint8_t ReadVram(uint16_t addr, MemoryOperationType type = MemoryOperationType::PpuRenderingRead);
		__forceinline void WriteVram(uint16_t addr, uint8_t value);

		void StreamState(bool saving) override;

	public:
		static constexpr int32_t ScreenWidth = 256;
		static constexpr int32_t ScreenHeight = 240;
		static constexpr int32_t PixelCount = 256*240;
		static constexpr int32_t OutputBufferSize = 256*240*2;
		static constexpr int32_t OamDecayCycleCount = 3000;

		PPU(shared_ptr<Console> console);
		virtual ~PPU();

		void Reset();

		void DebugSendFrame();
		void DebugCopyOutputBuffer(uint16_t *target);
		void DebugUpdateFrameBuffer(bool toGrayscale);
		void GetState(PPUDebugState &state);
		void SetState(PPUDebugState &state);

		void GetMemoryRanges(MemoryRanges &ranges) override
		{
			ranges.AddHandler(MemoryOperation::Read, 0x2000, 0x3FFF);
			ranges.AddHandler(MemoryOperation::Write, 0x2000, 0x3FFF);
			ranges.AddHandler(MemoryOperation::Write, 0x4014);
		}

		__forceinline uint8_t ReadPaletteRAM(uint16_t addr);
		void WritePaletteRAM(uint16_t addr, uint8_t value);

		uint8_t ReadRAM(uint16_t addr) override;
		uint8_t PeekRAM(uint16_t addr) override;
		void WriteRAM(uint16_t addr, uint8_t value) override;

		void SetNesModel(NesModel model);
		double GetOverclockRate();
		
		void Exec();
		void ProcessCpuClock();

		uint32_t GetFrameCount()
		{
			return _frameCount;
		}

		uint32_t GetFrameCycle()
		{
			return ((_scanline + 1) * 341) + _cycle;
		}

		PPUControlFlags GetControlFlags()
		{
			return _flags;
		}

		uint32_t GetCurrentCycle()
		{
			return _cycle;
		}

		int32_t GetCurrentScanline()
		{
			return _scanline;
		}
		
		uint8_t* GetSpriteRam();

		uint8_t* GetSecondarySpriteRam()
		{
			return _secondarySpriteRAM;
		}
		
		uint32_t GetPixelBrightness(uint8_t x, uint8_t y);

		uint16_t GetPixel(uint8_t x, uint8_t y)
		{
			return _currentOutputBuffer[y << 8 | x];
		}
};
