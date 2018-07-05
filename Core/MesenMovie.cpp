#include "stdafx.h"
#include "../Utilities/ZipReader.h"
#include "../Utilities/StringUtilities.h"
#include "../Utilities/HexUtilities.h"
#include "MesenMovie.h"
#include "MessageManager.h"
#include "ControlManager.h"
#include "BaseControlDevice.h"
#include "Console.h"
#include "SaveStateManager.h"
#include "CheatManager.h"
#include "MovieRecorder.h"
#include "BatteryManager.h"
#include "VirtualFile.h"

MesenMovie::MesenMovie()
{
}

MesenMovie::~MesenMovie()
{
	Stop();
}

void MesenMovie::Stop()
{
	if(_playing) {
		EndMovie();
		_playing = false;
	}
	EmulationSettings::SetInputPollScanline(241);
	ControlManager::UnregisterInputProvider(this);
}

bool MesenMovie::SetInput(BaseControlDevice *device)
{
	uint32_t inputRowIndex = ControlManager::GetPollCounter();

	if(_inputData.size() > inputRowIndex && _inputData[inputRowIndex].size() > _deviceIndex) {
		device->SetTextState(_inputData[inputRowIndex][_deviceIndex]);

		_deviceIndex++;
		if(_deviceIndex >= _inputData[inputRowIndex].size()) {
			//Move to the next frame's data
			_deviceIndex = 0;
		}
	} else {
		Stop();
	}
	return true;
}

bool MesenMovie::IsPlaying()
{
	return _playing;
}

vector<uint8_t> MesenMovie::LoadBattery(string extension)
{
	vector<uint8_t> batteryData;
	_reader->ExtractFile("Battery" + extension, batteryData);
	return batteryData;
}

bool MesenMovie::Play(VirtualFile &file)
{
	_movieFile = file;

	std::stringstream ss;
	file.ReadFile(ss);

	_reader.reset(new ZipReader());
	_reader->LoadArchive(ss);

	stringstream settingsData, inputData;
	if(!_reader->GetStream("GameSettings.txt", settingsData)) {
		MessageManager::Log("[Movie] File not found: GameSettings.txt");
		return false;
	}
	if(!_reader->GetStream("Input.txt", inputData)) {
		MessageManager::Log("[Movie] File not found: Input.txt");
		return false;
	}

	while(inputData) {
		string line;
		std::getline(inputData, line);
		if(line.substr(0, 1) == "|") {
			_inputData.push_back(StringUtilities::Split(line.substr(1), '|'));
		}
	}

	_deviceIndex = 0;

	ParseSettings(settingsData);
	
	Console::Pause();
		
	BatteryManager::SetBatteryProvider(shared_from_this());
	ControlManager::RegisterInputProvider(this);
	ApplySettings();

	//Disable auto-configure input option (otherwise the movie file's input types are ignored)
	bool autoConfigureInput = EmulationSettings::CheckFlag(EmulationFlags::AutoConfigureInput);
	EmulationSettings::ClearFlags(EmulationFlags::AutoConfigureInput);
	ControlManager::ResetPollCounter();
	bool gameLoaded = LoadGame();
	EmulationSettings::SetFlagState(EmulationFlags::AutoConfigureInput, autoConfigureInput);

	if(!gameLoaded) {
		Console::Resume();
		return false;
	}

	stringstream saveStateData;
	if(_reader->GetStream("SaveState.mst", saveStateData)) {
		if(!SaveStateManager::LoadState(saveStateData, true)) {
			Console::Resume();
			return false;
		} else {
			ControlManager::ResetPollCounter();
		}
	}

	_playing = true;

	Console::Resume();

	return true;
}

template<typename T>
T FromString(string name, const vector<string> &enumNames, T defaultValue)
{
	for(size_t i = 0; i < enumNames.size(); i++) {
		if(name == enumNames[i]) {
			return (T)i;
		}
	}
	return defaultValue;
}

