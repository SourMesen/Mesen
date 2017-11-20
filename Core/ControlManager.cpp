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

ControlManager* ControlManager::_instance = nullptr;
vector<IInputRecorder*> ControlManager::_inputRecorders;
vector<IInputProvider*> ControlManager::_inputProviders;
SimpleLock ControlManager::_deviceLock;

ControlManager::ControlManager(shared_ptr<BaseControlDevice> systemActionManager, shared_ptr<BaseControlDevice> mapperControlDevice)
{
	_systemActionManager = systemActionManager;
	_mapperControlDevice = mapperControlDevice;
	_instance = this;
}

ControlManager::~ControlManager()
{
	if(_instance == this) {
		_instance = nullptr;
	}
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

	auto result = std::find_if(_instance->_controlDevices.begin(), _instance->_controlDevices.end(), [port](const shared_ptr<BaseControlDevice> control) { return control->GetPort() == port; });
	if(result != _instance->_controlDevices.end()) {
		return *result;
	}
	return nullptr;
}

void ControlManager::RegisterControlDevice(shared_ptr<BaseControlDevice> controlDevice)
{
	_controlDevices.push_back(controlDevice);
}

ControllerType ControlManager::GetControllerType(uint8_t port)
{
	return EmulationSettings::GetControllerType(port);
}

shared_ptr<BaseControlDevice> ControlManager::CreateControllerDevice(ControllerType type, uint8_t port)
{
	shared_ptr<BaseControlDevice> device;

	switch(type) {
		case ControllerType::StandardController: device.reset(new StandardController(port, EmulationSettings::GetControllerKeys(port))); break;
		case ControllerType::Zapper: device.reset(new Zapper(port)); break;
		case ControllerType::ArkanoidController: device.reset(new ArkanoidController(port)); break;
		case ControllerType::SnesController: device.reset(new SnesController(port, EmulationSettings::GetControllerKeys(port))); break;
		case ControllerType::PowerPad: device.reset(new PowerPad(port, EmulationSettings::GetControllerKeys(port))); break;
		case ControllerType::SnesMouse: device.reset(new SnesMouse(port)); break;
		case ControllerType::SuborMouse: device.reset(new SuborMouse(port)); break;
		case ControllerType::VsZapper: device.reset(new VsZapper(port)); break;
	}

	return device;
}

shared_ptr<BaseControlDevice> ControlManager::CreateExpansionDevice(ExpansionPortDevice type)
{
	shared_ptr<BaseControlDevice> device;

	switch(type) {
		case ExpansionPortDevice::Zapper: device.reset(new Zapper(BaseControlDevice::ExpDevicePort)); break;
		case ExpansionPortDevice::ArkanoidController: device.reset(new ArkanoidController(BaseControlDevice::ExpDevicePort)); break;
		case ExpansionPortDevice::OekaKidsTablet: device.reset(new OekaKidsTablet()); break;
		case ExpansionPortDevice::FamilyTrainerMat: device.reset(new FamilyMatTrainer(EmulationSettings::GetControllerKeys(0))); break;
		case ExpansionPortDevice::KonamiHyperShot: device.reset(new KonamiHyperShot(EmulationSettings::GetControllerKeys(0), EmulationSettings::GetControllerKeys(1))); break;
		case ExpansionPortDevice::FamilyBasicKeyboard: device.reset(new FamilyBasicKeyboard(EmulationSettings::GetControllerKeys(0))); break; //TODO: tape reader
		case ExpansionPortDevice::PartyTap: device.reset(new PartyTap(EmulationSettings::GetControllerKeys(0))); break;
		case ExpansionPortDevice::Pachinko: device.reset(new PachinkoController(EmulationSettings::GetControllerKeys(0))); break;
		case ExpansionPortDevice::ExcitingBoxing: device.reset(new ExcitingBoxingController(EmulationSettings::GetControllerKeys(0))); break;
		case ExpansionPortDevice::JissenMahjong: device.reset(new JissenMahjongController(EmulationSettings::GetControllerKeys(0))); break;
		case ExpansionPortDevice::SuborKeyboard: device.reset(new SuborKeyboard(EmulationSettings::GetControllerKeys(0))); break;
		case ExpansionPortDevice::BarcodeBattler: device.reset(new BarcodeBattlerReader()); break;
		case ExpansionPortDevice::HoriTrack: device.reset(new HoriTrack(EmulationSettings::GetControllerKeys(0))); break;
		case ExpansionPortDevice::BandaiHyperShot: device.reset(new BandaiHyperShot(EmulationSettings::GetControllerKeys(0))); break;
		case ExpansionPortDevice::AsciiTurboFile: device.reset(new AsciiTurboFile()); break;
		case ExpansionPortDevice::BattleBox: device.reset(new BattleBox()); break;

		case ExpansionPortDevice::FourPlayerAdapter:
		default: break;
	}

	return device;
}

