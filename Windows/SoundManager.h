#pragma once

#include "stdafx.h"
#include "../Core/APU.h"

class SoundManager : public IAudioDevice
{
public:
	SoundManager(HWND hWnd);
	~SoundManager();

	void Release();
	void PlayBuffer(int16_t *soundBuffer, uint32_t bufferSize);
	void Play();	
	void Pause();
	void Stop();

private:
	bool InitializeDirectSound(HWND);
	void ShutdownDirectSound();
	void ClearSecondaryBuffer();
	void CopyToSecondaryBuffer(uint8_t *data, uint32_t size);

private:
	uint16_t _lastWriteOffset = 0;
	uint16_t _previousLatency = 0;

	IDirectSound8* _directSound;
	IDirectSoundBuffer* _primaryBuffer;
	IDirectSoundBuffer8* _secondaryBuffer;
};
