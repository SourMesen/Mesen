#include "stdafx.h"
#include "SoundMixer.h"
#include "APU.h"
#include "CPU.h"

SoundMixer::SoundMixer()
{
	_outputBuffer = new int16_t[APU::SamplesPerFrame];
	_blipBuf = blip_new(APU::SamplesPerFrame);
	
	InitializeLookupTables();

	Reset();
}

SoundMixer::~SoundMixer()
{
	delete[] _outputBuffer;
	_outputBuffer = nullptr;

	blip_delete(_blipBuf);
}

void SoundMixer::Reset()
{
	_previousOutput = 0;
	blip_clear(_blipBuf);

	for(int i = 0; i < 5; i++) {
		_volumes[0] = 0;
	}
	memset(_channelOutput, 0, sizeof(_channelOutput));
	memset(_currentOutput, 0, sizeof(_currentOutput));
}

void SoundMixer::PlayAudioBuffer(uint32_t time)
{
	EndFrame(time);
	size_t sampleCount = blip_read_samples(_blipBuf, _outputBuffer, APU::SamplesPerFrame, 0);
	IAudioDevice *audioDevice = APU::GetAudioDevice();	
	if(audioDevice) {
		audioDevice->PlayBuffer(_outputBuffer, (uint32_t)(sampleCount * APU::BitsPerSample / 8));
	}
}

void SoundMixer::SetNesModel(NesModel model)
{
	blip_set_rates(_blipBuf, model == NesModel::NTSC ? CPU::ClockRateNtsc : CPU::ClockRatePal, APU::SampleRate);
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
	return squareOutput + tndOutput;
}

void SoundMixer::AddDelta(AudioChannel channel, uint32_t time, int8_t delta)
{
	_timestamps.push_back(time);
	_channelOutput[(int)channel][time] += delta;
}

void SoundMixer::EndFrame(uint32_t time)
{
	double masterVolume = EmulationSettings::GetMasterVolume();
	sort(_timestamps.begin(), _timestamps.end());
	_timestamps.erase(std::unique(_timestamps.begin(), _timestamps.end()), _timestamps.end());

	for(size_t i = 0, len = _timestamps.size(); i < len; i++) {
		uint32_t time = _timestamps[i];
		_currentOutput[0] += _channelOutput[0][time];
		_currentOutput[1] += _channelOutput[1][time];
		_currentOutput[2] += _channelOutput[2][time];
		_currentOutput[3] += _channelOutput[3][time];
		_currentOutput[4] += _channelOutput[4][time];

		int16_t currentOutput = GetOutputVolume();
		blip_add_delta(_blipBuf, time, (currentOutput - _previousOutput) * masterVolume);
		_previousOutput = currentOutput;
	}
	blip_end_frame(_blipBuf, time);

	//Reset everything
	for(int i = 0; i < 5; i++) {
		_volumes[i] = EmulationSettings::GetChannelVolume((AudioChannel)i);
	}

	_timestamps.clear();
	memset(_channelOutput, 0, sizeof(_channelOutput));
}
