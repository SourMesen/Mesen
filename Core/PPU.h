#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"

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

		PPURegisters GetRegisterID(uint16_t addr)
		{
			return (PPURegisters)(addr & 0x07);
		}

	public:
		PPU();
		
		std::array<int, 2> GetIOAddresses() 
		{ 
			return std::array<int, 2> {{ 0x2000, 0x3FFF }};
		}

		uint8_t MemoryRead(uint16_t addr);
		void MemoryWrite(uint16_t addr, uint8_t value);
};