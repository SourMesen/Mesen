#pragma once
#include "stdafx.h"
#include "EmulationSettings.h"
#include "../Utilities/LowPassFilter.h"
#include "../BlipBuffer/blip_buf.h"
#include "IAudioDevice.h"
#include "Snapshotable.h"

class SoundMixer : public Snapshotable
{
public:
	static const uint32_t CycleLength = 10000;
	static const uint32_t BitsPerSample = 16;

private:
	static IAudioDevice* AudioDevice;
	static const uint32_t MaxSampleRate = 48000;
	static const uint32_t MaxSamplesPerFrame = MaxSampleRate / 60;
	static const uint32_t MaxChannelCount = 6;
	static const uint32_t ExpansionAudioIndex = MaxChannelCount - 1;

	AudioChannel _expansionAudioType;
	LowPassFilter _lowPassFilter;

	int16_t _previousOutput = 0;

	vector<uint32_t> _timestamps;
	int8_t _channelOutput[MaxChannelCount][CycleLength];
	int8_t _currentOutput[MaxChannelCount];

	int16_t _lupSquare[31];
	int16_t _lupTnd[203];

	blip_t* _blipBuf;
	int16_t *_outputBuffer;
	double _volumes[MaxChannelCount];

	uint32_t _sampleRate;
	uint32_t _clockRate;

	void InitializeLookupTables();
	int16_t GetOutputVolume();
	void EndFrame(uint32_t time);

	void UpdateRates();

protected:
	virtual void StreamState(bool saving);

public:
	SoundMixer();
	~SoundMixer();

	void SetNesModel(NesModel model);
	void Reset();
	
	void PlayAudioBuffer(uint32_t cycle);
	void AddDelta(AudioChannel channel, uint32_t time, int8_t delta);

	void SetExpansionAudioType(AudioChannel channel);
	void AddExpansionAudioDelta(uint32_t time, int8_t delta);
	
	static void StopAudio(bool clearBuffer = false);
	static void RegisterAudioDevice(IAudioDevice *audioDevice);
};
