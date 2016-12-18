#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "MemoryManager.h"

class Txc22000 : public BaseMapper
{
private:
	uint8_t _state;
	bool _prgBankingMode;
	uint8_t _prgBank;

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

		_state = 0;
		_prgBank = 0;
		_prgBankingMode = 0;

		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		Stream(_state, _prgBank, _prgBankingMode);
	}

	virtual uint8_t ReadRegister(uint16_t addr) override
	{
		return MemoryManager::GetOpenBus() & 0xCF | (_state << 4);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr < 0x8000) {
			switch(addr & 0xE303) {
				//"when M=0, copy PP to RR. When M=1, RR=RR+1"
				case 0x4100:
					if(_prgBankingMode) {
						_state++;
					} else {
						_state = _prgBank;
					}
					break;

				case 0x4101: break; //"$4101: no visible effect"
				
				case 0x4102: _prgBank = (value >> 4) & 0x03; break;
				case 0x4103: _prgBankingMode = (value >> 4) & 0x01; break;
				
				case 0x4200: case 0x4201: case 0x4202: case 0x4203:
					SelectCHRPage(0, value & 0x0F);
					break;
			}
		} else {
			SelectPRGPage(0, _state);
		}

	}
};