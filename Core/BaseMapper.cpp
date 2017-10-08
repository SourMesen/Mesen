#include "stdafx.h"
#include <random>
#include <assert.h>
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/IpsPatcher.h"
#include "BaseMapper.h"
#include "Console.h"
#include "CheatManager.h"
#include "Debugger.h"
#include "MemoryManager.h"

void BaseMapper::WriteRegister(uint16_t addr, uint8_t value) { }
uint8_t BaseMapper::ReadRegister(uint16_t addr) { return 0; }
void BaseMapper::InitMapper(RomData &romData) { }
void BaseMapper::Reset(bool softReset) { }

//Make sure the page size is no bigger than the size of the ROM itself
//Otherwise we will end up reading from unallocated memory
uint16_t BaseMapper::InternalGetPrgPageSize() { return std::min((uint32_t)GetPRGPageSize(), _prgSize); }
uint16_t BaseMapper::InternalGetSaveRamPageSize() { return std::min((uint32_t)GetSaveRamPageSize(), _saveRamSize); }
uint16_t BaseMapper::InternalGetWorkRamPageSize() { return std::min((uint32_t)GetWorkRamPageSize(), _workRamSize); }
uint16_t BaseMapper::InternalGetChrPageSize() { return std::min((uint32_t)GetCHRPageSize(), _chrRomSize); }
uint16_t BaseMapper::InternalGetChrRamPageSize() { return std::min((uint32_t)GetChrRamPageSize(), _chrRamSize); }
	
void BaseMapper::SetCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, int16_t pageNumber, PrgMemoryType type, int8_t accessType)
{
#ifdef _DEBUG
	if((startAddr & 0xFF) || (endAddr & 0xFF) != 0xFF) {
		throw new std::runtime_error("Start/End address must be multiples of 256/0x100");
	}
#endif

	uint8_t* source = nullptr;
	uint32_t pageCount;
	uint32_t pageSize;
	uint8_t defaultAccessType = MemoryAccessType::Read;
	switch(type) {
		case PrgMemoryType::PrgRom:
			source = _prgRom;
			pageCount = GetPRGPageCount();
			pageSize = InternalGetPrgPageSize();
			break;
		case PrgMemoryType::SaveRam:
			source = _saveRam;
			pageSize = InternalGetSaveRamPageSize();
			if(pageSize == 0) {
				#ifdef _DEBUG
				MessageManager::DisplayMessage("Debug", "Tried to map undefined save ram.");
				#endif
				return;
			}
			pageCount = _saveRamSize / pageSize;
			
			defaultAccessType |= MemoryAccessType::Write;
			break;
		case PrgMemoryType::WorkRam:
			source = _workRam;
			pageSize = InternalGetWorkRamPageSize();
			if(pageSize == 0) {
				#ifdef _DEBUG
				MessageManager::DisplayMessage("Debug", "Tried to map undefined work ram.");
				#endif
				return;
			}

			pageCount = _workRamSize / pageSize;
			
			defaultAccessType |= MemoryAccessType::Write;
			break;
		default:
			throw new std::runtime_error("Invalid parameter");
	}

	if(pageCount == 0) {
		#ifdef _DEBUG
		MessageManager::DisplayMessage("Debug", "Tried to map undefined save/work ram.");
		#endif
		return;
	}

	auto wrapPageNumber = [=](int16_t &page) -> void {
		if(page < 0) {
			//Can't use modulo for negative number because pageCount is sometimes not a power of 2.  (Fixes some Mapper 191 games)
			page = pageCount + page;
		} else {
			page = page % pageCount;
		}
	};
	wrapPageNumber(pageNumber);
	
	uint8_t* sourceBuffer = &source[pageNumber * pageSize];

	accessType = accessType != -1 ? accessType : defaultAccessType;

	if((uint16_t)(endAddr - startAddr) >= pageSize) {
		#ifdef _DEBUG
		MessageManager::DisplayMessage("Debug", "Tried to map undefined prg - page size too small for selected range.");
		#endif
		
		//If range is bigger than a single page, keep going until we reach the last page
		uint32_t addr = startAddr;
		while(addr <= endAddr - pageSize + 1) {
			SetCpuMemoryMapping(addr, addr + pageSize - 1, sourceBuffer, accessType);
			addr += pageSize;
			pageNumber++;
			wrapPageNumber(pageNumber);
			sourceBuffer = &source[pageNumber * pageSize];
		}
	} else {
		SetCpuMemoryMapping(startAddr, endAddr, sourceBuffer, accessType);
	}
}

void BaseMapper::SetCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint8_t *source, int8_t accessType)
{
	#ifdef _DEBUG
	if((startAddr & 0xFF) || (endAddr & 0xFF) != 0xFF) {
		throw new std::runtime_error("Start/End address must be multiples of 256/0x100");
	}
	#endif

	startAddr >>= 8;
	endAddr >>= 8;
	for(uint16_t i = startAddr; i <= endAddr; i++) {
		_prgPages[i] = source;
		_prgPageAccessType[i] = accessType != -1 ? accessType : MemoryAccessType::Read;

		source += 0x100;
	}
}