void MesenMovie::ParseSettings(stringstream &data)
{
	while(!data.eof()) {
		string line;
		std::getline(data, line);

		if(!line.empty()) {
			size_t index = line.find_first_of(' ');
			if(index != string::npos) {
				string name = line.substr(0, index);
				string value = line.substr(index + 1);

				if(name == "Cheat") {
					_cheats.push_back(value);
				} else {
					_settings[name] = value;
				}
			}
		}
	}
}

bool MesenMovie::LoadGame()
{
	string mesenVersion = LoadString(_settings, MovieKeys::MesenVersion);
	string gameFile = LoadString(_settings, MovieKeys::GameFile);
	string sha1Hash = LoadString(_settings, MovieKeys::Sha1);
	//string patchFile = LoadString(_settings, MovieKeys::PatchFile);
	//string patchFileSha1 = LoadString(_settings, MovieKeys::PatchFileSha1);
	//string patchedRomSha1 = LoadString(_settings, MovieKeys::PatchedRomSha1);

	if(EmulationSettings::CheckFlag(EmulationFlags::AllowMismatchingSaveState) && Console::GetMapperInfo().RomName == gameFile) {
		//Loaded game has the right name, and we don't want to validate the hash values
		Console::GetInstance()->PowerCycle();
		return true;
	}

	HashInfo hashInfo;
	hashInfo.Sha1Hash = sha1Hash;

	VirtualFile romFile = Console::FindMatchingRom(gameFile, hashInfo);
	bool gameLoaded = false;
	if(romFile.IsValid()) {
		VirtualFile patchFile(_movieFile.GetFilePath(), "PatchData.dat");
		if(patchFile.IsValid()) {
			gameLoaded = Console::LoadROM(romFile, patchFile);
		} else {
			gameLoaded = Console::LoadROM(romFile);
		}
	}

	return gameLoaded;
}

void MesenMovie::ApplySettings()
{
	NesModel region = FromString(LoadString(_settings, MovieKeys::Region), NesModelNames, NesModel::NTSC);
	ConsoleType consoleType = FromString(LoadString(_settings, MovieKeys::ConsoleType), ConsoleTypeNames, ConsoleType::Nes);
	ControllerType controller1 = FromString(LoadString(_settings, MovieKeys::Controller1), ControllerTypeNames, ControllerType::None);
	ControllerType controller2 = FromString(LoadString(_settings, MovieKeys::Controller2), ControllerTypeNames, ControllerType::None);
	ControllerType controller3 = FromString(LoadString(_settings, MovieKeys::Controller3), ControllerTypeNames, ControllerType::None);
	ControllerType controller4 = FromString(LoadString(_settings, MovieKeys::Controller4), ControllerTypeNames, ControllerType::None);
	ExpansionPortDevice expansionDevice = FromString<ExpansionPortDevice>(LoadString(_settings, MovieKeys::ExpansionDevice), ExpansionPortDeviceNames, ExpansionPortDevice::None);

	EmulationSettings::SetNesModel(region);
	EmulationSettings::SetConsoleType(consoleType);
	EmulationSettings::SetControllerType(0, controller1);
	EmulationSettings::SetControllerType(1, controller2);
	EmulationSettings::SetControllerType(2, controller3);
	EmulationSettings::SetControllerType(3, controller4);
	EmulationSettings::SetExpansionDevice(expansionDevice);

	uint32_t ramPowerOnState = LoadInt(_settings, MovieKeys::RamPowerOnState);
	if(ramPowerOnState == 0xFF) {
		EmulationSettings::SetRamPowerOnState(RamPowerOnState::AllOnes);
	} else {
		EmulationSettings::SetRamPowerOnState(RamPowerOnState::AllZeros);
	}

	EmulationSettings::SetInputPollScanline(LoadInt(_settings, MovieKeys::InputPollScanline, 240));

	EmulationSettings::SetZapperDetectionRadius(LoadInt(_settings, MovieKeys::ZapperDetectionRadius));
	
	uint32_t cpuClockRate = LoadInt(_settings, MovieKeys::CpuClockRate);
	if(cpuClockRate != 100) {
		bool adjustApu = LoadBool(_settings, MovieKeys::OverclockAdjustApu);
		EmulationSettings::SetOverclockRate(cpuClockRate, adjustApu);
	} else {
		EmulationSettings::SetOverclockRate(100, true);
	}

	EmulationSettings::SetPpuNmiConfig(
		LoadInt(_settings, MovieKeys::ExtraScanlinesBeforeNmi),
		LoadInt(_settings, MovieKeys::ExtraScanlinesAfterNmi)
	);

	EmulationSettings::SetFlagState(EmulationFlags::DisablePpu2004Reads, LoadBool(_settings, MovieKeys::DisablePpu2004Reads));
	EmulationSettings::SetFlagState(EmulationFlags::DisablePaletteRead, LoadBool(_settings, MovieKeys::DisablePaletteRead));
	EmulationSettings::SetFlagState(EmulationFlags::DisableOamAddrBug, LoadBool(_settings, MovieKeys::DisableOamAddrBug));
	EmulationSettings::SetFlagState(EmulationFlags::UseNes101Hvc101Behavior, LoadBool(_settings, MovieKeys::UseNes101Hvc101Behavior));
	EmulationSettings::SetFlagState(EmulationFlags::EnableOamDecay, LoadBool(_settings, MovieKeys::EnableOamDecay));
	EmulationSettings::SetFlagState(EmulationFlags::DisablePpuReset, LoadBool(_settings, MovieKeys::DisablePpuReset));

	//VS System flags
	EmulationSettings::SetPpuModel(FromString(LoadString(_settings, MovieKeys::PpuModel), PpuModelNames, PpuModel::Ppu2C02));
	EmulationSettings::SetDipSwitches(HexUtilities::FromHex(LoadString(_settings, MovieKeys::DipSwitches)));

	LoadCheats();
}

