#pragma once
#include "stdafx.h"
#include "BaseVideoFilter.h"
#include "../Utilities/nes_ntsc.h"

class NtscFilter : public BaseVideoFilter
{
private:
	nes_ntsc_setup_t _ntscSetup;
	nes_ntsc_t* _ntscData;

	void DoubleOutputHeight(uint32_t *outputBuffer);

public:
	NtscFilter();
	virtual ~NtscFilter();

	virtual void ApplyFilter(uint16_t *ppuOutputBuffer);
	virtual FrameInfo GetFrameInfo();
};