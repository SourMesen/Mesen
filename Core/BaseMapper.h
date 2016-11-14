#pragma once

#include "stdafx.h"
#include "Snapshotable.h"
#include "IMemoryHandler.h"
#include "MessageManager.h"
#include "RomLoader.h"
#include "EmulationSettings.h"

enum class DebugMemoryType;

enum class PrgMemoryType
{
	PrgRom,
	SaveRam,
	WorkRam,
};

enum class ChrMemoryType
{
	Default,
	ChrRom,
	ChrRam
};

enum MemoryAccessType
{
	Unspecified = -1,
	NoAccess = 0x00,
	Read = 0x01,
	Write = 0x02,
	ReadWrite = 0x03
};

enum ChrSpecialPage
{
	NametableA = 0x7000,
	NametableB = 0x7001
};

struct CartridgeState
{
	uint32_t PrgRomSize;
	uint32_t ChrRomSize;
	uint32_t ChrRamSize;

	uint32_t PrgPageCount;
	uint32_t PrgPageSize;
	uint32_t PrgSelectedPages[64];
	uint32_t ChrPageCount;
	uint32_t ChrPageSize;
	uint32_t ChrSelectedPages[64];
	uint32_t Nametables[8];
};

class BaseMapper : public IMemoryHandler, public Snapshotable, public INotificationListener
{
private:
	const uint16_t PrgAddressRangeSize = 0x8000;
	const uint16_t ChrAddressRangeSize = 0x2000;

	MirroringType _mirroringType;
	string _batteryFilename;

	uint16_t InternalGetPrgPageSize();
	uint16_t InternalGetSaveRamPageSize();
	uint16_t InternalGetWorkRamPageSize();
	uint16_t InternalGetChrPageSize();
	uint16_t InternalGetChrRamPageSize();	

	uint8_t *_nesNametableRam[2];
	uint8_t *_cartNametableRam[10];
	uint8_t _nametableIndexes[4];

	bool _onlyChrRam = false;
	bool _hasBusConflicts = false;
	
	string _romFilename;
	string _romName;

	bool _allowRegisterRead = false;
	uint8_t _isReadRegisterAddr[0x10000];
	uint8_t _isWriteRegisterAddr[0x10000];

	vector<uint8_t*> _prgPages;
	vector<uint8_t*> _chrPages;
	vector<uint8_t> _prgPageAccessType;
	vector<uint8_t> _chrPageAccessType;

	uint32_t _prgPageNumbers[64];
	uint32_t _chrPageNumbers[64];

	uint32_t _crc32 = 0;
	uint32_t _prgCrc32 = 0;

	vector<uint8_t> _originalPrgRom;

protected:
	NESHeader _nesHeader;
	GameInfo _databaseInfo;

	uint16_t _mapperID;
	uint8_t _subMapperID;
	GameSystem _gameSystem;

	uint8_t* _prgRom = nullptr;
	uint8_t* _chrRom = nullptr;
	uint8_t* _chrRam = nullptr;
	uint32_t _prgSize = 0;
	uint32_t _chrRomSize = 0;
	uint32_t _chrRamSize = 0;

	uint8_t* _saveRam = nullptr;
	uint32_t _saveRamSize = 0;
	uint32_t _workRamSize = 0;
	uint8_t* _workRam = nullptr;
	bool _hasBattery = false;
	bool _hasChrBattery = false;

	virtual void InitMapper() = 0;
	virtual void InitMapper(RomData &romData);
	virtual uint16_t GetPRGPageSize() = 0;
	virtual uint16_t GetCHRPageSize() = 0;

	bool IsNes20();

	virtual uint16_t GetChrRamPageSize() { return 0x2000; }

	//Save ram is battery backed and saved to disk
	virtual uint32_t GetSaveRamSize() { return HasBattery() ? 0x2000 : 0; }
	virtual uint32_t GetSaveRamPageSize() { return 0x2000; }
	virtual bool ForceBattery() { return false; }
	virtual bool ForceChrBattery() { return false; }
	
	virtual bool ForceSaveRamSize() { return false; }
	virtual bool ForceWorkRamSize() { return false; }

	virtual uint32_t GetChrRamSize() { return 0x0000; }

