#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Fk23C : public BaseMapper
{
private:
	uint8_t _prgBankingMode;
	uint8_t _outerChrBankSize;
	bool _selectChrRam;
	bool _mmc3ChrMode;
	bool _cnromChrMode;
	uint16_t _prgBaseBits;
	uint8_t _chrBaseBits;
	bool _extendedMmc3Mode;
	uint8_t _wramBankSelect;
	bool _ramInFirstChrBank;
	bool _allowSingleScreenMirroring;
	bool _fk23RegistersEnabled;
	bool _wramConfigEnabled;
	
	bool _wramEnabled;
	bool _wramWriteProtected;
	
	bool _invertPrgA14;
	bool _invertChrA12;

	uint8_t _currentRegister;

	uint8_t _irqReloadValue;
	uint8_t _irqCounter;
	bool _irqReload;
	bool _irqEnabled;

	uint8_t _mirroringReg;

	uint8_t _cnromChrReg;
	uint8_t _mmc3Registers[12];
	
	uint8_t _irqDelay;
	A12Watcher _a12Watcher;

protected:
	uint16_t GetPRGPageSize() override { return 0x2000; }
	uint16_t GetCHRPageSize() override { return 0x0400; }
	
	uint32_t GetChrRamSize() override { return 0x20000; } //only used for iNES 1.0 files w/ no DB entry
	uint16_t GetChrRamPageSize() override { return 0x400; }

	uint32_t GetWorkRamSize() override { return 0x8000; } //only used for iNES 1.0 files w/ no DB entry
	uint32_t GetWorkRamPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		//$5000
		_prgBankingMode = 0;
		_outerChrBankSize = 0;
		_selectChrRam = false;
		_mmc3ChrMode = true;

		//$5001 (mostly)
		//Subtype 1, 1024 KiB PRG-ROM, 1024 KiB CHR-ROM: boot in second 512 KiB of PRG-ROM.
		_prgBaseBits = (_prgSize == 1024*1024 && _prgSize == _chrRomSize) ? 0x20 : 0;

		//$5002
		_chrBaseBits = 0;

		//$5003
		_extendedMmc3Mode = false;
		_cnromChrMode = false;

		//$A001
		_wramBankSelect = 0;
		_ramInFirstChrBank = false;
		_allowSingleScreenMirroring = false;
		_wramConfigEnabled = false;
		_fk23RegistersEnabled = false;
		_wramEnabled = false;
		_wramWriteProtected = false;

		_currentRegister = 0;

		_cnromChrReg = 0;

		constexpr uint8_t initValues[12] = { 0,2,4,5,6,7,0,1,0xFE, 0xFF, 0xFF, 0xFF };
		for(int i = 0; i < 12; i++) {
			_mmc3Registers[i] = initValues[i];
		}

		_invertPrgA14 = false;
		_invertChrA12 = false;

		_mirroringReg = 0;

		_irqCounter = 0;
		_irqEnabled = false;
		_irqReload = false;
		_irqReloadValue = 0;
		_irqDelay = 0;

		AddRegisterRange(0x5000, 0x5FFF, MemoryOperation::Write);
		UpdateState();
	}

	void Reset(bool softReset) override
	{
		if(softReset) {
			if(_wramConfigEnabled && _selectChrRam && HasBattery()) {
				_prgBaseBits = 0;
				UpdateState();
			}
		}
	}

	void StreamState(bool saving) override 
	{
		SnapshotInfo a12Watcher { &_a12Watcher };
		ArrayInfo<uint8_t> regs { _mmc3Registers, 12 };

		Stream(
			_prgBankingMode, _outerChrBankSize, _selectChrRam, _mmc3ChrMode, _cnromChrMode, _prgBaseBits, _chrBaseBits, _extendedMmc3Mode,
			_wramBankSelect, _ramInFirstChrBank, _allowSingleScreenMirroring, _fk23RegistersEnabled, _wramConfigEnabled, _wramEnabled, _wramWriteProtected,
			_invertPrgA14, _invertChrA12, _currentRegister, _irqReloadValue,_irqCounter, _irqReload, _irqEnabled, _mirroringReg, _cnromChrReg,
			_irqDelay, regs, a12Watcher
		);

		if(!saving) {
			UpdateState();
		}
	}

	void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default)
	{
		bool useChrRam = !HasChrRom() || (_selectChrRam && _chrRamSize > 0) || (_wramConfigEnabled && _ramInFirstChrBank && page <= 7);
		BaseMapper::SelectCHRPage(slot, page, useChrRam ? ChrMemoryType::ChrRam : ChrMemoryType::ChrRom);
	}

	void UpdatePrg()
	{
		switch(_prgBankingMode) {
			case 0:
			case 1:
			case 2:
				if(_extendedMmc3Mode) {
					uint8_t swap = _invertPrgA14 ? 2 : 0;
					uint16_t outer = (_prgBaseBits << 1);
					SelectPRGPage(0 ^ swap, _mmc3Registers[6] | outer);
					SelectPRGPage(1, _mmc3Registers[7] | outer);
					SelectPRGPage(2 ^ swap, _mmc3Registers[8] | outer);
					SelectPRGPage(3, _mmc3Registers[9] | outer);
				} else {
					uint8_t swap = _invertPrgA14 ? 2 : 0;
					uint8_t innerMask = 0x3F >> _prgBankingMode;
					uint16_t outer = (_prgBaseBits << 1) & ~innerMask;
					SelectPRGPage(0 ^ swap, (_mmc3Registers[6] & innerMask) | outer);
					SelectPRGPage(1, (_mmc3Registers[7] & innerMask) | outer);
					SelectPRGPage(2 ^ swap, (0xFE & innerMask) | outer);
					SelectPRGPage(3, (0xFF & innerMask) | outer);
				}
				break;

			case 3:
				SelectPrgPage2x(0, _prgBaseBits << 1);
				SelectPrgPage2x(1, _prgBaseBits << 1);
				break;

			case 4:
				SelectPrgPage4x(0, (_prgBaseBits & 0xFFE) << 1);
				break;

			default:
				break;
		}
	}

	void UpdateChr()
	{
		if(!_mmc3ChrMode) {
			uint16_t innerMask = _cnromChrMode ? (_outerChrBankSize ? 1 : 3) : 0;
			for(int i = 0; i < 8; i++) {
				SelectCHRPage(i, (((_cnromChrReg & innerMask) | _chrBaseBits) << 3) + i);
			}
		} else {
			uint8_t swap = _invertChrA12 ? 0x04 : 0;
			if(_extendedMmc3Mode) {
				uint16_t outer = (_chrBaseBits << 3);
				SelectCHRPage(0 ^ swap, _mmc3Registers[0] | outer);
				SelectCHRPage(1 ^ swap, _mmc3Registers[10] | outer);
				SelectCHRPage(2 ^ swap, _mmc3Registers[1] | outer);
				SelectCHRPage(3 ^ swap, _mmc3Registers[11] | outer);
				SelectCHRPage(4 ^ swap, _mmc3Registers[2] | outer);
				SelectCHRPage(5 ^ swap, _mmc3Registers[3] | outer);
				SelectCHRPage(6 ^ swap, _mmc3Registers[4] | outer);
				SelectCHRPage(7 ^ swap, _mmc3Registers[5] | outer);
			} else {
				uint8_t innerMask = (_outerChrBankSize ? 0x7F : 0xFF);
				uint16_t outer = (_chrBaseBits << 3) & ~innerMask;

				SelectCHRPage(0 ^ swap, ((_mmc3Registers[0] & 0xFE) & innerMask) | outer);
				SelectCHRPage(1 ^ swap, ((_mmc3Registers[0] | 0x01) & innerMask) | outer);
				SelectCHRPage(2 ^ swap, ((_mmc3Registers[1] & 0xFE) & innerMask) | outer);
				SelectCHRPage(3 ^ swap, ((_mmc3Registers[1] | 0x01) & innerMask) | outer);
				SelectCHRPage(4 ^ swap, (_mmc3Registers[2] & innerMask) | outer);
				SelectCHRPage(5 ^ swap, (_mmc3Registers[3] & innerMask) | outer);
				SelectCHRPage(6 ^ swap, (_mmc3Registers[4] & innerMask) | outer);
				SelectCHRPage(7 ^ swap, (_mmc3Registers[5] & innerMask) | outer);
			}
		}
	}

	void UpdateState()
	{
		switch(_mirroringReg & (_allowSingleScreenMirroring ? 0x03 : 0x01)) {
			case 0: SetMirroringType(MirroringType::Vertical); break;
			case 1: SetMirroringType(MirroringType::Horizontal); break;
			case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
			case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
		}

		UpdatePrg();
		UpdateChr();

		if(_wramConfigEnabled) {
			uint8_t nextBank = (_wramBankSelect + 1) & 0x03;
			SetCpuMemoryMapping(0x4000, 0x5FFF, (nextBank & 0x02) >> 1, (nextBank & 0x01) ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam, MemoryAccessType::ReadWrite);
			SetCpuMemoryMapping(0x6000, 0x7FFF, (_wramBankSelect & 0x02) >> 1, (_wramBankSelect & 0x01) ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam, MemoryAccessType::ReadWrite);
		} else {
			if(_wramEnabled) {
				SetCpuMemoryMapping(0x6000, 0x7FFF, 0, PrgMemoryType::WorkRam, _wramWriteProtected ? MemoryAccessType::Read : MemoryAccessType::ReadWrite);
			} else {
				RemoveCpuMemoryMapping(0x6000, 0x7FFF);
			}
			RemoveCpuMemoryMapping(0x4000, 0x5FFF);
		};
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr < 0x8000) {
			if(_fk23RegistersEnabled || !_wramConfigEnabled) {
				uint16_t mask = 0x5010;
				if((addr & mask) != mask) {
					//not a register
					return;
				}

				switch(addr & 0x03) {
					case 0:
						_prgBankingMode = value & 0x07;
						_outerChrBankSize = (value & 0x10) >> 4;
						_selectChrRam = (value & 0x20) != 0;
						_mmc3ChrMode = (value & 0x40) == 0;
						_prgBaseBits = (_prgBaseBits & ~0x180) | ((value & 0x80) << 1) | ((value & 0x08) << 4);
						break;

					case 1:
						_prgBaseBits = (_prgBaseBits & ~0x7F) | (value & 0x7F);
						break;

					case 2:
						_prgBaseBits = (_prgBaseBits & ~0x200) | ((value & 0x40) << 3);
						_chrBaseBits = value;
						_cnromChrReg = 0;
						break;

					case 3:
						_extendedMmc3Mode = (value & 0x02) != 0;
						_cnromChrMode = (value & 0x44) != 0;
						break;
				}
				UpdateState();
			} else {
				//FK23C Registers disabled, $5000-$5FFF maps to the second 4 KiB of the 8 KiB WRAM bank 2
				WritePrgRam(addr, value);
			}
		} else {
			if(_cnromChrMode && (addr <= 0x9FFF || addr >= 0xC000)) {
				_cnromChrReg = value & 0x03;
				UpdateState();
			}

			switch(addr & 0xE001) {
				case 0x8000:
					if(_prgSize == 16384*1024 && (value == 0x46 || value == 0x47)) {
						//Subtype 2, 16384 KiB PRG-ROM, no CHR-ROM: Like Subtype 0, but MMC3 registers $46 and $47 swapped.
						value ^= 1;
					}

					_invertPrgA14 = (value & 0x40) != 0;
					_invertChrA12 = (value & 0x80) != 0;
					_currentRegister = value & 0x0F;
					UpdateState();
					break;

				case 0x8001: {
					uint8_t reg = _currentRegister & (_extendedMmc3Mode ? 0x0F : 0x07);
					if(reg < 12) {
						_mmc3Registers[_currentRegister & (_extendedMmc3Mode ? 0x0F : 0x07)] = value;
						UpdateState();
					}
					break;
				}

				case 0xA000:
					_mirroringReg = value & 0x03;
					UpdateState();
					break;

				case 0xA001:
					if((value & 0x20) == 0) {
						//Ignore extra bits if bit 5 is not set
						value &= 0xC0;
					}

					_wramBankSelect = (value & 0x03);
					_ramInFirstChrBank = (value & 0x04) != 0;
					_allowSingleScreenMirroring = (value & 0x08) != 0;
					_wramConfigEnabled = (value & 0x20) != 0;
					_fk23RegistersEnabled = (value & 0x40) != 0;
					
					_wramWriteProtected = (value & 0x40) != 0;
					_wramEnabled = (value & 0x80) != 0;

					UpdateState();
					break;

				case 0xC000:
					_irqReloadValue = value;
					break;

				case 0xC001:
					_irqCounter = 0;
					_irqReload = true;
					break;

				case 0xE000:
					_irqEnabled = false;
					_console->GetCpu()->ClearIrqSource(IRQSource::External);
					break;

				case 0xE001:
					_irqEnabled = true;
					break;
			}
		}
	}

public:
	void ProcessCpuClock() override
	{
		if(_irqDelay > 0) {
			_irqDelay--;
			if(_irqDelay == 0) {
				_console->GetCpu()->SetIrqSource(IRQSource::External);
			}
		}
	}

	void NotifyVRAMAddressChange(uint16_t addr) override
	{
		switch(_a12Watcher.UpdateVramAddress(addr, _console->GetPpu()->GetFrameCycle())) {
			case A12StateChange::None:
			case A12StateChange::Fall:
				break;

			case A12StateChange::Rise:
				if(_irqCounter == 0 || _irqReload) {
					_irqCounter = _irqReloadValue;
				} else {
					_irqCounter--;
				}

				if(_irqCounter == 0 && _irqEnabled) {
					_irqDelay = 2;
				}
				_irqReload = false;
				break;
		}
	}
};