#pragma once
#include "stdafx.h"
#include "PPU.h"
#include "HdNesPack.h"
#include "VideoDecoder.h"
#include "RewindManager.h"
#include "HdPackBuilder.h"
#include "HdPpu.h"

class ControlManager;

class HdBuilderPpu : public HdPpu
{
private:
	HdPackBuilder* _hdPackBuilder;
	bool _needChrHash = false;
	uint32_t _chrRamBankSize;
	uint32_t _chrRamIndexMask;
	vector<uint32_t> _bankHashes;
	HdPpuTileInfo sprite;
	HdPpuTileInfo tile;

protected:
	void DrawPixel()
	{
		if(IsRenderingEnabled() || ((_state.VideoRamAddr & 0x3F00) != 0x3F00)) {
			bool isChrRam = !_console->GetMapper()->HasChrRom();
			BaseMapper *mapper = _console->GetMapper();

			_lastSprite = nullptr;
			uint32_t color = GetPixelColor();
			_currentOutputBuffer[(_scanline << 8) + _cycle - 1] = _paletteRAM[color & 0x03 ? color : 0];
			uint32_t backgroundColor = 0;
			if(_flags.BackgroundEnabled && _cycle > _minimumDrawBgCycle) {
				backgroundColor = (((_state.LowBitShift << _state.XScroll) & 0x8000) >> 15) | (((_state.HighBitShift << _state.XScroll) & 0x8000) >> 14);
			}
			
			if(_needChrHash) {
				uint16_t addr = 0;
				_bankHashes.clear();
				while(addr < 0x2000) {
					uint32_t hash = 0;
					for(uint16_t i = 0; i < _chrRamBankSize; i++) {
						hash += _console->GetMapper()->DebugReadVRAM(i + addr);
						hash = (hash << 1) | (hash >> 31);
					}
					_bankHashes.push_back(hash);
					addr += _chrRamBankSize;
				}
				_needChrHash = false;
			}

			bool hasBgSprite = false;
			if(_lastSprite && _flags.SpritesEnabled) {
				if(backgroundColor == 0) {
					for(uint8_t i = 0; i < _spriteCount; i++) {
						if(_spriteTiles[i].BackgroundPriority) {
							hasBgSprite = true;
							break;
						}
					}
				}

				if(_lastSprite->AbsoluteTileAddr >= 0) {
					sprite.TileIndex = (isChrRam ? (_lastSprite->TileAddr & _chrRamIndexMask) : _lastSprite->AbsoluteTileAddr) / 16;
					sprite.PaletteColors = ReadPaletteRAM(_lastSprite->PaletteOffset + 3) | (ReadPaletteRAM(_lastSprite->PaletteOffset + 2) << 8) | (ReadPaletteRAM(_lastSprite->PaletteOffset + 1) << 16) | 0xFF000000;
					sprite.IsChrRamTile = isChrRam;
					_console->GetMapper()->CopyChrTile(_lastSprite->AbsoluteTileAddr & 0xFFFFFFF0, sprite.TileData);

					_hdPackBuilder->ProcessTile(_cycle - 1, _scanline, _lastSprite->AbsoluteTileAddr, sprite, mapper, false, _bankHashes[_lastSprite->TileAddr / _chrRamBankSize], false);
				}
			}

			if(_flags.BackgroundEnabled) {
				TileInfo* lastTile = &((_state.XScroll + ((_cycle - 1) & 0x07) < 8) ? _previousTile : _currentTile);
				if(lastTile->AbsoluteTileAddr >= 0) {
					tile.TileIndex = (isChrRam ? (lastTile->TileAddr & _chrRamIndexMask) : lastTile->AbsoluteTileAddr) / 16;
					tile.PaletteColors = ReadPaletteRAM(lastTile->PaletteOffset + 3) | (ReadPaletteRAM(lastTile->PaletteOffset + 2) << 8) | (ReadPaletteRAM(lastTile->PaletteOffset + 1) << 16) | (ReadPaletteRAM(0) << 24);
					tile.IsChrRamTile = isChrRam;
					_console->GetMapper()->CopyChrTile(lastTile->AbsoluteTileAddr & 0xFFFFFFF0, tile.TileData);

					_hdPackBuilder->ProcessTile(_cycle - 1, _scanline, lastTile->AbsoluteTileAddr, tile, mapper, false, _bankHashes[lastTile->TileAddr / _chrRamBankSize], hasBgSprite);
				}
			}
		} else {
			//"If the current VRAM address points in the range $3F00-$3FFF during forced blanking, the color indicated by this palette location will be shown on screen instead of the backdrop color."
			_currentOutputBuffer[(_scanline << 8) + _cycle - 1] = _paletteRAM[_state.VideoRamAddr & 0x1F];
		}

		if(_hdData) {
			HdPpu::DrawPixel();
		}
	}

	void WriteRAM(uint16_t addr, uint8_t value)
	{
		if(GetRegisterID(addr) == PPURegisters::VideoMemoryData) {
			if(_state.VideoRamAddr < 0x2000) {
				_needChrHash = true;
			}
		}
		PPU::WriteRAM(addr, value);
	}

	void StreamState(bool saving)
	{
		PPU::StreamState(saving);
		if(!saving) {
			_needChrHash = true;
		}
	}

public:
	HdBuilderPpu(shared_ptr<Console> console, HdPackBuilder* hdPackBuilder, uint32_t chrRamBankSize, shared_ptr<HdPackData> hdData) : HdPpu(console, hdData.get())
	{
		_hdPackBuilder = hdPackBuilder;
		_chrRamBankSize = chrRamBankSize;
		_chrRamIndexMask = chrRamBankSize - 1;
		_needChrHash = true;
	}

	void SendFrame()
	{
		if(_hdData) {
			HdPpu::SendFrame();
		} else {
			PPU::SendFrame();
		}
	}
};