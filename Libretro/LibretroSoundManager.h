#pragma once
#include "stdafx.h"
#include "../Core/IAudioDevice.h"
#include "../Core/SoundMixer.h"
#include "libretro.h"

class LibretroSoundManager : public IAudioDevice
{
private:
	retro_audio_sample_batch_t _sendAudioBuffer = nullptr;
	bool _skipMode = false;

public:
	LibretroSoundManager()
	{
		SoundMixer::RegisterAudioDevice(this);
	}

	~LibretroSoundManager()
	{
		SoundMixer::RegisterAudioDevice(nullptr);
	}

	// Inherited via IAudioDevice
	virtual void PlayBuffer(int16_t *soundBuffer, uint32_t sampleCount, uint32_t sampleRate, bool isStereo) override
	{
		if(!_skipMode && _sendAudioBuffer) {
			_sendAudioBuffer(soundBuffer, sampleCount);
		}
	}

	void SetSendAudioBuffer(retro_audio_sample_batch_t sendAudioBuffer)
	{
		_sendAudioBuffer = sendAudioBuffer;
	}

	void SetSkipMode(bool skip)
	{
		_skipMode = skip;
	}

	virtual void Stop() override
	{
	}

	virtual void Pause() override
	{
	}

	virtual string GetAvailableDevices() override
	{
		return string();
	}

	virtual void SetAudioDevice(string deviceName) override
	{
	}

	virtual void ProcessEndOfFrame() override
	{
	}

	virtual AudioStatistics GetStatistics() override
	{
		return AudioStatistics();
	}
};
