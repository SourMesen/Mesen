#pragma once
#include "stdafx.h"
#include "HdData.h"

class HdNesPack
{
private:
	shared_ptr<HdPackData> _hdData;

	int32_t _backgroundIndex = -1;
	HdScreenInfo *_hdScreenInfo = nullptr;
	uint32_t* _palette = nullptr;
	bool _contoursEnabled = false;
	HdPackTileInfo* _cachedTile = nullptr;
	bool _cacheEnabled = false;
	bool _useCachedTile = false;
	int32_t _scrollX = 0;
	int32_t _bgScrollX = 0;
	int32_t _bgScrollY = 0;

	__forceinline void BlendColors(uint8_t output[4], uint8_t input[4]);
	__forceinline uint32_t AdjustBrightness(uint8_t input[4], uint16_t brightness);
	__forceinline void DrawColor(uint32_t color, uint32_t* outputBuffer, uint32_t scale, uint32_t screenWidth);
	__forceinline void DrawTile(HdPpuTileInfo &tileInfo, HdPackTileInfo &hdPackTileInfo, uint32_t* outputBuffer, uint32_t screenWidth);
	
	__forceinline HdPackTileInfo* GetCachedMatchingTile(uint32_t x, uint32_t y, HdPpuTileInfo* tile);
	__forceinline HdPackTileInfo* GetMatchingTile(uint32_t x, uint32_t y, HdPpuTileInfo* tile, bool* disableCache = nullptr);

	__forceinline bool IsNextToSprite(uint32_t x, uint32_t y);
	__forceinline void DrawCustomBackground(uint32_t *outputBuffer, uint32_t x, uint32_t y, uint32_t scale, uint32_t screenWidth);

	void OnLineStart(HdPpuPixelInfo &lineFirstPixel);
	void OnBeforeApplyFilter();
	__forceinline void GetPixels(uint32_t x, uint32_t y, HdPpuPixelInfo &pixelInfo, uint32_t *outputBuffer, uint32_t screenWidth);
	__forceinline void ProcessGrayscaleAndEmphasis(HdPpuPixelInfo &pixelInfo, uint32_t* outputBuffer, uint32_t hdScreenWidth);

public:
	static constexpr uint32_t CurrentVersion = 102;

	HdNesPack(shared_ptr<HdPackData> hdData);
	~HdNesPack();

	uint32_t GetScale();
	
	void Process(HdScreenInfo *hdScreenInfo, uint32_t *outputBuffer, OverscanDimensions &overscan);
};