uint32_t MesenMovie::LoadInt(std::unordered_map<string, string> &settings, string name, uint32_t defaultValue)
{
	auto result = settings.find(name);
	if(result != settings.end()) {
		try {
			return (uint32_t)std::stoul(result->second);
		} catch(std::exception ex) {
			MessageManager::Log("[Movies] Invalid value for tag: " + name);
			return defaultValue;
		}
	} else {
		return defaultValue;
	}
}

bool MesenMovie::LoadBool(std::unordered_map<string, string> &settings, string name)
{
	auto result = settings.find(name);
	if(result != settings.end()) {
		if(result->second == "true") {
			return true;
		} else if(result->second == "false") {
			return false;
		} else {			
			MessageManager::Log("[Movies] Invalid value for tag: " + name);
			return false;
		}
	} else {
		return false;
	}
}

string MesenMovie::LoadString(std::unordered_map<string, string> &settings, string name)
{
	auto result = settings.find(name);
	if(result != settings.end()) {
		return result->second;
	} else {
		return "";
	}
}

void MesenMovie::LoadCheats()
{
	vector<CodeInfo> cheats;
	for(string cheatData : _cheats) {
		CodeInfo code;
		if(LoadCheat(cheatData, code)) {
			cheats.push_back(code);
		}
	}
	CheatManager::SetCheats(cheats);
}

bool MesenMovie::LoadCheat(string cheatData, CodeInfo &code)
{
	vector<string> data = StringUtilities::Split(cheatData, ' ');

	if(data.size() >= 3) {
		uint32_t address = HexUtilities::FromHex(data[0]);
		uint8_t value = HexUtilities::FromHex(data[1]);
		bool relativeAddress = data[2] == "true";
		int32_t compareValue = data.size() > 3 ? HexUtilities::FromHex(data[3]) : -1;

		code.Address = address;
		code.Value = value;
		code.IsRelativeAddress = relativeAddress;
		code.CompareValue = compareValue;
		return true;
	} else {
		MessageManager::Log("[Movie] Invalid cheat definition: " + cheatData);
	}
	return false;
}
