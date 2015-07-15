#include "stdafx.h"
#include "../BlipBuffer/Blip_Buffer.h"
#include "APU.h"
#include "CPU.h"
#include "SquareChannel.h"
#include "TriangleChannel.h"
#include "NoiseChannel.h"
#include "DeltaModulationChannel.h"
#include "ApuFrameCounter.h"

APU* APU::Instance = nullptr;
IAudioDevice* APU::AudioDevice = nullptr;

APU::APU(MemoryManager* memoryManager)
{
	APU::Instance = this;

	_memoryManager = memoryManager;
	_blipBuffer = new Blip_Buffer();
	_blipBuffer->sample_rate(APU::SampleRate);
	_blipBuffer->clock_rate(CPU::ClockRate);

	_outputBuffer = new int16_t[APU::SamplesPerFrame];

	_squareChannel.push_back(unique_ptr<SquareChannel>(new SquareChannel(true)));
	_squareChannel.push_back(unique_ptr<SquareChannel>(new SquareChannel(false)));
	_triangleChannel.reset(new TriangleChannel());
	_noiseChannel.reset(new NoiseChannel());
	_deltaModulationChannel.reset(new DeltaModulationChannel(_memoryManager));
	_frameCounter.reset(new ApuFrameCounter(&APU::FrameCounterTick));

	_squareChannel[0]->SetBuffer(_blipBuffer);
	_squareChannel[1]->SetBuffer(_blipBuffer);
	_triangleChannel->SetBuffer(_blipBuffer);
	_noiseChannel->SetBuffer(_blipBuffer);
	_deltaModulationChannel->SetBuffer(_blipBuffer);

	_memoryManager->RegisterIODevice(_squareChannel[0].get());
	_memoryManager->RegisterIODevice(_squareChannel[1].get());
	_memoryManager->RegisterIODevice(_frameCounter.get());
	_memoryManager->RegisterIODevice(_triangleChannel.get());
	_memoryManager->RegisterIODevice(_noiseChannel.get());
	_memoryManager->RegisterIODevice(_deltaModulationChannel.get());
}

APU::~APU()
{
	delete[] _outputBuffer;
}

void APU::Reset()
{
	//_apu.reset();
}

void APU::FrameCounterTick(FrameType type)
{
	//Quarter & half frame clock envelope & linear counter
	Instance->_squareChannel[0]->TickEnvelope();
	Instance->_squareChannel[1]->TickEnvelope();
	Instance->_triangleChannel->TickLinearCounter();
	Instance->_noiseChannel->TickEnvelope();

	if(type == FrameType::HalfFrame) {
		//Half frames clock length counter & sweep
		Instance->_squareChannel[0]->TickLengthCounter();
		Instance->_squareChannel[1]->TickLengthCounter();
		Instance->_triangleChannel->TickLengthCounter();
		Instance->_noiseChannel->TickLengthCounter();

		Instance->_squareChannel[0]->TickSweep();
		Instance->_squareChannel[1]->TickSweep();
	}
}

uint8_t APU::ReadRAM(uint16_t addr)
{
	//$4015 read
	Run();

	uint8_t status = 0;
	status |= _squareChannel[0]->GetStatus() ? 0x01 : 0x00;
	status |= _squareChannel[1]->GetStatus() ? 0x02 : 0x00;
	status |= _triangleChannel->GetStatus() ? 0x04 : 0x00;
	status |= _noiseChannel->GetStatus() ? 0x08 : 0x00;
	status |= _deltaModulationChannel->GetStatus() ? 0x10 : 0x00;
	status |= CPU::HasIRQSource(IRQSource::FrameCounter) ? 0x40 : 0x00;
	status |= CPU::HasIRQSource(IRQSource::DMC) ? 0x80 : 0x00;

	//Reading $4015 clears the Frame Counter interrupt flag.
	CPU::ClearIRQSource(IRQSource::FrameCounter);

	return status;
}

void APU::WriteRAM(uint16_t addr, uint8_t value)
{
	//$4015 write
	Run();
	_squareChannel[0]->SetEnabled((value & 0x01) == 0x01);
	_squareChannel[1]->SetEnabled((value & 0x02) == 0x02);
	_triangleChannel->SetEnabled((value & 0x04) == 0x04);
	_noiseChannel->SetEnabled((value & 0x08) == 0x08);
	_deltaModulationChannel->SetEnabled((value & 0x10) == 0x10);

	//Writing to $4015 clears the DMC interrupt flag.
	CPU::ClearIRQSource(IRQSource::DMC);
}

void APU::GetMemoryRanges(MemoryRanges &ranges)
{
	ranges.AddHandler(MemoryType::RAM, MemoryOperation::Read, 0x4015);
	ranges.AddHandler(MemoryType::RAM, MemoryOperation::Write, 0x4015);
}

