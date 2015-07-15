#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"
#include "IAudioDevice.h"
#include "Snapshotable.h"

class MemoryManager;
class SquareChannel;
class TriangleChannel;
class NoiseChannel;
class DeltaModulationChannel;
class ApuFrameCounter;
class Blip_Buffer;
enum class FrameType;

class APU : public Snapshotable, public IMemoryHandler
{
	private:
		static IAudioDevice* AudioDevice;
		static APU* Instance;

		uint32_t _previousCycle = 0;

		vector<unique_ptr<SquareChannel>> _squareChannel;
		unique_ptr<TriangleChannel> _triangleChannel;
		unique_ptr<NoiseChannel> _noiseChannel;
		unique_ptr<DeltaModulationChannel> _deltaModulationChannel;

		unique_ptr<ApuFrameCounter> _frameCounter;

		Blip_Buffer* _blipBuffer;
		int16_t* _outputBuffer;
		MemoryManager* _memoryManager;

	private:
		bool IrqPending(uint32_t currentCycle);
		void Run();

		static void FrameCounterTick(FrameType type);

	protected:
		void StreamState(bool saving);

	public:
		static const uint32_t SampleRate = 44100;
		static const uint32_t SamplesPerFrame = 44100 / 60;
		static const uint32_t BitsPerSample = 16;

	public:
		APU(MemoryManager* memoryManager);
		~APU();

		void Reset();
		
		static void RegisterAudioDevice(IAudioDevice *audioDevice)
		{
			APU::AudioDevice = audioDevice;
		}

		uint8_t ReadRAM(uint16_t addr);
		void WriteRAM(uint16_t addr, uint8_t value);
		void GetMemoryRanges(MemoryRanges &ranges);

		bool Exec(uint32_t currentCpuCycle);
		static void ExecStatic(uint32_t currentCpuCycle);

		static void StaticRun();
		static void StopAudio();
};