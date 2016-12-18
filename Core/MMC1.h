#pragma once
#include "stdafx.h"
#include "CPU.h"
#include "BaseMapper.h"

class MMC1 : public BaseMapper
{
	private:
		enum class MMC1Registers
		{
			Reg8000 = 0,
			RegA000 = 1,
			RegC000 = 2,
			RegE000 = 3
		};

		enum class PrgMode
		{
			_16k = 16,
			_32k = 32,
		};

		enum class ChrMode
		{
			_4k = 4,
			_8k = 8,
		};

		enum class SlotSelect
		{
			x8000 = 0x8000,
			xC000 = 0xC000,
		};

		uint8_t _writeBuffer = 0;
		uint8_t _shiftCount = 0;

		bool _wramDisable;
		ChrMode _chrMode;
		PrgMode _prgMode;
		SlotSelect _slotSelect;

		uint8_t _chrReg0;
		uint8_t _chrReg1;
		uint8_t _prgReg;

		int32_t _lastWriteCycle = -1;
		
		bool _forceWramOn;
		MMC1Registers _lastChrReg;
		
	private:
		bool HasResetFlag(uint8_t value)
		{
			return (value & 0x80) == 0x80;
		}

		void ResetBuffer()
		{
			_shiftCount = 0;
			_writeBuffer = 0;
		}

		bool IsBufferFull(uint8_t value)
		{
			if(HasResetFlag(value)) {
				//When 'r' is set:
				//	- 'd' is ignored
				//	- hidden temporary reg is reset (so that the next write is the "first" write)
				//	- bits 2,3 of reg $8000 are set (16k PRG mode, $8000 swappable)
				//	- other bits of $8000 (and other regs) are unchanged
				ResetBuffer();
				_state.Reg8000 |= 0x0C;
				return false;
			} else {
				_writeBuffer >>= 1;
				_writeBuffer |= ((value << 4) & 0x10);

				_shiftCount++;

				return _shiftCount == 5;
			}
		}

	protected:
		struct
		{
			uint8_t Reg8000;
			uint8_t RegA000;
			uint8_t RegC000;
			uint8_t RegE000;
		} _state;

		virtual void UpdateState()
		{
			switch(_state.Reg8000 & 0x03) {
				case 0: SetMirroringType(MirroringType::ScreenAOnly); break;
				case 1: SetMirroringType(MirroringType::ScreenBOnly); break;
				case 2: SetMirroringType(MirroringType::Vertical); break;
				case 3: SetMirroringType(MirroringType::Horizontal); break;
			}

			_wramDisable = (_state.RegE000 & 0x10) == 0x10;

			_slotSelect = ((_state.Reg8000 & 0x04) == 0x04) ? SlotSelect::x8000 : SlotSelect::xC000;
			_prgMode = ((_state.Reg8000 & 0x08) == 0x08) ? PrgMode::_16k : PrgMode::_32k;
			_chrMode = ((_state.Reg8000 & 0x10) == 0x10) ? ChrMode::_4k : ChrMode::_8k;

			_chrReg0 = _state.RegA000 & 0x1F;
			_chrReg1 = _state.RegC000 & 0x1F;
			_prgReg = _state.RegE000 & 0x0F;

			uint8_t extraReg = _lastChrReg == MMC1Registers::RegC000 && _chrMode == ChrMode::_4k ? _chrReg1 : _chrReg0;
			uint8_t prgBankSelect = 0;
			if(_prgSize == 0x80000) {
				//512kb carts use bit 7 of $A000/$C000 to select page
				//This is used for SUROM (Dragon Warrior 3/4, Dragon Quest 4)
				prgBankSelect = extraReg & 0x10;
			} 

			if(_wramDisable && !_forceWramOn) {
				RemoveCpuMemoryMapping(0x6000, 0x7FFF);
			} else {
				if(_saveRamSize + _workRamSize > 0x4000) {
					//SXROM, 32kb of save ram
					SetCpuMemoryMapping(0x6000, 0x7FFF, (extraReg >> 2) & 0x03, HasBattery() ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam);
				} else if(_saveRamSize + _workRamSize > 0x2000) {
					if(_saveRamSize == 0x2000 && _workRamSize == 0x2000) {
						//SOROM, half of the 16kb ram is battery backed
						SetCpuMemoryMapping(0x6000, 0x7FFF, 0, (extraReg >> 3) & 0x01 ? PrgMemoryType::WorkRam : PrgMemoryType::SaveRam);
					} else {
						//Unknown, shouldn't happen
						SetCpuMemoryMapping(0x6000, 0x7FFF, (extraReg >> 2) & 0x01, HasBattery() ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam);
					}
				} else {
					//Everything else - 8kb of work or save ram
					SetCpuMemoryMapping(0x6000, 0x7FFF, 0, HasBattery() ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam);
				}
			}

			if(_subMapperID == 5) {
				//SubMapper 5
				//"001: 5 Fixed PRG    SEROM, SHROM, SH1ROM use a fixed 32k PRG ROM with no banking support.
				SelectPrgPage2x(0, 0);
			} else {
				if(_prgMode == PrgMode::_32k) {
					SelectPrgPage2x(0, (_prgReg & 0xFE) | prgBankSelect);
				} else if(_prgMode == PrgMode::_16k) {
					if(_slotSelect == SlotSelect::x8000) {
						SelectPRGPage(0, _prgReg | prgBankSelect);
						SelectPRGPage(1, 0x0F | prgBankSelect);
					} else if(_slotSelect == SlotSelect::xC000) {
						SelectPRGPage(0, 0 | prgBankSelect);
						SelectPRGPage(1, _prgReg | prgBankSelect);
					}
				}
			} 

			if(_chrMode == ChrMode::_8k) {
				SelectCHRPage(0, _chrReg0 & 0x1E);
				SelectCHRPage(1, (_chrReg0 & 0x1E) + 1);
			} else if(_chrMode == ChrMode::_4k) {
				SelectCHRPage(0, _chrReg0);
				SelectCHRPage(1, _chrReg1);
			}
		}

