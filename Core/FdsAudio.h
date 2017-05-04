#pragma once
#include "stdafx.h"
#include <algorithm>
#include "Snapshotable.h"
#include "EmulationSettings.h"
#include "APU.h"
#include "BaseFdsChannel.h"
#include "ModChannel.h"
#include "BaseExpansionAudio.h"
#include "MemoryManager.h"

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
	void StreamState(bool saving) override
	{
		BaseExpansionAudio::StreamState(saving);

		ArrayInfo<uint8_t> waveTable = { _waveTable, 64 };
		SnapshotInfo volume{ &_volume };
		SnapshotInfo mod{ &_mod };

		Stream(volume, mod, _waveWriteEnabled, _disableEnvelopes, _haltWaveform, _masterVolume, _waveOverflowCounter, _wavePitch, _wavePosition, _lastOutput, waveTable);
	}

	void ClockAudio() override
	{
		int frequency = _volume.GetFrequency();
		if(!_haltWaveform && !_disableEnvelopes) {
			_volume.TickEnvelope();
			if(_mod.TickEnvelope()) {
				_mod.UpdateOutput(frequency);
			}
		}

		if(_mod.TickModulator()) {
			//Modulator was ticked, update wave pitch
			_mod.UpdateOutput(frequency);
		}
	
		if(_haltWaveform) {
			_wavePosition = 0;
			UpdateOutput();
		} else {
			UpdateOutput();

			if(frequency + _mod.GetOutput() > 0 && !_waveWriteEnabled) {
				_waveOverflowCounter += frequency + _mod.GetOutput();
				if(_waveOverflowCounter < frequency + _mod.GetOutput()) {
					_wavePosition = (_wavePosition + 1) & 0x3F;
				}
			}
		}
	}

	void UpdateOutput()
	{
		uint32_t level = std::min((int)_volume.GetGain(), 32) * WaveVolumeTable[_masterVolume];
		uint8_t outputLevel = (_waveTable[_wavePosition] * level) / 1152;


		if(_lastOutput != outputLevel) {
			APU::AddExpansionAudioDelta(AudioChannel::FDS, outputLevel - _lastOutput);
			_lastOutput = outputLevel;
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

		return MemoryManager::GetOpenBus();
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
					if(_disableEnvelopes) {
						_volume.ResetTimer();
						_mod.ResetTimer();
					}
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