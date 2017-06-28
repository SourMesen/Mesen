#pragma once

#include "stdafx.h"
#include "DefaultVideoFilter.h"

class ScaleFilter : public DefaultVideoFilter
{
private:
	static bool _hqxInitDone;
	uint32_t *_decodedPpuBuffer;
	uint32_t _filterScale;
	ScaleFilterType _scaleFilterType;

	void ApplyPrescaleFilter();

public:
	ScaleFilter(ScaleFilterType scaleFilterType, uint32_t scale);
	virtual ~ScaleFilter();

	void ApplyFilter(uint16_t *ppuOutputBuffer);
	FrameInfo GetFrameInfo();
};