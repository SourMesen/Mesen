#include "stdafx.h"
#include "MemoryManager.h"
#include "PPU.h"

MemoryManager::MemoryManager(shared_ptr<BaseMapper> mapper)
{
	_mapper = mapper;

	_hasExpansionRAM = false;

	_internalRAM = new uint8_t[InternalRAMSize];
	_SRAM = new uint8_t[SRAMSize];
	for(int i = 0; i < 4; i++) {
		_nametableRAM[i] = new uint8_t[NameTableScreenSize];
		memset(_nametableRAM[i], 0, NameTableScreenSize);
	}
	_expansionRAM = new uint8_t[ExpansionRAMSize];

	_ramReadHandlers = new IMemoryHandler*[RAMSize];
	_ramWriteHandlers = new IMemoryHandler*[RAMSize];
	_vramReadHandlers = new IMemoryHandler*[VRAMSize];
	_vramWriteHandlers = new IMemoryHandler*[VRAMSize];
	
	memset(_internalRAM, 0, InternalRAMSize);
	memset(_SRAM, 0, SRAMSize);
	memset(_expansionRAM, 0, ExpansionRAMSize);

	memset(_ramReadHandlers, 0, RAMSize * sizeof(IMemoryHandler*));
	memset(_ramWriteHandlers, 0, RAMSize * sizeof(IMemoryHandler*));

	memset(_vramReadHandlers, 0, VRAMSize * sizeof(IMemoryHandler*));
	memset(_vramWriteHandlers, 0, VRAMSize * sizeof(IMemoryHandler*));

	//Load battery data if present
	if(_mapper->HasBattery()) {
		_mapper->LoadBattery(_SRAM);
	}
}

MemoryManager::~MemoryManager()
{
	if(_mapper->HasBattery()) {
		_mapper->SaveBattery(_SRAM);
	}
	delete[] _internalRAM;
	for(int i = 0; i < 4; i++) {
		delete[] _nametableRAM[i];
	}
	delete[] _SRAM;
	delete[] _expansionRAM;

	delete[] _ramReadHandlers;
	delete[] _ramWriteHandlers;
	delete[] _vramReadHandlers;
	delete[] _vramWriteHandlers;
}

uint8_t MemoryManager::ReadRegister(uint16_t addr)
{
	if(_ramReadHandlers[addr]) {
		return _ramReadHandlers[addr]->ReadRAM(addr);
	} else {
		return 0;
	}
}

void MemoryManager::WriteRegister(uint16_t addr, uint8_t value)
{
	if(_ramWriteHandlers[addr]) {
		_ramWriteHandlers[addr]->WriteRAM(addr, value);
	}
}

uint8_t MemoryManager::ReadMappedVRAM(uint16_t addr)
{
	if(_vramReadHandlers[addr]) {
		return _vramReadHandlers[addr]->ReadVRAM(addr);
	} else {
		return 0;
	}
}

void MemoryManager::WriteMappedVRAM(uint16_t addr, uint8_t value)
{
	if(_vramWriteHandlers[addr]) {
		_vramWriteHandlers[addr]->WriteVRAM(addr, value);
	}
}

void MemoryManager::InitializeMemoryHandlers(IMemoryHandler** memoryHandlers, IMemoryHandler* handler, vector<uint16_t> *addresses)
{
	for(uint16_t address : *addresses) {
		if(memoryHandlers[address] != nullptr) {
			throw exception("Not supported");
		}
		memoryHandlers[address] = handler;
	}
}

void MemoryManager::RegisterIODevice(IMemoryHandler *handler)
{
	MemoryRanges ranges;
	handler->GetMemoryRanges(ranges);

	InitializeMemoryHandlers(_ramReadHandlers, handler, ranges.GetRAMReadAddresses());
	InitializeMemoryHandlers(_ramWriteHandlers, handler, ranges.GetRAMWriteAddresses());
	InitializeMemoryHandlers(_vramReadHandlers, handler, ranges.GetVRAMReadAddresses());
	InitializeMemoryHandlers(_vramWriteHandlers, handler, ranges.GetVRAMWriteAddresses());
}

