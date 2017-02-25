#include "stdafx.h"
#include "PPU.h"
#include "CPU.h"
#include "EmulationSettings.h"
#include "VideoDecoder.h"
#include "Debugger.h"
#include "BaseMapper.h"

PPU* PPU::Instance = nullptr;

PPU::PPU(MemoryManager *memoryManager)
{
	PPU::Instance = this;

	EmulationSettings::SetPpuModel(PpuModel::Ppu2C02);

	_memoryManager = memoryManager;
	_outputBuffers[0] = new uint16_t[256 * 240];
	_outputBuffers[1] = new uint16_t[256 * 240];

	_currentOutputBuffer = _outputBuffers[0];
	memset(_outputBuffers[0], 0, 256 * 240 * sizeof(uint16_t));
	memset(_outputBuffers[1], 0, 256 * 240 * sizeof(uint16_t));

	uint8_t paletteRamBootValues[0x20] { 0x09, 0x01, 0x00, 0x01, 0x00, 0x02, 0x02, 0x0D, 0x08, 0x10, 0x08, 0x24, 0x00, 0x00, 0x04, 0x2C,
												0x09, 0x01, 0x34, 0x03, 0x00, 0x04, 0x00, 0x14, 0x08, 0x3A, 0x00, 0x02, 0x00, 0x20, 0x2C, 0x08 };
	memcpy(_paletteRAM, paletteRamBootValues, sizeof(_paletteRAM));
	
	BaseMapper::InitializeRam(_spriteRAM, 0x100);
	BaseMapper::InitializeRam(_secondarySpriteRAM, 0x20);

	_simpleMode = false;

	Reset();
}

PPU::~PPU() 
{
	delete[] _outputBuffers[0];
	delete[] _outputBuffers[1];
}

void PPU::Reset()
{
	_cyclesNeeded = 0;

	_prevRenderingEnabled = false;
	_renderingEnabled = false;
	_skipScrollingIncrement = true;

	_ignoreVramRead = 0;
	_openBus = 0;
	memset(_openBusDecayStamp, 0, sizeof(_openBusDecayStamp));

	_state = {};
	_flags = {};
	_statusFlags = {};

	_intensifyColorBits = 0;
	_paletteRamMask = 0;
	_lastSprite = nullptr;
	_oamCopybuffer = 0;
	_spriteInRange = false;
	_sprite0Added = false;
	_spriteAddrH = 0;
	_spriteAddrL = 0;
	_oamCopyDone = false;
	_renderingEnabled = false;
	_prevRenderingEnabled = false;
	_cyclesNeeded = 0.0;
	_simpleMode = false;
	_skipScrollingIncrement = false;

	memset(_spriteTiles, 0, sizeof(SpriteInfo));	
	_spriteCount = 0;
	_secondaryOAMAddr = 0;
	_sprite0Visible = false;
	_overflowSpriteAddr = 0;
	_spriteIndex = 0;
	_openBus = 0;
	memset(_openBusDecayStamp, 0, sizeof(_openBusDecayStamp));
	_ignoreVramRead = 0;

	_scanline = 0;
	_cycle = 0;
	_frameCount = 1;
	_memoryReadBuffer = 0;
	
	UpdateMinimumDrawCycles();
}

void PPU::SetNesModel(NesModel model)
{
	_nesModel = model;

	switch(_nesModel) {
		case NesModel::NTSC:
			_nmiScanline = 241;
			_vblankEnd = 260;
			EmulationSettings::SetPpuScanlineCount(262);
			break;
		case NesModel::PAL:
			_nmiScanline = 241;
			_vblankEnd = 310;
			EmulationSettings::SetPpuScanlineCount(312);
			break;
		case NesModel::Dendy:
			_nmiScanline = 291;
			_vblankEnd = 310;
			EmulationSettings::SetPpuScanlineCount(312);
			break;
	}

	_nmiScanline += EmulationSettings::GetPpuExtraScanlinesBeforeNmi();
	_vblankEnd += EmulationSettings::GetPpuExtraScanlinesAfterNmi() + EmulationSettings::GetPpuExtraScanlinesBeforeNmi();
}

PPUDebugState PPU::GetState()
{
	PPUDebugState state;
	state.ControlFlags = _flags;
	state.StatusFlags = _statusFlags;
	state.State = _state;
	state.Cycle = _cycle;
	state.Scanline = _scanline;
	state.FrameCount = _frameCount;
	return state;
}

void PPU::SetState(PPUDebugState state)
{
	_flags = state.ControlFlags;
	_statusFlags = state.StatusFlags;
	_state = state.State;
	_cycle = state.Cycle;
	_scanline = state.Scanline;
	_frameCount = state.FrameCount;
}

