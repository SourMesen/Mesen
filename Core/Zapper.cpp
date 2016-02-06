#include "stdafx.h"
#include "Zapper.h"
#include "CPU.h"
#include "PPU.h"

struct ZapperButtonState
{
	bool TriggerPressed = false;
	bool LightNotDetected = false;

	uint8_t ToByte()
	{
		return (LightNotDetected ? 0x08 : 0x00) | (TriggerPressed ? 0x10 : 0x00);
	}
};

void Zapper::StreamState(bool saving)
{
	BaseControlDevice::StreamState(saving);
	Stream<int32_t>(_xPosition);
	Stream<int32_t>(_yPosition);
	Stream<bool>(_pulled);
}

void Zapper::SetPosition(int32_t x, int32_t y)
{
	_xPosition = x;
	_yPosition = y;
}

void Zapper::SetTriggerState(bool pulled)
{
	_pulled = pulled;
}

uint32_t Zapper::GetZapperState()
{
	//Used by netplay
	uint32_t state;
	if(_yPosition == -1 || _xPosition == -1) {
		state = 0x80000000;
	} else {
		state = _xPosition | (_yPosition << 8);
	}

	if(_pulled) {
		state |= 0x40000000;
	}

	return state;
}

uint8_t Zapper::ProcessNetPlayState(uint32_t netplayState)
{
	if(netplayState >> 31) {
		_xPosition = -1;
		_yPosition = -1;
	} else {
		_xPosition = netplayState & 0xFF;
		_yPosition = (netplayState >> 8) & 0xFF;
	}
	_pulled = ((netplayState >> 30) & 0x01) == 0x01;
	
	return RefreshState();
}

uint8_t Zapper::RefreshState()
{
	ZapperButtonState state;
	state.TriggerPressed = _pulled;

	int32_t scanline = PPU::GetCurrentScanline();
	int32_t cycle = PPU::GetCurrentCycle();
	if(_xPosition == -1 || _yPosition == -1 || scanline > 240 || scanline < _yPosition || (scanline == _yPosition && cycle < _xPosition) || PPU::GetPixelBrightness(_xPosition, _yPosition) < 50) {
		//Light cannot be detected if the Y/X position is further ahead than the PPU, or if the PPU drew a dark color
		state.LightNotDetected = true;
	}

	return state.ToByte();
}

uint8_t Zapper::GetPortOutput()
{
	return GetControlState();
}

