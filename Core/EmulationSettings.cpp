#include "stdafx.h"
#include "EmulationSettings.h"
#include "Console.h"
#include "RewindManager.h"

//Version 0.9.9
uint16_t EmulationSettings::_versionMajor = 0;
uint8_t EmulationSettings::_versionMinor = 9;
uint8_t EmulationSettings::_versionRevision = 9;

SimpleLock EmulationSettings::_lock;
SimpleLock EmulationSettings::_shortcutLock;
SimpleLock EmulationSettings::_equalizerLock;

const vector<uint32_t> EmulationSettings::_speedValues = { { 1, 3, 6, 12, 25, 50, 75, 100, 150, 200, 250, 300, 350, 400, 450, 500, 750, 1000, 2000, 4000 } };

Language EmulationSettings::_displayLanguage = Language::English;

uint32_t EmulationSettings::GetEmulationSpeed(bool ignoreTurbo)
{
	if(ignoreTurbo) {
		return _emulationSpeed;
	} else if(CheckFlag(EmulationFlags::ForceMaxSpeed)) {
		return 0;
	} else if(CheckFlag(EmulationFlags::Turbo)) {
		return _turboSpeed;
	} else if(CheckFlag(EmulationFlags::Rewind)) {
		return _rewindSpeed;
	} else {
		return _emulationSpeed;
	}
}

double EmulationSettings::GetAspectRatio(shared_ptr<Console> console)
{
	switch(_aspectRatio) {
		case VideoAspectRatio::NoStretching: return 0.0;

		case VideoAspectRatio::Auto:
		{
			NesModel model = GetNesModel();
			if(model == NesModel::Auto) {
				model = console->GetModel();
			}
			return (model == NesModel::PAL || model == NesModel::Dendy) ? (9440000.0 / 6384411.0) : (128.0 / 105.0);
		}

		case VideoAspectRatio::NTSC: return 128.0 / 105.0;
		case VideoAspectRatio::PAL: return 9440000.0 / 6384411.0;
		case VideoAspectRatio::Standard: return 4.0 / 3.0;
		case VideoAspectRatio::Widescreen: return 16.0 / 9.0;
		case VideoAspectRatio::Custom: return _customAspectRatio;
	}
	return 0.0;
}

