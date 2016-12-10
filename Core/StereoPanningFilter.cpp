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

void StereoPanningFilter::ApplyFilter(int16_t* stereoBuffer, size_t sampleCount)
{
	UpdateFactors();

	for(size_t i = 0; i < sampleCount * 2; i+=2) {
		int16_t leftSample = stereoBuffer[i];
		int16_t rightSample = stereoBuffer[i+1];
		stereoBuffer[i] = (int16_t)((_leftChannelFactor * leftSample + _leftChannelFactor * rightSample) / 2);
		stereoBuffer[i+1] = (int16_t)((_rightChannelFactor * rightSample + _rightChannelFactor * leftSample) / 2);
	}
}