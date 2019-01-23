#include "stdafx.h"
#include "ControlManager.h"
#include "BaseMapper.h"
#include "EmulationSettings.h"
#include "Console.h"
#include "GameServerConnection.h"
#include "MemoryManager.h"
#include "IKeyManager.h"
#include "IInputProvider.h"
#include "IInputRecorder.h"
#include "BatteryManager.h"
#include "StandardController.h"
#include "Zapper.h"
#include "ArkanoidController.h"
#include "OekaKidsTablet.h"
#include "FourScore.h"
#include "SnesController.h"
#include "SnesMouse.h"
#include "PowerPad.h"
#include "FamilyMatTrainer.h"
#include "KonamiHyperShot.h"
#include "FamilyBasicKeyboard.h"
#include "FamilyBasicDataRecorder.h"
#include "PartyTap.h"
#include "PachinkoController.h"
#include "ExcitingBoxingController.h"
#include "SuborKeyboard.h"
#include "SuborMouse.h"
#include "JissenMahjongController.h"
#include "BarcodeBattlerReader.h"
#include "HoriTrack.h"
#include "BandaiHyperShot.h"
#include "VsZapper.h"
#include "AsciiTurboFile.h"
#include "BattleBox.h"

ControlManager::ControlManager(shared_ptr<Console> console, shared_ptr<BaseControlDevice> systemActionManager, shared_ptr<BaseControlDevice> mapperControlDevice)
{
	_console = console;
	_systemActionManager = systemActionManager;
	_mapperControlDevice = mapperControlDevice;
	_pollCounter = 0;
}

ControlManager::~ControlManager()
{
}

void ControlManager::RegisterInputProvider(IInputProvider* provider)
{
	auto lock = _deviceLock.AcquireSafe();
	_inputProviders.push_back(provider);
}

void ControlManager::UnregisterInputProvider(IInputProvider* provider)
{
	auto lock = _deviceLock.AcquireSafe();
	vector<IInputProvider*> &vec = _inputProviders;
	vec.erase(std::remove(vec.begin(), vec.end(), provider), vec.end());
}

void ControlManager::RegisterInputRecorder(IInputRecorder* provider)
{
	auto lock = _deviceLock.AcquireSafe();
	_inputRecorders.push_back(provider);
}

void ControlManager::UnregisterInputRecorder(IInputRecorder* provider)
{
	auto lock = _deviceLock.AcquireSafe();
	vector<IInputRecorder*> &vec = _inputRecorders;
	vec.erase(std::remove(vec.begin(), vec.end(), provider), vec.end());
}

vector<ControlDeviceState> ControlManager::GetPortStates()
{
	auto lock = _deviceLock.AcquireSafe();

	vector<ControlDeviceState> states;
	for(int i = 0; i < 4; i++) {
		shared_ptr<BaseControlDevice> device = GetControlDevice(i);
		if(device) {
			states.push_back(device->GetRawState());
		} else {
			states.push_back(ControlDeviceState());
		}
	}
	return states;
}

shared_ptr<BaseControlDevice> ControlManager::GetControlDevice(uint8_t port)
{
	auto lock = _deviceLock.AcquireSafe();

	auto result = std::find_if(_controlDevices.begin(), _controlDevices.end(), [port](const shared_ptr<BaseControlDevice> control) { return control->GetPort() == port; });
	if(result != _controlDevices.end()) {
		return *result;
	}
	return nullptr;
}

vector<shared_ptr<BaseControlDevice>> ControlManager::GetControlDevices()
{
	return _controlDevices;
}

void ControlManager::RegisterControlDevice(shared_ptr<BaseControlDevice> controlDevice)
{
	_controlDevices.push_back(controlDevice);
}

ControllerType ControlManager::GetControllerType(uint8_t port)
{
	return _console->GetSettings()->GetControllerType(port);
}

shared_ptr<BaseControlDevice> ControlManager::CreateControllerDevice(ControllerType type, uint8_t port, shared_ptr<Console> console)
{
	shared_ptr<BaseControlDevice> device;

	switch(type) {
		case ControllerType::None: break;
		case ControllerType::StandardController: device.reset(new StandardController(console, port, console->GetSettings()->GetControllerKeys(port))); break;
		case ControllerType::Zapper: device.reset(new Zapper(console, port)); break;
		case ControllerType::ArkanoidController: device.reset(new ArkanoidController(console, port)); break;
		case ControllerType::SnesController: device.reset(new SnesController(console, port, console->GetSettings()->GetControllerKeys(port))); break;
		case ControllerType::PowerPad: device.reset(new PowerPad(console, port, console->GetSettings()->GetControllerKeys(port))); break;
		case ControllerType::SnesMouse: device.reset(new SnesMouse(console, port)); break;
		case ControllerType::SuborMouse: device.reset(new SuborMouse(console, port)); break;
		case ControllerType::VsZapper: device.reset(new VsZapper(console, port)); break;
	}
	
	return device;
}

