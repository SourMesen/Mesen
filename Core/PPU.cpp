#include "stdafx.h"
#include "PPU.h"
#include "CPU.h"

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

uint8_t *PPU::FrameBuffer = new uint8_t[256*240*4];
atomic<int> PPU::WaitCounter = 0;

PPU::PPU(MemoryManager *memoryManager)
{
	_memoryManager = memoryManager;
	_state = {};
	_flags = {};
	_statusFlags = {};

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
			_state.VideoRamAddr += _flags.VerticalWrite ? 32 : 1;

			if(_state.VideoRamAddr >= 0x3F00) {
				//No buffer for palette
				//TODO: Update read buffer when reading palette (See: http://wiki.nesdev.com/w/index.php/PPU_registers#The_PPUDATA_read_buffer_.28post-fetch.29)
				return _memoryReadBuffer;
			} else {
				return returnValue;
			}
		default:
			//other registers are meant to be read-only
			break;
	}
	return 0;
}

void PPU::WriteRAM(uint16_t addr, uint8_t value)
{
	static int counter = 0;
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
			_state.SpriteRamAddr++;
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
			if(counter < 50) {
				std::cout << "=> " << std::hex << (short)value << std::endl;
			}

			if(_state.WriteToggle) {
				_state.TmpVideoRamAddr = (_state.TmpVideoRamAddr & ~0x00FF) | value;
				_state.VideoRamAddr = _state.TmpVideoRamAddr;
				if(counter < 50) {
					std::cout << std::hex << _state.VideoRamAddr << std::endl;
				}
				counter++;

			} else {
				_state.TmpVideoRamAddr = (_state.TmpVideoRamAddr & ~0xFF00) | ((value & 0x3F) << 8);
			}
			_state.WriteToggle = !_state.WriteToggle;
			break;
		case PPURegisters::VideoMemoryData:
			if(_state.VideoRamAddr == 0x2001 || _state.VideoRamAddr == 0x2401 || _state.VideoRamAddr == 0x2801 || _state.VideoRamAddr == 0x2C01) {
				//std::cout << "test";
			}
			_memoryManager->WriteVRAM(_state.VideoRamAddr, value);
			_state.VideoRamAddr += _flags.VerticalWrite ? 32 : 1;
			break;
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
		//std::cout << std::endl;
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
uint16_t PPU::GetTileAddr()
{
	return 0x2000 | (_state.VideoRamAddr & 0x0FFF);
}

//Take from http://wiki.nesdev.com/w/index.php/The_skinny_on_NES_scrolling#Tile_and_attribute_fetching
uint16_t PPU::GetAttributeAddr()
{
	return 0x23C0 | (_state.VideoRamAddr & 0x0C00) | ((_state.VideoRamAddr >> 4) & 0x38) | ((_state.VideoRamAddr >> 2) & 0x07);
}

void PPU::UpdateScrolling()
{
	//For pre-render scanline & all visible scanlines
	if(_cycle == 256) {
		IncVerticalScrolling();
	} else if(_cycle == 257) {
		//copy horizontal scrolling value from t
		_state.VideoRamAddr = (_state.VideoRamAddr & ~0x041F) | (_state.TmpVideoRamAddr & 0x041F);
	} else if((_cycle % 8 == 0 && _cycle > 0 && _cycle < 256) || _cycle == 328 || _cycle == 336) {
		IncHorizontalScrolling();
	} else if((_cycle - 1) % 8 == 0 && _cycle < 250) {
		LoadTileInfo();
	} else if(_cycle == 321 || _cycle == 329) {
		LoadTileInfo();
	}
}

void PPU::ProcessPrerenderScanline()
{
	if(IsRenderingEnabled()) {
		UpdateScrolling();
	}

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
	}
}

