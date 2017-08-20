#pragma once
#include "stdafx.h"
#include "../Utilities/stb_vorbis.h"
#include "../Utilities/blip_buf.h"
#include "../Utilities/VirtualFile.h"
#include "EmulationSettings.h"

class OggReader
{
private:
	const int SamplesToRead = 100;

	stb_vorbis* _vorbis;
	int16_t* _oggBuffer;
	int16_t* _outputBuffer;

	bool _loop;
	bool _done;

	blip_t* _blipLeft;
	blip_t* _blipRight;

	int _sampleRate;
	int _oggSampleRate;

	vector<uint8_t> _fileData;
	
	bool LoadSamples();

public:
	OggReader();
	~OggReader();

	bool Init(string filename, bool loop, uint32_t sampleRate, uint32_t startOffset = 0);
	bool IsPlaybackOver();
	void SetSampleRate(int sampleRate);
	void SetLoopFlag(bool loop);
	void ApplySamples(int16_t* buffer, size_t sampleCount, uint8_t volume);
	uint32_t GetOffset();
};
