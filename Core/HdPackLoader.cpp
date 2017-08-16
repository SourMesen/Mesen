#include "stdafx.h"
#include <algorithm>
#include <unordered_map>
#include "../Utilities/ZipReader.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/StringUtilities.h"
#include "../Utilities/HexUtilities.h"
#include "../Utilities/PNGHelper.h"
#include "Console.h"
#include "HdPackLoader.h"

HdPackLoader::HdPackLoader()
{
}

bool HdPackLoader::InitializeLoader(VirtualFile &romFile, HdPackData *data)
{
	_data = data;

	string romName = FolderUtilities::GetFilename(romFile.GetFileName(), false);
	string hdPackFolder = FolderUtilities::GetHdPackFolder();
	string zipName = romName + ".hdn";
	string definitionPath = FolderUtilities::CombinePath(romName, "hires.txt");

	string legacyPath = FolderUtilities::CombinePath(hdPackFolder, definitionPath);
	if(ifstream(legacyPath)) {
		_loadFromZip = false;
		_hdPackFolder = FolderUtilities::GetFolderName(legacyPath);
		return true;
	} else {
		vector<string> hdnPackages = FolderUtilities::GetFilesInFolder(romFile.GetFolderPath(), { ".hdn" }, false);
		vector<string> more = FolderUtilities::GetFilesInFolder(hdPackFolder, { ".hdn", ".zip" }, false);
		hdnPackages.insert(hdnPackages.end(), more.begin(), more.end());

		string sha1Hash = romFile.GetSha1Hash();
		for(string path : hdnPackages) {
			_reader.LoadArchive(path);

			vector<uint8_t> hdDefinition;
			if(_reader.ExtractFile("hires.txt", hdDefinition)) {
				if(FolderUtilities::GetFilename(path, false) == romName) {
					_loadFromZip = true;
					_hdPackFolder = path;
					return true;
				} else {
					for(string line : StringUtilities::Split(string(hdDefinition.data(), hdDefinition.data() + hdDefinition.size()), '\n')) {
						std::transform(line.begin(), line.end(), line.begin(), ::tolower);
						if(line.find("<supportedrom>") != string::npos && line.find(sha1Hash) != string::npos) {
							_loadFromZip = true;
							_hdPackFolder = path;
							return true;
						}
					}
				}
			}
		}
	}
	return false;
}

bool HdPackLoader::LoadHdNesPack(string definitionFile, HdPackData &outData)
{
	HdPackLoader loader;
	if(ifstream(definitionFile)) {
		loader._data = &outData;
		loader._loadFromZip = false;
		loader._hdPackFolder = FolderUtilities::GetFolderName(definitionFile);
		return loader.LoadPack();
	}
	return false;
}

bool HdPackLoader::LoadHdNesPack(VirtualFile &romFile, HdPackData &outData)
{
	HdPackLoader loader;
	if(loader.InitializeLoader(romFile, &outData)) {
		return loader.LoadPack();
	}
	return false;
}

bool HdPackLoader::LoadFile(string filename, vector<uint8_t> &fileData)
{
	fileData.clear();

	if(_loadFromZip) {
		if(_reader.ExtractFile(filename, fileData)) {
			return true;
		}
	} else {
		ifstream file(FolderUtilities::CombinePath(_hdPackFolder, filename), ios::in | ios::binary);
		if(file.good()) {
			file.seekg(0, ios::end);
			uint32_t fileSize = (uint32_t)file.tellg();
			file.seekg(0, ios::beg);

			fileData = vector<uint8_t>(fileSize, 0);
			file.read((char*)fileData.data(), fileSize);
			
			return true;
		}
	}

	return false;
}

