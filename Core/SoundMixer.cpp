#include "stdafx.h"
#include "SoundMixer.h"
#include "APU.h"
#include "CPU.h"

IAudioDevice* SoundMixer::AudioDevice = nullptr;

SoundMixer::SoundMixer()
{
	_outputBuffer = new int16_t[SoundMixer::MaxSamplesPerFrame];
	_blipBuf = blip_new(SoundMixer::MaxSamplesPerFrame);
	_sampleRate = EmulationSettings::GetSampleRate();

	Reset();
}

SoundMixer::~SoundMixer()
{
	delete[] _outputBuffer;
	_outputBuffer = nullptr;

	blip_delete(_blipBuf);
}

void SoundMixer::StreamState(bool saving)
{
	Stream(_clockRate, _sampleRate, _expansionAudioType);
	
	if(!saving) {
		Reset();
		UpdateRates();
	}

	Stream(_previousOutput, ArrayInfo<int8_t>{_currentOutput, MaxChannelCount});
}

void SoundMixer::RegisterAudioDevice(IAudioDevice *audioDevice)
{
	SoundMixer::AudioDevice = audioDevice;
}

void SoundMixer::StopAudio(bool clearBuffer)
{
	if(SoundMixer::AudioDevice) {
		if(clearBuffer) {
			SoundMixer::AudioDevice->Stop();
		} else {
			SoundMixer::AudioDevice->Pause();
		}
	}
}

void SoundMixer::Reset()
{
	_previousOutput = 0;
	blip_clear(_blipBuf);

	_timestamps.clear();

	for(int i = 0; i < MaxChannelCount; i++) {
		_volumes[0] = 0;
	}
	memset(_channelOutput, 0, sizeof(_channelOutput));
	memset(_currentOutput, 0, sizeof(_currentOutput));
}

void SoundMixer::PlayAudioBuffer(uint32_t time)
{
	EndFrame(time);
	size_t sampleCount = blip_read_samples(_blipBuf, _outputBuffer, SoundMixer::MaxSamplesPerFrame, 0);
	if(SoundMixer::AudioDevice) {
		//Apply low pass filter/volume reduction when in background (based on options)
		if(EmulationSettings::CheckFlag(EmulationFlags::InBackground)) {
			if(EmulationSettings::CheckFlag(EmulationFlags::MuteSoundInBackground)) {
				_lowPassFilter.ApplyFilter(_outputBuffer, sampleCount, 0, 0);
			} else if(EmulationSettings::CheckFlag(EmulationFlags::ReduceSoundInBackground)) {
				_lowPassFilter.ApplyFilter(_outputBuffer, sampleCount, 6, 0.75);
			}
		}

		int16_t* soundBuffer = _outputBuffer;
		if(EmulationSettings::GetReverbStrength() > 0) {
			soundBuffer = _reverbFilter.ApplyFilter(soundBuffer, sampleCount, _sampleRate, EmulationSettings::GetReverbStrength(), EmulationSettings::GetReverbDelay());
		} else {
			_reverbFilter.ResetFilter();
		}

		bool isStereo = false;
		switch(EmulationSettings::GetStereoFilter()) {
			case StereoFilter::Delay:
				soundBuffer = _stereoDelay.ApplyFilter(soundBuffer, sampleCount, _sampleRate);
				isStereo = true;
				break;

			case StereoFilter::Panning:
				soundBuffer = _stereoPanning.ApplyFilter(soundBuffer, sampleCount);
				isStereo = true;
				break;
		}

		SoundMixer::AudioDevice->PlayBuffer(soundBuffer, (uint32_t)sampleCount, _sampleRate, isStereo);
	}

	if(EmulationSettings::GetSampleRate() != _sampleRate) {
		//Update sample rate for next frame if setting changed
		_sampleRate = EmulationSettings::GetSampleRate();
		UpdateRates();
	}
}

void SoundMixer::SetNesModel(NesModel model)
{
	switch(model) {
		case NesModel::NTSC: _clockRate = CPU::ClockRateNtsc; break;
		case NesModel::PAL: _clockRate = CPU::ClockRatePal; break;
		case NesModel::Dendy: _clockRate = CPU::ClockRateDendy; break;
	}

	UpdateRates();
}

void SoundMixer::UpdateRates()
{
	blip_set_rates(_blipBuf, _clockRate, _sampleRate);
}

double SoundMixer::GetChannelOutput(AudioChannel channel)
{
	return _currentOutput[(int)channel] * _volumes[(int)channel];
}

int16_t SoundMixer::GetOutputVolume()
{
	double squareOutput = GetChannelOutput(AudioChannel::Square1) + GetChannelOutput(AudioChannel::Square2);
	double tndOutput = 3 * GetChannelOutput(AudioChannel::Triangle) + 2 * GetChannelOutput(AudioChannel::Noise) + GetChannelOutput(AudioChannel::DMC);

	uint16_t squareVolume = (uint16_t)(95.52 / (8128.0 / squareOutput + 100.0) * 5000);
	uint16_t tndVolume = (uint16_t)(163.67 / (24329.0 / tndOutput + 100.0) * 5000);
	
	int16_t expansionOutput = 0;
	switch(_expansionAudioType) {
		case AudioChannel::FDS: expansionOutput = (int16_t)(_currentOutput[ExpansionAudioIndex] * _volumes[ExpansionAudioIndex] * 20); break;
	}
	return squareVolume + tndVolume + expansionOutput;
}

void SoundMixer::AddDelta(AudioChannel channel, uint32_t time, int8_t delta)
{
	if(delta != 0) {
		_timestamps.push_back(time);
		_channelOutput[(int)channel][time] += delta;
	}
}

void SoundMixer::AddExpansionAudioDelta(uint32_t time, int8_t delta)
{
	if(delta != 0) {
		_timestamps.push_back(time);
		_channelOutput[ExpansionAudioIndex][time] += delta;
	}
}

void SoundMixer::SetExpansionAudioType(AudioChannel channel)
{
	_expansionAudioType = channel;
}

void SoundMixer::EndFrame(uint32_t time)
{
	double masterVolume = EmulationSettings::GetMasterVolume();
	sort(_timestamps.begin(), _timestamps.end());
	_timestamps.erase(std::unique(_timestamps.begin(), _timestamps.end()), _timestamps.end());

	for(size_t i = 0, len = _timestamps.size(); i < len; i++) {
		uint32_t time = _timestamps[i];
		for(int j = 0; j < MaxChannelCount; j++) {
			_currentOutput[j] += _channelOutput[j][time];
		}

		int16_t currentOutput = GetOutputVolume();
		blip_add_delta(_blipBuf, time, (int)((currentOutput - _previousOutput) * masterVolume));
		_previousOutput = currentOutput;
	}
	blip_end_frame(_blipBuf, time);

	//Reset everything
	for(int i = 0; i < MaxChannelCount; i++) {
		_volumes[i] = EmulationSettings::GetChannelVolume(i < 5 ? (AudioChannel)i : _expansionAudioType);
	}

	_timestamps.clear();
	memset(_channelOutput, 0, sizeof(_channelOutput));
}