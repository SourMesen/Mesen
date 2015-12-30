#include "stdafx.h"
#include "ControlManager.h"

unique_ptr<IKeyManager> ControlManager::_keyManager = nullptr;

IControlDevice* ControlManager::ControlDevices[] = { nullptr, nullptr, nullptr, nullptr };
IControlDevice* ControlManager::OriginalControlDevices[] = { nullptr, nullptr, nullptr, nullptr };
IGameBroadcaster* ControlManager::GameBroadcaster = nullptr;
SimpleLock ControlManager::ControllerLock[4];

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
	ControlManager::GameBroadcaster = gameBroadcaster;
}

void ControlManager::UnregisterBroadcaster(IGameBroadcaster* gameBroadcaster)
{
	if(ControlManager::GameBroadcaster == gameBroadcaster) {
		ControlManager::GameBroadcaster = nullptr;
	}
}

void ControlManager::BackupControlDevices()
{
	for(int i = 0; i < 4; i++) {
		OriginalControlDevices[i] = ControlDevices[i];
	}
}

void ControlManager::RestoreControlDevices()
{
	for(int i = 0; i < 4; i++) {
		ControlManager::ControllerLock[i].Acquire();
		ControlDevices[i] = OriginalControlDevices[i];
		ControlManager::ControllerLock[i].Release();
	}	
}

IControlDevice* ControlManager::GetControlDevice(uint8_t port)
{
	return ControlManager::ControlDevices[port];
}

void ControlManager::RegisterControlDevice(IControlDevice* controlDevice, uint8_t port)
{
	ControlManager::ControllerLock[port].Acquire();
	ControlManager::ControlDevices[port] = controlDevice;
	ControlManager::ControllerLock[port].Release();
}

void ControlManager::UnregisterControlDevice(IControlDevice* controlDevice)
{
	for(int i = 0; i < 4; i++) {
		if(ControlManager::ControlDevices[i] == controlDevice) {
			ControlManager::ControllerLock[i].Acquire();
			ControlManager::ControlDevices[i] = nullptr;
			ControlManager::ControllerLock[i].Release();
			break;
		}
	}

}

void ControlManager::RefreshAllPorts()
{
	RefreshStateBuffer(0);
	RefreshStateBuffer(1);
}

uint8_t ControlManager::GetControllerState(uint8_t controllerID)
{
	ControlManager::ControllerLock[controllerID].Acquire();
	IControlDevice* controlDevice = ControlManager::ControlDevices[controllerID];

	uint8_t state;
	if(Movie::Playing()) {
		state = Movie::Instance->GetState(controllerID);
	} else {
		if(controlDevice) {
			if(_keyManager) {
				_keyManager->RefreshState();
			}
			state = controlDevice->GetButtonState().ToByte();
		} else {
			state = 0x00;
		}
	}
	ControlManager::ControllerLock[controllerID].Release();

	//Used when recording movies
	Movie::Instance->RecordState(controllerID, state);

	if(ControlManager::GameBroadcaster) {
		//Used when acting as a game server
		ControlManager::GameBroadcaster->BroadcastInput(state, controllerID);
	}

	return state;
}

bool ControlManager::HasFourScoreAdapter()
{
	//When a movie is playing, always assume 4 controllers are plugged in (TODO: Change this so movies know how many controllers were plugged when recording)
	return ControlManager::ControlDevices[2] != nullptr || ControlManager::ControlDevices[3] != nullptr || Movie::Playing();
}

void ControlManager::RefreshStateBuffer(uint8_t port)
{
	if(port >= 2) {
		throw std::runtime_error("Invalid port");
	}

	//First 8 bits : Gamepad 1/2
	uint32_t state = GetControllerState(port);
	if(HasFourScoreAdapter()) {
		//Next 8 bits = Gamepad 3/4
		state |= GetControllerState(port + 2) << 8;

		//Last 8 bits = signature
		//Signature for port 0 = 0x10, reversed bit order => 0x08
		//Signature for port 1 = 0x20, reversed bit order => 0x04
		state |= (port == 0 ? 0x08 : 0x04) << 16;
	} else {
		//"All subsequent reads will return D=1 on an authentic controller but may return D=0 on third party controllers."
		state |= 0xFFFF00;
	}
	
	_stateBuffer[port] = state;
}

uint8_t ControlManager::GetPortValue(uint8_t port)
{
	if(port >= 2) {
		throw std::runtime_error("Invalid port");
	}

	if(_refreshState) {
		RefreshStateBuffer(port);
	}

	uint8_t returnValue = _stateBuffer[port] & 0x01;
	_stateBuffer[port] >>= 1;

	//"All subsequent reads will return D=1 on an authentic controller but may return D=0 on third party controllers."
	_stateBuffer[port] |= 0x800000;

	//"In the NES and Famicom, the top three (or five) bits are not driven, and so retain the bits of the previous byte on the bus. 
	//Usually this is the most significant byte of the address of the controller port - 0x40.
	//Paperboy relies on this behavior and requires that reads from the controller ports return exactly $40 or $41 as appropriate."
	return 0x40 | returnValue;
}

uint8_t ControlManager::ReadRAM(uint16_t addr)
{
	switch(addr) {
		case 0x4016:
			return GetPortValue(0);

		case 0x4017:
			return GetPortValue(1);
	}

	return 0;
}

void ControlManager::WriteRAM(uint16_t addr, uint8_t value)
{
	switch(addr) {
		case 0x4016:
			_refreshState = (value & 0x01) == 0x01;
			if(_refreshState) {
				RefreshAllPorts();
			}
			break;
	}

}

void ControlManager::StreamState(bool saving)
{
	StreamArray<uint32_t>(_stateBuffer, 2);
	Stream<bool>(_refreshState);
}