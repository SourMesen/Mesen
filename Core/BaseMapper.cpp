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
#include "BatteryManager.h"
#include "MessageManager.h"
#include "EmulationSettings.h"

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
	
bool BaseMapper::ValidateAddressRange(uint16_t startAddr, uint16_t endAddr)
{
	if((startAddr & 0xFF) || (endAddr & 0xFF) != 0xFF) {
		#ifdef _DEBUG
			throw new std::runtime_error("Start/End address must be multiples of 256/0x100");
		#else
			//Ignore this request in release mode - granularity smaller than 256 bytes is not supported
			return false;
		#endif
	}
	return true;
}

void BaseMapper::SetCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, int16_t pageNumber, PrgMemoryType type, int8_t accessType)
{
	if(!ValidateAddressRange(startAddr, endAddr) || startAddr > 0xFF00 || endAddr <= startAddr) {
		return;
	}

	uint32_t pageCount;
	uint32_t pageSize;
	uint8_t defaultAccessType = MemoryAccessType::Read;
	switch(type) {
		case PrgMemoryType::PrgRom:
			pageCount = GetPRGPageCount();
			pageSize = InternalGetPrgPageSize();
			break;
		case PrgMemoryType::SaveRam:
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
	
	accessType = accessType != -1 ? accessType : defaultAccessType;
	
	if((uint16_t)(endAddr - startAddr) >= pageSize) {
		#ifdef _DEBUG
		MessageManager::DisplayMessage("Debug", "Tried to map undefined prg - page size too small for selected range.");
		#endif
		
		//If range is bigger than a single page, keep going until we reach the last page
		uint32_t addr = startAddr;
		while(addr <= endAddr - pageSize + 1) {
			SetCpuMemoryMapping(addr, addr + pageSize - 1, type, pageNumber * pageSize, accessType);
			addr += pageSize;
			pageNumber++;
			wrapPageNumber(pageNumber);
		}
	} else {
		SetCpuMemoryMapping(startAddr, endAddr, type, pageNumber * pageSize, accessType);
	}
}

void BaseMapper::SetCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, PrgMemoryType type, uint32_t sourceOffset, int8_t accessType)
{
	uint8_t* source = nullptr;
	switch(type) {
		default:
		case PrgMemoryType::PrgRom: source = _prgRom; break;
		case PrgMemoryType::SaveRam: source = _saveRam; break;
		case PrgMemoryType::WorkRam: source = _workRam; break;
	}

	int firstSlot = startAddr >> 8;
	int slotCount = (endAddr - startAddr + 1) >> 8;
	for(int i = 0; i < slotCount; i++) {
		_prgMemoryOffset[firstSlot + i] = sourceOffset + i * 0x100;
		_prgMemoryType[firstSlot + i] = type;
		_prgMemoryAccess[firstSlot + i] = (MemoryAccessType)accessType;
	}

	SetCpuMemoryMapping(startAddr, endAddr, source+sourceOffset, accessType);
}

void BaseMapper::SetCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint8_t *source, int8_t accessType)
{
	if(!ValidateAddressRange(startAddr, endAddr)) {
		return;
	}

	startAddr >>= 8;
	endAddr >>= 8;
	for(uint16_t i = startAddr; i <= endAddr; i++) {
		_prgPages[i] = source;
		_prgMemoryAccess[i] = accessType != -1 ? (MemoryAccessType)accessType : MemoryAccessType::Read;

		source += 0x100;
	}
}

void BaseMapper::RemoveCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr)
{
	//Unmap this section of memory (causing open bus behavior)
	int firstSlot = startAddr >> 8;
	int slotCount = (endAddr - startAddr + 1) >> 8;
	for(int i = 0; i < slotCount; i++) {
		_prgMemoryOffset[firstSlot + i] = -1;
		_prgMemoryType[firstSlot + i] = PrgMemoryType::PrgRom;
		_prgMemoryAccess[firstSlot + i] = MemoryAccessType::NoAccess;
	}

	SetCpuMemoryMapping(startAddr, endAddr, nullptr, MemoryAccessType::NoAccess);
}

