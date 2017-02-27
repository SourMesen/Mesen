#pragma once
#include "stdafx.h"
#include "DebuggerTypes.h"

class PPU;
class MemoryManager;
class BaseMapper;
class CodeDataLogger;
class Debugger;

class MemoryDumper
{
private:
	Debugger* _debugger;
	shared_ptr<PPU> _ppu;
	shared_ptr<MemoryManager> _memoryManager;
	shared_ptr<BaseMapper> _mapper;
	shared_ptr<CodeDataLogger> _codeDataLogger;

public:
	MemoryDumper(shared_ptr<PPU> ppu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper, shared_ptr<CodeDataLogger> codeDataLogger, Debugger *debugger);

	uint32_t GetMemoryState(DebugMemoryType type, uint8_t *buffer);
	void GetNametable(int nametableIndex, uint32_t* frameBuffer, uint8_t* tileData, uint8_t* paletteData);
	void GetChrBank(int bankIndex, uint32_t* frameBuffer, uint8_t palette, bool largeSprites, CdlHighlightType highlightType);
	void GetSprites(uint32_t* frameBuffer);
	void GetPalette(uint32_t* frameBuffer);

	void SetMemoryValue(DebugMemoryType memoryType, uint32_t address, uint8_t value);
	void SetMemoryState(DebugMemoryType type, uint8_t *buffer);
};