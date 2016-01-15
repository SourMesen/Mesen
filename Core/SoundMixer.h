#pragma once
#include "stdafx.h"
#include "EmulationSettings.h"
#include "../BlipBuffer/blip_buf.h"
#include "IAudioDevice.h"

class SoundMixer
{
private:
	static IAudioDevice* AudioDevice;
	static const uint32_t MaxSampleRate = 48000;
	static const uint32_t MaxSamplesPerFrame = MaxSampleRate / 60;

	int16_t _previousOutput = 0;

	vector<uint32_t> _timestamps;
	int8_t _channelOutput[5][15000];
	int8_t _currentOutput[5];

	int16_t _lupSquare[31];
	int16_t _lupTnd[203];

	blip_t* _blipBuf;
	int16_t *_outputBuffer;
	double _volumes[5];

	uint32_t _sampleRate;
	uint32_t _clockRate;

	void InitializeLookupTables();
	int16_t GetOutputVolume();
	void EndFrame(uint32_t time);

	void UpdateRates();

public:
	static const uint32_t BitsPerSample = 16;

	SoundMixer();
	~SoundMixer();

	void SetNesModel(NesModel model);
	void Reset();
	
	void PlayAudioBuffer(uint32_t cycle);
	void AddDelta(AudioChannel channel, uint32_t time, int8_t output);

	static void StopAudio(bool clearBuffer = false);
	static void RegisterAudioDevice(IAudioDevice *audioDevice);
};
