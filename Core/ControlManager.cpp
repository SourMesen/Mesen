#include "stdafx.h"
#include "ControlManager.h"

IControlDevice* ControlManager::ControlDevices[] = { nullptr, nullptr, nullptr, nullptr };
IControlDevice* ControlManager::OriginalControlDevices[] = { nullptr, nullptr, nullptr, nullptr };
IGameBroadcaster* ControlManager::GameBroadcaster = nullptr;
SimpleLock ControlManager::ControllerLock[4];

ControlManager::ControlManager()
{

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
	RefreshStateBuffer(2);
	RefreshStateBuffer(3);
}

void ControlManager::RefreshStateBuffer(uint8_t port)
{
	if(port >= 4) {
		throw exception("Invalid port");
	}

	ControlManager::ControllerLock[port].Acquire();
	IControlDevice* controlDevice = ControlManager::ControlDevices[port];

	uint8_t state;
	if(Movie::Playing()) {
		state = Movie::Instance->GetState(port);
	} else {
		if(controlDevice) {
			state = controlDevice->GetButtonState().ToByte();
		} else {
			state = 0x00;
		}
	}
	ControlManager::ControllerLock[port].Release();
	
	//Used when recording movies
	Movie::Instance->RecordState(port, state);
	if(ControlManager::GameBroadcaster) {
		ControlManager::GameBroadcaster->BroadcastInput(state, port);
	}

	_stateBuffer[port] = state;
}

uint8_t ControlManager::GetPortValue(uint8_t port)
{
	if(port >= 4) {
		throw exception("Invalid port");
	}

	if(_refreshState) {
		RefreshStateBuffer(port);
	}

	uint8_t returnValue = _stateBuffer[port] & 0x01;
	_stateBuffer[port] >>= 1;

	//"All subsequent reads will return D=1 on an authentic controller but may return D=0 on third party controllers."
	_stateBuffer[port] |= 0x80;

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
	StreamArray<uint8_t>(_stateBuffer, 4);
	Stream<bool>(_refreshState);
}