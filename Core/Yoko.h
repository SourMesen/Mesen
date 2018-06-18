#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "CPU.h"
#include "MemoryManager.h"

class Yoko : public BaseMapper
{
	uint8_t _regs[7];
	uint8_t _exRegs[4];
	uint8_t _mode;
	uint8_t _bank;
	uint16_t _irqCounter;
	bool _irqEnabled;
	uint8_t _dipSwitch;

protected:
	virtual uint16_t RegisterStartAddress() override { return 0x5000; }
	virtual uint16_t RegisterEndAddress() override { return 0x5FFF; }
	virtual uint16_t GetPRGPageSize() override { return 0x2000; }
	virtual uint16_t GetCHRPageSize() override { return 0x800; }
	virtual bool AllowRegisterRead() override { return true; }

	void InitMapper() override
	{
		memset(_regs, 0, sizeof(_regs));
		memset(_exRegs, 0, sizeof(_exRegs));
		_mode = 0;
		_bank = 0;
		_irqCounter = 0;
		_irqEnabled = false;
		_dipSwitch = 3;

		RemoveRegisterRange(0x5000, 0x53FF, MemoryOperation::Write);
		AddRegisterRange(0x8000, 0xFFFF, MemoryOperation::Write);

		UpdateState();
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);

		ArrayInfo<uint8_t> regs { _regs, 7 };
		ArrayInfo<uint8_t> exRegs { _exRegs, 4 };
		Stream(regs, exRegs, _mode, _bank, _irqCounter, _irqEnabled, _dipSwitch);
	}

	void Reset(bool softReset) override
	{
		if(softReset) {
			_dipSwitch = (_dipSwitch + 1) & 0x03;
			_mode = 0;
			_bank = 0;
		}
	}

	void ProcessCpuClock() override
	{
		if(_irqEnabled) {
			_irqCounter--;
			if(_irqCounter == 0) {
				_irqEnabled = false;
				_irqCounter = 0xFFFF;
				CPU::SetIRQSource(IRQSource::External);
			}
		}
	}

	void UpdateState()
	{
		SetMirroringType(_mode & 0x01 ? MirroringType::Horizontal : MirroringType::Vertical);

		SelectCHRPage(0, _regs[3]);
		SelectCHRPage(1, _regs[4]);
		SelectCHRPage(2, _regs[5]);
		SelectCHRPage(3, _regs[6]);

		if(_mode & 0x10) {
			uint32_t outer = (_bank & 0x08) << 1;
			SelectPRGPage(0, outer | (_regs[0] & 0x0F));
			SelectPRGPage(1, outer | (_regs[1] & 0x0F));
			SelectPRGPage(2, outer | (_regs[2] & 0x0F));
			SelectPRGPage(3, outer | 0x0F);
		} else if(_mode & 0x08) {
			SelectPrgPage4x(0, (_bank & 0xFE) << 1);
		} else {
			SelectPrgPage2x(0, _bank << 1);
			SelectPrgPage2x(1, -2);
		}
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		if(addr <= 0x53FF) {
			return (MemoryManager::GetOpenBus() & 0xFC) | _dipSwitch;
		} else {
			return _exRegs[addr & 0x03];
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr < 0x8000) {
			_exRegs[addr & 0x03] = value;
		} else {
			switch(addr & 0x8C17) {
				case 0x8000: _bank = value; UpdateState(); break;
				case 0x8400: _mode = value; UpdateState(); break;
				case 0x8800:
					_irqCounter = (_irqCounter & 0xFF00) | value;
					CPU::ClearIRQSource(IRQSource::External);
					break;
				case 0x8801:
					_irqEnabled = (_mode & 0x80) != 0;
					_irqCounter = (_irqCounter & 0xFF) | (value << 8);
					break;
				case 0x8c00: _regs[0] = value; UpdateState(); break;
				case 0x8c01: _regs[1] = value; UpdateState(); break;
				case 0x8c02: _regs[2] = value; UpdateState(); break;
				case 0x8c10: _regs[3] = value; UpdateState(); break;
				case 0x8c11: _regs[4] = value; UpdateState(); break;
				case 0x8c16: _regs[5] = value; UpdateState(); break;
				case 0x8c17: _regs[6] = value; UpdateState(); break;
			}
		}
	}
};
