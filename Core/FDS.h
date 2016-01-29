#pragma once
#include "stdafx.h"
#include <assert.h>
#include "BaseMapper.h"
#include "CPU.h"
#include "EmulationSettings.h"
#include "FdsLoader.h"
#include "Console.h"

class FDS : public BaseMapper
{
private:
	static const uint32_t NoDiskInserted = 0xFF;
	static const uint32_t DiskInsertDelay = 3600000; //approx 2 sec delay
	
	static FDS* Instance;

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

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x2000; }
	virtual uint32_t GetWorkRamPageSize() { return 0x8000; }
	virtual uint32_t GetWorkRamSize() { return 0x8000; }
	uint16_t RegisterStartAddress() { return 0x4020; }
	uint16_t RegisterEndAddress() { return 0x4092; }
	bool AllowRegisterRead() { return true; }

	void InitMapper()
	{
		_diskNumber = EmulationSettings::CheckFlag(EmulationFlags::FdsAutoLoadDisk) ? 0 : FDS::NoDiskInserted;

		//FDS BIOS
		SetCpuMemoryMapping(0xE000, 0xFFFF, 0, PrgMemoryType::PrgRom, MemoryAccessType::Read);

		//Work RAM
		SetCpuMemoryMapping(0x6000, 0xDFFF, 0, PrgMemoryType::WorkRam, MemoryAccessType::ReadWrite);

		//8k of CHR RAM
		SelectCHRPage(0, 0);		
	}

	void InitMapper(RomData &romData)
	{
		_romFilepath = romData.Filename;
		_fdsDiskSides = romData.FdsDiskData;
		_fdsRawData = romData.RawData;
	}

	uint32_t GetFdsDiskSideSize(uint8_t side)
	{
		assert(side < _fdsDiskSides.size());
		return (uint32_t)_fdsDiskSides[side].size();
	}

	uint8_t ReadFdsDisk()
	{
		assert(_diskNumber < _fdsDiskSides.size());
		assert(_diskPosition < _fdsDiskSides[_diskNumber].size());
		return _fdsDiskSides[_diskNumber][_diskPosition];
	}

	void WriteFdsDisk(uint8_t value)
	{
		assert(_diskNumber < _fdsDiskSides.size());
		assert(_diskPosition < _fdsDiskSides[_diskNumber].size());
		if(_fdsDiskSides[_diskNumber][_diskPosition - 2] != value) {
			_isDirty = true;
		}
		_fdsDiskSides[_diskNumber][_diskPosition - 2] = value;
	}

	void ClockIrq()
	{
		if(_needIrq) {
			CPU::SetIRQSource(IRQSource::External);
			_needIrq = false;
		}

		if(_irqEnabled && _irqCounter > 0) {
			_irqCounter--;
			if(_irqCounter == 0) {
				_needIrq = true;
				if(_irqReloadEnabled) {
					_irqCounter = _irqReloadValue;
				} else {
					_irqEnabled = false;
					_irqReloadValue = 0;
				}
			}
		}
	}

	void ProcessCpuClock()
	{
		ClockIrq();

		if(EmulationSettings::CheckFlag(EmulationFlags::FdsFastForwardOnLoad)) {
			EmulationSettings::SetEmulationSpeed(_scanningDisk ? 0 : 100);
		}

		if(_newDiskInsertDelay > 0) {
			//Insert new disk after delay expires, to allow games to notice the disk was ejected
			_newDiskInsertDelay--;
			_diskNumber = FDS::NoDiskInserted;
		} else {
			_diskNumber = _newDiskNumber;
		}

		if(_diskNumber == FDS::NoDiskInserted || !_motorOn) {
			//Disk has been ejected
			_endOfHead = true;
			_scanningDisk = false;
			return;
		}

		if(_resetTransfer && !_scanningDisk) {
			return;
		}

		if(_endOfHead) {
			_delay = 50000;
			_endOfHead = false;
			_diskPosition = 0;
			_gapEnded = false;
			return;
		} 
		
		if(_delay > 0) {
			_delay--;
		} else {
			_scanningDisk = true;

			uint8_t diskData = 0;
			bool needIrq = _diskIrqEnabled;

			if(_readMode) {
				diskData = ReadFdsDisk();

				if(!_previousCrcControlFlag) {
					UpdateCrc(diskData);
				}

				if(!_diskReady) {
					_gapEnded = false;
					_crcAccumulator = 0;
				} else if(diskData && !_gapEnded) {
					_gapEnded = true;
					needIrq = false;
				}

				if(_gapEnded) {
					_transferComplete = true;
					_readDataReg = diskData;
					if(needIrq) {
						CPU::SetIRQSource(IRQSource::FdsDisk);
					}
				}
			} else {
				if(!_crcControl) {
					_transferComplete = true;
					diskData = _writeDataReg;
					if(needIrq) {
						CPU::SetIRQSource(IRQSource::FdsDisk);
					}
				}

				if(!_diskReady) {
					diskData = 0x00;
				}

				if(!_crcControl) {
					UpdateCrc(diskData);
				} else {
					if(!_previousCrcControlFlag) {
						//Finish CRC calculation
						UpdateCrc(0x00);
						UpdateCrc(0x00);
					}
					diskData = _crcAccumulator & 0xFF;
					_crcAccumulator >>= 8;
				}

				WriteFdsDisk(diskData);
				_gapEnded = false;
			}

			_previousCrcControlFlag = _crcControl;
				
			_diskPosition++;
			if(_diskPosition >= GetFdsDiskSideSize(_diskNumber)) {
				_motorOn = false;
			} else {
				_delay = 150;
			}
		}
	}

	void UpdateCrc(uint8_t value)
	{
		for(uint16_t n = 0x01; n <= 0x80; n <<= 1) {
			uint8_t carry = (_crcAccumulator & 1);
			_crcAccumulator >>= 1;
			if(carry) {
				_crcAccumulator ^= 0x8408;
			}

			if(value & n) {
				_crcAccumulator ^= 0x8000;
			}
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(!_diskRegEnabled && addr >= 0x4024 && addr <= 0x4026) {
			return;
		}

		switch(addr) {
			case 0x4020: 
				_irqReloadValue = (_irqReloadValue & 0xFF00) | value; 
				CPU::ClearIRQSource(IRQSource::External);
				break;

			case 0x4021: 
				_irqReloadValue = (_irqReloadValue & 0x00FF) | (value << 8); 
				CPU::ClearIRQSource(IRQSource::External);
				break;

			case 0x4022: 
				_irqReloadEnabled = (value & 0x01) == 0x01;
				_irqEnabled = (value & 0x02) == 0x02; 
				_irqCounter = _irqReloadValue;
				CPU::ClearIRQSource(IRQSource::External);
				break;

			case 0x4023: 
				_diskRegEnabled = (value & 0x01) == 0x01;
				_soundRegEnabled = (value & 0x02) == 0x02;
				break;

			case 0x4024: 
				_writeDataReg = value;
				_transferComplete = false;
				CPU::ClearIRQSource(IRQSource::FdsDisk);
				break;

			case 0x4025: 
				_motorOn = (value & 0x01) == 0x01;
				_resetTransfer = (value & 0x02) == 0x02;
				_readMode = (value & 0x04) == 0x04;
				SetMirroringType(value & 0x08 ? MirroringType::Horizontal : MirroringType::Vertical);
				_crcControl = (value & 0x10) == 0x10;
				//Bit 6 is not used, always 1
				_diskReady = (value & 0x40) == 0x40;
				_diskIrqEnabled = (value & 0x80) == 0x80;
				break;

			case 0x4026: 
				_extConWriteReg = value; 
				break;
		}
	}

	bool IsDiskInserted()
	{
		return _diskNumber != FDS::NoDiskInserted;
	}

	uint8_t ReadRegister(uint16_t addr)
	{
		uint8_t value = 0;
		if(_diskRegEnabled) {
			switch(addr) {
				case 0x4030:
					value |= CPU::HasIRQSource(IRQSource::External) ? 0x01 : 0x00;
					value |= _transferComplete ? 0x02 : 0x00;
					value |= _badCrc ? 0x10 : 0x00;
					//value |= _endOfHead ? 0x40 : 0x00;
					//value |= _diskRegEnabled ? 0x80 : 0x00;

					_transferComplete = false;
					CPU::ClearIRQSource(IRQSource::External);
					CPU::ClearIRQSource(IRQSource::FdsDisk);
					return value;

				case 0x4031:
					_transferComplete = false;
					CPU::ClearIRQSource(IRQSource::FdsDisk);
					return _readDataReg;

				case 0x4032:
					value |= !IsDiskInserted() ? 0x01 : 0x00;  //Disk not in drive
					value |= !IsDiskInserted() || !_scanningDisk ? 0x02 : 0x00;  //Disk not ready
					value |= !IsDiskInserted() ? 0x04 : 0x00;  //Disk not writable
					return value;

				case 0x4033:
					//Always return good battery
					return 0x80 & _extConWriteReg;
			}
		}

		//Return open bus
		return (addr & 0xFF00) >> 8;
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);

		Stream<uint16_t>(_irqReloadValue);
		Stream<uint16_t>(_irqCounter);
		Stream<bool>(_irqEnabled);
		Stream<bool>(_irqReloadEnabled);

		Stream<bool>(_diskRegEnabled);
		Stream<bool>(_soundRegEnabled);

		Stream<uint8_t>(_writeDataReg);

		Stream<bool>(_motorOn);
		Stream<bool>(_resetTransfer);
		Stream<bool>(_readMode);
		Stream<bool>(_crcControl);
		Stream<bool>(_diskReady);
		Stream<bool>(_diskIrqEnabled);

		Stream<uint8_t>(_extConWriteReg);

		Stream<bool>(_badCrc);
		Stream<bool>(_endOfHead);
		Stream<bool>(_readWriteEnabled);

		Stream<uint8_t>(_readDataReg);

		Stream<bool>(_diskWriteProtected);

		Stream<uint32_t>(_diskNumber);
		Stream<uint32_t>(_newDiskNumber);
		Stream<uint32_t>(_newDiskInsertDelay);
		Stream<uint32_t>(_diskPosition);
		Stream<uint32_t>(_delay);
		Stream<bool>(_previousCrcControlFlag);
		Stream<bool>(_gapEnded);
		Stream<bool>(_scanningDisk);
		Stream<bool>(_needIrq);
		Stream<bool>(_transferComplete);
		Stream<bool>(_isDirty);
	}

