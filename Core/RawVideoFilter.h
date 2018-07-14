#pragma once

#include "stdafx.h"
#include "BaseVideoFilter.h"

class Console;

class RawVideoFilter : public BaseVideoFilter
{
private:
	uint32_t _rawPalette[512];

public:
	RawVideoFilter(shared_ptr<Console> console);

	void ApplyFilter(uint16_t *ppuOutputBuffer);	
	FrameInfo GetFrameInfo();
};