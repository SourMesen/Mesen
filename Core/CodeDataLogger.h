#pragma once

#include "stdafx.h"
#include "../Utilities/SimpleLock.h"
#include "DebuggerTypes.h"

class Debugger;

enum class CdlPrgFlags
{
	None = 0x00,
	Code = 0x01,
	Data = 0x02,
	IndirectCode = 0x10,
	IndirectData = 0x20,
	PcmData = 0x40,
	SubEntryPoint = 0x80
};

enum class CdlChrFlags
{
	Drawn = 0x01,
	Read = 0x02,
};

enum class CdlStripFlag
{
	StripNone = 0,
	StripUnused,
	StripUsed,
};

struct CdlRatios
{
	float CodeRatio;
	float DataRatio;
	float PrgRatio;
	
	float ChrRatio;
	float ChrReadRatio;
	float ChrDrawnRatio;
};

class CodeDataLogger
{
private:
	Debugger* _debugger = nullptr;
	uint8_t *_cdlData = nullptr;
	uint32_t _prgSize = 0;
	uint32_t _chrSize = 0;

	uint32_t _codeSize = 0;
	uint32_t _dataSize = 0;
	uint32_t _usedChrSize = 0;
	uint32_t _readChrSize = 0;
	uint32_t _drawnChrSize = 0;

	SimpleLock _lock;
	
public:
	CodeDataLogger(Debugger *debugger, uint32_t prgSize, uint32_t chrSize);
	~CodeDataLogger();

	void Reset();

	bool LoadCdlFile(string cdlFilepath);
	bool SaveCdlFile(string cdlFilepath);

	void SetFlag(int32_t absoluteAddr, CdlPrgFlags flag);
	void SetFlag(int32_t chrAbsoluteAddr, CdlChrFlags flag);

	CdlRatios GetRatios();

	bool IsCode(uint32_t absoluteAddr);
	bool IsSubEntryPoint(uint32_t absoluteAddr);
	bool IsData(uint32_t absoluteAddr);
	bool IsRead(uint32_t absoluteAddr);
	bool IsDrawn(uint32_t absoluteAddr);

	void GetCdlData(uint32_t offset, uint32_t length, DebugMemoryType memoryType, uint8_t* cdlData);

	void StripData(uint8_t* romBuffer, CdlStripFlag flag);

	void MarkPrgBytesAs(uint32_t start, uint32_t end, CdlPrgFlags type);
};