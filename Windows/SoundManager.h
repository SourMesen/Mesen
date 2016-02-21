#pragma once

#include "stdafx.h"
#include "../Core/IAudioDevice.h"

struct SoundDeviceInfo
{
	string description;
	GUID guid;
};

class SoundManager : public IAudioDevice
{
public:
	SoundManager(HWND hWnd);
	~SoundManager();

	void Release();
	void PlayBuffer(int16_t *soundBuffer, uint32_t bufferSize, uint32_t sampleRate, bool isStereo);
	void Play();	
	void Pause();
	void Stop();

	string GetAvailableDevices();
	void SetAudioDevice(string deviceName);

private:
	vector<SoundDeviceInfo> GetAvailableDeviceInfo();
	static bool CALLBACK DirectSoundEnumProc(LPGUID lpGUID, LPCWSTR lpszDesc, LPCSTR lpszDrvName, LPVOID lpContext);
	bool InitializeDirectSound(uint32_t sampleRate, bool isStereo);
	void ShutdownDirectSound();
	void ClearSecondaryBuffer();
	void CopyToSecondaryBuffer(uint8_t *data, uint32_t size);

private:
	HWND _hWnd;
	GUID _audioDeviceID;
	bool _needReset = false;

	uint16_t _lastWriteOffset = 0;
	uint16_t _previousLatency = 0;
	uint32_t _sampleRate = 0;
	bool _isStereo = false;

	IDirectSound8* _directSound;
	IDirectSoundBuffer* _primaryBuffer;
	IDirectSoundBuffer8* _secondaryBuffer;
};
