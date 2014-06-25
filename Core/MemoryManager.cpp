#include "stdafx.h"
#include "MemoryManager.h"

MemoryManager::MemoryManager(shared_ptr<BaseMapper> mapper)
{
	_mapper = mapper;

	_internalRAM = new uint8_t[InternalRAMSize];
	_SRAM = new uint8_t[SRAMSize];
	_videoRAM = new uint8_t[VRAMSize];
	_expansionRAM = new uint8_t[0x2000];

	_ramReadHandlers = new IMemoryHandler*[RAMSize];
	_ramWriteHandlers = new IMemoryHandler*[RAMSize];
	_vramReadHandlers = new IMemoryHandler*[VRAMSize];
	_vramWriteHandlers = new IMemoryHandler*[VRAMSize];
	
	memset(_internalRAM, 0, InternalRAMSize);
	memset(_SRAM, 0, SRAMSize);
	memset(_videoRAM, 0, VRAMSize);
	memset(_expansionRAM, 0, 0x2000);

	memset(_ramReadHandlers, 0, RAMSize * sizeof(IMemoryHandler*));
	memset(_ramWriteHandlers, 0, RAMSize * sizeof(IMemoryHandler*));

	memset(_vramReadHandlers, 0, VRAMSize * sizeof(IMemoryHandler*));
	memset(_vramWriteHandlers, 0, VRAMSize * sizeof(IMemoryHandler*));

	//Load battery data if present
	_mapper->LoadBattery(_SRAM);
}

MemoryManager::~MemoryManager()
{
	delete[] _internalRAM;
	delete[] _videoRAM;
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
	if(addr <= 0x1FFF) {
		return _internalRAM[addr & 0x07FF];
	} else if(addr <= 0x401F) {
		return ReadRegister(addr);
	} else if(addr <= 0x5FFF) {
		return _expansionRAM[addr & 0x1FFF];
	} else if(addr <= 0x7FFF) {
		return _SRAM[addr & 0x1FFF];
	} else {
		return ReadRegister(addr);
	}
}

void MemoryManager::Write(uint16_t addr, uint8_t value)
{
	if(addr <= 0x1FFF) {
		_internalRAM[addr & 0x07FF] = value;
	} else if(addr <= 0x401F) {
		WriteRegister(addr, value);
	} else if(addr <= 0x5FFF) {
		_expansionRAM[addr & 0x1FFF] = value;
	} else if(addr <= 0x7FFF) {
		if(_SRAM[addr & 0x1FFF] != value) {
			_SRAM[addr & 0x1FFF] = value;
			_mapper->SaveBattery(_SRAM);
		}
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
	if(addr <= 0x1FFF) {
		return ReadMappedVRAM(addr & 0x1FFF);
	} else {
		if(addr >= 0x3000) {
			addr -= 0x1000;
		}
		return _videoRAM[addr & 0x3FFF];
	}
}

void MemoryManager::WriteVRAM(uint16_t addr, uint8_t value)
{
	addr = addr & 0x3FFF;
	if(addr <= 0x1FFF) {
		WriteMappedVRAM(addr, value);
	} else {
		if(addr >= 0x3000) {
			addr -= 0x1000;
		}

		_videoRAM[addr] = value;

		switch(_mapper->GetMirroringType()) {
			case MirroringType::Vertical:
				if(addr >= 0x2000 && addr < 0x2400) {
					_videoRAM[addr + 0x800] = value;
				} else if(addr >= 0x2400 && addr < 0x2800) {
					_videoRAM[addr + 0x800] = value;
				} else if(addr >= 0x2800 && addr < 0x2C00) {
					_videoRAM[addr - 0x800] = value;
				} else if(addr >= 0x2C00 && addr < 0x3000) {
					_videoRAM[addr - 0x800] = value;
				}
				break;

			case MirroringType::Horizontal:
				if(addr >= 0x2000 && addr < 0x2400) {
					_videoRAM[addr + 0x400] = value;
				} else if(addr >= 0x2400 && addr < 0x2800) {
					_videoRAM[addr - 0x400] = value;
				} else if(addr >= 0x2800 && addr < 0x2C00) {
					_videoRAM[addr + 0x400] = value;
				} else if(addr >= 0x2C00 && addr < 0x3000) {
					_videoRAM[addr - 0x400] = value;
				}
				break;

			case MirroringType::ScreenAOnly:
			case MirroringType::ScreenBOnly:
				if(addr >= 0x2000 && addr < 0x2400) {
					_videoRAM[addr + 0x400] = value;
					_videoRAM[addr + 0x800] = value;
					_videoRAM[addr + 0xC00] = value;
				} else if(addr >= 0x2400 && addr < 0x2800) {
					_videoRAM[addr - 0x400] = value;
					_videoRAM[addr + 0x400] = value;
					_videoRAM[addr + 0x800] = value;
				} else if(addr >= 0x2800 && addr < 0x2C00) {
					_videoRAM[addr + 0x400] = value;
					_videoRAM[addr - 0x400] = value;
					_videoRAM[addr - 0x800] = value;
				} else if(addr >= 0x2C00 && addr < 0x3000) {
					_videoRAM[addr - 0x400] = value;
					_videoRAM[addr - 0x800] = value;
					_videoRAM[addr - 0xC00] = value;
				}
				break;

			default:
				throw exception("Not implemented yet");
		}
	}
}