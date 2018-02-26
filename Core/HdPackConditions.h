#pragma once
#include "stdafx.h"
#include "HdData.h"
#include "../Utilities/HexUtilities.h"

struct HdPackBaseTileCondition : public HdPackCondition
{
	int32_t TileX;
	int32_t TileY;
	uint32_t PaletteColors;
	uint8_t TileData[16];
	int32_t TileIndex;
	int32_t PixelOffset;

	void Initialize(int32_t x, int32_t y, uint32_t palette, int32_t tileIndex, string tileData = "")
	{
		TileX = x;
		TileY = y;
		PixelOffset = (y * 256) + x;
		PaletteColors = palette;
		TileIndex = tileIndex;
		if(tileData.size() == 32) {
			for(int i = 0; i < 16; i++) {
				TileData[i] = HexUtilities::FromHex(tileData.substr(i * 2, 2));
			}
			TileIndex = -1;
		}
	}

	string ToString() override
	{
		stringstream out;
		out << "<condition>" << Name << "," << GetConditionName() << ",";
		out << TileX << ",";
		out << TileY << ",";
		if(TileIndex >= 0) {
			out << TileIndex << ",";
		} else {
			for(int i = 0; i < 16; i++) {
				out << HexUtilities::ToHex(TileData[i]);
			}
		}
		out << HexUtilities::ToHex(PaletteColors, true);

		return out.str();
	}
};

enum class HdPackConditionOperator
{
	Equal = 0,
	NotEqual = 1,
	GreaterThan = 2,
	LowerThan = 3
};

struct HdPackBaseMemoryCondition : public HdPackCondition
{
	uint32_t OperandA;
	HdPackConditionOperator Operator;
	uint32_t OperandB;

	void Initialize(uint32_t operandA, HdPackConditionOperator op, uint32_t operandB)
	{
		OperandA = operandA;
		Operator = op;
		OperandB = operandB;
	}

	string ToString() override
	{
		stringstream out;
		out << "<condition>" << Name << "," << GetConditionName() << ",";
		out << OperandA << ",";
		switch(Operator) {
			case HdPackConditionOperator::Equal: out << "=="; break;
			case HdPackConditionOperator::NotEqual: out << "!="; break;
			case HdPackConditionOperator::GreaterThan: out << ">"; break;
			case HdPackConditionOperator::LowerThan: out << "<"; break;
		}
		out << ",";
		out << OperandB;

		return out.str();
	}
};

struct HdPackHorizontalMirroringCondition : public HdPackCondition
{
	string GetConditionName() override { return "hmirror"; }
	string ToString() override { return ""; }
	bool IsExcludedFromFile() override { return true; }

	bool InternalCheckCondition(HdScreenInfo *screenInfo, int x, int y, HdPpuTileInfo* tile) override
	{
		return tile && tile->HorizontalMirroring;
	}
};

struct HdPackVerticalMirroringCondition : public HdPackCondition
{
	string GetConditionName() override { return "vmirror"; }
	string ToString() override { return ""; }
	bool IsExcludedFromFile() override { return true; }

	bool InternalCheckCondition(HdScreenInfo *screenInfo, int x, int y, HdPpuTileInfo* tile) override
	{
		return tile && tile->VerticalMirroring;
	}
};

struct HdPackBgPriorityCondition : public HdPackCondition
{
	string GetConditionName() override { return "bgpriority"; }
	string ToString() override { return ""; }
	bool IsExcludedFromFile() override { return true; }

	bool InternalCheckCondition(HdScreenInfo *screenInfo, int x, int y, HdPpuTileInfo* tile) override
	{
		return tile && tile->BackgroundPriority;
	}
};

struct HdPackMemoryCheckCondition : public HdPackBaseMemoryCondition
{
	HdPackMemoryCheckCondition() { _useCache = true; }
	string GetConditionName() override { return "memoryCheck"; }

	bool InternalCheckCondition(HdScreenInfo *screenInfo, int x, int y, HdPpuTileInfo* tile) override
	{
		switch(Operator) {
			case HdPackConditionOperator::Equal: return screenInfo->WatchedAddressValues[OperandA] == screenInfo->WatchedAddressValues[OperandB];
			case HdPackConditionOperator::NotEqual: return screenInfo->WatchedAddressValues[OperandA] != screenInfo->WatchedAddressValues[OperandB];
			case HdPackConditionOperator::GreaterThan: return screenInfo->WatchedAddressValues[OperandA] > screenInfo->WatchedAddressValues[OperandB];
			case HdPackConditionOperator::LowerThan: return screenInfo->WatchedAddressValues[OperandA] < screenInfo->WatchedAddressValues[OperandB];
		}
		return false;
	}
};

struct HdPackMemoryCheckConstantCondition : public HdPackBaseMemoryCondition
{
	HdPackMemoryCheckConstantCondition() { _useCache = true; }
	string GetConditionName() override { return "memoryCheckConstant"; }

