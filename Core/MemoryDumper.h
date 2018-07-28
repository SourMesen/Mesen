#pragma once
#include "stdafx.h"
#include <unordered_map>
#include "DebuggerTypes.h"

class PPU;
class MemoryManager;
class BaseMapper;
class CodeDataLogger;
class Debugger;
class Disassembler;

struct TileKey
{
	uint8_t TileData[16];
	uint32_t TileIndex;
	bool IsChrRamTile = false;

	TileKey GetKey(bool defaultKey)
	{
		return *this;
	}

	uint32_t GetHashCode() const
	{
		if(IsChrRamTile) {
			return CalculateHash(TileData, 16);
		} else {
			uint64_t key = TileIndex;
			return CalculateHash((uint8_t*)&key, sizeof(key));
		}
	}

	size_t operator() (const TileKey &tile) const
	{
		return tile.GetHashCode();
	}

	bool operator==(const TileKey &other) const
	{
		if(IsChrRamTile) {
			return memcmp((uint8_t*)&TileData, (uint8_t*)&other.TileData, 16) == 0;
		} else {
			return TileIndex == other.TileIndex;
		}
	}

	uint32_t CalculateHash(const uint8_t* key, size_t len) const
	{
		uint32_t result = 0;
		for(size_t i = 0; i < len; i += 4) {
			uint32_t chunk;
			memcpy(&chunk, key, sizeof(uint32_t));
			
			result += chunk;
			result = (result << 2) | (result >> 30);
			key += 4;
		}
		return result;
	}
};

namespace std {
	template <> struct hash<TileKey>
	{
		size_t operator()(const TileKey& x) const
		{
			return x.GetHashCode();
		}
	};
}

class MemoryDumper
{
private:
	Debugger* _debugger;
	shared_ptr<PPU> _ppu;
	shared_ptr<MemoryManager> _memoryManager;
	shared_ptr<BaseMapper> _mapper;
	shared_ptr<CodeDataLogger> _codeDataLogger;
	shared_ptr<Disassembler> _disassembler;

	std::deque<vector<uint8_t>> _undoHistory;

	std::unordered_map<TileKey, uint32_t> _paletteByTile;

	void AddUndoHistory(vector<uint8_t>& originalRomData);

public:
	MemoryDumper(shared_ptr<PPU> ppu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper, shared_ptr<CodeDataLogger> codeDataLogger, Debugger *debugger, shared_ptr<Disassembler> disassembler);

	void GatherChrPaletteInfo();

	bool HasUndoHistory();
	void PerformUndo();

	uint32_t GetMemorySize(DebugMemoryType type);
	uint32_t GetMemoryState(DebugMemoryType type, uint8_t *buffer);
	void GetNametable(int nametableIndex, bool useGrayscalePalette, uint32_t* frameBuffer, uint8_t* tileData, uint8_t* paletteData);
	void GetChrBank(int bankIndex, uint32_t* frameBuffer, uint8_t palette, bool largeSprites, CdlHighlightType highlightType, bool useAutoPalette, bool showSingleColorTilesInGrayscale, uint32_t* paletteBuffer);
	void GetSprites(uint32_t* frameBuffer);
	void GetPalette(uint32_t* frameBuffer);

	uint8_t GetMemoryValue(DebugMemoryType memoryType, uint32_t address, bool disableSideEffects = true);
	uint16_t GetMemoryValueWord(DebugMemoryType memoryType, uint32_t address, bool disableSideEffects = true);
	void SetMemoryValue(DebugMemoryType memoryType, uint32_t address, uint8_t value, bool preventRebuildCache = false, bool disableSideEffects = true);
	void SetMemoryValueWord(DebugMemoryType memoryType, uint32_t address, uint16_t value, bool preventRebuildCache = false, bool disableSideEffects = true);
	void SetMemoryValues(DebugMemoryType memoryType, uint32_t address, uint8_t* data, int32_t length);
	void SetMemoryState(DebugMemoryType type, uint8_t *buffer);
};