void APU::Run()
{
	//Update framecounter and all channels
	//This is called:
	//-At the end of a frame
	//-Before APU registers are read/written to
	//-When a DMC or FrameCounter interrupt needs to be fired
	uint32_t targetCycle = CPU::GetCycleCount();
	uint32_t currentCycle = _previousCycle;
	uint32_t cyclesToRun = targetCycle - _previousCycle;

	while(currentCycle < targetCycle) {
		currentCycle += _frameCounter->Run(cyclesToRun);

		_squareChannel[0]->Run(currentCycle);
		_squareChannel[1]->Run(currentCycle);
		_noiseChannel->Run(currentCycle);
		_triangleChannel->Run(currentCycle);
		_deltaModulationChannel->Run(currentCycle);
	}

	_previousCycle = targetCycle;
}

void APU::StaticRun()
{
	Instance->Run();
}

bool APU::IrqPending(uint32_t currentCycle)
{
	uint32_t cyclesToRun = currentCycle - _previousCycle;
	if(_frameCounter->IrqPending(cyclesToRun)) {
		return true;
	} else if(_deltaModulationChannel->IrqPending(cyclesToRun)) {
		return true;
	}
	return false;
}

void APU::ExecStatic(uint32_t currentCpuCycle)
{
	Instance->Exec(currentCpuCycle);
}

bool APU::Exec(uint32_t currentCpuCycle)
{
	if(IrqPending(currentCpuCycle)) {
		Run();
	}

	if(currentCpuCycle >= 29780) {
		Run();

		_previousCycle = 0;

		_squareChannel[0]->EndFrame();
		_squareChannel[1]->EndFrame();
		_triangleChannel->EndFrame();
		_noiseChannel->EndFrame();
		_deltaModulationChannel->EndFrame();

		_blipBuffer->end_frame(currentCpuCycle);

		// Read some samples out of Blip_Buffer if there are enough to fill our output buffer
		uint32_t availableSampleCount = _blipBuffer->samples_avail();
		if(availableSampleCount >= APU::SamplesPerFrame) {
			size_t sampleCount = _blipBuffer->read_samples(_outputBuffer, APU::SamplesPerFrame);
			if(APU::AudioDevice) {
				APU::AudioDevice->PlayBuffer(_outputBuffer, (uint32_t)(sampleCount * BitsPerSample / 8));
			}
		}
		return true;
	}
	return false;
}

void APU::StopAudio()
{
	if(APU::AudioDevice) {
		APU::AudioDevice->Pause();
	}
}

void APU::StreamState(bool saving)
{
	/*apu_snapshot_t snapshot;
	if(saving) {
		//_apu.save_snapshot(&snapshot);
	} 

	Stream<uint32_t>(_currentClock);
	
	StreamArray<uint8_t>(snapshot.w40xx, 0x14);
	Stream<uint8_t>(snapshot.w4015);
	Stream<uint8_t>(snapshot.w4017);
	Stream<uint16_t>(snapshot.delay);
	Stream<uint8_t>(snapshot.step);
	Stream<uint8_t>(snapshot.irq_flag);

	Stream<uint16_t>(snapshot.square1.delay);
	StreamArray<uint8_t>(snapshot.square1.env, 3);
	Stream<uint8_t>(snapshot.square1.length);
	Stream<uint8_t>(snapshot.square1.phase);
	Stream<uint8_t>(snapshot.square1.swp_delay);
	Stream<uint8_t>(snapshot.square1.swp_reset);
	StreamArray<uint8_t>(snapshot.square1.unused, 1);

	Stream<uint16_t>(snapshot.square2.delay);
	StreamArray<uint8_t>(snapshot.square2.env, 3);
	Stream<uint8_t>(snapshot.square2.length);
	Stream<uint8_t>(snapshot.square2.phase);
	Stream<uint8_t>(snapshot.square2.swp_delay);
	Stream<uint8_t>(snapshot.square2.swp_reset);
	StreamArray<uint8_t>(snapshot.square2.unused, 1);

	Stream<uint16_t>(snapshot.triangle.delay);
	Stream<uint8_t>(snapshot.triangle.length);
	Stream<uint8_t>(snapshot.triangle.phase);
	Stream<uint8_t>(snapshot.triangle.linear_counter);
	Stream<uint8_t>(snapshot.triangle.linear_mode);

	Stream<uint16_t>(snapshot.noise.delay);
	StreamArray<uint8_t>(snapshot.noise.env, 3);
	Stream<uint8_t>(snapshot.noise.length);
	Stream<uint16_t>(snapshot.noise.shift_reg);

	Stream<uint16_t>(snapshot.dmc.delay);
	Stream<uint16_t>(snapshot.dmc.remain);
	Stream<uint16_t>(snapshot.dmc.addr);
	Stream<uint8_t>(snapshot.dmc.buf);
	Stream<uint8_t>(snapshot.dmc.bits_remain);
	Stream<uint8_t>(snapshot.dmc.bits);
	Stream<uint8_t>(snapshot.dmc.buf_empty);
	Stream<uint8_t>(snapshot.dmc.silence);
	Stream<uint8_t>(snapshot.dmc.irq_flag);

	if(!saving) {
		//_apu.load_snapshot(snapshot);
	}*/
}