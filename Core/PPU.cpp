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

void PPU::UpdateVideoRamAddr()
{
	if(_scanline >= 239 || !IsRenderingEnabled()) {
		_state.VideoRamAddr += _flags.VerticalWrite ? 32 : 1;
	} else {
		//"During rendering (on the pre-render line and the visible lines 0-239, provided either background or sprite rendering is enabled), "
		//it will update v in an odd way, triggering a coarse X increment and a Y increment simultaneously"
		IncHorizontalScrolling();
		IncVerticalScrolling();
	}
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

			UpdateVideoRamAddr();
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
			SetControlRegister(value);
			break;
		case PPURegisters::Mask:
			SetMaskRegister(value);
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
			UpdateVideoRamAddr();
			break;
		case PPURegisters::SpriteDMA:
			//DMA transfer starts at SpriteRamAddr and wraps around
			for(int i = 0; i < 0x100; i++) {
				_spriteRAM[(_state.SpriteRamAddr+i)&0xFF] = _memoryManager->Read(value*0x100 + i);
			}

			//"the DMA procedure takes 513 CPU cycles (+1 on odd CPU cycles)"
			CPU::IncCycleCount((CPU::GetCycleCount() % 2 == 0) ? 513 : 514);
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
		return ReadPaletteRAM(0x3F00) % 64;
	} else {
		return ReadPaletteRAM(0x3F00 + paletteOffset + pixel) % 64;
	}
}

uint8_t PPU::GetSpritePaletteEntry(uint8_t paletteOffset, uint8_t pixel)
{
	if(pixel == 0) {
		return ReadPaletteRAM(0x3F00) % 64;
	} else {
		return ReadPaletteRAM(0x3F10 + paletteOffset + pixel) % 64;
	}
}

bool PPU::IsRenderingEnabled()
{
	return _flags.BackgroundEnabled || _flags.SpritesEnabled;
}

void PPU::SetControlRegister(uint8_t value)
{
	_state.Control = value;

	uint8_t nameTable = (_state.Control & 0x03);
	_state.TmpVideoRamAddr = (_state.TmpVideoRamAddr & ~0x0C00) | (nameTable << 10);

	_flags.VerticalWrite = (_state.Control & 0x04) == 0x04;
	_flags.SpritePatternAddr = ((_state.Control & 0x08) == 0x08) ? 0x1000 : 0x0000;
	_flags.BackgroundPatternAddr = ((_state.Control & 0x10) == 0x10) ? 0x1000 : 0x0000;
	_flags.LargeSprites = (_state.Control & 0x20) == 0x20;

	bool originalVBlank = _flags.VBlank;
	_flags.VBlank = (_state.Control & 0x80) == 0x80;
	if(!originalVBlank && _flags.VBlank && _statusFlags.VerticalBlank) {
		CPU::SetNMIFlag();
	}
}

void PPU::SetMaskRegister(uint8_t value)
{
	_state.Mask = value;
	_flags.Grayscale = (_state.Mask & 0x01) == 0x01;
	_flags.BackgroundMask = (_state.Mask & 0x02) == 0x02;
	_flags.SpriteMask = (_state.Mask & 0x04) == 0x04;
	_flags.BackgroundEnabled = (_state.Mask & 0x08) == 0x08;
	_flags.SpritesEnabled = (_state.Mask & 0x10) == 0x10;
	_flags.IntensifyRed = (_state.Mask & 0x20) == 0x20;
	_flags.IntensifyGreen = (_state.Mask & 0x40) == 0x40;
	_flags.IntensifyBlue = (_state.Mask & 0x80) == 0x80;
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
			y++;                  // increment coarse Y
		}
		addr = (addr & ~0x03E0) | (y << 5);     // put coarse Y back into v
	}
	_state.VideoRamAddr = addr;
}