void BaseMapper::SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint16_t pageNumber, ChrMemoryType type, int8_t accessType)
{
	if(!ValidateAddressRange(startAddr, endAddr) || startAddr > 0x3F00 || endAddr > 0x3FFF || endAddr <= startAddr) {
		return;
	}

	uint32_t pageCount = 0;
	uint32_t pageSize = 0;
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
			defaultAccessType |= MemoryAccessType::Write;
			break;

		case ChrMemoryType::NametableRam:
			pageSize = BaseMapper::NametableSize;
			pageCount = BaseMapper::NametableCount;
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

	if((uint16_t)(endAddr - startAddr) >= pageSize) {
		#ifdef _DEBUG
		MessageManager::DisplayMessage("Debug", "Tried to map undefined chr - page size too small for selected range.");
		#endif

		uint32_t addr = startAddr;
		while(addr <= endAddr - pageSize + 1) {
			SetPpuMemoryMapping(addr, addr + pageSize - 1, type, pageNumber * pageSize, accessType);
			addr += pageSize;
			pageNumber = (pageNumber + 1) % pageCount;
		}
	} else {
		SetPpuMemoryMapping(startAddr, endAddr, type, pageNumber * pageSize, accessType == -1 ? defaultAccessType : accessType);
	}
}

void BaseMapper::SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, ChrMemoryType type, uint32_t sourceOffset, int8_t accessType)
{
	uint8_t* sourceMemory = nullptr;
	switch(type) {
		default:
		case ChrMemoryType::Default: sourceMemory = _onlyChrRam ? _chrRam : _chrRom; break;
		case ChrMemoryType::ChrRom: sourceMemory = _chrRom; break;
		case ChrMemoryType::ChrRam: sourceMemory = _chrRam; break;
		case ChrMemoryType::NametableRam: sourceMemory = _nametableRam; break;
	}
	int firstSlot = startAddr >> 8;
	int slotCount = (endAddr - startAddr + 1) >> 8;
	for(int i = 0; i < slotCount; i++) {
		_chrMemoryOffset[firstSlot + i] = sourceOffset + i * 256;
		_chrMemoryType[firstSlot + i] = type;
		_chrMemoryAccess[firstSlot + i] = (MemoryAccessType)accessType;
	}

	SetPpuMemoryMapping(startAddr, endAddr, sourceMemory + sourceOffset, accessType);
}

void BaseMapper::SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint8_t* sourceMemory, int8_t accessType)
{
	if(!ValidateAddressRange(startAddr, endAddr)) {
		return;
	}

	startAddr >>= 8;
	endAddr >>= 8;
	for(uint16_t i = startAddr; i <= endAddr; i++) {
		_chrPages[i] = sourceMemory;
		_chrMemoryAccess[i] = accessType != -1 ? (MemoryAccessType)accessType : MemoryAccessType::ReadWrite;

		if(sourceMemory != nullptr) {
			sourceMemory += 0x100;
		}
	}
}

void BaseMapper::RemovePpuMemoryMapping(uint16_t startAddr, uint16_t endAddr)
{
	//Unmap this section of memory (causing open bus behavior)
	int firstSlot = startAddr >> 8;
	int slotCount = (endAddr - startAddr + 1) >> 8;
	for(int i = 0; i < slotCount; i++) {
		_chrMemoryOffset[firstSlot + i] = -1;
		_chrMemoryType[firstSlot + i] = ChrMemoryType::Default;
		_chrMemoryAccess[firstSlot + i] = MemoryAccessType::NoAccess;
	}

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
	uint16_t pageSize;
	if(memoryType == ChrMemoryType::NametableRam) {
		pageSize = BaseMapper::NametableSize;
	} else {
		pageSize = memoryType == ChrMemoryType::ChrRam ? InternalGetChrRamPageSize() : InternalGetChrPageSize();
	}

	uint16_t startAddr = slot * pageSize;
	uint16_t endAddr = startAddr + pageSize - 1;
	SetPpuMemoryMapping(startAddr, endAddr, page, memoryType);
}

void BaseMapper::InitializeRam(void* data, uint32_t length)
{
	InitializeRam(_console->GetSettings()->GetRamPowerOnState(), data, length);
}

