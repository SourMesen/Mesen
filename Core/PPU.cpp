#include "stdafx.h"
#include "PPU.h"
#include "CPU.h"

IVideoDevice *PPU::VideoDevice = nullptr;

uint32_t PPU_PALETTE_RGB[] = {
    0x666666, 0x002A88, 0x1412A7, 0x3B00A4, 0x5C007E,
    0x6E0040, 0x6C0600, 0x561D00, 0x333500, 0x0B4800,
    0x005200, 0x004F08, 0x00404D, 0x000000, 0x000000,
    0x000000, 0xADADAD, 0x155FD9, 0x4240FF, 0x7527FE,
    0xA01ACC, 0xB71E7B, 0xB53120, 0x994E00, 0x6B6D00,
    0x388700, 0x0C9300, 0x008F32, 0x007C8D, 0x000000,
    0x000000, 0x000000, 0xFFFEFF, 0x64B0FF, 0x9290FF,
    0xC676FF, 0xF36AFF, 0xFE6ECC, 0xFE8170, 0xEA9E22,
    0xBCBE00, 0x88D800, 0x5CE430, 0x45E082, 0x48CDDE,
    0x4F4F4F, 0x000000, 0x000000, 0xFFFEFF, 0xC0DFFF,
    0xD3D2FF, 0xE8C8FF, 0xFBC2FF, 0xFEC4EA, 0xFECCC5,
    0xF7D8A5, 0xE4E594, 0xCFEF96, 0xBDF4AB, 0xB3F3CC,
    0xB5EBF2, 0xB8B8B8, 0x000000, 0x000000,
};

PPU::PPU(MemoryManager *memoryManager)
{
	_memoryManager = memoryManager;
	_state = {};
	_flags = {};
	_statusFlags = {};

	memset(_spriteRAM, 0xFF, 0x100);

	_outputBuffer = new uint8_t[256 * 240 * 4];
}

PPU::~PPU() 
{
	delete[] _outputBuffer;
}

bool PPU::CheckFlag(PPUControlFlags flag)
{
	return false;
}

uint8_t PPU::ReadRAM(uint16_t addr)
{
	uint8_t returnValue;
	switch(GetRegisterID(addr)) {
		case PPURegisters::Status:
			_state.WriteToggle = false;
			_flags.IntensifyBlue = false;
			UpdateStatusFlag();
			return _state.Status;
		case PPURegisters::SpriteData:
			return _spriteRAM[_state.SpriteRamAddr];
		case PPURegisters::VideoMemoryData:
			returnValue = _memoryReadBuffer;
			_memoryReadBuffer = _memoryManager->ReadVRAM(_state.VideoRamAddr);

			if(_state.VideoRamAddr >= 0x3F00) {
				returnValue = ReadPaletteRAM(_state.VideoRamAddr);
			}

			_state.VideoRamAddr += _flags.VerticalWrite ? 32 : 1;
			return returnValue;
		default:
			//other registers are meant to be read-only
			break;
	}
	return 0;
}

void PPU::WriteRAM(uint16_t addr, uint8_t value)
{
	switch(GetRegisterID(addr)) {
		case PPURegisters::Control:
			_state.Control = value;
			UpdateFlags();
			break;
		case PPURegisters::Control2:
			_state.Control2 = value;
			UpdateFlags();
			break;
		case PPURegisters::SpriteAddr:
			_state.SpriteRamAddr = value;
			break;
		case PPURegisters::SpriteData:
			_spriteRAM[_state.SpriteRamAddr] = value;
			_state.SpriteRamAddr = (_state.SpriteRamAddr + 1) % 0x100;
			break;
		case PPURegisters::ScrollOffsets:
			if(_state.WriteToggle) {
				_state.TmpVideoRamAddr = (_state.TmpVideoRamAddr & ~0x73E0) | ((value & 0xF8) << 2) | ((value & 0x07) << 12);
			} else {
				_state.XScroll = value & 0x07;
				_state.TmpVideoRamAddr = (_state.TmpVideoRamAddr & ~0x001F) | (value >> 3);
			}
			_state.WriteToggle = !_state.WriteToggle;
			break;
		case PPURegisters::VideoMemoryAddr:
			if(_state.WriteToggle) {
				_state.TmpVideoRamAddr = (_state.TmpVideoRamAddr & ~0x00FF) | value;
				_state.VideoRamAddr = _state.TmpVideoRamAddr;
			} else {
				_state.TmpVideoRamAddr = (_state.TmpVideoRamAddr & ~0xFF00) | ((value & 0x3F) << 8);
			}
			_state.WriteToggle = !_state.WriteToggle;
			break;
		case PPURegisters::VideoMemoryData:
			if(_state.VideoRamAddr >= 0x3F00) {
				WritePaletteRAM(_state.VideoRamAddr, value);
			} else {
				_memoryManager->WriteVRAM(_state.VideoRamAddr, value);
			}
			_state.VideoRamAddr += _flags.VerticalWrite ? 32 : 1;
			break;
		case PPURegisters::SpriteDMA:
			for(int i = 0; i < 0x100; i++) {
				_spriteRAM[(_state.SpriteRamAddr+i)&0xFF] = _memoryManager->Read(value*0x100 + i);
			}
			break;
	}
}

