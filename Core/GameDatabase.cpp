#include "stdafx.h"
#include "RomData.h"
#include "MessageManager.h"
#include "../Utilities/CRC32.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/StringUtilities.h"
#include "../Utilities/HexUtilities.h"
#include "GameDatabase.h"
#include "EmulationSettings.h"
#include "UnifLoader.h"

std::unordered_map<uint32_t, GameInfo> GameDatabase::_gameDatabase;

template<typename T> 
T GameDatabase::ToInt(string value)
{
	if(value.empty()) {
		return 0;
	}
	return std::stoi(value);
}

void GameDatabase::LoadGameDb(vector<string> data)
{
	for(string &row : data) {
		vector<string> values = StringUtilities::Split(row, ',');
		if(values.size() >= 16) {
			GameInfo gameInfo {
				(uint32_t)std::stoll(values[0], nullptr, 16),
				values[1],
				values[2],
				values[3],
				values[4],
				(uint16_t)ToInt<uint32_t>(values[5]),
				ToInt<uint32_t>(values[6]),
				ToInt<uint32_t>(values[7]),
				ToInt<uint32_t>(values[8]),
				ToInt<uint32_t>(values[9]),
				ToInt<uint32_t>(values[10]),
				ToInt<uint32_t>(values[11]) == 0 ? false : true,
				values[12],
				values[13],
				values[14],
				values[15]
			};

			if(gameInfo.MapperID == 65000) {
				gameInfo.MapperID = UnifLoader::GetMapperID(gameInfo.Board);
			}

			if(!gameInfo.InputType.empty() && gameInfo.InputType[gameInfo.InputType.size() - 1] == '\r') {
				gameInfo.InputType = gameInfo.InputType.substr(0, gameInfo.InputType.size() - 1);
			}

			_gameDatabase[gameInfo.Crc] = gameInfo;
		}
	}

	MessageManager::Log();
	MessageManager::Log("[DB] Initialized - " + std::to_string(_gameDatabase.size()) + " games in DB");
}

void GameDatabase::InitDatabase()
{
	if(_gameDatabase.size() == 0) {
		string dbPath = FolderUtilities::CombinePath(FolderUtilities::GetHomeFolder(), "MesenDB.txt");
		ifstream db(dbPath, ios::in | ios::binary);
		vector<string> dbData;
		while(db.good()) {
			string lineContent;
			std::getline(db, lineContent);
			if(lineContent[lineContent.size() - 1] == '\r') {
				lineContent = lineContent.substr(0, lineContent.size() - 1);
			}
			if(lineContent.empty() || lineContent[0] == '#') {
				continue;
			}
			dbData.push_back(lineContent);
		}
		LoadGameDb(dbData);
	}
}

BusConflictType GameDatabase::GetBusConflictType(string busConflictSetting)
{
	if(busConflictSetting.compare("Y") == 0) {
		return BusConflictType::Yes;
	} else if(busConflictSetting.compare("N") == 0) {
		return BusConflictType::No;
	}
	return BusConflictType::Default;
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
	} else if(system.compare("VsDual") == 0) {
		return GameSystem::VsDualSystem;
	} else if(system.compare("Dendy") == 0) {
		return GameSystem::Dendy;
	} else if(system.compare("Playchoice") == 0) {
		return GameSystem::Playchoice;
	}
	
	return GameSystem::NesNtsc;
}

void GameDatabase::InitializeInputDevices(uint32_t romCrc)
{
	InitDatabase();

	auto result = _gameDatabase.find(romCrc);
	if(result != _gameDatabase.end()) {
		InitializeInputDevices(result->second.InputType, GetGameSystem(result->second.System), true);
	} else {
		InitializeInputDevices("", GameSystem::NesNtsc, true);
	}
}

