#pragma once

#include "stdafx.h"

class IAudioDevice
{
	public:
		virtual ~IAudioDevice() {}
		virtual void PlayBuffer(int16_t *soundBuffer, uint32_t bufferSize, uint32_t sampleRate, bool isStereo) = 0;
		virtual void Stop() = 0;
		virtual void Pause() = 0;
		
		virtual string GetAvailableDevices() = 0;
		virtual void SetAudioDevice(string deviceName) = 0;
		
};