#pragma once

#include "stdafx.h"
#include "DefaultVideoFilter.h"

enum class ScaleFilterType
{
	xBRZ,
	HQX,
	Scale2x,
	_2xSai,
	Super2xSai,
	SuperEagle,
};

class ScaleFilter : public DefaultVideoFilter
{
private:
	uint32_t *_decodedPpuBuffer;
	uint32_t _filterScale;
	ScaleFilterType _scaleFilterType;

public:
	ScaleFilter(ScaleFilterType scaleFilterType, uint32_t scale);
	virtual ~ScaleFilter();

	void ApplyFilter(uint16_t *ppuOutputBuffer);
	FrameInfo GetFrameInfo();
};