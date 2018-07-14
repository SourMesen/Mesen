#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"

class ExcitingBoxingController : public BaseControlDevice
{
private:
	uint8_t _selectedSensors = 0;
	enum Buttons { LeftHook = 0, MoveRight, MoveLeft, RightHook, LeftJab, HitBody, RightJab, Straight };

protected:
	void StreamState(bool saving) override
	{
		BaseControlDevice::StreamState(saving);
		Stream(_selectedSensors);
	}

	string GetKeyNames() override
	{
		return "HRLhJBjS";
	}

	void InternalSetStateFromInput() override
	{
		for(KeyMapping keyMapping : _keyMappings) {
			for(int i = 0; i < 8; i++) {
				SetPressedState(i, keyMapping.ExcitingBoxingButtons[i]);
			}
		}
	}

public:
	ExcitingBoxingController(shared_ptr<Console> console, KeyMappingSet keyMappings) : BaseControlDevice(console, BaseControlDevice::ExpDevicePort, keyMappings)
	{
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		if(addr == 0x4017) {
			if(_selectedSensors == 0) {
				return
					(IsPressed(ExcitingBoxingController::Buttons::LeftHook) ? 0 : 0x02) |
					(IsPressed(ExcitingBoxingController::Buttons::MoveRight) ? 0 : 0x04) |
					(IsPressed(ExcitingBoxingController::Buttons::MoveLeft) ? 0 : 0x08) |
					(IsPressed(ExcitingBoxingController::Buttons::RightHook) ? 0 : 0x10);
			} else {
				return
					(IsPressed(ExcitingBoxingController::Buttons::LeftJab) ? 0 : 0x02) |
					(IsPressed(ExcitingBoxingController::Buttons::HitBody) ? 0 : 0x04) |
					(IsPressed(ExcitingBoxingController::Buttons::RightJab) ? 0 : 0x08) |
					(IsPressed(ExcitingBoxingController::Buttons::Straight) ? 0 : 0x10);
			}
		}
		return 0;
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
		_selectedSensors = (value & 0x02) >> 1;
	}
};