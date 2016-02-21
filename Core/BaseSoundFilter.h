#pragma once
#include "stdafx.h"

class BaseSoundFilter
{
protected:
	int16_t* _filterBuffer = nullptr;
	size_t _maxSampleCount = 0;

	void UpdateBufferSize(size_t sampleCount, bool isStereo);

public:
	virtual ~BaseSoundFilter();
};