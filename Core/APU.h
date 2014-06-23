#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"
#include "IAudioDevice.h"
#include "Nes_Apu/Nes_Apu.h"

class APU : public IMemoryHandler
{
	Nes_Apu _apu;
	Blip_Buffer _buf;

	private:
		static IAudioDevice* AudioDevice;
		static int DMCRead(void*, cpu_addr_t addr);
		static APU* Instance;

	public:
		APU();

		vector<std::array<uint16_t, 2>> GetRAMAddresses()
		{
			return{ { { 0x4000, 0x4013 } }, { { 0x4015, 0x4015 } }, {{ 0x4017, 0x4017 } } };
		}

		static void RegisterAudioDevice(IAudioDevice *audioDevice)
		{
			APU::AudioDevice = audioDevice;
		}

		uint8_t ReadRAM(uint16_t addr);
		void WriteRAM(uint16_t addr, uint8_t value);

		void Exec(uint32_t executedCycles);
};