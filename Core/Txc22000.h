#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "MemoryManager.h"
#include "TxcChip.h"

class Txc22000 : public BaseMapper
{
private:
	TxcChip _txc = TxcChip(false);
	uint8_t _chrBank;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }
	virtual uint16_t RegisterStartAddress() override { return 0x8000; }
	virtual uint16_t RegisterEndAddress() override { return 0xFFFF; }
	virtual bool AllowRegisterRead() override { return true; }

	void InitMapper() override
	{
		AddRegisterRange(0x4100, 0x5FFF, MemoryOperation::Any);
		RemoveRegisterRange(0x8000, 0xFFFF, MemoryOperation::Read);

		_chrBank = 0;
		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(&_txc);
		Stream(_chrBank);
	}

	void UpdateState()
	{
		SelectPRGPage(0, _txc.GetOutput() & 0x03);
		SelectCHRPage(0, _chrBank);
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		uint8_t openBus = _console->GetMemoryManager()->GetOpenBus();
		uint8_t value = openBus;
		if((addr & 0x103) == 0x100) {
			value = (openBus & 0xCF) | ((_txc.Read() << 4) & 0x30);
		}
		UpdateState();
		return value;
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if((addr & 0xF200) == 0x4200) {
			_chrBank = value;
		}
		_txc.Write(addr, (value >> 4) & 0x03);
		UpdateState();
	}
};