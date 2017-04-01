#include "stdafx.h"
#include "MemoryManager.h"
#include "BaseMapper.h"
#include "Debugger.h"
#include "CheatManager.h"

//Used for open bus
uint8_t MemoryManager::_lastReadValue = 0;

MemoryManager::MemoryManager(shared_ptr<BaseMapper> mapper)
{
	_mapper = mapper;
	_lastReadValue = 0;

	_internalRAM = new uint8_t[InternalRAMSize];
	for(int i = 0; i < 2; i++) {
		_nametableRAM[i] = new uint8_t[NameTableScreenSize];
		BaseMapper::InitializeRam(_nametableRAM[i], NameTableScreenSize);
	}

	_mapper->SetDefaultNametables(_nametableRAM[0], _nametableRAM[1]);

	_ramReadHandlers = new IMemoryHandler*[RAMSize];
	_ramWriteHandlers = new IMemoryHandler*[RAMSize];

	memset(_ramReadHandlers, 0, RAMSize * sizeof(IMemoryHandler*));
	memset(_ramWriteHandlers, 0, RAMSize * sizeof(IMemoryHandler*));
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

void MemoryManager::Reset(bool softReset)
{
	if(!softReset) {
		BaseMapper::InitializeRam(_internalRAM, InternalRAMSize);
	}

	_mapper->Reset(softReset);
}

uint8_t MemoryManager::ReadRegister(uint16_t addr)
{
	if(_ramReadHandlers[addr]) {
		return _ramReadHandlers[addr]->ReadRAM(addr);
	} else {
		return GetOpenBus();
	}
}

void MemoryManager::WriteRegister(uint16_t addr, uint8_t value)
{
	if(_ramWriteHandlers[addr]) {
		_ramWriteHandlers[addr]->WriteRAM(addr, value);
	}
}

void MemoryManager::InitializeMemoryHandlers(IMemoryHandler** memoryHandlers, IMemoryHandler* handler, vector<uint16_t> *addresses, bool allowOverride)
{
	for(uint16_t address : *addresses) {
		if(!allowOverride && memoryHandlers[address] != nullptr && memoryHandlers[address] != handler) {
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
		_ramReadHandlers[address] = nullptr;
	}

	for(uint16_t address : *ranges.GetRAMWriteAddresses()) {
		_ramWriteHandlers[address] = nullptr;
	}
}

uint8_t* MemoryManager::GetInternalRAM()
{
	return _internalRAM;
}

uint8_t MemoryManager::DebugRead(uint16_t addr, bool disableRegisterReads)
{
	uint8_t value = 0x00;
	if(addr <= 0x1FFF) {
		value = _internalRAM[addr & 0x07FF];
	} else if(!disableRegisterReads || addr > 0x4017) {
		value = ReadRegister(addr);
	}

	CheatManager::ApplyRamCodes(addr, value);

	return value;
}

uint16_t MemoryManager::DebugReadWord(uint16_t addr)
{
	return DebugRead(addr) | (DebugRead(addr + 1) << 8);
}

void MemoryManager::ProcessCpuClock()
{
	_mapper->ProcessCpuClock();
}

uint8_t MemoryManager::Read(uint16_t addr, MemoryOperationType operationType)
{
	uint8_t value;
	if(addr <= 0x1FFF) {
		value = _internalRAM[addr & 0x07FF];
	} else {
		value = ReadRegister(addr);
	}

	CheatManager::ApplyRamCodes(addr, value);

	Debugger::ProcessRamOperation(operationType, addr, value);

	_lastReadValue = value;

	return value;
}

void MemoryManager::Write(uint16_t addr, uint8_t value)
{
	if(Debugger::ProcessRamOperation(MemoryOperationType::Write, addr, value)) {
		if(addr <= 0x1FFF) {
			_internalRAM[addr & 0x07FF] = value;
		} else {
			WriteRegister(addr, value);
		}
	}
}

void MemoryManager::DebugWrite(uint16_t addr, uint8_t value)
{
	if(addr <= 0x1FFF) {
		_internalRAM[addr & 0x07FF] = value;
	} else {
		WriteRegister(addr, value);
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
	return _lastReadValue & mask;
}