#include "stdafx.h"
#include "SoundMixer.h"
#include "APU.h"
#include "CPU.h"

IAudioDevice* SoundMixer::AudioDevice = nullptr;
unique_ptr<WaveRecorder> SoundMixer::_waveRecorder;
SimpleLock SoundMixer::_waveRecorderLock;
double SoundMixer::_fadeRatio;
uint32_t SoundMixer::_muteFrameCount;

SoundMixer::SoundMixer()
{
	_outputBuffer = new int16_t[SoundMixer::MaxSamplesPerFrame];
	_blipBuf = blip_new(SoundMixer::MaxSamplesPerFrame);
	_sampleRate = EmulationSettings::GetSampleRate();
	_model = NesModel::NTSC;

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
	Stream(_clockRate, _sampleRate);
	
	if(!saving) {
		Reset();
		UpdateRates(true);
	}

	ArrayInfo<int16_t> currentOutput = { _currentOutput, MaxChannelCount };
	Stream(_previousOutput, currentOutput);
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
	_fadeRatio = 1.0;
	_muteFrameCount = 0;

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
		if(!_waveRecorder && EmulationSettings::CheckFlag(EmulationFlags::InBackground)) {
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
		if(_waveRecorder) {
			auto lock = _waveRecorderLock.AcquireSafe();
			if(_waveRecorder) {
				if(!_waveRecorder->WriteSamples(soundBuffer, (uint32_t)sampleCount, _sampleRate, isStereo)) {
					_waveRecorder.reset();
				}
			}
		}
	}

	if(EmulationSettings::GetSampleRate() != _sampleRate) {
		//Update sample rate for next frame if setting changed
		_sampleRate = EmulationSettings::GetSampleRate();
		UpdateRates(true);
	}
	UpdateRates(false);
}

void SoundMixer::SetNesModel(NesModel model)
{
	_model = model;
	UpdateRates(true);
}

void SoundMixer::UpdateRates(bool forceUpdate)
{
	uint32_t newRate = CPU::GetClockRate(_model);
	if(!EmulationSettings::GetOverclockAdjustApu()) {
		newRate = (uint32_t)(newRate * (double)EmulationSettings::GetOverclockRate(false, true) / 100);
	}

	if(_clockRate != newRate || forceUpdate) {
		_clockRate = newRate;
		blip_set_rates(_blipBuf, _clockRate, _sampleRate);
	}
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
	
	return (int16_t)(squareVolume + tndVolume +
		GetChannelOutput(AudioChannel::FDS) * 20 +
		GetChannelOutput(AudioChannel::MMC5) * 40 +
		GetChannelOutput(AudioChannel::Namco163) * 20 +
		GetChannelOutput(AudioChannel::Sunsoft5B) * 15 +
		GetChannelOutput(AudioChannel::VRC6) * 75 + 
		GetChannelOutput(AudioChannel::VRC7));
}

void SoundMixer::AddDelta(AudioChannel channel, uint32_t time, int16_t delta)
{
	if(delta != 0) {
		_timestamps.push_back(time);
		_channelOutput[(int)channel][time] += delta;
	}
}

void SoundMixer::EndFrame(uint32_t time)
{
	double masterVolume = EmulationSettings::GetMasterVolume();
	sort(_timestamps.begin(), _timestamps.end());
	_timestamps.erase(std::unique(_timestamps.begin(), _timestamps.end()), _timestamps.end());

	bool muteFrame = true;
	int16_t originalOutput = _previousOutput;
	for(size_t i = 0, len = _timestamps.size(); i < len; i++) {
		uint32_t stamp = _timestamps[i];
		for(int j = 0; j < MaxChannelCount; j++) {
			_currentOutput[j] += _channelOutput[j][stamp];
		}

		int16_t currentOutput = GetOutputVolume();
		blip_add_delta(_blipBuf, stamp, (int)((currentOutput - _previousOutput) * masterVolume * _fadeRatio));

		if(currentOutput != _previousOutput) {
			if(std::abs(currentOutput - _previousOutput) > 100) {
				muteFrame = false;
			}
			_previousOutput = currentOutput;
		}
	}

	if(std::abs(originalOutput - _previousOutput) > 1500) {
		//Count mute frames (10000 cycles each) - used by NSF player
		muteFrame = false;
	}

	blip_end_frame(_blipBuf, time);

	if(muteFrame) {
		_muteFrameCount++;
	} else {
		_muteFrameCount = 0;
	}

	//Reset everything
	for(int i = 0; i < MaxChannelCount; i++) {
		_volumes[i] = EmulationSettings::GetChannelVolume((AudioChannel)i);
	}

	_timestamps.clear();
	memset(_channelOutput, 0, sizeof(_channelOutput));
}

void SoundMixer::StartRecording(string filepath)
{
	auto lock = _waveRecorderLock.AcquireSafe();
	_waveRecorder.reset(new WaveRecorder(filepath, EmulationSettings::GetSampleRate(), EmulationSettings::GetStereoFilter() != StereoFilter::None));
}

void SoundMixer::StopRecording()
{
	auto lock = _waveRecorderLock.AcquireSafe();
	_waveRecorder.reset();
}

bool SoundMixer::IsRecording()
{
	return _waveRecorder.get() != nullptr;
}

void SoundMixer::SetFadeRatio(double fadeRatio)
{
	_fadeRatio = fadeRatio;
}

uint32_t SoundMixer::GetMuteFrameCount()
{
	return _muteFrameCount;
}

void SoundMixer::ResetMuteFrameCount()
{
	_muteFrameCount = 0;
}