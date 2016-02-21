#include "stdafx.h"
#include "StereoPanningFilter.h"
#include "EmulationSettings.h"
#include <cmath>

void StereoPanningFilter::UpdateFactors()
{
	double angle = EmulationSettings::GetStereoPanningAngle();
	_leftChannelFactor = _baseFactor * (std::cos(angle) - std::sin(angle));
	_rightChannelFactor = _baseFactor * (std::cos(angle) + std::sin(angle));
}

int16_t* StereoPanningFilter::ApplyFilter(int16_t* monoBuffer, size_t sampleCount)
{
	UpdateFactors();
	UpdateBufferSize(sampleCount, true);

	for(size_t i = 0; i < sampleCount; i++) {
		_filterBuffer[i * 2] = (int16_t)(_leftChannelFactor * (double)monoBuffer[i]);
		_filterBuffer[i * 2 + 1] = (int16_t)(_rightChannelFactor * (double)monoBuffer[i]);
	}

	return _filterBuffer;
}