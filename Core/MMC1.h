#include "stdafx.h"
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

		struct {
			uint8_t Reg8000; 
			uint8_t RegA000;
			uint8_t RegC000;
			uint8_t RegE000;
		} _state;
		
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
				//std::cout << std::hex << "input value: " << (short)value << std::endl;
				_writeBuffer >>= 1;
				_writeBuffer |= ((value << 4) & 0x10);

				_shiftCount++;

				if(_shiftCount == 5) {
					//std::cout << std::hex << "value: " << (short)_writeBuffer << std::endl;
				}

				return _shiftCount == 5;
			}
		}

		void UpdateState()
		{
			switch(_state.Reg8000 & 0x03) {
				case 0: _mirroringType = MirroringType::ScreenAOnly; break;
				case 1: _mirroringType = MirroringType::ScreenBOnly; break;
				case 2: _mirroringType = MirroringType::Vertical; break;
				case 3: _mirroringType = MirroringType::Horizontal; break;
			}

			_wramDisable = (_state.RegE000 & 0x10) == 0x10;

			_slotSelect = ((_state.Reg8000 & 0x04) == 0x04) ? SlotSelect::x8000 : SlotSelect::xC000;
			_prgMode = ((_state.Reg8000 & 0x08) == 0x08) ? PrgMode::_16k : PrgMode::_32k;
			_chrMode = ((_state.Reg8000 & 0x10) == 0x10) ? ChrMode::_4k : ChrMode::_8k;

			_chrReg0 = _state.RegA000 & 0x1F;
			_chrReg1 = _state.RegC000 & 0x1F;
			_prgReg = _state.RegE000 & 0x0F;

			//This is used for SUROM (Dragon Warrior 3/4, Dragon Quest 4)
			uint8_t prgPageAdjust = (_state.RegA000 & 0x10);

			if(_prgMode == PrgMode::_32k) {
				SelectPRGPage(0, _prgReg + prgPageAdjust);
				SelectPRGPage(1, _prgReg + prgPageAdjust + 1);
			} else if(_prgMode == PrgMode::_16k) {
				if(_slotSelect == SlotSelect::x8000) {
					SelectPRGPage(0, _prgReg + prgPageAdjust);
					SelectPRGPage(1, 0xF + prgPageAdjust);
				} else if(_slotSelect == SlotSelect::xC000) {
					SelectPRGPage(0, prgPageAdjust);
					SelectPRGPage(1, _prgReg + prgPageAdjust);
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

	protected:
		void StreamState(bool saving)
		{
			Stream<uint8_t>(_state.Reg8000);
			Stream<uint8_t>(_state.RegA000);
			Stream<uint8_t>(_state.RegC000);
			Stream<uint8_t>(_state.RegE000);

			Stream<uint8_t>(_writeBuffer);
			Stream<uint8_t>(_shiftCount);

			BaseMapper::StreamState(saving);
		}

		virtual uint32_t GetPRGPageSize() { return 0x4000; }
		virtual uint32_t GetCHRPageSize() {	return 0x1000; }

		void InitMapper()
		{
			_state.Reg8000 = 0x0C; //On powerup: bits 2,3 of $8000 are set 
			_state.RegA000 = 0x00;
			_state.RegC000 = 0x00;
			_state.RegE000 = 0x10; //WRAM Disable: assume it's enabled at startup

			UpdateState();
		}

		void WriteRegister(uint16_t addr, uint8_t value)
		{
			if(IsBufferFull(value)) {
				switch((MMC1Registers)((addr & 0x6000) >> 13)) {
					case MMC1Registers::Reg8000: _state.Reg8000 = _writeBuffer; break;
					case MMC1Registers::RegA000: _state.RegA000 = _writeBuffer; break;
					case MMC1Registers::RegC000: _state.RegC000 = _writeBuffer; break;
					case MMC1Registers::RegE000: _state.RegE000 = _writeBuffer; break;
				}

				UpdateState();

				//Reset buffer after writing 5 bits
				ResetBuffer();
			}
		}
};
