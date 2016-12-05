#include "stdafx.h"
#include "StereoDelayFilter.h"

int16_t* StereoDelayFilter::ApplyFilter(int16_t* monoBuffer, size_t sampleCount, uint32_t sampleRate)
{
	UpdateBufferSize(sampleCount, true);

	size_t delaySampleCount = (int32_t)((double)EmulationSettings::GetStereoDelay() / 1000 * sampleRate);
	if(delaySampleCount != _lastDelay) {
		_delayedSamples.clear();
	}
	_lastDelay = delaySampleCount;
	
	for(size_t i = 0; i < sampleCount; i++) {
		_delayedSamples.push_back(monoBuffer[i]);
	}

	for(size_t i = 0; i < sampleCount; i++) {
		_filterBuffer[i * 2] = monoBuffer[i];
		_filterBuffer[i * 2 + 1] = 0;
	}

	if(_delayedSamples.size() > delaySampleCount) {
		size_t samplesToInsert = std::max<size_t>(_delayedSamples.size() - delaySampleCount, sampleCount);

		for(size_t i = sampleCount - samplesToInsert; i < sampleCount; i++) {
			_filterBuffer[i * 2 + 1] = _delayedSamples.front();
			_delayedSamples.pop_front();
		}
	}

	return _filterBuffer;
}
