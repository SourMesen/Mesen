#pragma once
#include "stdafx.h"
#include "StandardController.h"

class PachinkoController : public StandardController
{
private:
	uint8_t _analogData = 0;

protected:
	enum PachinkoButtons { Press = 8, Release = 9 };

	void InternalSetStateFromInput() override
	{
		StandardController::InternalSetStateFromInput();

		for(KeyMapping keyMapping : _keyMappings) {
			SetPressedState(PachinkoButtons::Press, keyMapping.PachinkoButtons[0]);
			SetPressedState(PachinkoButtons::Release, keyMapping.PachinkoButtons[1]);
		}
	}

public:
	PachinkoController(KeyMappingSet keyMappings) : StandardController(BaseControlDevice::ExpDevicePort, keyMappings)
	{
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		uint8_t output = 0;
		if(addr == 0x4016) {
			output = (_stateBuffer & 0x01) << 1;
			_stateBuffer >>= 1;
			StrobeProcessRead();
		}
		return output;
	}

	void RefreshStateBuffer() override
	{
		if(_analogData < 63 && IsPressed(PachinkoController::PachinkoButtons::Press)) {
			_analogData++;
		} else if(_analogData > 0 && IsPressed(PachinkoController::PachinkoButtons::Release)) {
			_analogData--;
		}

		uint8_t analogData =
			((_analogData & 0x01) << 7) |
			((_analogData & 0x02) << 5) |
			((_analogData & 0x04) << 3) |
			((_analogData & 0x08) << 1) |
			((_analogData & 0x10) >> 1) |
			((_analogData & 0x20) >> 3) |
			((_analogData & 0x40) >> 5) |
			((_analogData & 0x80) >> 7);

		StandardController::RefreshStateBuffer();
		_stateBuffer = (_stateBuffer & 0xFF) | (~analogData << 8);
	}
};