shared_ptr<BaseControlDevice> ControlManager::CreateExpansionDevice(ExpansionPortDevice type, shared_ptr<Console> console)
{
	shared_ptr<BaseControlDevice> device;

	switch(type) {
		case ExpansionPortDevice::Zapper: device.reset(new Zapper(console, BaseControlDevice::ExpDevicePort)); break;
		case ExpansionPortDevice::ArkanoidController: device.reset(new ArkanoidController(console, BaseControlDevice::ExpDevicePort)); break;
		case ExpansionPortDevice::OekaKidsTablet: device.reset(new OekaKidsTablet(console)); break;
		case ExpansionPortDevice::FamilyTrainerMat: device.reset(new FamilyMatTrainer(console, console->GetSettings()->GetControllerKeys(0))); break;
		case ExpansionPortDevice::KonamiHyperShot: device.reset(new KonamiHyperShot(console, console->GetSettings()->GetControllerKeys(0), console->GetSettings()->GetControllerKeys(1))); break;
		case ExpansionPortDevice::FamilyBasicKeyboard: device.reset(new FamilyBasicKeyboard(console, console->GetSettings()->GetControllerKeys(0))); break;
		case ExpansionPortDevice::PartyTap: device.reset(new PartyTap(console, console->GetSettings()->GetControllerKeys(0))); break;
		case ExpansionPortDevice::Pachinko: device.reset(new PachinkoController(console, console->GetSettings()->GetControllerKeys(0))); break;
		case ExpansionPortDevice::ExcitingBoxing: device.reset(new ExcitingBoxingController(console, console->GetSettings()->GetControllerKeys(0))); break;
		case ExpansionPortDevice::JissenMahjong: device.reset(new JissenMahjongController(console, console->GetSettings()->GetControllerKeys(0))); break;
		case ExpansionPortDevice::SuborKeyboard: device.reset(new SuborKeyboard(console, console->GetSettings()->GetControllerKeys(0))); break;
		case ExpansionPortDevice::BarcodeBattler: device.reset(new BarcodeBattlerReader(console)); break;
		case ExpansionPortDevice::HoriTrack: device.reset(new HoriTrack(console, console->GetSettings()->GetControllerKeys(0))); break;
		case ExpansionPortDevice::BandaiHyperShot: device.reset(new BandaiHyperShot(console, console->GetSettings()->GetControllerKeys(0))); break;
		case ExpansionPortDevice::AsciiTurboFile: device.reset(new AsciiTurboFile(console)); break;
		case ExpansionPortDevice::BattleBox: device.reset(new BattleBox(console)); break;

		case ExpansionPortDevice::FourPlayerAdapter:
		default: break;
	}

	return device;
}

void ControlManager::UpdateControlDevices()
{
	auto lock = _deviceLock.AcquireSafe();
	EmulationSettings* settings = _console->GetSettings();

	//Reset update flag
	settings->NeedControllerUpdate();

	bool hadKeyboard = HasKeyboard();

	_controlDevices.clear();

	RegisterControlDevice(_systemActionManager);

	bool fourScore = settings->CheckFlag(EmulationFlags::HasFourScore);
	ConsoleType consoleType = settings->GetConsoleType();
	ExpansionPortDevice expansionDevice = settings->GetExpansionDevice();

	if(consoleType != ConsoleType::Famicom) {
		expansionDevice = ExpansionPortDevice::None;
	} else if(expansionDevice != ExpansionPortDevice::FourPlayerAdapter) {
		fourScore = false;
	}

	for(int i = 0; i < (fourScore ? 4 : 2); i++) {
		shared_ptr<BaseControlDevice> device = CreateControllerDevice(GetControllerType(i), i, _console);
		if(device) {
			RegisterControlDevice(device);
		}
	}

	if(fourScore && consoleType == ConsoleType::Nes) {
		//FourScore is only used to provide the signature for reads past the first 16 reads
		RegisterControlDevice(shared_ptr<FourScore>(new FourScore(_console)));
	}

	shared_ptr<BaseControlDevice> expDevice = CreateExpansionDevice(expansionDevice, _console);
	if(expDevice) {
		RegisterControlDevice(expDevice);
	}

	bool hasKeyboard = HasKeyboard();
	if(!hasKeyboard) {
		settings->DisableKeyboardMode();
	} else if(!hadKeyboard && hasKeyboard) {
		settings->EnableKeyboardMode();
	}

	if(_mapperControlDevice) {
		RegisterControlDevice(_mapperControlDevice);
	}

	if(std::dynamic_pointer_cast<FamilyBasicKeyboard>(expDevice)) {
		//Automatically connect the data recorder if the keyboard is connected
		RegisterControlDevice(shared_ptr<FamilyBasicDataRecorder>(new FamilyBasicDataRecorder(_console)));
	}
}

bool ControlManager::HasKeyboard()
{
	shared_ptr<BaseControlDevice> expDevice = GetControlDevice(BaseControlDevice::ExpDevicePort);
	return expDevice && expDevice->IsKeyboard();
}

