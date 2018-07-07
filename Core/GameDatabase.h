#pragma once
#include "stdafx.h"
#include <unordered_map>
#include "RomData.h"

class GameDatabase
{
private:
	static std::unordered_map<uint32_t, GameInfo> _gameDatabase;

	template<typename T> static T ToInt(string value);

	static BusConflictType GetBusConflictType(string busConflictSetting);
	static GameSystem GetGameSystem(string system);
	static VsSystemType GetVsSystemType(string system);
	static PpuModel GetPpuModel(string model);
	static uint8_t GetSubMapper(GameInfo &info);

	static void InitDatabase();
	static void UpdateRomData(GameInfo &info, RomData &romData);
	static void SetVsSystemDefaults(uint32_t prgCrc32);

public:
	static void LoadGameDb(vector<string> data);

	static void InitializeInputDevices(string inputType, GameSystem system, bool silent = false);
	static void InitializeInputDevices(uint32_t romCrc);
	static void SetGameInfo(uint32_t romCrc, RomData &romData, bool updateRomData, bool forHeaderlessRom);
	static bool GetiNesHeader(uint32_t romCrc, NESHeader &nesHeader);
	static bool GetDbRomSize(uint32_t romCrc, uint32_t &prgSize, uint32_t &chrSize);
};