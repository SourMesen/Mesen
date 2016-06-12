#pragma once
#include "stdafx.h"
#include "Snapshotable.h"
#include "EmulationSettings.h"
#include "APU.h"
#include "BaseFdsChannel.h"
#include "ModChannel.h"
#include <algorithm>
#include "BaseExpansionAudio.h"

class FdsAudio : public BaseExpansionAudio
{
private:
	const uint32_t WaveVolumeTable[4] = { 36, 24, 17, 14 };

	//Register values
	uint8_t _waveTable[64];
	bool _waveWriteEnabled = false;

	BaseFdsChannel _volume;
	ModChannel _mod;

	bool _disableEnvelopes = false;
	bool _haltWaveform = false;

	uint8_t _masterVolume = 0;

	//Internal values
	uint16_t _waveOverflowCounter = 0;
	int32_t _wavePitch = 0;
	uint8_t _wavePosition = 0;
	
	uint8_t _lastOutput = 0;

protected:
	void StreamState(bool saving)
	{
		BaseExpansionAudio::StreamState(saving);

		ArrayInfo<uint8_t> waveTable = { _waveTable, 64 };
		SnapshotInfo volume{ &_volume };
		SnapshotInfo mod{ &_mod };

		Stream(volume, mod, _waveWriteEnabled, _disableEnvelopes, _haltWaveform, _masterVolume, _waveOverflowCounter, _wavePitch, _wavePosition, _lastOutput, waveTable);
	}

	void ClockAudio()
	{
		//"The envelopes are not ticked while the waveform is halted."
		_volume.TickEnvelope(_disableEnvelopes || _haltWaveform);
		_mod.TickEnvelope(_disableEnvelopes || _haltWaveform);
		
		if(_mod.IsEnabled()) {
			if(_mod.TickModulator()) {
				//Modulator was ticked, update wave pitch
				_wavePitch = _mod.GetWavePitch(_volume.GetFrequency());
			}
		} else {
			_wavePitch = 0;
		}

		if(_haltWaveform) {
			//"The high bit of this register halts the waveform and resets its phase to 0. Note that if halted it will output the constant value at $4040"
			//"writes to the volume register $4080 or master volume $4089 will affect the output."
			_wavePosition = 0;
		}
		
		int32_t freq = _volume.GetFrequency() + _wavePitch;
		if(freq > 0 && !_waveWriteEnabled) {
			_waveOverflowCounter += freq;
			if(_waveOverflowCounter < freq) {
				//Overflow, tick
				uint32_t level = std::min((int)_volume.GetGain(), 32) * WaveVolumeTable[_masterVolume];
				
				uint8_t outputLevel = (_waveTable[_wavePosition] * level) / 1152;
				APU::AddExpansionAudioDelta(AudioChannel::FDS, outputLevel - _lastOutput);
				_lastOutput = outputLevel;

				_wavePosition = (_wavePosition + 1) & 0x3F;
			}
		}
	}

public:
	uint8_t ReadRegister(uint16_t addr)
	{
		if(addr <= 0x407F) {
			return 0x40 | _waveTable[addr & 0x3F];
		} else {
			switch(addr) {
				case 0x4090: return 0x40 | _volume.GetGain();
				case 0x4092: return 0x40 | _mod.GetGain();
			}
		}

		//Open bus
		return (addr >> 8);
	}

	void WriteRegister(uint16_t addr, uint8_t value)
	{
		if(addr <= 0x407F) {
			if(_waveWriteEnabled) {
				_waveTable[addr & 0x3F] = value & 0x3F;
			}
		} else {
			switch(addr) {
				case 0x4080:
				case 0x4082:
					_volume.WriteReg(addr, value);
					break;

				case 0x4083:
					_disableEnvelopes = (value & 0x40) == 0x40;
					_haltWaveform = (value & 0x80) == 0x80;
					_volume.WriteReg(addr, value);
					break;

				case 0x4084:
				case 0x4085:
				case 0x4086:
				case 0x4087:
					_mod.WriteReg(addr, value);
					break;

				case 0x4088:
					_mod.WriteModTable(value);
					break;

				case 0x4089:
					_masterVolume = value & 0x03;
					_waveWriteEnabled = (value & 0x80) == 0x80;
					break;

				case 0x408A:
					_volume.SetMasterEnvelopeSpeed(value);
					_mod.SetMasterEnvelopeSpeed(value);
					break;
			}
		}
	}
};