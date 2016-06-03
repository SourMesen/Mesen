#pragma once

#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"
#include "PPU.h"
#include "EmulationSettings.h"

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

		uint8_t _subMapperID;

		uint8_t _currentRegister;
		uint8_t _chrMode;
		uint8_t _prgMode;

		uint8_t _irqReloadValue;
		uint8_t _irqCounter;
		bool _irqReload;

		bool _irqEnabled;
		uint32_t _lastCycle;
		uint32_t _cyclesDown;

		bool _wramEnabled;
		bool _wramWriteProtected;

		bool _needIrq;

		struct {
			uint8_t Reg8000;
			uint8_t RegA000;
			uint8_t RegA001;
		} _state;

		void Reset()
		{
			_state.Reg8000 = 0;
			_state.RegA000 = 0;
			_state.RegA001 = 0;
			_chrMode = 0;
			_prgMode = 0;
			_currentRegister = 0;
			memset(_registers, 0, sizeof(_registers));

			_irqCounter = 0;
			_irqReloadValue = 0;
			_irqReload = false;
			_irqEnabled = false;
			_lastCycle = 0xFFFF;
			_cyclesDown = 0xFFFF;

			_wramEnabled = false;
			_wramWriteProtected = false;

			_needIrq = false;
		}

	protected:
		uint8_t GetCurrentRegister() 
		{
			return _currentRegister;
		}

		uint8_t GetChrMode()
		{
			return _chrMode;
		}

		virtual bool ForceMmc3RevAIrqs() { return false; }
		uint8_t _registers[8];

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

		bool CanWriteToWorkRam()
		{
			return _wramEnabled && !_wramWriteProtected;
		}

		virtual void UpdateState()
		{
			_currentRegister = _state.Reg8000 & 0x07;
			_chrMode = (_state.Reg8000 & 0x80) >> 7;
			_prgMode = (_state.Reg8000 & 0x40) >> 6;

			UpdateMirroring();

			_wramEnabled = (_state.RegA001 & 0x80) == 0x80;
			_wramWriteProtected = (_state.RegA001 & 0x40) == 0x40;

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

			UpdateChrMapping();
		}

		virtual void StreamState(bool saving)
		{
			BaseMapper::StreamState(saving);
			Stream(_state.Reg8000, _state.RegA000, _state.RegA001, _currentRegister, _chrMode, _prgMode,
				_irqReloadValue, _irqCounter, _irqReload, _irqEnabled, _lastCycle, _cyclesDown,
				_wramEnabled, _wramWriteProtected, ArrayInfo<uint8_t>{_registers, 8}, _needIrq);
		}

		virtual uint16_t GetPRGPageSize() { return 0x2000; }
		virtual uint16_t GetCHRPageSize() {	return 0x0400; }

		virtual void InitMapper() 
		{
			Reset();
			SetCpuMemoryMapping(0x6000, 0x7FFF, 0, HasBattery() ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam);
			UpdateState();
		}

		virtual void WriteRegister(uint16_t addr, uint8_t value)
		{
			switch((MMC3Registers)(addr & 0xE001)) {
				case MMC3Registers::Reg8000:
					_state.Reg8000 = value;
					UpdateState();
					break;

				case MMC3Registers::Reg8001:
					if(_currentRegister >= 6) {
						//"Writes to registers 6 and 7 always ignore bits 6 and 7, as the MMC3 has only 6 PRG ROM address output lines."
						value &= 0x3F;
					} else if(_currentRegister <= 1) {
						//"Writes to registers 0 and 1 always ignore bit 0"
						value &= ~0x01;
					}
					_registers[_currentRegister] = value;
					UpdateState();
					break;

				case MMC3Registers::RegA000:
					_state.RegA000 = value;
					UpdateState();
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

		void TriggerIrq()
		{
			if(_subMapperID != 3) {
				CPU::SetIRQSource(IRQSource::External);
			} else {
				//MM-ACC (Acclaim copy of the MMC3)
				//IRQ will be triggered on the next falling edge of A12 instead of on the rising edge like normal MMC3 behavior
				//This adds a 4 ppu cycle delay (until the PPU fetches the next garbage NT tile between sprites)
				_needIrq = true;
			}
		}


	public:
		MMC3(uint8_t subMapperID)
		{
			_subMapperID = subMapperID;
		}

		MMC3()
		{
			_subMapperID = 0;
		}

		virtual void NotifyVRAMAddressChange(uint16_t addr)
		{
			uint32_t cycle = PPU::GetFrameCycle();

			if((addr & 0x1000) == 0) {
				if(_needIrq) {
					//Used by MM-ACC (Acclaim copy of the MMC3), see TriggerIrq above
					CPU::SetIRQSource(IRQSource::External);
					_needIrq = false;
				}

				if(_cyclesDown == 0) {
					_cyclesDown = 1;
				} else {
					if(_lastCycle > cycle) {
						//We changed frames
						_cyclesDown += (89342 - _lastCycle) + cycle;
					} else {
						_cyclesDown += (cycle - _lastCycle);
					}
				}
			} else if(addr & 0x1000) {
				if(_cyclesDown > 8) {
					uint32_t count = _irqCounter;
					if(_irqCounter == 0 || _irqReload) {
						_irqCounter = _irqReloadValue;
					} else {
						_irqCounter--;
					}

					//SubMapper 2 = MM-ACC (Acclaim MMC3 clone)
					if(_subMapperID != 2 && (ForceMmc3RevAIrqs() || EmulationSettings::CheckFlag(EmulationFlags::Mmc3IrqAltBehavior))) {
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
				}
				_cyclesDown = 0;
			}
			_lastCycle = cycle;
		}
};