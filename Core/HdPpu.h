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
	bool _isChrRam;
	uint32_t _version;

protected:
	void DrawPixel()
	{
		uint16_t bufferOffset = (_scanline << 8) + _cycle - 1;
		uint16_t &pixel = _currentOutputBuffer[bufferOffset];
		_lastSprite = nullptr;

		if(IsRenderingEnabled() || ((_state.VideoRamAddr & 0x3F00) != 0x3F00)) {
			uint32_t color = GetPixelColor();
			pixel = (_paletteRAM[color & 0x03 ? color : 0] & _paletteRamMask) | _intensifyColorBits;
			
			TileInfo* lastTile = &((_state.XScroll + ((_cycle - 1) & 0x07) < 8) ? _previousTile : _currentTile);
			uint32_t backgroundColor = 0;
			if(_flags.BackgroundEnabled && _cycle > _minimumDrawBgCycle) {
				backgroundColor = (((_state.LowBitShift << _state.XScroll) & 0x8000) >> 15) | (((_state.HighBitShift << _state.XScroll) & 0x8000) >> 14);
			}

			HdPpuPixelInfo &tileInfo = _screenTiles[bufferOffset];

			tileInfo.Tile.PpuBackgroundColor = ReadPaletteRAM(0);
			tileInfo.Tile.BgColorIndex = backgroundColor;
			if(backgroundColor == 0) {
				tileInfo.Tile.BgColor = tileInfo.Tile.PpuBackgroundColor;
			} else {
				tileInfo.Tile.BgColor = ReadPaletteRAM(lastTile->PaletteOffset + backgroundColor);
			}

			if(_lastSprite && _flags.SpritesEnabled) {
				int j = 0;
				for(uint8_t i = 0; i < _spriteCount; i++) {
					int32_t shift = (int32_t)_cycle - _spriteTiles[i].SpriteX - 1;
					SpriteInfo& sprite = _spriteTiles[i];
					if(shift >= 0 && shift < 8) {
						tileInfo.Sprite[j].TileIndex = sprite.AbsoluteTileAddr / 16;
						if(_version >= 100) {
							tileInfo.Sprite[j].PaletteColors = 0xFF000000 | ReadPaletteRAM(sprite.PaletteOffset + 3) | (ReadPaletteRAM(sprite.PaletteOffset + 2) << 8) | (ReadPaletteRAM(sprite.PaletteOffset + 1) << 16);
						} else {
							tileInfo.Sprite[j].PaletteColors = ReadPaletteRAM(sprite.PaletteOffset + 3) | (ReadPaletteRAM(sprite.PaletteOffset + 2) << 8) | (ReadPaletteRAM(sprite.PaletteOffset + 1) << 16);
						}
						tileInfo.Sprite[j].OffsetY = sprite.OffsetY;
						if(tileInfo.Sprite[j].OffsetY >= 8) {
							tileInfo.Sprite[j].OffsetY -= 8;
						}
						tileInfo.Sprite[j].IsChrRamTile = _isChrRam;
						if(_isChrRam) {
							for(int k = 0; k < 16; k++) {
								tileInfo.Sprite[j].TileData[k] = _mapper->GetMemoryValue(DebugMemoryType::ChrRom, sprite.AbsoluteTileAddr / 16 * 16 + k);
							}
						}

						tileInfo.Sprite[j].OffsetX = shift;
						tileInfo.Sprite[j].HorizontalMirroring = sprite.HorizontalMirror;
						tileInfo.Sprite[j].VerticalMirroring = sprite.VerticalMirror;
						tileInfo.Sprite[j].BackgroundPriority = sprite.BackgroundPriority;

						int32_t shift = (int32_t)_cycle - sprite.SpriteX - 1;
						if(sprite.HorizontalMirror) {
							tileInfo.Sprite[j].SpriteColorIndex = ((sprite.LowByte >> shift) & 0x01) | ((sprite.HighByte >> shift) & 0x01) << 1;
						} else {
							tileInfo.Sprite[j].SpriteColorIndex = ((sprite.LowByte << shift) & 0x80) >> 7 | ((sprite.HighByte << shift) & 0x80) >> 6;
						}

						if(tileInfo.Sprite[j].SpriteColorIndex == 0) {
							tileInfo.Sprite[j].SpriteColor = ReadPaletteRAM(0);
						} else {
							tileInfo.Sprite[j].SpriteColor = ReadPaletteRAM(sprite.PaletteOffset + tileInfo.Sprite[j].SpriteColorIndex);
						}

						tileInfo.Sprite[j].PpuBackgroundColor = tileInfo.Tile.PpuBackgroundColor;
						tileInfo.Sprite[j].BgColorIndex = tileInfo.Tile.BgColorIndex;

						j++;
						if(j >= 4) {
							break;
						}
					}
				}
				tileInfo.SpriteCount = j;
			} else {
				tileInfo.SpriteCount = 0;
			}

			if(_flags.BackgroundEnabled && _cycle > _minimumDrawBgCycle) {
				tileInfo.Tile.TileIndex = lastTile->AbsoluteTileAddr / 16;
				if(_version >= 100) {
					tileInfo.Tile.PaletteColors = ReadPaletteRAM(lastTile->PaletteOffset + 3) | (ReadPaletteRAM(lastTile->PaletteOffset + 2) << 8) | (ReadPaletteRAM(lastTile->PaletteOffset + 1) << 16) | (ReadPaletteRAM(0) << 24);
				} else {
					tileInfo.Tile.PaletteColors = ReadPaletteRAM(lastTile->PaletteOffset + 3) | (ReadPaletteRAM(lastTile->PaletteOffset + 2) << 8) | (ReadPaletteRAM(lastTile->PaletteOffset + 1) << 16);
				}
				tileInfo.Tile.OffsetY = lastTile->OffsetY;
				tileInfo.Tile.BackgroundPriority = false;
				tileInfo.Tile.IsChrRamTile = _isChrRam;
				if(_isChrRam) {
					for(int i = 0; i < 16; i++) {
						tileInfo.Tile.TileData[i] = _mapper->GetMemoryValue(DebugMemoryType::ChrRom, lastTile->AbsoluteTileAddr / 16 * 16 + i);
					}
				}

				tileInfo.Tile.OffsetX = (_state.XScroll + ((_cycle - 1) & 0x07)) & 0x07;
				tileInfo.Tile.HorizontalMirroring = false;
				tileInfo.Tile.VerticalMirroring = false;
			} else {
				tileInfo.Tile.TileIndex = HdPpuTileInfo::NoTile;
			}
		} else {
			//"If the current VRAM address points in the range $3F00-$3FFF during forced blanking, the color indicated by this palette location will be shown on screen instead of the backdrop color."
			pixel = ReadPaletteRAM(_state.VideoRamAddr) | _intensifyColorBits;
			_screenTiles[bufferOffset].Tile.TileIndex = HdPpuTileInfo::NoTile;
			_screenTiles[bufferOffset].SpriteCount = 0;
		}
	}

public:
	HdPpu(BaseMapper* mapper, uint32_t version) : PPU(mapper)
	{
		_screenTileBuffers[0] = new HdPpuPixelInfo[256 * 240];
		_screenTileBuffers[1] = new HdPpuPixelInfo[256 * 240];
		_screenTiles = _screenTileBuffers[0];
		_isChrRam = !_mapper->HasChrRom();
		_version = version;
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