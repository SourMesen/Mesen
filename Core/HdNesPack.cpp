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

void HdNesPack::DrawTile(HdPpuTileInfo &tileInfo, HdPackTileInfo &hdPackTileInfo, uint32_t *outputBuffer, uint32_t screenWidth, bool drawBackground)
{
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
	for(uint32_t y = 0; y < scale; y++) {
		for(uint32_t x = 0; x < scale; x++) {
			if(drawBackground) {
				*outputBuffer = EmulationSettings::GetRgbPalette()[tileInfo.BgColor];
			}
			if(!tileInfo.BackgroundPriority || tileInfo.BgColorIndex == 0) {
				uint32_t rgbValue = AdjustBrightness((uint8_t*)(bitmapData + bitmapOffset), hdPackTileInfo.Brightness);
				if((bitmapData[bitmapOffset] & 0xFF000000) == 0xFF000000) {
					*outputBuffer = rgbValue;
				} else if((bitmapData[bitmapOffset] & 0xFF000000) != 0) {
					BlendColors((uint8_t*)outputBuffer, (uint8_t*)&rgbValue);
				}
			}
			outputBuffer++;
			bitmapOffset += bitmapSmallInc;
		}
		bitmapOffset += bitmapLargeInc;
		outputBuffer += screenWidth - scale;
	}
}

uint32_t HdNesPack::GetScale()
{
	return Console::GetHdData()->Scale;
}

void HdNesPack::OnBeforeApplyFilter(HdPpuPixelInfo *screenTiles)
{
	HdPackData* hdData = Console::GetHdData();

	if(hdData->OptionFlags & (int)HdPackOptions::NoSpriteLimit) {
		EmulationSettings::SetFlags(EmulationFlags::RemoveSpriteLimit | EmulationFlags::AdaptiveSpriteLimit);
	}

	if(hdData->Palette.size() == 0x40) {
		EmulationSettings::SetRgbPalette(hdData->Palette.data());
	}

	_backgroundIndex = -1;
	for(int i = 0; i < hdData->Backgrounds.size(); i++) {
		bool isMatch = true;
		for(HdPackCondition* condition : hdData->Backgrounds[i].Conditions) {
			if(!condition->CheckCondition(screenTiles, 0, 0)) {
				isMatch = false;
				break;
			}
		}

		if(isMatch) {
			_backgroundIndex = i;
			break;
		}
	}
}

HdPackTileInfo * HdNesPack::GetMatchingTile(HdPpuPixelInfo *screenTiles, uint32_t x, uint32_t y, HdTileKey &key)
{
	HdPackData *hdData = Console::GetHdData();
	std::unordered_map<HdTileKey, vector<HdPackTileInfo*>>::const_iterator hdTile;
	hdTile = hdData->TileByKey.find(key);
	if(hdTile == hdData->TileByKey.end()) {
		hdTile = hdData->TileByKey.find(key.GetKey(true));
	}

	if(hdTile != hdData->TileByKey.end()) {
		for(HdPackTileInfo* tile : hdTile->second) {
			if(tile->MatchesCondition(screenTiles, x, y)) {
				return tile;
			}
		}
	}

	return nullptr;
}

bool HdNesPack::IsNextToSprite(HdPpuPixelInfo *screenTiles, uint32_t x, uint32_t y)
{
	bool hasNonBackgroundSurrounding = false;
	auto processAdjacentTile = [&hasNonBackgroundSurrounding](HdPpuPixelInfo& pixelInfo) {
		if(pixelInfo.Sprite.TileIndex == HdPpuTileInfo::NoTile || pixelInfo.Sprite.SpriteColorIndex == 0 || pixelInfo.Sprite.SpriteColor != pixelInfo.Sprite.BgColor) {
			hasNonBackgroundSurrounding |= pixelInfo.Sprite.TileIndex != HdPpuTileInfo::NoTile && pixelInfo.Sprite.SpriteColorIndex != 0 || pixelInfo.Tile.BgColorIndex != 0;
		}
	};
	for(int i = -1; i <= 1; i++) {
		for(int j = -1; j <= 1; j++) {
			if(!hasNonBackgroundSurrounding) {
				processAdjacentTile(screenTiles[(i + y) * 256 + j + x]);
			}
		}
	}
	return hasNonBackgroundSurrounding;
}

