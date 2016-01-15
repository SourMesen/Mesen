#pragma once

#include "stdafx.h"
#include "../Core/IAudioDevice.h"

class SoundManager : public IAudioDevice
{
public:
	SoundManager(HWND hWnd);
	~SoundManager();

	void Release();
	void PlayBuffer(int16_t *soundBuffer, uint32_t bufferSize, uint32_t sampleRate);
	void Play();	
	void Pause();
	void Stop();

private:
	bool InitializeDirectSound(uint32_t sampleRate);
	void ShutdownDirectSound();
	void ClearSecondaryBuffer();
	void CopyToSecondaryBuffer(uint8_t *data, uint32_t size);

private:
	HWND _hWnd;

	uint16_t _lastWriteOffset = 0;
	uint16_t _previousLatency = 0;
	uint32_t _sampleRate = 0;

	IDirectSound8* _directSound;
	IDirectSoundBuffer* _primaryBuffer;
	IDirectSoundBuffer8* _secondaryBuffer;
};
