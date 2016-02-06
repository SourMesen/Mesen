#pragma once
#include "stdafx.h"
#include <assert.h>
#include <numeric>

class LowPassFilter
{
private:
	uint8_t _prevSampleCounter = 0;
	int16_t _prevSamples[10] = { 0,0,0,0,0,0,0,0,0,0};

public:
	void ApplyFilter(int16_t *buffer, size_t sampleCount, int strength, double volume = 1.0f)
	{
		assert(strength <= 10);

		for(size_t i = 0; i < sampleCount; i++) {
			if(strength > 0) {
				int32_t sum = std::accumulate(&_prevSamples[0], &_prevSamples[strength], 0);
				buffer[i] = (int16_t)((sum + buffer[i]) / (strength + 1) * volume);
				_prevSamples[_prevSampleCounter] = buffer[i];
				_prevSampleCounter = (_prevSampleCounter + 1) % strength;
			} else {
				buffer[i] = (int16_t)(buffer[i] * volume);
			}
		}
	}
};