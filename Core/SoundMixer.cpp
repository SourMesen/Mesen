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
	InitializeLookupTables();

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
	Stream<uint32_t>(_clockRate);
	Stream<uint32_t>(_sampleRate);
	Stream<AudioChannel>(_expansionAudioType);
	
	if(!saving) {
		Reset();
		UpdateRates();
	}

	Stream<int16_t>(_previousOutput);
	StreamArray<int8_t>(_currentOutput, MaxChannelCount);
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
		SoundMixer::AudioDevice->PlayBuffer(_outputBuffer, (uint32_t)(sampleCount * SoundMixer::BitsPerSample / 8), _sampleRate);
	}
	
	if(EmulationSettings::GetSampleRate() != _sampleRate) {
		//Update sample rate for next frame if setting changed
		_sampleRate = EmulationSettings::GetSampleRate();
		UpdateRates();
	}
}

void SoundMixer::SetNesModel(NesModel model)
{
	_clockRate = model == NesModel::NTSC ? CPU::ClockRateNtsc : CPU::ClockRatePal;
	UpdateRates();
}

void SoundMixer::UpdateRates()
{
	blip_set_rates(_blipBuf, _clockRate, _sampleRate);
}

void SoundMixer::InitializeLookupTables()
{
	for(int i = 0; i < 31; i++) {
		double volume = 95.52 / (8128.0 / (double)i + 100.0);
		_lupSquare[i] = (int16_t)(volume * 5000);
	}

	for(int i = 0; i < 203; i++) {
		double volume = 163.67 / (24329.0 / (double)i + 100.0);
		_lupTnd[i] = (int16_t)(volume * 5000);
	}
}

int16_t SoundMixer::GetOutputVolume()
{
	int16_t squareOutput = _lupSquare[(int)(_currentOutput[(int)AudioChannel::Square1] * _volumes[(int)AudioChannel::Square1] + _currentOutput[(int)AudioChannel::Square2] * _volumes[(int)AudioChannel::Square2])];
	int16_t tndOutput = _lupTnd[(int)(3 * _currentOutput[(int)AudioChannel::Triangle] * _volumes[(int)AudioChannel::Triangle] + 2 * _currentOutput[(int)AudioChannel::Noise] * _volumes[(int)AudioChannel::Noise] + _currentOutput[(int)AudioChannel::DMC] * _volumes[(int)AudioChannel::DMC])];
	int16_t expansionOutput = 0;
	switch(_expansionAudioType) {
		case AudioChannel::FDS: expansionOutput = (int16_t)(_currentOutput[ExpansionAudioIndex] * _volumes[ExpansionAudioIndex] * 20); break;
	}
	return squareOutput + tndOutput + expansionOutput;
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