void GameDatabase::InitializeInputDevices(string inputType, GameSystem system, bool silent)
{
	ControllerType controllers[4] = { ControllerType::StandardController, ControllerType::StandardController, ControllerType::None, ControllerType::None };
	ExpansionPortDevice expDevice = ExpansionPortDevice::None;
	EmulationSettings::ClearFlags(EmulationFlags::HasFourScore);
	
	auto log = [silent](string text) {
		if(!silent) {
			MessageManager::Log(text);
		}
	};

	bool isVsSystem = system == GameSystem::VsUniSystem;
	bool isFamicom = (system == GameSystem::Famicom || system == GameSystem::FDS || system == GameSystem::Dendy);

	if(inputType.compare("Zapper") == 0) {
		log("[DB] Input: Zapper connected");
		if(isFamicom) {
			expDevice = ExpansionPortDevice::Zapper;
		} else {
			if(isVsSystem) {
				//VS Duck Hunt, etc. need the zapper in the first port
				controllers[0] = ControllerType::Zapper;
			} else {
				controllers[1] = ControllerType::Zapper;
			}
		}
	} else if(inputType.compare("FourPlayer") == 0) {
		log("[DB] Input: Four player adapter connected");
		EmulationSettings::SetFlags(EmulationFlags::HasFourScore);
		if(isFamicom) {
			expDevice = ExpansionPortDevice::FourPlayerAdapter;
			controllers[2] = controllers[3] = ControllerType::StandardController;
		} else {
			controllers[2] = controllers[3] = ControllerType::StandardController;
		}
	} else if(inputType.compare("Arkanoid") == 0) {
		log("[DB] Input: Arkanoid controller connected");
		if(isFamicom) {
			expDevice = ExpansionPortDevice::ArkanoidController;
		} else {
			controllers[1] = ControllerType::ArkanoidController;
		}
	} else if(inputType.compare("OekaKidsTablet") == 0) {
		log("[DB] Input: Oeka Kids Tablet connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::OekaKidsTablet;
	} else if(inputType.compare("KonamiHypershot") == 0) {
		log("[DB] Input: Konami Hyper Shot connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::KonamiHyperShot;
	} else if(inputType.compare("FamilyKeyboard") == 0) {
		log("[DB] Input: Family Basic Keyboard connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::FamilyBasicKeyboard;
	} else if(inputType.compare("PartyTap") == 0) {
		log("[DB] Input: Party Tap connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::PartyTap;
	} else if(inputType.compare("Pachinko") == 0) {
		log("[DB] Input: Pachinko controller connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::Pachinko;
	} else if(inputType.compare("ExcitingBoxing") == 0) {
		log("[DB] Input: Exciting Boxing controller connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::ExcitingBoxing;
	} else if(inputType.compare("SuborKeyboard") == 0) {
		log("[DB] Input: Subor mouse connected");
		log("[DB] Input: Subor keyboard connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::SuborKeyboard;
		controllers[1] = ControllerType::SuborMouse;
	} else if(inputType.compare("Mahjong") == 0) {
		log("[DB] Input: Jissen Mahjong controller connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::JissenMahjong;
	} else if(inputType.compare("BarCodeWorld") == 0) {
		log("[DB] Input: Barcode Battler barcode reader connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::BarcodeBattler;
	} else if(inputType.compare("BandaiHypershot") == 0) {
		log("[DB] Input: Bandai Hyper Shot gun connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::BandaiHyperShot;
	} else if(inputType.compare("BattleBox") == 0) {
		log("[DB] Input: Battle Box connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::BattleBox;
	} else if(inputType.compare("TurboFile") == 0) {
		log("[DB] Input: Ascii Turbo File connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::AsciiTurboFile;
	} else if(inputType.compare("FamilyTrainer") == 0) {
		log("[DB] Input: Family Trainer mat connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::FamilyTrainerMat;
	} else if(inputType.compare("PowerPad") == 0 || inputType.compare("FamilyFunFitness") == 0) {
		log("[DB] Input: Power Pad connected");
		system = GameSystem::NesNtsc;
		controllers[1] = ControllerType::PowerPad;
	} else {
		log("[DB] Input: 2 standard controllers connected");
	}

	isFamicom = (system == GameSystem::Famicom || system == GameSystem::FDS || system == GameSystem::Dendy);
	EmulationSettings::SetConsoleType(isFamicom ? ConsoleType::Famicom : ConsoleType::Nes);
	for(int i = 0; i < 4; i++) {
		EmulationSettings::SetControllerType(i, controllers[i]);
	}
	EmulationSettings::SetExpansionDevice(expDevice);
}

uint8_t GameDatabase::GetSubMapper(GameInfo &info)
{
	if(!info.SubmapperID.empty()) {
		return ToInt<uint8_t>(info.SubmapperID);
	} else {
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
	}
	return 0;
}

bool GameDatabase::GetDbRomSize(uint32_t romCrc, uint32_t &prgSize, uint32_t &chrSize)
{
	InitDatabase();
	auto result = _gameDatabase.find(romCrc);
	if(result != _gameDatabase.end()) {
		prgSize = result->second.PrgRomSize * 1024;
		chrSize = result->second.ChrRomSize * 1024;
		return true;
	}
	return false;
}

bool GameDatabase::GetiNesHeader(uint32_t romCrc, NESHeader &nesHeader)
{
	GameInfo info = {};
	InitDatabase();
	auto result = _gameDatabase.find(romCrc);
	if(result != _gameDatabase.end()) {
		info = result->second;

		nesHeader.Byte9 = 0;
		if(info.PrgRomSize > 4096) {
			uint16_t prgSize = info.PrgRomSize / 16;
			nesHeader.PrgCount = prgSize & 0xFF;
			nesHeader.Byte9 |= (prgSize & 0xF00) >> 8;
		} else {
			nesHeader.PrgCount = info.PrgRomSize / 16;
		}

		if(info.ChrRomSize > 2048) {
			uint16_t chrSize = info.ChrRomSize / 8;
			nesHeader.ChrCount = chrSize & 0xFF;
			nesHeader.Byte9 |= (chrSize & 0xF00) >> 4;
		} else {
			nesHeader.ChrCount = info.ChrRomSize / 8;
		}
		
		nesHeader.Byte6 = (info.MapperID & 0x0F) << 4;
		if(info.HasBattery) {
			nesHeader.Byte6 |= 0x02;
		}
		if(info.Mirroring.compare("v") == 0) {
			nesHeader.Byte6 |= 0x01;
		}

		nesHeader.Byte7 = (info.MapperID & 0xF0);
		GameSystem system = GetGameSystem(info.System);
		if(system == GameSystem::Playchoice) {
			nesHeader.Byte7 |= 0x02;
		} else if(system == GameSystem::VsUniSystem) {
			nesHeader.Byte7 |= 0x01;
		}
		
		//Don't set this, otherwise the header will be used over the game DB data
		//nesHeader.Byte7 |= 0x08; //NES 2.0 marker

		nesHeader.Byte8 = ((GetSubMapper(info) & 0x0F) << 4) | ((info.MapperID & 0xF00) >> 8);

		nesHeader.Byte10 = 0;
		if(info.SaveRamSize > 0) {
			nesHeader.Byte10 |= ((int)log2(info.SaveRamSize * 1024) - 6) << 4;
		}
		if(info.WorkRamSize > 0) {
			nesHeader.Byte10 |= ((int)log2(info.WorkRamSize * 1024) - 6);
		}

		nesHeader.Byte11 = 0;
		if(info.ChrRamSize > 0) {
			nesHeader.Byte11 |= ((int)log2(info.ChrRamSize * 1024) - 6);
		}
		
		nesHeader.Byte12 = system == GameSystem::NesPal ? 0x01 : 0;
		nesHeader.Byte13 = 0; //VS PPU variant

		return true;
	}

	return false;
}

void GameDatabase::SetGameInfo(uint32_t romCrc, RomData &romData, bool updateRomData, bool forHeaderlessRom)
{	
	GameInfo info = {};

	InitDatabase();

	auto result = _gameDatabase.find(romCrc);

	if(result != _gameDatabase.end()) {
		info = result->second;
		if(!forHeaderlessRom && info.Board == "UNK") {
			//Boards marked as UNK should only be used for headerless roms (since their data is unverified)
			romData.DatabaseInfo = {};
			return;
		}

		MessageManager::Log("[DB] Game found in database");

		if(info.MapperID < UnifBoards::UnknownBoard) {
			MessageManager::Log("[DB] Mapper: " + std::to_string(info.MapperID) + "  Sub: " + std::to_string(GetSubMapper(info)));
		} else if(info.MapperID == UnifBoards::UnknownBoard) {
			MessageManager::DisplayMessage("Error", "UnsupportedMapper", "UNIF: " + info.Board);
		}

		MessageManager::Log("[DB] System : " + info.System);
		if(!info.Board.empty()) {
			MessageManager::Log("[DB] Board: " + info.Board);
		}
		if(!info.Chip.empty()) {
			MessageManager::Log("[DB] Chip: " + info.Chip);
		}

		switch(GetBusConflictType(info.BusConflicts)) {
			case BusConflictType::Default: break;
			case BusConflictType::Yes: MessageManager::Log("[DB] Bus conflicts: Yes"); break;
			case BusConflictType::No: MessageManager::Log("[DB] Bus conflicts: No"); break;
		}

		if(!info.Mirroring.empty()) {
			string msg = "[DB] Mirroring: ";
			switch(info.Mirroring[0]) {
				case 'h': msg += "Horizontal"; break;
				case 'v': msg += "Vertical"; break;
				case '4': msg += "4 Screens"; break;
				case 'a': msg += "Single screen"; break;
			}
			MessageManager::Log(msg);
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

		if(EmulationSettings::CheckFlag(EmulationFlags::AutoConfigureInput)) {
			InitializeInputDevices(info.InputType, romData.System);
		}
#ifdef _DEBUG
		MessageManager::DisplayMessage("DB", "Mapper: " + std::to_string(romData.MapperID) + "  Sub: " + std::to_string(romData.SubMapperID) + "  System: " + info.System);
#endif
	} else {
		MessageManager::Log("[DB] Game not found in database");
	}

#ifdef LIBRETRO
	SetVsSystemDefaults(romData.PrgCrc32);
#endif

	romData.DatabaseInfo = info;
}

void GameDatabase::UpdateRomData(GameInfo &info, RomData &romData)
{
	romData.MapperID = info.MapperID;
	romData.System = GetGameSystem(info.System);
	romData.SubMapperID = GetSubMapper(info);
	romData.BusConflicts = GetBusConflictType(info.BusConflicts);
	if(info.ChrRamSize > 0) {
		romData.ChrRamSize = info.ChrRamSize * 1024;
	}
	if(info.WorkRamSize > 0) {
		romData.WorkRamSize = info.WorkRamSize * 1024;
	}
	if(info.SaveRamSize > 0) {
		romData.SaveRamSize = info.SaveRamSize * 1024;
	}
	romData.HasBattery |= info.HasBattery;

	if(!info.Mirroring.empty()) {
		switch(info.Mirroring[0]) {
			case 'h': romData.Mirroring = MirroringType::Horizontal; break;
			case 'v': romData.Mirroring = MirroringType::Vertical; break;
			case '4': romData.Mirroring = MirroringType::FourScreens; break;
			case 'a': romData.Mirroring = MirroringType::ScreenAOnly; break;
		}
	}
}

void GameDatabase::SetVsSystemDefaults(uint32_t prgCrc32)
{
	//Used by Libretro port since the data for VS system games is stored in the C# UI (needs to be refactored eventually)
	VsInputType inputType = VsInputType::Default;
	PpuModel model = PpuModel::Ppu2C03;
	uint8_t defaultDip = 0;

	switch(prgCrc32) {
		case 0xEB2DBA63: case 0x98CFE016:
			//TKOBoxing
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04C;
			break;

		case 0x135ADF7C:
			//RBIBaseball
			inputType = VsInputType::TypeB;
			model = PpuModel::Ppu2C04A;
			break;

		case 0xED588F00:
			//"DuckHunt", use defaults
			model = PpuModel::Ppu2C03;
			break;

		case 0x16D3F469:
			//NinjaJajamaruKun
			inputType = VsInputType::TypeC;
			model = PpuModel::Ppu2C05A;
			break;

		case 0x8850924B:
			//Tetris
			model = PpuModel::Ppu2C03;
			inputType = VsInputType::TypeB;
			defaultDip = 32;
			break;

		case 0x8C0C2DF5:
			//TopGun
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C05D;
			break;

		case 0x70901B25:
			//Slalom
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04B;
			break;

		case 0xCF36261E:
			//SuperSkyKid
			inputType = VsInputType::TypeC;
			model = PpuModel::Ppu2C04A;
			break;

		case 0xE1AA8214:
			//StarLuster
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04A;
			defaultDip = 32;
			break;

		case 0xD5D7EAC4:
			//DrMario
			inputType = VsInputType::TypeB;
			model = PpuModel::Ppu2C04C;
			break;

		case 0xFFBEF374:
			//Castlevania
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04B;
			break;

		case 0xE2C0A2BE:
			//Platoon
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04A;
			break;

		case 0x29155E0C:
			//ExciteBike
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04D;
			break;

		case 0xCBE85490:
			//ExciteBikeB
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04C;
			break;

		case 0x07138C06:
			//Clu Clu Land
			inputType = VsInputType::TypeB;
			model = PpuModel::Ppu2C04D;
			break;

		case 0x43A357EF:
			//IceClimber
			inputType = VsInputType::TypeB;
			model = PpuModel::Ppu2C04D;
			break;

		case 0xD4EB5923:
			//IceClimberB
			inputType = VsInputType::TypeD;
			model = PpuModel::Ppu2C04D;
			break;

		case 0x737DD1BF: case 0x4BF3972D: case 0x8B60CC58: case 0x8192C804:
			//SuperMarioBros
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04D;
			break;

		case 0xE528F651:
			//Pinball
			inputType = VsInputType::TypeE;
			model = PpuModel::Ppu2C03;
			break;

		case 0xEC461DB9:
			//PinballB
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04A;
			break;

		case 0xAE8063EF:
			//MachRiderFightingCourse, defaults
			model = PpuModel::Ppu2C03;
			break;

		case 0x0B65A917: case 0x8A6A9848:
			//MachRider
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04B;
			break;

		case 0x46914E3E:
			//Soccer
			inputType = VsInputType::TypeB;
			model = PpuModel::Ppu2C04C;
			break;
			
		case 0x70433F2C:
			//Battle City
			inputType = VsInputType::TypeB;
			model = PpuModel::Ppu2C04A;
			break;

		case 0xD99A2087:
			//Gradius
			inputType = VsInputType::TypeB;
			model = PpuModel::Ppu2C04A;
			break;

		case 0x1E438D52:
			//Goonies
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04C;
			break;

		case 0xFF5135A3:
			//HoganAlley
			model = PpuModel::Ppu2C04A;
			break;

		case 0x17AE56BE:
			//FreedomForce
			model = PpuModel::Ppu2C04A;
			break;

		case 0xC99EC059:
			//RaidBungelingBay
			inputType = VsInputType::TypeD;
			model = PpuModel::Ppu2C04B;
			break;

		case 0xF9D3B0A3: case 0x66BB838F: case 0x9924980A:
			//SuperXevious
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C04A;
			break;

		case 0xA93A5AEE:
			//Golf
			inputType = VsInputType::TypeB;
			model = PpuModel::Ppu2C03;
			break;

		case 0xCC2C4B5D: case 0x86167220:
			//GolfB
			inputType = VsInputType::TypeB;
			model = PpuModel::Ppu2C04B;
			break;

		case 0xCA85E56D:
			//MightyBombJack
			inputType = VsInputType::TypeA;
			model = PpuModel::Ppu2C05B;
			break;

		case 0xFE446787:
			//Gumshoe
			model = PpuModel::Ppu2C05C;
			break;
	}

	EmulationSettings::SetPpuModel(model);
	EmulationSettings::SetDipSwitches(defaultDip);
	EmulationSettings::SetVsInputType(inputType);
}
