#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"
#include "Snapshotable.h"
#include "OpenBusHandler.h"
#include "InternalRamHandler.h"

class BaseMapper;
class Console;

class MemoryManager : public Snapshotable
{
	private:
		static constexpr int RAMSize = 0x10000;
		static constexpr int VRAMSize = 0x4000;
		static constexpr int NameTableScreenSize = 0x400;
		
		shared_ptr<Console> _console;
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

		MemoryManager(shared_ptr<Console> console);
		~MemoryManager();

		void SetMapper(shared_ptr<BaseMapper> mapper);
		
		void Reset(bool softReset);
		void RegisterIODevice(IMemoryHandler *handler);
		void UnregisterIODevice(IMemoryHandler *handler);

		uint8_t DebugRead(uint16_t addr, bool disableSideEffects = true);
		uint16_t DebugReadWord(uint16_t addr);
		void DebugWrite(uint16_t addr, uint8_t value, bool disableSideEffects = true);

		uint8_t* GetInternalRAM();

		uint8_t Read(uint16_t addr, MemoryOperationType operationType = MemoryOperationType::Read);
		void Write(uint16_t addr, uint8_t value, MemoryOperationType operationType);

		uint32_t ToAbsolutePrgAddress(uint16_t ramAddr);

		uint8_t GetOpenBus(uint8_t mask = 0xFF);
};