void ControlManager::UpdateControlDevices()
{
	auto lock = _deviceLock.AcquireSafe();

	//Reset update flag
	EmulationSettings::NeedControllerUpdate();

	ControlManager::_controlDevices.clear();

	ControlManager::RegisterControlDevice(_systemActionManager);

	bool fourScore = EmulationSettings::CheckFlag(EmulationFlags::HasFourScore);
	ConsoleType consoleType = EmulationSettings::GetConsoleType();
	ExpansionPortDevice expansionDevice = EmulationSettings::GetExpansionDevice();

	if(consoleType != ConsoleType::Famicom) {
		expansionDevice = ExpansionPortDevice::None;
	} else if(expansionDevice != ExpansionPortDevice::FourPlayerAdapter) {
		fourScore = false;
	}

	for(int i = 0; i < (fourScore ? 4 : 2); i++) {
		shared_ptr<BaseControlDevice> device = CreateControllerDevice(GetControllerType(i), i);
		if(device) {
			ControlManager::RegisterControlDevice(device);
		}
	}

	if(fourScore && consoleType == ConsoleType::Nes) {
		//FourScore is only used to provide the signature for reads past the first 16 reads
		ControlManager::RegisterControlDevice(shared_ptr<FourScore>(new FourScore()));
	}

	shared_ptr<BaseControlDevice> expDevice = CreateExpansionDevice(expansionDevice);
	if(expDevice) {
		ControlManager::RegisterControlDevice(expDevice);
	}

	if(_mapperControlDevice) {
		ControlManager::RegisterControlDevice(_mapperControlDevice);
	}
}

uint8_t ControlManager::GetOpenBusMask(uint8_t port)
{
	//"In the NES and Famicom, the top three (or five) bits are not driven, and so retain the bits of the previous byte on the bus. 
	//Usually this is the most significant byte of the address of the controller port - 0x40.
	//Paperboy relies on this behavior and requires that reads from the controller ports return exactly $40 or $41 as appropriate."

	switch(EmulationSettings::GetConsoleType()) {
		default:
		case ConsoleType::Nes:
			if(EmulationSettings::CheckFlag(EmulationFlags::UseNes101Hvc101Behavior)) {
				return port == 0 ? 0xE4 : 0xE0;
			} else {
				return 0xE0;
			}

		case ConsoleType::Famicom:
			if(EmulationSettings::CheckFlag(EmulationFlags::UseNes101Hvc101Behavior)) {
				return port == 0 ? 0xF8 : 0xE0;
			} else {
				return port == 0 ? 0xF8 : 0xE0;
			}
	}
}

void ControlManager::UpdateInputState()
{
	if(_isLagging) {
		_lagCounter++;
	} else {
		_isLagging = true;
	}

	auto lock = _deviceLock.AcquireSafe();

	string log = "";
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

		shared_ptr<Debugger> debugger = Console::GetInstance()->GetDebugger(false);
		if(debugger) {
			debugger->ProcessEvent(EventType::InputPolled);
		}

		log += "|" + device->GetTextState();
		
		for(IInputRecorder* recorder : _inputRecorders) {
			recorder->RecordInput(device.get());
		}
	}
	
	for(IInputRecorder* recorder : _inputRecorders) {
		recorder->EndFrame();
	}

	MessageManager::Log(log);
}

uint32_t ControlManager::GetLagCounter()
{
	return _lagCounter;
}

void ControlManager::ResetLagCounter()
{
	_lagCounter = 0;
}

uint8_t ControlManager::ReadRAM(uint16_t addr)
{
	//Used for lag counter - any frame where the input is read does not count as lag
	_isLagging = false;

	uint8_t value = MemoryManager::GetOpenBus(GetOpenBusMask(addr - 0x4016));
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
	ControllerType controllerTypes[4];
	NesModel nesModel;
	ExpansionPortDevice expansionDevice;
	ConsoleType consoleType;
	bool hasFourScore = false;
	bool useNes101Hvc101Behavior = false;
	uint32_t zapperDetectionRadius = 0;
	if(saving) {
		nesModel = Console::GetModel();
		expansionDevice = EmulationSettings::GetExpansionDevice();
		consoleType = EmulationSettings::GetConsoleType();
		hasFourScore = EmulationSettings::CheckFlag(EmulationFlags::HasFourScore);
		useNes101Hvc101Behavior = EmulationSettings::CheckFlag(EmulationFlags::UseNes101Hvc101Behavior);
		zapperDetectionRadius = EmulationSettings::GetZapperDetectionRadius();
		for(int i = 0; i < 4; i++) {
			controllerTypes[i] = EmulationSettings::GetControllerType(i);
		}
	}

	int32_t unusedMousePositionX = 0;
	int32_t unusedMousePositionY = 0;

	ArrayInfo<ControllerType> types = { controllerTypes, 4 };
	Stream(_refreshState, unusedMousePositionX, unusedMousePositionY, nesModel, expansionDevice, consoleType, types, hasFourScore, useNes101Hvc101Behavior, zapperDetectionRadius, _lagCounter);

	if(!saving) {
		EmulationSettings::SetNesModel(nesModel);
		EmulationSettings::SetExpansionDevice(expansionDevice);
		EmulationSettings::SetConsoleType(consoleType);
		for(int i = 0; i < 4; i++) {
			EmulationSettings::SetControllerType(i, controllerTypes[i]);
		}

		EmulationSettings::SetZapperDetectionRadius(zapperDetectionRadius);
		EmulationSettings::SetFlagState(EmulationFlags::HasFourScore, hasFourScore);
		EmulationSettings::SetFlagState(EmulationFlags::UseNes101Hvc101Behavior, useNes101Hvc101Behavior);

		UpdateControlDevices();
	}

	if(GetStateVersion() >= 7) {
		for(uint8_t i = 0; i < _controlDevices.size(); i++) {
			SnapshotInfo device{ _controlDevices[i].get() };
			Stream(device);
		}
	}
}
