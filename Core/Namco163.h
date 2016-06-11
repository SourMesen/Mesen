#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "Namco163Audio.h"

enum class NamcoVariant
{
	Namco163,
	Namco175,
	Namco340,
	Unknown,
};

class Namco163 : public BaseMapper
{
private:
	Namco163Audio _audio;

	NamcoVariant _variant;
	bool _notNamco340;
	bool _autoDetectVariant;
	uint8_t _writeProtect;
	bool _lowChrNtMode;
	bool _highChrNtMode;
	uint16_t _irqCounter;

	void SetVariant(NamcoVariant variant)
	{
		if(_autoDetectVariant) {
			if(!_notNamco340 || variant != NamcoVariant::Namco340) {
				_variant = variant;
			}
		}
	}

	void UpdateSaveRamAccess()
	{
		PrgMemoryType memType = HasBattery() ? PrgMemoryType::SaveRam : PrgMemoryType::WorkRam;
		if(_variant == NamcoVariant::Namco163) {
			bool globalWriteEnable = (_writeProtect & 0x40) == 0x40;
			SetCpuMemoryMapping(0x6000, 0x67FF, 0, memType, globalWriteEnable && (_writeProtect & 0x01) == 0x00 ? MemoryAccessType::ReadWrite : MemoryAccessType::Read);
			SetCpuMemoryMapping(0x6800, 0x6FFF, 1, memType, globalWriteEnable && (_writeProtect & 0x02) == 0x00 ? MemoryAccessType::ReadWrite : MemoryAccessType::Read);
			SetCpuMemoryMapping(0x7000, 0x77FF, 2, memType, globalWriteEnable && (_writeProtect & 0x04) == 0x00 ? MemoryAccessType::ReadWrite : MemoryAccessType::Read);
			SetCpuMemoryMapping(0x7800, 0x7FFF, 3, memType, globalWriteEnable && (_writeProtect & 0x08) == 0x00 ? MemoryAccessType::ReadWrite : MemoryAccessType::Read);
		} else if(_variant == NamcoVariant::Namco175) {
			SetCpuMemoryMapping(0x6000, 0x7FFF, 0, memType, (_writeProtect & 0x01) == 0x01 ? MemoryAccessType::ReadWrite : MemoryAccessType::Read);
		} else {
			SetCpuMemoryMapping(0x6000, 0x7FFF, 0, memType, MemoryAccessType::NoAccess);
		}
	}

protected:
	virtual uint16_t GetPRGPageSize() { return 0x2000; }
	virtual uint16_t GetCHRPageSize() { return 0x400; }
	virtual uint32_t GetSaveRamPageSize() { return 0x800; }
	virtual bool AllowRegisterRead() { return true; }
	
	void InitMapper()
	{
		switch(_mapperID) {
			case 19: _variant = NamcoVariant::Namco163; _autoDetectVariant = true; break;
			case 210: 
				switch(_subMapperID) {
					case 0: _variant = NamcoVariant::Unknown; _autoDetectVariant = true; break;
					case 1: _variant = NamcoVariant::Namco175; _autoDetectVariant = false; break;
					case 2: _variant = NamcoVariant::Namco340; _autoDetectVariant = false; break;
				}
				break;
		}
		
		_notNamco340 = false;

		_writeProtect = 0;
		_lowChrNtMode = false;
		_highChrNtMode = false;
		_irqCounter = 0;

		AddRegisterRange(0x4800, 0x5FFF, MemoryOperation::Any);
		RemoveRegisterRange(0x6000, 0xFFFF, MemoryOperation::Read);

		SelectPRGPage(3, -1);
		UpdateSaveRamAccess();
	}
	
	void StreamState(bool saving)
	{
		BaseMapper::StreamState(saving);

		SnapshotInfo audio{ &_audio };
		Stream(_variant, _notNamco340, _autoDetectVariant, _writeProtect, _lowChrNtMode, _highChrNtMode, _irqCounter, audio);
		if(!saving) {
			UpdateSaveRamAccess();
		}
	}

	void ProcessCpuClock()
	{
		if(_irqCounter & 0x8000 && (_irqCounter & 0x7FFF) != 0x7FFF) {
			_irqCounter++;
			if((_irqCounter & 0x7FFF) == 0x7FFF) {
				CPU::SetIRQSource(IRQSource::External);
			}
		}

		if(_variant == NamcoVariant::Namco163) {
			_audio.ProcessCpuClock();
		}
	}

