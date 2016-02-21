#include "stdafx.h"
#include "BaseSoundFilter.h"

void BaseSoundFilter::UpdateBufferSize(size_t sampleCount, bool isStereo)
{
	if(_maxSampleCount < sampleCount) {
		if(_filterBuffer) {
			delete[] _filterBuffer;
		}
		_maxSampleCount = sampleCount;
		_filterBuffer = new int16_t[_maxSampleCount * (isStereo ? 2 : 1)];
		memset(_filterBuffer, 0, _maxSampleCount * sizeof(int16_t) * (isStereo ? 2 : 1));
	}
}

BaseSoundFilter::~BaseSoundFilter()
{
	if(_filterBuffer) {
		delete[] _filterBuffer;
	}
}