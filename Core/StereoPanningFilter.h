#pragma once
#include "stdafx.h"
#include "BaseSoundFilter.h"

class StereoPanningFilter : public BaseSoundFilter
{
private:
	const double _baseFactor = 0.70710678118654752440084436210485; // == sqrt(2)/2
	double _leftChannelFactor = 0;
	double _rightChannelFactor = 0;

	void UpdateFactors();

public:
	int16_t* ApplyFilter(int16_t* monoBuffer, size_t sampleCount);
};