void BaseMapper::InitializeRam(RamPowerOnState powerOnState, void* data, uint32_t length)
{
	switch(powerOnState) {
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

uint8_t BaseMapper::GetPowerOnByte(uint8_t defaultValue)
{
	if(_console->GetSettings()->CheckFlag(EmulationFlags::RandomizeMapperPowerOnState)) {
		std::random_device rd;
		std::mt19937 mt(rd());
		std::uniform_int_distribution<> dist(0, 255);
		return dist(mt);
	} else {
		return defaultValue;
	}
}

uint32_t BaseMapper::GetDipSwitches()
{
	uint32_t mask = (1 << GetDipSwitchCount()) - 1;
	return _console->GetSettings()->GetDipSwitches() & mask;
}
		
bool BaseMapper::HasBattery()
{
	return _romInfo.HasBattery;
}

void BaseMapper::LoadBattery()
{
	if(HasBattery() && _saveRamSize > 0) {
		_console->GetBatteryManager()->LoadBattery(".sav", _saveRam, _saveRamSize);
	}

	if(_hasChrBattery && _chrRamSize > 0) {
		_console->GetBatteryManager()->LoadBattery(".sav.chr", _chrRam, _chrRamSize);
	}
}

void BaseMapper::SaveBattery()
{
	if(HasBattery() && _saveRamSize > 0) {
		_console->GetBatteryManager()->SaveBattery(".sav", _saveRam, _saveRamSize);
	}

	if(_hasChrBattery && _chrRamSize > 0) {
		_console->GetBatteryManager()->SaveBattery(".sav.chr", _chrRam, _chrRamSize);
	}
}

uint32_t BaseMapper::GetPRGPageCount()
{
	uint16_t pageSize = InternalGetPrgPageSize();
	return pageSize ? (_prgSize / pageSize) : 0;
}

uint32_t BaseMapper::GetCHRPageCount()
{
	uint16_t pageSize = InternalGetChrPageSize();
	return pageSize ? (_chrRomSize / pageSize) : 0;
}

string BaseMapper::GetBatteryFilename()
{
	return FolderUtilities::CombinePath(FolderUtilities::GetSaveFolder(), FolderUtilities::GetFilename(_romInfo.RomName, false) + ".sav");
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
		InitializeRam(_chrRam, _chrRamSize);
	}
}

void BaseMapper::SetupDefaultWorkRam()
{
	//Setup a default work/save ram in 0x6000-0x7FFF space
	if(HasBattery() && _saveRamSize > 0) {
		SetCpuMemoryMapping(0x6000, 0x7FFF, 0, PrgMemoryType::SaveRam);
	} else if(_workRamSize > 0) {
		SetCpuMemoryMapping(0x6000, 0x7FFF, 0, PrgMemoryType::WorkRam);
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
	//Need to get the number of nametables in the state first, before we try to stream the nametable ram array
	Stream(_nametableCount);

	ArrayInfo<uint8_t> chrRam = { _chrRam, _chrRamSize };
	ArrayInfo<uint8_t> workRam = { _workRam, _workRamSize };
	ArrayInfo<uint8_t> saveRam = { _saveRam, _saveRamSize };
	ArrayInfo<uint8_t> nametableRam = { _nametableRam, _nametableCount * BaseMapper::NametableSize };

	ArrayInfo<int32_t> prgMemoryOffset = { _prgMemoryOffset, 0x100 };
	ArrayInfo<int32_t> chrMemoryOffset = { _chrMemoryOffset, 0x40 };
	ArrayInfo<PrgMemoryType> prgMemoryType = { _prgMemoryType, 0x100 };
	ArrayInfo<ChrMemoryType> chrMemoryType = { _chrMemoryType, 0x40 };
	ArrayInfo<MemoryAccessType> prgMemoryAccess = { _prgMemoryAccess, 0x100 };
	ArrayInfo<MemoryAccessType> chrMemoryAccess = { _chrMemoryAccess, 0x40 };

	Stream(_mirroringType, chrRam, workRam, saveRam, nametableRam, prgMemoryOffset, chrMemoryOffset, prgMemoryType, chrMemoryType, prgMemoryAccess, chrMemoryAccess);

	if(!saving) {
		RestorePrgChrState();
	}
}

void BaseMapper::RestorePrgChrState()
{
	for(uint16_t i = 0; i < 0x100; i++) {
		uint16_t startAddr = i << 8;
		if(_prgMemoryAccess[i] != MemoryAccessType::NoAccess) {
			SetCpuMemoryMapping(startAddr, startAddr + 0xFF, _prgMemoryType[i], _prgMemoryOffset[i], _prgMemoryAccess[i]);
		} else {
			RemoveCpuMemoryMapping(startAddr, startAddr + 0xFF);
		}
	}

	for(uint16_t i = 0; i < 0x40; i++) {
		uint16_t startAddr = i << 8;
		if(_chrMemoryAccess[i] != MemoryAccessType::NoAccess) {
			SetPpuMemoryMapping(startAddr, startAddr + 0xFF, _chrMemoryType[i], _chrMemoryOffset[i], _chrMemoryAccess[i]);
		} else {
			RemovePpuMemoryMapping(startAddr, startAddr + 0xFF);
		}
	}
}

void BaseMapper::Initialize(RomData &romData)
{
	_romInfo = romData.Info;

	_batteryFilename = GetBatteryFilename();
	
	if(romData.SaveRamSize == -1 || ForceSaveRamSize()) {
		_saveRamSize = GetSaveRamSize();
	} else {
		_saveRamSize = romData.SaveRamSize;
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

	switch(romData.Info.BusConflicts) {
		case BusConflictType::Default: _hasBusConflicts = HasBusConflicts(); break;
		case BusConflictType::Yes: _hasBusConflicts = true; break;
		case BusConflictType::No: _hasBusConflicts = false; break;
	}	

	_saveRam = new uint8_t[_saveRamSize];
	_workRam = new uint8_t[_workRamSize];

	InitializeRam(_saveRam, _saveRamSize);
	InitializeRam(_workRam, _workRamSize);

	_nametableCount = 2;
	_nametableRam = new uint8_t[BaseMapper::NametableSize*BaseMapper::NametableCount];
	InitializeRam(_nametableRam, BaseMapper::NametableSize*BaseMapper::NametableCount);

	for(int i = 0; i < 0x100; i++) {
		//Allow us to map a different page every 256 bytes
		_prgPages[i] = nullptr;
		_prgMemoryOffset[i] = -1;
		_prgMemoryType[i] = PrgMemoryType::PrgRom;
		_prgMemoryAccess[i] = MemoryAccessType::NoAccess;

		_chrPages[i] = nullptr;
		_chrMemoryOffset[i] = -1;
		_chrMemoryType[i] = ChrMemoryType::Default;
		_chrMemoryAccess[i] = MemoryAccessType::NoAccess;
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

	if(romData.Info.HasTrainer) {
		if(_workRamSize >= 0x2000) {
			memcpy(_workRam + 0x1000, romData.TrainerData.data(), 512);
		} else if(_saveRamSize >= 0x2000) {
			memcpy(_saveRam + 0x1000, romData.TrainerData.data(), 512);
		}
	}

	SetupDefaultWorkRam();

	SetMirroringType(romData.Info.Mirroring);

	InitMapper();
	InitMapper(romData);

	//Load battery data if present
	LoadBattery();

	ApplyCheats();

	_romInfo.HasChrRam = HasChrRam();
}

BaseMapper::~BaseMapper()
{
	delete[] _chrRam;
	delete[] _chrRom;
	delete[] _prgRom;
	delete[] _saveRam;
	delete[] _workRam;
	delete[] _nametableRam;
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
	_console->GetCheatManager()->ApplyPrgCodes(_prgRom, _prgSize);
}

void BaseMapper::GetMemoryRanges(MemoryRanges &ranges)
{
	if(_romInfo.System == GameSystem::VsSystem) {
		ranges.AddHandler(MemoryOperation::Read, 0x6000, 0xFFFF);
		ranges.AddHandler(MemoryOperation::Write, 0x6000, 0xFFFF);
	} else {
		ranges.AddHandler(MemoryOperation::Read, 0x4018, 0xFFFF);
		ranges.AddHandler(MemoryOperation::Write, 0x4018, 0xFFFF);
	}
}

void BaseMapper::SetConsole(shared_ptr<Console> console)
{
	_console = console;
}

uint8_t* BaseMapper::GetNametable(uint8_t nametableIndex)
{
	if(nametableIndex >= BaseMapper::NametableCount) {
		#ifdef _DEBUG
		MessageManager::Log("Invalid nametable index");
		#endif
		return _nametableRam;
	}
	_nametableCount = std::max<uint8_t>(_nametableCount, nametableIndex + 1);

	return _nametableRam + (nametableIndex * BaseMapper::NametableSize);
}

void BaseMapper::SetNametable(uint8_t index, uint8_t nametableIndex)
{
	if(nametableIndex >= BaseMapper::NametableCount) {
		#ifdef _DEBUG
		MessageManager::Log("Invalid nametable index");
		#endif
		return;
	}
	_nametableCount = std::max<uint8_t>(_nametableCount, nametableIndex + 1);

	SetPpuMemoryMapping(0x2000 + index * 0x400, 0x2000 + (index + 1) * 0x400 - 1, nametableIndex, ChrMemoryType::NametableRam);
	
	//Mirror $2000-$2FFF to $3000-$3FFF, while keeping a distinction between the addresses
	//Previously, $3000-$3FFF was being "redirected" to $2000-$2FFF to avoid MMC3 IRQ issues (which is incorrect)
	//More info here: https://forums.nesdev.com/viewtopic.php?p=132145#p132145
	SetPpuMemoryMapping(0x3000 + index * 0x400, 0x3000 + (index + 1) * 0x400 - 1, nametableIndex, ChrMemoryType::NametableRam);
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

ConsoleFeatures BaseMapper::GetAvailableFeatures()
{
	return ConsoleFeatures::None;
}

shared_ptr<BaseControlDevice> BaseMapper::GetMapperControlDevice()
{
	return _mapperControlDevice;
}

RomInfo BaseMapper::GetRomInfo()
{
	return _romInfo;
}

uint32_t BaseMapper::GetMapperDipSwitchCount()
{
	return GetDipSwitchCount();
}

MirroringType BaseMapper::GetMirroringType()
{
	return _mirroringType;
}
	
uint8_t BaseMapper::ReadRAM(uint16_t addr)
{
	if(_allowRegisterRead && _isReadRegisterAddr[addr]) {
		return ReadRegister(addr);
	} else if(_prgMemoryAccess[addr >> 8] & MemoryAccessType::Read) {
		return _prgPages[addr >> 8][(uint8_t)addr];
	} else {
		//assert(false);
	}
	return _console->GetMemoryManager()->GetOpenBus();
}

uint8_t BaseMapper::PeekRAM(uint16_t addr)
{
	return DebugReadRAM(addr);
}

uint8_t BaseMapper::DebugReadRAM(uint16_t addr)
{
	if(_prgMemoryAccess[addr >> 8] & MemoryAccessType::Read) {
		return _prgPages[addr >> 8][(uint8_t)addr];
	} else {
		//assert(false);
	}
	
	//Fake open bus
	return addr >> 8;
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
	if(_prgMemoryAccess[addr >> 8] & MemoryAccessType::Write) {
		_prgPages[addr >> 8][(uint8_t)addr] = value;
	}
}

void BaseMapper::NotifyVRAMAddressChange(uint16_t addr)
{
	//This is called when the VRAM addr on the PPU memory bus changes
	//Used by MMC3/MMC5/etc
}

uint8_t BaseMapper::InternalReadVRAM(uint16_t addr)
{
	if(_chrMemoryAccess[addr >> 8] & MemoryAccessType::Read) {
		return _chrPages[addr >> 8][(uint8_t)addr];
	}

	//Open bus - "When CHR is disabled, the pattern tables are open bus. Theoretically, this should return the LSB of the address read, but real-world behavior varies."
	return _vramOpenBusValue >= 0 ? _vramOpenBusValue : addr;
}

uint8_t BaseMapper::DebugReadVRAM(uint16_t addr, bool disableSideEffects)
{
	addr &= 0x3FFF;
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
	addr &= 0x3FFF;
	if(disableSideEffects) {
		if(_chrPages[addr >> 8]) {
			//Always allow writes when side-effects are disabled
			_chrPages[addr >> 8][(uint8_t)addr] = value;
		}
	} else {
		NotifyVRAMAddressChange(addr);
		if(_chrMemoryAccess[addr >> 8] & MemoryAccessType::Write) {
			_chrPages[addr >> 8][(uint8_t)addr] = value;
		}
	}
}

void BaseMapper::WriteVRAM(uint16_t addr, uint8_t value)
{
	_console->DebugProcessVramWriteOperation(addr, value);

	if(_chrMemoryAccess[addr >> 8] & MemoryAccessType::Write) {
		_chrPages[addr >> 8][(uint8_t)addr] = value;
	}
}

bool BaseMapper::IsNes20()
{
	return _romInfo.NesHeader.GetRomHeaderVersion() == RomHeaderVersion::Nes2_0;
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
	uint32_t size = GetMemorySize(type);
	switch(type) {
		default: break;
		case DebugMemoryType::ChrRam: memcpy(buffer, _chrRam, size); break;
		case DebugMemoryType::ChrRom: memcpy(buffer, _chrRom, size); break;
		case DebugMemoryType::NametableRam: memcpy(buffer, _nametableRam, size); break;
		case DebugMemoryType::SaveRam: memcpy(buffer, _saveRam, size); break;
		case DebugMemoryType::PrgRom: memcpy(buffer, _prgRom, size); break;
		case DebugMemoryType::WorkRam: memcpy(buffer, _workRam, size); break;
	}
	return size;
}

void BaseMapper::WriteMemory(DebugMemoryType type, uint8_t* buffer, int32_t length)
{
	int32_t size = std::min(length, (int32_t)GetMemorySize(type));
	switch(type) {
		default: break;
		case DebugMemoryType::ChrRam: memcpy(_chrRam, buffer, size); break;
		case DebugMemoryType::SaveRam: memcpy(_saveRam, buffer, size); break;
		case DebugMemoryType::WorkRam: memcpy(_workRam, buffer, size); break;
		case DebugMemoryType::NametableRam: memcpy(_nametableRam, buffer, size); break;
	}
}

uint32_t BaseMapper::GetMemorySize(DebugMemoryType type)
{
	switch(type) {
		default: return 0;
		case DebugMemoryType::ChrRom: return _onlyChrRam ? 0 : _chrRomSize;
		case DebugMemoryType::ChrRam: return _chrRamSize;
		case DebugMemoryType::NametableRam: return _nametableCount * BaseMapper::NametableSize;
		case DebugMemoryType::SaveRam: return _saveRamSize;
		case DebugMemoryType::PrgRom: return _prgSize;
		case DebugMemoryType::WorkRam: return _workRamSize;
	}
}

void BaseMapper::CopyChrTile(uint32_t address, uint8_t *dest)
{
	if(_chrRamSize > 0 && address <= _chrRamSize - 16) {
		memcpy(dest, _chrRam + address, 16);
	} else if(_chrRomSize > 0 && address <= _chrRomSize - 16) {
		memcpy(dest, _chrRom + address, 16);
	}
}

uint8_t BaseMapper::GetMemoryValue(DebugMemoryType memoryType, uint32_t address)
{
	uint32_t memorySize = GetMemorySize(memoryType);
	if(memorySize > 0) {
		if(address > memorySize) {
			address %= memorySize;
		}

		switch(memoryType) {
			default: break;
			case DebugMemoryType::ChrRom: return _chrRom[address];
			case DebugMemoryType::ChrRam: return _chrRam[address];
			case DebugMemoryType::SaveRam: return _saveRam[address];
			case DebugMemoryType::PrgRom: return _prgRom[address];
			case DebugMemoryType::WorkRam: return _workRam[address];
			case DebugMemoryType::NametableRam: return _nametableRam[address];
		}
	}
	return 0;
}

void BaseMapper::SetMemoryValue(DebugMemoryType memoryType, uint32_t address, uint8_t value)
{
	uint32_t memorySize = GetMemorySize(memoryType);
	if(memorySize > 0) {
		if(address > memorySize) {
			address %= memorySize;
		}

		switch(memoryType) {
			default: break;
			case DebugMemoryType::ChrRom: _chrRom[address] = value; break;
			case DebugMemoryType::ChrRam: _chrRam[address] = value; break;
			case DebugMemoryType::SaveRam: _saveRam[address] = value; break;
			case DebugMemoryType::PrgRom: _prgRom[address] = value; break;
			case DebugMemoryType::WorkRam: _workRam[address] = value; break;
			case DebugMemoryType::NametableRam: _nametableRam[address] = value; break;
		}
	}
}

void BaseMapper::GetAbsoluteAddressAndType(uint32_t relativeAddr, AddressTypeInfo* info)
{
	if(relativeAddr < 0x2000) {
		info->Address = relativeAddr;
		info->Type = AddressType::InternalRam;
	} else {
		uint8_t *prgAddr = _prgPages[relativeAddr >> 8] + (uint8_t)relativeAddr;
		if(prgAddr >= _prgRom && prgAddr < _prgRom + _prgSize) {
			info->Address = (uint32_t)(prgAddr - _prgRom);
			info->Type = AddressType::PrgRom;
		} else if(prgAddr >= _workRam && prgAddr < _workRam + _workRamSize) {
			info->Address = (uint32_t)(prgAddr - _workRam);
			info->Type = AddressType::WorkRam;
		} else if(prgAddr >= _saveRam && prgAddr < _saveRam + _saveRamSize) {
			info->Address = (uint32_t)(prgAddr - _saveRam);
			info->Type = AddressType::SaveRam;
		} else {
			info->Address = -1;
			info->Type = AddressType::InternalRam;
		}
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

void BaseMapper::GetPpuAbsoluteAddressAndType(uint32_t relativeAddr, PpuAddressTypeInfo* info)
{
	if(relativeAddr >= 0x3F00) {
		info->Address = relativeAddr & 0x1F;
		info->Type = PpuAddressType::PaletteRam;
	} else {
		uint8_t *addr = _chrPages[relativeAddr >> 8] + (uint8_t)relativeAddr;
		if(addr >= _chrRom && addr < _chrRom + _chrRomSize) {
			info->Address = (uint32_t)(addr - _chrRom);
			info->Type = PpuAddressType::ChrRom;
		} else if(addr >= _chrRam && addr < _chrRam + _chrRamSize) {
			info->Address = (uint32_t)(addr - _chrRam);
			info->Type = PpuAddressType::ChrRam;
		} else if(addr >= _nametableRam && addr < _nametableRam + BaseMapper::NametableSize * BaseMapper::NametableCount) {
			info->Address = (uint32_t)(addr - _nametableRam);
			info->Type = PpuAddressType::NametableRam;
		} else {
			info->Address = -1;
			info->Type = PpuAddressType::None;
		}
	}
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

int32_t BaseMapper::ToAbsoluteChrRamAddress(uint16_t addr)
{
	uint8_t *chrAddr = _chrPages[addr >> 8] + (uint8_t)addr;
	if(chrAddr >= _chrRam && chrAddr < _chrRam + _chrRamSize) {
		return (uint32_t)(chrAddr - _chrRam);
	}
	return -1;
}

int32_t BaseMapper::ToAbsoluteChrRomAddress(uint16_t addr)
{
	uint8_t *chrAddr = _chrPages[addr >> 8] + (uint8_t)addr;
	if(chrAddr >= _chrRom && chrAddr < _chrRom + _chrRomSize) {
		return (uint32_t)(chrAddr - _chrRom);
	}
	return -1;
}

int32_t BaseMapper::FromAbsoluteChrAddress(uint32_t addr)
{
	uint8_t* ptrAddress = (_onlyChrRam ? _chrRam : _chrRom) + (addr & 0x3FFF);

	for(int i = 0; i < 64; i++) {
		uint8_t* pageAddress = _chrPages[i];
		if(pageAddress != nullptr && ptrAddress >= pageAddress && ptrAddress <= pageAddress + 0xFF) {
			return (i << 8) + (uint32_t)(ptrAddress - pageAddress);
		}
	}

	//Address is currently not mapped
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

int32_t BaseMapper::FromAbsolutePpuAddress(uint32_t addr, PpuAddressType type)
{
	uint8_t* ptrAddress;

	switch(type) {
		case PpuAddressType::ChrRom: ptrAddress = _chrRom; break;
		case PpuAddressType::ChrRam: ptrAddress = _chrRam; break;
		case PpuAddressType::NametableRam: ptrAddress = _nametableRam; break;
		default: return -1;
	}
	ptrAddress += addr;

	for(int i = 0; i < 0x40; i++) {
		uint8_t* pageAddress = _chrPages[i];
		if(pageAddress != nullptr && ptrAddress >= pageAddress && ptrAddress <= pageAddress + 0xFF) {
			return (i << 8) + (uint32_t)(ptrAddress - pageAddress);
		}
	}

	//Address is currently not mapped
	return -1;
}

bool BaseMapper::IsWriteRegister(uint16_t addr)
{
	return _isWriteRegisterAddr[addr];
}

bool BaseMapper::IsReadRegister(uint16_t addr)
{
	return _allowRegisterRead && _isReadRegisterAddr[addr];
}

CartridgeState BaseMapper::GetState()
{
	CartridgeState state;

	state.Mirroring = _mirroringType;
	state.HasBattery = _romInfo.HasBattery;

	state.PrgRomSize = _prgSize;
	state.ChrRomSize = _onlyChrRam ? 0 : _chrRomSize;
	state.ChrRamSize = _chrRamSize;

	state.PrgPageCount = GetPRGPageCount();
	state.PrgPageSize = InternalGetPrgPageSize();
	state.ChrPageCount = GetCHRPageCount();
	state.ChrPageSize = InternalGetChrPageSize();
	for(int i = 0; i < 0x100; i++) {
		state.PrgMemoryOffset[i] = _prgMemoryOffset[i];
		state.PrgType[i] = _prgMemoryType[i];
		state.PrgMemoryAccess[i] = _prgMemoryAccess[i];
	}
	for(int i = 0; i < 0x40; i++) {
		state.ChrMemoryOffset[i] = _chrMemoryOffset[i];
		state.ChrType[i] = _chrMemoryType[i];
		state.ChrMemoryAccess[i] = _chrMemoryAccess[i];
	}

	state.WorkRamPageSize = GetWorkRamPageSize();
	state.SaveRamPageSize = GetSaveRamPageSize();
	
	return state;
}

void BaseMapper::GetRomFileData(vector<uint8_t> &out, bool asIpsFile, uint8_t* header)
{
	if(header) {
		//Get original rom with edited header
		vector<uint8_t> originalFile;
		_console->GetRomPath().ReadFile(originalFile);

		out.insert(out.end(), header, header+sizeof(NESHeader));
		if(_romInfo.IsHeaderlessRom) {
			out.insert(out.end(), originalFile.begin(), originalFile.end());
		} else {
			out.insert(out.end(), originalFile.begin() + sizeof(NESHeader), originalFile.end());
		}
	} else {
		vector<uint8_t> newFile;
		newFile.insert(newFile.end(), (uint8_t*)&_romInfo.NesHeader, ((uint8_t*)&_romInfo.NesHeader) + sizeof(NESHeader));
		newFile.insert(newFile.end(), _prgRom, _prgRom + _prgSize);
		if(HasChrRom()) {
			newFile.insert(newFile.end(), _chrRom, _chrRom + _chrRomSize);
		}
		
		//Get edited rom
		if(asIpsFile) {
			vector<uint8_t> originalFile;
			_console->GetRomPath().ReadFile(originalFile);

			vector<uint8_t> patchData = IpsPatcher::CreatePatch(originalFile, newFile);
			out.insert(out.end(), patchData.begin(), patchData.end());
		} else {
			out.insert(out.end(), newFile.begin(), newFile.end());
		}
	}
}

vector<uint8_t> BaseMapper::GetPrgChrCopy()
{
	vector<uint8_t> data;
	data.resize(_prgSize + (_onlyChrRam ? 0 : _chrRomSize));
	memcpy(data.data(), _prgRom, _prgSize);
	if(!_onlyChrRam) {
		memcpy(data.data() + _prgSize, _chrRom, _chrRomSize);
	}
	return data;
}

void BaseMapper::RestorePrgChrBackup(vector<uint8_t> &backupData)
{
	memcpy(_prgRom, backupData.data(), _prgSize);
	if(!_onlyChrRam) {
		memcpy(_chrRom, backupData.data() + _prgSize, _chrRomSize);
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