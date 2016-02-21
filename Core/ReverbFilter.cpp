#include "stdafx.h"
#include "ReverbFilter.h"

void ReverbFilter::ResetFilter()
{
	for(int i = 0; i < 5; i++) {
		_delay[i].Reset();
	}
}

int16_t* ReverbFilter::ApplyFilter(int16_t* monoBuffer, size_t sampleCount, uint32_t sampleRate, double reverbStrength, double reverbDelay)
{
	_delay[0].SetParameters(550 * reverbDelay, 0.25 * reverbStrength, sampleRate);
	_delay[1].SetParameters(330 * reverbDelay, 0.15 * reverbStrength, sampleRate);
	_delay[2].SetParameters(485 * reverbDelay, 0.12 * reverbStrength, sampleRate);
	_delay[3].SetParameters(150 * reverbDelay, 0.20 * reverbStrength, sampleRate);
	_delay[4].SetParameters(285 * reverbDelay, 0.05 * reverbStrength, sampleRate);

	UpdateBufferSize(sampleCount, false);

	memcpy(_filterBuffer, monoBuffer, sampleCount * sizeof(int16_t));

	for(int i = 0; i < 5; i++) {
		_delay[i].ApplyReverb(_filterBuffer, sampleCount);
	}
	for(int i = 0; i < 5; i++) {
		_delay[i].AddSamples(_filterBuffer, sampleCount);
	}

	return _filterBuffer;
}