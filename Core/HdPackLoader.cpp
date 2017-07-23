#include "stdafx.h"
#include <algorithm>
#include <unordered_map>
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/StringUtilities.h"
#include "../Utilities/HexUtilities.h"
#include "../Utilities/PNGHelper.h"
#include "Console.h"
#include "HdPackLoader.h"

HdPackLoader::HdPackLoader(string hdPackDefinitionFile, HdPackData *data)
{
	_hdPackDefinitionFile = hdPackDefinitionFile;
	_hdPackFolder = FolderUtilities::GetFolderName(_hdPackDefinitionFile);
	_data = data;
}

bool HdPackLoader::LoadHdNesPack(string hdPackDefinitionFile, HdPackData &outData)
{
	//outData = HdPackData();

	HdPackLoader loader(hdPackDefinitionFile, &outData);
	return loader.LoadPack();
}

bool HdPackLoader::LoadPack()
{
	try {
		ifstream packDefinition(_hdPackDefinitionFile, ios::in | ios::binary);
		if(!packDefinition.good()) {
			return false;
		}

		while(packDefinition.good()) {
			string lineContent;
			std::getline(packDefinition, lineContent);
			lineContent = lineContent.substr(0, lineContent.length() - 1);

			vector<HdPackCondition*> conditions;
			if(lineContent.substr(0, 1) == "[") {
				size_t endOfCondition = lineContent.find_first_of(']', 1);
				conditions = ParseConditionString(lineContent.substr(1, endOfCondition - 1), _data->Conditions);
				lineContent = lineContent.substr(endOfCondition + 1);
			}

			vector<string> tokens;
			if(lineContent.substr(0, 5) == "<ver>") {
				_data->Version = stoi(lineContent.substr(5));
			} else if(lineContent.substr(0, 7) == "<scale>") {
				lineContent = lineContent.substr(7);
				_data->Scale = std::stoi(lineContent);
			} else if(lineContent.substr(0, 5) == "<img>") {
				lineContent = lineContent.substr(5);
				HdPackBitmapInfo bitmapInfo;
				string imageFile = FolderUtilities::CombinePath(_hdPackFolder, lineContent);
				PNGHelper::ReadPNG(imageFile, bitmapInfo.PixelData, bitmapInfo.Width, bitmapInfo.Height);
				_hdNesBitmaps.push_back(bitmapInfo);
			} else if(lineContent.substr(0, 7) == "<patch>") {
				tokens = StringUtilities::Split(lineContent.substr(7), ',');
				ProcessPatchTag(tokens);
			} else if(lineContent.substr(0, 12) == "<background>") {
				tokens = StringUtilities::Split(lineContent.substr(12), ',');
				ProcessBackgroundTag(tokens, conditions);
			} else if(lineContent.substr(0, 11) == "<condition>") {
				tokens = StringUtilities::Split(lineContent.substr(11), ',');
				ProcessConditionTag(tokens);
			} else if(lineContent.substr(0, 6) == "<tile>") {
				tokens = StringUtilities::Split(lineContent.substr(6), ',');
				ProcessTileTag(tokens, conditions);
			} else if(lineContent.substr(0, 9) == "<options>") {
				tokens = StringUtilities::Split(lineContent.substr(9), ',');
				ProcessOptionTag(tokens);
			}
		}

		LoadCustomPalette();
		InitializeHdPack();

		packDefinition.close();
		return true;
	} catch(std::exception ex) {
		MessageManager::Log(string("[HDPack] Error loading HDPack: ") + ex.what());
		return false;
	}
}

void HdPackLoader::ProcessPatchTag(vector<string> &tokens)
{
	if(tokens[1].size() != 40) {
		MessageManager::Log(string("[HDPack] Invalid SHA1 hash for patch (" + tokens[0] + "): " + tokens[1]));
		return;
	}
	if(!ifstream(FolderUtilities::CombinePath(_hdPackFolder, tokens[0]))) {
		MessageManager::Log(string("[HDPack] Patch file not found: " + tokens[1]));
		return;
	}

	std::transform(tokens[1].begin(), tokens[1].end(), tokens[1].begin(), ::toupper);
	_data->PatchesByHash[tokens[1]] = tokens[0];
}

