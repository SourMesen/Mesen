#include "stdafx.h"
#include <algorithm>
#include <unordered_map>
#include "HdNesPack.h"
#include "Console.h"
#include "MessageManager.h"
#include "EmulationSettings.h"
#include "HdPackLoader.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/PNGHelper.h"

HdNesPack::HdNesPack()
{
}

HdNesPack::~HdNesPack()
{
}

void HdNesPack::BlendColors(uint8_t output[4], uint8_t input[4])
{
	uint8_t alpha = input[3] + 1;
	uint8_t invertedAlpha = 256 - input[3];
	output[0] = (uint8_t)((alpha * input[0] + invertedAlpha * output[0]) >> 8);
	output[1] = (uint8_t)((alpha * input[1] + invertedAlpha * output[1]) >> 8);
	output[2] = (uint8_t)((alpha * input[2] + invertedAlpha * output[2]) >> 8);
	output[3] = 0xFF;
}

uint32_t HdNesPack::AdjustBrightness(uint8_t input[4], uint16_t brightness)
{
	uint8_t output[4];
	output[0] = std::min(255, (brightness * input[0]) >> 8);
	output[1] = std::min(255, (brightness * input[1]) >> 8);
	output[2] = std::min(255, (brightness * input[2]) >> 8);
	output[3] = input[3];
	return *((uint32_t*)output);
}

void HdNesPack::DrawColor(uint32_t color, uint32_t *outputBuffer, uint32_t scale, uint32_t screenWidth)
{
	if(scale == 1) {
		*outputBuffer = color;
	} else {
		for(uint32_t i = 0; i < scale; i++) {
			std::fill(outputBuffer, outputBuffer + scale, color);
			outputBuffer += screenWidth;
		}
	}
}

void HdNesPack::DrawCustomBackground(uint32_t *outputBuffer, uint32_t x, uint32_t y, uint32_t scale, uint32_t screenWidth)
{
	HdPackData *hdData = Console::GetHdData();
	uint8_t brightness = hdData->Backgrounds[_backgroundIndex].Brightness;
	uint32_t width = hdData->Backgrounds[_backgroundIndex].Data->Width;
	uint32_t *pngData = hdData->Backgrounds[_backgroundIndex].data() + (y * hdData->Scale * width) + (x * hdData->Scale);

	if(scale == 1) {
		if(brightness == 255) {
			*outputBuffer = *pngData;
		} else {
			*outputBuffer = AdjustBrightness((uint8_t*)pngData, brightness);
		}
	} else {
		uint32_t *buffer = outputBuffer;
		for(uint32_t i = 0; i < scale; i++) {
			memcpy(outputBuffer, pngData, sizeof(uint32_t) * scale);
			outputBuffer += screenWidth;
			pngData += width;
		}

		if(brightness < 255) {
			for(uint32_t i = 0; i < scale; i++) {
				for(uint32_t j = 0; j < scale; j++) {
					*buffer = AdjustBrightness((uint8_t*)buffer, brightness);
					buffer++;
				}
				buffer += screenWidth - scale;
			}
		}
	}
}

