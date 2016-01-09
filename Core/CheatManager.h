#pragma once

#include "stdafx.h"

struct CodeInfo
{
	uint32_t Address;
	uint8_t Value;
	int32_t CompareValue;
	bool IsRelativeAddress;
};

class CheatManager
{
private:
	static CheatManager* Instance;
	vector<unique_ptr<vector<CodeInfo>>> _relativeCheatCodes;
	vector<CodeInfo> _absoluteCheatCodes;

	uint32_t DecodeValue(uint32_t code, uint32_t* bitIndexes, uint32_t bitCount);
	CodeInfo GetGGCodeInfo(string ggCode);
	CodeInfo GetPARCodeInfo(uint32_t parCode);
	void AddCode(CodeInfo &code);

public:
	CheatManager();

	static void AddGameGenieCode(string code);
	static void AddProActionRockyCode(uint32_t code);
	static void AddCustomCode(uint32_t address, uint8_t value, int32_t compareValue = -1, bool isRelativeAddress = true);
	static void ClearCodes();

	static void ApplyRamCodes(uint16_t addr, uint8_t &value);
	static void ApplyPrgCodes(uint8_t *prgRam, uint32_t prgSize);
};