void PPU::UpdateVideoRamAddr()
{
	if(_scanline >= 240 || !IsRenderingEnabled()) {
		_state.VideoRamAddr += _flags.VerticalWrite ? 32 : 1;
		
		//Trigger memory read when setting the vram address - needed by MMC3 IRQ counter
		//"Should be clocked when A12 changes to 1 via $2007 read/write"
		_memoryManager->ReadVRAM(_state.VideoRamAddr, MemoryOperationType::Read);
	} else {
		//"During rendering (on the pre-render line and the visible lines 0-239, provided either background or sprite rendering is enabled), "
		//it will update v in an odd way, triggering a coarse X increment and a Y increment simultaneously"
		IncHorizontalScrolling();
		IncVerticalScrolling();
	}
}

void PPU::SetOpenBus(uint8_t mask, uint8_t value)
{
	//Decay expired bits, set new bits and update stamps on each individual bit
	if(mask == 0xFF) {
		//Shortcut when mask is 0xFF - all bits are set to the value and stamps updated
		_openBus = value;
		for(int i = 0; i < 8; i++) {
			_openBusDecayStamp[i] = _frameCount;
		}
	} else {
		uint16_t openBus = (_openBus << 8);
		for(int i = 0; i < 8; i++) {
			openBus >>= 1;
			if(mask & 0x01) {
				if(value & 0x01) {
					openBus |= 0x80;
				} else {
					openBus &= 0xFF7F;
				}
				_openBusDecayStamp[i] = _frameCount;
			} else if(_frameCount - _openBusDecayStamp[i] > 30) {
				openBus &= 0xFF7F;
			}
			value >>= 1;
			mask >>= 1;
		}

		_openBus = openBus & 0xFF;
	}
}

uint8_t PPU::ApplyOpenBus(uint8_t mask, uint8_t value)
{
	SetOpenBus(~mask, value);
	return value | (_openBus & mask);
}

uint8_t PPU::ReadRAM(uint16_t addr)
{
	uint8_t openBusMask = 0xFF;
	uint8_t returnValue = 0;
	switch(GetRegisterID(addr)) {
		case PPURegisters::Status:
			_state.WriteToggle = false;
			_flags.IntensifyBlue = false;
			UpdateStatusFlag();
			returnValue = _state.Status;
			openBusMask = 0x1F;

			switch(EmulationSettings::GetPpuModel()) {
				case PpuModel::Ppu2C05A: openBusMask = 0x00; returnValue |= 0x1B; break;
				case PpuModel::Ppu2C05B: openBusMask = 0x00; returnValue |= 0x3D; break;
				case PpuModel::Ppu2C05C: openBusMask = 0x00; returnValue |= 0x1C; break;
				case PpuModel::Ppu2C05D: openBusMask = 0x00; returnValue |= 0x1B; break;
				case PpuModel::Ppu2C05E: openBusMask = 0x00; break;
			}
			break;

		case PPURegisters::SpriteData:
			if(!EmulationSettings::CheckFlag(EmulationFlags::DisablePpu2004Reads)) {
				if(_scanline <= 239 && IsRenderingEnabled() && (_cycle >= 257 || _cycle <= 64)) {
					if(_cycle >= 257 && _cycle <= 320) {
						//Set OAM copy buffer to its proper value.  This is done here for performance.
						//It's faster to only do this here when it's needed, rather than splitting LoadSpriteTileInfo() into an 8-step process
						uint8_t step = ((_cycle - 257) % 8) > 3 ? 3 : ((_cycle - 257) % 8);
						_secondaryOAMAddr = (_cycle - 257) / 8 * 4 + step;
						_oamCopybuffer = _secondarySpriteRAM[_secondaryOAMAddr];
					}
					returnValue = _oamCopybuffer;
				} else {
					returnValue = _spriteRAM[_state.SpriteRamAddr];
				}
				openBusMask = 0x00;
			}
			break;

		case PPURegisters::VideoMemoryData:
			if(_ignoreVramRead) {
				//2 reads to $2007 in quick succession (2 consecutive CPU cycles) causes the 2nd read to be ignored (normally depends on PPU/CPU timing, but this is the simplest solution)
				//Return open bus in this case? (which will match the last value read)
				openBusMask = 0xFF;
			} else {
				returnValue = _memoryReadBuffer;
				_memoryReadBuffer = _memoryManager->ReadVRAM(_state.VideoRamAddr, MemoryOperationType::Read);

				if((_state.VideoRamAddr & 0x3FFF) >= 0x3F00 && !EmulationSettings::CheckFlag(EmulationFlags::DisablePaletteRead)) {
					returnValue = ReadPaletteRAM(_state.VideoRamAddr) | (_openBus & 0xC0);
					openBusMask = 0xC0;
				} else {
					openBusMask = 0x00;
				}

				UpdateVideoRamAddr();
				_ignoreVramRead = 2;
			}				
			break;

		default:
			break;
	}
	return ApplyOpenBus(openBusMask, returnValue);
}

