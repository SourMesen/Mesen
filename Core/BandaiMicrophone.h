#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"

class BandaiMicrophone : public BaseControlDevice
{
protected:
	enum Buttons { A, B, Microphone };

	string GetKeyNames() override
	{
		return "ABM";
	}

	void InternalSetStateFromInput() override
	{
		for(KeyMapping keyMapping : _keyMappings) {
			//TODO: Add proper key mappings
			SetPressedState(Buttons::A, keyMapping.A);
			SetPressedState(Buttons::B, keyMapping.B);
			SetPressedState(Buttons::Microphone, keyMapping.Microphone);
		}
	}

public:
	BandaiMicrophone(KeyMappingSet keyMappings) : BaseControlDevice(BaseControlDevice::MapperInputPort, keyMappings)
	{
	}

	uint8_t ReadRAM(uint16_t addr) override
	{
		if(addr >= 0x6000 && addr <= 0x7FFF) {
			return
				(IsPressed(Buttons::A) ? 0 : 0x01) |
				(IsPressed(Buttons::B) ? 0 : 0x02) |
				(IsPressed(Buttons::Microphone) ? 0x04 : 0);
		} else {
			return 0;
		}
	}

	void WriteRAM(uint16_t addr, uint8_t value) override
	{
	}
};