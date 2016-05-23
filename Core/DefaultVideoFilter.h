#pragma once

#include "stdafx.h"
#include "BaseVideoFilter.h"

class DefaultVideoFilter : public BaseVideoFilter
{
protected:
	uint32_t ProcessIntensifyBits(uint16_t ppuPixel);

public:
	void ApplyFilter(uint16_t *ppuOutputBuffer);
	FrameInfo GetFrameInfo();
};