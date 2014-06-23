#pragma once

#include "stdafx.h"
#include "../Core/APU.h"

class SoundManager : public IAudioDevice
{
public:
	SoundManager();
	~SoundManager();

	bool Initialize(HWND hWnd);
	void Release();
	void PlayBuffer(int16_t *soundBuffer, uint32_t bufferSize);

private:
	bool InitializeDirectSound(HWND);
	void ShutdownDirectSound();
	void ClearSecondaryBuffer();
	void CopyToSecondaryBuffer(uint8_t *data, uint32_t size);

private:
	vector<uint8_t> _buffer;
	IDirectSound8* _directSound;
	IDirectSoundBuffer* _primaryBuffer;
	IDirectSoundBuffer8* _secondaryBuffer;
};
