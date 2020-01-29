#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "MemoryManager.h"
#include "Console.h"
#include "CPU.h"
#include "../Utilities/WavReader.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/StringUtilities.h"
#include "../Utilities/HexUtilities.h"

class StudyBox : public BaseMapper
{
private:
	shared_ptr<WavReader> _wavReader;
	uint32_t _audioSampleRate;

	bool _readyForBit = false;
	uint16_t _processBitDelay = 0;
	uint8_t _reg4202 = 0;

	uint8_t _commandCounter = 0;
	uint8_t _command = 0;

	uint8_t _currentPage = 0;
	int16_t _seekPage = 0;
	uint32_t _seekPageDelay = 0;
	
	bool _enableDecoder = false;

	bool _audioEnabled = false;
	bool _motorDisabled = false;
	uint16_t _byteReadDelay = 0;
	bool _irqEnabled = false;

	bool _pageFound = false;
	StudyBoxData _tapeData;
	int32_t _pageIndex = 0;
	int32_t _pagePosition = -1;

	uint32_t _inDataDelay = 0;
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

		//First bank (on the 2nd RAM chip, so bank #8 in the code) is mapped to 4000-4FFF, but the first 1kb is not accessible
		SetCpuMemoryMapping(0x4000, 0x4FFF, 8, PrgMemoryType::WorkRam);
		RemoveCpuMemoryMapping(0x4000, 0x43FF);