void HdNesPack::DrawTile(HdPpuTileInfo &tileInfo, HdPackTileInfo &hdPackTileInfo, uint32_t *outputBuffer, uint32_t screenWidth)
{
	if(hdPackTileInfo.IsFullyTransparent) {
		return;
	}

	uint32_t scale = GetScale();
	uint32_t *bitmapData = hdPackTileInfo.HdTileData.data();
	uint32_t tileWidth = 8 * scale;
	uint8_t tileOffsetX = tileInfo.HorizontalMirroring ? 7 - tileInfo.OffsetX : tileInfo.OffsetX;
	uint32_t bitmapOffset = (tileInfo.OffsetY * scale) * tileWidth + tileOffsetX * scale;
	int32_t bitmapSmallInc = 1;
	int32_t bitmapLargeInc = tileWidth - scale;
	if(tileInfo.HorizontalMirroring) {
		bitmapOffset += scale - 1;
		bitmapSmallInc = -1;
		bitmapLargeInc = tileWidth + scale;
	}
	if(tileInfo.VerticalMirroring) {
		bitmapOffset += tileWidth * (scale - 1);
		bitmapLargeInc = (tileInfo.HorizontalMirroring ? (int32_t)scale : -(int32_t)scale) - (int32_t)tileWidth;
	}

	uint32_t rgbValue;
	if(hdPackTileInfo.HasTransparentPixels || hdPackTileInfo.Brightness < 255) {
		for(uint32_t y = 0; y < scale; y++) {
			for(uint32_t x = 0; x < scale; x++) {
				if(hdPackTileInfo.Brightness == 255) {
					rgbValue = *(bitmapData + bitmapOffset);
				} else {
					rgbValue = AdjustBrightness((uint8_t*)(bitmapData + bitmapOffset), hdPackTileInfo.Brightness);
				}

				if(!hdPackTileInfo.HasTransparentPixels || (bitmapData[bitmapOffset] & 0xFF000000) == 0xFF000000) {
					*outputBuffer = rgbValue;
				} else {
					if(bitmapData[bitmapOffset] & 0xFF000000) {
						BlendColors((uint8_t*)outputBuffer, (uint8_t*)&rgbValue);
					}
				}
				outputBuffer++;
				bitmapOffset += bitmapSmallInc;
			}
			bitmapOffset += bitmapLargeInc;
			outputBuffer += screenWidth - scale;
		}
	} else {
		for(uint32_t y = 0; y < scale; y++) {
			for(uint32_t x = 0; x < scale; x++) {
				*outputBuffer = *(bitmapData + bitmapOffset);
				outputBuffer++;
				bitmapOffset += bitmapSmallInc;
			}
			bitmapOffset += bitmapLargeInc;
			outputBuffer += screenWidth - scale;
		}
	}
}

uint32_t HdNesPack::GetScale()
{
	return Console::GetHdData()->Scale;
}

void HdNesPack::OnLineStart(HdPpuPixelInfo &lineFirstPixel)
{
	_scrollX = ((lineFirstPixel.TmpVideoRamAddr & 0x1F) << 3) | lineFirstPixel.XScroll | ((lineFirstPixel.TmpVideoRamAddr & 0x400) ? 0x100 : 0);
	_useCachedTile = false;

	if(_backgroundIndex >= 0) {
		int32_t scrollY = (((lineFirstPixel.TmpVideoRamAddr & 0x3E0) >> 2) | ((lineFirstPixel.TmpVideoRamAddr & 0x7000) >> 12)) + ((lineFirstPixel.TmpVideoRamAddr & 0x800) ? 240 : 0);
		HdBackgroundInfo &bgInfo = Console::GetHdData()->Backgrounds[_backgroundIndex];

		_bgScrollX = (int32_t)(_scrollX * bgInfo.HorizontalScrollRatio);
		_bgScrollY = (int32_t)(scrollY * bgInfo.VerticalScrollRatio);
	}
}

void HdNesPack::OnBeforeApplyFilter()
{
	HdPackData* hdData = Console::GetHdData();

	_palette = hdData->Palette.size() == 0x40 ? hdData->Palette.data() : EmulationSettings::GetRgbPalette();
	_contoursEnabled = (hdData->OptionFlags & (int)HdPackOptions::NoContours) == 0;
	_cacheEnabled = (hdData->OptionFlags & (int)HdPackOptions::DisableCache) == 0;

	if(hdData->OptionFlags & (int)HdPackOptions::NoSpriteLimit) {
		EmulationSettings::SetFlags(EmulationFlags::RemoveSpriteLimit | EmulationFlags::AdaptiveSpriteLimit);
	}

	_backgroundIndex = -1;
	for(size_t i = 0; i < hdData->Backgrounds.size(); i++) {
		bool isMatch = true;
		for(HdPackCondition* condition : hdData->Backgrounds[i].Conditions) {
			if(!condition->CheckCondition(_hdScreenInfo, 0, 0, nullptr)) {
				isMatch = false;
				break;
			}
		}

		if(isMatch) {
			_backgroundIndex = (int32_t)i;
			break;
		}
	}

	for(unique_ptr<HdPackCondition> &condition : hdData->Conditions) {
		condition->ClearCache();
	}
}