uint8_t ControlManager::GetOpenBusMask(uint8_t port)
{
	//"In the NES and Famicom, the top three (or five) bits are not driven, and so retain the bits of the previous byte on the bus. 
	//Usually this is the most significant byte of the address of the controller port - 0x40.
	//Paperboy relies on this behavior and requires that reads from the controller ports return exactly $40 or $41 as appropriate."

	switch(_console->GetSettings()->GetConsoleType()) {
		default:
		case ConsoleType::Nes:
			if(_console->GetSettings()->CheckFlag(EmulationFlags::UseNes101Hvc101Behavior)) {
				return port == 0 ? 0xE4 : 0xE0;
			} else {
				return 0xE0;
			}

		case ConsoleType::Famicom:
			if(_console->GetSettings()->CheckFlag(EmulationFlags::UseNes101Hvc101Behavior)) {
				return port == 0 ? 0xF8 : 0xE0;
			} else {
				return port == 0 ? 0xF8 : 0xE0;
			}
	}
}

void ControlManager::RemapControllerButtons()
{
	//Used by VS System games
}

void ControlManager::UpdateInputState()
{
	if(_isLagging) {
		_lagCounter++;
	} else {
		_isLagging = true;
	}

	KeyManager::RefreshKeyState();

	auto lock = _deviceLock.AcquireSafe();

	//string log = "";
	for(shared_ptr<BaseControlDevice> &device : _controlDevices) {
		device->ClearState();

		bool inputSet = false;
		for(size_t i = 0; i < _inputProviders.size(); i++) {
			IInputProvider* provider = _inputProviders[i];
			if(provider->SetInput(device.get())) {
				inputSet = true;
				break;
			}
		}

		if(!inputSet) {
			device->SetStateFromInput();
		}

		device->OnAfterSetState();
		//log += "|" + device->GetTextState();
	}

	shared_ptr<Debugger> debugger = _console->GetDebugger(false);
	if(debugger) {
		debugger->ProcessEvent(EventType::InputPolled);
	}

	for(IInputRecorder* recorder : _inputRecorders) {
		recorder->RecordInput(_controlDevices);
	}

	//Used by VS System games
	RemapControllerButtons();

	//MessageManager::Log(log);

	_pollCounter++;
}

uint32_t ControlManager::GetLagCounter()
{
	return _lagCounter;
}

void ControlManager::ResetLagCounter()
{
	_lagCounter = 0;
}

uint32_t ControlManager::GetPollCounter()
{
	return ControlManager::_pollCounter;
}

void ControlManager::SetPollCounter(uint32_t value)
{
	_pollCounter = value;
}

uint8_t ControlManager::ReadRAM(uint16_t addr)
{
	//Used for lag counter - any frame where the input is read does not count as lag
	_isLagging = false;

	uint8_t value = _console->GetMemoryManager()->GetOpenBus(GetOpenBusMask(addr - 0x4016));
	for(shared_ptr<BaseControlDevice> &device : _controlDevices) {
		value |= device->ReadRAM(addr);
	}

	return value;
}

void ControlManager::WriteRAM(uint16_t addr, uint8_t value)
{
	for(shared_ptr<BaseControlDevice> &device : _controlDevices) {
		device->WriteRAM(addr, value);
	}
}

void ControlManager::Reset(bool softReset)
{
	ResetLagCounter();
}

void ControlManager::StreamState(bool saving)
{
	//Restore controllers that were being used at the time the snapshot was made
	//This is particularely important to ensure proper sync during NetPlay
	EmulationSettings* settings = _console->GetSettings();
	ControllerType controllerTypes[4];
	NesModel nesModel;
	ExpansionPortDevice expansionDevice;
	ConsoleType consoleType;
	bool hasFourScore = false;
	bool useNes101Hvc101Behavior = false;
	uint32_t zapperDetectionRadius = 0;
	if(saving) {
		nesModel = _console->GetModel();
		expansionDevice = settings->GetExpansionDevice();
		consoleType = settings->GetConsoleType();
		hasFourScore = settings->CheckFlag(EmulationFlags::HasFourScore);
		useNes101Hvc101Behavior = settings->CheckFlag(EmulationFlags::UseNes101Hvc101Behavior);
		zapperDetectionRadius = settings->GetZapperDetectionRadius();
		for(int i = 0; i < 4; i++) {
			controllerTypes[i] = settings->GetControllerType(i);
		}
	}

	ArrayInfo<ControllerType> types = { controllerTypes, 4 };
	Stream(nesModel, expansionDevice, consoleType, types, hasFourScore, useNes101Hvc101Behavior, zapperDetectionRadius, _lagCounter, _pollCounter);

	if(!saving) {
		settings->SetNesModel(nesModel);
		settings->SetExpansionDevice(expansionDevice);
		settings->SetConsoleType(consoleType);
		for(int i = 0; i < 4; i++) {
			settings->SetControllerType(i, controllerTypes[i]);
		}

		settings->SetZapperDetectionRadius(zapperDetectionRadius);
		settings->SetFlagState(EmulationFlags::HasFourScore, hasFourScore);
		settings->SetFlagState(EmulationFlags::UseNes101Hvc101Behavior, useNes101Hvc101Behavior);

		UpdateControlDevices();
	}

	for(uint8_t i = 0; i < _controlDevices.size(); i++) {
		SnapshotInfo device{ _controlDevices[i].get() };
		Stream(device);
	}
}
