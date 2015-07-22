#include "stdafx.h"
#include "PPU.h"
#include "CPU.h"
#include "EmulationSettings.h"

PPU* PPU::Instance = nullptr;
IVideoDevice *PPU::VideoDevice = nullptr;

uint32_t PPU_PALETTE_RGB[] = {
    0xFF666666, 0xFF002A88, 0xFF1412A7, 0xFF3B00A4, 0xFF5C007E,
    0xFF6E0040, 0xFF6C0600, 0xFF561D00, 0xFF333500, 0xFF0B4800,
    0xFF005200, 0xFF004F08, 0xFF00404D, 0xFF000000, 0xFF000000,
    0xFF000000, 0xFFADADAD, 0xFF155FD9, 0xFF4240FF, 0xFF7527FE,
    0xFFA01ACC, 0xFFB71E7B, 0xFFB53120, 0xFF994E00, 0xFF6B6D00,
    0xFF388700, 0xFF0C9300, 0xFF008F32, 0xFF007C8D, 0xFF000000,
    0xFF000000, 0xFF000000, 0xFFFFFEFF, 0xFF64B0FF, 0xFF9290FF,
    0xFFC676FF, 0xFFF36AFF, 0xFFFE6ECC, 0xFFFE8170, 0xFFEA9E22,
    0xFFBCBE00, 0xFF88D800, 0xFF5CE430, 0xFF45E082, 0xFF48CDDE,
    0xFF4F4F4F, 0xFF000000, 0xFF000000, 0xFFFFFEFF, 0xFFC0DFFF,
    0xFFD3D2FF, 0xFFE8C8FF, 0xFFFBC2FF, 0xFFFEC4EA, 0xFFFECCC5,
    0xFFF7D8A5, 0xFFE4E594, 0xFFCFEF96, 0xFFBDF4AB, 0xFFB3F3CC,
    0xFFB5EBF2, 0xFFB8B8B8, 0xFF000000, 0xFF000000,
};

PPU::PPU(MemoryManager *memoryManager)
{
	PPU::Instance = this;

	_memoryManager = memoryManager;
	_outputBuffer = new uint32_t[256 * 240];

	Reset();
}

PPU::~PPU() 
{
	delete[] _outputBuffer;
}

void PPU::Reset()
{
	_state = {};
	_flags = {};
	_statusFlags = {};

	_scanline = 0;
	_cycle = 0;
	_frameCount = 0;
	_memoryReadBuffer = 0;

	memset(_spriteRAM, 0xFF, 0x100);
}

void PPU::SetNesModel(NesModel model)
{
	_nesModel = model;
	_vblankEnd = (model == NesModel::NTSC ? 260 : 311);
}

PPUDebugState PPU::GetState()
{
	PPUDebugState state;
	state.ControlFlags = _flags;
	state.StatusFlags = _statusFlags;
	state.State = _state;
	state.Cycle = _cycle;
	state.Scanline = _scanline;
	return state;
}

