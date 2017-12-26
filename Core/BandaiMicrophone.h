#pragma once
#include "stdafx.h"
#include "BaseControlDevice.h"
#include "PPU.h"

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
		//Make sure the key bindings are properly updated (not ideal, but good enough)
		_keyMappings = EmulationSettings::GetControllerKeys(0).GetKeyMappingArray();

		for(KeyMapping keyMapping : _keyMappings) {
			SetPressedState(Buttons::A, keyMapping.BandaiMicrophoneButtons[0]);
			SetPressedState(Buttons::B, keyMapping.BandaiMicrophoneButtons[1]);
			if((PPU::GetFrameCount() % 2) == 0) {
				//Alternate between 1 and 0s (not sure if the game does anything with this data?)
				SetPressedState(Buttons::Microphone, keyMapping.BandaiMicrophoneButtons[2]);
			}
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