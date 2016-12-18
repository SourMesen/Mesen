#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Ac08 : public BaseMapper
{
private:
	uint8_t _reg;

protected:
	uint16_t GetPRGPageSize() override { return 0x2000; }
	uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		AddRegisterRange(0x4025, 0x4025, MemoryOperation::Write);

		_reg = 0;

		SelectPrgPage4x(0, -4);
		SelectCHRPage(0, 0);
		UpdateState();
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
		if(addr == 0x4025) {
			SetMirroringType(value & 0x08 ? MirroringType::Horizontal : MirroringType::Vertical);
		} else {
			if(addr == 0x8001) {
				//Green beret
				_reg = (value >> 1) & 0x0F;
			} else {
				//Castlevania?
				_reg = value & 0x0F;
			}
			UpdateState();
		}
	}
};