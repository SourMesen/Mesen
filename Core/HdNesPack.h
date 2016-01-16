#pragma once

#include <unordered_map>
#include "Console.h"
#include "MessageManager.h"
#include "EmulationSettings.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/PNGHelper.h"

extern const uint32_t PPU_PALETTE_ARGB[];

struct HdPpuTileInfo
{
	static const uint32_t NoTile = -1;
	uint32_t TileIndex;
	uint32_t PaletteColors;
	uint8_t OffsetX;
	uint8_t OffsetY;
	bool HorizontalMirroring;
	bool VerticalMirroring;
	bool BackgroundPriority;
	uint8_t BgColorIndex;
	uint8_t BgColor;

	uint64_t GetKey(bool defaultKey)
	{
		if(defaultKey) {
			return (uint64_t)TileIndex | 0xFFFFFFFF00000000;
		} else {
			return (uint64_t)TileIndex | ((uint64_t)PaletteColors << 32);
		}
	}
};

struct HdPpuPixelInfo
{
	HdPpuTileInfo Tile;
	HdPpuTileInfo Sprite;
};

struct HdPackTileInfo
{
	uint32_t TileIndex;
	uint32_t BitmapIndex;
	uint32_t PaletteColors;
	uint32_t X;
	uint32_t Y;
	bool DefaultTile;

	uint64_t GetKey(bool defaultKey)
	{
		if(defaultKey) {
			return (uint64_t)TileIndex | 0xFFFFFFFF00000000;
		} else {
			return (uint64_t)TileIndex | ((uint64_t)PaletteColors << 32);
		}
	}
};

struct HdPackBitmapInfo
{
	vector<uint8_t> PixelData;
	uint32_t Width;
	uint32_t Height;
};

class HdNesPack : public INotificationListener
{
private:
	vector<HdPackBitmapInfo> _hdNesBitmaps;
	vector<HdPackTileInfo> _hdNesTiles;
	std::unordered_map<uint64_t, HdPackTileInfo*> _tileInfoByKey;
	SimpleLock _loadLock;
	uint32_t _hdScale;

	void LoadHdNesPack()
	{
		_loadLock.Acquire();

		_hdNesBitmaps.clear();
		_hdNesTiles.clear();
		_tileInfoByKey.clear();

		string hdPackFolder = FolderUtilities::CombinePath(FolderUtilities::GetHdPackFolder(), FolderUtilities::GetFilename(Console::GetROMPath(), false));
		string hdPackDefinitionFile = FolderUtilities::CombinePath(hdPackFolder, "hires.txt");
		ifstream packDefinition(hdPackDefinitionFile, ios::in | ios::binary);
		while(packDefinition.good()) {
			string lineContent;
			std::getline(packDefinition, lineContent);
			lineContent = lineContent.substr(0, lineContent.length() - 1);

			if(lineContent.substr(0, 7) == "<scale>") {
				lineContent = lineContent.substr(7);
				_hdScale = std::stoi(lineContent);
			} else if(lineContent.substr(0, 5) == "<img>") {
				lineContent = lineContent.substr(5);
				HdPackBitmapInfo bitmapInfo;
				string imageFile = FolderUtilities::CombinePath(hdPackFolder, lineContent);
				PNGHelper::ReadPNG(imageFile, bitmapInfo.PixelData, bitmapInfo.Width, bitmapInfo.Height);
				_hdNesBitmaps.push_back(bitmapInfo);
			} else if(lineContent.substr(0, 6) == "<tile>") {
				lineContent = lineContent.substr(6);
				vector<string> tokens = split(lineContent, ',');
				HdPackTileInfo tileInfo;
				tileInfo.TileIndex = std::stoi(tokens[0]);
				tileInfo.BitmapIndex = std::stoi(tokens[1]);
				tileInfo.PaletteColors = std::stoi(tokens[2]) | (std::stoi(tokens[3]) << 8) | (std::stoi(tokens[4]) << 16);
				tileInfo.X = std::stoi(tokens[5]);
				tileInfo.Y = std::stoi(tokens[6]);
				tileInfo.DefaultTile = (tokens[7] == "Y");
				_hdNesTiles.push_back(tileInfo);
			}
		}

		for(HdPackTileInfo &tileInfo : _hdNesTiles) {
			_tileInfoByKey[tileInfo.GetKey(false)] = &tileInfo;
			if(tileInfo.DefaultTile) {
				_tileInfoByKey[tileInfo.GetKey(true)] = &tileInfo;
			}
		}

		packDefinition.close();

		_loadLock.Release();
	}

	vector<string> split(const string &s, char delim)
	{
		vector<string> tokens;
		std::stringstream ss(s);
		std::string item;
		while(std::getline(ss, item, delim)) {
			tokens.push_back(item);
		}
		return tokens;
	}

public:
	HdNesPack()
	{
		_hdScale = 2;

		LoadHdNesPack();
		MessageManager::RegisterNotificationListener(this);
	}

	~HdNesPack()
	{
		MessageManager::UnregisterNotificationListener(this);
	}

	uint32_t GetScale()
	{
		return _hdScale;
	}

	void BlendColors(uint8_t output[4], uint8_t input[4])
	{
		uint8_t alpha = input[3] + 1;
		uint8_t invertedAlpha = 256 - input[3];
		output[0] = (uint8_t)((alpha * input[0] + invertedAlpha * output[0]) >> 8);
		output[1] = (uint8_t)((alpha * input[1] + invertedAlpha * output[1]) >> 8);
		output[2] = (uint8_t)((alpha * input[2] + invertedAlpha * output[2]) >> 8);
		output[3] = 0xFF;
	}

