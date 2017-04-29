#include "stdafx.h"
#include "OekaKidsTablet.h"
#include "ControlManager.h"
#include "GameServerConnection.h"
#include "IKeyManager.h"
#include "IKeyManager.h"

void OekaKidsTablet::StreamState(bool saving)
{
	BaseControlDevice::StreamState(saving);
	Stream(_xPosition, _yPosition, _touch, _click);
}

uint32_t OekaKidsTablet::GetNetPlayState()
{
	//Used by netplay
	uint32_t state = _xPosition | (_yPosition << 8);

	if(_touch) {
		state |= 0x40000000;
	}

	if(_click) {
		state |= 0x80000000;
	}

	return state;
}

uint8_t OekaKidsTablet::ProcessNetPlayState(uint32_t netplayState)
{
	_xPosition = netplayState & 0xFF;
	_yPosition = (netplayState >> 8) & 0xFF;

	_touch = ((netplayState >> 30) & 0x01) == 0x01;
	_click = ((netplayState >> 31) & 0x01) == 0x01;
	
	return RefreshState();
}

uint8_t OekaKidsTablet::RefreshState()
{
	if(_strobe) {
		if(_shift) {
			return (_stateBuffer & 0x40000) ? 0x00 : 0x08;
		} else {
			return 0x04;
		}
	} else {
		return 0x00;
	}
}

uint8_t OekaKidsTablet::GetPortOutput()
{
	return GetControlState();
}

void OekaKidsTablet::WriteRam(uint8_t value)
{
	_strobe = (value & 0x01) == 0x01;
	bool shift = ((value >> 1) & 0x01) == 0x01;

	if(_strobe) {
		if(!_shift && shift) {
			_stateBuffer <<= 1;
		}
		_shift = shift;
	} else {
		if(!GameServerConnection::GetNetPlayDevice(_port)) {
			MousePosition position = ControlManager::GetMousePosition();
			_xPosition = (int32_t)((double)std::max(0, position.X + 8) / 256.0 * 240);
			_yPosition = (int32_t)((double)std::max(0, position.Y - 14) / 240.0 * 256);

			if(!EmulationSettings::CheckFlag(EmulationFlags::InBackground) || EmulationSettings::CheckFlag(EmulationFlags::AllowBackgroundInput)) {
				_touch = position.Y >= 48 || ControlManager::IsMouseButtonPressed(MouseButton::LeftButton);
				_click = ControlManager::IsMouseButtonPressed(MouseButton::LeftButton);
			} else {
				_touch = false;
				_click = false;
			}
		}

		_stateBuffer = ((_xPosition & 0xFF) << 10) | ((_yPosition & 0xFF) << 2) | (_touch ? 0x02 : 0x00) | (_click ? 0x01 : 0x00);
	}
}
