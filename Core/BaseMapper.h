#pragma once

#include "stdafx.h"
#include "Snapshotable.h"
#include "IMemoryHandler.h"
#include "DebuggerTypes.h"
#include "Debugger.h"
#include "Types.h"
#include "IBattery.h"
#include "RomData.h"
#include "Console.h"

class BaseControlDevice;

class BaseMapper : public IMemoryHandler, public Snapshotable, public INotificationListener, public IBattery
{
private:
	MirroringType _mirroringType;
	string _batteryFilename;

	uint16_t InternalGetPrgPageSize();
	uint16_t InternalGetSaveRamPageSize();
	uint16_t InternalGetWorkRamPageSize();
	uint16_t InternalGetChrPageSize();
	uint16_t InternalGetChrRamPageSize();
	bool ValidateAddressRange(uint16_t startAddr, uint16_t endAddr);

	uint8_t *_nesNametableRam[2];
	uint8_t *_cartNametableRam[10];
	uint8_t _nametableIndexes[4];

	bool _onlyChrRam = false;
	bool _hasBusConflicts = false;
	
	bool _allowRegisterRead = false;
	bool _isReadRegisterAddr[0x10000];
	bool _isWriteRegisterAddr[0x10000];

	uint8_t* _prgPages[0x100];
	uint8_t* _chrPages[0x100];
	uint8_t _prgPageAccessType[0x100];
	uint8_t _chrPageAccessType[0x100];

	int32_t _prgMemoryOffset[0x100];
	PrgMemoryType _prgMemoryType[0x100];
	MemoryAccessType _prgMemoryAccess[0x100];

	int32_t _chrMemoryOffset[0x40];
	ChrMemoryType _chrMemoryType[0x40];
	MemoryAccessType _chrMemoryAccess[0x40];

	vector<uint8_t> _originalPrgRom;
	vector<uint8_t> _originalChrRom;

protected:
	RomInfo _romInfo;

	shared_ptr<BaseControlDevice> _mapperControlDevice;
	shared_ptr<Console> _console;

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
	bool _hasChrBattery = false;
	int16_t _vramOpenBusValue = -1;

	virtual void InitMapper() = 0;
	virtual void InitMapper(RomData &romData);
	virtual uint16_t GetPRGPageSize() = 0;
	virtual uint16_t GetCHRPageSize() = 0;

	bool IsNes20();

	virtual uint16_t GetChrRamPageSize() { return 0x2000; }

