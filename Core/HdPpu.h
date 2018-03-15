#pragma once
#include "stdafx.h"
#include "PPU.h"
#include "CPU.h"
#include "HdNesPack.h"
#include "VideoDecoder.h"
#include "RewindManager.h"
#include "HdPackConditions.h"

class ControlManager;

class HdPpu : public PPU
{
private:
	HdScreenInfo *_screenInfo[2];
	HdScreenInfo *_info;
	bool _isChrRam;
	uint32_t _version;
	HdPackData *_hdData = nullptr;

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

			HdPpuPixelInfo &tileInfo = _info->ScreenTiles[bufferOffset];

			tileInfo.Grayscale = _paletteRamMask == 0x30;
			tileInfo.EmphasisBits = _intensifyColorBits >> 6;
			tileInfo.Tile.PpuBackgroundColor = ReadPaletteRAM(0);
			tileInfo.Tile.BgColorIndex = backgroundColor;
			if(backgroundColor == 0) {
				tileInfo.Tile.BgColor = tileInfo.Tile.PpuBackgroundColor;
			} else {
				tileInfo.Tile.BgColor = ReadPaletteRAM(lastTile->PaletteOffset + backgroundColor);
			}

			tileInfo.XScroll = _state.XScroll;
			tileInfo.TmpVideoRamAddr = _state.TmpVideoRamAddr;

			if(_lastSprite && _flags.SpritesEnabled) {
				int j = 0;
				for(uint8_t i = 0; i < _spriteCount; i++) {
					int32_t shift = (int32_t)_cycle - _spriteTiles[i].SpriteX - 1;
					SpriteInfo& sprite = _spriteTiles[i];
					if(shift >= 0 && shift < 8) {
						tileInfo.Sprite[j].TileIndex = sprite.AbsoluteTileAddr / 16;
						if(_isChrRam) {
							_mapper->CopyChrRamTile(sprite.AbsoluteTileAddr & 0xFFFFFFF0, tileInfo.Sprite[j].TileData);
						}
						if(_version >= 100) {
							tileInfo.Sprite[j].PaletteColors = 0xFF000000 | _paletteRAM[sprite.PaletteOffset + 3] | (_paletteRAM[sprite.PaletteOffset + 2] << 8) | (_paletteRAM[sprite.PaletteOffset + 1] << 16);
						} else {
							tileInfo.Sprite[j].PaletteColors = _paletteRAM[sprite.PaletteOffset + 3] | (_paletteRAM[sprite.PaletteOffset + 2] << 8) | (_paletteRAM[sprite.PaletteOffset + 1] << 16);
						}
						if(sprite.OffsetY >= 8) {
							tileInfo.Sprite[j].OffsetY = sprite.OffsetY - 8;
						} else {
							tileInfo.Sprite[j].OffsetY = sprite.OffsetY;
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
				if(_isChrRam) {
					_mapper->CopyChrRamTile(lastTile->AbsoluteTileAddr & 0xFFFFFFF0, tileInfo.Tile.TileData);
				}
				if(_version >= 100) {
					tileInfo.Tile.PaletteColors = _paletteRAM[lastTile->PaletteOffset + 3] | (_paletteRAM[lastTile->PaletteOffset + 2] << 8) | (_paletteRAM[lastTile->PaletteOffset + 1] << 16) | (_paletteRAM[0] << 24);
				} else {
					tileInfo.Tile.PaletteColors = _paletteRAM[lastTile->PaletteOffset + 3] | (_paletteRAM[lastTile->PaletteOffset + 2] << 8) | (_paletteRAM[lastTile->PaletteOffset + 1] << 16);
				}
				tileInfo.Tile.OffsetY = lastTile->OffsetY;
				tileInfo.Tile.OffsetX = (_state.XScroll + ((_cycle - 1) & 0x07)) & 0x07;
			} else {
				tileInfo.Tile.TileIndex = HdPpuTileInfo::NoTile;
			}
		} else {
			//"If the current VRAM address points in the range $3F00-$3FFF during forced blanking, the color indicated by this palette location will be shown on screen instead of the backdrop color."
			pixel = ReadPaletteRAM(_state.VideoRamAddr) | _intensifyColorBits;
			_info->ScreenTiles[bufferOffset].Tile.TileIndex = HdPpuTileInfo::NoTile;
			_info->ScreenTiles[bufferOffset].SpriteCount = 0;
		}
	}

public:
	HdPpu(BaseMapper* mapper, ControlManager* controlManager, HdPackData* hdData) : PPU(mapper, controlManager)
	{
		_isChrRam = !_mapper->HasChrRom();
		_hdData = hdData;
		_version = _hdData->Version;

		_screenInfo[0] = new HdScreenInfo(_isChrRam);
		_screenInfo[1] = new HdScreenInfo(_isChrRam);
		
		_info = _screenInfo[0];
	}

	~HdPpu()
	{
		delete _screenInfo[0];
		delete _screenInfo[1];
	}

	void SendFrame()
	{
		MessageManager::SendNotification(ConsoleNotificationType::PpuFrameDone, _currentOutputBuffer);

		_info->FrameNumber = _frameCount;
		_info->WatchedAddressValues.clear();
		for(uint32_t address : _hdData->WatchedMemoryAddresses) {
			if(address & HdPackBaseMemoryCondition::PpuMemoryMarker) {
				if((address & 0x3FFF) >= 0x3F00) {
					_info->WatchedAddressValues[address] = ReadPaletteRAM(address);
				} else {
					_info->WatchedAddressValues[address] = _mapper->DebugReadVRAM(address & 0x3FFF, true);
				}
			} else {
				_info->WatchedAddressValues[address] = CPU::DebugReadByte(address);
			}
		}

#ifdef  LIBRETRO
		VideoDecoder::GetInstance()->UpdateFrameSync(_currentOutputBuffer, _info);
#else
		if(RewindManager::IsRewinding()) {
			VideoDecoder::GetInstance()->UpdateFrameSync(_currentOutputBuffer, _info);
		} else {
			VideoDecoder::GetInstance()->UpdateFrame(_currentOutputBuffer, _info);
		}
		_currentOutputBuffer = (_currentOutputBuffer == _outputBuffers[0]) ? _outputBuffers[1] : _outputBuffers[0];
		_info = (_info == _screenInfo[0]) ? _screenInfo[1] : _screenInfo[0];
#endif
	}
};