	void DrawTile(HdPpuTileInfo &tileInfo, HdPackTileInfo &hdPackTileInfo, uint32_t* outputBuffer, uint32_t screenWidth, bool drawBackground)
	{
		HdPackBitmapInfo &bitmapInfo = _hdNesBitmaps[hdPackTileInfo.BitmapIndex];
		uint32_t* bitmapData = (uint32_t*)&bitmapInfo.PixelData[0];
		uint8_t tileOffsetX = tileInfo.HorizontalMirroring ? 7 - tileInfo.OffsetX : tileInfo.OffsetX;
		uint32_t bitmapOffset = (hdPackTileInfo.Y + tileInfo.OffsetY * _hdScale) * bitmapInfo.Width + hdPackTileInfo.X + tileOffsetX * _hdScale;
		int32_t bitmapSmallInc = 1;
		int32_t bitmapLargeInc = bitmapInfo.Width - _hdScale;
		if(tileInfo.HorizontalMirroring) {
			bitmapOffset += _hdScale - 1;
			bitmapSmallInc = -1;
			bitmapLargeInc = bitmapInfo.Width + _hdScale;
		}
		if(tileInfo.VerticalMirroring) {
			bitmapOffset += bitmapInfo.Width * (_hdScale - 1);
			bitmapLargeInc = tileInfo.HorizontalMirroring ? -(int32_t)bitmapInfo.Width + (int32_t)_hdScale : -(int32_t)bitmapInfo.Width - (int32_t)_hdScale;
		}
		for(uint32_t y = 0; y < _hdScale; y++) {
			for(uint32_t x = 0; x < _hdScale; x++) {
				if(drawBackground) {
					*outputBuffer = PPU_PALETTE_ARGB[tileInfo.BgColor];
				}
				if(!tileInfo.BackgroundPriority || tileInfo.BgColorIndex == 0) {
					if((bitmapData[bitmapOffset] & 0xFF000000) == 0xFF000000) {
						*outputBuffer = bitmapData[bitmapOffset];
					} else if((bitmapData[bitmapOffset] & 0xFF000000) != 0) {
						BlendColors((uint8_t*)outputBuffer, (uint8_t*)&bitmapData[bitmapOffset]);
					}
				}
				outputBuffer++;
				bitmapOffset += bitmapSmallInc;
			}
			bitmapOffset += bitmapLargeInc;
			outputBuffer += screenWidth - _hdScale;
		}
	}

	void GetPixels(HdPpuPixelInfo &pixelInfo, uint32_t sdPixel, uint32_t *outputBuffer, uint32_t screenWidth)
	{
		_loadLock.Acquire();

		HdPackTileInfo *hdPackTileInfo = nullptr;
		HdPackTileInfo *hdPackSpriteInfo = nullptr;		
		auto hdTile = _tileInfoByKey.find(pixelInfo.Tile.GetKey(false));
		if(hdTile != _tileInfoByKey.end()) {
			hdPackTileInfo = hdTile->second;
		} else {
			hdTile = _tileInfoByKey.find(pixelInfo.Tile.GetKey(true));
			if(hdTile != _tileInfoByKey.end()) {
				hdPackTileInfo = hdTile->second;
			}
		}

		if(pixelInfo.Sprite.TileIndex != HdPpuTileInfo::NoTile) {
			auto hdTile = _tileInfoByKey.find(pixelInfo.Sprite.GetKey(false));
			if(hdTile != _tileInfoByKey.end()) {
				hdPackSpriteInfo = hdTile->second;
			} else {
				hdTile = _tileInfoByKey.find(pixelInfo.Sprite.GetKey(true));
				if(hdTile != _tileInfoByKey.end()) {
					hdPackSpriteInfo = hdTile->second;
				}
			}
		}

		if(hdPackSpriteInfo && pixelInfo.Sprite.BackgroundPriority) {
			DrawTile(pixelInfo.Sprite, *hdPackSpriteInfo, outputBuffer, screenWidth, !hdPackTileInfo);
		}

		if(!hdPackTileInfo && !hdPackSpriteInfo) {
			//Write the standard SD tile if no HD tile is present
			uint32_t *buffer = outputBuffer;
			for(uint32_t y = 0; y < _hdScale; y++) {
				for(uint32_t x = 0; x < _hdScale; x++) {
					*buffer = sdPixel;
					buffer++;
				}
				buffer += screenWidth - _hdScale;
			}
		} 
		
		if(hdPackTileInfo) {
			DrawTile(pixelInfo.Tile, *hdPackTileInfo, outputBuffer, screenWidth, true);
		}
		
		if(hdPackSpriteInfo && !pixelInfo.Sprite.BackgroundPriority) {
			DrawTile(pixelInfo.Sprite, *hdPackSpriteInfo, outputBuffer, screenWidth, !hdPackTileInfo);
		}

		_loadLock.Release();
	}

	void ProcessNotification(ConsoleNotificationType type, void* parameter)
	{
		if(type == ConsoleNotificationType::GameLoaded) {
			LoadHdNesPack();
		}
	}

	static bool HasHdPack(string romFilepath)
	{
		string hdPackFolder = FolderUtilities::CombinePath(FolderUtilities::GetHdPackFolder(), FolderUtilities::GetFilename(romFilepath, false));
		string hdPackDefinitionFile = FolderUtilities::CombinePath(hdPackFolder, "hires.txt");

		if(ifstream(hdPackDefinitionFile)) {
			return EmulationSettings::CheckFlag(EmulationFlags::UseHdPacks);
		} else {
			return false;
		}
	}
};
