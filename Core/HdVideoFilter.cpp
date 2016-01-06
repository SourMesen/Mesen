#include "stdafx.h"
#include "HdNesPack.h"
#include "HdVideoFilter.h"

extern const uint32_t PPU_PALETTE_ARGB[];

HdVideoFilter::HdVideoFilter()
{
	_hdNesPack.reset(new HdNesPack());
}

FrameInfo HdVideoFilter::GetFrameInfo()
{
	OverscanDimensions overscan = GetOverscan();
	uint32_t hdScale = _hdNesPack->GetScale();

	return { overscan.GetScreenWidth() * hdScale, overscan.GetScreenHeight() * hdScale, 4 };
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

	for(uint32_t i = overscan.Top, iMax = 240 - overscan.Bottom; i < iMax; i++) {
		for(uint32_t j = overscan.Left, jMax = 256 - overscan.Right; j < jMax; j++) {
			uint32_t sdPixel = PPU_PALETTE_ARGB[ppuOutputBuffer[i * 256 + j] & 0x3F]; //ProcessIntensifyBits(inputBuffer[i * 256 + j]);
			uint32_t bufferIndex = (i - overscan.Top) * screenWidth * hdScale + (j - overscan.Left) * hdScale;
			_hdNesPack->GetPixels(_hdScreenTiles[i * 256 + j], sdPixel, (uint32_t*)GetOutputBuffer() + bufferIndex, screenWidth);
		}
	}
}