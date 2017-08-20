#pragma once
#include "stdafx.h"

class OggReader;

class OggMixer
{
private:
	shared_ptr<OggReader> _bgm;
	vector<shared_ptr<OggReader>> _sfx;

	uint32_t _sampleRate;
	uint8_t _bgmVolume;
	uint8_t _sfxVolume;
	uint8_t _options;
	bool _paused;

public:
	OggMixer();

	void SetSampleRate(int sampleRate);
	void ApplySamples(int16_t* buffer, size_t sampleCount);
	
	void Reset();
	bool Play(string filename, bool isSfx, uint32_t startOffset);
	void SetPlaybackOptions(uint8_t options);
	void SetPausedFlag(bool paused);
	void StopBgm();
	void StopSfx();
	void SetBgmVolume(uint8_t volume);
	void SetSfxVolume(uint8_t volume);
	bool IsBgmPlaying();
	bool IsSfxPlaying();
	int32_t GetBgmOffset();
};
