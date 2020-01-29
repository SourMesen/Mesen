#include "stdafx.h"
#include "MemoryManager.h"
#include "BaseMapper.h"
#include "Debugger.h"
#include "CheatManager.h"
#include "Console.h"

MemoryManager::MemoryManager(shared_ptr<Console> console)
{
	_console = console;
	_internalRAM = new uint8_t[InternalRAMSize];
	_internalRamHandler.SetInternalRam(_internalRAM);

	_ramReadHandlers = new IMemoryHandler*[RAMSize];
	_ramWriteHandlers = new IMemoryHandler*[RAMSize];

	for(int i = 0; i < RAMSize; i++) {
		_ramReadHandlers[i] = &_openBusHandler;
		_ramWriteHandlers[i] = &_openBusHandler;
	}

	RegisterIODevice(&_internalRamHandler);	
}

MemoryManager::~MemoryManager()
{
	delete[] _internalRAM;

	delete[] _ramReadHandlers;
	delete[] _ramWriteHandlers;
}

void MemoryManager::SetMapper(shared_ptr<BaseMapper> mapper)
{
	_mapper = mapper;
}

void MemoryManager::Reset(bool softReset)
{
	if(!softReset) {
		_console->InitializeRam(_internalRAM, InternalRAMSize);
	}

	_mapper->Reset(softReset);
}

void MemoryManager::InitializeMemoryHandlers(IMemoryHandler** memoryHandlers, IMemoryHandler* handler, vector<uint16_t> *addresses, bool allowOverride)
{
	for(uint16_t address : *addresses) {
		if(!allowOverride && memoryHandlers[address] != &_openBusHandler && memoryHandlers[address] != handler) {
			throw std::runtime_error("Not supported");
		}
		memoryHandlers[address] = handler;
	}
}

void MemoryManager::RegisterIODevice(IMemoryHandler *handler)
{
	MemoryRanges ranges;
	handler->GetMemoryRanges(ranges);

	InitializeMemoryHandlers(_ramReadHandlers, handler, ranges.GetRAMReadAddresses(), ranges.GetAllowOverride());
	InitializeMemoryHandlers(_ramWriteHandlers, handler, ranges.GetRAMWriteAddresses(), ranges.GetAllowOverride());
}

void MemoryManager::RegisterWriteHandler(IMemoryHandler* handler, uint32_t start, uint32_t end)
{
	for(uint32_t i = start; i < end; i++) {
		_ramWriteHandlers[i] = handler;
	}
}

void MemoryManager::UnregisterIODevice(IMemoryHandler *handler)
{
	MemoryRanges ranges;
	handler->GetMemoryRanges(ranges);

	for(uint16_t address : *ranges.GetRAMReadAddresses()) {
		_ramReadHandlers[address] = &_openBusHandler;
	}

	for(uint16_t address : *ranges.GetRAMWriteAddresses()) {
		_ramWriteHandlers[address] = &_openBusHandler;
	}
}

uint8_t* MemoryManager::GetInternalRAM()
{
	return _internalRAM;
}

uint8_t MemoryManager::DebugRead(uint16_t addr, bool disableSideEffects)
{
	uint8_t value = 0x00;
	if(addr <= 0x1FFF) {
		value = _ramReadHandlers[addr]->ReadRAM(addr);
	} else {
		IMemoryHandler* handler = _ramReadHandlers[addr];
		if(handler) {
			if(disableSideEffects) {
				value = handler->PeekRAM(addr);
			} else {
				value = handler->ReadRAM(addr);
			}
		} else {
			//Fake open bus
			value = addr >> 8;
		}
	}

	_console->GetCheatManager()->ApplyCodes(addr, value);

	return value;
}

uint16_t MemoryManager::DebugReadWord(uint16_t addr)
{
	return DebugRead(addr) | (DebugRead(addr + 1) << 8);
}

uint8_t MemoryManager::Read(uint16_t addr, MemoryOperationType operationType)
{
	uint8_t value = _ramReadHandlers[addr]->ReadRAM(addr);
	_console->GetCheatManager()->ApplyCodes(addr, value);
	_console->DebugProcessRamOperation(operationType, addr, value);

	_openBusHandler.SetOpenBus(value);

	return value;
}

void MemoryManager::Write(uint16_t addr, uint8_t value, MemoryOperationType operationType)
{
	if(_console->DebugProcessRamOperation(operationType, addr, value)) {
		_ramWriteHandlers[addr]->WriteRAM(addr, value);
	}
}

void MemoryManager::DebugWrite(uint16_t addr, uint8_t value, bool disableSideEffects)
{
	if(addr <= 0x1FFF) {
		_ramWriteHandlers[addr]->WriteRAM(addr, value);
	} else {
		IMemoryHandler* handler = _ramReadHandlers[addr];
		if(handler) {
			if(disableSideEffects) {
				if(handler == _mapper.get()) {
					//Only allow writes to prg/chr ram/rom (e.g not ppu, apu, mapper registers, etc.)
					((BaseMapper*)handler)->DebugWriteRAM(addr, value);
				}
			} else {
				handler->WriteRAM(addr, value);
			}
		}
	}
}

uint32_t MemoryManager::ToAbsolutePrgAddress(uint16_t ramAddr)
{
	return _mapper->ToAbsoluteAddress(ramAddr);
}

void MemoryManager::StreamState(bool saving)
{
	ArrayInfo<uint8_t> internalRam = { _internalRAM, MemoryManager::InternalRAMSize };
	Stream(internalRam);
}

uint8_t MemoryManager::GetOpenBus(uint8_t mask)
{
	return _openBusHandler.GetOpenBus() & mask;
}
