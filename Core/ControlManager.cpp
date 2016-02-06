#include "stdafx.h"
#include "ControlManager.h"
#include "StandardController.h"
#include "Zapper.h"
#include "EmulationSettings.h"
#include "Console.h"
#include "GameServerConnection.h"

unique_ptr<IKeyManager> ControlManager::_keyManager = nullptr;
shared_ptr<BaseControlDevice> ControlManager::_controlDevices[2];
IGameBroadcaster* ControlManager::_gameBroadcaster = nullptr;

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
	if(_refreshState) {
		if(_keyManager) {
			_keyManager->RefreshState();
		}

		for(int i = 0; i < 2; i++) {
			if(ControlManager::_controlDevices[i]) {
				ControlManager::_controlDevices[i]->RefreshStateBuffer();
			}
		}
	}
}

void ControlManager::UpdateControlDevices()
{
	bool fourScore = EmulationSettings::CheckFlag(EmulationFlags::HasFourScore);
	if(EmulationSettings::GetConsoleType() == ConsoleType::Famicom && EmulationSettings::GetExpansionDevice() != ExpansionPortDevice::FourPlayerAdapter) {
		fourScore = false;
	}

	for(int i = 0; i < 2; i++) {
		shared_ptr<BaseControlDevice> device;
		if(fourScore) {
			//Need to set standard controller in all slots if four score (to allow emulation to work correctly)
			device.reset(new StandardController(i));
		} else {
			switch(EmulationSettings::GetControllerType(i)) {
				case ControllerType::StandardController: device.reset(new StandardController(i)); break;
				case ControllerType::Zapper: device.reset(new Zapper(i)); break;
			}
		}

		if(device) {
			ControlManager::RegisterControlDevice(device, i);

			if(fourScore) {
				std::dynamic_pointer_cast<StandardController>(device)->AddAdditionalController(shared_ptr<StandardController>(new StandardController(i + 2)));
			} else if(i == 1 && EmulationSettings::GetConsoleType() == ConsoleType::Famicom && EmulationSettings::GetExpansionDevice() == ExpansionPortDevice::Zapper) {
				std::dynamic_pointer_cast<StandardController>(device)->AddAdditionalController(shared_ptr<Zapper>(new Zapper(2)));
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
		RefreshAllPorts();
	}

	//"In the NES and Famicom, the top three (or five) bits are not driven, and so retain the bits of the previous byte on the bus. 
	//Usually this is the most significant byte of the address of the controller port - 0x40.
	//Paperboy relies on this behavior and requires that reads from the controller ports return exactly $40 or $41 as appropriate."
	shared_ptr<BaseControlDevice> device = GetControlDevice(port);
	if(device) {
		return 0x40 | device->GetPortOutput();
	} else {
		return 0x40;
	}
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
	_refreshState = (value & 0x01) == 0x01;
	if(_refreshState) {
		RefreshAllPorts();
	}
}

void ControlManager::StreamState(bool saving)
{
	Stream<bool>(_refreshState);
	
	//Restore controllers that were being used at the time the snapshot was made
	//This is particularely important to ensure proper sync during NetPlay
	ControllerType controllerTypes[4];
	NesModel nesModel;
	ExpansionPortDevice expansionDevice;
	ConsoleType consoleType;
	if(saving) {
		nesModel = Console::GetNesModel();
		expansionDevice = EmulationSettings::GetExpansionDevice();
		consoleType = EmulationSettings::GetConsoleType();
		for(int i = 0; i < 4; i++) {
			controllerTypes[i] = EmulationSettings::GetControllerType(i);
		}
	}

	Stream<NesModel>(nesModel);
	Stream<ExpansionPortDevice>(expansionDevice);
	Stream<ConsoleType>(consoleType);
	for(int i = 0; i < 4; i++) {
		Stream<ControllerType>(controllerTypes[i]);
	}

	if(!saving) {
		EmulationSettings::SetNesModel(nesModel);
		EmulationSettings::SetExpansionDevice(expansionDevice);
		EmulationSettings::SetConsoleType(consoleType);
		for(int i = 0; i < 4; i++) {
			EmulationSettings::SetControllerType(i, controllerTypes[i]);
		}

		UpdateControlDevices();

		if(GetControlDevice(0)) {
			Stream(GetControlDevice(0));
		}
		if(GetControlDevice(1)) {
			Stream(GetControlDevice(1));
		}
	}
}

shared_ptr<Zapper> ControlManager::GetZapper(uint8_t port)
{
	shared_ptr<Zapper> zapper = std::dynamic_pointer_cast<Zapper>(GetControlDevice(port));
	if(zapper) {
		return zapper;
	} else {
		shared_ptr<StandardController> controller = std::dynamic_pointer_cast<StandardController>(GetControlDevice(port));
		if(controller) {
			return controller->GetZapper();
		}
	}

	return nullptr;
}

bool ControlManager::HasZapper()
{
	return GetZapper(0) != nullptr || GetZapper(1) != nullptr;
}

void ControlManager::ZapperSetPosition(uint8_t port, int32_t x, int32_t y)
{
	shared_ptr<Zapper> zapper = GetZapper(port);
	if(zapper) {
		zapper->SetPosition(x, y);
	}
}

void ControlManager::ZapperSetTriggerState(uint8_t port, bool pulled)
{
	shared_ptr<Zapper> zapper = GetZapper(port);
	if(zapper) {
		zapper->SetTriggerState(pulled);
	}
}