void EmulationSettings::InitializeInputDevices(GameInputType inputType, GameSystem system, bool silent)
{
	ControllerType controllers[4] = { ControllerType::StandardController, ControllerType::StandardController, ControllerType::None, ControllerType::None };
	ExpansionPortDevice expDevice = ExpansionPortDevice::None;
	ClearFlags(EmulationFlags::HasFourScore);

	auto log = [silent](string text) {
		if(!silent) {
			MessageManager::Log(text);
		}
	};

	bool isFamicom = (system == GameSystem::Famicom || system == GameSystem::FDS || system == GameSystem::Dendy);

	if(inputType == GameInputType::VsZapper) {
		//VS Duck Hunt, etc. need the zapper in the first port
		log("[Input] VS Zapper connected");
		controllers[0] = ControllerType::Zapper;
	} else if(inputType == GameInputType::Zapper) {
		log("[Input] Zapper connected");
		if(isFamicom) {
			expDevice = ExpansionPortDevice::Zapper;
		} else {
			controllers[1] = ControllerType::Zapper;
		}
	} else if(inputType == GameInputType::FourScore) {
		log("[Input] Four score connected");
		SetFlags(EmulationFlags::HasFourScore);
		controllers[2] = controllers[3] = ControllerType::StandardController;
	} else if(inputType == GameInputType::FourPlayerAdapter) {
		log("[Input] Four player adapter connected");
		SetFlags(EmulationFlags::HasFourScore);
		expDevice = ExpansionPortDevice::FourPlayerAdapter;
		controllers[2] = controllers[3] = ControllerType::StandardController;
	} else if(inputType == GameInputType::ArkanoidControllerFamicom) {
		log("[Input] Arkanoid controller (Famicom) connected");
		expDevice = ExpansionPortDevice::ArkanoidController;
	} else if(inputType == GameInputType::ArkanoidControllerNes) {
		log("[Input] Arkanoid controller (NES) connected");
		controllers[1] = ControllerType::ArkanoidController;
	} else if(inputType == GameInputType::DoubleArkanoidController) {
		log("[Input] 2x arkanoid controllers (NES) connected");
		controllers[0] = ControllerType::ArkanoidController;
		controllers[1] = ControllerType::ArkanoidController;
	} else if(inputType == GameInputType::OekaKidsTablet) {
		log("[Input] Oeka Kids Tablet connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::OekaKidsTablet;
	} else if(inputType == GameInputType::KonamiHyperShot) {
		log("[Input] Konami Hyper Shot connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::KonamiHyperShot;
	} else if(inputType == GameInputType::FamilyBasicKeyboard) {
		log("[Input] Family Basic Keyboard connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::FamilyBasicKeyboard;
	} else if(inputType == GameInputType::PartyTap) {
		log("[Input] Party Tap connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::PartyTap;
	} else if(inputType == GameInputType::PachinkoController) {
		log("[Input] Pachinko controller connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::Pachinko;
	} else if(inputType == GameInputType::ExcitingBoxing) {
		log("[Input] Exciting Boxing controller connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::ExcitingBoxing;
	} else if(inputType == GameInputType::SuborKeyboardMouse1) {
		log("[Input] Subor mouse connected");
		log("[Input] Subor keyboard connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::SuborKeyboard;
		controllers[1] = ControllerType::SuborMouse;
	} else if(inputType == GameInputType::JissenMahjong) {
		log("[Input] Jissen Mahjong controller connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::JissenMahjong;
	} else if(inputType == GameInputType::BarcodeBattler) {
		log("[Input] Barcode Battler barcode reader connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::BarcodeBattler;
	} else if(inputType == GameInputType::BandaiHypershot) {
		log("[Input] Bandai Hyper Shot gun connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::BandaiHyperShot;
	} else if(inputType == GameInputType::BattleBox) {
		log("[Input] Battle Box connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::BattleBox;
	} else if(inputType == GameInputType::TurboFile) {
		log("[Input] Ascii Turbo File connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::AsciiTurboFile;
	} else if(inputType == GameInputType::FamilyTrainerSideA || inputType == GameInputType::FamilyTrainerSideB) {
		log("[Input] Family Trainer mat connected");
		system = GameSystem::Famicom;
		expDevice = ExpansionPortDevice::FamilyTrainerMat;
	} else if(inputType == GameInputType::PowerPadSideA || inputType == GameInputType::PowerPadSideB) {
		log("[Input] Power Pad connected");
		system = GameSystem::NesNtsc;
		controllers[1] = ControllerType::PowerPad;
	} else if(inputType == GameInputType::SnesControllers) {
		log("[Input] 2 SNES controllers connected");
		controllers[0] = ControllerType::SnesController;
		controllers[1] = ControllerType::SnesController;
	} else {
		log("[Input] 2 standard controllers connected");
	}

	isFamicom = (system == GameSystem::Famicom || system == GameSystem::FDS || system == GameSystem::Dendy);
	SetConsoleType(isFamicom ? ConsoleType::Famicom : ConsoleType::Nes);
	for(int i = 0; i < 4; i++) {
		SetControllerType(i, controllers[i]);
	}
	SetExpansionDevice(expDevice);
}

const vector<string> NesModelNames = {
	"Auto",
	"NTSC",
	"PAL",
	"Dendy"
};

const vector<string> ConsoleTypeNames = {
	"Nes",
	"Famicom",
};

const vector<string> ControllerTypeNames = {
	"None",
	"StandardController",
	"Zapper",
	"ArkanoidController",
	"SnesController",
	"PowerPad",
	"SnesMouse",
	"SuborMouse",
	"VsZapper",
	"VbController",
};

const vector<string> ExpansionPortDeviceNames = {
	"None",
	"Zapper",
	"FourPlayerAdapter",
	"ArkanoidController",
	"OekaKidsTablet",
	"FamilyTrainerMat",
	"KonamiHyperShot",
	"FamilyBasicKeyboard",
	"PartyTap",
	"Pachinko",
	"ExcitingBoxing",
	"JissenMahjong",
	"SuborKeyboard",
	"BarcodeBattler",
	"HoriTrack",
	"BandaiHyperShot",
	"AsciiTurboFile",
	"BattleBox",
};