void PPU::WriteRAM(uint16_t addr, uint8_t value)
{
	if(addr != 0x4014) {
		SetOpenBus(0xFF, value);
	}

	switch(GetRegisterID(addr)) {
		case PPURegisters::Control:
			if(EmulationSettings::GetPpuModel() >= PpuModel::Ppu2C05A && EmulationSettings::GetPpuModel() <= PpuModel::Ppu2C05E) {
				SetMaskRegister(value);
			} else {
				SetControlRegister(value);				
			}
			break;
		case PPURegisters::Mask:
			if(EmulationSettings::GetPpuModel() >= PpuModel::Ppu2C05A && EmulationSettings::GetPpuModel() <= PpuModel::Ppu2C05E) {
				SetControlRegister(value);
			} else {				
				SetMaskRegister(value);
			}
			break;
		case PPURegisters::SpriteAddr:
			_state.SpriteRamAddr = value;
			break;
		case PPURegisters::SpriteData:
			if(_scanline >= 240 || !IsRenderingEnabled()) {
				if((_state.SpriteRamAddr & 0x03) == 0x02) {
					//"The three unimplemented bits of each sprite's byte 2 do not exist in the PPU and always read back as 0 on PPU revisions that allow reading PPU OAM through OAMDATA ($2004)"
					value &= 0xE3;
				}
				_spriteRAM[_state.SpriteRamAddr] = value;
				_state.SpriteRamAddr = (_state.SpriteRamAddr + 1) & 0xFF;
			} else {
				//"Writes to OAMDATA during rendering (on the pre-render line and the visible lines 0-239, provided either sprite or background rendering is enabled) do not modify values in OAM, 
				//but do perform a glitchy increment of OAMADDR, bumping only the high 6 bits"
				_state.SpriteRamAddr = (_state.SpriteRamAddr + 4) & 0xFF;
			}
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
				_memoryManager->ReadVRAM(_state.VideoRamAddr, MemoryOperationType::Read);

				if(_cycle == 255) {
					_skipScrollingIncrement = true;
				}
			} else {
				_state.TmpVideoRamAddr = (_state.TmpVideoRamAddr & ~0xFF00) | ((value & 0x3F) << 8);
			}
			_state.WriteToggle = !_state.WriteToggle;
			break;
		case PPURegisters::VideoMemoryData:
			if((_state.VideoRamAddr & 0x3FFF) >= 0x3F00) {
				WritePaletteRAM(_state.VideoRamAddr, value);
			} else {
				_memoryManager->WriteVRAM(_state.VideoRamAddr, value);
			}
			UpdateVideoRamAddr();
			break;
		case PPURegisters::SpriteDMA:
			CPU::RunDMATransfer(value);
			break;
		default:
			break;
	}
}

uint8_t PPU::ReadPaletteRAM(uint16_t addr)
{
	addr &= 0x1F;
	if(addr == 0x10 || addr == 0x14 || addr == 0x18 || addr == 0x1C) {
		addr &= ~0x10;
	}
	return (_paletteRAM[addr] & _paletteRamMask);
}

void PPU::WritePaletteRAM(uint16_t addr, uint8_t value)
{
	addr &= 0x1F;
	if(addr == 0x10 || addr == 0x14 || addr == 0x18 || addr == 0x1C) {
		addr &= ~0x10;
	}
	_paletteRAM[addr] = value;
}

bool PPU::IsRenderingEnabled()
{
	return _renderingEnabled;
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
	
	if(!originalVBlank && _flags.VBlank && _statusFlags.VerticalBlank && (_scanline != -1 || _cycle != 0)) {
		CPU::SetNMIFlag();
	}
	if(_scanline == 241 && _cycle < 3 && !_flags.VBlank) {
		CPU::ClearNMIFlag();
	}
}

void PPU::UpdateMinimumDrawCycles()
{
	_minimumDrawBgCycle = _flags.BackgroundEnabled ? ((_flags.BackgroundMask || EmulationSettings::CheckFlag(EmulationFlags::ForceBackgroundFirstColumn)) ? 0 : 8) : 300;
	_minimumDrawSpriteCycle = _flags.SpritesEnabled ? ((_flags.SpriteMask || EmulationSettings::CheckFlag(EmulationFlags::ForceSpritesFirstColumn)) ? 0 : 8) : 300;
	_minimumDrawSpriteStandardCycle = _flags.SpritesEnabled ? (_flags.SpriteMask ? 0 : 8) : 300;
}

