#include "stdafx.h"
#include "PPU.h"
#include "HdNesPack.h"
#include "HdVideoFilter.h"
#include "Console.h"

HdVideoFilter::HdVideoFilter()
{
	_hdNesPack.reset(new HdNesPack());
}

FrameInfo HdVideoFilter::GetFrameInfo()
{
	OverscanDimensions overscan = GetOverscan();
	uint32_t hdScale = _hdNesPack->GetScale();

	return { overscan.GetScreenWidth() * hdScale, overscan.GetScreenHeight() * hdScale, PPU::ScreenWidth*hdScale, PPU::ScreenHeight*hdScale, 4 };
}

OverscanDimensions HdVideoFilter::GetOverscan()
{
	HdPackData* hdData = Console::GetHdData();
	if(hdData->HasOverscanConfig) {
		return hdData->Overscan;
	} else {
		return BaseVideoFilter::GetOverscan();
	}
}

void HdVideoFilter::SetHdScreenTiles(HdScreenInfo *hdScreenInfo)
{
	_hdScreenInfo = hdScreenInfo;
}

void HdVideoFilter::ApplyFilter(uint16_t *ppuOutputBuffer)
{
	OverscanDimensions overscan = GetOverscan();
	_hdNesPack->Process(_hdScreenInfo, GetOutputBuffer(), overscan);
}