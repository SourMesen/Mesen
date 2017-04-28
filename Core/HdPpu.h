#pragma once
#include "stdafx.h"
#include "PPU.h"
#include "HdNesPack.h"
#include "VideoDecoder.h"
#include "RewindManager.h"

class HdPpu : public PPU
{
private:
	HdPpuPixelInfo* _screenTileBuffers[2];
	HdPpuPixelInfo* _screenTiles;

protected:
	void DrawPixel()
	{
		uint16_t bufferOffset = (_scanline << 8) + _cycle - 1;
		uint16_t &pixel = _currentOutputBuffer[bufferOffset];
		_lastSprite = nullptr;

		if(IsRenderingEnabled() || ((_state.VideoRamAddr & 0x3F00) != 0x3F00)) {
			uint32_t color = GetPixelColor();
			pixel = (_paletteRAM[color & 0x03 ? color : 0] & _paletteRamMask) | _intensifyColorBits;
			
			HdPpuPixelInfo &tileInfo = _screenTiles[bufferOffset];
			if(_lastSprite && _flags.SpritesEnabled) {
				tileInfo.Sprite.TileIndex = _mapper->ToAbsoluteChrAddress(_lastSprite->TileAddr) / 16;
				tileInfo.Sprite.PaletteColors = ReadPaletteRAM(_lastSprite->PaletteOffset + 1) | (ReadPaletteRAM(_lastSprite->PaletteOffset + 2) << 8) | (ReadPaletteRAM(_lastSprite->PaletteOffset + 3) << 16);
				tileInfo.Sprite.OffsetY = _lastSprite->OffsetY;

				tileInfo.Sprite.OffsetX = _cycle - _lastSprite->SpriteX - 1;
				tileInfo.Sprite.HorizontalMirroring = _lastSprite->HorizontalMirror;
				tileInfo.Sprite.VerticalMirroring = _lastSprite->VerticalMirror;
				tileInfo.Sprite.BackgroundPriority = _lastSprite->BackgroundPriority;

				uint32_t backgroundColor = 0;
				if((_cycle > 8 || _flags.BackgroundMask) && _flags.BackgroundEnabled) {
					//BackgroundMask = false: Hide background in leftmost 8 pixels of screen
					backgroundColor = (((_state.LowBitShift << _state.XScroll) & 0x8000) >> 15) | (((_state.HighBitShift << _state.XScroll) & 0x8000) >> 14);
				}

				tileInfo.Sprite.BgColorIndex = backgroundColor;
				if(backgroundColor == 0) {
					tileInfo.Sprite.BgColor = ReadPaletteRAM(0);
				} else {
					tileInfo.Sprite.BgColor = ReadPaletteRAM(_lastSprite->PaletteOffset + backgroundColor);
				}
			} else {
				tileInfo.Sprite.TileIndex = HdPpuTileInfo::NoTile;
			}

			if(_flags.BackgroundEnabled) {
				TileInfo* lastTile = &((_state.XScroll + ((_cycle - 1) & 0x07) < 8) ? _previousTile : _currentTile);
				tileInfo.Tile.TileIndex = _mapper->ToAbsoluteChrAddress(lastTile->TileAddr) / 16;
				tileInfo.Tile.PaletteColors = ReadPaletteRAM(lastTile->PaletteOffset + 1) | (ReadPaletteRAM(lastTile->PaletteOffset + 2) << 8) | (ReadPaletteRAM(lastTile->PaletteOffset + 3) << 16);
				tileInfo.Tile.OffsetY = lastTile->OffsetY;
				tileInfo.Tile.BackgroundPriority = false;

				tileInfo.Tile.OffsetX = (_state.XScroll + ((_cycle - 1) & 0x07)) & 0x07;
				tileInfo.Tile.HorizontalMirroring = false;
				tileInfo.Tile.VerticalMirroring = false;
				tileInfo.Tile.BgColor = ReadPaletteRAM(0);
			} else {
				tileInfo.Tile.TileIndex = HdPpuTileInfo::NoTile;
			}
		} else {
			//"If the current VRAM address points in the range $3F00-$3FFF during forced blanking, the color indicated by this palette location will be shown on screen instead of the backdrop color."
			pixel = ReadPaletteRAM(_state.VideoRamAddr) | _intensifyColorBits;
			_screenTiles[bufferOffset].Tile.TileIndex = HdPpuTileInfo::NoTile;
			_screenTiles[bufferOffset].Sprite.TileIndex = HdPpuTileInfo::NoTile;
		}
	}

public:
	HdPpu(BaseMapper* mapper) : PPU(mapper)
	{
		_screenTileBuffers[0] = new HdPpuPixelInfo[256 * 240];
		_screenTileBuffers[1] = new HdPpuPixelInfo[256 * 240];
		_screenTiles = _screenTileBuffers[0];
	}

	~HdPpu()
	{
		delete[] _screenTileBuffers[0];
		delete[] _screenTileBuffers[1];
	}

	void SendFrame()
	{
		MessageManager::SendNotification(ConsoleNotificationType::PpuFrameDone, _currentOutputBuffer);

		if(RewindManager::IsRewinding()) {
			VideoDecoder::GetInstance()->UpdateFrameSync(_currentOutputBuffer, _screenTiles);
		} else {
			VideoDecoder::GetInstance()->UpdateFrame(_currentOutputBuffer, _screenTiles);
		}

		_currentOutputBuffer = (_currentOutputBuffer == _outputBuffers[0]) ? _outputBuffers[1] : _outputBuffers[0];
		_screenTiles = (_screenTiles == _screenTileBuffers[0]) ? _screenTileBuffers[1] : _screenTileBuffers[0];
	}
};