		SetMirroringType(MirroringType::FourScreens);
	}

	void InitMapper(RomData& romData) override
	{
		_tapeData = romData.StudyBox;
		_wavReader = WavReader::Create(_tapeData.AudioFile.data(), (uint32_t)_tapeData.AudioFile.size());
		if(!_wavReader) {
			_audioSampleRate = 44100;
			MessageManager::Log("[Study Box] Unsupported audio file format.");
		} else {
			_audioSampleRate = _wavReader->GetSampleRate();
		}
	}

	void StreamState(bool saving) override
	{
		BaseMapper::StreamState(saving);
		int32_t audioPosition = _wavReader ? _wavReader->GetPosition() : -1;
		Stream(
			_readyForBit, _processBitDelay, _reg4202, _commandCounter, _command, _currentPage, _seekPage, _seekPageDelay, _enableDecoder,
			_audioEnabled, _motorDisabled, _byteReadDelay, _irqEnabled, _pageFound, _pageIndex, _pagePosition, _inDataDelay, _inDataRegion, audioPosition
		);

		if(!saving && audioPosition >= 0 && _wavReader) {
			_wavReader->SetSampleRate(_console->GetSettings()->GetSampleRate());
			_wavReader->Play(audioPosition);
		}
	}

	void ReadLeadInTrack()
	{
		//Wait for the tape to read through the lead-in before the actual data
		_inDataDelay = (uint64_t)(_tapeData.Pages[_pageIndex].AudioOffset - _tapeData.Pages[_pageIndex].LeadInOffset) * _console->GetCpu()->GetClockRate(_console->GetModel()) / _audioSampleRate;
		_pagePosition = -1;
		_byteReadDelay = 0;
		_motorDisabled = false;
		_pageFound = true;
	}

	void ProcessCpuClock() override
	{
		if(_processBitDelay > 0) {
			_processBitDelay--;
			if(_processBitDelay == 0) {
				_readyForBit = true;
			}
		}

		if(_motorDisabled) {
			return;
		}

		if(_seekPage != _currentPage) {
			_seekPageDelay--;
			if(_seekPageDelay == 0) {
				_seekPageDelay = 3000000;
				if(_seekPage > _currentPage) {
					_currentPage++;
				} else {
					_currentPage--;
				}

				_pageIndex = 0;
				for(size_t i = 0; i < _tapeData.Pages.size(); i++) {
					if(_tapeData.Pages[i].Data[5] == _currentPage - 1) {
						//Find the first page that matches the requested page number
						_pageIndex = (int32_t)i;
						break;
					}
				}
				
				ReadLeadInTrack();
			}
		} else if(_inDataDelay > 0) {
			_inDataRegion = true;
			_inDataDelay--;
			if(_inDataDelay == 0) {
				_byteReadDelay = 7820;
				if(_wavReader) {
					_wavReader->SetSampleRate(_console->GetSettings()->GetSampleRate());
					_wavReader->Play(_tapeData.Pages[_pageIndex].AudioOffset);
				}
				_console->GetCpu()->SetIrqSource(IRQSource::External);
			}
		} else if(_byteReadDelay > 0) {
			_byteReadDelay--;
			if(_byteReadDelay == 0) {
				_byteReadDelay = 3355;
				_pagePosition++;

				if(_pagePosition >= (int32_t)_tapeData.Pages[_pageIndex].Data.size()) {
					_pageFound = false;
					_inDataRegion = false;
					_motorDisabled = true;
				}

				if(_irqEnabled) {
					_console->GetCpu()->SetIrqSource(IRQSource::External);
				}
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
				if(_pagePosition >= 0 && _pagePosition < (int32_t)_tapeData.Pages[_pageIndex].Data.size()) {
					//MessageManager::Log("Read: " + HexUtilities::ToHex(_tapeData.Pages[_pageIndex].Data[_pagePosition]));
					return _tapeData.Pages[_pageIndex].Data[_pagePosition];
				}

				//After command $86, games expect to read 1 $AA byte before the $C5 header
				return 0xAA;
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

				_pageFound = false;
				
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
				SetCpuMemoryMapping(0x6000, 0x6FFF, (value & 0xC0) >> 5, PrgMemoryType::WorkRam);
				SetCpuMemoryMapping(0x7000, 0x7FFF, ((value & 0xC0) >> 5) + 1, PrgMemoryType::WorkRam);
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
						//MessageManager::Log("Command sent: " + std::to_string(_command));

						if(_command >= 1 && _command < 0x40) {
							//Move forward X pages
							_seekPage = _command + _currentPage;
							_seekPageDelay = 3000000;
							_motorDisabled = false;
						} else if(_command > 0x40 && _command < 0x80) {
							//Move back X pages
							_seekPage = -(_command - 0x40) + _currentPage;
							_seekPageDelay = 3000000;
							_motorDisabled = false;
						} else if(_command == 0) {
							//Move back to the start of this page (based on the internal page ID, not the page "index" in the array? - to validate)
							_seekPage = _currentPage;
							_currentPage = _currentPage - 1;
							_seekPageDelay = 3000000;
							_motorDisabled = false;
						} else if(_command == 0x86) {
							//Reenable motor (and continue to the next page)
							if(_pageIndex < (int32_t)_tapeData.Pages.size() - 1) {
								_pageIndex++;
							} else {
								//Pretend we go back to the start of the tape (inaccurate)
								_pageIndex = 0;
							}
							ReadLeadInTrack();
						} else {
							MessageManager::Log("Unknown command sent: " + std::to_string(_command));
						}
					}
				}

				if(value & 0x10) {
					_readyForBit = false;
					_processBitDelay = 100;
				}

				/*if((_reg4202 & 0x6E) != (value & 0x6E)) {
					MessageManager::Log("Reg 4202 value changed: " + HexUtilities::ToHex(_reg4202) + " -> " + HexUtilities::ToHex(value));
				}*/

				if((_reg4202 & 0x20) && !(value & 0x20)) {
					//Reset drive
					_command = 0;
					_commandCounter = 0;
					_readyForBit = true;
				}

				if((value & 0x04) != (_reg4202 & 0x04)) {
					_audioEnabled = (value & 0x04) == 0;
					//MessageManager::Log(_audioEnabled ? "Audio enabled" : "Audio disabled");
				}

				if((value & 0x02) != (_reg4202 & 0x02)) {
					//MessageManager::Log((value & 0x02) ? "IRQ enabled" : "IRQ disabled");
				}

				/*if((value & 0x01) != (_reg4202 & 0x01)) {
					MessageManager::Log((value & 0x01) ? "Decoder enabled" : "Decoder disabled");
				}*/

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

public:
	void ApplySamples(int16_t* buffer, size_t sampleCount, double volume) override
	{
		if(!_motorDisabled && _wavReader) {
			_wavReader->ApplySamples(buffer, sampleCount, _audioEnabled ? volume : 0);
			_wavReader->SetSampleRate(_console->GetSettings()->GetSampleRate());
		}
	}
};