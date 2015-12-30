#pragma once

#include "stdafx.h"
#include "Snapshotable.h"
#include "IMemoryHandler.h"
#include "ROMLoader.h"
#include <assert.h>
#include "../Utilities/FolderUtilities.h"
#include "CheatManager.h"
#include "MessageManager.h"

enum class PrgMemoryType
{
	PrgRom,
	SaveRam,
	WorkRam,
};

enum MemoryAccessType
{
	NoAccess = 0x00,
	Read = 0x01,
	Write = 0x02,
	ReadWrite = 0x03
};

class BaseMapper : public IMemoryHandler, public Snapshotable, public INotificationListener
{
	private:
		const uint16_t PrgAddressRangeSize = 0x8000;
		const uint16_t ChrAddressRangeSize = 0x2000;

		MirroringType _mirroringType;
		string _batteryFilename;
		
		uint16_t InternalGetPrgPageSize()
		{
			//Make sure the page size is no bigger than the size of the ROM itself
			//Otherwise we will end up reading from unallocated memory
			return std::min((uint32_t)GetPRGPageSize(), _prgSize);
		}

		uint16_t InternalGetChrPageSize()
		{
			//Make sure the page size is no bigger than the size of the ROM itself
			//Otherwise we will end up reading from unallocated memory
			return std::min((uint32_t)GetCHRPageSize(), _chrSize);
		}

	protected:
		uint8_t* _prgRom;
		uint8_t* _originalPrgRom;
		uint8_t* _chrRam;
		uint32_t _prgSize;
		uint32_t _chrSize;

		uint8_t* _saveRam;
		uint32_t _saveRamSize;
		uint8_t* _workRam;

		uint8_t *_nesNametableRam[2];
		uint8_t *_cartNametableRam[2];

		bool _hasChrRam;
		bool _hasBattery;
		bool _isPalRom;
		string _romFilename;
		bool _allowRegisterRead;
		uint16_t _registerStartAddress;
		uint16_t _registerEndAddress;

		vector<uint8_t*> _prgPages;
		vector<uint8_t*> _chrPages;
		vector<uint8_t> _prgPageAccessType;
		vector<uint8_t> _chrPageAccessType;

		uint32_t _prgPageNumbers[64];
		uint32_t _chrPageNumbers[64];

		virtual void InitMapper() = 0;

	protected:
		virtual uint16_t GetPRGPageSize() = 0;
		virtual uint16_t GetCHRPageSize() = 0;
		
		//Save ram is battery backed and saved to disk
		virtual uint32_t GetSaveRamSize() { return 0x2000; }
		virtual uint32_t GetSaveRamPageSize() { return 0x2000; }
		virtual bool ForceBattery() { return false; }

		virtual uint32_t GetChrRamSize() { return 0x2000; }

		//Work ram is NOT saved - aka Expansion ram, etc.
		virtual uint32_t GetWorkRamPageSize() { return 0x2000; }
		virtual uint32_t GetWorkRamSize() { return 0x2000; }

		virtual uint16_t RegisterStartAddress() { return 0x8000; }
		virtual uint16_t RegisterEndAddress() { return 0xFFFF; }
		virtual bool AllowRegisterRead() { return false; }

		virtual void WriteRegister(uint16_t addr, uint8_t value) { }
		virtual uint8_t ReadRegister(uint16_t addr) { return 0;  }

