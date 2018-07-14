#pragma once
#include "stdafx.h"
#include "BaseVideoFilter.h"
#include "../Utilities/nes_ntsc.h"

class Console;

class NtscFilter : public BaseVideoFilter
{
private:
	nes_ntsc_setup_t _ntscSetup;
	nes_ntsc_t* _ntscData;
	bool _keepVerticalRes = false;
	uint8_t _basePalette[64 * 3];
	uint32_t* _ntscBuffer;

	void GenerateArgbFrame(uint32_t *outputBuffer);

protected:
	void OnBeforeApplyFilter();

public:
	NtscFilter(shared_ptr<Console> console);
	virtual ~NtscFilter();

	virtual void ApplyFilter(uint16_t *ppuOutputBuffer);
	virtual FrameInfo GetFrameInfo();
};