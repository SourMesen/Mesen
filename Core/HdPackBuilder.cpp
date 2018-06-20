#include <algorithm>
#include "stdafx.h"
#include "VirtualFile.h"
#include "HdPackBuilder.h"
#include "HdNesPack.h"

HdPackBuilder* HdPackBuilder::_instance = nullptr;

enum HdPackRecordFlags
{
	None = 0,
	UseLargeSprites = 1,
	SortByUsageFrequency = 2,
	GroupBlankTiles = 4,
	IgnoreOverscan = 8,
};

HdPackBuilder::HdPackBuilder(string saveFolder, ScaleFilterType filterType, uint32_t scale, uint32_t flags, uint32_t chrRamBankSize, bool isChrRam)
{
	_saveFolder = saveFolder;
	_filterType = filterType;
	_chrRamBankSize = chrRamBankSize;
	_flags = flags;
	_isChrRam = isChrRam;

	string existingPackDefinition = FolderUtilities::CombinePath(saveFolder, "hires.txt");
	if(ifstream(existingPackDefinition)) {
		HdPackLoader::LoadHdNesPack(existingPackDefinition, _hdData);
		for(unique_ptr<HdPackTileInfo> &tile : _hdData.Tiles) {
			//Mark the tiles in the first PNGs as higher usage (preserves order when adding new tiles to an existing set)
			AddTile(tile.get(), 0xFFFFFFFF - tile->BitmapIndex);
		}

		if(_hdData.Scale != scale) {
			_filterType = ScaleFilterType::Prescale;
		}
	} else {
		_hdData.Scale = scale;
	}

	_romName = FolderUtilities::GetFilename(Console::GetMapperInfo().RomName, false);
	_instance = this;
}

HdPackBuilder::~HdPackBuilder()
{
	SaveHdPack();
	if(_instance == this) {
		_instance = nullptr;
	}
}

void HdPackBuilder::AddTile(HdPackTileInfo *tile, uint32_t usageCount)
{
	bool isTileBlank = (_flags & HdPackRecordFlags::GroupBlankTiles) ? tile->Blank : false;

	int chrBankId = isTileBlank ? 0xFFFFFFFF : tile->ChrBankId;
	int palette = isTileBlank ? _blankTilePalette : tile->PaletteColors;

	if(_tilesByChrBankByPalette.find(chrBankId) == _tilesByChrBankByPalette.end()) {
		_tilesByChrBankByPalette[chrBankId] = std::map<uint32_t, vector<HdPackTileInfo*>>();
	}

	std::map<uint32_t, vector<HdPackTileInfo*>> &paletteMap = _tilesByChrBankByPalette[chrBankId];
	if(paletteMap.find(palette) == paletteMap.end()) {
		paletteMap[palette] = vector<HdPackTileInfo*>(256, nullptr);
	}

	if(isTileBlank) {
		paletteMap[palette][_blankTileIndex] = tile;
		_blankTileIndex++;
		if(_blankTileIndex == _chrRamBankSize / 16) {
			_blankTileIndex = 0;
			_blankTilePalette++;
		}
	} else {
		paletteMap[palette][tile->TileIndex % 256] = tile;
	}

	_tilesByKey[tile->GetKey(false)] = tile;
	_tileUsageCount[tile->GetKey(false)] = usageCount;
}

