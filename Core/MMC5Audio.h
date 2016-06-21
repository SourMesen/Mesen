#pragma once
#include "stdafx.h"
#include "SquareChannel.h"
#include "BaseExpansionAudio.h"

class MMC5Square : public SquareChannel
{
	int8_t _currentOutput;

private:
	virtual void InitializeSweep()
	{
		//"$5001 has no effect. The MMC5 pulse channels will not sweep, as they have no sweep unit."
	}

	bool IsMuted()
	{
		//"Frequency values less than 8 do not silence the MMC5 pulse channels; they can output ultrasonic frequencies."
		return false;
	}
public:
	MMC5Square() : SquareChannel(AudioChannel::MMC5, nullptr, false)
	{
		_currentOutput = 0;
	}
	
	virtual void AddOutput(int8_t output)
	{
		_currentOutput = output;
	}

	int8_t GetOutput()
	{
		return _currentOutput;
	}

	void Run()
	{
		SquareChannel::Run(1);
		EndFrame();
	}
};

class MMC5Audio : public BaseExpansionAudio
{
private:
	MMC5Square _square1;
	MMC5Square _square2;
	int16_t _audioCounter;
	int16_t _lastOutput;

	bool _pcmReadMode;
	bool _pcmIrqEnabled;
	uint8_t _pcmOutput;

protected:
	void StreamState(bool saving)
	{
		BaseExpansionAudio::StreamState(saving);

		SnapshotInfo square1{ &_square1 };
		SnapshotInfo square2{ &_square2 };
		Stream(square1, square2, _audioCounter, _lastOutput, _pcmReadMode, _pcmIrqEnabled, _pcmOutput);
	}

	void ClockAudio()
	{
		_audioCounter--;
		_square1.Run();
		_square2.Run();
		if(_audioCounter <= 0) {
			//~240hz envelope/length counter
			_audioCounter = CPU::GetClockRate(EmulationSettings::GetNesModel()) / 240;
			_square1.TickLengthCounter();
			_square1.TickEnvelope();
			_square2.TickLengthCounter();
			_square2.TickEnvelope();
		}

		int16_t summedOutput = (_square1.GetOutput() + _square2.GetOutput()) * 4 + _pcmOutput;
		APU::AddExpansionAudioDelta(AudioChannel::MMC5, summedOutput - _lastOutput);
		_lastOutput = summedOutput;

		_square1.ReloadCounter();
		_square2.ReloadCounter();
	}

public:
	MMC5Audio()
	{
		_audioCounter = 0;
		_lastOutput = 0;
		_pcmReadMode = false;
		_pcmIrqEnabled = false;
		_pcmOutput = 0;
	}

	uint8_t ReadRegister(uint16_t addr)
	{
		switch(addr) {
			case 0x5010:
				//TODO: PCM IRQ
				return 0;

			case 0x5015:
				uint8_t status = 0;
				status |= _square1.GetStatus() ? 0x01 : 0x00;
				status |= _square2.GetStatus() ? 0x02 : 0x00;
				return status;
		}

		//Open bus
		return (addr >> 8);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr) {
			case 0x5000: case 0x5001: case 0x5002: case 0x5003:
				_square1.WriteRAM(addr, value);
				break;

			case 0x5004: case 0x5005: case 0x5006: case 0x5007:
				_square2.WriteRAM(addr, value);
				break;

			case 0x5010:
				//TODO: Read mode & PCM IRQs are not implemented
				_pcmReadMode = (value & 0x01) == 0x01;
				_pcmIrqEnabled = (value & 0x80) == 0x80;
				break;

			case 0x5011:
				if(!_pcmReadMode) {
					if(value != 0) {
						_pcmOutput = value;
					}
				}
				break;

			case 0x5015:
				_square1.SetEnabled((value & 0x01) == 0x01);
				_square2.SetEnabled((value & 0x02) == 0x02);
				break;
		}
	}
};