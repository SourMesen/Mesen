#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"
#include "Snapshotable.h"
#include "OpenBusHandler.h"
#include "InternalRamHandler.h"
#include "IMemoryManager.h"

class BaseMapper;

class MemoryManager: public IMemoryManager, public Snapshotable
{
	private:
		static const int RAMSize = 0x10000;
		static const int VRAMSize = 0x4000;
		static const int NameTableScreenSize = 0x400;

		shared_ptr<BaseMapper> _mapper;

		uint8_t *_internalRAM;
		uint8_t *_nametableRAM[2];

		OpenBusHandler _openBusHandler;
		InternalRamHandler<0x7FF> _internalRamHandler;
		IMemoryHandler** _ramReadHandlers;
		IMemoryHandler** _ramWriteHandlers;

		void InitializeMemoryHandlers(IMemoryHandler** memoryHandlers, IMemoryHandler* handler, vector<uint16_t> *addresses, bool allowOverride);

	protected:
		void StreamState(bool saving) override;

	public:
		static const int InternalRAMSize = 0x800;

		MemoryManager(shared_ptr<BaseMapper> mapper);
		~MemoryManager();

		void Reset(bool softReset);
		void RegisterIODevice(IMemoryHandler *handler) override;
		void UnregisterIODevice(IMemoryHandler *handler) override;

		uint8_t DebugRead(uint16_t addr, bool disableSideEffects = true);
		uint16_t DebugReadWord(uint16_t addr);
		void DebugWrite(uint16_t addr, uint8_t value, bool disableSideEffects = true);

		uint8_t* GetInternalRAM();

		void ProcessCpuClock();

		uint8_t Read(uint16_t addr, MemoryOperationType operationType = MemoryOperationType::Read);
		void Write(uint16_t addr, uint8_t value);

		uint32_t ToAbsolutePrgAddress(uint16_t ramAddr);

		static uint8_t GetOpenBus(uint8_t mask = 0xFF);
};

