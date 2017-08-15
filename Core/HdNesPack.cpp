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
	if(scale == 1) {
		*outputBuffer = GetCustomBackgroundPixel(x, y, 0, 0);
	} else {
		for(uint32_t i = 0; i < scale; i++) {
			for(uint32_t j = 0; j < scale; j++) {
				*outputBuffer = GetCustomBackgroundPixel(x, y, j, i);
				outputBuffer++;
			}
			outputBuffer += screenWidth - scale;
		}
	}
}

void HdNesPack::DrawTile(HdPpuTileInfo &tileInfo, HdPackTileInfo &hdPackTileInfo, uint32_t *outputBuffer, uint32_t screenWidth)
{
	if(hdPackTileInfo.IsFullyTransparent) {
		return;
	}

	uint32_t bgColor = _palette[tileInfo.PpuBackgroundColor];
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

void HdNesPack::OnBeforeApplyFilter(HdPpuPixelInfo *screenTiles)
{
	HdPackData* hdData = Console::GetHdData();

	_palette = hdData->Palette.size() == 0x40 ? hdData->Palette.data() : EmulationSettings::GetRgbPalette();

	if(hdData->OptionFlags & (int)HdPackOptions::NoSpriteLimit) {
		EmulationSettings::SetFlags(EmulationFlags::RemoveSpriteLimit | EmulationFlags::AdaptiveSpriteLimit);
	}

	_backgroundIndex = -1;
	for(size_t i = 0; i < hdData->Backgrounds.size(); i++) {
		bool isMatch = true;
		for(HdPackCondition* condition : hdData->Backgrounds[i].Conditions) {
			if(!condition->CheckCondition(screenTiles, 0, 0)) {
				isMatch = false;
				break;
			}
		}

		if(isMatch) {
			_backgroundIndex = (int32_t)i;
			break;
		}
	}
}

HdPackTileInfo * HdNesPack::GetMatchingTile(HdPpuPixelInfo *screenTiles, uint32_t x, uint32_t y, HdTileKey &key)
{
	HdPackData *hdData = Console::GetHdData();
	auto hdTile = hdData->TileByKey.find(key);
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
		if(y + i < 0 || y + i >= PPU::ScreenHeight) {
			continue;
		}

		for(int j = -1; j <= 1; j++) {
			if(x + j < 0 || x + j >= PPU::ScreenWidth) {
				continue;
			}

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
	uint8_t brightness = hdData->Backgrounds[_backgroundIndex].Brightness;
	uint32_t rgbColor = *(hdData->Backgrounds[_backgroundIndex].data() + (y * hdData->Scale + offsetY) * 256 * hdData->Scale + x * hdData->Scale + offsetX);
	if(brightness < 255) {
		return AdjustBrightness((uint8_t*)&rgbColor, brightness);
	} else {
		return rgbColor;
	}	
}

void HdNesPack::GetPixels(HdPpuPixelInfo *screenTiles, uint32_t x, uint32_t y, HdPpuPixelInfo &pixelInfo, uint32_t *outputBuffer, uint32_t screenWidth)
{
	HdPackTileInfo *hdPackTileInfo = nullptr;
	HdPackTileInfo *hdPackSpriteInfo = nullptr;
	HdPackData *hdData = Console::GetHdData();

	bool hasSprite = pixelInfo.SpriteCount > 0;
	if(pixelInfo.Tile.TileIndex != HdPpuTileInfo::NoTile) {
		hdPackTileInfo = GetMatchingTile(screenTiles, x, y, pixelInfo.Tile);
	}

	bool hasBgSprite = false;
	int lowestBgSprite = 999;
	
	DrawColor(_palette[pixelInfo.Tile.PpuBackgroundColor], outputBuffer, hdData->Scale, screenWidth);

	if(hasSprite) {
		for(int k = pixelInfo.SpriteCount - 1; k >= 0; k--) {
			if(pixelInfo.Sprite[k].BackgroundPriority) {
				hasBgSprite = true;
				lowestBgSprite = k;

				hdPackSpriteInfo = GetMatchingTile(screenTiles, x, y, pixelInfo.Sprite[k]);
				if(hdPackSpriteInfo) {
					DrawTile(pixelInfo.Sprite[k], *hdPackSpriteInfo, outputBuffer, screenWidth);
				} else if(pixelInfo.Sprite[k].SpriteColorIndex != 0) {
					DrawColor(_palette[pixelInfo.Sprite[k].SpriteColor], outputBuffer, hdData->Scale, screenWidth);
				}
			}
		}
	}
	
	bool hasCustomBackground = _backgroundIndex >= 0 && y < hdData->Backgrounds[_backgroundIndex].Data->Height;
	bool hasNonBackgroundSurrounding = hasCustomBackground ? IsNextToSprite(screenTiles, x, y) : false;
	if(hasCustomBackground) {
		DrawCustomBackground(outputBuffer, x, y, hdData->Scale, screenWidth);
	}

	if(hdPackTileInfo) {
		DrawTile(pixelInfo.Tile, *hdPackTileInfo, outputBuffer, screenWidth);
	} else {
		//Draw regular SD background tile
		bool useCustomBackground = !hasNonBackgroundSurrounding && hasCustomBackground && pixelInfo.Tile.BgColorIndex == 0;
		if(useCustomBackground) {
			DrawCustomBackground(outputBuffer, x, y, hdData->Scale, screenWidth);
		} else if(pixelInfo.Tile.BgColorIndex != 0 || hasNonBackgroundSurrounding) {
			DrawColor(_palette[pixelInfo.Tile.BgColor], outputBuffer, hdData->Scale, screenWidth);
		}
	}

	if(hasSprite) {
		for(int k = pixelInfo.SpriteCount - 1; k >= 0; k--) {
			if(!pixelInfo.Sprite[k].BackgroundPriority && lowestBgSprite > k) {
				hdPackSpriteInfo = GetMatchingTile(screenTiles, x, y, pixelInfo.Sprite[k]);
				if(hdPackSpriteInfo) {
					DrawTile(pixelInfo.Sprite[k], *hdPackSpriteInfo, outputBuffer, screenWidth);
				} else if(pixelInfo.Sprite[k].SpriteColorIndex != 0) {
					DrawColor(_palette[pixelInfo.Sprite[k].SpriteColor], outputBuffer, hdData->Scale, screenWidth);
				}
			}
		}
	}
}
