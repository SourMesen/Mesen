#pragma once
#include "stdafx.h"
#include <assert.h>
#include "BaseMapper.h"
#include "CPU.h"
#include "EmulationSettings.h"
#include "FdsLoader.h"
#include "Console.h"

class FdsAudio;

class FDS : public BaseMapper
{
private:
	static const uint32_t NoDiskInserted = 0xFF;
	bool _disableAutoInsertDisk;

	unique_ptr<FdsAudio> _audio;

	//Write registers
	uint16_t _irqReloadValue = 0;
	uint16_t _irqCounter = 0;
	bool _irqEnabled = false;
	bool _irqRepeatEnabled = false;

	bool _diskRegEnabled = true;
	bool _soundRegEnabled = true;

	uint8_t _writeDataReg = 0;
	
	bool _motorOn = false;
	bool _resetTransfer = false;
	bool _readMode = false;
	bool _crcControl = false;
	bool _diskReady = false;
	bool _diskIrqEnabled = false;

	int32_t _autoDiskEjectCounter = -1;
	int32_t _autoDiskSwitchCounter = -1;

	uint8_t _extConWriteReg = 0;

	//Read registers
	bool _badCrc = false;
	bool _endOfHead = false;
	bool _readWriteEnabled = false;

	uint8_t _readDataReg = 0;

	bool _diskWriteProtected = false;

	//Internal values
	uint32_t _diskNumber = FDS::NoDiskInserted;
	uint32_t _diskPosition = 0;
	uint32_t _delay = 0;	
	uint16_t _crcAccumulator;
	bool _previousCrcControlFlag = false;
	bool _gapEnded = true;
	bool _scanningDisk = false;
	bool _transferComplete = false;
	
	vector<uint8_t> _fdsRawData;
	vector<vector<uint8_t>> _fdsDiskSides;
	vector<vector<uint8_t>> _fdsDiskHeaders;
	string _romFilepath;

	bool _gameStarted;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x2000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }
	virtual uint32_t GetWorkRamPageSize() override { return 0x8000; }
	virtual uint32_t GetWorkRamSize() override { return 0x8000; }
	uint16_t RegisterStartAddress() override { return 0x4020; }
	uint16_t RegisterEndAddress() override { return 0x4092; }
	bool AllowRegisterRead() override { return true; }

	void InitMapper() override;
	void InitMapper(RomData &romData) override;
	void LoadDiskData(vector<uint8_t> ipsData = vector<uint8_t>());
	vector<uint8_t> CreateIpsPatch();
	void Reset(bool softReset) override;

	uint32_t GetFdsDiskSideSize(uint8_t side);
	uint8_t ReadFdsDisk();
	void WriteFdsDisk(uint8_t value);

	void ClockIrq();
	
	void ProcessCpuClock() override;
	void UpdateCrc(uint8_t value);

	void WriteRegister(uint16_t addr, uint8_t value) override;
	uint8_t ReadRegister(uint16_t addr) override;

	uint8_t ReadRAM(uint16_t addr) override;

	void StreamState(bool saving) override;

public:
	FDS();
	~FDS();

	void SaveBattery() override;
	ConsoleFeatures GetAvailableFeatures() override;

	uint32_t GetSideCount();

	void EjectDisk();
	void InsertDisk(uint32_t diskNumber);
	uint32_t GetCurrentDisk();
	bool IsDiskInserted();

	bool IsAutoInsertDiskEnabled();
};