#pragma once

#include "stdafx.h"

class Console;

struct CodeInfo
{
	uint32_t Address;
	uint8_t Value;
	int32_t CompareValue;
	bool IsRelativeAddress;
};

enum class CheatType
{
	GameGenie = 0,
	ProActionRocky = 1,
	Custom = 2
};

struct CheatInfo
{
	CheatType Type;
	uint32_t ProActionRockyCode;
	uint32_t Address;
	char GameGenieCode[9];
	uint8_t Value;
	uint8_t CompareValue;
	bool UseCompareValue;
	bool IsRelativeAddress;
};

class CheatManager
{
private:
	shared_ptr<Console> _console;

	vector<unique_ptr<vector<CodeInfo>>> _relativeCheatCodes;
	vector<CodeInfo> _absoluteCheatCodes;

	uint32_t DecodeValue(uint32_t code, uint32_t* bitIndexes, uint32_t bitCount);
	CodeInfo GetGGCodeInfo(string ggCode);
	CodeInfo GetPARCodeInfo(uint32_t parCode);
	void AddCode(CodeInfo &code);
	
public:
	CheatManager(shared_ptr<Console> console);

	void AddGameGenieCode(string code);
	void AddProActionRockyCode(uint32_t code);
	void AddCustomCode(uint32_t address, uint8_t value, int32_t compareValue = -1, bool isRelativeAddress = true);
	void ClearCodes();

	vector<CodeInfo> GetCheats();
	void SetCheats(vector<CodeInfo> &cheats);
	void SetCheats(CheatInfo cheats[], uint32_t length);

	void ApplyRamCodes(uint16_t addr, uint8_t &value);
	void ApplyPrgCodes(uint8_t *prgRam, uint32_t prgSize);
};