#pragma once
#include "stdafx.h"
#include <deque>
#include <algorithm>
#include "EmulationSettings.h"
#include "BaseSoundFilter.h"

class StereoDelayFilter : public BaseSoundFilter
{
private:
	std::deque<int16_t> _delayedSamples;
	int32_t _lastDelay;
	
public:
	int16_t* ApplyFilter(int16_t* monoBuffer, size_t sampleCount, uint32_t sampleRate);
};