uint8_t MemoryManager::Read(uint16_t addr)
{
	uint8_t value;
	PPU::ExecStatic(3);
	if(addr <= 0x1FFF) {
		value = _internalRAM[addr & 0x07FF];
	} else if(addr <= 0x401F) {
		value = ReadRegister(addr);
	} else if(addr <= 0x5FFF) {
		value = _expansionRAM[addr & 0x1FFF];
	} else if(addr <= 0x7FFF) {
		value = _SRAM[addr & 0x1FFF];
	} else {
		value = ReadRegister(addr);
	}
	return value;
}

void MemoryManager::Write(uint16_t addr, uint8_t value)
{
	PPU::ExecStatic(3);
	if(addr <= 0x1FFF) {
		_internalRAM[addr & 0x07FF] = value;
	} else if(addr <= 0x401F) {
		WriteRegister(addr, value);
	} else if(addr <= 0x5FFF) {
		_hasExpansionRAM = true;
		_expansionRAM[addr & 0x1FFF] = value;
	} else if(addr <= 0x7FFF) {
		_SRAM[addr & 0x1FFF] = value;
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

uint8_t MemoryManager::ReadVRAM(uint16_t addr)
{
	_mapper->NotifyVRAMAddressChange(addr);
	if(addr <= 0x1FFF) {
		return ReadMappedVRAM(addr & 0x1FFF);
	} else {
		switch(_mapper->GetMirroringType()) {
			case MirroringType::Vertical:
				return _nametableRAM[(addr&0x400)>>10][addr & 0x3FF];
			
			case MirroringType::Horizontal:
				return _nametableRAM[(addr&0x800)>>11][addr & 0x3FF];

			case MirroringType::FourScreens:
			default:
				return _nametableRAM[(addr&0xC00)>>10][addr & 0x3FF];

			case MirroringType::ScreenAOnly:
				return _nametableRAM[0][addr & 0x3FF];

			case MirroringType::ScreenBOnly:
				return _nametableRAM[1][addr & 0x3FF];
		}
		
	}
}

void MemoryManager::WriteVRAM(uint16_t addr, uint8_t value)
{
	_mapper->NotifyVRAMAddressChange(addr);

	addr = addr & 0x3FFF;
	if(addr <= 0x1FFF) {
		WriteMappedVRAM(addr, value);
	} else {
		switch(_mapper->GetMirroringType()) {
			case MirroringType::Vertical:
				_nametableRAM[(addr&0x400)>>10][addr & 0x3FF] = value;
				break;

			case MirroringType::Horizontal:
				_nametableRAM[(addr&0x800)>>11][addr & 0x3FF] = value;
				break;

			case MirroringType::ScreenAOnly:  //Always write to 0x2000
				_nametableRAM[0][addr & 0x3FF] = value;
				break;

			case MirroringType::ScreenBOnly:  //Always write to 0x2400
				_nametableRAM[1][addr & 0x3FF] = value;
				break;

			case MirroringType::FourScreens:
				_nametableRAM[(addr&0xC00)>>10][addr & 0x3FF] = value;
				break;

			default:
				throw exception("Not implemented yet");
		}
	}
}

void MemoryManager::StreamState(bool saving)
{
	StreamArray<uint8_t>(_internalRAM, MemoryManager::InternalRAMSize);
	Stream<bool>(_hasExpansionRAM);
	if(_hasExpansionRAM) {
		StreamArray<uint8_t>(_expansionRAM, MemoryManager::ExpansionRAMSize);
	}
	StreamArray<uint8_t>(_SRAM, MemoryManager::SRAMSize);
	for(int i = 0; i < 4; i++) {
		StreamArray<uint8_t>(_nametableRAM[i], MemoryManager::NameTableScreenSize);
	}
}