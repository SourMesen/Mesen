#include "stdafx.h"
#include "PPU.h"

PPU::PPU() 
{
	_state = {};
	_state.Status |= 0xFF;
}

uint8_t PPU::MemoryRead(uint16_t addr)
{
	switch(GetRegisterID(addr)) {
		case PPURegisters::Control:
			return (uint8_t)_state.Control;
		case PPURegisters::Control2:
			return (uint8_t)(_state.Control >> 8);
		case PPURegisters::Status:
			return _state.Status;
		case PPURegisters::SpriteData:
			return _spriteRAM[_state.SpriteRamAddr];
		case PPURegisters::VideoMemoryData:
			return _videoRAM[_state.VideoRamAddr];
	}
	return 0;
}

void PPU::MemoryWrite(uint16_t addr, uint8_t value)
{

}