	void WriteRAM(uint16_t addr, uint8_t value)
	{
		if(addr >= 0x6000 && addr <= 0x7FFF) {
			_notNamco340 = true;
			if(_variant == NamcoVariant::Namco340) {
				SetVariant(NamcoVariant::Unknown);
			}
		}
		BaseMapper::WriteRAM(addr, value);
	}

	uint8_t ReadRegister(uint16_t addr)
	{
		switch(addr & 0xF800) {
			case 0x4800: return _audio.ReadRegister(addr);
			case 0x5000: return _irqCounter & 0xFF;
			case 0x5800: return (_irqCounter >> 8);
			default:	return BaseMapper::ReadRegister(addr);
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		addr &= 0xF800;

		switch(addr) {
			case 0x4800:
				SetVariant(NamcoVariant::Namco163);
				_audio.WriteRegister(addr, value);
				break;

			case 0x5000:
				SetVariant(NamcoVariant::Namco163);
				_irqCounter = (_irqCounter & 0xFF00) | value;
				CPU::ClearIRQSource(IRQSource::External);
				break;

			case 0x5800:
				SetVariant(NamcoVariant::Namco163);
				_irqCounter = (_irqCounter & 0x00FF) | (value << 8);
				CPU::ClearIRQSource(IRQSource::External);
				break;

			case 0x8000: case 0x8800: case 0x9000: case 0x9800: {
				uint8_t bankNumber = (addr - 0x8000) >> 11;
				if(!_lowChrNtMode && value >= 0xE0 && _variant == NamcoVariant::Namco163) {
					SelectCHRPage(bankNumber, (value & 0x01) == 0x01 ? ChrSpecialPage::NametableB : ChrSpecialPage::NametableA);
				} else {
					SelectCHRPage(bankNumber, value);
				}
				break;
			}

			case 0xA000: case 0xA800: case 0xB000: case 0xB800: {
				uint8_t bankNumber = ((addr - 0xA000) >> 11) + 4;
				if(!_highChrNtMode && value >= 0xE0 && _variant == NamcoVariant::Namco163) {
					SelectCHRPage(bankNumber, (value & 0x01) == 0x01 ? ChrSpecialPage::NametableB : ChrSpecialPage::NametableA);
				} else {
					SelectCHRPage(bankNumber, value);
				}
				break;
			}

			case 0xC000: case 0xC800: case 0xD000: case 0xD800:
				if(addr >= 0xC8000) {
					SetVariant(NamcoVariant::Namco163);
				} else if(_variant != NamcoVariant::Namco163) {
					SetVariant(NamcoVariant::Namco175);
				}

				if(_variant == NamcoVariant::Namco175) {
					_writeProtect = value;
					UpdateSaveRamAccess();
				} else {
					uint8_t bankNumber = ((addr - 0xC000) >> 11) + 8;
					if(value >= 0xE0) {
						SelectCHRPage(bankNumber, (value & 0x01) == 0x01 ? ChrSpecialPage::NametableB : ChrSpecialPage::NametableA);
					} else {
						SelectCHRPage(bankNumber, value);
					}
				}
				break;

			case 0xE000:
				if((value & 0x80) == 0x80) {
					SetVariant(NamcoVariant::Namco340);
				} else if((value & 0x40) == 0x40 && _variant != NamcoVariant::Namco163) {
					SetVariant(NamcoVariant::Namco340);
				}

				SelectPRGPage(0, value & 0x3F);

				if(_variant == NamcoVariant::Namco340) {
					switch((value & 0xC0) >> 6) {
						case 0: SetMirroringType(MirroringType::ScreenAOnly); break;
						case 1: SetMirroringType(MirroringType::Vertical); break;
						case 2: SetMirroringType(MirroringType::Horizontal); break;
						case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
					}
				} else if(_variant == NamcoVariant::Namco163) {
					_audio.WriteRegister(addr, value);
				}
				break;

			case 0xE800:
				SelectPRGPage(1, value & 0x3F);
				if(_variant == NamcoVariant::Namco163) {
					_lowChrNtMode = (value & 0x40) == 0x40;
					_highChrNtMode = (value & 0x80) == 0x80;
				}
				break;

			case 0xF000:
				SelectPRGPage(2, value & 0x3F);
				break;

			case 0xF800:
				SetVariant(NamcoVariant::Namco163);
				if(_variant == NamcoVariant::Namco163) {
					_writeProtect = value;
					UpdateSaveRamAccess();

					_audio.WriteRegister(addr, value);
				}
				break;
		}
	}
};