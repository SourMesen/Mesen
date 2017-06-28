#pragma once
#include "stdafx.h"
#include "PPU.h"
#include "HdNesPack.h"
#include "VideoDecoder.h"
#include "RewindManager.h"
#include "HdPackBuilder.h"

class HdBuilderPpu : public PPU
{
private:
	HdPackBuilder* _hdPackBuilder;
	bool _isChrRam;
	bool _needChrHash = false;
	uint32_t _chrRamBankSize;
	uint32_t _chrRamIndexMask;
	vector<uint32_t> _bankHashes;

protected:
	void DrawPixel()
	{
		if(IsRenderingEnabled() || ((_state.VideoRamAddr & 0x3F00) != 0x3F00)) {
			_lastSprite = nullptr;
			uint32_t color = GetPixelColor();
			_currentOutputBuffer[(_scanline << 8) + _cycle - 1] = _paletteRAM[color & 0x03 ? color : 0];
			
			if(_needChrHash) {
				uint16_t addr = 0;
				_bankHashes.clear();
				while(addr < 0x2000) {
					uint32_t hash = 0;
					for(uint16_t i = 0; i < _chrRamBankSize; i++) {
						hash += _mapper->DebugReadVRAM(i + addr);
						hash = (hash << 1) | (hash >> 31);
					}
					_bankHashes.push_back(hash);
					addr += _chrRamBankSize;
				}
				_needChrHash = false;
			}

			HdPpuPixelInfo tileInfo;
			if(_lastSprite && _flags.SpritesEnabled) {
				if(_lastSprite->AbsoluteTileAddr >= 0) {
					tileInfo.Sprite.TileIndex = (_isChrRam ? (_lastSprite->TileAddr & _chrRamIndexMask) : _lastSprite->AbsoluteTileAddr) / 16;
					tileInfo.Sprite.PaletteColors = ReadPaletteRAM(_lastSprite->PaletteOffset + 3) | (ReadPaletteRAM(_lastSprite->PaletteOffset + 2) << 8) | (ReadPaletteRAM(_lastSprite->PaletteOffset + 1) << 16) | 0xFF000000;
					tileInfo.Sprite.IsChrRamTile = _isChrRam;
					for(int i = 0; i < 16; i++) {
						tileInfo.Sprite.TileData[i] = _mapper->GetMemoryValue(DebugMemoryType::ChrRom, _lastSprite->AbsoluteTileAddr / 16 * 16 + i);
					}

					_hdPackBuilder->ProcessTile(_cycle - 1, _scanline, _lastSprite->AbsoluteTileAddr, tileInfo.Sprite, _mapper, false, _bankHashes[_lastSprite->TileAddr / _chrRamBankSize]);
				}
			}

			if(_flags.BackgroundEnabled) {
				TileInfo* lastTile = &((_state.XScroll + ((_cycle - 1) & 0x07) < 8) ? _previousTile : _currentTile);
				if(lastTile->AbsoluteTileAddr >= 0) {
					tileInfo.Tile.TileIndex = (_isChrRam ? (lastTile->TileAddr & _chrRamIndexMask) : lastTile->AbsoluteTileAddr) / 16;
					tileInfo.Tile.PaletteColors = ReadPaletteRAM(lastTile->PaletteOffset + 3) | (ReadPaletteRAM(lastTile->PaletteOffset + 2) << 8) | (ReadPaletteRAM(lastTile->PaletteOffset + 1) << 16) | (ReadPaletteRAM(0) << 24);
					tileInfo.Tile.IsChrRamTile = _isChrRam;
					for(int i = 0; i < 16; i++) {
						tileInfo.Tile.TileData[i] = _mapper->GetMemoryValue(DebugMemoryType::ChrRom, lastTile->AbsoluteTileAddr / 16 * 16 + i);
					}

					_hdPackBuilder->ProcessTile(_cycle - 1, _scanline, lastTile->AbsoluteTileAddr, tileInfo.Tile, _mapper, false, _bankHashes[lastTile->TileAddr / _chrRamBankSize]);
				}
			}
		} else {
			//"If the current VRAM address points in the range $3F00-$3FFF during forced blanking, the color indicated by this palette location will be shown on screen instead of the backdrop color."
			_currentOutputBuffer[(_scanline << 8) + _cycle - 1] = _paletteRAM[_state.VideoRamAddr & 0x1F];
		}
	}

	void WriteRAM(uint16_t addr, uint8_t value)
	{
		switch(GetRegisterID(addr)) {
			case PPURegisters::VideoMemoryData:
				if(_state.VideoRamAddr < 0x2000) {
					_needChrHash = true;
				}
				break;
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
	HdBuilderPpu(BaseMapper* mapper, HdPackBuilder* hdPackBuilder, uint32_t chrRamBankSize) : PPU(mapper)
	{
		_hdPackBuilder = hdPackBuilder;
		_chrRamBankSize = chrRamBankSize;
		_chrRamIndexMask = chrRamBankSize - 1;
		_isChrRam = !_mapper->HasChrRom();
		_needChrHash = true;
	}

	void SendFrame()
	{
		PPU::SendFrame();
	}
};