#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"
#include "IAudioDevice.h"
#include "Nes_Apu/Nes_Apu.h"

class APU : public IMemoryHandler
{
	private:
		Nes_Apu _apu;
		Blip_Buffer _buf;
		int16_t* _outputBuffer;

	private:
		static IAudioDevice* AudioDevice;
		static int DMCRead(void*, cpu_addr_t addr);
		static APU* Instance;

	public:
		static const uint32_t SampleRate = 44100;
		static const uint32_t SamplesPerFrame = 44100 / 60;
		static const uint32_t BitsPerSample = 16;

	public:
		APU();
		~APU();

		void Reset();
		
		void GetMemoryRanges(MemoryRanges &ranges)
		{
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Read, 0x4015);
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Write, 0x4000, 0x4013);
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Write, 0x4015);
			ranges.AddHandler(MemoryType::RAM, MemoryOperation::Write, 0x4017);
		}

		static void RegisterAudioDevice(IAudioDevice *audioDevice)
		{
			APU::AudioDevice = audioDevice;
		}

		uint8_t ReadRAM(uint16_t addr);
		void WriteRAM(uint16_t addr, uint8_t value);

		bool Exec(uint32_t executedCycles);
};