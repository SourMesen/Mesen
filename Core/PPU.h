#include "stdafx.h"

enum PPURegisters
{
	Control = 0x00,
	Control2 = 0x01,
	Status = 0x02,
	SpriteAddr = 0x03,
	SpriteData = 0x04,
	ScrollOffsets = 0x05,
	VideoMemoryAddr = 0x06,
	VideoMemoryData = 0x07
};

struct PPUState
{
	uint16_t Control;
	uint8_t Status;
	uint8_t SpriteRamAddr;
	uint16_t VideoRamAddr;
};

class PPU : public IMemoryHandler
{
private:
	PPUState _state;
	uint8_t _spriteRAM[256];
	uint8_t _videoRAM[16*1024];

	PPURegisters GetRegisterID(uint16_t addr) {
		return (PPURegisters)(addr & 0x07);
	}

public:
	PPU() {
		_state = {};
		_state.Status |= 0xFF;
	}

	uint8_t MemoryRead(uint16_t addr)	{
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
	}

	void MemoryWrite(uint16_t addr, uint8_t value) {

	}
};