void BaseMapper::RemoveCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr)
{
	//Unmap this section of memory (causing open bus behavior)
	SetCpuMemoryMapping(startAddr, endAddr, nullptr, MemoryAccessType::NoAccess);
}

void BaseMapper::SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint16_t pageNumber, ChrMemoryType type, int8_t accessType)
{
	uint32_t pageCount = 0;
	uint32_t pageSize = 0;
	uint8_t* sourceMemory = nullptr;
	uint8_t defaultAccessType = MemoryAccessType::Read;
	switch(type) {
		case ChrMemoryType::Default:
			pageSize = InternalGetChrPageSize();
			if(pageSize == 0) {
				#ifdef _DEBUG
				MessageManager::DisplayMessage("Debug", "Tried to map undefined chr rom/ram.");
				#endif
				return;
			}
			pageCount = GetCHRPageCount();
			sourceMemory = _onlyChrRam ? _chrRam : _chrRom;
			if(_onlyChrRam) {
				defaultAccessType |= MemoryAccessType::Write;
			}
			break;

		case ChrMemoryType::ChrRom:
			pageSize = InternalGetChrPageSize();
			if(pageSize == 0) {
				#ifdef _DEBUG
				MessageManager::DisplayMessage("Debug", "Tried to map undefined chr rom.");
				#endif
				return;
			}
			pageCount = GetCHRPageCount();

			sourceMemory = _chrRom;
			break;

		case ChrMemoryType::ChrRam:
			pageSize = InternalGetChrRamPageSize();
			if(pageSize == 0) {
				#ifdef _DEBUG
				MessageManager::DisplayMessage("Debug", "Tried to map undefined chr ram.");
				#endif
				return;
			}
			pageCount = _chrRamSize / pageSize;
			sourceMemory = _chrRam;
			defaultAccessType |= MemoryAccessType::Write;
			break;
	}

	if(pageCount == 0) {
		#ifdef _DEBUG
		MessageManager::DisplayMessage("Debug", "Tried to map undefined chr ram.");
		#endif
		return;
	}

	pageNumber = pageNumber % pageCount;

	uint8_t* sourceBuffer = sourceMemory + pageNumber * pageSize;

	if((uint16_t)(endAddr - startAddr) >= pageSize) {
		#ifdef _DEBUG
		MessageManager::DisplayMessage("Debug", "Tried to map undefined chr - page size too small for selected range.");
		#endif

		uint32_t addr = startAddr;
		while(addr <= endAddr - pageSize + 1) {
			SetPpuMemoryMapping(addr, addr + pageSize - 1, sourceBuffer, accessType);
			addr += pageSize;
			pageNumber = (pageNumber + 1) % pageCount;
			sourceBuffer = &sourceMemory[pageNumber * pageSize];
		}
	} else {
		SetPpuMemoryMapping(startAddr, endAddr, sourceBuffer, accessType == -1 ? defaultAccessType : accessType);
	}
}

void BaseMapper::SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint8_t* sourceMemory, int8_t accessType)
{
	#ifdef _DEBUG
	if((startAddr & 0xFF) || (endAddr & 0xFF) != 0xFF) {
		throw new std::runtime_error("Start/End address must be multiples of 256/0x100");
	}
	#endif

	startAddr >>= 8;
	endAddr >>= 8;
	for(uint16_t i = startAddr; i <= endAddr; i++) {
		_chrPages[i] = sourceMemory;
		_chrPageAccessType[i] = accessType != -1 ? accessType : MemoryAccessType::ReadWrite;

		if(sourceMemory != nullptr) {
			sourceMemory += 0x100;
		}
	}
}

void BaseMapper::RemovePpuMemoryMapping(uint16_t startAddr, uint16_t endAddr)
{
	//Unmap this section of memory (causing open bus behavior)
	SetPpuMemoryMapping(startAddr, endAddr, nullptr, MemoryAccessType::NoAccess);
}

uint8_t BaseMapper::InternalReadRam(uint16_t addr)
{
	return _prgPages[addr >> 8] ? _prgPages[addr >> 8][(uint8_t)addr] : 0;
}

void BaseMapper::SelectPrgPage4x(uint16_t slot, uint16_t page, PrgMemoryType memoryType)
{
	BaseMapper::SelectPrgPage2x(slot*2, page, memoryType);
	BaseMapper::SelectPrgPage2x(slot*2+1, page+2, memoryType);
}

void BaseMapper::SelectPrgPage2x(uint16_t slot, uint16_t page, PrgMemoryType memoryType)
{
	BaseMapper::SelectPRGPage(slot*2, page, memoryType);
	BaseMapper::SelectPRGPage(slot*2+1, page+1, memoryType);
}