	//Save ram is battery backed and saved to disk
	virtual uint32_t GetSaveRamSize() { return HasBattery() ? 0x2000 : 0; }
	virtual uint32_t GetSaveRamPageSize() { return 0x2000; }
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
	void SetCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, PrgMemoryType type, uint32_t sourceOffset, int8_t accessType);
	void SetCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint8_t *source, int8_t accessType = -1);
	void RemoveCpuMemoryMapping(uint16_t startAddr, uint16_t endAddr);

	virtual void SelectChrPage8x(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default);
	virtual void SelectChrPage4x(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default);
	virtual void SelectChrPage2x(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default);
	virtual void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default);
	void SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint16_t pageNumber, ChrMemoryType type = ChrMemoryType::Default, int8_t accessType = -1);
	void SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, ChrMemoryType type, uint32_t sourceOffset, int8_t accessType);
	void SetPpuMemoryMapping(uint16_t startAddr, uint16_t endAddr, uint8_t* sourceMemory, int8_t accessType = -1);
	void RemovePpuMemoryMapping(uint16_t startAddr, uint16_t endAddr);

	bool HasBattery();
	void LoadBattery();
	string GetBatteryFilename();

	uint32_t GetPRGPageCount();
	uint32_t GetCHRPageCount();

	uint8_t GetPowerOnByte(uint8_t defaultValue = 0);
	
	void SetupDefaultWorkRam();

	void RestoreOriginalPrgRam();
	void InitializeChrRam(int32_t chrRamSize = -1);

	void AddRegisterRange(uint16_t startAddr, uint16_t endAddr, MemoryOperation operation = MemoryOperation::Any);
	void RemoveRegisterRange(uint16_t startAddr, uint16_t endAddr, MemoryOperation operation = MemoryOperation::Any);

	virtual void StreamState(bool saving) override;

	void RestorePrgChrState(uint32_t* prgPages, uint32_t* chrPages);

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

	virtual ConsoleFeatures GetAvailableFeatures();

	virtual void SetNesModel(NesModel model) { }
	virtual void ProcessCpuClock() { }
	virtual void NotifyVRAMAddressChange(uint16_t addr);
	void ProcessNotification(ConsoleNotificationType type, void* parameter) override; 
	virtual void GetMemoryRanges(MemoryRanges &ranges) override;
	
	void ApplyCheats();
	
	virtual void SaveBattery() override;

	void SetConsole(shared_ptr<Console> console);
	virtual void SetDefaultNametables(uint8_t* nametableA, uint8_t* nametableB);

	shared_ptr<BaseControlDevice> GetMapperControlDevice();
	RomInfo GetRomInfo();

	__forceinline uint8_t ReadRAM(uint16_t addr) override;
	uint8_t DebugReadRAM(uint16_t addr);
	virtual void WriteRAM(uint16_t addr, uint8_t value) override;
	void DebugWriteRAM(uint16_t addr, uint8_t value);
	void WritePrgRam(uint16_t addr, uint8_t value);

	__forceinline uint8_t InternalReadVRAM(uint16_t addr);
	__forceinline virtual uint8_t MapperReadVRAM(uint16_t addr, MemoryOperationType operationType);
	
	__forceinline uint8_t ReadVRAM(uint16_t addr, MemoryOperationType type = MemoryOperationType::PpuRenderingRead)
	{
		uint8_t value = MapperReadVRAM(addr, type);
		_console->DebugProcessVramReadOperation(type, addr, value);
		return value;
	}

	void DebugWriteVRAM(uint16_t addr, uint8_t value, bool disableSideEffects = true);
	void WriteVRAM(uint16_t addr, uint8_t value);

	uint8_t DebugReadVRAM(uint16_t addr, bool disableSideEffects = true);

	void InitializeRam(void* data, uint32_t length);

	void CopyChrTile(uint32_t address, uint8_t *dest);

	//Debugger Helper Functions
	bool HasChrRam();
	bool HasChrRom();

	CartridgeState GetState();
	uint8_t* GetPrgRom();
	uint8_t* GetWorkRam();
	uint8_t* GetSaveRam();
	
	uint8_t GetMemoryValue(DebugMemoryType memoryType, uint32_t address);
	void SetMemoryValue(DebugMemoryType memoryType, uint32_t address, uint8_t value);
	uint32_t GetMemorySize(DebugMemoryType type);

	uint32_t CopyMemory(DebugMemoryType type, uint8_t* buffer);
	void WriteMemory(DebugMemoryType type, uint8_t* buffer);

	void GetAbsoluteAddressAndType(uint32_t relativeAddr, AddressTypeInfo *info);
	int32_t ToAbsoluteAddress(uint16_t addr);
	int32_t ToAbsoluteSaveRamAddress(uint16_t addr);
	int32_t ToAbsoluteWorkRamAddress(uint16_t addr);
	int32_t ToAbsoluteChrAddress(uint16_t addr);
	int32_t ToAbsoluteChrRamAddress(uint16_t addr);
	int32_t ToAbsoluteChrRomAddress(uint16_t addr);
	int32_t FromAbsoluteChrAddress(uint32_t addr);
	int32_t FromAbsoluteAddress(uint32_t addr, AddressType type = AddressType::PrgRom);

	bool IsWriteRegister(uint16_t addr);
	bool IsReadRegister(uint16_t addr);

	void GetRomFileData(vector<uint8_t> &out, bool asIpsFile, uint8_t* header);

	vector<uint8_t> GetPrgChrCopy();
	void RestorePrgChrBackup(vector<uint8_t>& backupData);
	void RevertPrgChrChanges();
	bool HasPrgChrChanges();
};