#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "MemoryManager.h"
#include "VirtualFile.h"
#include "../Utilities/FolderUtilities.h"

class StudyBox : public BaseMapper
{
private:
	bool _readyForBit = false;
	uint16_t _processBitDelay = 0;
	uint8_t _reg4202 = 0;

	uint8_t _commandCounter = 0;
	uint8_t _command = 0;

	uint8_t _currentPage = 0;
	int16_t _seekPage = 0;
	uint32_t _seekPageDelay = 0;
	
	bool _enableDecoder = false;

	uint16_t _irqCounter = 3500;
	bool _irqEnabled = false;

	bool _pageFound = false;
	vector<uint8_t> _pageData;
	uint32_t _pagePosition = 0;
	bool _inDataRegion = false;

protected:
	uint16_t RegisterStartAddress() override { return 0x4200; }
	uint16_t RegisterEndAddress() override { return 0x4203; }
	bool AllowRegisterRead() override { return true; }

	uint16_t GetPRGPageSize() override { return 0x4000; }
	uint16_t GetCHRPageSize() override { return 0x2000; }

	uint32_t GetWorkRamSize() override { return 0x10000; }
	uint32_t GetWorkRamPageSize() override { return 0x1000; }

	void InitMapper() override
	{
		SelectPRGPage(1, 0);
		SelectCHRPage(0, 0);
		SetCpuMemoryMapping(0x4400, 0x4FFF, _workRam + 0x8400, MemoryAccessType::ReadWrite);
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
	}

	void ProcessCpuClock() override
	{
		if(_processBitDelay > 0) {
			_processBitDelay--;
			if(_processBitDelay == 0) {
				_readyForBit = true;
			}
		}

		if(_seekPage != 0) {
			_seekPageDelay--;
			if(_seekPageDelay == 0) {
				_seekPageDelay = 30000;
				_pageFound = true;
				if(_seekPage > 0) {
					_currentPage++;
					_seekPage--;
				} else {
					_currentPage--;
					_seekPage++;
				}
				VirtualFile file(FolderUtilities::CombinePath("StudyBox", "Page" + std::to_string(_currentPage) + ".bin"));
				file.ReadFile(_pageData);
				_pagePosition = 0;
			}
		}

		if(_irqEnabled) {
			_irqCounter--;
			if(_irqCounter == 0) {
				_irqCounter = 3500;
				_console->GetCpu()->SetIrqSource(IRQSource::External);
			}
		}
	}
	
	uint8_t ReadRegister(uint16_t addr) override
	{
		switch(addr) {
			case 0x4200: {
				if(!_enableDecoder) {
					MessageManager::Log("Error - read 4200 without decoder being enabled");
				}
				_console->GetCpu()->ClearIrqSource(IRQSource::External);
				if(_pagePosition < _pageData.size()) {
					uint8_t val = _pageData[_pagePosition++];
					if(_pagePosition >= _pageData.size()) {
						_inDataRegion = false;
						_pagePosition = 0;

						//Move to the next page
						_seekPage = 1;
						_seekPageDelay = 300000;
					}
					return val;
				}
				return 0;
			}
			
			case 0x4201: {
				/*	Tape read status ?
				$80 - something to do with $4202.0 ? decoder disabled ? | decoder data ready ?
				$40 - tape data clock synched ? current tape data bit ? | current tape flux polarity ? tape motor running ? | seek complete ?
				$20 - set when in data region during seek ? possibly set when in data region generally ? or set normally and cleared when in a data region ?
				$10 - ? ? ? */
				uint8_t value = (
					//0x10 |
					(_inDataRegion ? 0x20 : 0) |
					(_pageFound ? 0x40 : 0) |
					(_enableDecoder ? 0x80 : 0)
				);

				if(_pageFound) {
					_pageFound = false;
					_inDataRegion = true;
				}			

				return value;
			}

			case 0x4202:
				//Tape drive status?
				//$40 - shift register ready for next bit?
				//$08 - power supply not connected
				return (
					//(_powerDisconnected ? 0x08 : 0) |
					(_readyForBit ? 0x40 : 0)
				);

			case 0x4203: 
				//unused?
				return 0x00;
		}

		return 0;
	}

	void WriteRegister(uint16_t addr, uint8_t value) override
	{
		switch(addr) {
			case 0x4200:
				SetCpuMemoryMapping(0x6000, 0x7FFF, (value & 0xC0) >> 5, PrgMemoryType::WorkRam);
				SetCpuMemoryMapping(0x5000, 0x5FFF, (value & 0x07) + 8, PrgMemoryType::WorkRam);
				break;

			case 0x4201:
				//PRG Select
				SelectPRGPage(0, value); 
				break;

			case 0x4202:
				/*Tape drive control
					$80 - output data bit
					$40 - ???
					$20 - pulse low to reset drive controller?
					$10 - pulse low to clock data bit
					$08 - ???
					$04 - ??? maybe tape audio enable?
					$02 - irq enable?
					$01 - data decoding enable?
				*/
				if((_reg4202 & 0x10) && !(value & 0x10)) {
					if(!_readyForBit) {
						MessageManager::Log("Error - write without being ready");
					}
					//Clock command bit
					_command <<= 1;
					_command |= (value & 0x80) >> 7;
					_commandCounter++;

					if(_commandCounter == 8) {
						_commandCounter = 0;
						MessageManager::Log("Command sent: " + std::to_string(_command));

						if(_command >= 1 && _command < 0x40) {
							_seekPage = _command;
							_seekPageDelay = 30000;
						} else if(_command > 0x40 && _command < 0x80) {
							_seekPage = -(_command - 0x40);
							_seekPageDelay = 30000;
						} else if(_command == 0xFF) {
							//???
						}
					}
				}

				if(value & 0x10) {
					_readyForBit = false;
					_processBitDelay = 100;
				}

				if((_reg4202 & 0x20) && !(value & 0x20)) {
					//Reset drive
					_command = 0;
					_commandCounter = 0;
					_readyForBit = true;
				}

				_reg4202 = value;

				_enableDecoder = value & 0x01;
				_irqEnabled = value & 0x02;
				_console->GetCpu()->ClearIrqSource(IRQSource::External);
				break;

			case 0x4203:
				//Unused?
				break;
		}
	}
};