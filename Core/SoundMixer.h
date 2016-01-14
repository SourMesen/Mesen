#pragma once
#include "stdafx.h"
#include "EmulationSettings.h"
#include "../BlipBuffer/blip_buf.h"

class SoundMixer
{
private:
	int16_t _previousOutput = 0;

	vector<uint32_t> _timestamps;
	int8_t _channelOutput[5][15000];
	int8_t _currentOutput[5];

	int16_t _lupSquare[31];
	int16_t _lupTnd[203];

	blip_t* _blipBuf;
	int16_t *_outputBuffer;
	double _volumes[5];

	void InitializeLookupTables();
	int16_t GetOutputVolume();
	void EndFrame(uint32_t time);

public:
	SoundMixer();
	~SoundMixer();

	void SetNesModel(NesModel model);
	void Reset();
	
	void PlayAudioBuffer(uint32_t cycle);
	void AddDelta(AudioChannel channel, uint32_t time, int8_t output);
};