void BaseMapper::SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType)
{
	_prgPageNumbers[slot] = page;

	if(_prgSize < 0x8000 && GetPRGPageSize() > _prgSize) {
		//Total PRG size is smaller than available memory range, map the entire PRG to all slots
		//i.e same logic as NROM (mapper 0) when PRG is 16kb
		//Needed by "Pyramid" (mapper 79)
		#ifdef _DEBUG
			MessageManager::DisplayMessage("Debug", "PrgSizeWarning");
		#endif

		for(slot = 0; slot < 0x8000 / _prgSize; slot++) {
			uint16_t startAddr = 0x8000 + slot * _prgSize;
			uint16_t endAddr = startAddr + _prgSize - 1;
			SetCpuMemoryMapping(startAddr, endAddr, 0, memoryType);
		}
	} else {
		uint16_t startAddr = 0x8000 + slot * InternalGetPrgPageSize();
		uint16_t endAddr = startAddr + InternalGetPrgPageSize() - 1;
		SetCpuMemoryMapping(startAddr, endAddr, page, memoryType);
	}
}

void BaseMapper::SelectChrPage8x(uint16_t slot, uint16_t page, ChrMemoryType memoryType)
{
	BaseMapper::SelectChrPage4x(slot, page, memoryType);
	BaseMapper::SelectChrPage4x(slot*2+1, page+4, memoryType);
}

void BaseMapper::SelectChrPage4x(uint16_t slot, uint16_t page, ChrMemoryType memoryType)
{
	BaseMapper::SelectChrPage2x(slot*2, page, memoryType);
	BaseMapper::SelectChrPage2x(slot*2+1, page+2, memoryType);
}

void BaseMapper::SelectChrPage2x(uint16_t slot, uint16_t page, ChrMemoryType memoryType)
{
	BaseMapper::SelectCHRPage(slot*2, page, memoryType);
	BaseMapper::SelectCHRPage(slot*2+1, page+1, memoryType);
}

void BaseMapper::SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType)
{
	_chrPageNumbers[slot] = page;

	uint16_t pageSize = memoryType == ChrMemoryType::ChrRam ? InternalGetChrRamPageSize() : InternalGetChrPageSize();
	uint16_t startAddr = slot * pageSize;
	uint16_t endAddr = startAddr + pageSize - 1;
	if(page == ChrSpecialPage::NametableA) {
		SetPpuMemoryMapping(startAddr, endAddr, GetNametable(0));
	} else if(page == ChrSpecialPage::NametableB) {
		SetPpuMemoryMapping(startAddr, endAddr, GetNametable(1));
	} else {
		SetPpuMemoryMapping(startAddr, endAddr, page, memoryType);
	}
}

void BaseMapper::InitializeRam(void* data, uint32_t length)
{
	switch(EmulationSettings::GetRamPowerOnState()) {
		default:
		case RamPowerOnState::AllZeros: memset(data, 0, length); break;
		case RamPowerOnState::AllOnes: memset(data, 0xFF, length); break;
		case RamPowerOnState::Random:
			std::random_device rd;
			std::mt19937 mt(rd());
			std::uniform_int_distribution<> dist(0, 255);
			for(uint32_t i = 0; i < length; i++) {
				((uint8_t*)data)[i] = dist(mt);
			}
			break;
	}
}
		
bool BaseMapper::HasBattery()
{
	return _hasBattery;
}

void BaseMapper::LoadBattery()
{
	if(HasBattery()) {
		ifstream batteryFile(_batteryFilename, ios::in | ios::binary);
		if(batteryFile) {
			batteryFile.read((char*)_saveRam, _saveRamSize);
			batteryFile.close();
		}
	}

	if(_hasChrBattery) {
		ifstream batteryFile(_batteryFilename + ".chr", ios::in | ios::binary);
		if(batteryFile) {
			batteryFile.read((char*)_chrRam, _chrRamSize);
			batteryFile.close();
		}
	}
}

void BaseMapper::SaveBattery()
{
	if(HasBattery()) {
		ofstream batteryFile(_batteryFilename, ios::out | ios::binary);

		if(batteryFile) {
			batteryFile.write((char*)_saveRam, _saveRamSize);

			batteryFile.close();
		}
	}

	if(_hasChrBattery) {
		ofstream batteryFile(_batteryFilename + ".chr", ios::out | ios::binary);
		if(batteryFile) {
			batteryFile.write((char*)_chrRam, _chrRamSize);
			batteryFile.close();
		}
	}
}

uint32_t BaseMapper::GetPRGPageCount()
{
	return _prgSize / InternalGetPrgPageSize();
}

uint32_t BaseMapper::GetCHRPageCount()
{
	return _chrRomSize / InternalGetChrPageSize();
}

string BaseMapper::GetBatteryFilename()
{
	return FolderUtilities::CombinePath(FolderUtilities::GetSaveFolder(), FolderUtilities::GetFilename(_romName, false) + ".sav");
}
		
void BaseMapper::RestoreOriginalPrgRam()
{
	memcpy(_prgRom, _originalPrgRom.data(), _originalPrgRom.size());
}

void BaseMapper::InitializeChrRam(int32_t chrRamSize)
{
	uint32_t defaultRamSize = GetChrRamSize() ? GetChrRamSize() : 0x2000;
	_chrRamSize = chrRamSize >= 0 ? chrRamSize : defaultRamSize;
	if(_chrRamSize > 0) {
		_chrRam = new uint8_t[_chrRamSize];
		BaseMapper::InitializeRam(_chrRam, _chrRamSize);
	}
}

