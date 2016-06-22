#include "stdafx.h"
#include "ControlManager.h"
#include "StandardController.h"
#include "Zapper.h"
#include "ArkanoidController.h"
#include "EmulationSettings.h"
#include "Console.h"
#include "GameServerConnection.h"

unique_ptr<IKeyManager> ControlManager::_keyManager = nullptr;
shared_ptr<BaseControlDevice> ControlManager::_controlDevices[2] = { nullptr, nullptr };
IGameBroadcaster* ControlManager::_gameBroadcaster = nullptr;
MousePosition ControlManager::_mousePosition = { 0, 0 };

ControlManager::ControlManager()
{

}

void ControlManager::RegisterKeyManager(IKeyManager* keyManager)
{
	_keyManager.reset(keyManager);
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

void ControlManager::UpdateControlDevices()
{
	bool fourScore = EmulationSettings::CheckFlag(EmulationFlags::HasFourScore);
	ExpansionPortDevice expansionDevice = EmulationSettings::GetExpansionDevice();
	
	shared_ptr<BaseControlDevice> arkanoidController;
	if(EmulationSettings::GetConsoleType() != ConsoleType::Famicom) {
		expansionDevice = ExpansionPortDevice::None;
	} else if(expansionDevice != ExpansionPortDevice::FourPlayerAdapter) {
		fourScore = false;
		if(expansionDevice == ExpansionPortDevice::ArkanoidController) {
			arkanoidController.reset(new ArkanoidController(2));
		}
	}

	for(int i = 0; i < 2; i++) {
		shared_ptr<BaseControlDevice> device;
		if(fourScore || expansionDevice == ExpansionPortDevice::ArkanoidController || i == 1 && expansionDevice == ExpansionPortDevice::Zapper) {
			//Need to set standard controller in all slots if four score (to allow emulation to work correctly)
			device.reset(new StandardController(i));
		} else {
			switch(EmulationSettings::GetControllerType(i)) {
				case ControllerType::StandardController: device.reset(new StandardController(i)); break;
				case ControllerType::Zapper: device.reset(new Zapper(i)); break;
				case ControllerType::ArkanoidController: device.reset(new ArkanoidController(i)); break;
			}
		}

		if(device) {
			ControlManager::RegisterControlDevice(device, i);

			if(fourScore) {
				std::dynamic_pointer_cast<StandardController>(device)->AddAdditionalController(shared_ptr<StandardController>(new StandardController(i + 2)));
			} else if(i == 1 && expansionDevice == ExpansionPortDevice::Zapper) {
				std::dynamic_pointer_cast<StandardController>(device)->AddAdditionalController(shared_ptr<Zapper>(new Zapper(2)));
			} else if(expansionDevice == ExpansionPortDevice::ArkanoidController) {
				std::dynamic_pointer_cast<StandardController>(device)->AddAdditionalController(arkanoidController);
			}

		} else {
			//Remove current device if it's no longer in use
			ControlManager::UnregisterControlDevice(i);
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
	uint8_t value = 0x40;
	if(device) {
		value |= device->GetPortOutput();
	}

	return value;
}

uint8_t ControlManager::ReadRAM(uint16_t addr)
{
	switch(addr) {
		case 0x4016: return GetPortValue(0);
		case 0x4017: return GetPortValue(1);
	}

	return 0;
}

void ControlManager::WriteRAM(uint16_t addr, uint8_t value)
{
	//$4016 writes
	bool previousState = _refreshState;
	_refreshState = (value & 0x01) == 0x01;
	
	if(previousState && !_refreshState) {
		//Refresh controller once strobe bit is disabled
		RefreshAllPorts();
	}
}

void ControlManager::Reset(bool softReset)
{
}

void ControlManager::StreamState(bool saving)
{
	//Restore controllers that were being used at the time the snapshot was made
	//This is particularely important to ensure proper sync during NetPlay
	ControllerType controllerTypes[4];
	NesModel nesModel;
	ExpansionPortDevice expansionDevice;
	ConsoleType consoleType;
	bool hasFourScore;
	if(saving) {
		nesModel = Console::GetNesModel();
		expansionDevice = EmulationSettings::GetExpansionDevice();
		consoleType = EmulationSettings::GetConsoleType();
		hasFourScore = EmulationSettings::CheckFlag(EmulationFlags::HasFourScore);
		for(int i = 0; i < 4; i++) {
			controllerTypes[i] = EmulationSettings::GetControllerType(i);
		}
	}

	ArrayInfo<ControllerType> types = { controllerTypes, 4 };
	Stream(_refreshState, _mousePosition.X, _mousePosition.Y, nesModel, expansionDevice, consoleType, types, hasFourScore);

	if(!saving) {
		EmulationSettings::SetNesModel(nesModel);
		EmulationSettings::SetExpansionDevice(expansionDevice);
		EmulationSettings::SetConsoleType(consoleType);
		for(int i = 0; i < 4; i++) {
			EmulationSettings::SetControllerType(i, controllerTypes[i]);
		}

		if(hasFourScore) {
			EmulationSettings::SetFlags(EmulationFlags::HasFourScore);
		} else {
			EmulationSettings::ClearFlags(EmulationFlags::HasFourScore);
		}

		UpdateControlDevices();
	}

	SnapshotInfo device0{ GetControlDevice(0).get() };
	SnapshotInfo device1{ GetControlDevice(1).get() };
	Stream(device0, device1);
}

void ControlManager::SetMousePosition(double x, double y)
{
	OverscanDimensions overscan = EmulationSettings::GetOverscanDimensions();
	_mousePosition.X = (int32_t)(x * (PPU::ScreenWidth - overscan.Left - overscan.Right) + overscan.Left);
	_mousePosition.Y = (int32_t)(y * (PPU::ScreenHeight - overscan.Top - overscan.Bottom) + overscan.Top);
}

MousePosition ControlManager::GetMousePosition()
{
	return _mousePosition;
}