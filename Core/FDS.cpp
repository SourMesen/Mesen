#include "stdafx.h"
#include "../Utilities/IpsPatcher.h"
#include "Console.h"
#include "FDS.h"
#include "PPU.h"
#include "FdsAudio.h"
#include "MemoryManager.h"
#include "BatteryManager.h"
#include "MovieManager.h"

void FDS::InitMapper()
{
	_settings = _console->GetSettings();

	//FDS BIOS
	SetCpuMemoryMapping(0xE000, 0xFFFF, 0, PrgMemoryType::PrgRom, MemoryAccessType::Read);

	//Work RAM
	SetCpuMemoryMapping(0x6000, 0xDFFF, 0, PrgMemoryType::WorkRam, MemoryAccessType::ReadWrite);

	//8k of CHR RAM
	SelectCHRPage(0, 0);
}

void FDS::InitMapper(RomData &romData)
{
	_audio.reset(new FdsAudio(_console));
	_romFilepath = romData.Info.Filename;
	_fdsDiskSides = romData.FdsDiskData;
	_fdsDiskHeaders = romData.FdsDiskHeaders;
	_fdsRawData = romData.RawData;
	
	//Apply save data (saved as an IPS file), if found
	vector<uint8_t> ipsData = _console->GetBatteryManager()->LoadBattery(".ips");
	LoadDiskData(ipsData);
}

void FDS::LoadDiskData(vector<uint8_t> ipsData)
{
	_fdsDiskSides.clear();
	_fdsDiskHeaders.clear();
	
	FdsLoader loader;
	vector<uint8_t> patchedData;
	if(ipsData.size() > 0 && IpsPatcher::PatchBuffer(ipsData, _fdsRawData, patchedData)) {
		loader.LoadDiskData(patchedData, _fdsDiskSides, _fdsDiskHeaders);
	} else {
		loader.LoadDiskData(_fdsRawData, _fdsDiskSides, _fdsDiskHeaders);
	}
}

vector<uint8_t> FDS::CreateIpsPatch()
{
	FdsLoader loader;
	bool needHeader = (memcmp(_fdsRawData.data(), "FDS\x1a", 4) == 0);
	vector<uint8_t> newData = loader.RebuildFdsFile(_fdsDiskSides, needHeader);	
	return IpsPatcher::CreatePatch(_fdsRawData, newData);
}

void FDS::SaveBattery()
{
	vector<uint8_t> ipsData = CreateIpsPatch();
	_console->GetBatteryManager()->SaveBattery(".ips", ipsData.data(), (uint32_t)ipsData.size());
}

void FDS::Reset(bool softReset)
{
	_autoDiskEjectCounter = -1;
	_autoDiskSwitchCounter = -1;
	_disableAutoInsertDisk = false;
	_gameStarted = false;
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
	_fdsDiskSides[_diskNumber][_diskPosition - 2] = value;
}

void FDS::ClockIrq()
{
	if(_irqEnabled) {
		if(_irqCounter == 0) {
			_console->GetCpu()->SetIrqSource(IRQSource::External);
			_irqCounter = _irqReloadValue;
			if(!_irqRepeatEnabled) {
				_irqEnabled = false;
			}
		} else {
			_irqCounter--;
		}
	}
}

uint8_t FDS::ReadRAM(uint16_t addr)
{
	if(addr == 0xE18C && !_gameStarted && (_console->GetMemoryManager()->DebugRead(0x100) & 0xC0) != 0) {
		//$E18B is the NMI entry point (using $E18C due to dummy reads)
		//When NMI occurs while $100 & $C0 != 0, it typically means that the game is starting.
		_gameStarted = true;
	} else if(addr == 0xE445 && IsAutoInsertDiskEnabled()) {
		//Game is trying to check if a specific disk/side is inserted
		//Find the matching disk and insert it automatically
		uint16_t bufferAddr = _console->GetMemoryManager()->DebugReadWord(0);
		uint8_t buffer[10];
		for(int i = 0; i < 10; i++) {
			//Prevent infinite recursion
			if(bufferAddr + i != 0xE445) {
				buffer[i] = _console->GetMemoryManager()->DebugRead(bufferAddr + i);
			} else {
				buffer[i] = 0;
			}
		}

		int matchCount = 0;
		int matchIndex = -1;
		for(int j = 0; j < (int)_fdsDiskHeaders.size(); j++) {
			bool match = true;
			for(int i = 0; i < 10; i++) {
				if(buffer[i] != 0xFF && buffer[i] != _fdsDiskHeaders[j][i + 14]) {
					match = false;
					break;
				}
			}

			if(match) {
				matchCount++;
				matchIndex = matchCount > 1 ? -1 : j;
			}
		}

		if(matchCount > 1) {
			//More than 1 disk matches, can happen in unlicensed games - disable auto insert logic
			_disableAutoInsertDisk = true;
		}

		if(matchIndex >= 0) {
			//Found a single match, insert it
			_diskNumber = matchIndex;
			if(_diskNumber != _previousDiskNumber) {
				MessageManager::Log("[FDS] Disk automatically inserted: Disk " + std::to_string((_diskNumber / 2) + 1) + ((_diskNumber & 0x01) ? " Side B" : " Side A"));
				_previousDiskNumber = _diskNumber;
			}

			if(matchIndex > 0) {
				//Make sure we disable fast forward
				_gameStarted = true;
			}
		}

		//Prevent disk from being switched again until the disk is actually read
		_autoDiskSwitchCounter = -1;
		_restartAutoInsertCounter = -1;
	}

	return BaseMapper::ReadRAM(addr);
}

