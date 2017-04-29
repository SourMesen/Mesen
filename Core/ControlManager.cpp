#include "stdafx.h"
#include "ControlManager.h"
#include "StandardController.h"
#include "Zapper.h"
#include "ArkanoidController.h"
#include "OekaKidsTablet.h"
#include "EmulationSettings.h"
#include "Console.h"
#include "GameServerConnection.h"
#include "MemoryManager.h"
#include "PPU.h"
#include "IKeyManager.h"

unique_ptr<IKeyManager> ControlManager::_keyManager = nullptr;
shared_ptr<BaseControlDevice> ControlManager::_controlDevices[2] = { nullptr, nullptr };
IGameBroadcaster* ControlManager::_gameBroadcaster = nullptr;
MousePosition ControlManager::_mousePosition = { -1, -1 };

ControlManager::ControlManager()
{

}

ControlManager::~ControlManager()
{
}

void ControlManager::RegisterKeyManager(IKeyManager* keyManager)
{
	_keyManager.reset(keyManager);
}

void ControlManager::RefreshKeyState()
{
	if(_keyManager != nullptr) {
		return _keyManager->RefreshState();
	}
}

bool ControlManager::IsKeyPressed(uint32_t keyCode)
{
	if(_keyManager != nullptr) {
		return _keyManager->IsKeyPressed(keyCode);
	}
	return false;
}

bool ControlManager::IsMouseButtonPressed(MouseButton button)
{
	if(_keyManager != nullptr) {
		return _keyManager->IsMouseButtonPressed(button);
	}
	return false;
}

uint32_t ControlManager::GetPressedKey()
{
	if(_keyManager != nullptr) {
		return _keyManager->GetPressedKey();
	}
	return 0;
}

string ControlManager::GetKeyName(uint32_t keyCode)
{
	if(_keyManager != nullptr) {
		return _keyManager->GetKeyName(keyCode);
	}
	return 0;
}

uint32_t ControlManager::GetKeyCode(string keyName)
{
	if(_keyManager != nullptr) {
		return _keyManager->GetKeyCode(keyName);
	}
	return 0;
}

void ControlManager::RegisterBroadcaster(IGameBroadcaster* gameBroadcaster)
{
	ControlManager::_gameBroadcaster = gameBroadcaster;
}

void ControlManager::UnregisterBroadcaster(IGameBroadcaster* gameBroadcaster)
{
	if(ControlManager::_gameBroadcaster == gameBroadcaster) {
		ControlManager::_gameBroadcaster = nullptr;
	}
}

void ControlManager::BroadcastInput(uint8_t port, uint8_t state)
{
	if(ControlManager::_gameBroadcaster) {
		//Used when acting as a game server
		ControlManager::_gameBroadcaster->BroadcastInput(state, port);
	}
}

shared_ptr<BaseControlDevice> ControlManager::GetControlDevice(uint8_t port)
{
	return ControlManager::_controlDevices[port];
}

void ControlManager::RegisterControlDevice(shared_ptr<BaseControlDevice> controlDevice, uint8_t port)
{
	ControlManager::_controlDevices[port] = controlDevice;
}

void ControlManager::UnregisterControlDevice(uint8_t port)
{
	ControlManager::_controlDevices[port].reset();
}

void ControlManager::RefreshAllPorts()
{
	if(_keyManager) {
		_keyManager->RefreshState();
	}

	for(int i = 0; i < 2; i++) {
		if(ControlManager::_controlDevices[i]) {
			ControlManager::_controlDevices[i]->RefreshStateBuffer();
		}
	}
}

shared_ptr<BaseControlDevice> ControlManager::GetZapper(uint8_t port)
{
	return shared_ptr<BaseControlDevice>(new Zapper(port));
}

void ControlManager::UpdateControlDevices()
{
	bool fourScore = EmulationSettings::CheckFlag(EmulationFlags::HasFourScore);
	ExpansionPortDevice expansionDevice = EmulationSettings::GetExpansionDevice();
	
	if(EmulationSettings::GetConsoleType() != ConsoleType::Famicom) {
		expansionDevice = ExpansionPortDevice::None;
	} else if(expansionDevice != ExpansionPortDevice::FourPlayerAdapter) {
		fourScore = false;
	}

	for(int i = 0; i < 2; i++) {
		shared_ptr<BaseControlDevice> device;
		bool forceController =
			i == 1 && EmulationSettings::GetControllerType(1) != ControllerType::StandardController && 
			(expansionDevice == ExpansionPortDevice::ArkanoidController || expansionDevice == ExpansionPortDevice::Zapper || expansionDevice == ExpansionPortDevice::OekaKidsTablet);

		bool controllerRequired = forceController || (EmulationSettings::GetConsoleType() == ConsoleType::Famicom && !EmulationSettings::CheckFlag(EmulationFlags::UseNes101Hvc101Behavior));

		if(fourScore || controllerRequired) {
			//Need to set standard controller in all slots if four score (to allow emulation to work correctly)
			device.reset(new StandardController(i, forceController));
		} else {
			switch(EmulationSettings::GetControllerType(i)) {
				case ControllerType::StandardController: device.reset(new StandardController(i)); break;
				case ControllerType::Zapper: device = GetZapper(i); break;
				case ControllerType::ArkanoidController: device.reset(new ArkanoidController(i)); break;
			}
		}

		if(device) {
			ControlManager::RegisterControlDevice(device, i);

			if(fourScore) {
				if(EmulationSettings::GetControllerType(i + 2) == ControllerType::StandardController) {
					std::dynamic_pointer_cast<StandardController>(device)->AddAdditionalController(shared_ptr<StandardController>(new StandardController(i + 2)));
				}
			} else if(i == 1 || expansionDevice == ExpansionPortDevice::ArkanoidController) {
				switch(expansionDevice) {
					case ExpansionPortDevice::Zapper: std::dynamic_pointer_cast<StandardController>(device)->AddAdditionalController(shared_ptr<Zapper>(new Zapper(2))); break;
					case ExpansionPortDevice::ArkanoidController: std::dynamic_pointer_cast<StandardController>(device)->AddAdditionalController(shared_ptr<ArkanoidController>(new ArkanoidController(2))); break;
					case ExpansionPortDevice::OekaKidsTablet: std::dynamic_pointer_cast<StandardController>(device)->AddAdditionalController(shared_ptr<OekaKidsTablet>(new OekaKidsTablet(2))); break;
					default: break;
				}
			}

		} else {
			//Remove current device if it's no longer in use
			ControlManager::UnregisterControlDevice(i);
		}
	}
}