bool BaseMapper::HasChrRam()
{
	return _chrRamSize > 0;
}

bool BaseMapper::HasChrRom()
{
	return !_onlyChrRam;
}

void BaseMapper::AddRegisterRange(uint16_t startAddr, uint16_t endAddr, MemoryOperation operation)
{
	for(int i = startAddr; i <= endAddr; i++) {
		if((int)operation & (int)MemoryOperation::Read) {
			_isReadRegisterAddr[i] = true;
		}
		if((int)operation & (int)MemoryOperation::Write) {
			_isWriteRegisterAddr[i] = true;
		}
	}
}

void BaseMapper::RemoveRegisterRange(uint16_t startAddr, uint16_t endAddr, MemoryOperation operation)
{
	for(int i = startAddr; i <= endAddr; i++) {
		if((int)operation & (int)MemoryOperation::Read) {
			_isReadRegisterAddr[i] = false;
		}
		if((int)operation & (int)MemoryOperation::Write) {
			_isWriteRegisterAddr[i] = false;
		}
	}
}

void BaseMapper::StreamState(bool saving)
{
	ArrayInfo<uint8_t> chrRam = { _chrRam, _chrRamSize };
	ArrayInfo<uint8_t> workRam = { _workRam, _workRamSize };
	ArrayInfo<uint8_t> saveRam = { _saveRam, _saveRamSize };
	ArrayInfo<uint32_t> prgPageNumbers = { _prgPageNumbers, 64 };
	ArrayInfo<uint32_t> chrPageNumbers = { _chrPageNumbers, 64 };
	ArrayInfo<uint8_t> nametableIndexes = { _nametableIndexes, 4 };
	Stream(_mirroringType, chrRam, workRam, saveRam, prgPageNumbers, chrPageNumbers, nametableIndexes);

	bool hasExtraNametable[2] = { _cartNametableRam[0] != nullptr, _cartNametableRam[1] != nullptr };
	Stream(hasExtraNametable[0], hasExtraNametable[1]);
	
	for(int i = 0; i < 2; i++) {
		if(hasExtraNametable[i]) {
			if(!_cartNametableRam[i]) {
				_cartNametableRam[i] = new uint8_t[0x400];
			}

			ArrayInfo<uint8_t> ram = { _cartNametableRam[i], 0x400 };
			Stream(ram);
		}
	}
	
	if(!saving) {
		for(uint16_t i = 0; i < 64; i++) {
			if(_prgPageNumbers[i] != 0xEEEEEEEE) {
				BaseMapper::SelectPRGPage(i, (uint16_t)_prgPageNumbers[i]);
			}
		}

		for(uint16_t i = 0; i < 64; i++) {
			if(_chrPageNumbers[i] != 0xEEEEEEEE) {
				BaseMapper::SelectCHRPage(i, (uint16_t)_chrPageNumbers[i]);
			}
		}

		for(int i = 0; i < 4; i++) {
			SetNametable(i, _nametableIndexes[i]);
		}
	}
}