void FDS::ProcessAutoDiskInsert()
{
	if(IsAutoInsertDiskEnabled()) {
		bool fastForwardEnabled = _settings->CheckFlag(EmulationFlags::FdsFastForwardOnLoad);
		uint32_t currentFrame = _console->GetPpu()->GetFrameCount();
		if(_previousFrame != currentFrame) {
			_previousFrame = currentFrame;
			if(_autoDiskEjectCounter > 0) {
				//After reading a disk, wait until this counter reaches 0 before
				//automatically ejecting the disk the next time $4032 is read
				_autoDiskEjectCounter--;
				_settings->SetFlagState(EmulationFlags::ForceMaxSpeed, fastForwardEnabled && _autoDiskEjectCounter != 0);
			} else if(_autoDiskSwitchCounter > 0) {
				//After ejecting the disk, wait a bit before we insert a new one
				_autoDiskSwitchCounter--;
				_settings->SetFlagState(EmulationFlags::ForceMaxSpeed, fastForwardEnabled && _autoDiskSwitchCounter != 0);
				if(_autoDiskSwitchCounter == 0) {
					//Insert a disk (real disk/side will be selected when game executes $E445
					MessageManager::Log("[FDS] Auto-inserted dummy disk.");
					InsertDisk(0);

					//Restart process after 200 frames if the game hasn't read the disk yet
					_restartAutoInsertCounter = 200;
				}
			} else if(_restartAutoInsertCounter > 0) {
				//After ejecting the disk, wait a bit before we insert a new one
				_restartAutoInsertCounter--;
				_settings->SetFlagState(EmulationFlags::ForceMaxSpeed, fastForwardEnabled && _restartAutoInsertCounter != 0);
				if(_restartAutoInsertCounter == 0) {
					//Wait a bit before ejecting the disk (eject in ~34 frames)
					MessageManager::Log("[FDS] Game failed to load disk, try again.");
					_previousDiskNumber = FDS::NoDiskInserted;
					_autoDiskEjectCounter = 34;
					_autoDiskSwitchCounter = -1;
				}
			}
		}
	}
}