void HdPackLoader::ProcessTileTag(vector<string> &tokens, vector<HdPackCondition*> conditions)
{
	HdPackTileInfo *tileInfo = new HdPackTileInfo();
	int index = 0;
	if(_data->Version < 100) {
		tileInfo->TileIndex = std::stoi(tokens[index++]);
		tileInfo->BitmapIndex = std::stoi(tokens[index++]);
		tileInfo->PaletteColors = std::stoi(tokens[index + 2]) | (std::stoi(tokens[index + 1]) << 8) | (std::stoi(tokens[index]) << 16);
		index += 3;
	} else {
		tileInfo->BitmapIndex = std::stoi(tokens[index++]);
		string tileData = tokens[index++];
		if(tileData.size() >= 32) {
			//CHR RAM tile, read the tile data
			for(int i = 0; i < 16; i++) {
				tileInfo->TileData[i] = HexUtilities::FromHex(tileData.substr(i * 2, 2));
			}
			tileInfo->IsChrRamTile = true;
		} else {
			tileInfo->TileIndex = std::stoi(tokens[index++]);
			tileInfo->IsChrRamTile = false;
		}
		tileInfo->PaletteColors = HexUtilities::FromHex(tokens[index++]);
	}
	tileInfo->X = std::stoi(tokens[index++]);
	tileInfo->Y = std::stoi(tokens[index++]);
	tileInfo->Conditions = conditions;

	if(_data->Version > 0) {
		tileInfo->Brightness = (uint8_t)(std::stof(tokens[index++]) * 255);
	} else {
		tileInfo->Brightness = 255;
	}
	tileInfo->DefaultTile = (tokens[index++] == "Y");

	//For CHR ROM tiles, the ID is just the bank number in chr rom (4k banks)
	tileInfo->ChrBankId = tileInfo->TileIndex / 256;

	if(_data->Version < 100) {
		if(tokens.size() >= 24) {
			//CHR RAM tile, read the tile data
			for(int i = 0; i < 16; i++) {
				tileInfo->TileData[i] = std::stoi(tokens[index++]);
			}
			tileInfo->IsChrRamTile = true;
		} else {
			tileInfo->IsChrRamTile = false;
		}
	} else {
		if(tileInfo->IsChrRamTile && tokens.size() > index) {
			tileInfo->ChrBankId = std::stoul(tokens[index++]);
		}
		if(tileInfo->IsChrRamTile && tokens.size() > index) {
			tileInfo->TileIndex = std::stoi(tokens[index++]);
		}
	}

	if(tileInfo->BitmapIndex > _hdNesBitmaps.size()) {
		MessageManager::Log("[HDPack] Invalid bitmap index: " + std::to_string(tileInfo->BitmapIndex));
		return;
	}

	HdPackBitmapInfo &bitmapInfo = _hdNesBitmaps[tileInfo->BitmapIndex];
	uint32_t bitmapOffset = tileInfo->Y * bitmapInfo.Width + tileInfo->X;
	uint32_t* pngData = (uint32_t*)bitmapInfo.PixelData.data();
	for(uint32_t y = 0; y < 8 * _data->Scale; y++) {
		for(uint32_t x = 0; x < 8 * _data->Scale; x++) {
			tileInfo->HdTileData.push_back(pngData[bitmapOffset]);
			bitmapOffset++;
		}
		bitmapOffset += bitmapInfo.Width - 8 * _data->Scale;
	}

	tileInfo->UpdateBlankTileFlag();

	_data->Tiles.push_back(unique_ptr<HdPackTileInfo>(tileInfo));
}

void HdPackLoader::ProcessOptionTag(vector<string> &tokens)
{
	for(string token : tokens) {
		if(token == "disableSpriteLimit") {
			_data->OptionFlags |= (int)HdPackOptions::NoSpriteLimit;
		}
	}
}

void HdPackLoader::ProcessConditionTag(vector<string> &tokens)
{
	HdPackCondition *condition = new HdPackCondition();
	tokens[0].erase(tokens[0].find_last_not_of(" \n\r\t") + 1);
	condition->Name = tokens[0];

	if(tokens[1] == "tileAtPosition") {
		condition->Type = HdPackConditionType::TileAtPosition;
	} else if(tokens[1] == "tileNearby") {
		condition->Type = HdPackConditionType::TileNearby;
	} else if(tokens[1] == "spriteAtPosition") {
		condition->Type = HdPackConditionType::SpriteAtPosition;
	} else if(tokens[1] == "spriteNearby") {
		condition->Type = HdPackConditionType::SpriteNearby;
	}

	int index = 2;
	switch(condition->Type) {
		case HdPackConditionType::TileAtPosition:
		case HdPackConditionType::SpriteAtPosition:
		case HdPackConditionType::TileNearby:
		case HdPackConditionType::SpriteNearby:
			condition->TileX = std::stoi(tokens[index++]);
			condition->TileY = std::stoi(tokens[index++]);
			break;
	}

	string tileData = tokens[index++];
	if(tileData.size() == 32) {
		//CHR RAM tile, read the tile data
		for(int i = 0; i < 16; i++) {
			condition->TileData[i] = HexUtilities::FromHex(tileData.substr(i * 2, 2));
		}
		condition->TileIndex = -1;
	} else {
		condition->TileIndex = std::stoi(tokens[index++]);
	}

	condition->PaletteColors = HexUtilities::FromHex(tokens[index++]);

	_data->Conditions.push_back(unique_ptr<HdPackCondition>(condition));
}

