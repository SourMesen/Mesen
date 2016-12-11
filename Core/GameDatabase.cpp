#include "stdafx.h"
#include "RomData.h"
#include "MessageManager.h"
#include "../Utilities/CRC32.h"
#include "../Utilities/FolderUtilities.h"
#include "GameDatabase.h"
#include "EmulationSettings.h"

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
		string dbPath = FolderUtilities::CombinePath(FolderUtilities::GetHomeFolder(), "MesenDB.txt");
		ifstream db(dbPath, ios::in | ios::binary);
		while(db.good()) {
			string lineContent;
			std::getline(db, lineContent);
			if(lineContent.empty() || lineContent[0] == '#') {
				continue;
			}
			vector<string> values = split(lineContent, ',');
			if(values.size() >= 13) {
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
					ToInt<uint32_t>(values[10]),
					ToInt<uint32_t>(values[11]) == 0 ? false : true,
					values[12],
					values.size() > 13 ? values[13] : ""
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

void GameDatabase::InitializeInputDevices(string inputType, GameSystem system)
{
	if(EmulationSettings::CheckFlag(EmulationFlags::AutoConfigureInput)) {
		ControllerType controllers[4] = { ControllerType::StandardController, ControllerType::StandardController, ControllerType::None, ControllerType::None };
		ExpansionPortDevice expDevice = ExpansionPortDevice::None;
		EmulationSettings::ClearFlags(EmulationFlags::HasFourScore);

		bool isFamicom = (system == GameSystem::Famicom || system == GameSystem::FDS);

		if(inputType.compare("Zapper") == 0) {
			MessageManager::Log("[DB] Input: Zapper connected");
			if(isFamicom) {
				expDevice = ExpansionPortDevice::Zapper;
			} else {
				controllers[1] = ControllerType::Zapper;
			}
		} else if(inputType.compare("FourPlayer") == 0) {
			MessageManager::Log("[DB] Input: Four player adapter connected");
			EmulationSettings::SetFlags(EmulationFlags::HasFourScore);
			if(isFamicom) {
				expDevice = ExpansionPortDevice::FourPlayerAdapter;
				controllers[2] = controllers[3] = ControllerType::StandardController;
			} else {
				controllers[2] = controllers[3] = ControllerType::StandardController;
			}
		} else if(inputType.compare("Arkanoid") == 0) {
			MessageManager::Log("[DB] Input: Arkanoid controller connected");
			if(isFamicom) {
				expDevice = ExpansionPortDevice::ArkanoidController;
			} else {
				controllers[1] = ControllerType::ArkanoidController;
			}
		} else if(inputType.compare("OekaKidsTablet") == 0) {
			MessageManager::Log("[DB] Input: Oeka Kids Tablet connected");
			system = GameSystem::Famicom;
			expDevice = ExpansionPortDevice::OekaKidsTablet;
		} else {
			MessageManager::Log("[DB] Input: 2 standard controllers connected");
		}

		EmulationSettings::SetConsoleType(isFamicom ? ConsoleType::Famicom : ConsoleType::Nes);
		for(int i = 0; i < 4; i++) {
			EmulationSettings::SetControllerType(i, controllers[i]);
		}
		EmulationSettings::SetExpansionDevice(expDevice);
	}
}

uint8_t GameDatabase::GetSubMapper(GameInfo &info)
{
	switch(info.MapperID) {
		case 1:
			if(info.Board.find("SEROM") != string::npos ||
				info.Board.find("SHROM") != string::npos ||
				info.Board.find("SH1ROM") != string::npos) {
				//SEROM, SHROM, SH1ROM have fixed PRG banking
				return 5;
			}
			break;

		case 3:
			if(info.Board.compare("NES-CNROM") == 0) {
				//Enable bus conflicts for CNROM games
				//Fixes "Cybernoid - The Fighting Machine" which requires open bus behavior to work properly
				return 2;
			}
			break;
		case 4:
			if(info.Board.compare("ACCLAIM-MC-ACC") == 0) {
				return 3; //Acclaim MC-ACC (MMC3 clone)
			} else if(info.Chip.compare("MMC6B") == 0) {
				return 1; //MMC6 (Star Tropics)
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
		case 185:
			if(info.Crc == 0x0F05FF0A) {
				//Seicross (v2)
				//Not a real submapper, used to alter behavior specifically for this game
				//This is equivalent to FCEUX's mapper 181
				return 16; 
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

void GameDatabase::SetGameInfo(uint32_t romCrc, RomData &romData, bool updateRomData)
{	
	GameInfo info = {};

	InitDatabase();

	auto result = _gameDatabase.find(romCrc);

	if(result != _gameDatabase.end()) {
		MessageManager::Log("[DB] Game found in database");
		info = result->second;


		MessageManager::Log("[DB] Mapper: " + std::to_string(info.MapperID) + "  Sub: " + std::to_string(GetSubMapper(info)));
		MessageManager::Log("[DB] System : " + info.System);
		if(!info.Board.empty()) {
			MessageManager::Log("[DB] Board: " + info.Board);
		}
		if(!info.Chip.empty()) {
			MessageManager::Log("[DB] Chip: " + info.Chip);
		}

		if(!info.Mirroring.empty()) {
			MessageManager::Log("[DB] Mirroring: " + string(info.Mirroring.compare("h") == 0 ? "Horizontal" : "Vertical"));
		}
		MessageManager::Log("[DB] PRG ROM: " + std::to_string(info.PrgRomSize) + " KB");
		MessageManager::Log("[DB] CHR ROM: " + std::to_string(info.ChrRomSize) + " KB");
		if(info.ChrRamSize > 0) {
			MessageManager::Log("[DB] CHR RAM: " + std::to_string(info.ChrRamSize) + " KB");
		}
		if(info.WorkRamSize > 0) {
			MessageManager::Log("[DB] Work RAM: " + std::to_string(info.WorkRamSize) + " KB");
		}
		if(info.SaveRamSize > 0) {
			MessageManager::Log("[DB] Save RAM: " + std::to_string(info.SaveRamSize) + " KB");
		}
		MessageManager::Log("[DB] Battery: " + string(info.HasBattery ? "Yes" : "No"));

		if(updateRomData) {
			MessageManager::Log("[DB] Database info will be used instead of file header.");
			UpdateRomData(info, romData);
		}


#ifdef _DEBUG
		MessageManager::DisplayMessage("DB", "Mapper: " + std::to_string(romData.MapperID) + "  Sub: " + std::to_string(romData.SubMapperID) + "  System: " + info.System);
#endif
	} else {
		MessageManager::Log("[DB] Game not found in database");
	}

	InitializeInputDevices(info.InputType, romData.System);

	romData.DatabaseInfo = info;
}

void GameDatabase::UpdateRomData(GameInfo &info, RomData &romData)
{
	romData.MapperID = info.MapperID;
	romData.System = GetGameSystem(info.System);
	romData.SubMapperID = GetSubMapper(info);
	romData.ChrRamSize = info.ChrRamSize * 1024;
	if(info.WorkRamSize > 0) {
		romData.WorkRamSize = info.WorkRamSize * 1024;
	}
	if(info.SaveRamSize > 0) {
		romData.SaveRamSize = info.SaveRamSize * 1024;
	}
	romData.HasBattery |= info.HasBattery;

	if(!info.Mirroring.empty()) {
		romData.Mirroring = info.Mirroring.compare("h") == 0 ? MirroringType::Horizontal : MirroringType::Vertical;
	}
}