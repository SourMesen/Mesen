#include "stdafx.h"
#include "RawVideoFilter.h"
#include "PPU.h"

RawVideoFilter::RawVideoFilter(shared_ptr<Console> console) : BaseVideoFilter(console)
{
	//Use the same raw output as the Nestopia core
	for(int i = 0; i < 512; i++) {
		_rawPalette[i] = (
			(((i & 0x0F) * 255 / 15) << 16) |
			((((i >> 4) & 0x03) * 255 / 3) << 8) |
			(((i >> 6) & 0x07) * 255 / 7)
		);
	}
}

void RawVideoFilter::ApplyFilter(uint16_t * ppuOutputBuffer)
{
	//Do nothing - return 9-bit values (6-bit Palette + 3-bit emphasis)
	OverscanDimensions overscan = GetOverscan();
	uint32_t* out = GetOutputBuffer();
	for(uint32_t i = overscan.Top, iMax = 240 - overscan.Bottom; i < iMax; i++) {
		for(uint32_t j = overscan.Left, jMax = 256 - overscan.Right; j < jMax; j++) {
			*out = _rawPalette[ppuOutputBuffer[i * 256 + j]];
			out++;
		}
	}
}

FrameInfo RawVideoFilter::GetFrameInfo()
{
	OverscanDimensions overscan = GetOverscan();
	return { overscan.GetScreenWidth(), overscan.GetScreenHeight(), PPU::ScreenWidth, PPU::ScreenHeight, 4 };
}