void PPU::LoadTileInfo()
{
	_currentTile = _nextTile;

	uint16_t tileIndex = _memoryManager->ReadVRAM(GetTileAddr());
	uint16_t tileAddr = (tileIndex << 4) | (_state.VideoRamAddr >> 12) | _flags.BackgroundPatternAddr;
	
	//std::cout << std::hex << GetAttributeAddr() << " ";
	uint16_t shift = _state.VideoRamAddr&0x3FF;
	_nextTile.Attributes = ((_memoryManager->ReadVRAM(GetAttributeAddr()) >> shift) & 0x03) << 2;
	_nextTile.LowByte = _memoryManager->ReadVRAM(tileAddr);
	_nextTile.HighByte = _memoryManager->ReadVRAM(tileAddr + 8);

	_state.LowBitShift = (_state.LowBitShift << 8) | _nextTile.LowByte;
	_state.HighBitShift = (_state.HighBitShift << 8) | _nextTile.HighByte;
}

void PPU::DrawPixel()
{
	uint8_t palette = 0;

	uint8_t tileXPixel = _cycle % 8;
	uint32_t bufferPosition = _scanline * 256 + _cycle;

	uint8_t fineXScroll = _state.XScroll;

	uint8_t offset = (15 - tileXPixel - fineXScroll);

	uint8_t pixelColor = ((_state.LowBitShift >> offset) & 0x01) | (((_state.HighBitShift >> offset) & 0x01) << 1);

	// If we're grabbing the pixel from the high
	// part of the shift register, use the buffered
	// palette, not the current one
	
	if(pixelColor != 0 && _frameCount > 1000) {
		//std::cout << std::hex << (short)pixelColor << std::endl;
	}

	if(offset < 8) {
		palette = GetBGPaletteEntry(_nextTile.Attributes, pixelColor);
	} else {
		palette = GetBGPaletteEntry(_currentTile.Attributes, pixelColor);
	}
	/*
	if(p->palettebuffer[fbRow].value != 0) {
	// Pixel is already rendered and priority
	// 1 means show behind background
	continue;
	}*/

	//p->palettebuffer[fbRow].color = PPU_PALETTE_RGB[palette % 64];
	((uint32_t*)_outputBuffer)[bufferPosition] = PPU_PALETTE_RGB[palette % 64] | (0xFF << 24);
	//p->palettebuffer[fbRow].value = pixel;
	//p->palettebuffer[fbRow].pindex = -1;
}

void PPU::ProcessVisibleScanline()
{
	if(IsRenderingEnabled()) {
		UpdateScrolling();
	}

	if(_cycle <= 254) {
		DrawPixel();
	}

	if(_cycle == 254) {
		//DrawScanline();
		if(_scanline == 239) {
			CopyFrame();
			//std::cout << std::endl << std::endl << std::endl;
		}
		if(_flags.BackgroundEnabled) {
			//Ppu_renderTileRow(p);
		}

		if(_flags.SpritesEnabled) {
			//Ppu_evaluateScanlineSprites(p, p->scanline);
		}
	}
}