uint8_t PPU::ReadPaletteRAM(uint16_t addr)
{
	addr &= 0x1F;
	if(addr == 0x10 || addr == 0x14 || addr == 0x18 || addr == 0x1C) {
		addr &= ~0x10;
	}
	return _paletteRAM[addr];
}

void PPU::WritePaletteRAM(uint16_t addr, uint8_t value)
{
	addr &= 0x1F;
	if(addr == 0x10 || addr == 0x14 || addr == 0x18 || addr == 0x1C) {
		addr &= ~0x10;
	}
	_paletteRAM[addr] = value;
}

uint8_t PPU::GetBGPaletteEntry(uint8_t paletteOffset, uint8_t pixel)
{
	if(pixel == 0) {
		return ReadPaletteRAM(0x3F00);
	} else {
		return ReadPaletteRAM(0x3F00 + paletteOffset + pixel);
	}
}

uint8_t PPU::GetSpritePaletteEntry(uint8_t paletteOffset, uint8_t pixel)
{
	if(pixel == 0) {
		return ReadPaletteRAM(0x3F00);
	} else {
		return ReadPaletteRAM(0x3F10 + paletteOffset + pixel);
	}
}

bool PPU::IsRenderingEnabled()
{
	return _flags.BackgroundEnabled || _flags.SpritesEnabled;
}

void PPU::UpdateFlags()
{
	uint8_t nameTable = (_state.Control & 0x03);
	switch(nameTable) {
		case 0: _flags.NameTableAddr = 0x2000; break;
		case 1: _flags.NameTableAddr = 0x2400; break;
		case 2: _flags.NameTableAddr = 0x2800; break;
		case 3: _flags.NameTableAddr = 0x2C00; break;
	}
	_state.TmpVideoRamAddr = (_state.TmpVideoRamAddr & ~0xC0000) | (nameTable << 10);

	_flags.VerticalWrite = (_state.Control & 0x04) == 0x04;
	_flags.SpritePatternAddr = ((_state.Control & 0x08) == 0x08) ? 0x1000 : 0x0000;
	_flags.BackgroundPatternAddr = ((_state.Control & 0x10) == 0x10) ? 0x1000 : 0x0000;
	_flags.LargeSprites = (_state.Control & 0x20) == 0x20;
	_flags.VBlank = (_state.Control & 0x80) == 0x80;

	_flags.Grayscale = (_state.Control2 & 0x01) == 0x01;
	_flags.BackgroundMask = (_state.Control2 & 0x02) == 0x02;
	_flags.SpriteMask = (_state.Control2 & 0x04) == 0x04;
	_flags.BackgroundEnabled = (_state.Control2 & 0x08) == 0x08;
	_flags.SpritesEnabled = (_state.Control2 & 0x10) == 0x10;
	_flags.IntensifyRed = (_state.Control2 & 0x20) == 0x20;
	_flags.IntensifyGreen = (_state.Control2 & 0x40) == 0x40;
	_flags.IntensifyBlue = (_state.Control2 & 0x80) == 0x80;
}

void PPU::UpdateStatusFlag()
{
	_state.Status = ((uint8_t)_statusFlags.SpriteOverflow << 5) |
						 ((uint8_t)_statusFlags.Sprite0Hit << 6) |
						 ((uint8_t)_statusFlags.VerticalBlank << 7);
	_statusFlags.VerticalBlank = false;

	if(_scanline == 241 && _cycle == 0) {
		_doNotSetVBFlag = true;
	}
}

