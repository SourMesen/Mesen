#include "stdafx.h"
#include "MemoryManager.h"
#include "BaseMapper.h"
#include "Debugger.h"
#include "CheatManager.h"

MemoryManager::MemoryManager(shared_ptr<BaseMapper> mapper)
{
	_mapper = mapper;

	_internalRAM = new uint8_t[InternalRAMSize];
	for(int i = 0; i < 4; i++) {
		_nametableRAM[i] = new uint8_t[NameTableScreenSize];
		memset(_nametableRAM[i], 0, NameTableScreenSize);
	}

	_ramReadHandlers = new IMemoryHandler*[RAMSize];
	_ramWriteHandlers = new IMemoryHandler*[RAMSize];
	_vramReadHandlers = new IMemoryHandler*[VRAMSize];
	_vramWriteHandlers = new IMemoryHandler*[VRAMSize];
	
	memset(_internalRAM, 0, InternalRAMSize);

	memset(_ramReadHandlers, 0, RAMSize * sizeof(IMemoryHandler*));
	memset(_ramWriteHandlers, 0, RAMSize * sizeof(IMemoryHandler*));

	memset(_vramReadHandlers, 0, VRAMSize * sizeof(IMemoryHandler*));
	memset(_vramWriteHandlers, 0, VRAMSize * sizeof(IMemoryHandler*));
}

MemoryManager::~MemoryManager()
{
	delete[] _internalRAM;
	for(int i = 0; i < 4; i++) {
		delete[] _nametableRAM[i];
	}

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
	return _mapper->ReadVRAM(addr);
}

void MemoryManager::WriteMappedVRAM(uint16_t addr, uint8_t value)
{
	return _mapper->WriteVRAM(addr, value);
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

uint8_t* MemoryManager::GetInternalRAM()
{
	return _internalRAM;
}

uint8_t MemoryManager::DebugRead(uint16_t addr)
{
	if(addr <= 0x1FFF) {
		return _internalRAM[addr & 0x07FF];
	} else if(addr > 0x4017) {
		return ReadRegister(addr);
	}

	return 0;
}

uint8_t MemoryManager::Read(uint16_t addr, bool forExecution)
{
	Debugger::CheckBreakpoint(forExecution ? BreakpointType::Execute : BreakpointType::Read, addr);

	uint8_t value;
	if(addr <= 0x1FFF) {
		value = _internalRAM[addr & 0x07FF];
	} else {
		value = ReadRegister(addr);
	}

	CheatManager::ApplyRamCodes(addr, value);
	return value;
}

void MemoryManager::Write(uint16_t addr, uint8_t value)
{
	Debugger::CheckBreakpoint(BreakpointType::Write, addr);

	if(addr <= 0x1FFF) {
		_internalRAM[addr & 0x07FF] = value;
	} else {
		WriteRegister(addr, value);
	}
}

uint8_t MemoryManager::ReadVRAM(uint16_t addr)
{	
	addr &= 0x3FFF;

	if(addr <= 0x1FFF) {
		_mapper->NotifyVRAMAddressChange(addr);
		return ReadMappedVRAM(addr);
	} else {
		if(addr >= 0x3000) {
			//Need to mirror 0x3000 writes to 0x2000, this appears to be how hardware behaves
			//Required for proper MMC3 IRQ timing in Burai Fighter
			addr -= 0x1000;
		}
		_mapper->NotifyVRAMAddressChange(addr);
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
	addr &= 0x3FFF;

	if(addr <= 0x1FFF) {
		_mapper->NotifyVRAMAddressChange(addr);
		WriteMappedVRAM(addr, value);
	} else {
		if(addr >= 0x3000) {
			//Need to mirror 0x3000 writes to 0x2000, this appears to be how hardware behaves
			//Required for proper MMC3 IRQ timing in Burai Fighter
			addr -= 0x1000;
		}
		_mapper->NotifyVRAMAddressChange(addr);
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
	for(int i = 0; i < 4; i++) {
		StreamArray<uint8_t>(_nametableRAM[i], MemoryManager::NameTableScreenSize);
	}
}