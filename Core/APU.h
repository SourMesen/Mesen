#pragma once

#include "stdafx.h"
#include "IMemoryHandler.h"
#include "IAudioDevice.h"
#include "Snapshotable.h"
#include "EmulationSettings.h"

class MemoryManager;
class SquareChannel;
class TriangleChannel;
class NoiseChannel;
class DeltaModulationChannel;
class ApuFrameCounter;
class SoundMixer;
enum class FrameType;
enum class NesModel;

class APU : public Snapshotable, public IMemoryHandler
{
	private:
		static APU* Instance;

		uint32_t _previousCycle;
		uint32_t _currentCycle;

		unique_ptr<SquareChannel> _squareChannel[2];
		unique_ptr<TriangleChannel> _triangleChannel;
		unique_ptr<NoiseChannel> _noiseChannel;
		unique_ptr<DeltaModulationChannel> _deltaModulationChannel;

		unique_ptr<ApuFrameCounter> _frameCounter;

		MemoryManager* _memoryManager;

		shared_ptr<SoundMixer> _mixer;

		NesModel _nesModel;

		double _cyclesNeeded;

	private:
		__forceinline bool NeedToRun(uint32_t currentCycle);
		void Run();
		void EndFrame();

		static void FrameCounterTick(FrameType type);

	protected:
		void StreamState(bool saving) override;

	public:
		APU(MemoryManager* memoryManager);
		~APU();

		void Reset(bool softReset);
		void SetNesModel(NesModel model, bool forceInit = false);

		uint8_t ReadRAM(uint16_t addr) override;
		void WriteRAM(uint16_t addr, uint8_t value) override;
		void GetMemoryRanges(MemoryRanges &ranges) override;

		void Exec();

		__forceinline static void ExecStatic()
		{
			if(EmulationSettings::GetOverclockRate(true) == 100) {
				Instance->Exec();
			} else {
				Instance->_cyclesNeeded += 1.0 / ((double)EmulationSettings::GetOverclockRate(true) / 100.0);
				while(Instance->_cyclesNeeded >= 1.0) {
					Instance->Exec();
					Instance->_cyclesNeeded--;
				}
			}
		}

		static void StaticRun();

		static void AddExpansionAudioDelta(AudioChannel channel, int16_t delta);
};