//Taken from http://wiki.nesdev.com/w/index.php/The_skinny_on_NES_scrolling#Wrapping_around
void PPU::IncVerticalScrolling()
{
	uint16_t addr = _state.VideoRamAddr;

	if((addr & 0x7000) != 0x7000) {
		// if fine Y < 7
		addr += 0x1000;                    // increment fine Y
	} else {
		// fine Y = 0
		addr &= ~0x7000;
		int y = (addr & 0x03E0) >> 5;	// let y = coarse Y
		if(y == 29) {
			y = 0;                  // coarse Y = 0
			addr ^= 0x0800;                  // switch vertical nametable
		} else if(y == 31){
			y = 0;              // coarse Y = 0, nametable not switched
		} else {
			y += 1;                  // increment coarse Y
		}
		addr = (addr & ~0x03E0) | (y << 5);     // put coarse Y back into v
	}
	_state.VideoRamAddr = addr;
}

//Taken from http://wiki.nesdev.com/w/index.php/The_skinny_on_NES_scrolling#Wrapping_around
void PPU::IncHorizontalScrolling()
{
	//Increase coarse X scrolling value.
	//When the value is 31, wrap around to 0 and switch nametable
	uint16_t addr = _state.VideoRamAddr;
	if((addr & 0x001F) == 31) {
		addr &= ~0x001F;
		addr ^= 0x0400; // switch horizontal nametable
	} else {
		addr += 1;
	}
	_state.VideoRamAddr = addr;
}

//Take from http://wiki.nesdev.com/w/index.php/The_skinny_on_NES_scrolling#Tile_and_attribute_fetching
uint16_t PPU::GetNameTableAddr()
{
	return 0x2000 | (_state.VideoRamAddr & 0x0FFF);
}

//Take from http://wiki.nesdev.com/w/index.php/The_skinny_on_NES_scrolling#Tile_and_attribute_fetching
uint16_t PPU::GetAttributeAddr()
{
	return 0x23C0 | (_state.VideoRamAddr & 0x0C00) | ((_state.VideoRamAddr >> 4) & 0x38) | ((_state.VideoRamAddr >> 2) & 0x07);
}

void PPU::LoadTileInfo()
{
	_previousTile = _currentTile;
	_currentTile = _nextTile;

	uint16_t tileIndex = _memoryManager->ReadVRAM(GetNameTableAddr());
	uint16_t tileAddr = (tileIndex << 4) | (_state.VideoRamAddr >> 12) | _flags.BackgroundPatternAddr;
	
	uint16_t shift = ((_state.VideoRamAddr >> 4) & 0x04) | (_state.VideoRamAddr & 0x02);
	_nextTile.PaletteOffset = ((_memoryManager->ReadVRAM(GetAttributeAddr()) >> shift) & 0x03) << 2;
	_nextTile.LowByte = _memoryManager->ReadVRAM(tileAddr);
	_nextTile.HighByte = _memoryManager->ReadVRAM(tileAddr + 8);
}

void PPU::LoadSpriteTileInfo(uint8_t spriteIndex)
{
	uint32_t spriteAddr = spriteIndex * 4;
	uint8_t spriteY = _secondarySpriteRAM[spriteAddr];
	uint8_t tileIndex = _secondarySpriteRAM[spriteAddr+1];
	uint8_t attributes = _secondarySpriteRAM[spriteAddr+2];
	uint8_t spriteX = _secondarySpriteRAM[spriteAddr+3];
	bool horizontalMirror = (attributes & 0x40) == 0x40;

	if(spriteY < 240 && spriteY == _spriteRAM[63 * 4] && tileIndex == 0xFF && attributes == 0xFF && spriteX == 0xFF) {
		//Skip this sprite
		spriteY = 255;
	}
	
	if(spriteY < 240) {
		uint16_t tileAddr;
		if(_flags.LargeSprites) {
			throw exception("Not implemented yet");
		} else {
			tileAddr = ((tileIndex << 4) | _flags.SpritePatternAddr) + (_scanline - spriteY);
		}

		_spriteX[spriteIndex] = spriteX;
		_spriteTiles[spriteIndex].HorizontalMirror = horizontalMirror;
		_spriteTiles[spriteIndex].PaletteOffset = (attributes & 0x03) << 2;
		_spriteTiles[spriteIndex].LowByte = _memoryManager->ReadVRAM(tileAddr);
		_spriteTiles[spriteIndex].HighByte = _memoryManager->ReadVRAM(tileAddr + 8);
	} else {
		_spriteX[spriteIndex] = 256;
		_spriteTiles[spriteIndex].PaletteOffset = 0;
		_spriteTiles[spriteIndex].LowByte = 0;
		_spriteTiles[spriteIndex].HighByte = 0;
	}
}

