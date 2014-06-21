#include "stdafx.h"
#include "ControlManager.h"

IControlDevice* ControlManager::ControlDevices[] = { nullptr, nullptr, nullptr, nullptr };

ControlManager::ControlManager()
{

}

void ControlManager::RegisterControlDevice(IControlDevice* controlDevice, uint8_t port)
{
	ControlManager::ControlDevices[port] = controlDevice;
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

	IControlDevice* controlDevice = ControlManager::ControlDevices[port];

	if(controlDevice) {
		ButtonState buttonState = controlDevice->GetButtonState();

		//"Button status for each controller is returned as an 8-bit report in the following order: A, B, Select, Start, Up, Down, Left, Right."
		uint8_t state = (uint8_t)buttonState.A | ((uint8_t)buttonState.B << 1) | ((uint8_t)buttonState.Select << 2) | ((uint8_t)buttonState.Start << 3) |
							((uint8_t)buttonState.Up << 4) | ((uint8_t)buttonState.Down << 5) | ((uint8_t)buttonState.Left << 6) | ((uint8_t)buttonState.Right << 7);

		_stateBuffer[port] = state;
	} else {
		_stateBuffer[port] = 0x00;
	}
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