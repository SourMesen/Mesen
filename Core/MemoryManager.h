#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"
#include "Snapshotable.h"

class BaseMapper;

class MemoryManager: public Snapshotable
{
	private:
		static const int RAMSize = 0x10000;
		static const int InternalRAMSize = 0x800;
		static const int VRAMSize = 0x4000;
		static const int NameTableScreenSize = 0x400;

		static uint8_t _lastReadValue;

		shared_ptr<BaseMapper> _mapper;

		uint8_t *_internalRAM;
		uint8_t *_nametableRAM[2];

		IMemoryHandler** _ramReadHandlers;
		IMemoryHandler** _ramWriteHandlers;
			
		uint8_t ReadRegister(uint16_t addr);
		void WriteRegister(uint16_t addr, uint8_t value);
		void InitializeMemoryHandlers(IMemoryHandler** memoryHandlers, IMemoryHandler* handler, vector<uint16_t> *addresses, bool allowOverride);

	protected:
		void StreamState(bool saving);

	public:
		MemoryManager(shared_ptr<BaseMapper> mapper);
		~MemoryManager();

		void Reset(bool softReset);
		void RegisterIODevice(IMemoryHandler *handler);

		uint8_t DebugRead(uint16_t addr);
		uint16_t DebugReadWord(uint16_t addr);
		uint8_t DebugReadVRAM(uint16_t addr);
		void DebugWrite(uint16_t addr, uint8_t value);

		uint8_t* GetInternalRAM();

		void ProcessCpuClock();

		uint8_t Read(uint16_t addr, MemoryOperationType operationType = MemoryOperationType::Read);
		void Write(uint16_t addr, uint8_t value);

		void ProcessVramAccess(uint16_t &addr);
		uint8_t ReadVRAM(uint16_t addr, MemoryOperationType operationType = MemoryOperationType::PpuRenderingRead);
		void WriteVRAM(uint16_t addr, uint8_t value);

		uint32_t ToAbsolutePrgAddress(uint16_t ramAddr);
		uint32_t ToAbsoluteChrAddress(uint16_t vramAddr);

		static uint8_t GetOpenBus(uint8_t mask = 0xFF);
		static void InitializeRam(uint8_t* data, uint32_t length);
};

