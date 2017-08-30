#pragma once
#include "stdafx.h"
#include "BaseVideoFilter.h"

class HdNesPack;

class HdVideoFilter : public BaseVideoFilter
{
private:
	HdPpuPixelInfo *_hdScreenTiles = nullptr;
	unique_ptr<HdNesPack> _hdNesPack = nullptr;

public:
	HdVideoFilter();

	void ApplyFilter(uint16_t *ppuOutputBuffer) override;
	FrameInfo GetFrameInfo() override;
	OverscanDimensions GetOverscan() override;
	
	void SetHdScreenTiles(HdPpuPixelInfo *screenTiles);
};
