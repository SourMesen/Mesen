#pragma once

#include "stdafx.h"
#include "../Utilities/SimpleLock.h"

enum class CdlPrgFlags
{
	Code = 0x01,
	Data = 0x02,
	IndirectCode = 0x10,
	IndirectData = 0x20,
	PcmData = 0x40,
};

enum class CdlChrFlags
{
	Drawn = 0x01,
	Read = 0x02,
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
	CodeDataLogger(uint32_t prgSize, uint32_t chrSize);
	~CodeDataLogger();

	void Reset();

	bool LoadCdlFile(string cdlFilepath);
	bool SaveCdlFile(string cdlFilepath);

	void SetFlag(int32_t absoluteAddr, CdlPrgFlags flag);
	void SetFlag(int32_t chrAbsoluteAddr, CdlChrFlags flag);

	CdlRatios GetRatios();

	bool IsCode(uint32_t absoluteAddr);
	bool IsData(uint32_t absoluteAddr);
	bool IsRead(uint32_t absoluteAddr);
	bool IsDrawn(uint32_t absoluteAddr);
};