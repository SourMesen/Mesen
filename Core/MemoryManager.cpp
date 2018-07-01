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

	for(int i = 0; i < 2; i++) {
		_nametableRAM[i] = new uint8_t[NameTableScreenSize];
		BaseMapper::InitializeRam(_nametableRAM[i], NameTableScreenSize);
	}

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
	for(int i = 0; i < 2; i++) {
		delete[] _nametableRAM[i];
	}

	delete[] _ramReadHandlers;
	delete[] _ramWriteHandlers;
}

void MemoryManager::SetMapper(shared_ptr<BaseMapper> mapper)
{
	_mapper = mapper;
	_mapper->SetDefaultNametables(_nametableRAM[0], _nametableRAM[1]);
}

void MemoryManager::Reset(bool softReset)
{
	if(!softReset) {
		BaseMapper::InitializeRam(_internalRAM, InternalRAMSize);
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
				if(handler == _mapper.get()) {
					//Only allow reads from prg/chr ram/rom (e.g not ppu, apu, mapper registers, etc.)
					value = ((BaseMapper*)handler)->DebugReadRAM(addr);
				}
			} else {
				value = handler->ReadRAM(addr);
			}
		} else {
			//Fake open bus
			value = addr >> 8;
		}
	}

	_console->GetCheatManager()->ApplyRamCodes(addr, value);

	return value;
}

uint16_t MemoryManager::DebugReadWord(uint16_t addr)
{
	return DebugRead(addr) | (DebugRead(addr + 1) << 8);
}

uint8_t MemoryManager::Read(uint16_t addr, MemoryOperationType operationType)
{
	uint8_t value = _ramReadHandlers[addr]->ReadRAM(addr);
	_console->GetCheatManager()->ApplyRamCodes(addr, value);
	_console->DebugProcessRamOperation(operationType, addr, value);

	_openBusHandler.SetOpenBus(value);

	return value;
}

void MemoryManager::Write(uint16_t addr, uint8_t value)
{
	if(_console->DebugProcessRamOperation(MemoryOperationType::Write, addr, value)) {
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
	ArrayInfo<uint8_t> nameTable0Ram = { _nametableRAM[0], MemoryManager::NameTableScreenSize };
	ArrayInfo<uint8_t> nameTable1Ram = { _nametableRAM[1], MemoryManager::NameTableScreenSize };
	Stream(internalRam, nameTable0Ram, nameTable1Ram);
}

uint8_t MemoryManager::GetOpenBus(uint8_t mask)
{
	return _openBusHandler.GetOpenBus() & mask;
}
