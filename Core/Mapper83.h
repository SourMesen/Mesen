#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"
#include "MemoryManager.h"

class Mapper83 : public BaseMapper
{
	uint8_t _regs[11];
	uint8_t _exRegs[4];
	bool _is2kBank;
	bool _isNot2kBank;
	uint8_t _mode;
	uint8_t _bank;
	uint16_t _irqCounter;
	bool _irqEnabled;

protected:
	uint32_t GetDipSwitchCount() override { return 2; }
	uint16_t GetPRGPageSize() override { return 0x2000; }
	uint16_t GetCHRPageSize() override { return 0x400; }
	bool AllowRegisterRead() override { return true; }

	void InitMapper() override
	{
		memset(_regs, 0, sizeof(_regs));
		memset(_exRegs, 0, sizeof(_exRegs));
		_is2kBank = false;
		_isNot2kBank = false;
		_mode = 0;
		_bank = 0;
		_irqCounter = 0;
		_irqEnabled = false;

		AddRegisterRange(0x5000, 0x5000, MemoryOperation::Read);
		AddRegisterRange(0x5100, 0x5103, MemoryOperation::Any);
		RemoveRegisterRange(0x8000, 0xFFFF, MemoryOperation::Read);

		UpdateState();
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);

		ArrayInfo<uint8_t> regs{ _regs, 11 };
		ArrayInfo<uint8_t> exRegs{ _exRegs, 4 };
		Stream(regs, exRegs, _is2kBank, _isNot2kBank, _mode, _bank, _irqCounter, _irqEnabled);
	}

	void ProcessCpuClock() override
	{
		if(_irqEnabled) {
			_irqCounter--;
			if(_irqCounter == 0) {
				_irqEnabled = false;
				_irqCounter = 0xFFFF;
				_console->GetCpu()->SetIrqSource(IRQSource::External);
			}
		}
	}

	void UpdateState()
	{
		switch(_mode & 0x03) {
			case 0: SetMirroringType(MirroringType::Vertical); break;
			case 1: SetMirroringType(MirroringType::Horizontal); break;
			case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
			case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
		}
		
		if(_is2kBank && !_isNot2kBank) {
			SelectChrPage2x(0, _regs[0] << 1);
			SelectChrPage2x(1, _regs[1] << 1);
			SelectChrPage2x(2, _regs[6] << 1);
			SelectChrPage2x(3, _regs[7] << 1);
		} else {
			for(int i = 0; i < 8; i++) {
				SelectCHRPage(i, _regs[i] | ((_bank & 0x30) << 4));
			}
		}
		
		if(_mode & 0x40) {
			SelectPrgPage2x(0, (_bank & 0x3F) << 1);
			SelectPrgPage2x(1, ((_bank & 0x30) | 0x0F) << 1);
		} else {
			SelectPRGPage(0, _regs[8]);
			SelectPRGPage(1, _regs[9]);
			SelectPRGPage(2, _regs[10]);
			SelectPRGPage(3, -1);
		}
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		if(addr == 0x5000) {
			return (_console->GetMemoryManager()->GetOpenBus() & 0xFC) | GetDipSwitches();
		} else {
			return _exRegs[addr & 0x03];
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr < 0x8000) {
			_exRegs[addr & 0x03] = value;
		} else if(addr >= 0x8300 && addr <= 0x8302) {
			_mode &= 0xBF;
			_regs[addr - 0x8300 + 8] = value;
			UpdateState();
		} else if(addr >= 0x8310 && addr <= 0x8317) {
			_regs[addr - 0x8310] = value;
			if(addr >= 0x8312 && addr <= 0x8315) {
				_isNot2kBank = true;
			}
			UpdateState();
		} else {
			switch(addr) {
				case 0x8000: 
					_is2kBank = true;
					_bank = value;
					_mode |= 0x40;
					UpdateState();
					break;

				case 0xB000: case 0xB0FF: case 0xB1FF:
					// Dragon Ball Z Party [p1] BMC
					_bank = value; 
					_mode |= 0x40;
					UpdateState();
					break;

				case 0x8100: 
					_mode = value | (_mode & 0x40);
					UpdateState();
					break;

				case 0x8200: 
					_irqCounter = (_irqCounter & 0xFF00) | value;
					_console->GetCpu()->ClearIrqSource(IRQSource::External);
					break;

				case 0x8201: 
					_irqEnabled = (_mode & 0x80) == 0x80;
					_irqCounter = (_irqCounter & 0xFF) | (value << 8); 
					break;
			}
		}
	}
};
