#pragma once
#include "stdafx.h"
#include "../Utilities/blip_buf.h"

class WavReader
{
private:
	int16_t* _outputBuffer;

	uint8_t* _fileData;
	uint32_t _fileOffset;
	uint32_t _fileSize;
	uint32_t _dataStartOffset;

	int16_t _prevSample;

	bool _done;

	blip_t* _blip;

	uint32_t _fileSampleRate;
	uint32_t _sampleRate;

	void LoadSamples(uint32_t samplesToLoad);

	WavReader();

public:
	static shared_ptr<WavReader> Create(uint8_t* wavData, uint32_t length);

	~WavReader();

	void Play(uint32_t startSample);
	bool IsPlaybackOver();
	void SetSampleRate(uint32_t sampleRate);
	void ApplySamples(int16_t* buffer, size_t sampleCount, double volume);
	int32_t GetPosition();
	uint32_t GetSampleRate();
};
