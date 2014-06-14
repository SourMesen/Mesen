#include "stdafx.h"
#include "MemoryManager.h"

uint8_t MemoryManager::ReadRegister(uint16_t addr)
{
	if(_registerHandlers[addr]) {
		return _registerHandlers[addr]->MemoryRead(addr);
	} else {
		return 0;
	}
}

void MemoryManager::WriteRegister(uint16_t addr, uint8_t value)
{
	if(_registerHandlers[addr]) {
		_registerHandlers[addr]->MemoryWrite(addr, value);
	}
}

MemoryManager::MemoryManager()
{
	_internalRAM = new uint8_t[InternalRAMSize];
	_SRAM = new uint8_t[SRAMSize];
	ZeroMemory(_SRAM, SRAMSize);
	ZeroMemory(_internalRAM, InternalRAMSize);

	for(int i = 0; i < 0xFFFF; i++) {
		_registerHandlers.push_back(nullptr);
	}
}

MemoryManager::~MemoryManager()
{
	delete[] _internalRAM;
	delete[] _SRAM;
}

void MemoryManager::RegisterIODevice(IMemoryHandler *handler)
{
	std::array<int, 2> addresses = handler->GetIOAddresses();
	for(int i = addresses[0]; i < addresses[1]; i++) {
		_registerHandlers[i] = handler;
	}
}

uint8_t MemoryManager::Read(uint16_t addr)
{
	if(addr <= 0x1FFF) {
		return _internalRAM[addr & 0x07FF];
	} else if(addr <= 0x401F) {
		return ReadRegister(addr);
	} else if(addr <= 0x5FFF) {
		throw std::exception("Not implemented yet");
		//return ReadExpansionROM();
	} else if(addr <= 0x7FFF) {
		return _SRAM[addr];
	} else {
		return ReadRegister(addr);
	}
}

void MemoryManager::Write(uint16_t addr, uint8_t value)
{
	if(addr <= 0x1FFFF) {
		_internalRAM[addr & 0x07FF] = value;
	} else if(addr <= 0x401F) {
		WriteRegister(addr, value);
	} else if(addr <= 0x5FFF) {
		throw std::exception("Not implemented yet");
		//return ReadExpansionROM();
	} else if(addr <= 0x7FFF) {
		_SRAM[addr] = value;
	} else {
		WriteRegister(addr, value);
	}
}

uint16_t MemoryManager::ReadWord(uint16_t addr) 
{
	uint8_t lo = Read(addr);
	uint8_t hi = Read(addr+1);
	return lo | hi << 8;
}
	