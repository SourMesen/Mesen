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
#include "CrossFeedFilter.h"
#include "WaveRecorder.h"

namespace orfanidis_eq {
	class freq_grid;
	class eq1;
}

class SoundMixer : public Snapshotable
{
public:
	static const uint32_t CycleLength = 1000;
	static const uint32_t BitsPerSample = 16;

private:
	static unique_ptr<WaveRecorder> _waveRecorder;
	static SimpleLock _waveRecorderLock;
	static double _fadeRatio;
	static uint32_t _muteFrameCount;

	static IAudioDevice* AudioDevice;
	static const uint32_t MaxSampleRate = 48000;
	static const uint32_t MaxSamplesPerFrame = MaxSampleRate / 60 * 4 * 2; //x4 to allow CPU overclocking up to 10x, x2 for panning stereo
	static const uint32_t MaxChannelCount = 11;
	
	unique_ptr<orfanidis_eq::freq_grid> _eqFrequencyGrid;
	unique_ptr<orfanidis_eq::eq1> _equalizerLeft;
	unique_ptr<orfanidis_eq::eq1> _equalizerRight;

	CrossFeedFilter _crossFeedFilter;
	LowPassFilter _lowPassFilter;
	StereoPanningFilter _stereoPanning;
	StereoDelayFilter _stereoDelay;
	ReverbFilter _reverbFilter;

	int16_t _previousOutputLeft = 0;
	int16_t _previousOutputRight = 0;

	vector<uint32_t> _timestamps;
	int16_t _channelOutput[MaxChannelCount][CycleLength];
	int16_t _currentOutput[MaxChannelCount];

	blip_t* _blipBufLeft;
	blip_t* _blipBufRight;
	int16_t *_outputBuffer;
	double _volumes[MaxChannelCount];
	double _panning[MaxChannelCount];

	NesModel _model;
	uint32_t _sampleRate;
	uint32_t _clockRate;

	bool _hasPanning;

	double GetChannelOutput(AudioChannel channel, bool forRightChannel);
	int16_t GetOutputVolume(bool forRightChannel);
	void EndFrame(uint32_t time);

	void UpdateRates(bool forceUpdate);
	
	void UpdateEqualizers(bool forceUpdate);
	void ApplyEqualizer(orfanidis_eq::eq1* equalizer, size_t sampleCount);

protected:
	virtual void StreamState(bool saving) override;

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