HdPackTileInfo* HdNesPack::GetCachedMatchingTile(uint32_t x, uint32_t y, HdPpuTileInfo* tile)
{
	if(((_scrollX + x) & 0x07) == 0) {
		_useCachedTile = false;
	}

	bool disableCache = false;
	HdPackTileInfo* hdPackTileInfo;
	if(_useCachedTile) {
		hdPackTileInfo = _cachedTile;
	} else {
		hdPackTileInfo = GetMatchingTile(x, y, tile, &disableCache);

		if(!disableCache && _cacheEnabled) {
			//Use this tile for the next 8 horizontal pixels
			//Disable cache if a sprite condition is used, because sprites are not on a 8x8 grid
			_cachedTile = hdPackTileInfo;
			_useCachedTile = true;
		}
	}
	return hdPackTileInfo;
}

HdPackTileInfo* HdNesPack::GetMatchingTile(uint32_t x, uint32_t y, HdPpuTileInfo* tile, bool* disableCache)
{
	HdPackData *hdData = Console::GetHdData();
	auto hdTile = hdData->TileByKey.find(*tile);
	if(hdTile == hdData->TileByKey.end()) {
		hdTile = hdData->TileByKey.find(tile->GetKey(true));
	}

	if(hdTile != hdData->TileByKey.end()) {
		for(HdPackTileInfo* hdPackTile : hdTile->second) {
			if(disableCache != nullptr && hdPackTile->ForceDisableCache) {
				*disableCache = true;
			}

			if(hdPackTile->MatchesCondition(_hdScreenInfo, x, y, tile)) {
				return hdPackTile;
			}
		}
	}

	return nullptr;
}

bool HdNesPack::IsNextToSprite(uint32_t x, uint32_t y)
{
	bool hasNonBackgroundSurrounding = false;
	auto processAdjacentTile = [&hasNonBackgroundSurrounding](HdPpuPixelInfo& pixelInfo) {
		if(pixelInfo.Tile.BgColorIndex != 0) {
			hasNonBackgroundSurrounding = true;
		} else {
			for(int i = 0; i < pixelInfo.SpriteCount; i++) {
				if(pixelInfo.Sprite[i].SpriteColorIndex == 0 || pixelInfo.Sprite[i].SpriteColor != pixelInfo.Sprite[i].BgColor) {
					hasNonBackgroundSurrounding |= pixelInfo.Sprite[i].TileIndex != HdPpuTileInfo::NoTile && pixelInfo.Sprite[i].SpriteColorIndex != 0;
				}
				if(hasNonBackgroundSurrounding) {
					break;
				}
			}
		}
	};
	for(int i = -1; i <= 1; i++) {
		if((int)y + i < 0 || y + i >= PPU::ScreenHeight) {
			continue;
		}

		for(int j = -1; j <= 1; j++) {
			if((int)x + j < 0 || x + j >= PPU::ScreenWidth) {
				continue;
			}

			if(!hasNonBackgroundSurrounding) {
				processAdjacentTile(_hdScreenInfo->ScreenTiles[(i + y) * 256 + j + x]);
			}
		}
	}
	return hasNonBackgroundSurrounding;
}

