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
	static const uint32_t DiskInsertDelay = 3600000; //approx 2 sec delay
	
	static FDS* Instance;

	unique_ptr<FdsAudio> _audio;

	//Write registers
	uint16_t _irqReloadValue = 0;
	uint16_t _irqCounter = 0;
	bool _irqEnabled = false;
	bool _irqReloadEnabled = false;

	bool _diskRegEnabled = true;
	bool _soundRegEnabled = true;

	uint8_t _writeDataReg = 0;
	
	bool _motorOn = false;
	bool _resetTransfer = false;
	bool _readMode = false;
	bool _crcControl = false;
	bool _diskReady = false;
	bool _diskIrqEnabled = false;

	uint8_t _extConWriteReg = 0;

	//Read registers
	bool _badCrc = false;
	bool _endOfHead = false;
	bool _readWriteEnabled = false;

	uint8_t _readDataReg = 0;

	bool _diskWriteProtected = false;

	//Internal values
	uint32_t _diskNumber = 0;
	uint32_t _newDiskNumber = 0;
	uint32_t _newDiskInsertDelay = 0;
	uint32_t _diskPosition = 0;
	uint32_t _delay = 0;	
	uint16_t _crcAccumulator;
	bool _previousCrcControlFlag = false;
	bool _gapEnded = true;
	bool _scanningDisk = false;
	bool _needIrq = false;
	bool _transferComplete = false;
	bool _isDirty = false;
	
	vector<uint8_t> _fdsRawData;
	vector<vector<uint8_t>> _fdsDiskSides;
	string _romFilepath;

	uint8_t _gameStarted = 0;
	int32_t _previousSpeed = -1;
	bool _fastForwarding = false;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	virtual uint32_t GetWorkRamPageSize() { return 0x8000; }
	virtual uint32_t GetWorkRamSize() { return 0x8000; }
	uint16_t RegisterStartAddress() { return 0x4020; }
	uint16_t RegisterEndAddress() { return 0x4092; }
	bool AllowRegisterRead() { return true; }

	void InitMapper();
	void InitMapper(RomData &romData);

	uint32_t GetFdsDiskSideSize(uint8_t side);
	uint8_t ReadFdsDisk();
	void WriteFdsDisk(uint8_t value);

	void ClockIrq();

	void ProcessCpuClock();
	void UpdateCrc(uint8_t value);

	bool IsDiskInserted();

	void WriteRegister(uint16_t addr, uint8_t value);
	uint8_t ReadRegister(uint16_t addr);

	uint8_t ReadRAM(uint16_t addr);

	void StreamState(bool saving);

public:
	FDS();
	~FDS();

	static uint32_t GetSideCount();

	static void InsertDisk(uint32_t diskNumber);
	static void SwitchDiskSide();
	static void EjectDisk();
};