void BaseMapper::Initialize(RomData &romData)
{
	_mapperID = romData.MapperID;
	_subMapperID = romData.SubMapperID;

	_databaseInfo = romData.DatabaseInfo;

	_romName = romData.RomName;
	_romFilename = romData.Filename;
	_batteryFilename = GetBatteryFilename();
	
	_hasBattery = (romData.HasBattery || ForceBattery());

	if(romData.SaveRamSize == -1 || ForceSaveRamSize()) {
		_saveRamSize = GetSaveRamSize(); //Needed because we need to call SaveBattery() in the destructor (and calling virtual functions in the destructor doesn't work correctly)
	} else {
		_saveRamSize = romData.SaveRamSize;
	}

	if(_saveRamSize == 0) {
		_hasBattery = false;
	}

	if(romData.WorkRamSize == -1 || ForceWorkRamSize()) {
		_workRamSize = GetWorkRamSize();
	} else {
		_workRamSize = romData.WorkRamSize;
	}

	_allowRegisterRead = AllowRegisterRead();

	memset(_isReadRegisterAddr, 0, sizeof(_isReadRegisterAddr));
	memset(_isWriteRegisterAddr, 0, sizeof(_isWriteRegisterAddr));
	AddRegisterRange(RegisterStartAddress(), RegisterEndAddress(), MemoryOperation::Any);

	_nesHeader = romData.NesHeader;
	_romFormat = romData.Format;

	_mirroringType = romData.Mirroring;

	_prgSize = (uint32_t)romData.PrgRom.size();
	_chrRomSize = (uint32_t)romData.ChrRom.size();
	_originalPrgRom = romData.PrgRom;
	_originalChrRom = romData.ChrRom;

	_prgRom = new uint8_t[_prgSize];
	_chrRom = new uint8_t[_chrRomSize];
	memcpy(_prgRom, romData.PrgRom.data(), _prgSize);
	if(_chrRomSize > 0) {
		memcpy(_chrRom, romData.ChrRom.data(), _chrRomSize);
	}

	_hasChrBattery = romData.SaveChrRamSize > 0 || ForceChrBattery();

	_gameSystem = romData.System;
	_hashInfo.Crc32Hash = romData.Crc32;
	_hashInfo.PrgCrc32Hash = romData.PrgCrc32;
	_hashInfo.Sha1Hash = romData.Sha1;
	_hashInfo.PrgChrMd5Hash = romData.PrgChrMd5;
	switch(romData.BusConflicts) {
		case BusConflictType::Default: _hasBusConflicts = HasBusConflicts(); break;
		case BusConflictType::Yes: _hasBusConflicts = true; break;
		case BusConflictType::No: _hasBusConflicts = false; break;
	}	

	_saveRam = new uint8_t[_saveRamSize];
	_workRam = new uint8_t[_workRamSize];

	BaseMapper::InitializeRam(_saveRam, _saveRamSize);
	BaseMapper::InitializeRam(_workRam, _workRamSize);

	memset(_prgPageNumbers, 0xEE, sizeof(_prgPageNumbers));
	memset(_chrPageNumbers, 0xEE, sizeof(_chrPageNumbers));

	memset(_cartNametableRam, 0, sizeof(_cartNametableRam));
	memset(_nametableIndexes, 0, sizeof(_nametableIndexes));

	for(int i = 0; i <= 0xFF; i++) {
		//Allow us to map a different page every 256 bytes
		_prgPages[i] = nullptr;
		_prgPageAccessType[i] = MemoryAccessType::NoAccess;
		_chrPages[i] = nullptr;
		_chrPageAccessType[i] = MemoryAccessType::NoAccess;
	}

	if(_chrRomSize == 0) {
		//Assume there is CHR RAM if no CHR ROM exists
		_onlyChrRam = true;
		InitializeChrRam(romData.ChrRamSize);

		//Map CHR RAM to 0x0000-0x1FFF by default when no CHR ROM exists
		SetPpuMemoryMapping(0x0000, 0x1FFF, 0, ChrMemoryType::ChrRam);
		_chrRomSize = _chrRamSize;
	} else if(romData.ChrRamSize >= 0) {
		InitializeChrRam(romData.ChrRamSize);
	} else if(GetChrRamSize()) {
		InitializeChrRam();
	}

	//Load battery data if present
	LoadBattery();

	if(romData.HasTrainer) {
		if(_workRamSize >= 0x2000) {
			memcpy(_workRam + 0x1000, romData.TrainerData.data(), 512);
		} else if(_saveRamSize >= 0x2000) {
			memcpy(_saveRam + 0x1000, romData.TrainerData.data(), 512);
		}
	}

	//Setup a default work/save ram in 0x6000-0x7FFF space
	if(HasBattery() && _saveRamSize > 0) {
		SetCpuMemoryMapping(0x6000, 0x7FFF, 0, PrgMemoryType::SaveRam);
	} else if(_workRamSize > 0) {
		SetCpuMemoryMapping(0x6000, 0x7FFF, 0, PrgMemoryType::WorkRam);
	}

	InitMapper();
	InitMapper(romData);

	MessageManager::RegisterNotificationListener(this);

	ApplyCheats();
}

BaseMapper::~BaseMapper()
{
	delete[] _chrRam;
	delete[] _chrRom;
	delete[] _prgRom;
	delete[] _saveRam;
	delete[] _workRam;

	if(_cartNametableRam[0]) {
		delete[] _cartNametableRam[0];
	}

	if(_cartNametableRam[1]) {
		delete[] _cartNametableRam[1];
	}

	MessageManager::UnregisterNotificationListener(this);
}

void BaseMapper::ProcessNotification(ConsoleNotificationType type, void* parameter)
{
	switch(type) {
		case ConsoleNotificationType::CheatAdded:
		case ConsoleNotificationType::CheatRemoved:
			ApplyCheats();
			break;
		default:
			break;
	}
}


void BaseMapper::ApplyCheats()
{
	RestoreOriginalPrgRam();
	CheatManager::ApplyPrgCodes(_prgRom, _prgSize);
}

void BaseMapper::GetMemoryRanges(MemoryRanges &ranges)
{
	if(_gameSystem == GameSystem::VsUniSystem) {
		ranges.AddHandler(MemoryOperation::Read, 0x6000, 0xFFFF);
		ranges.AddHandler(MemoryOperation::Write, 0x6000, 0xFFFF);
	} else {
		ranges.AddHandler(MemoryOperation::Read, 0x4018, 0xFFFF);
		ranges.AddHandler(MemoryOperation::Write, 0x4018, 0xFFFF);
	}
}

void BaseMapper::SetDefaultNametables(uint8_t* nametableA, uint8_t* nametableB)
{
	_nesNametableRam[0] = nametableA;
	_nesNametableRam[1] = nametableB;
	SetMirroringType(_mirroringType);
}

void BaseMapper::AddNametable(uint8_t index, uint8_t *nametable)
{
	assert(index >= 4);
	_cartNametableRam[index - 2] = nametable;
}

uint8_t* BaseMapper::GetNametable(uint8_t index)
{
	if(index <= 1) {
		return _nesNametableRam[index];
	} else {
		return _cartNametableRam[index - 2];
	}
}