bool HdPackLoader::LoadPack()
{
	try {
		vector<uint8_t> hdDefinition;
		if(!LoadFile("hires.txt", hdDefinition)) {
			return false;
		}

		InitializeGlobalConditions();

		for(string lineContent : StringUtilities::Split(string(hdDefinition.data(), hdDefinition.data() + hdDefinition.size()), '\n')) {
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
			} else if(lineContent.substr(0, 10) == "<overscan>") {
				tokens = StringUtilities::Split(lineContent.substr(10), ',');
				ProcessOverscanTag(tokens);
			} else if(lineContent.substr(0, 5) == "<img>") {
				lineContent = lineContent.substr(5);
				if(!ProcessImgTag(lineContent)) {
					return false;
				}
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

		return true;
	} catch(std::exception ex) {
		MessageManager::Log(string("[HDPack] Error loading HDPack: ") + ex.what());
		return false;
	}
}

bool HdPackLoader::ProcessImgTag(string src)
{
	HdPackBitmapInfo bitmapInfo;
	vector<uint8_t> fileData;
	LoadFile(src, fileData);
	if(PNGHelper::ReadPNG(fileData, bitmapInfo.PixelData, bitmapInfo.Width, bitmapInfo.Height)) {
		_hdNesBitmaps.push_back(bitmapInfo);
		return true;
	} else {
		MessageManager::Log("[HDPack] Error loading HDPack: PNG file " + src + " could not be read.");
		return false;
	}
}

void HdPackLoader::InitializeGlobalConditions()
{
	HdPackCondition* hmirror = new HdPackCondition();
	hmirror->Name = "hmirror";
	hmirror->Type = HdPackConditionType::HorizontalMirroring;
	_data->Conditions.push_back(unique_ptr<HdPackCondition>(hmirror));

	HdPackCondition* vmirror = new HdPackCondition();
	vmirror->Name = "vmirror";
	vmirror->Type = HdPackConditionType::VerticalMirroring;
	_data->Conditions.push_back(unique_ptr<HdPackCondition>(vmirror));

	HdPackCondition* bgpriority = new HdPackCondition();
	bgpriority->Name = "bgpriority";
	bgpriority->Type = HdPackConditionType::BackgroundPriority;
	_data->Conditions.push_back(unique_ptr<HdPackCondition>(bgpriority));
}

void HdPackLoader::ProcessOverscanTag(vector<string> &tokens)
{
	OverscanDimensions overscan;
	overscan.Top = std::stoi(tokens[0]);
	overscan.Right = std::stoi(tokens[1]);
	overscan.Bottom = std::stoi(tokens[2]);
	overscan.Left = std::stoi(tokens[3]);
	_data->HasOverscanConfig = true;
	_data->Overscan = overscan;
}

void HdPackLoader::ProcessPatchTag(vector<string> &tokens)
{
	if(tokens[1].size() != 40) {
		MessageManager::Log(string("[HDPack] Invalid SHA1 hash for patch (" + tokens[0] + "): " + tokens[1]));
		return;
	}
	vector<uint8_t> fileData;
	if(!LoadFile(tokens[0], fileData)) {
		MessageManager::Log(string("[HDPack] Patch file not found: " + tokens[1]));
		return;
	}

	std::transform(tokens[1].begin(), tokens[1].end(), tokens[1].begin(), ::tolower);
	if(_loadFromZip) {
		_data->PatchesByHash[tokens[1]] = VirtualFile(_hdPackFolder, tokens[0]);
	} else {
		_data->PatchesByHash[tokens[1]] = FolderUtilities::CombinePath(_hdPackFolder, tokens[0]);
	}
}

void HdPackLoader::ProcessTileTag(vector<string> &tokens, vector<HdPackCondition*> conditions)
{
	HdPackTileInfo *tileInfo = new HdPackTileInfo();
	size_t index = 0;
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
			tileInfo->TileIndex = std::stoi(tileData);
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

	tileInfo->UpdateFlags();

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
	HdBackgroundFileData* bgFileData = nullptr;
	for(unique_ptr<HdBackgroundFileData> &bgData : _data->BackgroundFileData) {
		if(bgData->PngName == tokens[0]) {
			bgFileData = bgData.get();
		}
	}

	if(!bgFileData) {
		vector<uint8_t> pixelData;
		uint32_t width, height;
		vector<uint8_t> fileContent;
		if(LoadFile(tokens[0], fileContent)) {
			if(PNGHelper::ReadPNG(fileContent, pixelData, width, height)) {
				_data->BackgroundFileData.push_back(unique_ptr<HdBackgroundFileData>(new HdBackgroundFileData()));
				bgFileData = _data->BackgroundFileData.back().get();
				bgFileData->PixelData = pixelData;
				bgFileData->Width = width;
				bgFileData->Height = height;
				bgFileData->PngName = tokens[0];
			}
		}
	}

	HdBackgroundInfo backgroundInfo;
	if(bgFileData) {
		backgroundInfo.Data = bgFileData;
		backgroundInfo.Brightness = (uint8_t)(std::stof(tokens[1]) * 255);
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
	vector<uint8_t> fileData;
	if(LoadFile("palette.dat", fileData)) {
		vector<uint32_t> paletteData;

		for(size_t i = 0; i < fileData.size(); i+= 3){
			paletteData.push_back(0xFF000000 | (fileData[i] << 16) | (fileData[i+1] << 8) | fileData[i+2]);
		}

		if(paletteData.size() == 0x40) {
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
				_data->TileByKey[tileInfo->GetKey(true)] = vector<HdPackTileInfo*>();
			}
			_data->TileByKey[tileInfo->GetKey(true)].push_back(tileInfo.get());
		}
	}
}
