#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"
#include "Snapshotable.h"

class BaseMapper;

class MemoryManager: public Snapshotable
{
	private:
		const int RAMSize = 0x10000;
		const int InternalRAMSize = 0x800;
		const int VRAMSize = 0x4000;
		const int NameTableScreenSize = 0x400;

		shared_ptr<BaseMapper> _mapper;

		uint8_t *_internalRAM;
		uint8_t *_nametableRAM[2];

		IMemoryHandler** _ramReadHandlers;
		IMemoryHandler** _ramWriteHandlers;
			
		uint8_t ReadRegister(uint16_t addr);
		void WriteRegister(uint16_t addr, uint8_t value);

	protected:
		void StreamState(bool saving);

	public:
		MemoryManager(shared_ptr<BaseMapper> mapper);
		~MemoryManager();

		void Reset(bool softReset);

		void InitializeMemoryHandlers(IMemoryHandler** memoryHandlers, IMemoryHandler* handler, vector<uint16_t> *addresses);
		void RegisterIODevice(IMemoryHandler *handler);

		uint8_t DebugRead(uint16_t addr);
		uint8_t* GetInternalRAM();

		uint8_t Read(uint16_t addr, bool forExecution = false);
		void Write(uint16_t addr, uint8_t value);

		void ProcessVramAccess(uint16_t &addr);
		uint8_t ReadVRAM(uint16_t addr);
		void WriteVRAM(uint16_t addr, uint8_t value);
};

