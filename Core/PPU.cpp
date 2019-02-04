#include "stdafx.h"
#include "PPU.h"
#include "CPU.h"
#include "APU.h"
#include "EmulationSettings.h"
#include "VideoDecoder.h"
#include "Debugger.h"
#include "BaseMapper.h"
#include "RewindManager.h"
#include "ControlManager.h"
#include "MemoryManager.h"
#include "Console.h"
#include "NotificationManager.h"

PPU::PPU(shared_ptr<Console> console)
{
	_console = console;
	_settings = _console->GetSettings();

	_outputBuffers[0] = new uint16_t[256 * 240];
	_outputBuffers[1] = new uint16_t[256 * 240];

	_currentOutputBuffer = _outputBuffers[0];
	memset(_outputBuffers[0], 0, 256 * 240 * sizeof(uint16_t));
	memset(_outputBuffers[1], 0, 256 * 240 * sizeof(uint16_t));

	uint8_t paletteRamBootValues[0x20] { 0x09, 0x01, 0x00, 0x01, 0x00, 0x02, 0x02, 0x0D, 0x08, 0x10, 0x08, 0x24, 0x00, 0x00, 0x04, 0x2C,
		0x09, 0x01, 0x34, 0x03, 0x00, 0x04, 0x00, 0x14, 0x08, 0x3A, 0x00, 0x02, 0x00, 0x20, 0x2C, 0x08 };
	memcpy(_paletteRAM, paletteRamBootValues, sizeof(_paletteRAM));

	_console->GetMapper()->InitializeRam(_spriteRAM, 0x100);
	_console->GetMapper()->InitializeRam(_secondarySpriteRAM, 0x20);

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

	_needStateUpdate = false;
	_prevRenderingEnabled = false;
	_renderingEnabled = false;

	_ignoreVramRead = 0;
	_openBus = 0;
	memset(_openBusDecayStamp, 0, sizeof(_openBusDecayStamp));

	_state = {};
	_flags = {};
	_statusFlags = {};

	_previousTile = {};
	_currentTile = {};
	_nextTile = {};

	_ppuBusAddress = 0;
	_intensifyColorBits = 0;
	_paletteRamMask = 0x3F;
	_lastUpdatedPixel = -1;
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

	memset(_hasSprite, 0, sizeof(_hasSprite));
	memset(_spriteTiles, 0, sizeof(_spriteTiles));
	_spriteCount = 0;
	_secondaryOAMAddr = 0;
	_sprite0Visible = false;
	_overflowSpriteAddr = 0;
	_spriteIndex = 0;
	_openBus = 0;
	memset(_openBusDecayStamp, 0, sizeof(_openBusDecayStamp));
	_ignoreVramRead = 0;

	//First execution will be cycle 0, scanline 0
	_scanline = -1;
	_cycle = 340;

	_frameCount = 1;
	_memoryReadBuffer = 0;

	_overflowBugCounter = 0;

	_updateVramAddrDelay = 0;
	_updateVramAddr = 0;

	memset(_oamDecayCycles, 0, sizeof(_oamDecayCycles));
	_enableOamDecay = _settings->CheckFlag(EmulationFlags::EnableOamDecay);

	UpdateMinimumDrawCycles();
}

void PPU::SetNesModel(NesModel model)
{
	_nesModel = model;

	switch(_nesModel) {
		case NesModel::Auto:
			//Should never be Auto
			break;

		case NesModel::NTSC:
			_nmiScanline = 241;
			_vblankEnd = 260;
			_standardNmiScanline = 241;
			_standardVblankEnd = 260;
			break;
		case NesModel::PAL:
			_nmiScanline = 241;
			_vblankEnd = 310;
			_standardNmiScanline = 241;
			_standardVblankEnd = 310;
			break;
		case NesModel::Dendy:
			_nmiScanline = 291;
			_vblankEnd = 310;
			_standardNmiScanline = 291;
			_standardVblankEnd = 310;
			break;
	}

	_nmiScanline += _settings->GetPpuExtraScanlinesBeforeNmi();
	_palSpriteEvalScanline = _nmiScanline + 24;
	_standardVblankEnd += _settings->GetPpuExtraScanlinesBeforeNmi();
	_vblankEnd += _settings->GetPpuExtraScanlinesAfterNmi() + _settings->GetPpuExtraScanlinesBeforeNmi();
}

double PPU::GetOverclockRate()
{
	uint32_t regularVblankEnd;
	switch(_nesModel) {
		default:
		case NesModel::NTSC: regularVblankEnd = 260; break;
		case NesModel::PAL: regularVblankEnd = 310; break;
		case NesModel::Dendy: regularVblankEnd = 310; break;
	}

	return (double)(_vblankEnd + 2) / (regularVblankEnd + 2);
}

void PPU::GetState(PPUDebugState &state)
{
	state.ControlFlags = _flags;
	state.StatusFlags = _statusFlags;
	state.State = _state;
	state.Cycle = _cycle;
	state.Scanline = _scanline;
	state.FrameCount = _frameCount;
	state.NmiScanline = _nmiScanline;
	state.ScanlineCount = _vblankEnd + 2;
	state.SafeOamScanline = _nesModel == NesModel::NTSC ? _nmiScanline + 19 : _palSpriteEvalScanline;
	state.BusAddress = _ppuBusAddress;
	state.MemoryReadBuffer = _memoryReadBuffer;
}

