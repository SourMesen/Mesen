#pragma once
#include "stdafx.h"
#include "DebuggerTypes.h"

class PPU;
class MemoryManager;
class BaseMapper;
class CodeDataLogger;
class Debugger;
class Disassembler;

class MemoryDumper
{
private:
	Debugger* _debugger;
	shared_ptr<PPU> _ppu;
	shared_ptr<MemoryManager> _memoryManager;
	shared_ptr<BaseMapper> _mapper;
	shared_ptr<CodeDataLogger> _codeDataLogger;
	shared_ptr<Disassembler> _disassembler;

public:
	MemoryDumper(shared_ptr<PPU> ppu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper, shared_ptr<CodeDataLogger> codeDataLogger, Debugger *debugger, shared_ptr<Disassembler> disassembler);

	uint32_t GetMemorySize(DebugMemoryType type);
	uint32_t GetMemoryState(DebugMemoryType type, uint8_t *buffer);
	void GetNametable(int nametableIndex, uint32_t* frameBuffer, uint8_t* tileData, uint8_t* paletteData);
	void GetChrBank(int bankIndex, uint32_t* frameBuffer, uint8_t palette, bool largeSprites, CdlHighlightType highlightType);
	void GetSprites(uint32_t* frameBuffer);
	void GetPalette(uint32_t* frameBuffer);

	uint8_t GetMemoryValue(DebugMemoryType memoryType, uint32_t address, bool disableSideEffects = true);
	uint16_t GetMemoryValueWord(DebugMemoryType memoryType, uint32_t address, bool disableSideEffects = true);
	void SetMemoryValue(DebugMemoryType memoryType, uint32_t address, uint8_t value, bool preventRebuildCache = false, bool disableSideEffects = true);
	void SetMemoryValueWord(DebugMemoryType memoryType, uint32_t address, uint16_t value, bool preventRebuildCache = false, bool disableSideEffects = true);
	void SetMemoryValues(DebugMemoryType memoryType, uint32_t address, uint8_t* data, int32_t length);
	void SetMemoryState(DebugMemoryType type, uint8_t *buffer);
};