uint32_t HdNesPack::GetCustomBackgroundPixel(int x, int y, int offsetX, int offsetY)
{
	HdPackData *hdData = Console::GetHdData();
	return AdjustBrightness((uint8_t*)(hdData->Backgrounds[_backgroundIndex].data() + (y * hdData->Scale + offsetY) * 256 * hdData->Scale + x * hdData->Scale + offsetX), hdData->Backgrounds[_backgroundIndex].Brightness);
}

void HdNesPack::GetPixels(HdPpuPixelInfo *screenTiles, uint32_t x, uint32_t y, HdPpuPixelInfo &pixelInfo, uint32_t sdPixel, uint32_t *outputBuffer, uint32_t screenWidth)
{
	HdPackTileInfo *hdPackTileInfo = nullptr;
	HdPackTileInfo *hdPackSpriteInfo = nullptr;
	HdPackData *hdData = Console::GetHdData();

	if(hdData->Version <= 2) {
		pixelInfo.Sprite.PaletteColors &= 0xFFFFFF;
		pixelInfo.Tile.PaletteColors &= 0xFFFFFF;
	}

	bool hasTile = pixelInfo.Tile.TileIndex != HdPpuTileInfo::NoTile;
	bool hasSprite = pixelInfo.Sprite.TileIndex != HdPpuTileInfo::NoTile;

	std::unordered_map<HdTileKey, HdPackTileInfo*>::const_iterator hdTile;
	if(hasTile) {
		hdPackTileInfo = GetMatchingTile(screenTiles, x, y, pixelInfo.Tile);
	}

	if(hasSprite) {
		hdPackSpriteInfo = GetMatchingTile(screenTiles, x, y, pixelInfo.Sprite);
	}

	bool needPixel = true;
	if(hdPackSpriteInfo && pixelInfo.Sprite.BackgroundPriority && pixelInfo.Tile.BgColorIndex == 0) {
		DrawTile(pixelInfo.Sprite, *hdPackSpriteInfo, outputBuffer, screenWidth, !hdPackTileInfo);
		needPixel = false;
	}

	bool hasCustomBackground = _backgroundIndex >= 0 && y < hdData->Backgrounds[_backgroundIndex].Data->Height;
	bool hasNonBackgroundSurrounding = hasCustomBackground ? IsNextToSprite(screenTiles, x, y) : false;
	if(hasCustomBackground) {
		uint32_t *buffer = outputBuffer;
		for(uint32_t i = 0; i < hdData->Scale; i++) {
			for(uint32_t j = 0; j < hdData->Scale; j++) {
				*buffer = GetCustomBackgroundPixel(x, y, j, i);
				buffer++;
			}
			buffer += screenWidth - hdData->Scale;
		}
	}

	if(hdPackTileInfo) {
		DrawTile(pixelInfo.Tile, *hdPackTileInfo, outputBuffer, screenWidth, true);
		needPixel = false;
	}

	if(needPixel || (!hdPackSpriteInfo && hasSprite && pixelInfo.Sprite.SpriteColorIndex != 0 && (!pixelInfo.Sprite.BackgroundPriority || pixelInfo.Tile.BgColorIndex == 0))) {
		//Write the standard SD tile if no HD tile is present
		uint32_t *buffer = outputBuffer;

		if(hasSprite && hdPackSpriteInfo) {
			sdPixel = EmulationSettings::GetRgbPalette()[pixelInfo.Sprite.BgColor];
		}

		bool useCustomBackground = !hasNonBackgroundSurrounding && hasCustomBackground && (!hasSprite || pixelInfo.Sprite.SpriteColorIndex == 0 || pixelInfo.Sprite.SpriteColor == pixelInfo.Sprite.BgColor) && pixelInfo.Tile.BgColorIndex == 0;

		for(uint32_t i = 0; i < hdData->Scale; i++) {
			for(uint32_t j = 0; j < hdData->Scale; j++) {
				if(useCustomBackground) {
					sdPixel = GetCustomBackgroundPixel(x, y, j, i);
				}
				*buffer = sdPixel;
				buffer++;
			}
			buffer += screenWidth - hdData->Scale;
		}
	}

	if(hdPackSpriteInfo && (!pixelInfo.Sprite.BackgroundPriority || pixelInfo.Tile.BgColorIndex == 0)) {
		DrawTile(pixelInfo.Sprite, *hdPackSpriteInfo, outputBuffer, screenWidth, false);
	}
}