void PPU::SetState(PPUDebugState &state)
{
	_flags = state.ControlFlags;
	_statusFlags = state.StatusFlags;
	_state = state.State;
	_cycle = state.Cycle;
	_scanline = state.Scanline;
	_frameCount = state.FrameCount;

	UpdateMinimumDrawCycles();

	_paletteRamMask = _flags.Grayscale ? 0x30 : 0x3F;
	if(_nesModel == NesModel::NTSC) {
		_intensifyColorBits = (_flags.IntensifyGreen ? 0x40 : 0x00) | (_flags.IntensifyRed ? 0x80 : 0x00) | (_flags.IntensifyBlue ? 0x100 : 0x00);
	} else if(_nesModel == NesModel::PAL || _nesModel == NesModel::Dendy) {
		//"Note that on the Dendy and PAL NES, the green and red bits swap meaning."
		_intensifyColorBits = (_flags.IntensifyRed ? 0x40 : 0x00) | (_flags.IntensifyGreen ? 0x80 : 0x00) | (_flags.IntensifyBlue ? 0x100 : 0x00);
	}
}

void PPU::UpdateVideoRamAddr()
{
	if(_scanline >= 240 || !IsRenderingEnabled()) {
		_state.VideoRamAddr = (_state.VideoRamAddr + (_flags.VerticalWrite ? 32 : 1)) & 0x7FFF;

		//Trigger memory read when setting the vram address - needed by MMC3 IRQ counter
		//"Should be clocked when A12 changes to 1 via $2007 read/write"
		SetBusAddress(_state.VideoRamAddr & 0x3FFF);
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

		_openBus = (uint8_t)openBus;
	}
}

uint8_t PPU::ApplyOpenBus(uint8_t mask, uint8_t value)
{
	SetOpenBus(~mask, value);
	return value | (_openBus & mask);
}

void PPU::ProcessStatusRegOpenBus(uint8_t &openBusMask, uint8_t &returnValue)
{
	switch(_settings->GetPpuModel()) {
		case PpuModel::Ppu2C05A: openBusMask = 0x00; returnValue |= 0x1B; break;
		case PpuModel::Ppu2C05B: openBusMask = 0x00; returnValue |= 0x3D; break;
		case PpuModel::Ppu2C05C: openBusMask = 0x00; returnValue |= 0x1C; break;
		case PpuModel::Ppu2C05D: openBusMask = 0x00; returnValue |= 0x1B; break;
		case PpuModel::Ppu2C05E: openBusMask = 0x00; break;
		default: break;
	}
}

