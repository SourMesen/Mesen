#pragma once
#include "stdafx.h"
#include <unordered_map>
#include "RomData.h"

class GameDatabase
{
private:
	static std::unordered_map<uint32_t, GameInfo> _gameDatabase;

	template<typename T> static T ToInt(string value);
	static vector<string> split(const string &s, char delim);

	static GameSystem GetGameSystem(string system);
	static uint8_t GetSubMapper(GameInfo &info);

	static void InitDatabase();
	static void UpdateRomData(GameInfo &info, RomData &romData);

public:
	static void InitializeInputDevices(string inputType, GameSystem system);
	static void SetGameInfo(uint32_t romCrc, RomData &romData, bool updateRomData);
	static bool GetiNesHeader(uint32_t romCrc, NESHeader &nesHeader);
};