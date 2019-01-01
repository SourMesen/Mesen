#pragma once
#include "BaseExpansionAudio.h"
#include "Vrc6Pulse.h"
#include "Vrc6Saw.h"

class Vrc6Audio : public BaseExpansionAudio
{
private:
	Vrc6Pulse _pulse1;
	Vrc6Pulse _pulse2;
	Vrc6Saw _saw;
	bool _haltAudio = false;
	int32_t _lastOutput = 0;

protected:
	void StreamState(bool saving) override
	{
		SnapshotInfo pulse1{ &_pulse1 };
		SnapshotInfo pulse2{ &_pulse2 };
		SnapshotInfo saw{ &_saw };

		Stream(_lastOutput, _haltAudio, pulse1, pulse2, saw);
	}
	
	void ClockAudio() override
	{
		if(!_haltAudio) {
			_pulse1.Clock();
			_pulse2.Clock();
			_saw.Clock();
		}

		int32_t outputLevel = _pulse1.GetVolume() + _pulse2.GetVolume() + _saw.GetVolume();
		_console->GetApu()->AddExpansionAudioDelta(AudioChannel::VRC6, outputLevel - _lastOutput);
		_lastOutput = outputLevel;
	}

public:
	Vrc6Audio(shared_ptr<Console> console) : BaseExpansionAudio(console)
	{
		Reset();
	}

	void Reset()
	{
		_lastOutput = 0;
		_haltAudio = false;
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		switch(addr) {
			case 0x9000: case 0x9001: case 0x9002:
				_pulse1.WriteReg(addr, value);
				break;

			case 0x9003: {
				_haltAudio = (value & 0x01) == 0x01;
				uint8_t frequencyShift = (value & 0x04) == 0x04 ? 8 : ((value & 0x02) == 0x02 ? 4 : 0);
				_pulse1.SetFrequencyShift(frequencyShift);
				_pulse2.SetFrequencyShift(frequencyShift);
				_saw.SetFrequencyShift(frequencyShift);
				break;
			}

			case 0xA000: case 0xA001: case 0xA002:
				_pulse2.WriteReg(addr, value);
				break;

			case 0xB000: case 0xB001: case 0xB002:
				_saw.WriteReg(addr, value);
				break;
		}
	}
};