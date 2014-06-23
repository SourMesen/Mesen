#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"
#include "ROMLoader.h"

class MemoryManager
{
	private:
		const int InternalRAMSize = 0x800;
		const int SRAMSize = 0x2000;
		const int VRAMSize = 0x4000;

		NESHeader _header;
		MirroringType _mirroringType;

		uint8_t *_internalRAM;
		uint8_t *_expansionRAM;
		uint8_t *_SRAM;
		uint8_t *_videoRAM;

		vector<IMemoryHandler*> _ramHandlers;
		vector<IMemoryHandler*> _vramHandlers;
			
		uint8_t ReadRegister(uint16_t addr);
		void WriteRegister(uint16_t addr, uint8_t value);

		uint8_t ReadMappedVRAM(uint16_t addr);
		void WriteMappedVRAM(uint16_t addr, uint8_t value);

	public:
		MemoryManager(NESHeader header);
		~MemoryManager();

		void RegisterIODevice(IMemoryHandler *handler);

		uint8_t Read(uint16_t addr);
		uint16_t ReadWord(uint16_t addr);
		void Write(uint16_t addr, uint8_t value);

		uint8_t ReadVRAM(uint16_t addr);
		void WriteVRAM(uint16_t addr, uint8_t value);
};