void PPU::UpdateVideoRamAddr()
{
	if(_scanline >= 239 || !IsRenderingEnabled()) {
		_state.VideoRamAddr += _flags.VerticalWrite ? 32 : 1;
		
		//Trigger memory read when setting the vram address - needed by MMC3 IRQ counter
		//"Should be clocked when A12 changes to 1 via $2007 read/write"
		_memoryManager->ReadVRAM(_state.VideoRamAddr);
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
			_state.SpriteRamAddr = (_state.SpriteRamAddr + 1) & 0xFF;
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
				
				//Trigger memory read when setting the vram address - needed by MMC3 IRQ counter
				//"4) Should be clocked when A12 changes to 1 via $2006 write"
				_memoryManager->ReadVRAM(_state.VideoRamAddr);
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
			CPU::RunDMATransfer(_spriteRAM, _state.SpriteRamAddr, value);
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

uint32_t PPU::GetBGPaletteEntry(uint32_t paletteOffset, uint32_t pixel)
{
	if(pixel == 0) {
		return ReadPaletteRAM(0x3F00) & 0x3F;
	} else {
		return ReadPaletteRAM(0x3F00 + paletteOffset + pixel) & 0x3F;
	}
}

uint32_t PPU::GetSpritePaletteEntry(uint32_t paletteOffset, uint32_t pixel)
{
	if(pixel == 0) {
		return ReadPaletteRAM(0x3F00) & 0x3F;
	} else {
		return ReadPaletteRAM(0x3F10 + paletteOffset + pixel) & 0x3F;
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

	//"By toggling NMI_output ($2000 bit 7) during vertical blank without reading $2002, a program can cause /NMI to be pulled low multiple times, causing multiple NMIs to be generated."
	bool originalVBlank = _flags.VBlank;
	_flags.VBlank = (_state.Control & 0x80) == 0x80;
	
	if(!originalVBlank && _flags.VBlank && _statusFlags.VerticalBlank) {
		CPU::SetNMIFlag();
	} else if(_scanline == 241 && _cycle < 3 && !_flags.VBlank) {
		CPU::ClearNMIFlag();
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

	if(_scanline == 241) {
		if(_cycle < 3) {
			CPU::ClearNMIFlag();

			if(_cycle == 0) {
				_doNotSetVBFlag = true;
			}
		}
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

//Taken from http://wiki.nesdev.com/w/index.php/The_skinny_on_NES_scrolling#Tile_and_attribute_fetching
uint16_t PPU::GetNameTableAddr()
{
	return 0x2000 | (_state.VideoRamAddr & 0x0FFF);
}

//Taken from http://wiki.nesdev.com/w/index.php/The_skinny_on_NES_scrolling#Tile_and_attribute_fetching
uint16_t PPU::GetAttributeAddr()
{
	return 0x23C0 | (_state.VideoRamAddr & 0x0C00) | ((_state.VideoRamAddr >> 4) & 0x38) | ((_state.VideoRamAddr >> 2) & 0x07);
}

void PPU::LoadTileInfo()
{
	if(IsRenderingEnabled()) {
		_previousTile = _currentTile;
		_currentTile = _nextTile;

		uint16_t tileIndex = _memoryManager->ReadVRAM(GetNameTableAddr());
		uint16_t tileAddr = (tileIndex << 4) | (_state.VideoRamAddr >> 12) | _flags.BackgroundPatternAddr;

		uint16_t shift = ((_state.VideoRamAddr >> 4) & 0x04) | (_state.VideoRamAddr & 0x02);
		_nextTile.PaletteOffset = ((_memoryManager->ReadVRAM(GetAttributeAddr()) >> shift) & 0x03) << 2;
		_nextTile.LowByte = _memoryManager->ReadVRAM(tileAddr);
		_nextTile.HighByte = _memoryManager->ReadVRAM(tileAddr + 8);
	}
}

void PPU::LoadSpriteTileInfo(uint8_t spriteIndex)
{
	if(IsRenderingEnabled()) {
		uint32_t spriteAddr = spriteIndex * 4;
		uint8_t spriteY = _secondarySpriteRAM[spriteAddr];
		uint8_t tileIndex = _secondarySpriteRAM[spriteAddr + 1];
		uint8_t attributes = _secondarySpriteRAM[spriteAddr + 2];
		uint8_t spriteX = _secondarySpriteRAM[spriteAddr + 3];
		bool backgroundPriority = (attributes & 0x20) == 0x20;
		bool horizontalMirror = (attributes & 0x40) == 0x40;
		bool verticalMirror = (attributes & 0x80) == 0x80;

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

		if(spriteIndex < _spriteCount && spriteY < 240) {
			_spriteX[spriteIndex] = spriteX;
			_spriteTiles[spriteIndex].BackgroundPriority = backgroundPriority;
			_spriteTiles[spriteIndex].HorizontalMirror = horizontalMirror;
			_spriteTiles[spriteIndex].PaletteOffset = (attributes & 0x03) << 2;
			_spriteTiles[spriteIndex].LowByte = _memoryManager->ReadVRAM(tileAddr);
			_spriteTiles[spriteIndex].HighByte = _memoryManager->ReadVRAM(tileAddr + 8);
		} else {
			//Fetches to sprite 0xFF for remaining sprites/hidden - used by MMC3 IRQ counter
			lineOffset = 0;
			tileIndex = 0xFF;
			if(_flags.LargeSprites) {
				tileAddr = (((tileIndex & 0x01) ? 0x1000 : 0x0000) | ((tileIndex & ~0x01) << 4)) + (lineOffset >= 8 ? lineOffset + 8 : lineOffset);
			} else {
				tileAddr = ((tileIndex << 4) | _flags.SpritePatternAddr) + lineOffset;
			}

			_memoryManager->ReadVRAM(tileAddr);
			_memoryManager->ReadVRAM(tileAddr + 8);
		}
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

	uint32_t backgroundColor = 0;
	uint32_t &pixel = _outputBuffer[(_scanline << 8) + _cycle - 1];

	if((_cycle > 8 || _flags.BackgroundMask) && _flags.BackgroundEnabled) {
		//BackgroundMask = false: Hide background in leftmost 8 pixels of screen
		backgroundColor = (((_state.LowBitShift << offset) & 0x8000) >> 15) | (((_state.HighBitShift << offset) & 0x8000) >> 14);
	}

	if((_cycle > 8 || _flags.SpriteMask) && _flags.SpritesEnabled) {
		//SpriteMask = true: Hide sprites in leftmost 8 pixels of screen
		for(uint8_t i = 0; i < _spriteCount; i++) {
			int32_t shift = -((int32_t)_spriteX[i] - (int32_t)_cycle + 1);
			if(shift >= 0 && shift < 8) {
				uint32_t spriteColor;
				if(_spriteTiles[i].HorizontalMirror) {
					spriteColor = ((_spriteTiles[i].LowByte >> shift) & 0x01) | ((_spriteTiles[i].HighByte >> shift) & 0x01) << 1;
				} else {
					spriteColor = ((_spriteTiles[i].LowByte << shift) & 0x80) >> 7 | ((_spriteTiles[i].HighByte << shift) & 0x80) >> 6;
				}

				if(spriteColor != 0) {
					//First sprite without a 00 color, use it.
					if(i == 0 && backgroundColor != 0 && _sprite0Visible && _cycle != 256 && _flags.BackgroundEnabled) {
						//"The hit condition is basically sprite zero is in range AND the first sprite output unit is outputting a non-zero pixel AND the background drawing unit is outputting a non-zero pixel."
						//"Sprite zero hits do not register at x=255" (cycle 256)
						//"... provided that background and sprite rendering are both enabled"
						//"Should always miss when Y >= 239"
						_statusFlags.Sprite0Hit = true;
					}

					if(backgroundColor == 0 || !_spriteTiles[i].BackgroundPriority) {
						//Check sprite priority
						pixel = PPU_PALETTE_RGB[GetSpritePaletteEntry(_spriteTiles[i].PaletteOffset, spriteColor)];
						return;
					}

					break;
				}
			}
		}
	}
	pixel = PPU_PALETTE_RGB[GetBGPaletteEntry(offset + ((_cycle - 1) & 0x07) < 8 ? _previousTile.PaletteOffset : _currentTile.PaletteOffset, backgroundColor)];
}

void PPU::ProcessPreVBlankScanline()
{
	//For pre-render scanline & all visible scanlines
	if(IsRenderingEnabled()) {
		//Update video ram address according to scrolling logic
		if((_cycle > 0 && _cycle < 256 && (_cycle & 0x07) == 0) || _cycle == 328 || _cycle == 336) {
			IncHorizontalScrolling();
		} else if(_cycle == 256) {
			IncVerticalScrolling();
		} else if(_cycle == 257) {
			//copy horizontal scrolling value from t
			_state.VideoRamAddr = (_state.VideoRamAddr & ~0x041F) | (_state.TmpVideoRamAddr & 0x041F);
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

	if(_cycle == 0) {
		_statusFlags.SpriteOverflow = false;
		_statusFlags.Sprite0Hit = false;
		_statusFlags.VerticalBlank = false;
	}
	
	if(((_cycle - 1) & 0x07) == 0 && _cycle < 250) {
		LoadTileInfo();
	} else if(_cycle >= 280 && _cycle <= 304) {
		if(IsRenderingEnabled()) {
			//copy vertical scrolling value from t
			_state.VideoRamAddr = (_state.VideoRamAddr & ~0x7BE0) | (_state.TmpVideoRamAddr & 0x7BE0);
		}
	} else if(_nesModel == NesModel::NTSC && _cycle == 339 && IsRenderingEnabled() && (_frameCount & 0x01)) {
		//This behavior is NTSC-specific - PAL frames are always the same number of cycles
		//"With rendering enabled, each odd PPU frame is one PPU clock shorter than normal" (skip from 339 to 0, going over 340)
		_cycle = -1;
		_scanline = 0;
	} else if(_cycle == 321 || _cycle == 329) {
		LoadTileInfo();
		if(_cycle == 329) {
			InitializeShiftRegisters();
		}
	}

	if(_cycle >= 261 && ((_cycle - 261) & 0x07) == 0 && _cycle <= 320) {
		//Unused sprite tile fetches, but vital for MMC3 IRQ counter
		uint32_t spriteIndex = (_cycle - 261) / 8;
		LoadSpriteTileInfo(spriteIndex);
	} 
}

void PPU::ProcessVisibleScanline()
{
	if(_cycle > 0 && _cycle <= 256) {
		if(((_cycle - 1) & 0x07) == 0) {
			//Cycle 1, 9, 17, etc.
			if(_cycle != 1) {
				LoadNextTile();
			}
			LoadTileInfo();
		} 

		DrawPixel();
		ShiftTileRegisters();
		
		if(IsRenderingEnabled()) {
			CopyOAMData();
		}
	} else if(((_cycle - 261) & 0x07) == 0 && _cycle <= 320) {
		//Cycle 261, 269, etc.
		uint32_t spriteIndex = (_cycle - 261) / 8;
		LoadSpriteTileInfo(spriteIndex);
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
	if(_cycle < 65) {
		//Clear secondary OAM at between cycle 0 and 64
		_secondarySpriteRAM[_cycle >> 1] = 0xFF;
	} else {
		if(_cycle == 65) {
			_overflowCounter = 0;
			_sprite0Added = false;
			_writeOAMData = false;
			_secondaryOAMAddr = 0;
		} else if(_cycle == 256) {
			_sprite0Visible = _sprite0Added;
			_spriteCount = (_secondaryOAMAddr >> 2);
		}

		if(_cycle & 0x01) {
			//Read a byte from the primary OAM on odd cycles
			_oamCopybuffer = _spriteRAM[_state.SpriteRamAddr & 0xFF];
			_state.SpriteRamAddr++;
		} else {
			if(!_writeOAMData && _scanline >= _oamCopybuffer && _scanline < _oamCopybuffer + (_flags.LargeSprites ? 16 : 8) && _state.SpriteRamAddr < 0x100) {
				_writeOAMData = true;
			}

			if(_secondaryOAMAddr < 0x20) {
				//Copy 1 byte to secondary OAM
				_secondarySpriteRAM[_secondaryOAMAddr] = _oamCopybuffer;

				if(_writeOAMData) {
					_secondaryOAMAddr++;

					if(_state.SpriteRamAddr == 0x01) {
						_sprite0Added = true;
					}

					if((_secondaryOAMAddr & 0x03) == 0) {
						//Done copying
						_writeOAMData = false;
					}
				} else {
					_state.SpriteRamAddr += 3;
				}
			} else {
				//8 sprites have been found, check next sprite for overflow + emulate PPU bug
				//Based on: http://forums.nesdev.com/viewtopic.php?p=85431#p85431
				//Behavior matches: http://forums.nesdev.com/viewtopic.php?p=1387#p1387
				if(!_statusFlags.SpriteOverflow) {
					if(_writeOAMData) {
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
	if(_cycle == 0) {
		//Send frame to GUI once the last pixel has been output
		if(PPU::VideoDevice) {
			PPU::VideoDevice->UpdateFrame(_outputBuffer);
		}

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
	if(_scanline != -1 && _scanline < 240) {
		ProcessVisibleScanline();
	} else if(_scanline == -1) {
		ProcessPrerenderScanline();
	} else if(_scanline == 241) {
		BeginVBlank();
	} else if(_scanline == _vblankEnd) {
		EndVBlank();
	}

	if(_cycle == 340) {
		_cycle = -1;

		if(_scanline++ == _vblankEnd) {
			_scanline = -1;
		}
	}
	_cycle++;
}

void PPU::ExecStatic()
{
	PPU::Instance->Exec();
	PPU::Instance->Exec();
	PPU::Instance->Exec();
	if(PPU::Instance->_nesModel == NesModel::PAL && CPU::GetCycleCount() % 5 == 0) {
		//PAL PPU runs 3.2 clocks for every CPU clock, so we need to run an extra clock every 5 CPU clocks
		PPU::Instance->Exec();
	}
}

void PPU::StreamState(bool saving)
{
	Stream<uint8_t>(_state.Control);
	Stream<uint8_t>(_state.Mask);
	Stream<uint8_t>(_state.Status);
	Stream<uint32_t>(_state.SpriteRamAddr);
	Stream<uint16_t>(_state.VideoRamAddr);
	Stream<uint8_t>(_state.XScroll);
	Stream<uint16_t>(_state.TmpVideoRamAddr);
	Stream<bool>(_state.WriteToggle);
	Stream<uint16_t>(_state.HighBitShift);
	Stream<uint16_t>(_state.LowBitShift);

	Stream<bool>(_flags.VerticalWrite);
	Stream<uint16_t>(_flags.SpritePatternAddr);
	Stream<uint16_t>(_flags.BackgroundPatternAddr);
	Stream<bool>(_flags.LargeSprites);
	Stream<bool>(_flags.VBlank);
	
	Stream<bool>(_flags.Grayscale);
	Stream<bool>(_flags.BackgroundMask);
	Stream<bool>(_flags.SpriteMask);
	Stream<bool>(_flags.BackgroundEnabled);
	Stream<bool>(_flags.SpritesEnabled);
	Stream<bool>(_flags.IntensifyRed);
	Stream<bool>(_flags.IntensifyGreen);
	Stream<bool>(_flags.IntensifyBlue);

	Stream<bool>(_statusFlags.SpriteOverflow);
	Stream<bool>(_statusFlags.Sprite0Hit);
	Stream<bool>(_statusFlags.VerticalBlank);

	Stream<int32_t>(_scanline);
	Stream<uint32_t>(_cycle);
	Stream<uint32_t>(_frameCount);
	Stream<uint8_t>(_memoryReadBuffer);
	
	StreamArray<uint8_t>(_paletteRAM, 0x100);
	StreamArray<uint8_t>(_spriteRAM, 0x100);
	StreamArray<uint8_t>(_secondarySpriteRAM, 0x20);

	Stream<uint8_t>(_currentTile.LowByte);
	Stream<uint8_t>(_currentTile.HighByte);
	Stream<uint32_t>(_currentTile.PaletteOffset);

	Stream<uint8_t>(_nextTile.LowByte);
	Stream<uint8_t>(_nextTile.HighByte);
	Stream<uint32_t>(_nextTile.PaletteOffset);

	Stream<uint8_t>(_previousTile.LowByte);
	Stream<uint8_t>(_previousTile.HighByte);
	Stream<uint32_t>(_previousTile.PaletteOffset);

	StreamArray<int32_t>(_spriteX, 0x8);
	for(int i = 0; i < 8; i++) {
		Stream<uint8_t>(_spriteTiles[i].LowByte);
		Stream<uint8_t>(_spriteTiles[i].HighByte);
		Stream<uint32_t>(_spriteTiles[i].PaletteOffset);
		Stream<bool>(_spriteTiles[i].HorizontalMirror);
		Stream<bool>(_spriteTiles[i].BackgroundPriority);
	}
	Stream<uint32_t>(_spriteCount);
	Stream<uint32_t>(_secondaryOAMAddr);
	Stream<bool>(_sprite0Visible);

	Stream<uint8_t>(_oamCopybuffer);
	Stream<bool>(_writeOAMData);
	Stream<uint32_t>(_overflowCounter);
	Stream<bool>(_sprite0Added);

	Stream<NesModel>(_nesModel);

	if(!saving) {
		SetNesModel(_nesModel);
	}
}