#pragma once
#include "stdafx.h"
#include "BaseVideoFilter.h"

class HdNesPack;
class Console;
struct HdScreenInfo;
struct HdPackData;

class HdVideoFilter : public BaseVideoFilter
{
private:
	shared_ptr<HdPackData> _hdData;

	HdScreenInfo *_hdScreenInfo = nullptr;
	unique_ptr<HdNesPack> _hdNesPack = nullptr;

public:
	HdVideoFilter(shared_ptr<Console> console, shared_ptr<HdPackData> hdData);

	void ApplyFilter(uint16_t *ppuOutputBuffer) override;
	FrameInfo GetFrameInfo() override;
	OverscanDimensions GetOverscan() override;
	
	void SetHdScreenTiles(HdScreenInfo *hdScreenInfo);
};
