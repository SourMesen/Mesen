#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "PPU.h"

class Nanjing : public BaseMapper
{
private:
	uint8_t _registers[5];
	bool _toggle;
	bool _autoSwitchCHR;

	void UpdateState()
	{
		uint8_t prgPage = (_registers[0] & 0x0F) | ((_registers[2] & 0x0F) << 4);

		_autoSwitchCHR = (_registers[0] & 0x80) == 0x80;

		SelectPRGPage(0, prgPage);
	}

protected:
	virtual uint16_t GetPRGPageSize() override { return 0x8000; }
	virtual uint16_t GetCHRPageSize() override {	return 0x1000; }

	virtual void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		ArrayInfo<uint8_t> registers = { _registers, 5 };
		Stream(registers, _toggle, _autoSwitchCHR);
	}

	void InitMapper() override 
	{
		memset(_registers, 0, sizeof(_registers));
		_autoSwitchCHR = false;
		
		//"Initial value of this register is 1, initial value of "trigger" is 0."
		_toggle = false;
		_registers[4] = 1;

		SelectPRGPage(0, 0);
		SelectCHRPage(0, 0);
		SelectCHRPage(1, 0);
	}

	uint16_t RegisterStartAddress() override { return 0x5000; }
	uint16_t RegisterEndAddress() override { return 0x5FFF; }
	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		if(addr >= 0x5000 && addr <= 0x5FFF) {
			//"(Address is masked with 0x7300, except for 5101)"
			if(addr == 0x5101) {
				if(_registers[4] != 0 && value == 0) {
					//"If the value of this register is changed from nonzero to zero, "trigger" is toggled (XORed with 1)"
					_toggle = !_toggle;
				}
				_registers[4] = value;
			} else {
				switch(addr & 0x7300) {
					case 0x5000:
						_registers[0] = value;
						if(!(_registers[0] & 0x80) && PPU::GetCurrentScanline() < 128) {
							SelectCHRPage(0, 0);
							SelectCHRPage(1, 1);
						}
						UpdateState();
						break;
					case 0x5100:
						_registers[1] = value;
						if(value == 6) {
							SelectPRGPage(0, 3);
						}
						break;
					case 0x5200:
						_registers[2] = value;
						UpdateState();
						break;
					case 0x5300: _registers[3] = value; break;
				}
			}
		}
	}

public:
	uint8_t ReadRAM(uint16_t addr) override
	{
		if(addr >= 0x5000 && addr <= 0x5FFF) {
			//"Reading: (Address is masked with 0x7700)"
			switch(addr & 0x7700) {
				case 0x5100:
					//"5100 = Returns value of 5300"
					return _registers[3];

				case 0x5500:
					//"5500 = If "trigger" is 1, returns value of 5300, otherwise returns 0 "
					if(_toggle) {
						return _registers[3];
					}
					break;
			}
			return 0;
		} else {
			return BaseMapper::ReadRAM(addr);
		}
	}

	virtual void NotifyVRAMAddressChange(uint16_t addr) override
	{
		if(_autoSwitchCHR) {
			if(PPU::GetCurrentScanline() == 239) {
				SelectCHRPage(0, 0);
				SelectCHRPage(1, 0);
			} else if(PPU::GetCurrentScanline() == 127) {
				SelectCHRPage(0, 1);
				SelectCHRPage(1, 1);
			}
		}
	}
};