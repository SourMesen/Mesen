#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Mapper108 : public BaseMapper
{
private:
	uint8_t _reg;

protected:
	virtual uint16_t RegisterStartAddress() override { return 0x8000; }
	virtual uint16_t RegisterEndAddress() override { return 0x8FFF; }
	virtual uint16_t GetPRGPageSize() override { return 0x2000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		//Used by Bubble Bobble hack only
		AddRegisterRange(0xF000, 0xFFFF, MemoryOperation::Write);

		_reg = 0;

		SelectPrgPage4x(0, -4);
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_reg);

		if(!saving) {
			UpdateState();
		}
	}

	void UpdateState()
	{
		SetCpuMemoryMapping(0x6000, 0x7FFF, _reg, PrgMemoryType::PrgRom);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		_reg = value;
		UpdateState();
	}
};