void HdNesPack::GetPixels(uint32_t x, uint32_t y, HdPpuPixelInfo &pixelInfo, uint32_t *outputBuffer, uint32_t screenWidth)
{
	HdPackTileInfo *hdPackTileInfo = nullptr;
	HdPackTileInfo *hdPackSpriteInfo = nullptr;
	HdPackData *hdData = Console::GetHdData();

	bool hasSprite = pixelInfo.SpriteCount > 0;
	if(pixelInfo.Tile.TileIndex != HdPpuTileInfo::NoTile) {
		hdPackTileInfo = GetCachedMatchingTile(x, y, &pixelInfo.Tile);
	}

	bool hasBgSprite = false;
	int lowestBgSprite = 999;
	
	DrawColor(_palette[pixelInfo.Tile.PpuBackgroundColor], outputBuffer, hdData->Scale, screenWidth);

	if(hasSprite) {
		for(int k = pixelInfo.SpriteCount - 1; k >= 0; k--) {
			if(pixelInfo.Sprite[k].BackgroundPriority) {
				hasBgSprite = true;
				lowestBgSprite = k;

				hdPackSpriteInfo = GetMatchingTile(x, y, &pixelInfo.Sprite[k]);
				if(hdPackSpriteInfo) {
					DrawTile(pixelInfo.Sprite[k], *hdPackSpriteInfo, outputBuffer, screenWidth);
				} else if(pixelInfo.Sprite[k].SpriteColorIndex != 0) {
					DrawColor(_palette[pixelInfo.Sprite[k].SpriteColor], outputBuffer, hdData->Scale, screenWidth);
				}
			}
		}
	}
	
	bool hasCustomBackground = false;
	bool hasNonBackgroundSurrounding = false;
	if(_backgroundIndex >= 0) {
		HdBackgroundInfo &bgInfo = hdData->Backgrounds[_backgroundIndex];

		//Enable custom background if the current pixel fits within the background's boundaries
		hasCustomBackground =
			(int32_t)x >= -_bgScrollX &&
			(int32_t)y >= -_bgScrollY &&
			(y + _bgScrollY + 1) * hdData->Scale <= bgInfo.Data->Height &&
			(x + _bgScrollX + 1) * hdData->Scale <= bgInfo.Data->Width;

		if(hasCustomBackground) {
			hasNonBackgroundSurrounding = _contoursEnabled && IsNextToSprite(x, y);
			DrawCustomBackground(outputBuffer, x + _bgScrollX, y + _bgScrollY, hdData->Scale, screenWidth);
		}
	}

	if(hdPackTileInfo) {
		DrawTile(pixelInfo.Tile, *hdPackTileInfo, outputBuffer, screenWidth);
	} else {
		//Draw regular SD background tile
		bool useCustomBackground = !hasNonBackgroundSurrounding && hasCustomBackground && pixelInfo.Tile.BgColorIndex == 0;
		if(!useCustomBackground && (pixelInfo.Tile.BgColorIndex != 0 || hasNonBackgroundSurrounding)) {
			DrawColor(_palette[pixelInfo.Tile.BgColor], outputBuffer, hdData->Scale, screenWidth);
		}
	}

	if(hasSprite) {
		for(int k = pixelInfo.SpriteCount - 1; k >= 0; k--) {
			if(!pixelInfo.Sprite[k].BackgroundPriority && lowestBgSprite > k) {
				hdPackSpriteInfo = GetMatchingTile(x, y, &pixelInfo.Sprite[k]);
				if(hdPackSpriteInfo) {
					DrawTile(pixelInfo.Sprite[k], *hdPackSpriteInfo, outputBuffer, screenWidth);
				} else if(pixelInfo.Sprite[k].SpriteColorIndex != 0) {
					DrawColor(_palette[pixelInfo.Sprite[k].SpriteColor], outputBuffer, hdData->Scale, screenWidth);
				}
			}
		}
	}
}

void HdNesPack::Process(HdScreenInfo *hdScreenInfo, uint32_t* outputBuffer, OverscanDimensions &overscan)
{
	_hdScreenInfo = hdScreenInfo;
	uint32_t hdScale = GetScale();
	uint32_t screenWidth = overscan.GetScreenWidth() * hdScale;

	OnBeforeApplyFilter();
	for(uint32_t i = overscan.Top, iMax = 240 - overscan.Bottom; i < iMax; i++) {
		OnLineStart(hdScreenInfo->ScreenTiles[i << 8]);
		uint32_t bufferIndex = (i - overscan.Top) * screenWidth * hdScale;
		for(uint32_t j = overscan.Left, jMax = 256 - overscan.Right; j < jMax; j++) {
			GetPixels(j, i, hdScreenInfo->ScreenTiles[i * 256 + j], outputBuffer + bufferIndex, screenWidth);
			bufferIndex += hdScale;
		}
	}
}