#include "stdafx.h"
#include "FDS.h"
#include "FdsAudio.h"

FDS* FDS::Instance = nullptr;

void FDS::InitMapper()
{
	_diskNumber = EmulationSettings::CheckFlag(EmulationFlags::FdsAutoLoadDisk) ? 0 : FDS::NoDiskInserted;

	//FDS BIOS
	SetCpuMemoryMapping(0xE000, 0xFFFF, 0, PrgMemoryType::PrgRom, MemoryAccessType::Read);

	//Work RAM
	SetCpuMemoryMapping(0x6000, 0xDFFF, 0, PrgMemoryType::WorkRam, MemoryAccessType::ReadWrite);

	//8k of CHR RAM
	SelectCHRPage(0, 0);
}

void FDS::InitMapper(RomData &romData)
{
	_romFilepath = romData.Filename;
	_fdsDiskSides = romData.FdsDiskData;
	_fdsRawData = romData.RawData;
}

uint32_t FDS::GetFdsDiskSideSize(uint8_t side)
{
	assert(side < _fdsDiskSides.size());
	return (uint32_t)_fdsDiskSides[side].size();
}

uint8_t FDS::ReadFdsDisk()
{
	assert(_diskNumber < _fdsDiskSides.size());
	assert(_diskPosition < _fdsDiskSides[_diskNumber].size());
	return _fdsDiskSides[_diskNumber][_diskPosition];
}

void FDS::WriteFdsDisk(uint8_t value)
{
	assert(_diskNumber < _fdsDiskSides.size());
	assert(_diskPosition < _fdsDiskSides[_diskNumber].size());
	if(_fdsDiskSides[_diskNumber][_diskPosition - 2] != value) {
		_isDirty = true;
	}
	_fdsDiskSides[_diskNumber][_diskPosition - 2] = value;
}

void FDS::ClockIrq()
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

uint8_t FDS::ReadRAM(uint16_t addr)
{
	if(addr == 0xDFFC && _gameStarted < 2) {
		//The BIOS reads $DFFC twice before starting the game
		//The 2nd read occurs right at the end of the copyright screen
		//We can fast forward until _gameStarted == 2
		_gameStarted++;
	}

	return BaseMapper::ReadRAM(addr);
}

void FDS::ProcessCpuClock()
{
	if(EmulationSettings::CheckFlag(EmulationFlags::FdsFastForwardOnLoad)) {
		bool enableFastforward = (_scanningDisk || _gameStarted < 2);
		if(EmulationSettings::GetEmulationSpeed() > 0 || !_fastForwarding) {
			_previousSpeed = EmulationSettings::GetEmulationSpeed();
		}
		EmulationSettings::SetEmulationSpeed(enableFastforward ? 0 : _previousSpeed);
		_fastForwarding = enableFastforward;
	}

	ClockIrq();
	_audio->Clock();

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

void FDS::UpdateCrc(uint8_t value)
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

void FDS::WriteRegister(uint16_t addr, uint8_t value)
{
	if(!_diskRegEnabled && addr >= 0x4024 && addr <= 0x4026 || !_soundRegEnabled && addr >= 0x4040) {
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

		default:
			if(addr >= 0x4040) {
				_audio->WriteRegister(addr, value);
			}
			break;
	}
}

bool FDS::IsDiskInserted()
{
	return _diskNumber != FDS::NoDiskInserted;
}

uint8_t FDS::ReadRegister(uint16_t addr)
{
	uint8_t value = 0;
	if(_diskRegEnabled && addr < 0x4040 || _soundRegEnabled && addr >= 0x4040) {
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

			default:
				return _audio->ReadRegister(addr);
		}
	}

	//Return open bus
	return (addr & 0xFF00) >> 8;
}

void FDS::StreamState(bool saving)
{
	BaseMapper::StreamState(saving);

	SnapshotInfo audio{ _audio.get() };
	Stream(_irqReloadValue, _irqCounter, _irqEnabled, _irqReloadEnabled, _diskRegEnabled, _soundRegEnabled, _writeDataReg, _motorOn, _resetTransfer,
		_readMode, _crcControl, _diskReady, _diskIrqEnabled, _extConWriteReg, _badCrc, _endOfHead, _readWriteEnabled, _readDataReg, _diskWriteProtected,
		_diskNumber, _newDiskNumber, _newDiskInsertDelay, _diskPosition, _delay, _previousCrcControlFlag, _gapEnded, _scanningDisk, _needIrq,
		_transferComplete, _isDirty, audio);
}

FDS::FDS()
{
	FDS::Instance = this;

	_audio.reset(new FdsAudio());
}

FDS::~FDS()
{
	//Restore emulation speed to normal when closing
	if(_previousSpeed >= 0) {
		EmulationSettings::SetEmulationSpeed(_previousSpeed);
	}

	if(_isDirty) {
		FdsLoader loader;
		loader.SaveIpsFile(_romFilepath, _fdsRawData, _fdsDiskSides);
	}

	if(FDS::Instance == this) {
		FDS::Instance = nullptr;
	}
}

uint32_t FDS::GetSideCount()
{
	if(FDS::Instance) {
		return (uint32_t)FDS::Instance->_fdsDiskSides.size();
	} else {
		return 0;
	}
}

void FDS::InsertDisk(uint32_t diskNumber)
{
	if(FDS::Instance) {
		Console::Pause();
		FDS::Instance->_newDiskNumber = diskNumber;
		FDS::Instance->_newDiskInsertDelay = FDS::DiskInsertDelay;
		Console::Resume();

		MessageManager::SendNotification(ConsoleNotificationType::FdsDiskChanged);
	}
}

void FDS::SwitchDiskSide()
{
	if(FDS::Instance) {
		Console::Pause();
		FDS::Instance->_newDiskNumber = (FDS::Instance->_diskNumber & 0x01) ? (FDS::Instance->_diskNumber & 0xFE) : (FDS::Instance->_diskNumber | 0x01);
		FDS::Instance->_newDiskInsertDelay = FDS::DiskInsertDelay;
		Console::Resume();

		MessageManager::SendNotification(ConsoleNotificationType::FdsDiskChanged);
	}
}

void FDS::EjectDisk()
{
	if(FDS::Instance) {
		Console::Pause();
		FDS::Instance->_diskNumber = NoDiskInserted;
		FDS::Instance->_newDiskInsertDelay = 0;
		Console::Resume();

		MessageManager::SendNotification(ConsoleNotificationType::FdsDiskChanged);
	}
}
