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
enum class NesModel;

class APU : public Snapshotable, public IMemoryHandler
{
	private:
		static IAudioDevice* AudioDevice;
		static APU* Instance;

		uint32_t _previousCycle;
		uint32_t _currentCycle;

		vector<unique_ptr<SquareChannel>> _squareChannel;
		unique_ptr<TriangleChannel> _triangleChannel;
		unique_ptr<NoiseChannel> _noiseChannel;
		unique_ptr<DeltaModulationChannel> _deltaModulationChannel;

		unique_ptr<ApuFrameCounter> _frameCounter;

		unique_ptr<Blip_Buffer> _blipBuffer;
		int16_t* _outputBuffer;
		MemoryManager* _memoryManager;

		NesModel _nesModel;

	private:
		bool NeedToRun(uint32_t currentCycle);
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

		void Reset(bool softReset);
		
		static void RegisterAudioDevice(IAudioDevice *audioDevice)
		{
			APU::AudioDevice = audioDevice;
		}

		void SetNesModel(NesModel model, bool forceInit = false);

		uint8_t ReadRAM(uint16_t addr);
		void WriteRAM(uint16_t addr, uint8_t value);
		void GetMemoryRanges(MemoryRanges &ranges);

		void Exec();
		static void ExecStatic();

		static void StaticRun();
		static void StopAudio(bool clearBuffer = false);
};