#pragma once
#include <SDL2/SDL.h>
#include "../Core/IAudioDevice.h"

class SdlSoundManager : public IAudioDevice
{
public:
	SdlSoundManager();
	~SdlSoundManager();

	void PlayBuffer(int16_t *soundBuffer, uint32_t bufferSize, uint32_t sampleRate, bool isStereo);
	void Pause();
	void Stop();

	string GetAvailableDevices();
	void SetAudioDevice(string deviceName);

private:
	vector<string> GetAvailableDeviceInfo();
	bool InitializeAudio(uint32_t sampleRate, bool isStereo);

	static void FillAudioBuffer(void *userData, uint8_t *stream, int len);

	void ReadFromBuffer(uint8_t* output, uint32_t len);
	void WriteToBuffer(uint8_t* output, uint32_t len);

private:
	SDL_AudioDeviceID _audioDeviceID;
	string _deviceName;
	bool _needReset = false;

	uint16_t _lastWriteOffset = 0;
	uint16_t _previousLatency = 0;
	uint32_t _sampleRate = 0;
	bool _isStereo = false;

	uint8_t* _buffer = nullptr;
	uint32_t _writePosition = 0;
	uint32_t _readPosition = 0;
};
