#pragma once
#include "stdafx.h"
#include "HdData.h"

class HdNesPack
{
private:
	int32_t _backgroundIndex = -1;
	uint32_t* _palette = nullptr;

	__forceinline void BlendColors(uint8_t output[4], uint8_t input[4]);
	__forceinline uint32_t AdjustBrightness(uint8_t input[4], uint16_t brightness);
	__forceinline void DrawColor(uint32_t color, uint32_t* outputBuffer, uint32_t scale, uint32_t screenWidth);
	__forceinline void DrawTile(HdPpuTileInfo &tileInfo, HdPackTileInfo &hdPackTileInfo, uint32_t* outputBuffer, uint32_t screenWidth);
	__forceinline HdPackTileInfo* GetMatchingTile(HdPpuPixelInfo *screenTiles, uint32_t x, uint32_t y, HdPpuTileInfo* tile);

	__forceinline bool IsNextToSprite(HdPpuPixelInfo *screenTiles, uint32_t x, uint32_t y);
	__forceinline uint32_t GetCustomBackgroundPixel(int x, int y, int offsetX, int offsetY);
	__forceinline void DrawCustomBackground(uint32_t *outputBuffer, uint32_t x, uint32_t y, uint32_t scale, uint32_t screenWidth);

public:
	HdNesPack();
	~HdNesPack();

	uint32_t GetScale();
	
	void OnBeforeApplyFilter(HdPpuPixelInfo *screenTiles);
	void GetPixels(HdPpuPixelInfo *screenTiles, uint32_t x, uint32_t y, HdPpuPixelInfo &pixelInfo, uint32_t *outputBuffer, uint32_t screenWidth);
};
