#include "stdafx.h"
#include "PPU.h"
#include "HdNesPack.h"
#include "HdVideoFilter.h"
#include "Console.h"

HdVideoFilter::HdVideoFilter(shared_ptr<Console> console, shared_ptr<HdPackData> hdData) : BaseVideoFilter(console)
{
	_hdData = hdData;
	_hdNesPack.reset(new HdNesPack(hdData, console->GetSettings()));
}

FrameInfo HdVideoFilter::GetFrameInfo()
{
	OverscanDimensions overscan = GetOverscan();
	uint32_t hdScale = _hdNesPack->GetScale();

	return { overscan.GetScreenWidth() * hdScale, overscan.GetScreenHeight() * hdScale, PPU::ScreenWidth*hdScale, PPU::ScreenHeight*hdScale, 4 };
}

OverscanDimensions HdVideoFilter::GetOverscan()
{
	if(_hdData->HasOverscanConfig) {
		return _hdData->Overscan;
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