	bool InternalCheckCondition(HdScreenInfo *screenInfo, int x, int y, HdPpuTileInfo* tile) override
	{
		switch(Operator) {
			case HdPackConditionOperator::Equal: return screenInfo->WatchedAddressValues[OperandA] == OperandB;
			case HdPackConditionOperator::NotEqual: return screenInfo->WatchedAddressValues[OperandA] != OperandB;
			case HdPackConditionOperator::GreaterThan: return screenInfo->WatchedAddressValues[OperandA] > OperandB;
			case HdPackConditionOperator::LowerThan: return screenInfo->WatchedAddressValues[OperandA] < OperandB;
		}
		return false;
	}
};

struct HdPackFrameRangeCondition : public HdPackCondition
{
	uint32_t OperandA;
	uint32_t OperandB;

	HdPackFrameRangeCondition() { _useCache = true; }
	string GetConditionName() override { return "frameRange"; }

	void Initialize(uint32_t operandA, uint32_t operandB)
	{
		OperandA = operandA;
		OperandB = operandB;
	}

	string ToString() override
	{
		stringstream out;
		out << "<condition>" << Name << "," << GetConditionName() << ",";
		out << OperandA << ",";
		out << OperandB;

		return out.str();
	}

	bool InternalCheckCondition(HdScreenInfo *screenInfo, int x, int y, HdPpuTileInfo* tile) override
	{
		return screenInfo->FrameNumber % OperandA >= OperandB;
	}
};

struct HdPackTileAtPositionCondition : public HdPackBaseTileCondition
{
	HdPackTileAtPositionCondition() { _useCache = true; }
	string GetConditionName() override { return "tileAtPosition"; }

	bool InternalCheckCondition(HdScreenInfo *screenInfo, int x, int y, HdPpuTileInfo* tile) override
	{
		HdPpuTileInfo &targetTile = screenInfo->ScreenTiles[PixelOffset].Tile;
		if(TileIndex >= 0) {
			return targetTile.PaletteColors == PaletteColors && targetTile.TileIndex == TileIndex;
		} else {
			return memcmp(&targetTile.PaletteColors, &PaletteColors, sizeof(PaletteColors) + sizeof(TileData)) == 0;
		}
	}
};

struct HdPackSpriteAtPositionCondition : public HdPackBaseTileCondition
{
	HdPackSpriteAtPositionCondition() { _useCache = true; }	
	string GetConditionName() override { return "spriteAtPosition"; }

	bool InternalCheckCondition(HdScreenInfo *screenInfo, int x, int y, HdPpuTileInfo* tile) override
	{
		for(int i = 0, len = screenInfo->ScreenTiles[PixelOffset].SpriteCount; i < len;  i++) {
			HdPpuTileInfo &targetTile = screenInfo->ScreenTiles[PixelOffset].Sprite[i];
			if(TileIndex >= 0) {
				if(targetTile.PaletteColors == PaletteColors && targetTile.TileIndex == TileIndex) {
					return true;
				}
			} else {
				if(memcmp(&targetTile.PaletteColors, &PaletteColors, sizeof(PaletteColors) + sizeof(TileData)) == 0) {
					return true;
				}
			}
		}
		return false;
	}
};

struct HdPackTileNearbyCondition : public HdPackBaseTileCondition
{
	string GetConditionName() override { return "tileNearby"; }

	bool InternalCheckCondition(HdScreenInfo *screenInfo, int x, int y, HdPpuTileInfo* tile) override
	{
		int pixelIndex = PixelOffset + (y * 256) + x;
		if(pixelIndex < 0 || pixelIndex > PPU::PixelCount) {
			return false;
		}

		HdPpuTileInfo &targetTile = screenInfo->ScreenTiles[pixelIndex].Tile;
		if(TileIndex >= 0) {
			return targetTile.PaletteColors == PaletteColors && targetTile.TileIndex == TileIndex;
		} else {
			return memcmp(&targetTile.PaletteColors, &PaletteColors, sizeof(PaletteColors) + sizeof(TileData)) == 0;
		}
	}
};

struct HdPackSpriteNearbyCondition : public HdPackBaseTileCondition
{
	string GetConditionName() override { return "spriteNearby"; }

	bool InternalCheckCondition(HdScreenInfo *screenInfo, int x, int y, HdPpuTileInfo* tile) override
	{
		int xSign = tile && tile->HorizontalMirroring ? -1 : 1;
		int ySign = tile && tile->VerticalMirroring ? -1 : 1;
		int pixelIndex = ((y + TileY * ySign) * 256) + x + (TileX * xSign);

		if(pixelIndex < 0 || pixelIndex > PPU::PixelCount) {
			return false;
		}

		for(int i = 0, len = screenInfo->ScreenTiles[pixelIndex].SpriteCount; i < len;  i++) {
			HdPpuTileInfo &targetTile = screenInfo->ScreenTiles[pixelIndex].Sprite[i];
			if(TileIndex >= 0) {
				if(targetTile.PaletteColors == PaletteColors && targetTile.TileIndex == TileIndex) {
					return true;
				}
			} else {
				if(memcmp(&targetTile.PaletteColors, &PaletteColors, sizeof(PaletteColors) + sizeof(TileData)) == 0) {
					return true;
				}
			}
		}

		return false;
	}
};
