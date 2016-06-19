#pragma once
#include "stdafx.h"
#include "RomData.h"
#include "MessageManager.h"
#include "../Utilities/CRC32.h"
#include "GameDatabase.h"

std::unordered_map<uint32_t, GameInfo> GameDatabase::_gameDatabase;

template<typename T> 
T GameDatabase::ToInt(string value)
{
	if(value.empty()) {
		return 0;
	}
	return std::stoi(value);
}

vector<string> GameDatabase::split(const string &s, char delim)
{
	vector<string> tokens;
	std::stringstream ss(s);
	std::string item;
	while(std::getline(ss, item, delim)) {
		tokens.push_back(item);
	}
	return tokens;
}

void GameDatabase::InitDatabase()
{
	if(_gameDatabase.size() == 0) {
		ifstream db("MesenDB.txt", ios::in | ios::binary);
		while(db.good()) {
			string lineContent;
			std::getline(db, lineContent);
			if(lineContent.empty() || lineContent[0] == '#') {
				continue;
			}
			vector<string> values = split(lineContent, ',');
			if(values.size() >= 11) {
				GameInfo gameInfo{
					(uint32_t)std::stoll(values[0], nullptr, 16),
					values[1],
					values[2],
					values[3],
					values[4],
					ToInt<uint8_t>(values[5]),
					ToInt<uint32_t>(values[6]),
					ToInt<uint32_t>(values[7]),
					ToInt<uint32_t>(values[8]),
					ToInt<uint32_t>(values[9]),
					ToInt<uint32_t>(values[10]) == 0 ? false : true,
					values.size() > 11 ? values[11] : ""
				};
				_gameDatabase[gameInfo.Crc] = gameInfo;
			}
		}

		MessageManager::Log();
		MessageManager::Log("[DB] Initialized - " + std::to_string(_gameDatabase.size()) + " games in DB");		
	}
}

GameSystem GameDatabase::GetGameSystem(string system)
{
	if(system.compare("NesNtsc") == 0) {
		return GameSystem::NesNtsc;
	} else if(system.compare("NesPal") == 0) {
		return GameSystem::NesPal;
	} else if(system.compare("Famicom") == 0) {
		return GameSystem::Famicom;
	} else if(system.compare("VsUni") == 0) {
		return GameSystem::VsUniSystem;
	} else if(system.compare("Dendy") == 0) {
		return GameSystem::Dendy;
	} else if(system.compare("Playchoice") == 0) {
		return GameSystem::Playchoice;
	}
	
	return GameSystem::NesNtsc;
}

uint8_t GameDatabase::GetSubMapper(GameInfo &info)
{
	switch(info.MapperID) {
		case 4:
			if(info.Board.compare("ACCLAIM-MC-ACC") == 0) {
				return 3; //Acclaim MC-ACC (MMC3 clone)
			}
			break;

		case 21:
			if(info.Pcb.compare("352398") == 0) {
				return 1; //VRC4a
			} else if(info.Pcb.compare("352889") == 0) {
				return 2; //VRC4c
			}
			break;

		case 23:
			if(info.Pcb.compare("352396") == 0) {
				return 2; //VRC4e
			} else if(info.Pcb.compare("350603") == 0 || info.Pcb.compare("350636") == 0 || info.Pcb.compare("350926") == 0 || info.Pcb.compare("351179") == 0 || info.Pcb.compare("LROG009-00") == 0) {
				return 3; //VRC2b
			}
			break;

		case 25:
			if(info.Pcb.compare("351406") == 0) {
				return 1; //VRC4b
			} else if(info.Pcb.compare("352400") == 0) {
				return 2; //VRC4d	
			} else if(info.Pcb.compare("351948") == 0) {
				return 3; //VRC2c	
			}
			break;

		case 32:
			if(info.Board.compare("IREM-G101-B") == 0) {
				return 1; //Major League
			}
			break;
		case 71:
			if(info.Board.compare("CAMERICA-BF9097") == 0) {
				return 1; //Fire Hawk
			}
			break;
		case 78:
			if(info.Board.compare("IREM-HOLYDIVER") == 0) {
				return 3; //Holy Diver
			}
			break;
		case 210:
			if(info.Board.compare("NAMCOT-175") == 0) {
				return 1; //Namco 175
			} else if(info.Board.compare("NAMCOT-340") == 0) {
				return 2; //Namco 340
			}
			break;
	}

	return 0;
}

void GameDatabase::UpdateRomData(uint32_t romCrc, RomData &romData)
{
	InitDatabase();

	auto result = _gameDatabase.find(romCrc);

	if(result != _gameDatabase.end()) {
		MessageManager::Log("[DB] Game found in database");
		GameInfo info = result->second;

		romData.MapperID = info.MapperID;
		romData.System = GetGameSystem(info.System);
		romData.SubMapperID = GetSubMapper(info);
		if(info.ChrRamSize > 0) {
			romData.ChrRamSize = info.ChrRamSize * 1024;
		}
		romData.HasBattery |= info.HasBattery;

		if(!info.Mirroring.empty()) {
			romData.MirroringType = info.Mirroring.compare("h") == 0 ? MirroringType::Horizontal : MirroringType::Vertical;
		}

		MessageManager::Log("[DB] Mapper: " + std::to_string(romData.MapperID) + "  Sub: " + std::to_string(romData.SubMapperID));
		MessageManager::Log("[DB] System : " + info.System);
		if(!info.Mirroring.empty()) {
			MessageManager::Log("[DB] Mirroring: " + string(info.Mirroring.compare("h") == 0 ? "Horizontal" : "Vertical"));
		}
		MessageManager::Log("[DB] PRG ROM: " + std::to_string(info.PrgRomSize) + " KB");
		MessageManager::Log("[DB] CHR ROM: " + std::to_string(info.ChrRomSize) + " KB");
		if(info.ChrRamSize > 0) {
			MessageManager::Log("[DB] CHR RAM: " + std::to_string(info.ChrRamSize) + " KB");
		}
		MessageManager::Log("[DB] Battery: " + string(info.HasBattery ? "Yes" : "No"));

		#ifdef _DEBUG
		MessageManager::DisplayMessage("DB", "Mapper: " + std::to_string(romData.MapperID) + "  Sub: " + std::to_string(romData.SubMapperID) + "  System: " + info.System);
		#endif
	}
}