void BaseMapper::SetNametable(uint8_t index, uint8_t nametableIndex)
{
	if(nametableIndex == 2 && _cartNametableRam[0] == nullptr) {
		_cartNametableRam[0] = new uint8_t[0x400];
		BaseMapper::InitializeRam(_cartNametableRam[0], 0x400);
	}
	if(nametableIndex == 3 && _cartNametableRam[1] == nullptr) {
		_cartNametableRam[1] = new uint8_t[0x400];
		BaseMapper::InitializeRam(_cartNametableRam[1], 0x400);
	}

	_nametableIndexes[index] = nametableIndex;

	SetPpuMemoryMapping(0x2000 + index * 0x400, 0x2000 + (index + 1) * 0x400 - 1, GetNametable(nametableIndex));
}

void BaseMapper::SetNametables(uint8_t nametable1Index, uint8_t nametable2Index, uint8_t nametable3Index, uint8_t nametable4Index)
{
	SetNametable(0, nametable1Index);
	SetNametable(1, nametable2Index);
	SetNametable(2, nametable3Index);
	SetNametable(3, nametable4Index);
}

void BaseMapper::SetMirroringType(MirroringType type)
{
	_mirroringType = type;
	switch(type) {
		case MirroringType::Vertical: SetNametables(0, 1, 0, 1); break;
		case MirroringType::Horizontal: SetNametables(0, 0, 1, 1); break;
		case MirroringType::FourScreens:	SetNametables(0, 1, 2, 3); break;
		case MirroringType::ScreenAOnly: SetNametables(0, 0, 0, 0);	break;
		case MirroringType::ScreenBOnly: SetNametables(1, 1, 1, 1);	break;
	}
}

GameSystem BaseMapper::GetGameSystem()
{
	return _gameSystem;
}

string BaseMapper::GetRomName()
{
	return _romName;
}

RomFormat BaseMapper::GetRomFormat()
{
	return _romFormat;
}

HashInfo BaseMapper::GetHashInfo()
{
	return _hashInfo;
}

MirroringType BaseMapper::GetMirroringType()
{
	return _mirroringType;
}
	
uint8_t BaseMapper::ReadRAM(uint16_t addr)
{
	if(_allowRegisterRead && _isReadRegisterAddr[addr]) {
		return ReadRegister(addr);
	} else if(_prgPageAccessType[addr >> 8] & MemoryAccessType::Read) {
		return _prgPages[addr >> 8][(uint8_t)addr];
	} else {
		//assert(false);
	}
	return MemoryManager::GetOpenBus();
}

uint8_t BaseMapper::DebugReadRAM(uint16_t addr)
{
	if(_prgPageAccessType[addr >> 8] & MemoryAccessType::Read) {
		return _prgPages[addr >> 8][(uint8_t)addr];
	} else {
		//assert(false);
	}
	return MemoryManager::GetOpenBus();
}

void BaseMapper::WriteRAM(uint16_t addr, uint8_t value)
{
	if(_isWriteRegisterAddr[addr]) {
		if(_hasBusConflicts) {
			value &= _prgPages[addr >> 8][(uint8_t)addr];
		}
		WriteRegister(addr, value);
	} else {
		WritePrgRam(addr, value);
	}
}

void BaseMapper::DebugWriteRAM(uint16_t addr, uint8_t value)
{
	if(_isWriteRegisterAddr[addr]) {
		if(_hasBusConflicts) {
			value &= _prgPages[addr >> 8][(uint8_t)addr];
		}
	} else {
		WritePrgRam(addr, value);
	}
}

void BaseMapper::WritePrgRam(uint16_t addr, uint8_t value)
{
	if(_prgPageAccessType[addr >> 8] & MemoryAccessType::Write) {
		_prgPages[addr >> 8][(uint8_t)addr] = value;
	}
}

void BaseMapper::ProcessVramAccess(uint16_t &addr)
{
	addr &= 0x3FFF;
	if(addr >= 0x3000) {
		//Need to mirror 0x3000 writes to 0x2000, this appears to be how hardware behaves
		//Required for proper MMC3 IRQ timing in Burai Fighter
		addr -= 0x1000;
	}
}

void BaseMapper::NotifyVRAMAddressChange(uint16_t addr)
{
	//This is called when the VRAM addr on the PPU memory bus changes
	//Used by MMC3/MMC5/etc
}

uint8_t BaseMapper::InternalReadVRAM(uint16_t addr)
{
	if(_chrPageAccessType[addr >> 8] & MemoryAccessType::Read) {
		return _chrPages[addr >> 8][(uint8_t)addr];
	}

	//Open bus - "When CHR is disabled, the pattern tables are open bus. Theoretically, this should return the LSB of the address read, but real-world behavior varies."
	return _vramOpenBusValue >= 0 ? _vramOpenBusValue : addr;
}

uint8_t BaseMapper::DebugReadVRAM(uint16_t addr, bool disableSideEffects)
{
	ProcessVramAccess(addr);
	if(!disableSideEffects) {
		NotifyVRAMAddressChange(addr);
	}
	return InternalReadVRAM(addr);
}

uint8_t BaseMapper::MapperReadVRAM(uint16_t addr, MemoryOperationType operationType)
{
	return InternalReadVRAM(addr);
}