void PPU::SetMaskRegister(uint8_t value)
{
	_state.Mask = value;
	_flags.Grayscale = (_state.Mask & 0x01) == 0x01;
	_flags.BackgroundMask = (_state.Mask & 0x02) == 0x02;
	_flags.SpriteMask = (_state.Mask & 0x04) == 0x04;
	_flags.BackgroundEnabled = (_state.Mask & 0x08) == 0x08;
	_flags.SpritesEnabled = (_state.Mask & 0x10) == 0x10;
	_flags.IntensifyBlue = (_state.Mask & 0x80) == 0x80;

	UpdateMinimumDrawCycles();

	 //"Bit 0 controls a greyscale mode, which causes the palette to use only the colors from the grey column: $00, $10, $20, $30. This is implemented as a bitwise AND with $30 on any value read from PPU $3F00-$3FFF"
	_paletteRamMask = _flags.Grayscale ? 0x30 : 0x3F;

	if(_nesModel == NesModel::NTSC) {
		_flags.IntensifyRed = (_state.Mask & 0x20) == 0x20;
		_flags.IntensifyGreen = (_state.Mask & 0x40) == 0x40;
		_intensifyColorBits = (value & 0xE0) << 1;
	} else if(_nesModel == NesModel::PAL || _nesModel == NesModel::Dendy) {
		//"Note that on the Dendy and PAL NES, the green and red bits swap meaning."
		_flags.IntensifyRed = (_state.Mask & 0x40) == 0x40;
		_flags.IntensifyGreen = (_state.Mask & 0x20) == 0x20;
		_intensifyColorBits = (_flags.IntensifyRed ? 0x40 : 0x00) | (_flags.IntensifyGreen ? 0x80 : 0x00) | (_flags.IntensifyBlue ? 0x100 : 0x00);
	}
}