	//Work ram is NOT saved - aka Expansion ram, etc.
	virtual uint32_t GetWorkRamSize() { return HasBattery() ? 0 : 0x2000; }
	virtual uint32_t GetWorkRamPageSize() { return 0x2000; }
	
	virtual uint16_t RegisterStartAddress() { return 0x8000; }
	virtual uint16_t RegisterEndAddress() { return 0xFFFF; }
	virtual bool AllowRegisterRead() { return false; }

	virtual bool HasBusConflicts() { return false; }

	uint8_t InternalReadRam(uint16_t addr);

	virtual void WriteRegister(uint16_t addr, uint8_t value);
	virtual uint8_t ReadRegister(uint16_t addr);

	void SelectPrgPage4x(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom);
	void SelectPrgPage2x(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom);
	virtual void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom);
	void SetCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, int16_t pageNumber, PrgMemoryType type, int8_t accessType = -1);
	void SetCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint8_t *source, int8_t accessType = -1);
	void RemoveCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr);

	virtual void SelectChrPage8x(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default);
	virtual void SelectChrPage4x(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default);
	virtual void SelectChrPage2x(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default);
	virtual void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default);
	void SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint16_t pageNumber, ChrMemoryType type = ChrMemoryType::Default, int8_t accessType = -1);
	void SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint8_t* sourceMemory, int8_t accessType = -1);
	void RemovePpuMemoryMapping(uint16_t startAddr, uint16_t endAddr);

	bool HasBattery();
	void LoadBattery();
	string GetBatteryFilename();

	uint32_t GetPRGPageCount();
	uint32_t GetCHRPageCount();

	void RestoreOriginalPrgRam();
	void InitializeChrRam(int32_t chrRamSize = -1);
	bool HasChrRam();
	bool HasChrRom();

	void AddRegisterRange(uint16_t startAddr, uint16_t endAddr, MemoryOperation operation = MemoryOperation::Any);
	void RemoveRegisterRange(uint16_t startAddr, uint16_t endAddr, MemoryOperation operation = MemoryOperation::Any);

	virtual void StreamState(bool saving);

	uint8_t* GetNametable(uint8_t index);
	void AddNametable(uint8_t index, uint8_t *nametable);
	void SetNametable(uint8_t index, uint8_t nametableIndex);
	void SetNametables(uint8_t nametable1Index, uint8_t nametable2Index, uint8_t nametable3Index, uint8_t nametable4Index);
	void SetMirroringType(MirroringType type);
	MirroringType GetMirroringType();

public:
	void Initialize(RomData &romData);

	virtual ~BaseMapper();
	virtual void Reset(bool softReset);

	virtual void SetNesModel(NesModel model) { }
	virtual void ProcessCpuClock() { }
	virtual void NotifyVRAMAddressChange(uint16_t addr);
	void ProcessNotification(ConsoleNotificationType type, void* parameter);
	virtual void GetMemoryRanges(MemoryRanges &ranges);
	
	void ApplyCheats();
	void SaveBattery();

	virtual void SetDefaultNametables(uint8_t* nametableA, uint8_t* nametableB);

	GameSystem GetGameSystem();
	uint32_t GetCrc32();
	uint32_t GetPrgCrc32();
	string GetRomName();

	uint8_t ReadRAM(uint16_t addr);
	virtual void WriteRAM(uint16_t addr, uint8_t value);
	void WritePrgRam(uint16_t addr, uint8_t value);

	uint8_t InternalReadVRAM(uint16_t addr);	
	virtual uint8_t ReadVRAM(uint16_t addr, MemoryOperationType type = MemoryOperationType::Read);
	void WriteVRAM(uint16_t addr, uint8_t value);

	void InitializeRam(void* data, uint32_t length);

	//Debugger Helper Functions
	CartridgeState GetState();
	uint8_t* GetPrgRom();
	uint8_t* GetWorkRam();
	uint32_t GetPrgSize(bool getWorkRamSize = false);
	uint32_t GetChrSize(bool getRamSize = false);
	uint32_t CopyMemory(DebugMemoryType type, uint8_t* buffer);
	void WriteMemory(DebugMemoryType type, uint8_t* buffer);
	int32_t ToAbsoluteAddress(uint16_t addr);
	int32_t ToAbsoluteRamAddress(uint16_t addr);
	int32_t ToAbsoluteChrAddress(uint16_t addr);
	int32_t FromAbsoluteAddress(uint32_t addr);
};