#pragma once
#include "stdafx.h"
#include <unordered_map>
#include "RomData.h"

class iNesLoader
{
private:
	static std::unordered_map<uint32_t, uint8_t> _mapperByCrc;
	static std::unordered_map<uint32_t, uint8_t> _submapperByCrc;

public:
	RomData LoadRom(vector<uint8_t>& romFile);
};