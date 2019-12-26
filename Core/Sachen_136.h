#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "TxcChip.h"

class Sachen_136 : public BaseMapper
{
private:
	TxcChip _txc = TxcChip(true);

protected:
	uint16_t GetPRGPageSize() override { return 0x8000; }
	uint16_t GetCHRPageSize() override { return 0x2000; }
	uint16_t RegisterStartAddress() override { return 0x8000; }
	uint16_t RegisterEndAddress() override { return 0xFFFF; }
	bool AllowRegisterRead() override { return true; }

	void InitMapper() override
	{
		AddRegisterRange(0x4020, 0x5FFF, MemoryOperation::Any);
		RemoveRegisterRange(0x8000, 0xFFFF, MemoryOperation::Read);

		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(&_txc);
	}

	void UpdateState()
	{
		SelectCHRPage(0, _txc.GetOutput());
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		uint8_t openBus = _console->GetMemoryManager()->GetOpenBus();
		uint8_t value;
		if((addr & 0x103) == 0x100) {
			value = (openBus & 0xC0) | (_txc.Read() & 0x3F);
		} else {
			value = openBus;
		}
		UpdateState();
		return value;
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		_txc.Write(addr, value & 0x3F);
		UpdateState();
	}
};