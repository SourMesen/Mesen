#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class UnlD1038 : public BaseMapper
{
private:
	bool _returnDipSwitch;

protected:
	uint32_t GetDipSwitchCount() override { return 2; }
	uint16_t GetPRGPageSize() override { return 0x4000; }
	uint16_t GetCHRPageSize() override { return 0x2000; }
	bool AllowRegisterRead() override { return true; }

	void InitMapper() override
	{
		_returnDipSwitch = false;
		WriteRegister(0x8000, 0);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_returnDipSwitch);
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		if(_returnDipSwitch) {
			return GetDipSwitches();
		} else {
			return InternalReadRam(addr);
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr & 0x80) {
			SelectPRGPage(0, (addr & 0x70) >> 4);
			SelectPRGPage(1, (addr & 0x70) >> 4);
		} else {
			SelectPrgPage2x(0, (addr & 0x60) >> 4);
		}
		SelectCHRPage(0, addr & 0x07);
		SetMirroringType(addr & 0x08 ? MirroringType::Horizontal : MirroringType::Vertical);
		_returnDipSwitch = (addr & 0x100) == 0x100;
	}
};