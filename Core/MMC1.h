#include "stdafx.h"
#include "BaseMapper.h"

class MMC1 : public DefaultMapper
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

		MirroringType _mirroringType;
		bool _wramDisable;
		ChrMode _chrMode;
		PrgMode _prgMode;
		SlotSelect _slotSelect;

		uint8_t _chrReg1;
		uint8_t _chrReg2;
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

			_chrReg1 = _state.RegA000 & 0x1F;
			_chrReg2 = _state.RegC000 & 0x1F;
			_prgReg = _state.RegE000 & 0x0F;

			uint8_t page1;
			uint8_t page2;

			if(_prgMode == PrgMode::_32k) {
				page1 = (_prgReg >> 1);
				page2 = page1 + 1;
			} else if(_prgMode == PrgMode::_16k) {
				if(_slotSelect == SlotSelect::x8000) {
					page1 = _prgReg;
					page2 = 0x0F; //$C000 fixed to page $0F (mode B)
				} else if(_slotSelect == SlotSelect::xC000) {
					page1 = 0;
					page2 = _prgReg;
				}
			}

			uint8_t numberOfPRGPages = _prgSize / 0x4000;
			page1 &= (numberOfPRGPages - 1);
			page2 &= (numberOfPRGPages - 1);
			_mappedRomBanks[0] = &_prgRAM[page1 * 0x4000];
			_mappedRomBanks[1] = &_prgRAM[page2 * 0x4000];
			//std::cout << std::dec << "PRG Bank: " << (short)page1 << " & " << (short)page2 << std::endl;


			if(_chrMode == ChrMode::_8k) {
				page1 = (_chrReg1 >> 1);
				page2 = page1 + 1;
			} else if(_chrMode == ChrMode::_4k) {
				page1 = _chrReg1;
				page2 = _chrReg2;
			}

			uint8_t numberOfCHRPages = _chrSize / 0x1000;
			page1 &= (numberOfCHRPages - 1);
			page2 &= (numberOfCHRPages - 1);
			_mappedVromBanks[0] = &_chrRAM[page1 * 0x1000];
			_mappedVromBanks[1] = &_chrRAM[page2 * 0x1000];
			//std::cout << std::dec << "CHR Bank: " << (short)page1 << " & " << (short)page2 << std::endl;
		}

	protected:
		void InitMapper()
		{
			_mappedRomBanks.push_back(nullptr);
			_mappedRomBanks.push_back(nullptr);
			_mappedVromBanks.push_back(nullptr);
			_mappedVromBanks.push_back(nullptr);

			_state.Reg8000 = 0x0C; //On powerup: bits 2,3 of $8000 are set 
			_state.RegA000 = 0x00;
			_state.RegC000 = 0x00;
			_state.RegE000 = 0x10; //WRAM Disable: assume it's enabled at startup

			UpdateState();
		}

	public:
		void WriteRAM(uint16_t addr, uint8_t value)
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

		uint8_t ReadRAM(uint16_t addr)
		{
			return _mappedRomBanks[(addr >> 14) & 0x01][addr & 0x3FFF];
		}

		uint8_t ReadVRAM(uint16_t addr)
		{
			return _mappedVromBanks[(addr >> 12) & 0x01][addr & 0x0FFF];
		}

		void WriteVRAM(uint16_t addr, uint8_t value)
		{
			_mappedVromBanks[(addr >> 12) & 0x01][addr & 0x0FFF] = value;
		}
		
		MirroringType GetMirroringType()
		{
			return _mirroringType;
		}
};
