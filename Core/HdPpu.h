#pragma once
#include "stdafx.h"
#include "PPU.h"
#include "HdNesPack.h"

class HdPpu : public PPU
{
private:
	HdPpuPixelInfo* _screenTiles;

protected:
	void DrawPixel()
	{
		uint16_t bufferOffset = (_scanline << 8) + _cycle - 1;
		uint16_t &pixel = _outputBuffer[bufferOffset];
		_lastSprite = nullptr;

		if(IsRenderingEnabled() || ((_state.VideoRamAddr & 0x3F00) != 0x3F00)) {
			uint32_t paletteOffset;
			uint32_t color = GetPixelColor(paletteOffset);
			if(color == 0) {
				pixel = ReadPaletteRAM(0) | _intensifyColorBits;
			} else {
				pixel = ReadPaletteRAM(paletteOffset + color) | _intensifyColorBits;
			}
			
			HdPpuPixelInfo &tileInfo = _screenTiles[bufferOffset];
			if(_lastSprite) {
				tileInfo.Sprite.TileIndex = _memoryManager->ToAbsoluteChrAddress(_lastSprite->TileAddr) / 16;
				tileInfo.Sprite.PaletteColors = ReadPaletteRAM(_lastSprite->PaletteOffset + 1) | (ReadPaletteRAM(_lastSprite->PaletteOffset + 2) << 8) | (ReadPaletteRAM(_lastSprite->PaletteOffset + 3) << 16);
				tileInfo.Sprite.OffsetY = _lastSprite->OffsetY;

				tileInfo.Sprite.OffsetX = _cycle - _lastSprite->SpriteX - 1;
				tileInfo.Sprite.HorizontalMirroring = _lastSprite->HorizontalMirror;
				tileInfo.Sprite.BackgroundPriority = _lastSprite->BackgroundPriority;

				uint32_t backgroundColor = 0;
				if((_cycle > 8 || _flags.BackgroundMask) && _flags.BackgroundEnabled) {
					//BackgroundMask = false: Hide background in leftmost 8 pixels of screen
					backgroundColor = (((_state.LowBitShift << _state.XScroll) & 0x8000) >> 15) | (((_state.HighBitShift << _state.XScroll) & 0x8000) >> 14);
				}

				paletteOffset = ((_state.XScroll + ((_cycle - 1) & 0x07) < 8) ? _previousTile.PaletteOffset : _currentTile.PaletteOffset);
				if(backgroundColor == 0) {
					tileInfo.Sprite.BgColor = ReadPaletteRAM(0);
				} else {
					tileInfo.Sprite.BgColor = ReadPaletteRAM(paletteOffset + backgroundColor);
				}
			} else {
				tileInfo.Sprite.TileIndex = HdPpuTileInfo::NoTile;
			}
			TileInfo* lastTile = &((_state.XScroll + ((_cycle - 1) & 0x07) < 8) ? _previousTile : _currentTile);
			tileInfo.Tile.TileIndex = _memoryManager->ToAbsoluteChrAddress(lastTile->TileAddr) / 16;
			tileInfo.Tile.PaletteColors = ReadPaletteRAM(lastTile->PaletteOffset + 1) | (ReadPaletteRAM(lastTile->PaletteOffset + 2) << 8) | (ReadPaletteRAM(lastTile->PaletteOffset + 3) << 16);
			tileInfo.Tile.OffsetY = lastTile->OffsetY;

			tileInfo.Tile.OffsetX = (_state.XScroll + ((_cycle - 1) & 0x07)) & 0x07;
			tileInfo.Tile.HorizontalMirroring = false;
			tileInfo.Tile.BgColor = ReadPaletteRAM(0);
		} else {
			//"If the current VRAM address points in the range $3F00-$3FFF during forced blanking, the color indicated by this palette location will be shown on screen instead of the backdrop color."
			pixel = ReadPaletteRAM(_state.VideoRamAddr) | _intensifyColorBits;
			_screenTiles[bufferOffset].Tile.TileIndex = HdPpuTileInfo::NoTile;
			_screenTiles[bufferOffset].Sprite.TileIndex = HdPpuTileInfo::NoTile;
		}
	}

public:
	HdPpu(MemoryManager* memoryManager) : PPU(memoryManager)
	{
		_screenTiles = new HdPpuPixelInfo[256 * 240];
	}

	~HdPpu()
	{
		delete[] _screenTiles;
	}

	void SendFrame()
	{
		if(VideoDevice) {
			VideoDevice->UpdateHdFrame(_outputBuffer, _screenTiles);
		}
	}
};