void FDS::ProcessCpuClock()
{
	if(_settings->CheckFlag(EmulationFlags::FdsFastForwardOnLoad)) {
		_settings->SetFlagState(EmulationFlags::ForceMaxSpeed, _scanningDisk || !_gameStarted);
	} else {
		_settings->ClearFlags(EmulationFlags::ForceMaxSpeed);
	}

	ProcessAutoDiskInsert();

	ClockIrq();
	_audio->Clock();

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
		_autoDiskEjectCounter = -1;
		_autoDiskSwitchCounter = -1;

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
					_console->GetCpu()->SetIrqSource(IRQSource::FdsDisk);
				}
			}
		} else {
			if(!_crcControl) {
				_transferComplete = true;
				diskData = _writeDataReg;
				if(needIrq) {
					_console->GetCpu()->SetIrqSource(IRQSource::FdsDisk);
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

			//Wait a bit before ejecting the disk (eject in ~77 frames)
			_autoDiskEjectCounter = 77;
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
	if((!_diskRegEnabled && addr >= 0x4024 && addr <= 0x4026) || (!_soundRegEnabled && addr >= 0x4040)) {
		return;
	}

	switch(addr) {
		case 0x4020:
			_irqReloadValue = (_irqReloadValue & 0xFF00) | value;
			break;

		case 0x4021:
			_irqReloadValue = (_irqReloadValue & 0x00FF) | (value << 8);
			break;

		case 0x4022:
			_irqRepeatEnabled = (value & 0x01) == 0x01;
			_irqEnabled = (value & 0x02) == 0x02 && _diskRegEnabled;

			if(_irqEnabled) {
				_irqCounter = _irqReloadValue;
			} else {
				_console->GetCpu()->ClearIrqSource(IRQSource::External);
			}
			break;

		case 0x4023:
			_diskRegEnabled = (value & 0x01) == 0x01;
			_soundRegEnabled = (value & 0x02) == 0x02;

			if(!_diskRegEnabled) {
				_irqEnabled = false;
				_console->GetCpu()->ClearIrqSource(IRQSource::External);
				_console->GetCpu()->ClearIrqSource(IRQSource::FdsDisk);
			}
			break;

		case 0x4024:
			_writeDataReg = value;
			_transferComplete = false;

			//Unsure about clearing irq here: FCEUX/Nintendulator don't do this, puNES does.
			_console->GetCpu()->ClearIrqSource(IRQSource::FdsDisk);
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

			//Writing to $4025 clears IRQ according to FCEUX, puNES & Nintendulator
			//Fixes issues in some unlicensed games (error $20 at power on)
			_console->GetCpu()->ClearIrqSource(IRQSource::FdsDisk);
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

uint8_t FDS::ReadRegister(uint16_t addr)
{
	uint8_t value = _console->GetMemoryManager()->GetOpenBus();
	if(_soundRegEnabled && addr >= 0x4040) {
		return _audio->ReadRegister(addr);
	} else if(_diskRegEnabled && addr <= 0x4033) {
		switch(addr) {
			case 0x4030:
				//These 3 pins are open bus
				value &= 0x2C;

				value |= _console->GetCpu()->HasIrqSource(IRQSource::External) ? 0x01 : 0x00;
				value |= _transferComplete ? 0x02 : 0x00;
				value |= _badCrc ? 0x10 : 0x00;
				//value |= _endOfHead ? 0x40 : 0x00;
				//value |= _diskRegEnabled ? 0x80 : 0x00;

				_transferComplete = false;
				_console->GetCpu()->ClearIrqSource(IRQSource::External);
				_console->GetCpu()->ClearIrqSource(IRQSource::FdsDisk);
				return value;

			case 0x4031:
				_transferComplete = false;
				_console->GetCpu()->ClearIrqSource(IRQSource::FdsDisk);
				return _readDataReg;

			case 0x4032:
				//These 5 pins are open bus
				value &= 0xF8;

				value |= !IsDiskInserted() ? 0x01 : 0x00;  //Disk not in drive
				value |= (!IsDiskInserted() || !_scanningDisk) ? 0x02 : 0x00;  //Disk not ready
				value |= !IsDiskInserted() ? 0x04 : 0x00;  //Disk not writable

				if(IsAutoInsertDiskEnabled()) {
					if(_console->GetPpu()->GetFrameCount() - _lastDiskCheckFrame < 100) {
						_successiveChecks++;
					} else {
						_successiveChecks = 0;
					}
					_lastDiskCheckFrame = _console->GetPpu()->GetFrameCount();

					if(_successiveChecks > 20 && _autoDiskEjectCounter == 0 && _autoDiskSwitchCounter == -1) {
						//Game tried to check if a disk was inserted or not - this is usually done when the disk needs to be changed
						//Eject the current disk and insert the new one in ~77 frames
						_lastDiskCheckFrame = 0;
						_successiveChecks = 0;
						_autoDiskSwitchCounter = 77;
						_previousDiskNumber = _diskNumber;
						_diskNumber = NoDiskInserted;

						MessageManager::Log("[FDS] Disk automatically ejected.");
					}
				}
				return value;

			case 0x4033:
				//Always return good battery
				return _extConWriteReg;
		}
	}

	return _console->GetMemoryManager()->GetOpenBus();
}

void FDS::StreamState(bool saving)
{
	BaseMapper::StreamState(saving);

	SnapshotInfo audio{ _audio.get() };
	
	Stream(_irqReloadValue, _irqCounter, _irqEnabled, _irqRepeatEnabled, _diskRegEnabled, _soundRegEnabled, _writeDataReg, _motorOn, _resetTransfer,
		_readMode, _crcControl, _diskReady, _diskIrqEnabled, _extConWriteReg, _badCrc, _endOfHead, _readWriteEnabled, _readDataReg, _diskWriteProtected,
		_diskNumber, _diskPosition, _delay, _previousCrcControlFlag, _gapEnded, _scanningDisk, _transferComplete, audio);

	if(saving) {
		vector<uint8_t> ipsData = CreateIpsPatch();
		VectorInfo<uint8_t> data{ &ipsData };
		Stream(data);
	} else {
		vector<uint8_t> ipsData;
		VectorInfo<uint8_t> data{ &ipsData };
		Stream(data);
		
		if(ipsData.size() > 0) {
			LoadDiskData(ipsData);
		}

		//Make sure we disable fast forwarding when loading a state
		//Otherwise it's possible to get stuck in fast forward mode
		_gameStarted = true;
	}
}

FDS::~FDS()
{
	//Restore emulation speed to normal when closing
	_settings->ClearFlags(EmulationFlags::ForceMaxSpeed);
}

ConsoleFeatures FDS::GetAvailableFeatures()
{
	return ConsoleFeatures::Fds;
}

uint32_t FDS::GetSideCount()
{
	return (uint32_t)_fdsDiskSides.size();
}

void FDS::EjectDisk()
{
	_diskNumber = FDS::NoDiskInserted;
}

void FDS::InsertDisk(uint32_t diskNumber)
{
	if(_diskNumber == FDS::NoDiskInserted) {
		_diskNumber = diskNumber % GetSideCount();
	}
}

uint32_t FDS::GetCurrentDisk()
{
	return _diskNumber;
}

bool FDS::IsDiskInserted()
{
	return _diskNumber != FDS::NoDiskInserted;
}

bool FDS::IsAutoInsertDiskEnabled()
{
	return !_disableAutoInsertDisk && _settings->CheckFlag(EmulationFlags::FdsAutoInsertDisk) && !MovieManager::Playing() && !MovieManager::Recording();
}