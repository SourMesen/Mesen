#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Eh8813A : public BaseMapper
{
private:
	bool _alterReadAddress;

protected:
	uint32_t GetDipSwitchCount() override { return 4; }
	uint16_t GetPRGPageSize() override { return 0x4000; }
	uint16_t GetCHRPageSize() override { return 0x2000; }
	bool AllowRegisterRead() override {	return true; }

	void InitMapper() override
	{
		SetMirroringType(MirroringType::Vertical);
	}

	void Reset(bool softReset) override
	{
		WriteRegister(0x8000, 0);
		_alterReadAddress = false;
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_alterReadAddress);
	}

	uint8_t ReadRegister(uint16_t addr) override
	{
		if(_alterReadAddress) {
			addr = (addr & 0xFFF0) + GetDipSwitches();
		}
		return InternalReadRam(addr);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if((addr & 0x0100) == 0) {
			_alterReadAddress = (addr & 0x40) == 0x40;

			if(addr & 0x80) {
				SelectPRGPage(0, addr & 0x07);
				SelectPRGPage(1, addr & 0x07);
			} else {
				SelectPrgPage2x(0, addr & 0x06);
			}

			SelectCHRPage(0, value & 0x0F);
		}
	}
};