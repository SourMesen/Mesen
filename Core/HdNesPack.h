#pragma once
#include "stdafx.h"
#include "HdData.h"

class HdNesPack
{
private:
	int32_t _backgroundIndex = -1;

	void BlendColors(uint8_t output[4], uint8_t input[4]);
	uint32_t AdjustBrightness(uint8_t input[4], uint16_t brightness);
	void DrawTile(HdPpuTileInfo &tileInfo, HdPackTileInfo &hdPackTileInfo, uint32_t* outputBuffer, uint32_t screenWidth, bool drawBackground);

	bool IsNextToSprite(HdPpuPixelInfo *screenTiles, uint32_t x, uint32_t y);
	uint32_t GetCustomBackgroundPixel(int x, int y, int offsetX, int offsetY);

public:
	HdNesPack();
	~HdNesPack();

	uint32_t GetScale();

	void OnBeforeApplyFilter(HdPpuPixelInfo *screenTiles);
	HdPackTileInfo* GetMatchingTile(HdPpuPixelInfo *screenTiles, uint32_t x, uint32_t y, HdTileKey& key);
	void GetPixels(HdPpuPixelInfo *screenTiles, uint32_t x, uint32_t y, HdPpuPixelInfo &pixelInfo, uint32_t sdPixel, uint32_t *outputBuffer, uint32_t screenWidth);
};
