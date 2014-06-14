#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"

class MemoryManager
{
	private:
		const int InternalRAMSize = 0x800;
		const int SRAMSize = 0x800;

		uint8_t *_internalRAM;
		uint8_t *_SRAM;

		vector<IMemoryHandler*> _registerHandlers;
			
		inline uint8_t ReadRegister(uint16_t addr);
		inline void WriteRegister(uint16_t addr, uint8_t value);

	public:
		MemoryManager();
		~MemoryManager();

		void RegisterIODevice(IMemoryHandler *handler);

		uint8_t Read(uint16_t addr);
		void Write(uint16_t addr, uint8_t value);
		uint16_t ReadWord(uint16_t addr);
};

