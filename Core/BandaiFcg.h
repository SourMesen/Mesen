#pragma once
#include "BaseMapper.h"
#include "CPU.h"
#include "Console.h"
#include "MemoryManager.h"
#include "DatachBarcodeReader.h"

class BandaiFcg : public BaseMapper
{
private:
	bool _irqEnabled;
	uint16_t _irqCounter;
	uint16_t _irqReload;
	uint8_t _prgPage;
	uint8_t _prgBankSelect;
	uint8_t _chrRegs[8];
	shared_ptr<DatachBarcodeReader> _barcodeReader;

protected:
	uint16_t GetPRGPageSize() override { return 0x4000; }
	uint16_t GetCHRPageSize() override { return 0x400; }
	uint16_t RegisterStartAddress() override { return 0x6000; }
	uint16_t RegisterEndAddress() override { return 0xFFFF; }
	bool AllowRegisterRead() override { return true; }
	ConsoleFeatures GetAvailableFeatures() override { return _mapperID == 157 ? (ConsoleFeatures)((int)ConsoleFeatures::BarcodeReader | (int)ConsoleFeatures::DatachBarcodeReader) : ConsoleFeatures::None; }

	void InitMapper() override
	{
		memset(_chrRegs, 0, sizeof(_chrRegs));
		_irqEnabled = false;
		_irqCounter = 0;
		_irqReload = 0;
		_prgPage = 0;
		_prgBankSelect = 0;
		
		if(_mapperID == 157) {
			//"Mapper 157 is used for Datach Joint ROM System boards"
			_barcodeReader.reset(new DatachBarcodeReader(_console));
			_mapperControlDevice = _barcodeReader;
		}

		//Only allow reads from 0x6000 to 0x7FFF
		RemoveRegisterRange(0x8000, 0xFFFF, MemoryOperation::Read);

		if(_mapperID != 16 || GetPRGPageCount() >= 0x20) {
			//"For iNES Mapper 153 (with SRAM), the writeable ports must only be mirrored across $8000-$FFFF."
			//"Mappers 157 and 159 do not need to support the FCG-1 and -2 and so should only mirror the ports across $8000-$FFFF."

			//TODO: Check if this is needed
			//RemoveRegisterRange(0x6000, 0x7FFF, MemoryOperation::Any);
		}

		//Last bank
		SelectPRGPage(1, 0x0F);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);

		ArrayInfo<uint8_t> chrRegs{ _chrRegs, 8 };
		Stream(_irqEnabled, _irqCounter, _irqReload, _prgPage, _prgBankSelect, chrRegs);
	}

	void ProcessCpuClock() override
	{
		if(_irqEnabled) {
			//Checking counter before decrementing seems to be the only way to get both
			//Famicom Jump II - Saikyou no 7 Nin (J) and Magical Taruruuto-kun 2 - Mahou Daibouken (J)
			//to work without glitches with the same code.
			if(_irqCounter == 0) {
				_console->GetCpu()->SetIrqSource(IRQSource::External);
			}
			_irqCounter--;
		}
	}
	
	uint8_t ReadRegister(uint16_t addr) override
	{
		//Pretend EEPROM data is always 0
		return (_barcodeReader ? _barcodeReader->GetOutput() : 0) | _console->GetMemoryManager()->GetOpenBus(0xE7);
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr & 0x000F) {
			case 0x00: case 0x01: case 0x02: case 0x03: case 0x04: case 0x05: case 0x06: case 0x07:
				_chrRegs[addr & 0x07] = value;
				if(_mapperID == 153 || GetPRGPageCount() >= 0x20) {
					_prgBankSelect = 0;
					for(int i = 0; i < 8; i++) {
						_prgBankSelect |= (_chrRegs[i] & 0x01) << 4;
					}
					SelectPRGPage(0, _prgPage | _prgBankSelect);
					SelectPRGPage(1, 0x0F | _prgBankSelect);
				} else if(!HasChrRam() && _mapperID != 157) {
					SelectCHRPage(addr & 0x07, value);
				}
				break;

			case 0x08:
				_prgPage = value & 0x0F;
				SelectPRGPage(0, _prgPage | _prgBankSelect);
				break;

			case 0x09:
				switch(value & 0x03) {
					case 0: SetMirroringType(MirroringType::Vertical); break;
					case 1: SetMirroringType(MirroringType::Horizontal); break;
					case 2: SetMirroringType(MirroringType::ScreenAOnly); break;
					case 3: SetMirroringType(MirroringType::ScreenBOnly); break;
				}
				break;

			case 0x0A:
				//Wiki claims there is no reload value, however this seems to be the only way to make Famicom Jump II - Saikyou no 7 Nin work properly 
				_irqEnabled = (value & 0x01) == 0x01;
				_irqCounter = _irqReload;
				_console->GetCpu()->ClearIrqSource(IRQSource::External);
				break;

			case 0x0B:
				_irqReload = (_irqReload & 0xFF00) | value;
				break;

			case 0x0C:
				_irqReload = (_irqReload & 0xFF) | (value << 8);
				break;

			case 0x0D:
				//TODO: PRG RAM Enable / EEPROM Control
				break;
		}
	}
};