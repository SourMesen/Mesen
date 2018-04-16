#pragma once

#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"
#include "EmulationSettings.h"
#include "A12Watcher.h"

class MMC3 : public BaseMapper
{
	private: 
		enum class MMC3Registers
		{
			Reg8000 = 0x8000,
			Reg8001 = 0x8001,
			RegA000 = 0xA000,
			RegA001 = 0xA001,
			RegC000 = 0xC000,
			RegC001 = 0xC001,
			RegE000 = 0xE000,
			RegE001 = 0xE001
		};

		uint8_t _currentRegister;

		bool _wramEnabled;
		bool _wramWriteProtected;

		A12Watcher _a12Watcher;
		bool _needIrq;

		bool _forceMmc3RevAIrqs;

		struct Mmc3State {
			uint8_t Reg8000;
			uint8_t RegA000;
			uint8_t RegA001;
		} _state;

		bool IsMcAcc()
		{
			return _mapperID == 4 && _subMapperID == 3;
		}

	protected:
		uint8_t _irqReloadValue;
		uint8_t _irqCounter;
		bool _irqReload;
		bool _irqEnabled;
		uint8_t _prgMode;
		uint8_t _chrMode;
		uint8_t _registers[8];

		uint8_t GetCurrentRegister() 
		{
			return _currentRegister;
		}

		Mmc3State GetState()
		{
			return _state;
		}

		uint8_t GetChrMode()
		{
			return _chrMode;
		}

		void ResetMmc3()
		{
			_state.Reg8000 = GetPowerOnByte();
			_state.RegA000 = GetPowerOnByte();
			_state.RegA001 = GetPowerOnByte();
			
			_chrMode = GetPowerOnByte() & 0x01;
			_prgMode = GetPowerOnByte() & 0x01;
			
			_currentRegister = GetPowerOnByte();
			
			_registers[0] = GetPowerOnByte(0);
			_registers[1] = GetPowerOnByte(2);
			_registers[2] = GetPowerOnByte(4);
			_registers[3] = GetPowerOnByte(5);
			_registers[4] = GetPowerOnByte(6);
			_registers[5] = GetPowerOnByte(7);
			_registers[6] = GetPowerOnByte(0);
			_registers[7] = GetPowerOnByte(1);

			_irqCounter = GetPowerOnByte();
			_irqReloadValue = GetPowerOnByte();
			_irqReload = GetPowerOnByte() & 0x01;
			_irqEnabled = GetPowerOnByte() & 0x01;

			_wramEnabled = GetPowerOnByte() & 0x01;
			_wramWriteProtected = GetPowerOnByte() & 0x01;

			_needIrq = false;
		}

		virtual bool ForceMmc3RevAIrqs() { return _forceMmc3RevAIrqs; }

		virtual void UpdateMirroring()
		{
			if(GetMirroringType() != MirroringType::FourScreens) {
				SetMirroringType(((_state.RegA000 & 0x01) == 0x01) ? MirroringType::Horizontal : MirroringType::Vertical);
			}
		}

		virtual void UpdateChrMapping()
		{
			if(_chrMode == 0) {
				SelectCHRPage(0, _registers[0] & 0xFE);
				SelectCHRPage(1, _registers[0] | 0x01);
				SelectCHRPage(2, _registers[1] & 0xFE);
				SelectCHRPage(3, _registers[1] | 0x01);

				SelectCHRPage(4, _registers[2]);
				SelectCHRPage(5, _registers[3]);
				SelectCHRPage(6, _registers[4]);
				SelectCHRPage(7, _registers[5]);
			} else if(_chrMode == 1) {
				SelectCHRPage(0, _registers[2]);
				SelectCHRPage(1, _registers[3]);
				SelectCHRPage(2, _registers[4]);
				SelectCHRPage(3, _registers[5]);

				SelectCHRPage(4, _registers[0] & 0xFE);
				SelectCHRPage(5, _registers[0] | 0x01);
				SelectCHRPage(6, _registers[1] & 0xFE);
				SelectCHRPage(7, _registers[1] | 0x01);
			}
		}

		virtual void UpdatePrgMapping()
		{
			if(_prgMode == 0) {
				SelectPRGPage(0, _registers[6]);
				SelectPRGPage(1, _registers[7]);
				SelectPRGPage(2, -2);
				SelectPRGPage(3, -1);
			} else if(_prgMode == 1) {
				SelectPRGPage(0, -2);
				SelectPRGPage(1, _registers[7]);
				SelectPRGPage(2, _registers[6]);
				SelectPRGPage(3, -1);
			}
		}

		bool CanWriteToWorkRam()
		{
			return _wramEnabled && !_wramWriteProtected;
		}