public:
	FDS()
	{
		FDS::Instance = this;
	}

	~FDS()
	{
		if(_isDirty) {
			FdsLoader loader;
			loader.SaveIpsFile(_romFilepath, _fdsRawData, _fdsDiskSides);
		}

		if(FDS::Instance == this) {
			FDS::Instance = nullptr;
		}
	}

	static uint32_t GetSideCount()
	{
		if(FDS::Instance) {
			return (uint32_t)FDS::Instance->_fdsDiskSides.size();
		} else {
			return 0;
		}
	}

	static void InsertDisk(uint32_t diskNumber)
	{
		if(FDS::Instance) {
			Console::Pause();
			FDS::Instance->_newDiskNumber = diskNumber;
			FDS::Instance->_newDiskInsertDelay = FDS::DiskInsertDelay;
			Console::Resume();

			MessageManager::SendNotification(ConsoleNotificationType::FdsDiskChanged);
		}
	}

	static void SwitchDiskSide()
	{
		if(FDS::Instance) {
			Console::Pause();
			FDS::Instance->_newDiskNumber = (FDS::Instance->_diskNumber & 0x01) ? (FDS::Instance->_diskNumber & 0xFE) : (FDS::Instance->_diskNumber | 0x01);
			FDS::Instance->_newDiskInsertDelay = FDS::DiskInsertDelay;
			Console::Resume();

			MessageManager::SendNotification(ConsoleNotificationType::FdsDiskChanged);
		}
	}

	static void EjectDisk()
	{
		if(FDS::Instance) {
			Console::Pause();
			FDS::Instance->_diskNumber = NoDiskInserted;
			FDS::Instance->_newDiskInsertDelay = 0;
			Console::Resume();

			MessageManager::SendNotification(ConsoleNotificationType::FdsDiskChanged);
		}
	}
};