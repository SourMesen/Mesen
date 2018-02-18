#pragma once
#include "stdafx.h"
#include "BaseVideoFilter.h"

class HdNesPack;
struct HdScreenInfo;

class HdVideoFilter : public BaseVideoFilter
{
private:
	HdScreenInfo *_hdScreenInfo = nullptr;
	unique_ptr<HdNesPack> _hdNesPack = nullptr;

public:
	HdVideoFilter();

	void ApplyFilter(uint16_t *ppuOutputBuffer) override;
	FrameInfo GetFrameInfo() override;
	OverscanDimensions GetOverscan() override;
	
	void SetHdScreenTiles(HdScreenInfo *hdScreenInfo);
};
