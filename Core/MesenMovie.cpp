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
#include "NotificationManager.h"
#include "RomData.h"

MesenMovie::MesenMovie(shared_ptr<Console> console)
{
	_console = console;
}

MesenMovie::~MesenMovie()
{
	Stop();
}

void MesenMovie::Stop()
{
	if(_playing) {
		MessageManager::DisplayMessage("Movies", "MovieEnded");

		_console->GetNotificationManager()->SendNotification(ConsoleNotificationType::MovieEnded);
		if(_console->GetSettings()->CheckFlag(EmulationFlags::PauseOnMovieEnd)) {
			_console->GetSettings()->SetFlags(EmulationFlags::Paused);
		}

		_playing = false;
	}
	_console->GetSettings()->SetInputPollScanline(241);
	_console->GetControlManager()->UnregisterInputProvider(this);
}

bool MesenMovie::SetInput(BaseControlDevice *device)
{
	uint32_t inputRowIndex = _console->GetControlManager()->GetPollCounter();

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

void MesenMovie::ProcessNotification(ConsoleNotificationType type, void * parameter)
{
	if(type == ConsoleNotificationType::GameLoaded) {
		_console->GetControlManager()->RegisterInputProvider(this);
	}
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
	
	_console->Pause();
		
	_console->GetBatteryManager()->SetBatteryProvider(shared_from_this());
	_console->GetNotificationManager()->RegisterNotificationListener(shared_from_this());
	ApplySettings();

	//Disable auto-configure input option (otherwise the movie file's input types are ignored)
	bool autoConfigureInput = _console->GetSettings()->CheckFlag(EmulationFlags::AutoConfigureInput);
	_console->GetSettings()->ClearFlags(EmulationFlags::AutoConfigureInput);
	_console->GetControlManager()->SetPollCounter(0);
	bool gameLoaded = LoadGame();
	_console->GetSettings()->SetFlagState(EmulationFlags::AutoConfigureInput, autoConfigureInput);

	if(!gameLoaded) {
		_console->Resume();
		return false;
	}

	stringstream saveStateData;
	if(_reader->GetStream("SaveState.mst", saveStateData)) {
		if(!_console->GetSaveStateManager()->LoadState(saveStateData, true)) {
			_console->Resume();
			return false;
		} else {
			_console->GetControlManager()->SetPollCounter(0);
		}
	}

	_playing = true;

	_console->Resume();

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

	if(_console->GetSettings()->CheckFlag(EmulationFlags::AllowMismatchingSaveState) && _console->GetRomInfo().RomName == gameFile) {
		//Loaded game has the right name, and we don't want to validate the hash values
		_console->PowerCycle();
		return true;
	}

	HashInfo hashInfo;
	hashInfo.Sha1 = sha1Hash;

	VirtualFile romFile = _console->FindMatchingRom(gameFile, hashInfo);
	bool gameLoaded = false;
	if(romFile.IsValid()) {
		VirtualFile patchFile(_movieFile.GetFilePath(), "PatchData.dat");
		if(patchFile.IsValid()) {
			gameLoaded = _console->Initialize(romFile, patchFile);
		} else {
			gameLoaded = _console->Initialize(romFile);
		}
	}

	return gameLoaded;
}

void MesenMovie::ApplySettings()
{
	EmulationSettings* settings = _console->GetSettings();

	NesModel region = FromString(LoadString(_settings, MovieKeys::Region), NesModelNames, NesModel::NTSC);
	ConsoleType consoleType = FromString(LoadString(_settings, MovieKeys::ConsoleType), ConsoleTypeNames, ConsoleType::Nes);
	ControllerType controller1 = FromString(LoadString(_settings, MovieKeys::Controller1), ControllerTypeNames, ControllerType::None);
	ControllerType controller2 = FromString(LoadString(_settings, MovieKeys::Controller2), ControllerTypeNames, ControllerType::None);
	ControllerType controller3 = FromString(LoadString(_settings, MovieKeys::Controller3), ControllerTypeNames, ControllerType::None);
	ControllerType controller4 = FromString(LoadString(_settings, MovieKeys::Controller4), ControllerTypeNames, ControllerType::None);
	ExpansionPortDevice expansionDevice = FromString<ExpansionPortDevice>(LoadString(_settings, MovieKeys::ExpansionDevice), ExpansionPortDeviceNames, ExpansionPortDevice::None);

	settings->SetNesModel(region);
	settings->SetConsoleType(consoleType);
	settings->SetControllerType(0, controller1);
	settings->SetControllerType(1, controller2);
	settings->SetControllerType(2, controller3);
	settings->SetControllerType(3, controller4);
	settings->SetExpansionDevice(expansionDevice);

	uint32_t ramPowerOnState = LoadInt(_settings, MovieKeys::RamPowerOnState);
	if(ramPowerOnState == 0xFF) {
		settings->SetRamPowerOnState(RamPowerOnState::AllOnes);
	} else {
		settings->SetRamPowerOnState(RamPowerOnState::AllZeros);
	}

	settings->SetInputPollScanline(LoadInt(_settings, MovieKeys::InputPollScanline, 240));

	settings->SetZapperDetectionRadius(LoadInt(_settings, MovieKeys::ZapperDetectionRadius));
	
	uint32_t cpuClockRate = LoadInt(_settings, MovieKeys::CpuClockRate);
	if(cpuClockRate != 100) {
		bool adjustApu = LoadBool(_settings, MovieKeys::OverclockAdjustApu);
		settings->SetOverclockRate(cpuClockRate, adjustApu);
	} else {
		settings->SetOverclockRate(100, true);
	}

	settings->SetPpuNmiConfig(
		LoadInt(_settings, MovieKeys::ExtraScanlinesBeforeNmi),
		LoadInt(_settings, MovieKeys::ExtraScanlinesAfterNmi)
	);

	settings->SetFlagState(EmulationFlags::DisablePpu2004Reads, LoadBool(_settings, MovieKeys::DisablePpu2004Reads));
	settings->SetFlagState(EmulationFlags::DisablePaletteRead, LoadBool(_settings, MovieKeys::DisablePaletteRead));
	settings->SetFlagState(EmulationFlags::DisableOamAddrBug, LoadBool(_settings, MovieKeys::DisableOamAddrBug));
	settings->SetFlagState(EmulationFlags::UseNes101Hvc101Behavior, LoadBool(_settings, MovieKeys::UseNes101Hvc101Behavior));
	settings->SetFlagState(EmulationFlags::EnableOamDecay, LoadBool(_settings, MovieKeys::EnableOamDecay));
	settings->SetFlagState(EmulationFlags::DisablePpuReset, LoadBool(_settings, MovieKeys::DisablePpuReset));

	//VS System flags
	settings->SetDipSwitches(HexUtilities::FromHex(LoadString(_settings, MovieKeys::DipSwitches)));

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
	_console->GetCheatManager()->SetCheats(cheats);
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
