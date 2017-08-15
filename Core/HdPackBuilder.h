#pragma once
#include "stdafx.h"
#include "../Utilities/xBRZ/xbrz.h"
#include "../Utilities/HQX/hqx.h"
#include "../Utilities/Scale2x/scalebit.h"
#include "../Utilities/KreedSaiEagle/SaiEagle.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/PNGHelper.h"
#include "../Utilities/HexUtilities.h"
#include "Console.h"
#include "HdPackLoader.h"
#include "HdNesPack.h"
#include "BaseMapper.h"
#include "Types.h"
#include <map>

class HdPackBuilder
{
private:
	static HdPackBuilder* _instance;

	HdPackData _hdData;
	std::unordered_map<HdTileKey, uint32_t> _tileUsageCount;
	std::unordered_map<HdTileKey, HdPackTileInfo*> _tilesByKey;
	std::map<uint32_t, std::map<uint32_t, vector<HdPackTileInfo*>>> _tilesByChrBankByPalette;
	bool _isChrRam;
	uint32_t _chrRamBankSize;
	ScaleFilterType _filterType;
	string _saveFolder;
	string _romName;
	uint32_t _flags;

	//Used to group blank tiles together
	int _blankTileIndex = 0;
	int _blankTilePalette = 0;

	void AddTile(HdPackTileInfo *tile, uint32_t usageCount);
	void GenerateHdTile(HdPackTileInfo *tile);
	void DrawTile(HdPackTileInfo *tile, int tileIndex, uint32_t* pngBuffer, int pageNumber, bool containsSpritesOnly);

public:
	HdPackBuilder(string saveFolder, ScaleFilterType filterType, uint32_t scale, uint32_t flags, uint32_t chrRamBankSize, bool isChrRam);
	~HdPackBuilder();

	void ProcessTile(uint32_t x, uint32_t y, uint16_t tileAddr, HdPpuTileInfo& tile, BaseMapper* mapper, bool isSprite, uint32_t chrBankHash, bool transparencyRequired);
	void SaveHdPack();
	
	static void GetChrBankList(uint32_t *banks);
	static void GetBankPreview(uint32_t bankNumber, uint32_t pageNumber, uint8_t *rgbBuffer);
};