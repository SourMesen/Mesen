#pragma once
#include "stdafx.h"
#include <unordered_map>

struct GameInfo
{
	uint32_t Crc;
	string System;
	string Board;
	string Pcb;
	string Chip;
	uint8_t MapperID;
	uint32_t PrgRomSize;
	uint32_t ChrRomSize;
	uint32_t ChrRamSize;
	uint32_t WorkRamSize;
	bool HasBattery;
	string Mirroring;
};

class GameDatabase
{
private:
	static std::unordered_map<uint32_t, GameInfo> _gameDatabase;

	template<typename T> static T ToInt(string value);
	static vector<string> split(const string &s, char delim);

	static GameSystem GetGameSystem(string system);
	static uint8_t GetSubMapper(GameInfo &info);

	static void InitDatabase();

public:
	static void UpdateRomData(uint32_t romCrc, RomData &romData);
};