uint8_t ControlManager::GetOpenBusMask(uint8_t port)
{
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

uint8_t ControlManager::GetPortValue(uint8_t port)
{
	if(_refreshState) {
		//Reload until strobe bit is set to off
		RefreshAllPorts();
	}
	shared_ptr<BaseControlDevice> device = GetControlDevice(port);

	//"In the NES and Famicom, the top three (or five) bits are not driven, and so retain the bits of the previous byte on the bus. 
	//Usually this is the most significant byte of the address of the controller port - 0x40.
	//Paperboy relies on this behavior and requires that reads from the controller ports return exactly $40 or $41 as appropriate."
	uint8_t value = MemoryManager::GetOpenBus(GetOpenBusMask(port));
	if(device) {
		value |= device->GetPortOutput();
	}

	return value;
}

uint8_t ControlManager::ReadRAM(uint16_t addr)
{
	//Used for lag counter
	//Any frame where the input is read does not count as lag
	_isLagging = false;

	switch(addr) {
		case 0x4016: return GetPortValue(0);
		case 0x4017: return GetPortValue(1);
	}

	return 0;
}

template<typename T>
shared_ptr<T> ControlManager::GetExpansionDevice()
{
	shared_ptr<StandardController> controller;
	controller = std::dynamic_pointer_cast<StandardController>(GetControlDevice(1));
	if(controller) {
		shared_ptr<T> expansionDevice;
		expansionDevice = std::dynamic_pointer_cast<T>(controller->GetAdditionalController());
		return expansionDevice;
	}
	return nullptr;
}

void ControlManager::WriteRAM(uint16_t addr, uint8_t value)
{
	//$4016 writes
	bool previousState = _refreshState;
	_refreshState = (value & 0x01) == 0x01;

	auto tablet = GetExpansionDevice<OekaKidsTablet>();
	if(tablet) {
		tablet->WriteRam(value);
	} else {
		if(previousState && !_refreshState) {
			//Refresh controller once strobe bit is disabled
			RefreshAllPorts();
		}
	}
}

void ControlManager::Reset(bool softReset)
{
	if(_keyManager != nullptr) {
		_keyManager->UpdateDevices();
	}
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
	if(saving) {
		nesModel = Console::GetNesModel();
		expansionDevice = EmulationSettings::GetExpansionDevice();
		consoleType = EmulationSettings::GetConsoleType();
		hasFourScore = EmulationSettings::CheckFlag(EmulationFlags::HasFourScore);
		useNes101Hvc101Behavior = EmulationSettings::CheckFlag(EmulationFlags::UseNes101Hvc101Behavior);
		for(int i = 0; i < 4; i++) {
			controllerTypes[i] = EmulationSettings::GetControllerType(i);
		}
	}

	ArrayInfo<ControllerType> types = { controllerTypes, 4 };
	Stream(_refreshState, _mousePosition.X, _mousePosition.Y, nesModel, expansionDevice, consoleType, types, hasFourScore, useNes101Hvc101Behavior);

	if(!saving) {
		EmulationSettings::SetNesModel(nesModel);
		EmulationSettings::SetExpansionDevice(expansionDevice);
		EmulationSettings::SetConsoleType(consoleType);
		for(int i = 0; i < 4; i++) {
			EmulationSettings::SetControllerType(i, controllerTypes[i]);
		}

		EmulationSettings::SetFlagState(EmulationFlags::HasFourScore, hasFourScore);
		EmulationSettings::SetFlagState(EmulationFlags::UseNes101Hvc101Behavior, useNes101Hvc101Behavior);

		UpdateControlDevices();
	}

	SnapshotInfo device0{ GetControlDevice(0).get() };
	SnapshotInfo device1{ GetControlDevice(1).get() };
	Stream(device0, device1);
}

void ControlManager::SetMousePosition(double x, double y)
{
	if(x < 0 || y < 0) {
		_mousePosition.X = -1;
		_mousePosition.Y = -1;
	} else {
		OverscanDimensions overscan = EmulationSettings::GetOverscanDimensions();
		_mousePosition.X = (int32_t)(x * (PPU::ScreenWidth - overscan.Left - overscan.Right) + overscan.Left);
		_mousePosition.Y = (int32_t)(y * (PPU::ScreenHeight - overscan.Top - overscan.Bottom) + overscan.Top);
	}
}

MousePosition ControlManager::GetMousePosition()
{
	return _mousePosition;
}

bool ControlManager::GetLagFlag()
{
	bool flag = _isLagging;
	_isLagging = true;
	return flag;
}