void PPU::DrawScanline()
{
	// Generates each tile, one scanline at a time and applies the palette

	// Move first tile into shift registers
	//PpuTileAttributes tileAttrs;
	//FetchTileAttributes(&tileAttrs);
/*	_state.LowBitShift = tileAttrs.low;
	_state.HighBitShift = tileAttrs.high;
	uint8_t attr = tileAttrs.attr;

	FetchTileAttributes(&tileAttrs);
	// Get second tile, move the pixels into the right side of
	// shift registers
	_state.LowBitShift = (_state.LowBitShift << 8) | tileAttrs.low;
	_state.HighBitShift = (_state.HighBitShift << 8) | tileAttrs.high;
	// Current tile to render is attrBuf
	uint8_t attrBuf = tileAttrs.attr;

	for(int x = 0; x < 32; x++) {
		int palette = 0;

		for(unsigned int b = 0; b < 8; b++) {
			int intB = b;
			int fbRow = _scanline * 256 + ((x * 8) + intB);

			unsigned int uintFineX = _state.XScroll;
			uint16_t pixel = (_state.LowBitShift >> (15 - b - uintFineX)) & 0x01;
			pixel += ((_state.HighBitShift >> (15 - b - uintFineX) & 0x01) << 1);

			// If we're grabbing the pixel from the high
			// part of the shift register, use the buffered
			// palette, not the current one
			if((15 - b - uintFineX) < 8) {
				palette = GetBGPaletteEntry(attrBuf, pixel);
			} else {
				palette = GetBGPaletteEntry(attr, pixel);
			}
			
			if(p->palettebuffer[fbRow].value != 0) {
			// Pixel is already rendered and priority
			// 1 means show behind background
			continue;
			}

			//p->palettebuffer[fbRow].color = PPU_PALETTE_RGB[palette % 64];
			_outputBuffer[fbRow] = PPU_PALETTE_RGB[palette % 64];
			//p->palettebuffer[fbRow].value = pixel;
			//p->palettebuffer[fbRow].pindex = -1;
		}

		// xcoord = p->registers.vramAddress & 0x1F
		attr = attrBuf;

		// Shift the first tile out, bring the new tile in
		FetchTileAttributes(&tileAttrs);
		_state.LowBitShift = (_state.LowBitShift << 8) | tileAttrs.low;
		_state.HighBitShift = (_state.HighBitShift << 8) | tileAttrs.high;
		attrBuf = tileAttrs.attr;
	}*/
}

uint8_t PPU::GetBGPaletteEntry(uint8_t a, uint16_t pix) 
{
	uint16_t baseAddr = 0x3F00;
	if(pix == 0x0) {
		return _memoryManager->ReadVRAM(baseAddr);
	}

	switch(a) {
		case 0x0:
			return _memoryManager->ReadVRAM(baseAddr + pix);
		case 0x4:
			return _memoryManager->ReadVRAM(baseAddr + 0x04 + pix);
		case 0x8:
			return _memoryManager->ReadVRAM(baseAddr + 0x08 + pix);
		case 0xC:
			return _memoryManager->ReadVRAM(baseAddr + 0x0C + pix);
	}

	return 0;
}

void PPU::CopyFrame()
{
	int counter = PPU::WaitCounter.fetch_add(1);
	if(counter != 0) {
		//We weren't the first thread to increment the value, wait until other locks are released
		while(PPU::WaitCounter > 1) {}
	}
	
	memcpy(PPU::FrameBuffer, _outputBuffer, 256 * 240 * 4);	
	PPU::WaitCounter--;
}

uint8_t* PPU::GetFrame()
{
	uint8_t *copyBuffer = new uint8_t[256 * 240 * 4];
	
	int counter = PPU::WaitCounter.fetch_add(1);
	if(counter != 0) {
		//We weren't the first thread to increment the value, wait until other locks are released
		while(PPU::WaitCounter > 1) {}
	}
	
	memcpy(copyBuffer, PPU::FrameBuffer, 256 * 240 * 4);
	PPU::WaitCounter--;

	return copyBuffer;
}

void PPU::BeginVBlank()
{
	if(_cycle == 1) {
		_statusFlags.VerticalBlank = true;
		/*if(!_suppressVBlank) {
			// We're in VBlank
			Ppu_setStatus(p, STATUS_VBLANK_STARTED);
			p->cycleCount = 0;
		}*/
		if(_flags.VBlank) {
			CPU::SetNMIFlag();
		}
		//Ppu_raster(p);
	}
}

void PPU::EndVBlank()
{
	if(_cycle == 340) {
		_frameCount++;
		//std::cout << _frameCount << std::endl;
	}
}

void PPU::Exec()
{
	uint64_t equivalentCycleCount = CPU::GetCycleCount() * 3;
	while(_cycleCount < equivalentCycleCount) {
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

		_cycleCount++;
	}
}