void PPU::LoadNextTile()
{
	_state.LowBitShift |= _nextTile.LowByte;
	_state.HighBitShift |= _nextTile.HighByte;
}

void PPU::InitializeShiftRegisters()
{
	_state.LowBitShift = (_currentTile.LowByte << 8) | _nextTile.LowByte;
	_state.HighBitShift = (_currentTile.HighByte << 8) | _nextTile.HighByte;
}

void PPU::ShiftTileRegisters()
{
	_state.LowBitShift <<= 1;
	_state.HighBitShift <<= 1;
}

void PPU::DrawPixel()
{
	bool useBackground = true;
	uint32_t pixelColor = 0;

	uint32_t offset = _state.XScroll;
	uint32_t backgroundColor = (((_state.LowBitShift << offset) & 0x8000) >> 15) | (((_state.HighBitShift << offset) & 0x8000) >> 14);
	
	for(int i = 0; i < 8; i++) {
		if(useBackground && _spriteX[i] <= 0 && _spriteX[i] > -8) {
			if(_cycle == 256) {
				pixelColor = 10;
			}
			uint32_t spriteColor;
			if(_spriteTiles[i].HorizontalMirror) {
				spriteColor = ((_spriteTiles[i].LowByte >> -_spriteX[i]) & 0x01) | ((_spriteTiles[i].HighByte >> -_spriteX[i]) & 0x01) << 1;
			} else {
				spriteColor = ((_spriteTiles[i].LowByte << -_spriteX[i]) & 0x80) >> 7 | ((_spriteTiles[i].HighByte << -_spriteX[i]) & 0x80) >> 6;
			}

			if(spriteColor != 0) {
				uint8_t spritePaletteColor = GetSpritePaletteEntry(_spriteTiles[i].PaletteOffset, spriteColor);
				pixelColor = 0xFF000000 | PPU_PALETTE_RGB[spritePaletteColor % 64];
				useBackground = false;
			}
		}
		_spriteX[i]--;
	}
	
	if(useBackground) {
		// If we're grabbing the pixel from the high part of the shift register, use the previous tile's palette, not the current one
		uint8_t backgroundPaletteColor = GetBGPaletteEntry(offset < 8 ? _previousTile.PaletteOffset : _currentTile.PaletteOffset, backgroundColor);
		pixelColor = 0xFF000000 | PPU_PALETTE_RGB[backgroundPaletteColor % 64];
	}

	uint32_t bufferPosition = _scanline * 256 + (_cycle - 1);
	((uint32_t*)_outputBuffer)[bufferPosition] = pixelColor;

	//Shift the tile registers to prepare for the next cycle
	ShiftTileRegisters();
}

void PPU::ProcessPreVBlankScanline()
{
	//For pre-render scanline & all visible scanlines
	if(IsRenderingEnabled()) {
		//Update video ram address according to scrolling logic
		if(_cycle == 256) {
			IncVerticalScrolling();
		} else if(_cycle == 257) {
			//copy horizontal scrolling value from t
			_state.VideoRamAddr = (_state.VideoRamAddr & ~0x041F) | (_state.TmpVideoRamAddr & 0x041F);
		} else if((_cycle % 8 == 0 && _cycle > 0 && _cycle < 256) || _cycle == 328 || _cycle == 336) {
			IncHorizontalScrolling();
		}
	}

	if(_cycle >= 257 && _cycle <= 320) {
		//"OAMADDR is set to 0 during each of ticks 257-320 (the sprite tile loading interval) of the pre-render and visible scanlines."
		_state.SpriteRamAddr = 0;
	}
}

