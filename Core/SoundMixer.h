#pragma once
#include "stdafx.h"
#include "EmulationSettings.h"
#include "../Utilities/LowPassFilter.h"
#include "../Utilities/blip_buf.h"
#include "../Utilities/SimpleLock.h"
#include "IAudioDevice.h"
#include "Snapshotable.h"
#include "StereoPanningFilter.h"
#include "StereoDelayFilter.h"
#include "ReverbFilter.h"
#include "WaveRecorder.h"

class SoundMixer : public Snapshotable
{
public:
	static const uint32_t CycleLength = 10000;
	static const uint32_t BitsPerSample = 16;

private:
	static unique_ptr<WaveRecorder> _waveRecorder;
	static SimpleLock _waveRecorderLock;
	static double _fadeRatio;
	static uint32_t _muteFrameCount;

	static IAudioDevice* AudioDevice;
	static const uint32_t MaxSampleRate = 48000;
	static const uint32_t MaxSamplesPerFrame = MaxSampleRate / 60 * 4; //x4 to allow CPU overclocking up to 10x
	static const uint32_t MaxChannelCount = 11;
	
	LowPassFilter _lowPassFilter;
	StereoPanningFilter _stereoPanning;
	StereoDelayFilter _stereoDelay;
	ReverbFilter _reverbFilter;

	int16_t _previousOutput = 0;

	vector<uint32_t> _timestamps;
	int16_t _channelOutput[MaxChannelCount][CycleLength];
	int16_t _currentOutput[MaxChannelCount];

	blip_t* _blipBuf;
	int16_t *_outputBuffer;
	double _volumes[MaxChannelCount];

	NesModel _model;
	uint32_t _sampleRate;
	uint32_t _clockRate;

	double GetChannelOutput(AudioChannel channel);
	int16_t GetOutputVolume();
	void EndFrame(uint32_t time);

	void UpdateRates(bool forceUpdate);

protected:
	virtual void StreamState(bool saving);

public:
	SoundMixer();
	~SoundMixer();

	void SetNesModel(NesModel model);
	void Reset();
	
	void PlayAudioBuffer(uint32_t cycle);
	void AddDelta(AudioChannel channel, uint32_t time, int16_t delta);

	static void StartRecording(string filepath);
	static void StopRecording();
	static bool IsRecording();

	//For NSF/NSFe
	static uint32_t GetMuteFrameCount();
	static void ResetMuteFrameCount();
	static void SetFadeRatio(double fadeRatio);

	static void StopAudio(bool clearBuffer = false);
	static void RegisterAudioDevice(IAudioDevice *audioDevice);
};
