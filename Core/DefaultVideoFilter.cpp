#include "stdafx.h"
#include "DefaultVideoFilter.h"
#include "EmulationSettings.h"

FrameInfo DefaultVideoFilter::GetFrameInfo()
{
	OverscanDimensions overscan = GetOverscan();
	return { overscan.GetScreenWidth(), overscan.GetScreenHeight(), 4 };
}

void DefaultVideoFilter::ApplyFilter(uint16_t *ppuOutputBuffer)
{
	OverscanDimensions overscan = EmulationSettings::GetOverscanDimensions();
	
	uint32_t* outputBuffer = (uint32_t*)GetOutputBuffer();
	for(uint32_t i = overscan.Top, iMax = 240 - overscan.Bottom; i < iMax; i++) {
		for(uint32_t j = overscan.Left, jMax = 256 - overscan.Right; j < jMax; j++) {
			*outputBuffer = ProcessIntensifyBits(ppuOutputBuffer[i * 256 + j]);
			outputBuffer++;
		}
	}
}

uint32_t DefaultVideoFilter::ProcessIntensifyBits(uint16_t ppuPixel)
{
	uint32_t pixelOutput = EmulationSettings::GetRgbPalette()[ppuPixel & 0x3F];

	//Incorrect emphasis bit implementation, but will do for now.
	float redChannel = (float)((pixelOutput & 0xFF0000) >> 16);
	float greenChannel = (float)((pixelOutput & 0xFF00) >> 8);
	float blueChannel = (float)(pixelOutput & 0xFF);

	if(ppuPixel & 0x40) {
		//Intensify red
		redChannel *= 1.1f;
		greenChannel *= 0.9f;
		blueChannel *= 0.9f;
	}
	if(ppuPixel & 0x80) {
		//Intensify green
		greenChannel *= 1.1f;
		redChannel *= 0.9f;
		blueChannel *= 0.9f;
	}
	if(ppuPixel & 0x100) {
		//Intensify blue
		blueChannel *= 1.1f;
		redChannel *= 0.9f;
		greenChannel *= 0.9f;
	}

	uint8_t r, g, b;
	r = (uint8_t)(redChannel > 255 ? 255 : redChannel);
	g = (uint8_t)(greenChannel > 255 ? 255 : greenChannel);
	b = (uint8_t)(blueChannel > 255 ? 255 : blueChannel);

	return 0xFF000000 | (r << 16) | (g << 8) | b;
}