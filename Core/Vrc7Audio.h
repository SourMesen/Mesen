#pragma once 
#include "stdafx.h"
#include "BaseExpansionAudio.h"
#include "Console.h"
#include "OpllEmulator.h"

class Vrc7Audio : public BaseExpansionAudio
{
private:
	unique_ptr<Vrc7Opll::OpllEmulator> _opllEmulator;
	uint8_t _currentReg;
	int16_t _previousOutput;
	double _clockTimer;
	bool _muted;

protected:
	void ClockAudio() override
	{
		_clockTimer--;
		if(_clockTimer <= 0) {
			int16_t output = _opllEmulator->GetOutput();
			APU::AddExpansionAudioDelta(AudioChannel::VRC7, _muted ? 0 : (output - _previousOutput));
			_previousOutput = output;
			_clockTimer = ((double)CPU::GetClockRate(Console::GetModel())) / 49716;
		}
	}

	void StreamState(bool saving) override
	{
		BaseExpansionAudio::StreamState(saving);

		SnapshotInfo opllEmulator{ _opllEmulator.get() };
		Stream(opllEmulator, _currentReg, _previousOutput, _clockTimer, _muted);
	}

public:
	Vrc7Audio()
	{
		_previousOutput = 0;
		_currentReg = 0;
		_muted = false;
		_clockTimer = ((double)CPU::GetClockRate(Console::GetModel())) / 49716;

		_opllEmulator.reset(new Vrc7Opll::OpllEmulator());
	}

	void SetMuteAudio(bool muted)
	{
		_muted = muted;
	}

	void WriteReg(uint16_t addr, uint8_t value)
	{
		switch(addr & 0xF030) {
			case 0x9010:
				_currentReg = value;
				break;
			case 0x9030:
				_opllEmulator->WriteReg(_currentReg, value);
				break;
		}
	}
};