void PPU::UpdateStatusFlag()
{
	_state.Status = ((uint8_t)_statusFlags.SpriteOverflow << 5) |
						 ((uint8_t)_statusFlags.Sprite0Hit << 6) |
						 ((uint8_t)_statusFlags.VerticalBlank << 7);
	_statusFlags.VerticalBlank = false;

	if(_scanline == 241 && _cycle < 3) {
		//"Reading on the same PPU clock or one later reads it as set, clears it, and suppresses the NMI for that frame."
		_statusFlags.VerticalBlank = false;		
		CPU::ClearNMIFlag();

		if(_cycle == 0) {
			//"Reading one PPU clock before reads it as clear and never sets the flag or generates NMI for that frame. "
			_state.Status = ((uint8_t)_statusFlags.SpriteOverflow << 5) | ((uint8_t)_statusFlags.Sprite0Hit << 6);
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
		uint16_t tileIndex, shift;
		switch((_cycle - 1) & 0x07) {
			case 0:
				_previousTile = _currentTile;
				_currentTile = _nextTile;

				if(_cycle > 1 && _cycle < 256) {
					LoadNextTile();
				}

				tileIndex = _memoryManager->ReadVRAM(GetNameTableAddr());
				_nextTile.TileAddr = (tileIndex << 4) | (_state.VideoRamAddr >> 12) | _flags.BackgroundPatternAddr;
				_nextTile.OffsetY = _state.VideoRamAddr >> 12;
				break;

			case 2:
				shift = ((_state.VideoRamAddr >> 4) & 0x04) | (_state.VideoRamAddr & 0x02);
				_nextTile.PaletteOffset = ((_memoryManager->ReadVRAM(GetAttributeAddr()) >> shift) & 0x03) << 2;
				break;

			case 3:
				_nextTile.LowByte = _memoryManager->ReadVRAM(_nextTile.TileAddr);
				break;

			case 5:
				_nextTile.HighByte = _memoryManager->ReadVRAM(_nextTile.TileAddr + 8);
				if(_cycle == 334) {
					InitializeShiftRegisters();
				}
				break;
		}		
	}
}

void PPU::LoadSprite(uint8_t spriteY, uint8_t tileIndex, uint8_t attributes, uint8_t spriteX, bool extraSprite)
{
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

	bool fetchLastSprite = true;	
	if((_spriteIndex < _spriteCount || extraSprite) && spriteY < 240) {
		_spriteTiles[_spriteIndex].BackgroundPriority = backgroundPriority;
		_spriteTiles[_spriteIndex].HorizontalMirror = horizontalMirror;
		_spriteTiles[_spriteIndex].VerticalMirror = verticalMirror;
		_spriteTiles[_spriteIndex].PaletteOffset = ((attributes & 0x03) << 2) | 0x10;
		if(extraSprite) {
			//Use DebugReadVRAM for extra sprites to prevent side-effects.
			_spriteTiles[_spriteIndex].LowByte = _memoryManager->DebugReadVRAM(tileAddr);
			_spriteTiles[_spriteIndex].HighByte = _memoryManager->DebugReadVRAM(tileAddr + 8);
		} else {
			fetchLastSprite = false;
			_spriteTiles[_spriteIndex].LowByte = _memoryManager->ReadVRAM(tileAddr);
			_spriteTiles[_spriteIndex].HighByte = _memoryManager->ReadVRAM(tileAddr + 8);
		}
		_spriteTiles[_spriteIndex].TileAddr = tileAddr;
		_spriteTiles[_spriteIndex].OffsetY = lineOffset;
		_spriteTiles[_spriteIndex].SpriteX = spriteX;
	} 
	
	if(fetchLastSprite) {
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

	_spriteIndex++;
}

void PPU::LoadExtraSprites()
{
	if(_spriteCount == 8 && EmulationSettings::CheckFlag(EmulationFlags::RemoveSpriteLimit)) {
		for(uint32_t i = _overflowSpriteAddr; i < 0x100; i += 4) {
			uint8_t spriteY = _spriteRAM[i];
			if(_scanline >= spriteY && _scanline < spriteY + (_flags.LargeSprites ? 16 : 8)) {
				LoadSprite(spriteY, _spriteRAM[i + 1], _spriteRAM[i + 2], _spriteRAM[i + 3], true);
				_spriteCount++;
			}
		}
	}
}

void PPU::LoadSpriteTileInfo()
{
	uint8_t *spriteAddr = _secondarySpriteRAM + _spriteIndex * 4;
	LoadSprite(*spriteAddr, *(spriteAddr+1), *(spriteAddr+2), *(spriteAddr+3), false);
	if(_cycle == 316) {
		LoadExtraSprites();
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

uint32_t PPU::GetPixelColor(uint32_t &paletteOffset)
{
	uint8_t offset = _state.XScroll;
	uint32_t backgroundColor = 0;
	uint32_t spriteBgColor = 0;
	
	if(_cycle > _minimumDrawBgCycle) {
		//BackgroundMask = false: Hide background in leftmost 8 pixels of screen
		spriteBgColor = (((_state.LowBitShift << offset) & 0x8000) >> 15) | (((_state.HighBitShift << offset) & 0x8000) >> 14);
		if(EmulationSettings::GetBackgroundEnabled()) {
			backgroundColor = spriteBgColor;
		}
	}

	if(_cycle > _minimumDrawSpriteCycle) {
		//SpriteMask = true: Hide sprites in leftmost 8 pixels of screen
		for(uint8_t i = 0; i < _spriteCount; i++) {
			int32_t shift = -((int32_t)_spriteTiles[i].SpriteX - (int32_t)_cycle + 1);
			if(shift >= 0 && shift < 8) {
				_lastSprite = &_spriteTiles[i];
				uint32_t spriteColor;
				if(_spriteTiles[i].HorizontalMirror) {
					spriteColor = ((_lastSprite->LowByte >> shift) & 0x01) | ((_lastSprite->HighByte >> shift) & 0x01) << 1;
				} else {
					spriteColor = ((_lastSprite->LowByte << shift) & 0x80) >> 7 | ((_lastSprite->HighByte << shift) & 0x80) >> 6;
				}
				
				if(spriteColor != 0) {
					//First sprite without a 00 color, use it.
					if(i == 0 && backgroundColor != 0 && _sprite0Visible && _cycle != 256 && _flags.BackgroundEnabled && !_statusFlags.Sprite0Hit && _cycle > _minimumDrawSpriteStandardCycle) {
						//"The hit condition is basically sprite zero is in range AND the first sprite output unit is outputting a non-zero pixel AND the background drawing unit is outputting a non-zero pixel."
						//"Sprite zero hits do not register at x=255" (cycle 256)
						//"... provided that background and sprite rendering are both enabled"
						//"Should always miss when Y >= 239"
						_statusFlags.Sprite0Hit = true;
					}

					if(EmulationSettings::GetSpritesEnabled() && (backgroundColor == 0 || !_spriteTiles[i].BackgroundPriority)) {
						//Check sprite priority
						paletteOffset = _lastSprite->PaletteOffset;						
						return spriteColor;
					}
					break;
				}
			}
		}
	} 
	paletteOffset = ((offset + ((_cycle - 1) & 0x07) < 8) ? _previousTile : _currentTile).PaletteOffset;
	return backgroundColor;
}

void PPU::DrawPixel()
{
	//This is called 3.7 million times per second - needs to be as fast as possible.
	uint16_t &pixel = _currentOutputBuffer[(_scanline << 8) + _cycle - 1];

	if(IsRenderingEnabled() || ((_state.VideoRamAddr & 0x3F00) != 0x3F00)) {
		uint32_t paletteOffset;
		uint32_t color = GetPixelColor(paletteOffset);
		if(color == 0) {
			pixel = ReadPaletteRAM(0x3F00) | _intensifyColorBits;
		} else {
			pixel = ReadPaletteRAM(0x3F00 + paletteOffset + color) | _intensifyColorBits;
		}
	} else {
		//"If the current VRAM address points in the range $3F00-$3FFF during forced blanking, the color indicated by this palette location will be shown on screen instead of the backdrop color."
		pixel = ReadPaletteRAM(_state.VideoRamAddr) | _intensifyColorBits;
	}
}

void PPU::ProcessPreVBlankScanline()
{
	//For pre-render scanline & all visible scanlines
	if(_prevRenderingEnabled) {
		//Use _prevRenderingEnabled to drive vert/horiz scrolling increments.
		//This delays the flag by an extra cycle.  So if rendering is disabled at cycle 254,
		//the vertical scrolling increment will not be performed.
		//This appears to fix freezes in Battletoads (Level 2), but may be incorrect.

		//Update video ram address according to scrolling logic
		if((_cycle > 0 && _cycle < 256 && (_cycle & 0x07) == 0) || _cycle == 328 || _cycle == 336) {
			IncHorizontalScrolling();
		} else if(_cycle == 256 && !_skipScrollingIncrement) {
			IncVerticalScrolling();
		} else if(_cycle == 257) {
			//copy horizontal scrolling value from t
			_state.VideoRamAddr = (_state.VideoRamAddr & ~0x041F) | (_state.TmpVideoRamAddr & 0x041F);
		}
	}
	_skipScrollingIncrement = false;

	if(_cycle >= 257 && _cycle <= 320) {
		if(_cycle == 257) {
			_spriteIndex = 0;
		}
		if(IsRenderingEnabled()) {
			//"OAMADDR is set to 0 during each of ticks 257-320 (the sprite tile loading interval) of the pre-render and visible scanlines." (When rendering)
			_state.SpriteRamAddr = 0;

			if((_cycle - 260) % 8 == 0) {
				//Cycle 260, 268, etc.  This is an approximation (each tile is actually loaded in 8 steps (e.g from 257 to 264))
				LoadSpriteTileInfo();
			} else if((_cycle - 257) % 8 == 0) {
				//Garbage NT sprite fetch (257, 265, 273, etc.) - Required for proper MC-ACC IRQs (MMC3 clone)
				_memoryManager->ReadVRAM(GetNameTableAddr());
			} else if((_cycle - 259) % 8 == 0) {
				//Garbage AT sprite fetch
				_memoryManager->ReadVRAM(GetAttributeAddr());
			}
		}
	} else if(_cycle == 321 && IsRenderingEnabled()) {
		_oamCopybuffer = _secondarySpriteRAM[0];
	}
}

void PPU::ProcessPrerenderScanline()
{
	ProcessPreVBlankScanline();

	if(_cycle == 0) {
		_statusFlags.SpriteOverflow = false;
		_statusFlags.Sprite0Hit = false;
	} else if(_cycle == 1) {
		_statusFlags.VerticalBlank = false;
	}
	
	if(_cycle >= 1 && _cycle <= 256) {
		LoadTileInfo();
		CopyOAMData();
	} else if(_cycle >= 280 && _cycle <= 304) {
		if(IsRenderingEnabled()) {
			//copy vertical scrolling value from t
			_state.VideoRamAddr = (_state.VideoRamAddr & ~0x7BE0) | (_state.TmpVideoRamAddr & 0x7BE0);
		}
	} else if(_nesModel == NesModel::NTSC && _cycle == 339 && IsRenderingEnabled() && (_frameCount & 0x01)) {
		//This behavior is NTSC-specific - PAL frames are always the same number of cycles
		//"With rendering enabled, each odd PPU frame is one PPU clock shorter than normal" (skip from 339 to 0, going over 340)
		_cycle = 340;
	} else if(_cycle >= 321 && _cycle <= 336) {
		if(_cycle == 321) {
			Debugger::SetLastFramePpuScroll(
				((_state.VideoRamAddr & 0x1F) << 3) | _state.XScroll | ((_state.VideoRamAddr & 0x400) ? 0x100 : 0),
				(((_state.VideoRamAddr & 0x3E0) >> 2) | ((_state.VideoRamAddr & 0x7000) >> 12)) + ((_state.VideoRamAddr & 0x800) ? 240 : 0)
			);
		}
		LoadTileInfo();
	} else if(_cycle == 337 || _cycle == 339) {
		if(IsRenderingEnabled()) {
			_memoryManager->ReadVRAM(GetNameTableAddr());
		}
	}
}

void PPU::ProcessVisibleScanline()
{
	if(_cycle > 0 && _cycle <= 256) {
		LoadTileInfo();

		DrawPixel();
		ShiftTileRegisters();
	
		CopyOAMData();
	} else if(_cycle >= 321 && _cycle <= 336) {
		LoadTileInfo();
	} else if(_cycle == 337 || _cycle == 339) {
		if(IsRenderingEnabled()) {
			_memoryManager->ReadVRAM(GetNameTableAddr());
		}
	}

	ProcessPreVBlankScanline();
}

void PPU::CopyOAMData()
{
	if(_nesModel == NesModel::NTSC && IsRenderingEnabled() || _nesModel == NesModel::PAL && (_scanline < 240 || _scanline > 260)) {
		if(_cycle < 65) {
			if(_cycle < 9 && _state.SpriteRamAddr >= 0x08 && _scanline == -1 && !EmulationSettings::CheckFlag(EmulationFlags::DisableOamAddrBug)) {
				//This should only be done if rendering is enabled (otherwise oam_stress test fails immediately)
				//"If OAMADDR is not less than eight when rendering starts, the eight bytes starting at OAMADDR & 0xF8 are copied to the first eight bytes of OAM"
				_spriteRAM[_cycle - 1] = _spriteRAM[((_state.SpriteRamAddr & 0xF8) + _cycle - 1) & 0xFF];
			}

			//Clear secondary OAM at between cycle 1 and 64
			_oamCopybuffer = 0xFF;
			_secondarySpriteRAM[(_cycle - 1) >> 1] = 0xFF;
		} else {
			if(_cycle == 65) {
				_sprite0Added = false;
				_spriteInRange = false;
				_secondaryOAMAddr = 0;
				_overflowSpriteAddr = 0;

				_oamCopyDone = false;
				_spriteAddrH = (_state.SpriteRamAddr >> 2) & 0x3F;
				_spriteAddrL = _state.SpriteRamAddr & 0x03;
			} else if(_cycle == 256) {
				_sprite0Visible = _sprite0Added;
				_spriteCount = (_secondaryOAMAddr >> 2);
			}

			if(_cycle & 0x01) {
				//Read a byte from the primary OAM on odd cycles
				_oamCopybuffer = _spriteRAM[(_spriteAddrH << 2) + _spriteAddrL];
			} else {
				if(_oamCopyDone) {
					_spriteAddrH = (_spriteAddrH + 1) & 0x3F;
				} else {
					if(!_spriteInRange && _scanline >= _oamCopybuffer && _scanline < _oamCopybuffer + (_flags.LargeSprites ? 16 : 8)) {
						_spriteInRange = true;
					}

					if(_secondaryOAMAddr < 0x20) {
						//Copy 1 byte to secondary OAM
						_secondarySpriteRAM[_secondaryOAMAddr] = _oamCopybuffer;

						if(_spriteInRange) {
							_spriteAddrL++;
							_secondaryOAMAddr++;

							if(_spriteAddrH == 0) {
								_sprite0Added = true;
							}

							if(_spriteAddrL == 4) {
								//Done copying all 4 bytes
								_spriteInRange = false;
								_spriteAddrL = 0;
								_spriteAddrH = (_spriteAddrH + 1) & 0x3F;
								if(_spriteAddrH == 0) {
									_oamCopyDone = true;
								}
							}
						} else {
							//Nothing to copy, skip to next sprite
							_spriteAddrH = (_spriteAddrH + 1) & 0x3F;
							if(_spriteAddrH == 0) {
								_oamCopyDone = true;
							}
						}
					} else {
						//8 sprites have been found, check next sprite for overflow + emulate PPU bug
						if(_overflowSpriteAddr == 0) {
							//Used to remove sprite limit
							_overflowSpriteAddr = _spriteAddrH * 4;
						}

						if(_spriteInRange) {
							//Sprite is visible, consider this to be an overflow
							_statusFlags.SpriteOverflow = true;
							_spriteAddrL = (_spriteAddrL + 1) & 0x03;
							if(_spriteAddrL == 4) {
								_spriteInRange = false;
								_spriteAddrH = (_spriteAddrH + 1) & 0x3F;
								_oamCopyDone = true;
								_spriteAddrL = 0;
							}
						} else {
							//Sprite isn't on this scanline, trigger sprite evaluation bug - increment both H & L at the same time
							_spriteAddrH = (_spriteAddrH + 1) & 0x3F;
							_spriteAddrL = (_spriteAddrL + 1) & 0x03;

							if(_spriteAddrH == 0) {
								_oamCopyDone = true;
							}
						}
					}
				}
			}
		}
	}
}

void PPU::DebugSendFrame()
{
	VideoDecoder::GetInstance()->UpdateFrame(_currentOutputBuffer);
}

void PPU::SendFrame()
{
	MessageManager::SendNotification(ConsoleNotificationType::PpuFrameDone, _currentOutputBuffer);	
	
	VideoDecoder::GetInstance()->UpdateFrame(_currentOutputBuffer);
	
	//Switch output buffer.  VideoDecoder will decode the last frame while we build the new one.
	//If VideoDecoder isn't fast enough, UpdateFrame will block until it is ready to accept a new frame.
	_currentOutputBuffer = (_currentOutputBuffer == _outputBuffers[0]) ? _outputBuffers[1] : _outputBuffers[0];
	if(Debugger::IsEnabled()) {
		memset(_currentOutputBuffer, 0, PPU::PixelCount * 2);
	}
}

void PPU::BeginVBlank()
{
	if(_cycle == 0) {
		SendFrame();
		TriggerNmi();
	}
}

void PPU::TriggerNmi()
{
	_statusFlags.VerticalBlank = true;
	if(_flags.VBlank) {
		CPU::SetNMIFlag();
	}
}

void PPU::Exec()
{
	if(_cycle > 339) {
		_cycle = -1;

		if(++_scanline > _vblankEnd) {
			_frameCount++;
			_scanline = -1;
			UpdateMinimumDrawCycles();
		}
	}
	_cycle++;

	Debugger::ProcessPpuCycle();

	if(!_simpleMode) {
		if(_scanline != -1 && _scanline < 240) {
			ProcessVisibleScanline();
		} else if(_scanline == -1) {
			ProcessPrerenderScanline();
		} else if(_scanline == _nmiScanline) {
			BeginVBlank();
		} else if(_nesModel == NesModel::PAL && _scanline > _nmiScanline + 20) {
			//"On a PAL machine, because of its extended vertical blank, the PPU begins refreshing OAM roughly 21 scanlines after NMI[2], to prevent it 
			//from decaying during the longer hiatus of rendering. Additionally, it will continue to refresh during the visible portion of the screen 
			//even if rendering is disabled. Because of this, OAM DMA must be done near the beginning of vertical blank on PAL, and everywhere else 
			//it is liable to conflict with the refresh. Since the refresh can't be disabled like on the NTSC hardware, OAM decay does not occur at all on the PAL NES."
			if(_cycle > 0 && _cycle <= 256) {
				CopyOAMData();
			} else if(_cycle >= 257 && _cycle < 320) {
				_state.SpriteRamAddr = 0;
			}
		}
	} else {
		//Used by NSF player to speed things up
		if(_scanline == _nmiScanline) {
			BeginVBlank();
		}
	}

	//Rendering enabled flag is apparently set with a 1 cycle delay (i.e setting it at cycle 5 will render cycle 6 like cycle 5 and then take the new settings for cycle 7)
	_prevRenderingEnabled = _renderingEnabled;
	_renderingEnabled = _flags.BackgroundEnabled || _flags.SpritesEnabled;
}

void PPU::ExecStatic()
{
	double overclockRate = EmulationSettings::GetOverclockRate();
	if(overclockRate == 100) {
		PPU::Instance->Exec();
		PPU::Instance->Exec();
		PPU::Instance->Exec();
		if(PPU::Instance->_nesModel == NesModel::PAL && CPU::GetCycleCount() % 5 == 0) {
			//PAL PPU runs 3.2 clocks for every CPU clock, so we need to run an extra clock every 5 CPU clocks
			PPU::Instance->Exec();
		}
	} else {
		if(PPU::Instance->_nesModel == NesModel::PAL) {
			//PAL PPU runs 3.2 clocks for every CPU clock, so we need to run an extra clock every 5 CPU clocks
			Instance->_cyclesNeeded += 3.2 / (overclockRate / 100.0);
		} else {
			Instance->_cyclesNeeded += 3.0 / (overclockRate / 100.0);
		}

		while(Instance->_cyclesNeeded >= 1.0) {
			PPU::Instance->Exec();
			Instance->_cyclesNeeded--;
		}
	}

	if(PPU::Instance->_ignoreVramRead) {
		PPU::Instance->_ignoreVramRead--;
	}
}

void PPU::StreamState(bool saving)
{
	ArrayInfo<uint8_t> paletteRam = { _paletteRAM, 0x20 };
	ArrayInfo<uint8_t> spriteRam = { _spriteRAM, 0x100 };
	ArrayInfo<uint8_t> secondarySpriteRam = { _secondarySpriteRAM, 0x20 };
	ArrayInfo<int32_t> openBusDecayStamp = { _openBusDecayStamp, 8 };
	
	uint16_t unusedSpriteDmaAddr = 0;
	uint16_t unusedSpriteDmaCounter = 0;

	Stream(_state.Control, _state.Mask, _state.Status, _state.SpriteRamAddr, _state.VideoRamAddr, _state.XScroll, _state.TmpVideoRamAddr, _state.WriteToggle,
		_state.HighBitShift, _state.LowBitShift, _flags.VerticalWrite, _flags.SpritePatternAddr, _flags.BackgroundPatternAddr, _flags.LargeSprites, _flags.VBlank,
		_flags.Grayscale, _flags.BackgroundMask, _flags.SpriteMask, _flags.BackgroundEnabled, _flags.SpritesEnabled, _flags.IntensifyRed, _flags.IntensifyGreen,
		_flags.IntensifyBlue, _paletteRamMask, _intensifyColorBits, _statusFlags.SpriteOverflow, _statusFlags.Sprite0Hit, _statusFlags.VerticalBlank, _scanline,
		_cycle, _frameCount, _memoryReadBuffer, _currentTile.LowByte, _currentTile.HighByte, _currentTile.PaletteOffset, _nextTile.LowByte, _nextTile.HighByte,
		_nextTile.PaletteOffset, _nextTile.TileAddr, _previousTile.LowByte, _previousTile.HighByte, _previousTile.PaletteOffset, _spriteIndex, _spriteCount,
		_secondaryOAMAddr, _sprite0Visible, _oamCopybuffer, _spriteInRange, _sprite0Added, _spriteAddrH, _spriteAddrL, _oamCopyDone, _nesModel, unusedSpriteDmaAddr,
		unusedSpriteDmaCounter, _prevRenderingEnabled, _renderingEnabled, _openBus, _ignoreVramRead, _skipScrollingIncrement, paletteRam, spriteRam, secondarySpriteRam,
		openBusDecayStamp, _cyclesNeeded);

	for(int i = 0; i < 64; i++) {
		Stream(_spriteTiles[i].SpriteX, _spriteTiles[i].LowByte, _spriteTiles[i].HighByte, _spriteTiles[i].PaletteOffset, _spriteTiles[i].HorizontalMirror, _spriteTiles[i].BackgroundPriority);
	}

	if(!saving) {
		SetNesModel(_nesModel);
		UpdateMinimumDrawCycles();
	}
}