void BaseMapper::DebugWriteVRAM(uint16_t addr, uint8_t value, bool disableSideEffects)
{
	ProcessVramAccess(addr);
	if(!disableSideEffects) {
		NotifyVRAMAddressChange(addr);
	}
	if(_chrPageAccessType[addr >> 8] & MemoryAccessType::Write) {
		_chrPages[addr >> 8][(uint8_t)addr] = value;
	}
}

void BaseMapper::WriteVRAM(uint16_t addr, uint8_t value)
{
	ProcessVramAccess(addr);
	Debugger::ProcessVramWriteOperation(addr, value);
	NotifyVRAMAddressChange(addr);

	if(_chrPageAccessType[addr >> 8] & MemoryAccessType::Write) {
		_chrPages[addr >> 8][(uint8_t)addr] = value;
	}
}

bool BaseMapper::IsNes20()
{
	return _nesHeader.GetRomHeaderVersion() == RomHeaderVersion::Nes2_0;
}

//Debugger Helper Functions
uint8_t* BaseMapper::GetPrgRom()
{
	return _prgRom;
}

uint8_t* BaseMapper::GetSaveRam()
{
	return _saveRam;
}

uint8_t* BaseMapper::GetWorkRam()
{
	return _workRam;
}

uint32_t BaseMapper::CopyMemory(DebugMemoryType type, uint8_t* buffer)
{
	uint32_t chrRomSize = _onlyChrRam ? 0 : _chrRomSize;
	switch(type) {
		case DebugMemoryType::ChrRam: memcpy(buffer, _chrRam, _chrRamSize); return _chrRamSize;
		case DebugMemoryType::ChrRom: memcpy(buffer, _chrRom, chrRomSize); return chrRomSize;
		case DebugMemoryType::PrgRom: memcpy(buffer, _prgRom, _prgSize); return _prgSize;
		case DebugMemoryType::SaveRam: memcpy(buffer, _saveRam, _saveRamSize); return _saveRamSize;
		case DebugMemoryType::WorkRam: memcpy(buffer, _workRam, _workRamSize); return _workRamSize;
	}

	return 0;
}

void BaseMapper::WriteMemory(DebugMemoryType type, uint8_t* buffer)
{
	switch(type) {
		case DebugMemoryType::ChrRam: memcpy(_chrRam, buffer, _chrRamSize); break;
		case DebugMemoryType::SaveRam: memcpy(_saveRam, buffer, _saveRamSize); break;
		case DebugMemoryType::WorkRam: memcpy(_workRam, buffer, _workRamSize); break;
	}
}

uint32_t BaseMapper::GetMemorySize(DebugMemoryType type)
{
	switch(type) {
		default: return 0;
		case DebugMemoryType::ChrRom: return _onlyChrRam ? 0 : _chrRomSize;
		case DebugMemoryType::ChrRam: return _chrRamSize;
		case DebugMemoryType::SaveRam: return _saveRamSize;
		case DebugMemoryType::PrgRom: return _prgSize;
		case DebugMemoryType::WorkRam: return _workRamSize;
	}
}

uint8_t BaseMapper::GetMemoryValue(DebugMemoryType memoryType, uint32_t address)
{
	switch(memoryType) {
		case DebugMemoryType::ChrRom: return _onlyChrRam ? _chrRam[address] : _chrRom[address];
		case DebugMemoryType::ChrRam: return _chrRam[address];
		case DebugMemoryType::SaveRam: return _saveRam[address];
		case DebugMemoryType::PrgRom: return _prgRom[address];
		case DebugMemoryType::WorkRam: return _workRam[address];
	}

	return 0;
}

void BaseMapper::SetMemoryValue(DebugMemoryType memoryType, uint32_t address, uint8_t value)
{
	switch(memoryType) {
		case DebugMemoryType::ChrRom: _chrRom[address] = value; break;
		case DebugMemoryType::ChrRam: _chrRam[address] = value; break;
		case DebugMemoryType::SaveRam: _saveRam[address] = value; break;
		case DebugMemoryType::PrgRom: _prgRom[address] = value; break;
		case DebugMemoryType::WorkRam: _workRam[address] = value; break;
	}
}

int32_t BaseMapper::ToAbsoluteAddress(uint16_t addr)
{
	uint8_t *prgAddr = _prgPages[addr >> 8] + (uint8_t)addr;
	if(prgAddr >= _prgRom && prgAddr < _prgRom + _prgSize) {
		return (uint32_t)(prgAddr - _prgRom);
	}
	return -1;
}

int32_t BaseMapper::ToAbsoluteWorkRamAddress(uint16_t addr)
{
	uint8_t *prgRamAddr = _prgPages[addr >> 8] + (uint8_t)addr;
	if(prgRamAddr >= _workRam && prgRamAddr < _workRam + _workRamSize) {
		return (uint32_t)(prgRamAddr - _workRam);
	}
	return -1;
}