void HdPackBuilder::ProcessTile(uint32_t x, uint32_t y, uint16_t tileAddr, HdPpuTileInfo &tile, BaseMapper *mapper, bool isSprite, uint32_t chrBankHash, bool transparencyRequired)
{
	if(_flags & HdPackRecordFlags::IgnoreOverscan) {
		OverscanDimensions overscan = EmulationSettings::GetOverscanDimensions();
		if(x < overscan.Left || y < overscan.Top || (PPU::ScreenWidth - x - 1) < overscan.Right || (PPU::ScreenHeight - y - 1) < overscan.Bottom) {
			//Ignore tiles inside overscan
			return;
		}
	}

	auto result = _tileUsageCount.find(tile.GetKey(false));
	if(result == _tileUsageCount.end()) {
		//Check to see if a default tile matches
		result = _tileUsageCount.find(tile.GetKey(true));
	}

	if(result == _tileUsageCount.end()) {
		//First time seeing this tile/palette combination, store it
		HdPackTileInfo* hdTile = new HdPackTileInfo();
		hdTile->PaletteColors = tile.PaletteColors;
		hdTile->TileIndex = tile.TileIndex;
		hdTile->DefaultTile = false;
		hdTile->IsChrRamTile = _isChrRam;
		hdTile->Brightness = 255;
		hdTile->ChrBankId = _isChrRam ? chrBankHash : (tileAddr / 16 / 256);
		hdTile->TransparencyRequired = transparencyRequired;

		memcpy(hdTile->TileData, tile.TileData, 16);

		_hdData.Tiles.push_back(unique_ptr<HdPackTileInfo>(hdTile));
		AddTile(hdTile, 1);
	} else {
		if(transparencyRequired) {
			auto existingTile = _tilesByKey.find(tile.GetKey(false));
			if(existingTile != _tilesByKey.end()) {
				existingTile->second->TransparencyRequired = true;
			}
		}
		
		if(result->second < 0x7FFFFFFF) {
			//Increase usage count
			result->second++;
		}
	}
}

void HdPackBuilder::GenerateHdTile(HdPackTileInfo *tile)
{
	uint32_t hdScale = _hdData.Scale;

	vector<uint32_t> originalTile = tile->ToRgb();
	vector<uint32_t> hdTile(8 * 8 * hdScale*hdScale, 0);

	switch(_filterType) {
		case ScaleFilterType::HQX: 
			hqx(hdScale, originalTile.data(), hdTile.data(), 8, 8);
			break;

		case ScaleFilterType::Prescale:
			hdTile.clear();
			for(uint8_t i = 0; i < 8 * hdScale; i++) {
				for(uint8_t j = 0; j < 8 * hdScale; j++) {
					hdTile.push_back(originalTile[i/hdScale*8+j/hdScale]);
				}
			}
			break;

		case ScaleFilterType::Scale2x:
			scale(hdScale, hdTile.data(), 8 * sizeof(uint32_t) * hdScale, originalTile.data(), 8 * sizeof(uint32_t), 4, 8, 8);
			break;

		case ScaleFilterType::_2xSai:
			twoxsai_generic_xrgb8888(8, 8, originalTile.data(), 8, hdTile.data(), 8 * hdScale);
			break;

		case ScaleFilterType::Super2xSai:
			supertwoxsai_generic_xrgb8888(8, 8, originalTile.data(), 8, hdTile.data(), 8 * hdScale);
			break;

		case ScaleFilterType::SuperEagle:
			supereagle_generic_xrgb8888(8, 8, originalTile.data(), 8, hdTile.data(), 8 * hdScale);
			break;

		case ScaleFilterType::xBRZ:
			xbrz::scale(hdScale, originalTile.data(), hdTile.data(), 8, 8, xbrz::ColorFormat::ARGB);
			break;
	}

	tile->HdTileData = hdTile;
}

void HdPackBuilder::DrawTile(HdPackTileInfo *tile, int tileNumber, uint32_t *pngBuffer, int pageNumber, bool containsSpritesOnly)
{
	if(tile->HdTileData.empty()) {
		GenerateHdTile(tile);
		tile->UpdateFlags();
	}

	if(containsSpritesOnly && (_flags & HdPackRecordFlags::UseLargeSprites)) {
		int row = tileNumber / 16;
		int column = tileNumber % 16;

		int newColumn = column / 2 + ((row & 1) ? 8 : 0);
		int newRow = (row & 0xFE) + ((column & 1) ? 1 : 0);

		tileNumber = newRow * 16 + newColumn;
	}

	tileNumber += pageNumber * (256 / (0x1000 / _chrRamBankSize));

	int tileDimension = 8 * _hdData.Scale;
	int x = tileNumber % 16 * tileDimension;
	int y = tileNumber / 16 * tileDimension;

	tile->X = x;
	tile->Y = y;

	int pngWidth = 128 * _hdData.Scale;
	int pngPos = y * pngWidth + x;
	int tilePos = 0;
	for(uint8_t i = 0; i < tileDimension; i++) {
		for(uint8_t j = 0; j < tileDimension; j++) {
			pngBuffer[pngPos] = tile->HdTileData[tilePos++];
			pngPos++;
		}
		pngPos += pngWidth - tileDimension;
	}
}

