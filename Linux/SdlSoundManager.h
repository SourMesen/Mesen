﻿#pragma once
#include <SDL2/SDL.h>
#include "../Core/BaseSoundManager.h"

class Console;

class SdlSoundManager : public BaseSoundManager
{
public:
	SdlSoundManager(shared_ptr<Console> console);
	~SdlSoundManager();

	void PlayBuffer(int16_t *soundBuffer, uint32_t bufferSize, uint32_t sampleRate, bool isStereo) override;
	void Pause() override;
	void Stop() override;

	void ProcessEndOfFrame() override;
	void UpdateSoundSettings() override;

	string GetAvailableDevices() override;
	void SetAudioDevice(string deviceName) override;

private:
	vector<string> GetAvailableDeviceInfo();
	bool InitializeAudio(uint32_t sampleRate, bool isStereo);
	void Release();

	static void FillAudioBuffer(void *userData, uint8_t *stream, int len);

	void ReadFromBuffer(uint8_t* output, uint32_t len);
	void WriteToBuffer(uint8_t* output, uint32_t len);

private:
	shared_ptr<Console> _console;
	SDL_AudioDeviceID _audioDeviceID;
	string _deviceName;
	bool _needReset = false;

	uint16_t _previousLatency = 0;

	uint8_t* _buffer = nullptr;
	uint32_t _writePosition = 0;
	uint32_t _readPosition = 0;
};
