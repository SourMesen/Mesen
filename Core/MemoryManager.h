#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"

class MemoryManager
{
	private:
		const int InternalRAMSize = 0x800;
		const int SRAMSize = 0x2000;
		const int VRAMSize = 0x4000;

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
		MemoryManager();
		~MemoryManager();

		void RegisterIODevice(IMemoryHandler *handler);

		uint8_t Read(uint16_t addr);
		uint16_t ReadWord(uint16_t addr);
		void Write(uint16_t addr, uint8_t value);

		uint8_t ReadVRAM(uint16_t addr);
		void WriteVRAM(uint16_t addr, uint8_t value);


		char* GetTestResult()
		{
			char *buffer = new char[0x2000];
			for(int i = 0; i < 0x1000; i++) {
				buffer[i] = Read(0x6004 + i);
			}
			return buffer;
		}
};