//Taken from http://wiki.nesdev.com/w/index.php/The_skinny_on_NES_scrolling#Wrapping_around
void PPU::IncHorizontalScrolling()
{
	//Increase coarse X scrolling value.
	uint16_t addr = _state.VideoRamAddr;
	if((addr & 0x001F) == 31) {
		//When the value is 31, wrap around to 0 and switch nametable
		addr = (addr & ~0x001F) ^ 0x0400; 
	} else {
		addr++;
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
	bool backgroundPriority = (attributes & 0x20) == 0x20;
	bool horizontalMirror = (attributes & 0x40) == 0x40;
	bool verticalMirror = (attributes & 0x80) == 0x80;
		
	if(spriteY < 240) {
		uint16_t tileAddr;
		uint8_t lineOffset;
		if(verticalMirror) {
			lineOffset = (_flags.LargeSprites ? 15 : 7) - (_scanline - spriteY);
		} else {
			lineOffset = _scanline - spriteY;
		}

		if(_flags.LargeSprites) {
			tileAddr = (((tileIndex & 0x01) ? 0x1000 : 0x0000) | ((tileIndex & ~0x01) << 4)) + (lineOffset >= 8 ? lineOffset + 8 : lineOffset);
		} else {
			tileAddr = ((tileIndex << 4) | _flags.SpritePatternAddr) + lineOffset;
		}

		_spriteX[spriteIndex] = spriteX;
		_spriteTiles[spriteIndex].BackgroundPriority = backgroundPriority;
		_spriteTiles[spriteIndex].HorizontalMirror = horizontalMirror;
		_spriteTiles[spriteIndex].PaletteOffset = (attributes & 0x03) << 2;
		_spriteTiles[spriteIndex].LowByte = _memoryManager->ReadVRAM(tileAddr);
		_spriteTiles[spriteIndex].HighByte = _memoryManager->ReadVRAM(tileAddr + 8);
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
	//This is called 3.7 million times per second - needs to be as fast as possible.
	uint8_t offset = _state.XScroll;

	bool useBackground = true;
	uint32_t backgroundColor = 0;
	uint32_t spriteColor = 0;
	
	if(_flags.BackgroundEnabled) {
		backgroundColor = (((_state.LowBitShift << offset) & 0x8000) >> 15) | (((_state.HighBitShift << offset) & 0x8000) >> 14);
	}

	uint8_t i;
	if(_flags.SpritesEnabled) {
		for(i = 0; i < _spriteCount; i++) {
			int32_t shift = -((int32_t)_spriteX[i] - (int32_t)_cycle + 1);
			if(shift >= 0 && shift < 8) {
				if(_spriteTiles[i].HorizontalMirror) {
					spriteColor = ((_spriteTiles[i].LowByte >> shift) & 0x01) | ((_spriteTiles[i].HighByte >> shift) & 0x01) << 1;
				} else {
					spriteColor = ((_spriteTiles[i].LowByte << shift) & 0x80) >> 7 | ((_spriteTiles[i].HighByte << shift) & 0x80) >> 6;
				}

				if(spriteColor != 0) {
					//First sprite without a 00 color, use it.
					break;
				}
			}
		}
	}

	if(_cycle <= 8) {
		if(!_flags.BackgroundMask) {
			//"0: Hide background in leftmost 8 pixels of screen;"
			backgroundColor = 0;
		}
		if(!_flags.SpriteMask) {
			//"0: Hide sprites in leftmost 8 pixels of screen;"
			spriteColor = 0;
		}
	}

	if(spriteColor != 0 && (backgroundColor == 0 || !_spriteTiles[i].BackgroundPriority)) {
		//Check sprite priority
		useBackground = false;
	}
	
	if(i == 0 && spriteColor != 0 && backgroundColor != 0 && _sprite0Visible && _cycle != 256 && _flags.BackgroundEnabled && _flags.SpritesEnabled) {
		//"The hit condition is basically sprite zero is in range AND the first sprite output unit is outputting a non-zero pixel AND the background drawing unit is outputting a non-zero pixel."
		//"Sprite zero hits do not register at x=255" (cycle 256)
		//"... provided that background and sprite rendering are both enabled"
		//"Should always miss when Y >= 239"
		_statusFlags.Sprite0Hit = true;
	}

	uint32_t pixelColor = 0;
	if(useBackground) {
		// If we're grabbing the pixel from the high part of the shift register, use the previous tile's palette, not the current one
		pixelColor = PPU_PALETTE_RGB[GetBGPaletteEntry(offset + ((_cycle - 1) % 8) < 8 ? _previousTile.PaletteOffset : _currentTile.PaletteOffset, backgroundColor)];
	} else {
		pixelColor = PPU_PALETTE_RGB[GetSpritePaletteEntry(_spriteTiles[i].PaletteOffset, spriteColor)];
	}
	
	uint32_t bufferPosition = _scanline * 256 + (_cycle - 1);
	((uint32_t*)_outputBuffer)[bufferPosition] = 0xFF000000 | pixelColor;

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
			_state.VideoRamAddr = (_state.VideoRamAddr & ~0x7BE0) | (_state.TmpVideoRamAddr & 0x7BE0);
		}
	} else if(_cycle == 339 && _flags.BackgroundEnabled && (_frameCount % 2 == 1)) {
		//Skip a cycle for odd frames, if background drawing is enabled
		_cycle = -1;
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
			}
			LoadTileInfo();
		} 

		DrawPixel();
		
		if(IsRenderingEnabled()) {
			CopyOAMData();
		}

		if(_cycle == 256 && _scanline == 239) {
			//Send frame to GUI once the last pixel has been output
			PPU::VideoDevice->UpdateFrame(_outputBuffer);
		}
	} else if((_cycle - 261) % 8 == 0 && _cycle <= 320) {
		uint32_t spriteIndex = (_cycle - 261) / 8;
		if(spriteIndex < _spriteCount) {
			LoadSpriteTileInfo(spriteIndex);
		}
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
	static uint8_t _buffer = 0;
	static bool _writeData = false;
	static uint32_t _overflowCounter = 0;
	static bool _sprite0Added = true;

	if(_cycle < 65) {
		//Clear secondary OAM at between cycle 0 and 64
		_secondarySpriteRAM[_cycle >> 1] = 0xFF;
	} else {
		if(_cycle == 65) {
			_overflowCounter = 0;
			_sprite0Added = false;
			_writeData = false;
			_secondaryOAMAddr = 0;
		} else if(_cycle == 256) {
			_sprite0Visible = _sprite0Added;
			_spriteCount = (_secondaryOAMAddr >> 2);
		}

		if(_cycle & 0x01) {
			//Read a byte from the primary OAM on odd cycles
			_buffer = _spriteRAM[_state.SpriteRamAddr & 0xFF];
			_state.SpriteRamAddr++;
		} else {
			if(!_writeData && _state.SpriteRamAddr < 0x100 && _scanline >= _buffer && _scanline < _buffer + (_flags.LargeSprites ? 16 : 8)) {
				_writeData = true;
			}

			if(_secondaryOAMAddr < 0x20) {
				//Copy 1 byte to secondary OAM
				_secondarySpriteRAM[_secondaryOAMAddr] = _buffer;

				if(_writeData) {
					_secondaryOAMAddr++;

					if(_state.SpriteRamAddr == 0x01) {
						_sprite0Added = true;
					}

					if((_secondaryOAMAddr & 0x03) == 0) {
						//Done copying
						_writeData = false;
					}
				} else {
					_state.SpriteRamAddr += 3;
				}
			} else {
				//8 sprites have been found, check next sprite for overflow + emulate PPU bug
				//Based on: http://forums.nesdev.com/viewtopic.php?p=85431#p85431
				//Behavior matches: http://forums.nesdev.com/viewtopic.php?p=1387#p1387
				if(!_statusFlags.SpriteOverflow) {
					if(_writeData) {
						//Sprite is visible, consider this to be an overflow
						_statusFlags.SpriteOverflow = true;
						_overflowCounter = 3;
					} else if((_state.SpriteRamAddr & 0x3) != 0) {
						//Sprite isn't on this scanline, trigger sprite evaluation bug
						_state.SpriteRamAddr += 4;
					}
				} else {
					if(_overflowCounter != 0) {
						_overflowCounter--;
						if(_overflowCounter == 0) {
							_state.SpriteRamAddr = (_state.SpriteRamAddr + 3) & 0x0FFC;
						}
					} else {
						_state.SpriteRamAddr = (_state.SpriteRamAddr + 4) & 0x0FFC;
					}
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
			if(_flags.VBlank) {
				CPU::SetNMIFlag();
			}
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