int32_t BaseMapper::ToAbsoluteSaveRamAddress(uint16_t addr)
{
	uint8_t *prgRamAddr = _prgPages[addr >> 8] + (uint8_t)addr;
	if(prgRamAddr >= _saveRam && prgRamAddr < _saveRam + _saveRamSize) {
		return (uint32_t)(prgRamAddr - _saveRam);
	}
	return -1;
}

int32_t BaseMapper::ToAbsoluteChrAddress(uint16_t addr)
{
	uint8_t *chrAddr = _chrPages[addr >> 8] + (uint8_t)addr;
	if(chrAddr >= _chrRom && chrAddr < _chrRom + _chrRomSize) {
		return (uint32_t)(chrAddr - _chrRom);
	}
	if(chrAddr >= _chrRam && chrAddr < _chrRam + _chrRamSize) {
		return (uint32_t)(chrAddr - _chrRam);
	}
	return -1;
}

int32_t BaseMapper::FromAbsoluteAddress(uint32_t addr, AddressType type)
{
	uint8_t* ptrAddress;

	switch(type) {
		case AddressType::PrgRom: ptrAddress = _prgRom; break;
		case AddressType::WorkRam: ptrAddress = _workRam; break;
		case AddressType::SaveRam: ptrAddress = _saveRam; break;
		case AddressType::Register: return addr & 0xFFFF; break;
		case AddressType::InternalRam: return addr & 0x1FFF; break;
		default: return -1;
	}
	ptrAddress += addr;

	for(int i = 0; i < 256; i++) {
		uint8_t* pageAddress = _prgPages[i];
		if(pageAddress != nullptr && ptrAddress >= pageAddress && ptrAddress <= pageAddress + 0xFF) {
			return (i << 8) + (uint32_t)(ptrAddress - pageAddress);
		}
	}

	//Address is currently not mapped
	return -1;
}

CartridgeState BaseMapper::GetState()
{
	CartridgeState state;

	state.PrgRomSize = _prgSize;
	state.ChrRomSize = _onlyChrRam ? 0 : _chrRomSize;
	state.ChrRamSize = _chrRamSize;

	state.PrgPageCount = GetPRGPageCount();
	state.PrgPageSize = InternalGetPrgPageSize();
	state.ChrPageCount = GetCHRPageCount();
	state.ChrPageSize = InternalGetChrPageSize();
	for(int i = 0, max = 0x8000 / state.PrgPageSize; i < max; i++) {
		if(_prgPageNumbers[i] != 0xEEEEEEEE) {
			int16_t pageNumber = (int16_t)_prgPageNumbers[i];
			state.PrgSelectedPages[i] = pageNumber < 0 ? state.PrgPageCount + pageNumber : pageNumber;
		} else {
			state.PrgSelectedPages[i] = 0xEEEEEEEE;
		}
	}

	for(int i = 0, max = 0x2000 / state.ChrPageSize; i < max; i++) {
		if(_chrPageNumbers[i] != 0xEEEEEEEE) {
			int16_t pageNumber = (int16_t)_chrPageNumbers[i];
			state.ChrSelectedPages[i] = pageNumber < 0 ? state.ChrPageCount + pageNumber : pageNumber;
		} else {
			state.ChrSelectedPages[i] = 0xEEEEEEEE;
		}
	}

	for(int i = 0; i < 4; i++) {
		state.Nametables[i] = _nametableIndexes[i];
	}
	
	return state;
}

NESHeader BaseMapper::GetNesHeader()
{
	return _nesHeader;
}

void BaseMapper::SaveRomToDisk(string filename, bool saveAsIps, uint8_t* header)
{
	ofstream file(filename, ios::out | ios::binary);
	if(file.good()) {
		vector<uint8_t> originalFile;
		Console::GetRomPath().ReadFile(originalFile);
			
		if(header) {
			//Save original rom with edited header
			file.write((char*)header, sizeof(NESHeader));
			file.write((char*)originalFile.data()+sizeof(NESHeader), originalFile.size());
		} else {
			vector<uint8_t> newFile;
			newFile.insert(newFile.end(), (uint8_t*)&_nesHeader, ((uint8_t*)&_nesHeader) + sizeof(NESHeader));
			newFile.insert(newFile.end(), _prgRom, _prgRom + _prgSize);
			newFile.insert(newFile.end(), _chrRom, _chrRom + _chrRomSize);

			//Save edited rom
			if(saveAsIps) {
				vector<uint8_t> patchData = IpsPatcher::CreatePatch(originalFile, newFile);
				file.write((char*)patchData.data(), patchData.size());
			} else {
				file.write((char*)newFile.data(), newFile.size());
			}
		}
		file.close();
	}
}

void BaseMapper::RevertPrgChrChanges()
{
	memcpy(_prgRom, _originalPrgRom.data(), _originalPrgRom.size());
	if(_chrRom) {
		memcpy(_chrRom, _originalChrRom.data(), _originalChrRom.size());
	}
}

bool BaseMapper::HasPrgChrChanges()
{
	if(memcmp(_prgRom, _originalPrgRom.data(), _originalPrgRom.size()) != 0) {
		return true;
	}
	if(_chrRom) {
		if(memcmp(_chrRom, _originalChrRom.data(), _originalChrRom.size()) != 0) {
			return true;
		}
	}
	return false;
}