void HdPackBuilder::SaveHdPack()
{
	FolderUtilities::CreateFolder(_saveFolder);

	stringstream pngRows;
	stringstream tileRows;
	stringstream ss;
	int pngIndex = 0;
	ss << "<ver>" << std::to_string(HdNesPack::CurrentVersion) << std::endl;
	ss << "<scale>" << _hdData.Scale << std::endl;
	ss << "<supportedRom>" << Console::GetMapperInfo().Hash.Sha1Hash << std::endl;
	if(_flags & HdPackRecordFlags::IgnoreOverscan) {
		OverscanDimensions overscan = EmulationSettings::GetOverscanDimensions();
		ss << "<overscan>" << overscan.Top << "," << overscan.Right << "," << overscan.Bottom << "," << overscan.Left << std::endl;
	}

	int tileDimension = 8 * _hdData.Scale;
	int pngDimension = 16 * tileDimension;
	int pngBufferSize = pngDimension * pngDimension;
	uint32_t* pngBuffer = new uint32_t[pngBufferSize];

	int maxPageNumber = 0x1000 / _chrRamBankSize;
	int pageNumber = 0;
	bool pngEmpty = true;
	int pngNumber = 0;

	for(int i = 0; i < pngBufferSize; i++) {
		pngBuffer[i] = 0xFFFF00FF;
	}

	auto savePng = [&tileRows, &pngRows, &ss, &pngBuffer, &pngDimension, &pngIndex, &pngBufferSize, &pngEmpty, &pngNumber, this](uint32_t chrBankId) {
		if(!pngEmpty) {
			string pngName;
			if(_isChrRam) {
				pngName = "Chr_" + std::to_string(pngNumber) + ".png";
			} else {
				pngName = "Chr_" + HexUtilities::ToHex(chrBankId) + "_" + std::to_string(pngNumber) + ".png";
			}

			tileRows << std::endl << "#" << pngName << std::endl;
			tileRows << pngRows.str();
			pngRows = stringstream();

			ss << "<img>" << pngName << std::endl;
			PNGHelper::WritePNG(FolderUtilities::CombinePath(_saveFolder, pngName), pngBuffer, pngDimension, pngDimension, 32);
			pngNumber++;
			pngIndex++;

			for(int i = 0; i < pngBufferSize; i++) {
				pngBuffer[i] = 0xFFFF00FF;
			}
			pngEmpty = true;
		}
	};

	for(std::pair<const uint32_t, std::map<uint32_t, vector<HdPackTileInfo*>>> &kvp : _tilesByChrBankByPalette) {
		if(_flags & HdPackRecordFlags::SortByUsageFrequency) {
			for(int i = 0; i < 256; i++) {
				vector<std::pair<uint32_t, HdPackTileInfo*>> tiles;
				for(std::pair<const uint32_t, vector<HdPackTileInfo*>> &paletteMap : kvp.second) {
					if(paletteMap.second[i]) {
						tiles.push_back({ _tileUsageCount[paletteMap.second[i]->GetKey(false)], paletteMap.second[i] });
					}
				}
				std::sort(tiles.begin(), tiles.end(), [=](std::pair<uint32_t, HdPackTileInfo*> &a, std::pair<uint32_t, HdPackTileInfo*> &b) {
					return a.first > b.first;
				});

				size_t j = 0;
				for(std::pair<const uint32_t, vector<HdPackTileInfo*>> &paletteMap : kvp.second) {
					if(j < tiles.size()) {
						paletteMap.second[i] = tiles[j].second;
						j++;
					} else {
						paletteMap.second[i] = nullptr;
					}
				}
			}
		}

		if(!_isChrRam) {
			pngNumber = 0;
		}

		for(std::pair<const uint32_t, vector<HdPackTileInfo*>> &tileKvp : kvp.second) {
			bool pageEmpty = true;
			bool spritesOnly = true;
			for(HdPackTileInfo* tileInfo : tileKvp.second) {
				if(tileInfo && !tileInfo->IsSpriteTile()) {
					spritesOnly = false;
				}
			}

			for(int i = 0; i < 256; i++) {
				HdPackTileInfo* tileInfo = tileKvp.second[i];
				if(tileInfo) {
					DrawTile(tileInfo, i, pngBuffer, pageNumber, spritesOnly);

					pngRows << tileInfo->ToString(pngIndex) << std::endl;

					pageEmpty = false;
					pngEmpty = false;
				}
			}

			if(!pageEmpty) {
				pageNumber++;

				if(pageNumber == maxPageNumber) {
					savePng(kvp.first);
					pageNumber = 0;
				}
			}
		}
	}
	savePng(-1);

	for(unique_ptr<HdPackCondition> &condition : _hdData.Conditions) {
		if(!condition->IsExcludedFromFile()) {
			ss << condition->ToString() << std::endl;
		}
	}

	for(HdBackgroundInfo &bgInfo : _hdData.Backgrounds) {
		ss << bgInfo.ToString() << std::endl;
	}

	ss << tileRows.str();

	ofstream hiresFile(FolderUtilities::CombinePath(_saveFolder, "hires.txt"), ios::out);
	hiresFile << ss.str();
	hiresFile.close();

	delete[] pngBuffer;
}