void HdPackLoader::ProcessBackgroundTag(vector<string> &tokens, vector<HdPackCondition*> conditions)
{
	HdBackgroundFileData* fileData = nullptr;
	for(unique_ptr<HdBackgroundFileData> &bgData : _data->BackgroundFileData) {
		if(bgData->PngName == tokens[0]) {
			fileData = bgData.get();
		}
	}

	if(!fileData) {
		vector<uint8_t> pixelData;
		uint32_t width, height;
		string imageFile = FolderUtilities::CombinePath(_hdPackFolder, tokens[0]);
		if(PNGHelper::ReadPNG(imageFile, pixelData, width, height)) {
			_data->BackgroundFileData.push_back(unique_ptr<HdBackgroundFileData>(new HdBackgroundFileData()));
			fileData = _data->BackgroundFileData.back().get();
			fileData->PixelData = pixelData;
			fileData->Width = width;
			fileData->Height = height;
			fileData->PngName = tokens[0];
		}
	}

	HdBackgroundInfo backgroundInfo;
	if(fileData) {
		backgroundInfo.Data = fileData;
		backgroundInfo.Brightness = (uint16_t)(std::stof(tokens[1]) * 255);
		backgroundInfo.Conditions = conditions;

		for(HdPackCondition* condition : backgroundInfo.Conditions) {
			if(condition->Type == HdPackConditionType::SpriteNearby || condition->Type == HdPackConditionType::TileNearby) {
				MessageManager::Log("[HDPack] Invalid condition type for background: " + tokens[0]);
			}
		}

		_data->Backgrounds.push_back(backgroundInfo);
	} else {
		MessageManager::Log("[HDPack] Error while loading background: " + tokens[0]);
	}
}

vector<HdPackCondition*> HdPackLoader::ParseConditionString(string conditionString, vector<unique_ptr<HdPackCondition>> &conditions)
{
	vector<string> conditionNames = StringUtilities::Split(conditionString, '&');

	vector<HdPackCondition*> result;
	for(string conditionName : conditionNames) {
		conditionName.erase(conditionName.find_last_not_of(" \n\r\t") + 1);

		bool found = false;
		for(unique_ptr<HdPackCondition> &condition : conditions) {
			if(conditionName == condition->Name) {
				result.push_back(condition.get());
				found = true;
				break;
			}
		}

		if(!found) {
			MessageManager::Log("[HDPack] Condition not found: " + conditionName);
		}
	}

	return result;
}

void HdPackLoader::LoadCustomPalette()
{
	string customPalettePath = FolderUtilities::CombinePath(_hdPackFolder, "palette.dat");
	ifstream file(customPalettePath, ios::binary);
	if(file.good()) {
		vector<uint32_t> paletteData;

		uint8_t rgb[3];
		while(!file.eof()) {
			file.read((char*)rgb, 3);
			if(!file.eof()) {
				paletteData.push_back(0xFF000000 | (rgb[0] << 16) | (rgb[1] << 8) | rgb[2]);
			}
		}

		if(paletteData.size() == 0x40) {
			_data->PaletteBackup = vector<uint32_t>(0x40, 0);
			EmulationSettings::GetRgbPalette(_data->PaletteBackup.data());
			_data->Palette = paletteData;
		}
	}
}

void HdPackLoader::InitializeHdPack()
{
	for(unique_ptr<HdPackTileInfo> &tileInfo : _data->Tiles) {
		auto tiles = _data->TileByKey.find(tileInfo->GetKey(false));
		if(tiles == _data->TileByKey.end()) {
			_data->TileByKey[tileInfo->GetKey(false)] = vector<HdPackTileInfo*>();
		}
		_data->TileByKey[tileInfo->GetKey(false)].push_back(tileInfo.get());

		if(tileInfo->DefaultTile) {
			auto tiles = _data->TileByKey.find(tileInfo->GetKey(true));
			if(tiles == _data->TileByKey.end()) {
				_data->TileByKey[tileInfo->GetKey(false)] = vector<HdPackTileInfo*>();
			}
			_data->TileByKey[tileInfo->GetKey(true)].push_back(tileInfo.get());
		}
	}
}
