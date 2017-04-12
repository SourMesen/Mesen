#pragma once
#include "stdafx.h"
#include "MMC3.h"

class MMC3_Fk23C : public MMC3
{
private:
	bool _isFk23Ca;
	uint8_t _exRegs[8];
	uint8_t _chrReg;

protected:
	uint32_t GetChrRamSize() override { return _isFk23Ca ? 0x2000 : 0; }
	uint16_t GetChrRamPageSize() override { return 0x400; }

	void InitMapper() override
	{
		_exRegs[0] = _exRegs[1] = _exRegs[2] = _exRegs[3] = 0;
		_exRegs[4] = _exRegs[5] = _exRegs[6] = _exRegs[7] = 0xFF;
		_chrReg = 0;

		AddRegisterRange(0x5000, 0x5FFF, MemoryOperation::Write);

		MMC3::InitMapper();
	}

	void Reset(bool softReset) override
	{
		_exRegs[0] = _exRegs[1] = _exRegs[2] = _exRegs[3] = 0;
		_exRegs[4] = _exRegs[5] = _exRegs[6] = _exRegs[7] = 0xFF;
		_chrReg = 0;
	}

	void StreamState(bool saving) override 
	{
		MMC3::StreamState(saving);
		ArrayInfo<uint8_t> exRegs{ _exRegs,8 };
		Stream(_chrReg, exRegs);
	}

	void SelectCHRPage(uint16_t slot, uint16_t page, ChrMemoryType memoryType = ChrMemoryType::Default) override
	{
		if(_exRegs[0] & 0x20) {
			MMC3::SelectCHRPage(slot, page, _isFk23Ca ? ChrMemoryType::ChrRam : ChrMemoryType::Default);
		} else {
			uint16_t base = (_exRegs[2] & 0x7F) << 3;
			MMC3::SelectCHRPage(slot, page | base, memoryType);
		}
	}

	void UpdateChrMapping() override
	{
		if(_exRegs[0] & 0x40) {
			SelectChrPage8x(0, (_exRegs[2] | _chrReg) << 3);
		} else if(_exRegs[3] & 0x02) {
			uint16_t base = (_exRegs[2] & 0x7F) << 3;
			int slotBase = _chrMode ? 4 : 0;
			MMC3::SelectCHRPage(0 ^ slotBase, _registers[0] | base);
			MMC3::SelectCHRPage(1 ^ slotBase, _exRegs[6] | base);
			MMC3::SelectCHRPage(2 ^ slotBase, _registers[1] | base);
			MMC3::SelectCHRPage(3 ^ slotBase, _exRegs[7] | base);
			MMC3::SelectCHRPage(4 ^ slotBase, _registers[2] | base);
			MMC3::SelectCHRPage(5 ^ slotBase, _registers[3] | base);
			MMC3::SelectCHRPage(6 ^ slotBase, _registers[4] | base);
			MMC3::SelectCHRPage(7 ^ slotBase, _registers[5] | base);
		} else {
			MMC3::UpdateChrMapping();
		}
	}

	void SelectPRGPage(uint16_t slot, uint16_t page, PrgMemoryType memoryType = PrgMemoryType::PrgRom) override
	{
		if((_exRegs[0] & 0x07) == 0x04) {
			SelectPrgPage4x(0, _exRegs[1] << 1);
		} else if((_exRegs[0] & 0x07) == 0x03) {
			SelectPrgPage2x(0, _exRegs[1] << 1);
			SelectPrgPage2x(1, _exRegs[1] << 1);
		} else {
			if(_exRegs[0] & 0x03) {
				uint32_t blocksize = 6 - (_exRegs[0] & 0x03);
				uint32_t mask = (1 << blocksize) - 1;
				MMC3::SelectPRGPage(slot, (page & mask) | (_exRegs[1] << 1));
			} else {
				MMC3::SelectPRGPage(slot, page & (_isFk23Ca ? 0x3F : 0x7F));
			}

			if(_exRegs[3] & 0x02) {
				MMC3::SelectPRGPage(2, _exRegs[4]);
				MMC3::SelectPRGPage(3, _exRegs[5]);
			}
		}
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr >= 0x8000) {
			if(_exRegs[0] & 0x40) {
				if(_exRegs[0] & 0x30) {
					_chrReg = 0;
				} else {
					_chrReg = value & 0x03;
					UpdateChrMapping();
				}
			} else {
				if(addr == 0x8001 && (_exRegs[3] & 0x02) && (GetState().Reg8000 & 0x08)) {
					_exRegs[0x04 | (GetState().Reg8000 & 0x03)] = value;
					UpdateChrMapping();
					UpdatePrgMapping();
				} else {
					if(addr < 0xC000) {
						if(HasChrRam()) {
							if((addr == 0x8000) && (value == 0x46)) {
								value = 0x47;
							} else if((addr == 0x8000) && (value == 0x47)) {
								value = 0x46;
							}
						}
					}
					MMC3::WriteRegister(addr, value);
				}
			}
		} else {
			if(addr & 0x10) {
				_exRegs[addr & 0x03] = value;
				if(((_exRegs[0] & 0xF0) == 0x20) || (addr & 0x03) == 1 || (addr & 0x03) == 2) {
					UpdateState();
				}
			}

			if(_isFk23Ca && (_exRegs[3] & 0x02)) {
				_exRegs[0] &= ~7;
			}
		}
	}

public:
	MMC3_Fk23C(bool isFk23Ca)
	{
		_isFk23Ca = isFk23Ca;
	}
};