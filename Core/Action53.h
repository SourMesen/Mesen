#pragma once
#include "stdafx.h"
#include "BaseMapper.h"

class Action53 : public BaseMapper
{
private:
	uint8_t _selectedReg;
	uint8_t _regs[4];
	uint8_t _mirroringBit;

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x4000; }
	virtual uint16_t GetCHRPageSize() override { return 0x2000; }

	void InitMapper() override
	{
		_selectedReg = 0;
		memset(_regs, 0, sizeof(_regs));

		AddRegisterRange(0x5000, 0x5FFF, MemoryOperation::Write);

		SelectPRGPage(1, -1);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);

		ArrayInfo<uint8_t> regs{ _regs,4 };
		Stream(_selectedReg, _mirroringBit, regs);

		if(!saving) {
			UpdateState();
		}
	}

	void UpdateState()
	{
		uint8_t mirroring = _regs[2] & 0x03;
		if(!(mirroring & 0x02)) {
			mirroring = _mirroringBit;
		}

		switch(mirroring) {
			case 0: SetMirroringType(MirroringType::ScreenAOnly); break;
			case 1: SetMirroringType(MirroringType::ScreenBOnly); break;
			case 2: SetMirroringType(MirroringType::Vertical); break;
			case 3: SetMirroringType(MirroringType::Horizontal); break;
		}

		uint8_t gameSize = (_regs[2] & 0x30) >> 4;
		uint8_t prgSize = (_regs[2] & 0x08) >> 3;
		uint8_t slotSelect = (_regs[2] & 0x04) >> 2;
		uint8_t chrSelect = _regs[0] & 0x03;
		uint8_t prgSelect = _regs[1] & 0x0F;
		uint8_t outerPrgSelect = (_regs[3] & 0x3F) << 1;

		SelectCHRPage(0, chrSelect);

		if(prgSize) {
			uint8_t bank = (slotSelect ? 0 : 1);
			switch(gameSize) {
				case 0: SelectPRGPage(bank, (outerPrgSelect & 0x7E) | (prgSelect & 0x01)); break;
				case 1: SelectPRGPage(bank, (outerPrgSelect & 0x7C) | (prgSelect & 0x03)); break;
				case 2: SelectPRGPage(bank, (outerPrgSelect & 0x78) | (prgSelect & 0x07)); break;
				case 3: SelectPRGPage(bank, (outerPrgSelect & 0x70) | (prgSelect & 0x0F)); break;
			}
			SelectPRGPage(slotSelect ? 1 : 0, (outerPrgSelect & 0x7E) | slotSelect);
		} else {
			prgSelect <<= 1;
			uint8_t outerAnd[4]{ 0x7E, 0x7C, 0x78, 0x70 };
			uint8_t innerAnd[4]{ 0x01, 0x03, 0x07, 0x0F };
			SelectPRGPage(0, (outerPrgSelect & outerAnd[gameSize]) | (prgSelect & innerAnd[gameSize]));
			SelectPRGPage(1, (outerPrgSelect & outerAnd[gameSize]) | ((prgSelect | 0x01) & innerAnd[gameSize]));
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr <= 0x5FFF) {
			_selectedReg = ((value & 0x80) >> 6) | (value & 0x01);
		} else if(addr >= 0x8000) {
			if(_selectedReg <= 1) {
				_mirroringBit = (value >> 4) & 0x01;
			} else if(_selectedReg == 2) {
				_mirroringBit = (value & 0x01);
			}

			_regs[_selectedReg] = value & 0x3F;
			UpdateState();
		}
	}
};
