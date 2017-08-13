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

void HdVideoFilter::SetHdScreenTiles(HdPpuPixelInfo *screenTiles)
{
	_hdScreenTiles = screenTiles;
}

void HdVideoFilter::ApplyFilter(uint16_t *ppuOutputBuffer)
{
	OverscanDimensions overscan = GetOverscan();
	
	uint32_t hdScale = _hdNesPack->GetScale();
	uint32_t screenWidth = overscan.GetScreenWidth() * hdScale;

	_hdNesPack->OnBeforeApplyFilter(_hdScreenTiles);
	for(uint32_t i = overscan.Top, iMax = 240 - overscan.Bottom; i < iMax; i++) {
		for(uint32_t j = overscan.Left, jMax = 256 - overscan.Right; j < jMax; j++) {
			uint32_t bufferIndex = (i - overscan.Top) * screenWidth * hdScale + (j - overscan.Left) * hdScale;

			_hdNesPack->GetPixels(_hdScreenTiles, j, i, _hdScreenTiles[i * 256 + j], (uint32_t*)GetOutputBuffer() + bufferIndex, screenWidth);
		}
	}
}