		void SetCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint16_t pageNumber, PrgMemoryType type, int8_t accessType = -1)
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
					pageCount = _saveRamSize / GetSaveRamPageSize();
					pageSize = GetSaveRamPageSize();
					defaultAccessType |= MemoryAccessType::Write;
					break;
				case PrgMemoryType::WorkRam:
					source = _workRam;
					pageCount = GetWorkRamSize() / GetWorkRamPageSize();
					pageSize = GetWorkRamPageSize();
					defaultAccessType |= MemoryAccessType::Write;
					break;
				default:
					throw new std::runtime_error("Invalid parameter");
			}

			pageNumber = pageNumber % pageCount;
			source = &source[pageNumber * pageSize];

			startAddr >>= 8;
			endAddr >>= 8;
			for(uint16_t i = startAddr; i <= endAddr; i++) {
				_prgPages[i] = source;
				_prgPageAccessType[i] = accessType != -1 ? accessType : defaultAccessType;
				source += 0x100;
			}
		}

		void SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint16_t pageNumber, int8_t accessType = -1)
		{
			pageNumber = pageNumber % GetCHRPageCount();
			SetPpuMemoryMapping(startAddr, endAddr, &_chrRam[pageNumber * InternalGetChrPageSize()], accessType);
		}

		void SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint8_t* sourceMemory, int8_t accessType = -1)
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
				sourceMemory += 0x100;
			}
		}

		uint8_t InternalReadRam(uint16_t addr)
		{
			return _prgPages[addr >> 8] ? _prgPages[addr >> 8][addr & 0xFF] : 0;
		}

		void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom)
		{
			_prgPageNumbers[slot] = page;

			if(_prgSize < PrgAddressRangeSize) {
				//Total PRG size is smaller than available memory range, map the entire PRG to all slots
				//i.e same logic as NROM (mapper 0) when PRG is 16kb
				//Needed by "Pyramid" (mapper 79)
				#ifdef _DEBUG
					MessageManager::DisplayMessage("Debug", "PRG size is smaller than 32kb");
				#endif

				for(slot = 0; slot < PrgAddressRangeSize / _prgSize; slot++) {
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

		void SelectCHRPage(uint16_t slot, uint16_t page)
		{
			_chrPageNumbers[slot] = page;

			if(_chrSize < ChrAddressRangeSize) {
				//Total CHR size is smaller than available memory range, map the entire CHR to all slots
				//Unsure if any game needs this, but assuming same behavior as PRG in similar situations
				#ifdef _DEBUG
					MessageManager::DisplayMessage("Debug", "CHR size is smaller than 8kb");
				#endif

				for(slot = 0; slot < ChrAddressRangeSize / _chrSize; slot++) {
					uint16_t startAddr = slot * _chrSize;
					uint16_t endAddr = startAddr + _chrSize - 1;
					SetPpuMemoryMapping(startAddr, endAddr, (uint16_t)0, _hasChrRam ? MemoryAccessType::ReadWrite : MemoryAccessType::Read);
				}
			} else {
				uint16_t startAddr = slot * InternalGetChrPageSize();
				uint16_t endAddr = startAddr + InternalGetChrPageSize() - 1;
				SetPpuMemoryMapping(startAddr, endAddr, page, _hasChrRam ? MemoryAccessType::ReadWrite : MemoryAccessType::Read);
			}
		}
		
		bool HasBattery()
		{
			return _hasBattery;
		}

		void LoadBattery()
		{
			ifstream batteryFile(_batteryFilename, ios::in | ios::binary);

			if(batteryFile) {
				batteryFile.read((char*)_saveRam, _saveRamSize);

				batteryFile.close();
			}

			//Set a default mapping for save ram (this is what most games/mappers use)
			SetCpuMemoryMapping(0x6000, 0x7FFF, 0, PrgMemoryType::SaveRam);
		}

		void SaveBattery()
		{
			ofstream batteryFile(_batteryFilename, ios::out | ios::binary);

			if(batteryFile) {
				batteryFile.write((char*)_saveRam, _saveRamSize);

				batteryFile.close();
			}
		}

		uint32_t GetPRGPageCount()
		{
			return _prgSize / InternalGetPrgPageSize();
		}

		uint32_t GetCHRPageCount()
		{
			return _chrSize / InternalGetChrPageSize();
		}

		string GetBatteryFilename()
		{
			return FolderUtilities::GetSaveFolder() + _romFilename + ".sav";
		}
		
		void RestoreOriginalPrgRam()
		{
			memcpy(_prgRom, _originalPrgRom, GetPrgSize());
		}

		virtual void StreamState(bool saving)
		{
			Stream<bool>(_hasChrRam);
			if(_hasChrRam) {
				StreamArray<uint8_t>(_chrRam, _chrSize);
			}

			Stream<MirroringType>(_mirroringType);

			StreamArray<uint8_t>(_workRam, GetWorkRamSize());
			StreamArray<uint8_t>(_saveRam, _saveRamSize);

			StreamArray<uint32_t>(_prgPageNumbers, 64);
			StreamArray<uint32_t>(_chrPageNumbers, 64);
			if(!saving) {
				for(uint16_t i = 0; i < 64; i++) {
					if(_prgPageNumbers[i] != 0xEEEEEEEE) {
						SelectPRGPage(i, (uint16_t)_prgPageNumbers[i]);
					}
				}

				for(uint16_t i = 0; i < 64; i++) {
					if(_chrPageNumbers[i] != 0xEEEEEEEE) {
						SelectCHRPage(i, (uint16_t)_chrPageNumbers[i]);
					}
				}

				if(_mirroringType != MirroringType::Custom) {
					SetMirroringType(_mirroringType);
				}
			}
		}

	public:
		void Initialize(ROMLoader &romLoader)
		{
			_romFilename = romLoader.GetFilename();
			_batteryFilename = GetBatteryFilename();
			_saveRamSize = GetSaveRamSize(); //Needed because we need to call SaveBattery() in the destructor (and calling virtual functions in the destructor doesn't work correctly)

			_allowRegisterRead = AllowRegisterRead();
			_registerStartAddress = RegisterStartAddress();
			_registerEndAddress = RegisterEndAddress();

			_mirroringType = romLoader.GetMirroringType();
			romLoader.GetPRGRam(&_prgRom);
			romLoader.GetPRGRam(&_originalPrgRom);
			romLoader.GetCHRRam(&_chrRam);
			_prgSize = romLoader.GetPRGSize();
			_chrSize = romLoader.GetCHRSize();
			_hasBattery = romLoader.HasBattery() || ForceBattery();
			_isPalRom = romLoader.IsPalRom();

			_saveRam = new uint8_t[_saveRamSize];
			_workRam = new uint8_t[GetWorkRamSize()];

			memset(_saveRam, 0, _saveRamSize);
			memset(_workRam, 0, GetWorkRamSize());

			memset(_prgPageNumbers, 0xEE, sizeof(_prgPageNumbers));
			memset(_chrPageNumbers, 0xEE, sizeof(_chrPageNumbers));

			for(int i = 0; i <= 0xFF; i++) {
				//Allow us to map a different page every 256 bytes
				_prgPages.push_back(nullptr);
				_prgPageAccessType.push_back(MemoryAccessType::NoAccess);
				_chrPages.push_back(nullptr);
				_chrPageAccessType.push_back(MemoryAccessType::NoAccess);
			}

			//Load battery data if present
			if(HasBattery()) {
				LoadBattery();
			}

			if(_chrSize == 0) {
				_hasChrRam = true;
				_chrRam = new uint8_t[GetChrRamSize()];
				_chrSize = GetChrRamSize();
			}

			//Setup a default work/save ram in 0x6000-0x7FFF space
			SetCpuMemoryMapping(0x6000, 0x7FFF, 0, HasBattery() ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam);

			InitMapper();

			MessageManager::RegisterNotificationListener(this);

			ApplyCheats();
		}

		virtual ~BaseMapper()
		{
			if(HasBattery()) {
				SaveBattery();
			}
			delete[] _chrRam;
			delete[] _prgRom;
			delete[] _originalPrgRom;
			delete[] _saveRam;
			delete[] _workRam;

			MessageManager::UnregisterNotificationListener(this);
		}

		virtual void ProcessCpuClock() { }

		void ProcessNotification(ConsoleNotificationType type, void* parameter)
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

		void ApplyCheats()
		{
			RestoreOriginalPrgRam();
			CheatManager::ApplyPrgCodes(_prgRom, GetPrgSize());
		}

		void GetMemoryRanges(MemoryRanges &ranges)
		{
			ranges.AddHandler(MemoryOperation::Read, 0x4018, 0xFFFF);
			ranges.AddHandler(MemoryOperation::Write, 0x4018, 0xFFFF);
		}

		void SetDefaultNametables(uint8_t* nametableA, uint8_t* nametableB)
		{
			_nesNametableRam[0] = nametableA;
			_nesNametableRam[1] = nametableB;
			SetMirroringType(_mirroringType);
		}

		void SetMirroringType(MirroringType type)
		{
			switch(type) {
				case MirroringType::Vertical:
					SetMirroringType(_nesNametableRam[0], _nesNametableRam[1], _nesNametableRam[0], _nesNametableRam[1]);
					break;

				case MirroringType::Horizontal:
					SetMirroringType(_nesNametableRam[0], _nesNametableRam[0], _nesNametableRam[1], _nesNametableRam[1]);
					break;

				case MirroringType::FourScreens:
				default:
					SetMirroringType(_nesNametableRam[0], _nesNametableRam[1], _cartNametableRam[0], _cartNametableRam[1]);
					break;

				case MirroringType::ScreenAOnly:
					SetMirroringType(_nesNametableRam[0], _nesNametableRam[0], _nesNametableRam[0], _nesNametableRam[0]);
					break;

				case MirroringType::ScreenBOnly:
					SetMirroringType(_nesNametableRam[1], _nesNametableRam[1], _nesNametableRam[1], _nesNametableRam[1]);
					break;
			}
			_mirroringType = type;
		}

		void SetMirroringType(uint8_t *nt0, uint8_t *nt1, uint8_t *nt2, uint8_t *nt3)
		{
			_mirroringType = MirroringType::Custom;

			SetPpuMemoryMapping(0x2000, 0x23FF, nt0);
			SetPpuMemoryMapping(0x2400, 0x27FF, nt1);
			SetPpuMemoryMapping(0x2800, 0x2BFF, nt2);
			SetPpuMemoryMapping(0x2C00, 0x2FFF, nt3);
		}

		bool IsPalRom()
		{
			return _isPalRom;
		}

		MirroringType GetMirroringType()
		{
			return _mirroringType;
		}
	
		uint8_t ReadRAM(uint16_t addr)
		{
			if(_allowRegisterRead && addr >= _registerStartAddress && addr <= _registerEndAddress) {
				return ReadRegister(addr);
			} else if(_prgPageAccessType[addr >> 8] & MemoryAccessType::Read) {
				return _prgPages[addr >> 8][addr & 0xFF];
			} else {
				//assert(false);
			}
			return 0;
		}

		virtual void WriteRAM(uint16_t addr, uint8_t value)
		{
			if(addr >= _registerStartAddress && addr <= _registerEndAddress) {
				WriteRegister(addr, value);
			} else if(_prgPageAccessType[addr >> 8] & MemoryAccessType::Write) {
				_prgPages[addr >> 8][addr & 0xFF] = value;
			} else {
				//assert(false);
			}
		}
		
		virtual uint8_t ReadVRAM(uint16_t addr)
		{
			if(_chrPageAccessType[addr >> 8] & MemoryAccessType::Read) {
				return _chrPages[addr >> 8][addr & 0xFF];
			} else {
				//assert(false);
			}
			return 0;
		}

		void WriteVRAM(uint16_t addr, uint8_t value)
		{
			if(_chrPageAccessType[addr >> 8] & MemoryAccessType::Write) {
				_chrPages[addr >> 8][addr & 0xFF] = value;
			} else {
				//assert(false);
			}
		}

		virtual void NotifyVRAMAddressChange(uint16_t addr)
		{
			//This is called when the VRAM addr on the PPU memory bus changes
			//Used by MMC3/MMC5/etc
		}

		//Debugger Helper Functions
		void GetPrgCopy(uint8_t **buffer)
		{
			*buffer = new uint8_t[_prgSize];
			memcpy(*buffer, _prgRom, _prgSize);
		}

		uint32_t GetPrgSize()
		{
			return _prgSize;
		}

		void GetChrCopy(uint8_t **buffer)
		{
			*buffer = new uint8_t[_chrSize];
			memcpy(*buffer, _chrRam, _chrSize);
		}

		uint32_t GetChrSize(bool includeChrRam = true)
		{
			if(includeChrRam || !_hasChrRam) {
				return _chrSize;
			} else {
				return 0;
			}
		}

		int32_t ToAbsoluteAddress(uint16_t addr)
		{
			uint8_t *prgAddr = _prgPages[addr >> 8] + (addr & 0xFF);
			if(prgAddr >= _prgRom && prgAddr < _prgRom + _prgSize) {
				return (uint32_t)(prgAddr - _prgRom);
			}
			return -1;
		}

		int32_t ToAbsoluteChrAddress(uint16_t addr)
		{
			uint8_t *chrAddr = _chrPages[addr >> 8] + (addr & 0xFF);
			if(chrAddr >= _chrRam && chrAddr < _chrRam + _chrSize) {
				return (uint32_t)(chrAddr - _chrRam);
			}
			return -1;
		}

		int32_t FromAbsoluteAddress(uint32_t addr)
		{
			uint8_t* ptrAddress = _prgRom + addr;

			for(int i = 0; i < 256; i++) {
				uint8_t* pageAddress = _prgPages[i];
				if(pageAddress != nullptr && ptrAddress >= pageAddress && ptrAddress <= pageAddress + 0xFF) {
					return (i << 8) + (uint32_t)(ptrAddress - pageAddress);
				}
			}
			
			//Address is currently not mapped
			return -1;
		}

		vector<int32_t> GetPRGRanges()
		{
			vector<int32_t> memoryRanges;

			for(uint32_t i = 0x8000; i <= 0xFFFF; i+=0x100) {
				int32_t pageStart = ToAbsoluteAddress((uint16_t)i);
				int32_t pageEnd = ToAbsoluteAddress((uint16_t)i+0xFF);
				memoryRanges.push_back(pageStart);
				memoryRanges.push_back(pageEnd);
			}

			return memoryRanges;
		}
};