uint8_t PPU::PeekRAM(uint16_t addr)
{
	//Used by debugger to get register values without side-effects (heavily edited copy of ReadRAM)
	uint8_t openBusMask = 0xFF;
	uint8_t returnValue = 0;
	switch(GetRegisterID(addr)) {
		case PPURegisters::Status:
			returnValue = ((uint8_t)_statusFlags.SpriteOverflow << 5) | ((uint8_t)_statusFlags.Sprite0Hit << 6) | ((uint8_t)_statusFlags.VerticalBlank << 7);
			if(_scanline == 241 && _cycle < 3) {
				//Clear vertical blank flag
				returnValue &= 0x7F;
			}
			openBusMask = 0x1F;
			ProcessStatusRegOpenBus(openBusMask, returnValue);
			break;

		case PPURegisters::SpriteData:
			if(!_settings->CheckFlag(EmulationFlags::DisablePpu2004Reads)) {
				if(_scanline <= 239 && IsRenderingEnabled()) {
					if(_cycle >= 257 && _cycle <= 320) {
						uint8_t step = ((_cycle - 257) % 8) > 3 ? 3 : ((_cycle - 257) % 8);
						uint8_t addr = (_cycle - 257) / 8 * 4 + step;
						returnValue = _secondarySpriteRAM[addr];
					} else {
						returnValue = _oamCopybuffer;
					}
				} else {
					returnValue = _spriteRAM[_state.SpriteRamAddr];
				}
				openBusMask = 0x00;
			}
			break;

		case PPURegisters::VideoMemoryData:
			returnValue = _memoryReadBuffer;

			if((_state.VideoRamAddr & 0x3FFF) >= 0x3F00 && !_settings->CheckFlag(EmulationFlags::DisablePaletteRead)) {
				returnValue = ReadPaletteRAM(_state.VideoRamAddr) | (_openBus & 0xC0);
				openBusMask = 0xC0;
			} else {
				openBusMask = 0x00;
			}
			break;

		default:
			break;
	}
	return returnValue | (_openBus & openBusMask);
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

			ProcessStatusRegOpenBus(openBusMask, returnValue);
			break;

		case PPURegisters::SpriteData:
			if(!_settings->CheckFlag(EmulationFlags::DisablePpu2004Reads)) {
				if(_scanline <= 239 && IsRenderingEnabled()) {
					//While the screen is begin drawn
					if(_cycle >= 257 && _cycle <= 320) {
						//If we're doing sprite rendering, set OAM copy buffer to its proper value.  This is done here for performance.
						//It's faster to only do this here when it's needed, rather than splitting LoadSpriteTileInfo() into an 8-step process
						uint8_t step = ((_cycle - 257) % 8) > 3 ? 3 : ((_cycle - 257) % 8);
						_secondaryOAMAddr = (_cycle - 257) / 8 * 4 + step;
						_oamCopybuffer = _secondarySpriteRAM[_secondaryOAMAddr];
					}
					//Return the value that PPU is currently using for sprite evaluation/rendering
					returnValue = _oamCopybuffer;
				} else {
					returnValue = ReadSpriteRam(_state.SpriteRamAddr);
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
				_memoryReadBuffer = ReadVram(_ppuBusAddress & 0x3FFF, MemoryOperationType::Read);

				if((_ppuBusAddress & 0x3FFF) >= 0x3F00 && !_settings->CheckFlag(EmulationFlags::DisablePaletteRead)) {
					returnValue = ReadPaletteRAM(_ppuBusAddress) | (_openBus & 0xC0);
					_console->DebugProcessVramReadOperation(MemoryOperationType::Read, _ppuBusAddress & 0x3FFF, returnValue);
					openBusMask = 0xC0;
				} else {
					openBusMask = 0x00;
				}

				UpdateVideoRamAddr();
				_ignoreVramRead = 6;
				_needStateUpdate = true;
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
			if(_settings->GetPpuModel() >= PpuModel::Ppu2C05A && _settings->GetPpuModel() <= PpuModel::Ppu2C05E) {
				SetMaskRegister(value);
			} else {
				SetControlRegister(value);
			}
			break;
		case PPURegisters::Mask:
			if(_settings->GetPpuModel() >= PpuModel::Ppu2C05A && _settings->GetPpuModel() <= PpuModel::Ppu2C05E) {
				SetControlRegister(value);
			} else {
				SetMaskRegister(value);
			}
			break;
		case PPURegisters::SpriteAddr:
			_state.SpriteRamAddr = value;
			break;
		case PPURegisters::SpriteData:
			if((_scanline >= 240 && (_nesModel != NesModel::PAL || _scanline < _palSpriteEvalScanline)) || !IsRenderingEnabled()) {
				if((_state.SpriteRamAddr & 0x03) == 0x02) {
					//"The three unimplemented bits of each sprite's byte 2 do not exist in the PPU and always read back as 0 on PPU revisions that allow reading PPU OAM through OAMDATA ($2004)"
					value &= 0xE3;
				}
				WriteSpriteRam(_state.SpriteRamAddr, value);
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

				//Video RAM update is apparently delayed by 2-3 PPU cycles (based on Visual NES findings)
				//A 3-cycle delay causes issues with the scanline test.
				_needStateUpdate = true;
				_updateVramAddrDelay = 2;
				_updateVramAddr = _state.TmpVideoRamAddr;
				_console->DebugSetLastFramePpuScroll(_updateVramAddr, _state.XScroll, false);
			} else {
				_state.TmpVideoRamAddr = (_state.TmpVideoRamAddr & ~0xFF00) | ((value & 0x3F) << 8);
			}
			_state.WriteToggle = !_state.WriteToggle;
			break;
		case PPURegisters::VideoMemoryData:
			if((_ppuBusAddress & 0x3FFF) >= 0x3F00) {
				WritePaletteRAM(_ppuBusAddress, value);
				_console->DebugProcessVramWriteOperation(_ppuBusAddress & 0x3FFF, value);
			} else {
				if(_scanline >= 240 || !IsRenderingEnabled()) {
					_console->GetMapper()->WriteVRAM(_ppuBusAddress & 0x3FFF, value);
				} else {
					//During rendering, the value written is ignored, and instead the address' LSB is used (not confirmed, based on Visual NES)
					_console->GetMapper()->WriteVRAM(_ppuBusAddress & 0x3FFF, _ppuBusAddress & 0xFF);
				}
			}
			UpdateVideoRamAddr();
			break;
		case PPURegisters::SpriteDMA:
			_console->GetCpu()->RunDMATransfer(value);
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
	return _paletteRAM[addr];
}

void PPU::WritePaletteRAM(uint16_t addr, uint8_t value)
{
	addr &= 0x1F;
	value &= 0x3F;
	if(addr == 0x00 || addr == 0x10) {
		_paletteRAM[0x00] = value;
		_paletteRAM[0x10] = value;
	} else if(addr == 0x04 || addr == 0x14) {
		_paletteRAM[0x04] = value;
		_paletteRAM[0x14] = value;
	} else if(addr == 0x08 || addr == 0x18) {
		_paletteRAM[0x08] = value;
		_paletteRAM[0x18] = value;
	} else if(addr == 0x0C || addr == 0x1C) {
		_paletteRAM[0x0C] = value;
		_paletteRAM[0x1C] = value;
	} else {
		_paletteRAM[addr] = value;
	}
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
		_console->GetCpu()->SetNmiFlag();
	}
	if(_scanline == 241 && _cycle < 3 && !_flags.VBlank) {
		_console->GetCpu()->ClearNmiFlag();
	}
}

void PPU::UpdateMinimumDrawCycles()
{
	_minimumDrawBgCycle = _flags.BackgroundEnabled ? ((_flags.BackgroundMask || _settings->CheckFlag(EmulationFlags::ForceBackgroundFirstColumn)) ? 0 : 8) : 300;
	_minimumDrawSpriteCycle = _flags.SpritesEnabled ? ((_flags.SpriteMask || _settings->CheckFlag(EmulationFlags::ForceSpritesFirstColumn)) ? 0 : 8) : 300;
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

	if(_renderingEnabled != (_flags.BackgroundEnabled | _flags.SpritesEnabled)) {
		_needStateUpdate = true;
	}

	UpdateMinimumDrawCycles();

	UpdateGrayscaleAndIntensifyBits();

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
		_console->GetCpu()->ClearNmiFlag();

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

void PPU::SetBusAddress(uint16_t addr)
{
	_ppuBusAddress = addr;
	_console->GetMapper()->NotifyVRAMAddressChange(addr);
}

uint8_t PPU::ReadVram(uint16_t addr, MemoryOperationType type)
{
	SetBusAddress(addr);
	return _console->GetMapper()->ReadVRAM(addr, type);
}

void PPU::WriteVram(uint16_t addr, uint8_t value)
{
	SetBusAddress(addr);
	_console->GetMapper()->WriteVRAM(addr, value);
}

void PPU::LoadTileInfo()
{
	if(IsRenderingEnabled()) {
		switch((_cycle - 1) & 0x07) {
			case 0: {
				_previousTile = _currentTile;
				_currentTile = _nextTile;

				_state.LowBitShift |= _nextTile.LowByte;
				_state.HighBitShift |= _nextTile.HighByte;

				uint8_t tileIndex = ReadVram(GetNameTableAddr());
				_nextTile.TileAddr = (tileIndex << 4) | (_state.VideoRamAddr >> 12) | _flags.BackgroundPatternAddr;
				_nextTile.OffsetY = _state.VideoRamAddr >> 12;
				break;
			}

			case 2: {
				uint8_t shift = ((_state.VideoRamAddr >> 4) & 0x04) | (_state.VideoRamAddr & 0x02);
				_nextTile.PaletteOffset = ((ReadVram(GetAttributeAddr()) >> shift) & 0x03) << 2;
				break;
			}

			case 3:
				_nextTile.LowByte = ReadVram(_nextTile.TileAddr);
				_nextTile.AbsoluteTileAddr = _console->GetMapper()->ToAbsoluteChrAddress(_nextTile.TileAddr);
				break;

			case 5:
				_nextTile.HighByte = ReadVram(_nextTile.TileAddr + 8);
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
		SpriteInfo &info = _spriteTiles[_spriteIndex];
		info.BackgroundPriority = backgroundPriority;
		info.HorizontalMirror = horizontalMirror;
		info.VerticalMirror = verticalMirror;
		info.PaletteOffset = ((attributes & 0x03) << 2) | 0x10;
		if(extraSprite) {
			//Use DebugReadVRAM for extra sprites to prevent side-effects.
			info.LowByte = _console->GetMapper()->DebugReadVRAM(tileAddr);
			info.HighByte = _console->GetMapper()->DebugReadVRAM(tileAddr + 8);
		} else {
			fetchLastSprite = false;
			info.LowByte = ReadVram(tileAddr);
			info.HighByte = ReadVram(tileAddr + 8);
		}
		info.TileAddr = tileAddr;
		info.AbsoluteTileAddr = _console->GetMapper()->ToAbsoluteChrAddress(tileAddr);
		info.OffsetY = lineOffset;
		info.SpriteX = spriteX;

		if(_scanline >= 0) {
			//Sprites read on prerender scanline are not shown on scanline 0
			for(int i = 0; i < 8 && spriteX + i + 1 < 257; i++) {
				_hasSprite[spriteX + i + 1] = true;
			}
		}
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

		ReadVram(tileAddr);
		ReadVram(tileAddr + 8);
	}

	_spriteIndex++;
}

void PPU::LoadExtraSprites()
{
	if(_spriteCount == 8 && _settings->CheckFlag(EmulationFlags::RemoveSpriteLimit)) {
		bool loadExtraSprites = true;
		
		if(_settings->CheckFlag(EmulationFlags::AdaptiveSpriteLimit)) {
			uint16_t lastPosition = 0xFFFF;
			uint8_t identicalSpriteCount = 0;
			uint8_t maxIdenticalSpriteCount = 0;
			for(int i = 0; i < 64; i++) {
				uint8_t y = _spriteRAM[i << 2];
				if(_scanline >= y && _scanline < y + (_flags.LargeSprites ? 16 : 8)) {
					uint8_t x = _spriteRAM[(i << 2) + 3];
					uint16_t position = (y << 8) | x;
					if(lastPosition != position) {
						if(identicalSpriteCount > maxIdenticalSpriteCount) {
							maxIdenticalSpriteCount = identicalSpriteCount;
						}
						lastPosition = position;
						identicalSpriteCount = 1;
					} else {
						identicalSpriteCount++;
					}
				}
			}
			loadExtraSprites = identicalSpriteCount < 8 && maxIdenticalSpriteCount < 8;
		}

		if(loadExtraSprites) {
			for(uint32_t i = _overflowSpriteAddr; i < 0x100; i += 4) {
				uint8_t spriteY = _spriteRAM[i];
				if(_scanline >= spriteY && _scanline < spriteY + (_flags.LargeSprites ? 16 : 8)) {
					LoadSprite(spriteY, _spriteRAM[i + 1], _spriteRAM[i + 2], _spriteRAM[i + 3], true);
					_spriteCount++;
				}
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

void PPU::ShiftTileRegisters()
{
	_state.LowBitShift <<= 1;
	_state.HighBitShift <<= 1;
}

uint8_t PPU::GetPixelColor()
{
	uint8_t offset = _state.XScroll;
	uint8_t backgroundColor = 0;
	uint8_t spriteBgColor = 0;

	if(_cycle > _minimumDrawBgCycle) {
		//BackgroundMask = false: Hide background in leftmost 8 pixels of screen
		spriteBgColor = (((_state.LowBitShift << offset) & 0x8000) >> 15) | (((_state.HighBitShift << offset) & 0x8000) >> 14);
		if(_settings->GetBackgroundEnabled()) {
			backgroundColor = spriteBgColor;
		}
	}

	if(_hasSprite[_cycle] && _cycle > _minimumDrawSpriteCycle) {
		//SpriteMask = true: Hide sprites in leftmost 8 pixels of screen
		for(uint8_t i = 0; i < _spriteCount; i++) {
			int32_t shift = (int32_t)_cycle - _spriteTiles[i].SpriteX - 1;
			if(shift >= 0 && shift < 8) {
				_lastSprite = &_spriteTiles[i];
				uint8_t spriteColor;
				if(_spriteTiles[i].HorizontalMirror) {
					spriteColor = ((_lastSprite->LowByte >> shift) & 0x01) | ((_lastSprite->HighByte >> shift) & 0x01) << 1;
				} else {
					spriteColor = ((_lastSprite->LowByte << shift) & 0x80) >> 7 | ((_lastSprite->HighByte << shift) & 0x80) >> 6;
				}

				if(spriteColor != 0) {
					//First sprite without a 00 color, use it.
					if(i == 0 && spriteBgColor != 0 && _sprite0Visible && _cycle != 256 && _flags.BackgroundEnabled && !_statusFlags.Sprite0Hit && _cycle > _minimumDrawSpriteStandardCycle) {
						//"The hit condition is basically sprite zero is in range AND the first sprite output unit is outputting a non-zero pixel AND the background drawing unit is outputting a non-zero pixel."
						//"Sprite zero hits do not register at x=255" (cycle 256)
						//"... provided that background and sprite rendering are both enabled"
						//"Should always miss when Y >= 239"
						_statusFlags.Sprite0Hit = true;

						_console->DebugProcessEvent(EventType::SpriteZeroHit);
					}

					if(_settings->GetSpritesEnabled() && (backgroundColor == 0 || !_spriteTiles[i].BackgroundPriority)) {
						//Check sprite priority
						return _lastSprite->PaletteOffset + spriteColor;
					}
					break;
				}
			}
		}
	}
	return ((offset + ((_cycle - 1) & 0x07) < 8) ? _previousTile : _currentTile).PaletteOffset + backgroundColor;
}

void PPU::DrawPixel()
{
	//This is called 3.7 million times per second - needs to be as fast as possible.
	if(IsRenderingEnabled() || ((_state.VideoRamAddr & 0x3F00) != 0x3F00)) {
		uint32_t color = GetPixelColor();
		_currentOutputBuffer[(_scanline << 8) + _cycle - 1] = _paletteRAM[color & 0x03 ? color : 0];
	} else {
		//"If the current VRAM address points in the range $3F00-$3FFF during forced blanking, the color indicated by this palette location will be shown on screen instead of the backdrop color."
		_currentOutputBuffer[(_scanline << 8) + _cycle - 1] = _paletteRAM[_state.VideoRamAddr & 0x1F];
	}
}

void PPU::UpdateGrayscaleAndIntensifyBits()
{
	if(_scanline < 0 || _scanline > _nmiScanline) {
		return;
	}

	int pixelNumber;
	if(_scanline >= 240) {
		pixelNumber = 61439;
	} else if(_cycle < 3) {
		pixelNumber = (_scanline << 8) - 1;
	} else if(_cycle <= 258) {
		pixelNumber = (_scanline << 8) + _cycle - 3;
	} else {
		pixelNumber = (_scanline << 8) + 255;
	}

	if(_paletteRamMask == 0x3F && _intensifyColorBits == 0) {
		//Nothing to do (most common case)
		_lastUpdatedPixel = pixelNumber;
		return;
	}

	if(_lastUpdatedPixel < pixelNumber) {
		uint16_t *out = _currentOutputBuffer + _lastUpdatedPixel + 1;
		while(_lastUpdatedPixel < pixelNumber) {
			*out = (*out & _paletteRamMask) | _intensifyColorBits;
			out++;
			_lastUpdatedPixel++;
		}
	}
}

void PPU::ProcessScanline()
{
	//Only called for cycle 1+
	if(_cycle <= 256) {
		LoadTileInfo();

		if(_prevRenderingEnabled && (_cycle & 0x07) == 0) {
			IncHorizontalScrolling();
			if(_cycle == 256) {
				IncVerticalScrolling();
			}
		}

		if(_scanline >= 0) {
			DrawPixel();
			ShiftTileRegisters();

			//"Secondary OAM clear and sprite evaluation do not occur on the pre-render line"
			ProcessSpriteEvaluation();
		} else if(_cycle < 9) {
			//Pre-render scanline logic
			if(_cycle == 1) {
				_statusFlags.VerticalBlank = false;
			}
			if(_state.SpriteRamAddr >= 0x08 && IsRenderingEnabled() && !_settings->CheckFlag(EmulationFlags::DisableOamAddrBug)) {
				//This should only be done if rendering is enabled (otherwise oam_stress test fails immediately)
				//"If OAMADDR is not less than eight when rendering starts, the eight bytes starting at OAMADDR & 0xF8 are copied to the first eight bytes of OAM"
				WriteSpriteRam(_cycle - 1, ReadSpriteRam((_state.SpriteRamAddr & 0xF8) + _cycle - 1));
			}
		}
	} else if(_cycle >= 257 && _cycle <= 320) {
		if(_cycle == 257) {
			_spriteIndex = 0;
			memset(_hasSprite, 0, sizeof(_hasSprite));
			if(_prevRenderingEnabled) {
				//copy horizontal scrolling value from t
				_state.VideoRamAddr = (_state.VideoRamAddr & ~0x041F) | (_state.TmpVideoRamAddr & 0x041F);
				_console->DebugSetLastFramePpuScroll(_state.VideoRamAddr, _state.XScroll, true);
			}
		}
		if(IsRenderingEnabled()) {
			//"OAMADDR is set to 0 during each of ticks 257-320 (the sprite tile loading interval) of the pre-render and visible scanlines." (When rendering)
			_state.SpriteRamAddr = 0;

			if((_cycle - 260) % 8 == 0) {
				//Cycle 260, 268, etc.  This is an approximation (each tile is actually loaded in 8 steps (e.g from 257 to 264))
				LoadSpriteTileInfo();
			} else if((_cycle - 257) % 8 == 0) {
				//Garbage NT sprite fetch (257, 265, 273, etc.) - Required for proper MC-ACC IRQs (MMC3 clone)
				ReadVram(GetNameTableAddr());
			} else if((_cycle - 259) % 8 == 0) {
				//Garbage AT sprite fetch
				ReadVram(GetAttributeAddr());
			}

			if(_scanline == -1 && _cycle >= 280 && _cycle <= 304) {
				//copy vertical scrolling value from t
				_state.VideoRamAddr = (_state.VideoRamAddr & ~0x7BE0) | (_state.TmpVideoRamAddr & 0x7BE0);
			}
		}
	} else if(_cycle >= 321 && _cycle <= 336) {
		LoadTileInfo();
		if(_cycle == 321) {
			if(IsRenderingEnabled()) {
				_oamCopybuffer = _secondarySpriteRAM[0];
			}
			if(_scanline == -1) {
				_console->DebugSetLastFramePpuScroll(_state.VideoRamAddr, _state.XScroll, false);
			}
		} else if(_prevRenderingEnabled && (_cycle == 328 || _cycle == 336)) {
			_state.LowBitShift <<= 8;
			_state.HighBitShift <<= 8;
			IncHorizontalScrolling();
		}
	} else if(_cycle == 337 || _cycle == 339) {
		if(IsRenderingEnabled()) {
			ReadVram(GetNameTableAddr());

			if(_scanline == -1 && _cycle == 339 && (_frameCount & 0x01) && _nesModel == NesModel::NTSC && _settings->GetPpuModel() == PpuModel::Ppu2C02) {
				//This behavior is NTSC-specific - PAL frames are always the same number of cycles
				//"With rendering enabled, each odd PPU frame is one PPU clock shorter than normal" (skip from 339 to 0, going over 340)
				_cycle = 340;
			}
		}
	}
}

void PPU::ProcessSpriteEvaluation()
{
	if(IsRenderingEnabled() || (_nesModel == NesModel::PAL && _scanline >= _palSpriteEvalScanline)) {
		if(_cycle < 65) {
			//Clear secondary OAM at between cycle 1 and 64
			_oamCopybuffer = 0xFF;
			_secondarySpriteRAM[(_cycle - 1) >> 1] = 0xFF;
		} else {
			if(_cycle == 65) {
				_sprite0Added = false;
				_spriteInRange = false;
				_secondaryOAMAddr = 0;
				_overflowSpriteAddr = 0;
				_overflowBugCounter = 0;

				_oamCopyDone = false;
				_spriteAddrH = (_state.SpriteRamAddr >> 2) & 0x3F;
				_spriteAddrL = _state.SpriteRamAddr & 0x03;
			} else if(_cycle == 256) {
				_sprite0Visible = _sprite0Added;
				_spriteCount = (_secondaryOAMAddr >> 2);
			}

			if(_cycle & 0x01) {
				//Read a byte from the primary OAM on odd cycles
				_oamCopybuffer = ReadSpriteRam(_state.SpriteRamAddr);
			} else {
				if(_oamCopyDone) {
					_spriteAddrH = (_spriteAddrH + 1) & 0x3F;
					if(_secondaryOAMAddr >= 0x20) {
						//"As seen above, a side effect of the OAM write disable signal is to turn writes to the secondary OAM into reads from it."
						_oamCopybuffer = _secondarySpriteRAM[_secondaryOAMAddr & 0x1F];
					}
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
						//"As seen above, a side effect of the OAM write disable signal is to turn writes to the secondary OAM into reads from it."
						_oamCopybuffer = _secondarySpriteRAM[_secondaryOAMAddr & 0x1F];

						//8 sprites have been found, check next sprite for overflow + emulate PPU bug
						if(_overflowSpriteAddr == 0) {
							//Used to remove sprite limit
							_overflowSpriteAddr = _spriteAddrH * 4;
						}

						if(_spriteInRange) {
							//Sprite is visible, consider this to be an overflow
							_statusFlags.SpriteOverflow = true;
							_spriteAddrL = (_spriteAddrL + 1);
							if(_spriteAddrL == 4) {
								_spriteAddrH = (_spriteAddrH + 1) & 0x3F;
								_spriteAddrL = 0;
							}

							if(_overflowBugCounter == 0) {
								_overflowBugCounter = 3;
							} else if(_overflowBugCounter > 0) {
								_overflowBugCounter--;
								if(_overflowBugCounter == 0) {
									//"After it finishes "fetching" this sprite(and setting the overflow flag), it realigns back at the beginning of this line and then continues here on the next sprite"
									_oamCopyDone = true;
									_spriteAddrL = 0;
								}
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
				_state.SpriteRamAddr = (_spriteAddrL & 0x03) | (_spriteAddrH << 2);
			}
		}
	}
}

uint8_t PPU::ReadSpriteRam(uint8_t addr)
{
	if(!_enableOamDecay) {
		return _spriteRAM[addr];
	} else {
		int32_t elapsedCycle = _console->GetCpu()->GetElapsedCycles(_oamDecayCycles[addr >> 3]);
		if(elapsedCycle <= PPU::OamDecayCycleCount) {
			_oamDecayCycles[addr >> 3] = _console->GetCpu()->GetCycleCount();
			return _spriteRAM[addr];
		} else {
			if(_flags.SpritesEnabled) {
				shared_ptr<Debugger> debugger = _console->GetDebugger(false);
				if(debugger && debugger->CheckFlag(DebuggerFlags::BreakOnDecayedOamRead)) {
					//When debugging with the break on decayed oam read flag turned on, break (only if sprite rendering is enabled to avoid false positives)
					debugger->BreakImmediately(BreakSource::BreakOnDecayedOamRead);
				}
			}
			//If this 8-byte row hasn't been read/written to in over 3000 cpu cycles (~1.7ms), return 0xFF to simulate decay
			return 0x10;
		}
	}
}

void PPU::WriteSpriteRam(uint8_t addr, uint8_t value)
{
	_spriteRAM[addr] = value;
	if(_enableOamDecay) {
		_oamDecayCycles[addr >> 3] = _console->GetCpu()->GetCycleCount();
	}
}

void PPU::DebugSendFrame()
{
	_console->GetVideoDecoder()->UpdateFrame(_currentOutputBuffer);
}

void PPU::DebugCopyOutputBuffer(uint16_t *target)
{
	memcpy(target, _currentOutputBuffer, PPU::PixelCount * sizeof(uint16_t));
}

void PPU::SendFrame()
{
	UpdateGrayscaleAndIntensifyBits();

	_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::PpuFrameDone, _currentOutputBuffer);

#ifdef LIBRETRO
	_console->GetVideoDecoder()->UpdateFrameSync(_currentOutputBuffer);
#else 
	if(_console->GetRewindManager()->IsRewinding()) {
		if(!_console->GetRewindManager()->IsStepBack()) {
			_console->GetVideoDecoder()->UpdateFrameSync(_currentOutputBuffer);
		}
	} else {
		//If VideoDecoder isn't done with the previous frame, UpdateFrame will block until it is ready to accept a new frame.
		_console->GetVideoDecoder()->UpdateFrame(_currentOutputBuffer);
	}

	_enableOamDecay = _settings->CheckFlag(EmulationFlags::EnableOamDecay);
#endif
}

void PPU::BeginVBlank()
{
	TriggerNmi();
}

void PPU::TriggerNmi()
{
	_statusFlags.VerticalBlank = true;
	if(_flags.VBlank) {
		_console->GetCpu()->SetNmiFlag();
	}
}

void PPU::UpdateApuStatus()
{
	APU* apu = _console->GetApu();
	apu->SetApuStatus(true);
	if(_scanline > 240) {
		if(_scanline > _standardVblankEnd) {
			//Disable APU for extra lines after NMI
			apu->SetApuStatus(false);
		} else if(_scanline >= _standardNmiScanline && _scanline < _nmiScanline) {
			//Disable APU for extra lines before NMI
			apu->SetApuStatus(false);
		}
	}
}

void PPU::DebugUpdateFrameBuffer(bool toGrayscale)
{
	//Clear output buffer for "Draw partial frame" feature
	if(toGrayscale) {
		for(int i = 0; i < PPU::PixelCount; i++) {
			_currentOutputBuffer[i] &= 0x30;
		}
	} else {
		memset(_currentOutputBuffer, 0, PPU::PixelCount * 2);
	}
}

void PPU::Exec()
{
	if(_cycle > 339) {
		_cycle = 0;
		if(++_scanline > _vblankEnd) {
			_lastUpdatedPixel = -1;
			_scanline = -1;
			UpdateMinimumDrawCycles();
		}

		_console->DebugProcessPpuCycle();
		
		UpdateApuStatus();
		
		if(_scanline == _settings->GetInputPollScanline()) {
			_console->GetControlManager()->UpdateInputState();
		}

		//Cycle = 0
		if(_scanline == -1) {
			_statusFlags.SpriteOverflow = false;
			_statusFlags.Sprite0Hit = false;
			
			//Switch to alternate output buffer (VideoDecoder may still be decoding the last frame buffer)
			_currentOutputBuffer = (_currentOutputBuffer == _outputBuffers[0]) ? _outputBuffers[1] : _outputBuffers[0];
		} else if(_scanline == 240) {
			//At the start of vblank, the bus address is set back to VideoRamAddr.
			//According to Visual NES, this occurs on scanline 240, cycle 1, but is done here on cycle for performance reasons
			SetBusAddress(_state.VideoRamAddr);
			SendFrame();
			_frameCount++;
		} else if(_scanline == _nmiScanline) {
			BeginVBlank();
		}
	} else {
		//Cycle > 0
		_cycle++;

		_console->DebugProcessPpuCycle();
		if(_scanline < 240) {
			ProcessScanline();
		} else if(_nesModel == NesModel::PAL && _scanline >= _palSpriteEvalScanline) {
			//"On a PAL machine, because of its extended vertical blank, the PPU begins refreshing OAM roughly 21 scanlines after NMI[2], to prevent it 
			//from decaying during the longer hiatus of rendering. Additionally, it will continue to refresh during the visible portion of the screen 
			//even if rendering is disabled. Because of this, OAM DMA must be done near the beginning of vertical blank on PAL, and everywhere else 
			//it is liable to conflict with the refresh. Since the refresh can't be disabled like on the NTSC hardware, OAM decay does not occur at all on the PAL NES."
			if(_cycle <= 256) {
				ProcessSpriteEvaluation();
			} else if(_cycle >= 257 && _cycle < 320) {
				_state.SpriteRamAddr = 0;
			}
		}
	}

	if(_needStateUpdate) {
		UpdateState();
	}
}

void PPU::UpdateState()
{
	_needStateUpdate = false;

	//Rendering enabled flag is apparently set with a 1 cycle delay (i.e setting it at cycle 5 will render cycle 6 like cycle 5 and then take the new settings for cycle 7)
	_prevRenderingEnabled = _renderingEnabled;
	_renderingEnabled = _flags.BackgroundEnabled | _flags.SpritesEnabled;
	if(_prevRenderingEnabled != _renderingEnabled) {
		_needStateUpdate = true;
	}

	if(_prevRenderingEnabled && !_renderingEnabled && _cycle >= 65 && _cycle <= 256 && _scanline < 240) {
		//Disabling rendering during OAM evaluation will trigger a glitch causing the current address to be incremented by 1
		//The increment can be "delayed" by 1 PPU cycle depending on whether or not rendering is disabled on an even/odd cycle
		//e.g, if rendering is disabled on an even cycle, the following PPU cycle will increment the address by 5 (instead of 4)
		//     if rendering is disabled on an odd cycle, the increment will wait until the next odd cycle (at which point it will be incremented by 1)
		//In practice, there is no way to see the difference, so we just increment by 1 at the end of the next cycle after rendering was disabled
		_state.SpriteRamAddr++;
	}

	if(_updateVramAddrDelay > 0) {
		_updateVramAddrDelay--;
		if(_updateVramAddrDelay == 0) {
			_state.VideoRamAddr = _updateVramAddr;

			if(_scanline >= 240 || !IsRenderingEnabled()) {
				//Only set the VRAM address on the bus if the PPU is rendering
				//More info here: https://forums.nesdev.com/viewtopic.php?p=132145#p132145
				//Trigger bus address change when setting the vram address - needed by MMC3 IRQ counter
				//"4) Should be clocked when A12 changes to 1 via $2006 write"
				SetBusAddress(_state.VideoRamAddr & 0x3FFF);
			}
		} else {
			_needStateUpdate = true;
		}
	}

	if(_ignoreVramRead > 0) {
		_ignoreVramRead--;
		if(_ignoreVramRead > 0) {
			_needStateUpdate = true;
		}
	}
}

void PPU::ProcessCpuClock()
{
	if(!_settings->HasOverclock()) {
		Exec();
		Exec();
		Exec();
		if(_nesModel == NesModel::PAL && _console->GetCpu()->GetCycleCount() % 5 == 0) {
			//PAL PPU runs 3.2 clocks for every CPU clock, so we need to run an extra clock every 5 CPU clocks
			Exec();
		}
	} else {
		if(_nesModel == NesModel::PAL) {
			//PAL PPU runs 3.2 clocks for every CPU clock, so we need to run an extra clock every 5 CPU clocks
			_cyclesNeeded += 3.2 / (_settings->GetOverclockRate() / 100.0);
		} else {
			_cyclesNeeded += 3.0 / (_settings->GetOverclockRate() / 100.0);
		}

		while(_cyclesNeeded >= 1.0) {
			Exec();
			_cyclesNeeded--;
		}
	}
}

uint8_t* PPU::GetSpriteRam()
{
	//Used by debugger
	if(_enableOamDecay) {
		for(int i = 0; i < 0x100; i++) {
			//Apply OAM decay to sprite RAM before letting debugger access it
			if(_console->GetCpu()->GetElapsedCycles(_oamDecayCycles[i >> 3]) > PPU::OamDecayCycleCount) {
				_spriteRAM[i] = 0x10;
			}
		}
	}
	return _spriteRAM;
}

uint32_t PPU::GetPixelBrightness(uint8_t x, uint8_t y)
{
	//Used by Zapper, gives a rough approximation of the brightness level of the specific pixel
	uint16_t pixelData = _currentOutputBuffer[y << 8 | x];
	uint32_t argbColor = _settings->GetRgbPalette()[pixelData & 0x3F];
	return (argbColor & 0xFF) + ((argbColor >> 8) & 0xFF) + ((argbColor >> 16) & 0xFF);
}

void PPU::StreamState(bool saving)
{
	ArrayInfo<uint8_t> paletteRam = { _paletteRAM, 0x20 };
	ArrayInfo<uint8_t> spriteRam = { _spriteRAM, 0x100 };
	ArrayInfo<uint8_t> secondarySpriteRam = { _secondarySpriteRAM, 0x20 };
	ArrayInfo<int32_t> openBusDecayStamp = { _openBusDecayStamp, 8 };

	bool disablePpu2004Reads = false;
	bool disablePaletteRead = false;
	bool disableOamAddrBug = false;

	if(saving) {
		disablePpu2004Reads = _settings->CheckFlag(EmulationFlags::DisablePpu2004Reads);
		disablePaletteRead = _settings->CheckFlag(EmulationFlags::DisablePaletteRead);
		disableOamAddrBug = _settings->CheckFlag(EmulationFlags::DisableOamAddrBug);
	}

	Stream(_state.Control, _state.Mask, _state.Status, _state.SpriteRamAddr, _state.VideoRamAddr, _state.XScroll, _state.TmpVideoRamAddr, _state.WriteToggle,
		_state.HighBitShift, _state.LowBitShift, _flags.VerticalWrite, _flags.SpritePatternAddr, _flags.BackgroundPatternAddr, _flags.LargeSprites, _flags.VBlank,
		_flags.Grayscale, _flags.BackgroundMask, _flags.SpriteMask, _flags.BackgroundEnabled, _flags.SpritesEnabled, _flags.IntensifyRed, _flags.IntensifyGreen,
		_flags.IntensifyBlue, _paletteRamMask, _intensifyColorBits, _statusFlags.SpriteOverflow, _statusFlags.Sprite0Hit, _statusFlags.VerticalBlank, _scanline,
		_cycle, _frameCount, _memoryReadBuffer, _currentTile.LowByte, _currentTile.HighByte, _currentTile.PaletteOffset, _nextTile.LowByte, _nextTile.HighByte,
		_nextTile.PaletteOffset, _nextTile.TileAddr, _previousTile.LowByte, _previousTile.HighByte, _previousTile.PaletteOffset, _spriteIndex, _spriteCount,
		_secondaryOAMAddr, _sprite0Visible, _oamCopybuffer, _spriteInRange, _sprite0Added, _spriteAddrH, _spriteAddrL, _oamCopyDone, _nesModel,
		_prevRenderingEnabled, _renderingEnabled, _openBus, _ignoreVramRead, paletteRam, spriteRam, secondarySpriteRam,
		openBusDecayStamp, _cyclesNeeded, disablePpu2004Reads, disablePaletteRead, disableOamAddrBug, _overflowBugCounter, _updateVramAddr, _updateVramAddrDelay,
		_needStateUpdate, _ppuBusAddress);

	for(int i = 0; i < 64; i++) {
		Stream(_spriteTiles[i].SpriteX, _spriteTiles[i].LowByte, _spriteTiles[i].HighByte, _spriteTiles[i].PaletteOffset, _spriteTiles[i].HorizontalMirror, _spriteTiles[i].BackgroundPriority);
	}

	if(!saving) {
		_settings->SetFlagState(EmulationFlags::DisablePpu2004Reads, disablePpu2004Reads);
		_settings->SetFlagState(EmulationFlags::DisablePaletteRead, disablePaletteRead);
		_settings->SetFlagState(EmulationFlags::DisableOamAddrBug, disableOamAddrBug);

		SetNesModel(_nesModel);
		UpdateMinimumDrawCycles();

		for(int i = 0; i < 0x20; i++) {
			//Set oam decay cycle to the current cycle to ensure it doesn't decay when loading a state
			_oamDecayCycles[i] = _console->GetCpu()->GetCycleCount();
		}

		for(int i = 0; i < 257; i++) {
			_hasSprite[i] = true;
		}

		_lastUpdatedPixel = -1;

		UpdateApuStatus();
	}
}