void PPU::ProcessPrerenderScanline()
{
	ProcessPreVBlankScanline();

	if(_cycle == 1) {
		_statusFlags.SpriteOverflow = false;
		_statusFlags.Sprite0Hit = false;
		_statusFlags.VerticalBlank = false;
	} else if(_cycle >= 280 && _cycle <= 304) {
		if(IsRenderingEnabled()) {
			//copy vertical scrolling value from t
			_state.VideoRamAddr = (_state.VideoRamAddr & ~0x7BF0) | (_state.TmpVideoRamAddr & 0x7BF0);
		}
	} else if(_cycle == 340 && _flags.BackgroundEnabled && (_frameCount % 2 == 1)) {
		//Skip a cycle for odd frames, if background drawing is enabled
		_cycle = 0;
		_scanline = 0;
	} else if(_cycle == 321 || _cycle == 329) {
		LoadTileInfo();
		if(_cycle == 329) {
			InitializeShiftRegisters();
		}
	}
}

void PPU::ProcessVisibleScanline()
{
	if(_cycle > 0 && _cycle <= 256) {
		if((_cycle - 1) % 8 == 0) {
			//Cycle 1, 9, 17, etc.
			if(_cycle != 1) {
				LoadNextTile();
			} else {
				//Clear secondary OAM at the start of every visible scanline (cycle 1)
				memset(_secondarySpriteRAM, 0xFF, 0x40);
			}
			LoadTileInfo();
		} 

		DrawPixel();
		CopyOAMData();

		if(_cycle == 256 && _scanline == 239) {
			//Send frame to GUI once the last pixel has been output
			PPU::VideoDevice->UpdateFrame(_outputBuffer);
		}
	} else if((_cycle - 261) % 8 == 0 && _cycle <= 320) {
		LoadSpriteTileInfo((_cycle - 261) / 8);
	} else if(_cycle == 321 || _cycle == 329) {
		LoadTileInfo();
		if(_cycle == 329) {
			InitializeShiftRegisters();
		}
	}

	ProcessPreVBlankScanline();
}

void PPU::CopyOAMData()
{
	static uint32_t _secondaryOAMAddr = 0;
	static uint8_t _buffer = 0;
	static bool _writeData = false;
	static bool _done = false;
	
	if(_cycle >= 65 && _cycle <= 256) {
		if(_cycle == 65) {
			_secondaryOAMAddr = 0;
			_done = false;
		}

		if(_state.SpriteRamAddr >= 0x100) {
			_done = true;
		}

		if(!_done) {
			if(_cycle & 0x01) {
				//Read a byte from the primary OAM
				_buffer = _spriteRAM[_state.SpriteRamAddr & 0xFF];
				_state.SpriteRamAddr++;
			} else {
				if(!_writeData && _state.SpriteRamAddr < 0x100 && _scanline >= _buffer && _scanline < _buffer + (_flags.LargeSprites ? 16 : 8)) {
					_writeData = true;
				}

				if(_secondaryOAMAddr < 0x40) {
					//Copy 1 byte to secondary OAM
					_secondarySpriteRAM[_secondaryOAMAddr] = _buffer;

					if(_writeData) {
						_secondaryOAMAddr++;

						if((_secondaryOAMAddr & 0x03) == 0) {
							//Done copying
							_writeData = false;
						}
					} else {
						_state.SpriteRamAddr += 3;
					}
				} else {
					//8 sprites have been found, check flags, etc?
				}
			}
		}
	}
}

void PPU::BeginVBlank()
{
	if(_cycle == 1) {
		if(!_doNotSetVBFlag) {
			_statusFlags.VerticalBlank = true;
		}
		if(_flags.VBlank) {
			CPU::SetNMIFlag();
		}
		_doNotSetVBFlag = false;
	}
}

void PPU::EndVBlank()
{
	if(_cycle == 340) {
		_frameCount++;
	}
}

void PPU::Exec()
{
	uint64_t equivalentCycleCount = CPU::GetCycleCount() * 3;
	uint32_t gap = (uint32_t)(equivalentCycleCount - _cycleCount);
	_cycleCount += gap;
	while(gap > 0) {
		if(_scanline == -1) {
			ProcessPrerenderScanline();
		} else if(_scanline < 240) {
			ProcessVisibleScanline();
		} else if(_scanline == 241) {
			BeginVBlank();
		} else if(_scanline == 260) {
			EndVBlank();
		}

		if(_cycle == 340) {
			_cycle = 0;
			_scanline++;

			if(_scanline == 261) {
				_scanline = -1;
			}
		} else {
			_cycle++;
		}

		gap--;
	}
}