		virtual void UpdateState()
		{
			_currentRegister = _state.Reg8000 & 0x07;
			_chrMode = (_state.Reg8000 & 0x80) >> 7;
			_prgMode = (_state.Reg8000 & 0x40) >> 6;

			if(_subMapperID == 1) {
				//bool wramEnabled = (_state.Reg8000 & 0x20) == 0x20;
				RemoveCpuMemoryMapping(0x6000, 0x7000);
				
				uint8_t firstBankAccess = (_state.RegA001 & 0x10 ? MemoryAccessType::Write : 0) | (_state.RegA001 & 0x20 ? MemoryAccessType::Read : 0);
				uint8_t lastBankAccess = (_state.RegA001 & 0x40 ? MemoryAccessType::Write : 0) | (_state.RegA001 & 0x80 ? MemoryAccessType::Read : 0);

				for(int i = 0; i < 4; i++) {
					SetCpuMemoryMapping(0x7000 + i * 0x400, 0x71FF + i * 0x400, 0, PrgMemoryType::SaveRam, firstBankAccess);
					SetCpuMemoryMapping(0x7200 + i * 0x400, 0x73FF + i * 0x400, 1, PrgMemoryType::SaveRam, lastBankAccess);
				}
			} else {
				_wramEnabled = (_state.RegA001 & 0x80) == 0x80;
				_wramWriteProtected = (_state.RegA001 & 0x40) == 0x40;

				if(IsNes20() && _subMapperID == 0) {
					if(_wramEnabled) {
						SetCpuMemoryMapping(0x6000, 0x7FFF, 0, HasBattery() ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam, CanWriteToWorkRam() ? MemoryAccessType::ReadWrite : MemoryAccessType::Read);
					} else {
						RemoveCpuMemoryMapping(0x6000, 0x7FFF);
					}
				}
			}

			UpdatePrgMapping();
			UpdateChrMapping();
		}

		virtual void StreamState(bool saving) override
		{
			BaseMapper::StreamState(saving);
			ArrayInfo<uint8_t> registers = { _registers, 8 };
			SnapshotInfo a12Watcher{ &_a12Watcher };
			Stream(_state.Reg8000, _state.RegA000, _state.RegA001, _currentRegister, _chrMode, _prgMode,
				_irqReloadValue, _irqCounter, _irqReload, _irqEnabled, a12Watcher,
				_wramEnabled, _wramWriteProtected, registers, _needIrq);
		}

		void AfterLoadState() override
		{
			UpdateState();
		}

		virtual uint16_t GetPRGPageSize() override { return 0x2000; }
		virtual uint16_t GetCHRPageSize() override {	return 0x0400; }
		virtual uint32_t GetSaveRamPageSize() override { return _subMapperID == 1 ? 0x200 : 0x2000; }
		virtual uint32_t GetSaveRamSize() override { return _subMapperID == 1 ? 0x400 : 0x2000; }

		virtual void InitMapper() override 
		{
			//Force MMC3A irqs for boards that are known to use the A revision.
			//Some MMC3B boards also have the A behavior, but currently no way to tell them apart.
			_forceMmc3RevAIrqs = _databaseInfo.Chip.substr(0, 5).compare("MMC3A") == 0;

			ResetMmc3();
			SetCpuMemoryMapping(0x6000, 0x7FFF, 0, HasBattery() ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam);
			UpdateState();
			UpdateMirroring();
		}

		virtual void WriteRegister(uint16_t addr, uint8_t value) override
		{
			switch((MMC3Registers)(addr & 0xE001)) {
				case MMC3Registers::Reg8000:
					_state.Reg8000 = value;
					UpdateState();
					break;

				case MMC3Registers::Reg8001:
					if(_currentRegister <= 1) {
						//"Writes to registers 0 and 1 always ignore bit 0"
						value &= ~0x01;
					}
					_registers[_currentRegister] = value;
					UpdateState();
					break;

				case MMC3Registers::RegA000:
					_state.RegA000 = value;
					UpdateMirroring();
					break;

				case MMC3Registers::RegA001:
					_state.RegA001 = value;
					UpdateState();
					break;

				case MMC3Registers::RegC000:
					_irqReloadValue = value;
					break;

				case MMC3Registers::RegC001:
					_irqCounter = 0;
					_irqReload = true;
					break;

				case MMC3Registers::RegE000:
					_irqEnabled = false;
					CPU::ClearIRQSource(IRQSource::External);
					break;

				case MMC3Registers::RegE001:
					_irqEnabled = true;
					break;
			}
		}

		virtual void TriggerIrq()
		{
			if(IsMcAcc()) {
				//MC-ACC (Acclaim copy of the MMC3)
				//IRQ will be triggered on the next falling edge of A12 instead of on the rising edge like normal MMC3 behavior
				//This adds a 4 ppu cycle delay (until the PPU fetches the next garbage NT tile between sprites)
				_needIrq = true;
			} else {
				CPU::SetIRQSource(IRQSource::External);
			}
		}


	public:
		virtual void NotifyVRAMAddressChange(uint16_t addr) override
		{
			switch(_a12Watcher.UpdateVramAddress(addr)) {
				case A12StateChange::Fall:
					if(_needIrq) {
						//Used by MC-ACC (Acclaim copy of the MMC3), see TriggerIrq above
						CPU::SetIRQSource(IRQSource::External);
						_needIrq = false;
					}
					break;
				case A12StateChange::Rise:
					uint32_t count = _irqCounter;
					if(_irqCounter == 0 || _irqReload) {
						_irqCounter = _irqReloadValue;
					} else {
						_irqCounter--;
					}

					//SubMapper 2 = MC-ACC (Acclaim MMC3 clone)
					if(!IsMcAcc() && (ForceMmc3RevAIrqs() || EmulationSettings::CheckFlag(EmulationFlags::Mmc3IrqAltBehavior))) {
						//MMC3 Revision A behavior
						if((count > 0 || _irqReload) && _irqCounter == 0 && _irqEnabled) {
							TriggerIrq();
						}
					} else {
						if(_irqCounter == 0 && _irqEnabled) {
							TriggerIrq();
						}
					}
					_irqReload = false;
					break;
			}
		}
};