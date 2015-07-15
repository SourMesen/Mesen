#pragma once
#include "stdafx.h"
#include "IMemoryHandler.h"
#include "../BlipBuffer/Blip_Buffer.h"

template<int range>
class BaseApuChannel : public IMemoryHandler, public Snapshotable
{
private:
	unique_ptr<Blip_Synth<blip_good_quality, range>> _synth;
	uint16_t _lastOutput = 0;
	uint32_t _previousCycle = 0;
	Blip_Buffer *_buffer;

protected:
	uint16_t _timer = 0;
	uint16_t _period = 0;
	uint32_t _clockDivider = 2; //All channels except triangle clock overy other cpu clock

public:
	virtual void Clock() = 0;
	virtual bool GetStatus() = 0;

	BaseApuChannel(Blip_Buffer *buffer)
	{
		_buffer = buffer;
		_synth.reset(new Blip_Synth<blip_good_quality, range>());
		
		Reset();
	}

	virtual void Reset()
	{
		_timer = 0;
		_period = 0;
		_lastOutput = 0;
		_previousCycle = 0;
		_buffer->clear();
	}

	virtual void StreamState(bool saving)
	{
		Stream<uint16_t>(_lastOutput);
		Stream<uint32_t>(_previousCycle);
		Stream<uint16_t>(_timer);
		Stream<uint16_t>(_period);

		if(!saving) {
			_buffer->clear();
		}
	}

	void SetVolume(double volume)
	{
		_synth->volume(volume);
	}

	virtual void Run(uint32_t targetCycle)
	{
		while(_previousCycle < targetCycle) {
			if(_timer == 0) {
				Clock();
				_timer = _period;
				_previousCycle += _clockDivider;
			} else {
				uint32_t cyclesToRun = (targetCycle - _previousCycle) / _clockDivider;
				uint16_t skipCount = _timer > cyclesToRun ? cyclesToRun : _timer;
				_timer -= skipCount;
				_previousCycle += skipCount * _clockDivider;

				if(cyclesToRun == 0) {
					break;
				}
			}
		}
	}
	
	uint8_t ReadRAM(uint16_t addr)
	{
		return 0;
	}

	void AddOutput(uint16_t output)
	{
		if(output != _lastOutput) {
			_synth->offset_inline(_previousCycle, output - _lastOutput, _buffer);
		}
		_lastOutput = output;
	}

	void EndFrame()
	{
		_previousCycle = 0;
	}
};