void HdPackBuilder::GetChrBankList(uint32_t *banks)
{
	Console::Pause();
	for(std::pair<const uint32_t, std::map<uint32_t, vector<HdPackTileInfo*>>> &kvp : _instance->_tilesByChrBankByPalette) {
		*banks = kvp.first;
		banks++;
	}
	*banks = -1;
	Console::Resume();
}

void HdPackBuilder::GetBankPreview(uint32_t bankNumber, uint32_t pageNumber, uint32_t *rgbBuffer)
{
	Console::Pause();

	for(uint32_t i = 0; i < 128 * 128 * _instance->_hdData.Scale*_instance->_hdData.Scale; i++) {
		rgbBuffer[i] = 0xFF666666;
	}

	auto result = _instance->_tilesByChrBankByPalette.find(bankNumber);
	if(result != _instance->_tilesByChrBankByPalette.end()) {
		std::map<uint32_t, vector<HdPackTileInfo*>> bankData = result->second;

		if(_instance->_flags & HdPackRecordFlags::SortByUsageFrequency) {
			for(int i = 0; i < 256; i++) {
				vector<std::pair<uint32_t, HdPackTileInfo*>> tiles;
				for(std::pair<const uint32_t, vector<HdPackTileInfo*>> &pageData : bankData) {
					if(pageData.second[i]) {
						tiles.push_back({ _instance->_tileUsageCount[pageData.second[i]->GetKey(false)], pageData.second[i] });
					}
				}

				std::sort(tiles.begin(), tiles.end(), [=](std::pair<uint32_t, HdPackTileInfo*> &a, std::pair<uint32_t, HdPackTileInfo*> &b) {
					return a.first > b.first;
				});

				size_t j = 0;
				for(std::pair<const uint32_t, vector<HdPackTileInfo*>> &pageData : bankData) {
					if(j < tiles.size()) {
						pageData.second[i] = tiles[j].second;
						j++;
					} else {
						pageData.second[i] = nullptr;
					}
				}
			}
		}

		bool spritesOnly = true;
		for(HdPackTileInfo* tileInfo : (*bankData.begin()).second) {
			if(tileInfo && !tileInfo->IsSpriteTile()) {
				spritesOnly = false;
			}
		}

		for(int i = 0; i < 256; i++) {
			HdPackTileInfo* tileInfo = (*bankData.begin()).second[i];
			if(tileInfo) {
				_instance->DrawTile(tileInfo, i, (uint32_t*)rgbBuffer, 0, spritesOnly);
			}
		}
	}

	Console::Resume();
}
