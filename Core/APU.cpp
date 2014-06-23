#pragma once

#include "stdafx.h"
#include "APU.h"
#include "CPU.h"

void output_samples(const blip_sample_t*, size_t count);

APU* APU::Instance = nullptr;
IAudioDevice* APU::AudioDevice = nullptr;

APU::APU()
{
	APU::Instance = this;

	blargg_err_t error = _buf.sample_rate(44100);
	if(error) {
		//report_error(error);
	}

	_buf.clock_rate(1789773);
	_apu.output(&_buf);

	_apu.dmc_reader(&APU::DMCRead);
	//_apu.irq_notifier(irq_changed);
}

int APU::DMCRead(void*, cpu_addr_t addr)
{
	return APU::Instance->ReadRAM(addr);
}

uint8_t APU::ReadRAM(uint16_t addr)
{
	switch(addr) {
		case 0x4015:
			return _apu.read_status(0);
			break;
	}

	return 0;
}

void APU::WriteRAM(uint16_t addr, uint8_t value)
{
	_apu.write_register(0, addr, value);
}

void APU::Exec(uint32_t executedCycles)
{
	_apu.end_frame(executedCycles);
	_buf.end_frame(executedCycles);
	
	// Read some samples out of Blip_Buffer if there are enough to
	// fill our output buffer
	const size_t out_size = 4096;
	blip_sample_t out_buf[out_size];

	if(_buf.samples_avail() >= out_size) {
		size_t count = _buf.read_samples(out_buf, out_size);
		APU::AudioDevice->PlayBuffer(out_buf, count * sizeof(blip_sample_t));
	}

}
