#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper253 : public BaseMapper
{
private:
	uint8_t _chrLow[8];
	uint8_t _chrHigh[8];
	bool _forceChrRom;
	uint8_t _irqReloadValue;
	uint8_t _irqCounter;
	bool _irqEnabled;
	uint16_t _irqScaler;

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x400; }
	virtual uint32_t GetChrRamSize() { return 0x800; }
	virtual uint16_t GetChrRamPageSize() { return 0x400; }

	void InitMapper()
	{
		memset(_chrLow, 0, sizeof(_chrLow));
		memset(_chrHigh, 0, sizeof(_chrHigh));
		_forceChrRom = false;
		_irqReloadValue = 0;
		_irqScaler = 0;
		_irqCounter = 0;
		_irqEnabled = false;

		SelectPRGPage(2, -2);
		SelectPRGPage(3, -1);
	}

	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);

		ArrayInfo<uint8_t> chrLow{ _chrLow, 8 };
		ArrayInfo<uint8_t> chrHigh{ _chrHigh, 8 };
		Stream(_forceChrRom, _irqReloadValue, _irqCounter, _irqEnabled, _irqScaler, chrLow, chrHigh);

		if(!saving) {
			UpdateChr();
		}
	}

	void UpdateChr()
	{
		for(uint16_t i = 0; i < 8; i++) {
			uint16_t page = _chrLow[i] | (_chrHigh[i] << 8);
			if((_chrLow[i] == 4 || _chrLow[i] == 5) && !_forceChrRom) {
				SelectCHRPage(i, page & 0x01, ChrMemoryType::ChrRam);
			} else {
				SelectCHRPage(i, page);
			}
		}
	}

	void ProcessCpuClock()
	{
		if(_irqEnabled) {
			_irqScaler++;
			if(_irqScaler >= 114) {
				_irqScaler = 0;
				_irqCounter++;
				if(_irqCounter == 0) {
					_irqCounter = _irqReloadValue;
					CPU::SetIRQSource(IRQSource::External);
				}
			}
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr >= 0xB000 && addr <= 0xE00C) {
			uint8_t slot = ((((addr & 0x08) | (addr >> 8)) >> 3) + 2) & 0x07;
			uint8_t shift = addr & 0x04;
			uint8_t chrLow = (_chrLow[slot] & (0xF0 >> shift)) | (value << shift);
			_chrLow[slot] = chrLow;
			if(slot == 0) {
				if(chrLow == 0xc8) {
					_forceChrRom = false;
				} else if(chrLow == 0x88) {
					_forceChrRom = true;
				}
			}
			if(shift) {
				_chrHigh[slot] = value >> 4;
			}
			UpdateChr();
		} else {
			switch(addr) {
				case 0x8010: SelectPRGPage(0, value); break;
				case 0xA010: SelectPRGPage(1, value); break;
				case 0x9400:
					switch(value & 0x03) {
						case 0: SetMirroringType(MirroringType::Vertical); break;
						case 1: SetMirroringType(MirroringType::Horizontal); break;
						case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
						case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
					}
					break;

				case 0xF000:
					_irqReloadValue = (_irqReloadValue & 0xF0) | (value & 0x0F);
					CPU::ClearIRQSource(IRQSource::External);
					break;

				case 0xF004:
					_irqReloadValue = (_irqReloadValue & 0x0F) | (value << 4);
					CPU::ClearIRQSource(IRQSource::External);
					break;

				case 0xF008:
					_irqCounter = _irqReloadValue;
					_irqEnabled = (value & 0x02) == 0x02;
					_irqScaler = 0;
					CPU::ClearIRQSource(IRQSource::External);
					break;
			}
		}
	}
};