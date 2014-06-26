#pragma once

#include "stdafx.h"
#include "APU.h"
#include "CPU.h"
#include "Nes_Apu\apu_snapshot.h"

APU* APU::Instance = nullptr;
IAudioDevice* APU::AudioDevice = nullptr;

APU::APU(MemoryManager* memoryManager)
{
	APU::Instance = this;

	_memoryManager = memoryManager;

	_buf.sample_rate(APU::SampleRate);
	_buf.clock_rate(CPU::ClockRate);
	_apu.output(&_buf);

	_apu.dmc_reader(&APU::DMCRead);
	//_apu.irq_notifier(&APU::IRQChanged);

	_outputBuffer = new int16_t[APU::SamplesPerFrame];
}

APU::~APU()
{
	delete[] _outputBuffer;
}

void APU::Reset()
{
	_apu.reset();
}

int APU::DMCRead(void*, cpu_addr_t addr)
{
	return APU::Instance->_memoryManager->Read(addr);
}

uint8_t APU::ReadRAM(uint16_t addr)
{
	switch(addr) {
		case 0x4015:
			CPU::ClearIRQSource(IRQSource::FrameCounter);
			return _apu.read_status(_currentClock);
	}

	return 0;
}

void APU::WriteRAM(uint16_t addr, uint8_t value)
{
	_apu.write_register(_currentClock, addr, value);
}

bool APU::Exec(uint32_t executedCycles)
{
	_currentClock += executedCycles;

	if(_currentClock >= 29780) {
		_apu.end_frame(_currentClock);
		_buf.end_frame(_currentClock);

		_currentClock -= 29780;

		if(APU::Instance->_apu.earliest_irq() == Nes_Apu::irq_waiting) {
			CPU::SetIRQSource(IRQSource::FrameCounter);
		}

		// Read some samples out of Blip_Buffer if there are enough to fill our output buffer
		uint32_t availableSampleCount = _buf.samples_avail();
		if(availableSampleCount >= APU::SamplesPerFrame) {
			size_t sampleCount = _buf.read_samples(_outputBuffer, APU::SamplesPerFrame);
			APU::AudioDevice->PlayBuffer(_outputBuffer, sampleCount * BitsPerSample / 8);

			return true;
		}
	}
	return false;
}

void APU::StreamState(bool saving)
{
	apu_snapshot_t snapshot;
	if(saving) {
		_apu.save_snapshot(&snapshot);
	} 
	
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
		_apu.load_snapshot(snapshot);
	}
}