#pragma once

#include "stdafx.h"
#include "MMC3.h"

class Mapper112 : public MMC3
{
private:
	uint8_t _currentReg;

protected:
	void InitMapper() override
	{
		MMC3::InitMapper();
		SetMirroringType(MirroringType::Vertical);
	}

	void UpdateMirroring() override
	{
	}

	void StreamState(bool saving) override
	{
		MMC3::StreamState(saving);
		Stream(_currentReg);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr & 0xE001) {
			case 0x8000:
				//Adjust register numbers to match MMC3
				_currentReg = value & 0x07;
				if(_currentReg >= 2) {
					_currentReg -= 2;
				} else {
					_currentReg += 6;
				}
				break;

			case 0xA000:
				_registers[_currentReg] = value;
				break;

			case 0xE000:
				SetMirroringType(value & 0x01 ? MirroringType::Horizontal : MirroringType::Vertical);
				break;
		}
	
		UpdateState();
	}
};