#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"

class JalecoSs88006 : public BaseMapper
{
	protected:
		virtual uint16_t GetPRGPageSize() { return 0x2000; }
		virtual uint16_t GetCHRPageSize() {	return 0x0400; }

		const vector<uint16_t> _irqMask = { { 0xFFFF, 0x0FFF, 0x00FF, 0x000F } };

		uint8_t _prgBanks[3];
		uint8_t _chrBanks[8];
		uint8_t _irqReloadValue[4];
		uint16_t _irqCounter;
		uint8_t _irqCounterSize;
		bool _irqEnabled;

		void InitMapper() 
		{
			memset(_prgBanks, 0, 3);
			memset(_chrBanks, 0, 8);
			memset(_irqReloadValue, 0, 4);
			_irqCounter = 0;
			_irqCounterSize = 0;
			_irqEnabled = false;

			SelectPRGPage(3, -1);
		}

		virtual void StreamState(bool saving)
		{
			BaseMapper::StreamState(saving);

			ArrayInfo<uint8_t> prgBanks = { _prgBanks, 3 };
			ArrayInfo<uint8_t> chrBanks = { _chrBanks, 8 };
			ArrayInfo<uint8_t> irqReloadValue = { _irqReloadValue, 4 };
			Stream(_irqCounter, _irqCounterSize, _irqEnabled, prgBanks, chrBanks, irqReloadValue);
		}

		void SetMirroring(uint8_t value)
		{
			switch(value) {
				case 0: SetMirroringType(MirroringType::Horizontal); break;
				case 1: SetMirroringType(MirroringType::Vertical); break;
				case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
				case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
			}
		}

		void UpdatePrgBank(uint8_t bankNumber, uint8_t value, bool updateUpperBits)
		{
			if(updateUpperBits) {
				_prgBanks[bankNumber] = (_prgBanks[bankNumber] & 0x0F) | (value << 4);
			} else {
				_prgBanks[bankNumber] = (_prgBanks[bankNumber] & 0xF0) | value;
			}

			SelectPRGPage(bankNumber, _prgBanks[bankNumber]);
		}

		void UpdateChrBank(uint8_t bankNumber, uint8_t value, bool updateUpperBits)
		{
			if(updateUpperBits) {
				_chrBanks[bankNumber] = (_chrBanks[bankNumber] & 0x0F) | (value << 4);
			} else {
				_chrBanks[bankNumber] = (_chrBanks[bankNumber] & 0xF0) | value;
			}

			SelectCHRPage(bankNumber, _chrBanks[bankNumber]);
		}

		virtual void ProcessCpuClock()
		{
			//Clock irq counter every memory read/write (each cpu cycle either reads or writes memory)
			ClockIrqCounter();
		}

		void ReloadIrqCounter()
		{
			_irqCounter = _irqReloadValue[0] | (_irqReloadValue[1] << 4) | (_irqReloadValue[2] << 8) | (_irqReloadValue[3] << 12);
		}

		void ClockIrqCounter()
		{
			if(_irqEnabled) {
				uint16_t counter = _irqCounter & _irqMask[_irqCounterSize];

				if(--counter == 0) {
					CPU::SetIRQSource(IRQSource::External);
				}

				_irqCounter = (_irqCounter & ~_irqMask[_irqCounterSize]) | (counter & _irqMask[_irqCounterSize]);
			}
		}

		void WriteRegister(uint16_t addr, uint8_t value)
		{
			bool updateUpperBits = (addr & 0x01) == 0x01;
			value &= 0x0F;

			switch(addr & 0xF003) {
				case 0x8000: case 0x8001: UpdatePrgBank(0, value, updateUpperBits); break;
				case 0x8002: case 0x8003: UpdatePrgBank(1, value, updateUpperBits); break;
				case 0x9000: case 0x9001: UpdatePrgBank(2, value, updateUpperBits); break;

				case 0xA000: case 0xA001: UpdateChrBank(0, value, updateUpperBits); break;
				case 0xA002: case 0xA003: UpdateChrBank(1, value, updateUpperBits); break;
				case 0xB000: case 0xB001: UpdateChrBank(2, value, updateUpperBits); break;
				case 0xB002: case 0xB003: UpdateChrBank(3, value, updateUpperBits); break;
				case 0xC000: case 0xC001: UpdateChrBank(4, value, updateUpperBits); break;
				case 0xC002: case 0xC003: UpdateChrBank(5, value, updateUpperBits); break;
				case 0xD000: case 0xD001: UpdateChrBank(6, value, updateUpperBits); break;
				case 0xD002: case 0xD003: UpdateChrBank(7, value, updateUpperBits); break;

				case 0xE000: case 0xE001: case 0xE002:	case 0xE003:
					_irqReloadValue[addr & 0x03] = value;
					break;

				case 0xF000:
					CPU::ClearIRQSource(IRQSource::External);
					ReloadIrqCounter();
					break;

				case 0xF001:
					CPU::ClearIRQSource(IRQSource::External);
					_irqEnabled = (value & 0x01) & 0x01;
					if(value & 0x08) {
						_irqCounterSize = 3; //4-bit counter
					} else if(value & 0x04) {
						_irqCounterSize = 2; //8-bit counter
					} else if(value & 0x02) {
						_irqCounterSize = 1; //12-bit counter
					} else {
						_irqCounterSize = 0; //16-bit counter
					}
					break;

				case 0xF002:
					SetMirroring(value & 0x03);
					break;

				case 0xF003:
					//Expansion audio, not supported yet
					break;
			}
		}
};