		virtual void StreamState(bool saving) override
		{
			BaseMapper::StreamState(saving);
			Stream(_state.Reg8000, _state.RegA000, _state.RegC000, _state.RegE000, _writeBuffer, _shiftCount, _lastWriteCycle, _lastChrReg);
			if(!saving) {
				UpdateState();
			}
		}

		virtual uint16_t GetPRGPageSize() override { return 0x4000; }
		virtual uint16_t GetCHRPageSize() override {	return 0x1000; }

		virtual void InitMapper() override
		{
			_state.Reg8000 = 0x0C; //On powerup: bits 2,3 of $8000 are set (this ensures the $8000 is bank 0, and $C000 is the last bank - needed for SEROM/SHROM/SH1ROM which do no support banking)
			_state.RegA000 = 0x00;
			_state.RegC000 = 0x00;
			_state.RegE000 = (_databaseInfo.Board.find("MMC1B") != string::npos ? 0x10 : 0x00); //WRAM Disable: enabled by default for MMC1B

			//"MMC1A: PRG RAM is always enabled" - Normally these roms should be classified as mapper 155
			_forceWramOn = (_databaseInfo.Board.compare("MMC1A") == 0);

			_lastChrReg = MMC1Registers::RegA000;

			UpdateState();
		}

		virtual void WriteRegister(uint16_t addr, uint8_t value) override
		{
			int32_t currentCycle = CPU::GetCycleCount();
			
			//Ignore write if within 2 cycles of another write (i.e the real write after a dummy write)
			if(abs(currentCycle - _lastWriteCycle) >= 2) {
				if(IsBufferFull(value)) {
					switch((MMC1Registers)((addr & 0x6000) >> 13)) {
						case MMC1Registers::Reg8000: _state.Reg8000 = _writeBuffer; break;
						case MMC1Registers::RegA000: 
							_lastChrReg = MMC1Registers::RegA000;
							_state.RegA000 = _writeBuffer; 
							break;

						case MMC1Registers::RegC000: 
							_lastChrReg = MMC1Registers::RegC000;
							_state.RegC000 = _writeBuffer;
							break;

						case MMC1Registers::RegE000: _state.RegE000 = _writeBuffer; break;
					}

					UpdateState();

					//Reset buffer after writing 5 bits
					ResetBuffer();
				}
			}
			_lastWriteCycle = currentCycle;
		}
};
