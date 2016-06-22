#include "stdafx.h"
#include "VsZapper.h"
#include "CPU.h"
#include "PPU.h"
#include "ControlManager.h"
#include "GameServerConnection.h"

void VsZapper::RefreshStateBuffer()
{
	_stateBuffer = GetControlState();
}

uint8_t VsZapper::RefreshState()
{
	Zapper::RefreshState();
	ZapperButtonState state = GetZapperState();
	return 0x10 | (state.LightNotDetected ? 0x00 : 0x40) | (state.TriggerPressed ? 0x80 : 0x00);
}

uint8_t VsZapper::GetPortOutput()
{
	uint8_t returnValue = _stateBuffer & 0x01;
	_stateBuffer >>= 1;
	return returnValue;
}

